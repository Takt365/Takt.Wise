// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Extensions
// 文件名称：TaktObservabilityCollectionExtensions.cs
// 创建时间：2026-08-12
// 创建人：Takt365(Cursor AI)
// 功能描述：HealthChecks + Prometheus Metrics + OpenTelemetry Tracing 注册与管道映射
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Prometheus;
using Takt.Infrastructure.Health;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Extensions;

/// <summary>
/// 可观测性（Health / Metrics / Tracing）服务与管道扩展
/// </summary>
public static class TaktObservabilityCollectionExtensions
{
    /// <summary>
    /// 就绪探针标签
    /// </summary>
    public const string ReadyTag = "ready";

    /// <summary>
    /// 注册 HealthChecks、Prometheus 默认进程指标依赖、OpenTelemetry Tracing
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">配置</param>
    /// <param name="environmentName">主机环境名</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddTaktObservability(
        this IServiceCollection services,
        IConfiguration configuration,
        string environmentName)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.Configure<TaktObservabilityOptions>(opts =>
        {
            configuration.GetSection(TaktObservabilityOptions.SectionName).Bind(opts);
            opts.Validate();
        });
        var options = configuration.GetSection(TaktObservabilityOptions.SectionName)
            .Get<TaktObservabilityOptions>() ?? new TaktObservabilityOptions();
        options.Validate();

        if (options.HealthChecksEnabled)
        {
            services.AddHttpContextAccessor();
            services.AddHealthChecks()
                .AddCheck<TaktTenantDatabaseHealthCheck>(
                    "tenant_database",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: [ReadyTag]);
        }

        if (options.TracingEnabled)
        {
            services.AddOpenTelemetry()
                .ConfigureResource(resource => resource.AddService(
                    serviceName: options.ServiceName,
                    serviceVersion: typeof(TaktObservabilityCollectionExtensions).Assembly.GetName().Version?.ToString() ?? "0.0.0"))
                .WithTracing(tracing =>
                {
                    tracing
                        .AddAspNetCoreInstrumentation(instrumentation =>
                        {
                            instrumentation.Filter = httpContext =>
                                !IsObservabilityProbePath(httpContext.Request.Path);
                            instrumentation.RecordException = true;
                        })
                        .AddHttpClientInstrumentation();

                    if (!string.IsNullOrWhiteSpace(options.OtlpEndpoint))
                    {
                        tracing.AddOtlpExporter(otlp =>
                        {
                            otlp.Endpoint = new Uri(options.OtlpEndpoint);
                        });
                    }

                    var useConsole = options.ConsoleExporterEnabled
                        || (string.IsNullOrWhiteSpace(options.OtlpEndpoint)
                            && string.Equals(environmentName, "Development", StringComparison.OrdinalIgnoreCase));
                    if (useConsole)
                    {
                        tracing.AddConsoleExporter();
                    }
                });
        }

        TaktLogger.Information(
            "可观测性已注册 - HealthChecks={Health}, Prometheus={Prometheus}, Tracing={Tracing}, MetricsPath={MetricsPath}",
            options.HealthChecksEnabled,
            options.PrometheusEnabled,
            options.TracingEnabled,
            options.MetricsPath);

        return services;
    }

    /// <summary>
    /// 启用 Prometheus HTTP 指标中间件（须在路由映射前）
    /// </summary>
    /// <param name="app">Web 应用</param>
    /// <returns>Web 应用</returns>
    public static IApplicationBuilder UseTaktPrometheusHttpMetrics(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);
        var options = app.ApplicationServices.GetRequiredService<IOptions<TaktObservabilityOptions>>().Value;
        if (!options.PrometheusEnabled)
        {
            return app;
        }

        return app.UseHttpMetrics();
    }

    /// <summary>
    /// 映射 /health、/health/ready 与 /metrics（匿名）
    /// </summary>
    /// <param name="app">Web 应用</param>
    /// <returns>Web 应用</returns>
    public static WebApplication MapTaktObservabilityEndpoints(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);
        var options = app.Services.GetRequiredService<IOptions<TaktObservabilityOptions>>().Value;
        options.Validate();

        if (options.HealthChecksEnabled)
        {
            app.MapHealthChecks("/health", new HealthCheckOptions
                {
                    Predicate = _ => false,
                    ResponseWriter = WriteTaktHealthResponseAsync,
                })
                .AllowAnonymous()
                .WithName("HealthCheck")
                .WithOpenApi();

            app.MapHealthChecks("/health/ready", new HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains(ReadyTag),
                    ResponseWriter = WriteTaktHealthResponseAsync,
                })
                .AllowAnonymous()
                .WithName("HealthCheckReady")
                .WithOpenApi();
        }

        if (options.PrometheusEnabled)
        {
            app.MapMetrics(options.MetricsPath)
                .AllowAnonymous();
        }

        return app;
    }

    /// <summary>
    /// 是否为可观测性探针路径（跳过 XSS/Tracing 采样等）
    /// </summary>
    /// <param name="path">请求路径</param>
    /// <returns>探针路径时为 true</returns>
    public static bool IsObservabilityProbePath(PathString path)
    {
        var value = path.Value ?? string.Empty;
        return value.Equals("/health", StringComparison.OrdinalIgnoreCase)
            || value.Equals("/health/ready", StringComparison.OrdinalIgnoreCase)
            || value.StartsWith("/health/", StringComparison.OrdinalIgnoreCase)
            || value.Equals("/metrics", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 写出与历史手写探针兼容的 JSON（含 Status / Timestamp）
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <param name="report">健康报告</param>
    /// <returns>异步任务</returns>
    private static async Task WriteTaktHealthResponseAsync(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json; charset=utf-8";
        var status = report.Status switch
        {
            HealthStatus.Healthy => "Healthy",
            HealthStatus.Degraded => "Degraded",
            _ => "Unhealthy",
        };

        var payload = new Dictionary<string, object?>
        {
            ["Status"] = status,
            ["Timestamp"] = DateTime.UtcNow,
            ["Environment"] = context.RequestServices
                .GetService<Microsoft.AspNetCore.Hosting.IWebHostEnvironment>()?.EnvironmentName,
            ["TotalDurationMs"] = report.TotalDuration.TotalMilliseconds,
        };

        if (report.Entries.Count > 0)
        {
            var entries = new Dictionary<string, object>();
            foreach (var (name, entry) in report.Entries)
            {
                entries[name] = new
                {
                    Status = entry.Status.ToString(),
                    Description = entry.Description,
                    DurationMs = entry.Duration.TotalMilliseconds,
                    Data = entry.Data,
                };
            }

            payload["Entries"] = entries;
        }

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8);
    }
}

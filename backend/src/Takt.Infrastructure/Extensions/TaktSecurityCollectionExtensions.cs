// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Extensions
// 文件名称：TaktSecurityCollectionExtensions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：安全相关服务注册与管道扩展（限流、CSRF、XSS、数据保护）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Takt.Infrastructure.Middleware;
using Takt.Shared.Constants;
using Takt.Shared.Enums;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Extensions;

/// <summary>
/// 安全相关服务注册扩展方法
/// </summary>
public static class TaktSecurityCollectionExtensions
{
    /// <summary>
    /// 添加安全相关服务（数据保护、限流、防伪令牌、配置绑定）
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">配置</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddTaktSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        var securityOptions = configuration.RequireOptions<TaktSecurityOptions>(TaktSecurityOptions.SectionName);
        securityOptions.Validate();

        services.Configure<TaktSecurityOptions>(configuration.GetSection(TaktSecurityOptions.SectionName));

        services.AddDataProtection();

        services.AddAntiforgery(options =>
        {
            options.HeaderName = "X-XSRF-TOKEN";
            options.Cookie.Name = "Takt-XSRF-TOKEN";
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        });

        services.AddRateLimiter(rateLimiterOptions =>
        {
            rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            rateLimiterOptions.OnRejected = async (context, cancellationToken) =>
            {
                context.HttpContext.Response.ContentType = "application/json; charset=utf-8";
                var result = TaktApiResult<object>.Fail(
                    "请求过于频繁，请稍后再试",
                    TaktResultCode.TooManyRequests);
                await context.HttpContext.Response.WriteAsJsonAsync(result, cancellationToken);
            };

            rateLimiterOptions.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            {
                var securityOptions = httpContext.RequestServices
                    .GetRequiredService<IOptions<TaktSecurityOptions>>()
                    .Value;
                var rateLimit = securityOptions.RateLimit;

                if (!rateLimit.Enabled)
                {
                    return RateLimitPartition.GetNoLimiter("disabled");
                }

                var partitionKey = httpContext.Connection.RemoteIpAddress?.ToString()
                    ?? httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                    ?? "unknown";

                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey,
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = Math.Max(1, rateLimit.MaxRequests),
                        Window = TimeSpan.FromSeconds(Math.Max(1, rateLimit.TimeWindowSeconds)),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0,
                    });
            });

            rateLimiterOptions.AddPolicy(TaktRateLimitPolicyNames.Login, httpContext =>
            {
                var securityOptions = httpContext.RequestServices
                    .GetRequiredService<IOptions<TaktSecurityOptions>>()
                    .Value;
                var loginRateLimit = securityOptions.LoginRateLimit;

                if (!loginRateLimit.Enabled)
                {
                    return RateLimitPartition.GetNoLimiter("login-disabled");
                }

                var partitionKey = httpContext.Connection.RemoteIpAddress?.ToString()
                    ?? httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                    ?? "unknown";

                return RateLimitPartition.GetFixedWindowLimiter(
                    $"login:{partitionKey}",
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = Math.Max(1, loginRateLimit.MaxRequests),
                        Window = TimeSpan.FromSeconds(Math.Max(1, loginRateLimit.TimeWindowSeconds)),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0,
                    });
            });
        });

        return services;
    }

    /// <summary>
    /// 启用安全中间件管道（限流、XSS、CSRF）并映射 CSRF 令牌端点
    /// </summary>
    /// <param name="app">Web 应用程序</param>
    /// <returns>Web 应用程序</returns>
    public static WebApplication UseTaktSecurity(this WebApplication app)
    {
        var securityOptions = app.Services.GetRequiredService<IOptions<TaktSecurityOptions>>().Value;

        app.UseMiddleware<TaktFullWidthMiddleware>();
        if (securityOptions.RateLimit.Enabled)
        {
            app.UseRateLimiter();
        }

        if (securityOptions.XssProtection.Enabled)
        {
            app.UseMiddleware<TaktXssProtectionMiddleware>();
        }

        if (securityOptions.CsrfProtection.Enabled)
        {
            app.MapGet("/api/security/csrf-token", (
                    HttpContext httpContext,
                    IAntiforgery antiforgery) =>
                {
                    var tokens = antiforgery.GetAndStoreTokens(httpContext);
                    return Results.Ok(new
                    {
                        token = tokens.RequestToken,
                        headerName = "X-XSRF-TOKEN",
                        cookieName = "Takt-XSRF-TOKEN",
                    });
                })
                .AllowAnonymous()
                .WithName("GetCsrfToken")
                .WithOpenApi();

            app.UseMiddleware<TaktCsrfProtectionMiddleware>();
        }

        return app;
    }
}

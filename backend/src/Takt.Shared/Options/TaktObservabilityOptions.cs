// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Options
// 文件名称：TaktObservabilityOptions.cs
// 创建时间：2026-08-12
// 创建人：Takt365(Cursor AI)
// 功能描述：可观测性开关（HealthChecks / Prometheus Metrics / OpenTelemetry Tracing）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Options;

/// <summary>
/// 可观测性配置（appsettings <c>Observability</c> 节）
/// </summary>
public class TaktObservabilityOptions
{
    /// <summary>
    /// 配置节名称
    /// </summary>
    public const string SectionName = "Observability";

    /// <summary>
    /// 是否启用 ASP.NET Core HealthChecks（/health、/health/ready）
    /// </summary>
    public bool HealthChecksEnabled { get; set; } = true;

    /// <summary>
    /// 是否启用 Prometheus Metrics（HTTP 指标 + 进程 CPU/内存；暴露 /metrics）
    /// </summary>
    public bool PrometheusEnabled { get; set; } = true;

    /// <summary>
    /// Prometheus 抓取路径（默认 /metrics）
    /// </summary>
    public string MetricsPath { get; set; } = "/metrics";

    /// <summary>
    /// 是否启用 OpenTelemetry Tracing（ASP.NET Core + HttpClient 调用链）
    /// </summary>
    public bool TracingEnabled { get; set; } = true;

    /// <summary>
    /// OTLP 导出端点（为空则开发环境可改用控制台导出；生产建议指向 Collector）
    /// </summary>
    public string? OtlpEndpoint { get; set; }

    /// <summary>
    /// 是否向控制台导出 Trace（仅建议 Development）
    /// </summary>
    public bool ConsoleExporterEnabled { get; set; }

    /// <summary>
    /// 服务名（写入 OTel Resource）
    /// </summary>
    public string ServiceName { get; set; } = "Takt.WebApi";

    /// <summary>
    /// 校验配置合法性
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(MetricsPath))
        {
            MetricsPath = "/metrics";
        }

        if (!MetricsPath.StartsWith('/'))
        {
            MetricsPath = "/" + MetricsPath.Trim();
        }

        if (string.IsNullOrWhiteSpace(ServiceName))
        {
            ServiceName = "Takt.WebApi";
        }
    }
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Models.Logging
// 文件名称：TaktLogModels.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：统一日志模型（采集、格式化、上报）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Enums;

namespace Takt.Shared.Models.Logging;

/// <summary>
/// 日志业务上下文
/// </summary>
public class TaktLogContext
{
    /// <summary>
    /// 模块名（如 request、identity、middleware）
    /// </summary>
    public string? Module { get; set; }

    /// <summary>
    /// 动作名（如 start、finish、validate）
    /// </summary>
    public string? Action { get; set; }

    /// <summary>
    /// 用户 ID
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; }

    /// <summary>
    /// 公司编码
    /// </summary>
    public string? CompanyCode { get; set; }

    /// <summary>
    /// 请求路径或路由
    /// </summary>
    public string? Route { get; set; }

    /// <summary>
    /// 请求追踪 ID（短 RequestId，中间件生成）
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// OpenTelemetry TraceId（Activity.Current；无活动时为空）
    /// </summary>
    public string? TraceId { get; set; }

    /// <summary>
    /// 客户端 IP
    /// </summary>
    public string? ClientIp { get; set; }

    /// <summary>
    /// 扩展字段
    /// </summary>
    public Dictionary<string, object?>? Extra { get; set; }
}

/// <summary>
/// 序列化后的错误信息
/// </summary>
public class TaktLogErrorInfo
{
    /// <summary>
    /// 异常类型名
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 异常消息
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// 堆栈
    /// </summary>
    public string? Stack { get; set; }
}

/// <summary>
/// 标准日志条目（采集与上报结构）
/// </summary>
public class TaktLogEntry
{
    /// <summary>
    /// 日志级别
    /// </summary>
    public TaktLogLevel Level { get; set; }

    /// <summary>
    /// 主消息
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// ISO8601 时间戳
    /// </summary>
    public string Timestamp { get; set; } = string.Empty;

    /// <summary>
    /// 应用名称
    /// </summary>
    public string AppName { get; set; } = string.Empty;

    /// <summary>
    /// 应用版本
    /// </summary>
    public string AppVersion { get; set; } = string.Empty;

    /// <summary>
    /// 运行环境
    /// </summary>
    public string Environment { get; set; } = string.Empty;

    /// <summary>
    /// 机器名
    /// </summary>
    public string MachineName { get; set; } = string.Empty;

    /// <summary>
    /// 业务上下文
    /// </summary>
    public TaktLogContext? Context { get; set; }

    /// <summary>
    /// 关联错误
    /// </summary>
    public TaktLogErrorInfo? Error { get; set; }

    /// <summary>
    /// 标签
    /// </summary>
    public List<string>? Tags { get; set; }
}

/// <summary>
/// 批量上报载荷
/// </summary>
public class TaktLogReportPayload
{
    /// <summary>
    /// 批次 ID
    /// </summary>
    public string BatchId { get; set; } = string.Empty;

    /// <summary>
    /// 上报时间（ISO8601）
    /// </summary>
    public string ReportedAt { get; set; } = string.Empty;

    /// <summary>
    /// 日志条目
    /// </summary>
    public List<TaktLogEntry> Entries { get; set; } = [];
}

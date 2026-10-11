// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Models.Foundation
// 文件名称：TaktSignalRQuartzPushModels.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 定时任务 SignalR 实时推送模型（定义变更、执行完成）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Newtonsoft.Json;

namespace Takt.Shared.Models.Foundation;

/// <summary>
/// 定时任务定义变更推送模型
/// </summary>
public class TaktSignalRQuartzTaskChangedPush
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司编码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 定时任务 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QuartzTaskId { get; set; }

    /// <summary>
    /// 任务编码
    /// </summary>
    public string TaskCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称
    /// </summary>
    public string TaskName { get; set; } = string.Empty;

    /// <summary>
    /// 变更类型（create / update / delete / status）
    /// </summary>
    public string ChangeType { get; set; } = string.Empty;

    /// <summary>
    /// 操作人用户名
    /// </summary>
    public string? OperatorUserName { get; set; }

    /// <summary>
    /// 变更时间
    /// </summary>
    public DateTime ChangedAt { get; set; }
}

/// <summary>
/// 定时任务执行完成推送模型
/// </summary>
public class TaktSignalRQuartzTaskExecutedPush
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司编码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 定时任务 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QuartzTaskId { get; set; }

    /// <summary>
    /// 执行日志 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QuartzLogId { get; set; }

    /// <summary>
    /// 任务编码
    /// </summary>
    public string TaskCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称
    /// </summary>
    public string TaskName { get; set; } = string.Empty;

    /// <summary>
    /// 执行状态（int，与 TaktExecuteStatus 一致：0 失败 / 1 成功）
    /// </summary>
    public int ExecuteStatus { get; set; }

    /// <summary>
    /// 执行摘要消息（来自 TaktQuartzLog.ExecuteMessage）
    /// </summary>
    public string? ExecuteMessage { get; set; }

    /// <summary>
    /// 错误信息（来自 TaktQuartzLog.ErrorInfo；成功时为空）
    /// </summary>
    public string? ErrorInfo { get; set; }

    /// <summary>
    /// 执行耗时（毫秒）
    /// </summary>
    public long ExecuteDuration { get; set; }

    /// <summary>
    /// 累计执行次数
    /// </summary>
    public int ExecuteCount { get; set; }

    /// <summary>
    /// 上次执行
    /// </summary>
    public DateTime? LastRunAt { get; set; }

    /// <summary>
    /// 下次执行
    /// </summary>
    public DateTime? NextRunAt { get; set; }

    /// <summary>
    /// 触发用户名（手动触发时有值）
    /// </summary>
    public string? TriggerUserName { get; set; }

    /// <summary>
    /// 执行时间
    /// </summary>
    public DateTime ExecutedAt { get; set; }
}

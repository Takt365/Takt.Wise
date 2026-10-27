// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Models.Logistics.Manufacturing
// 文件名称：TaktEcChangeSignalRPushModels.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：工程变更 SignalR 推送模型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Newtonsoft.Json;

namespace Takt.Shared.Models.Logistics.Manufacturing;

/// <summary>
/// 变更通知推送模型（SendChangeNotification）
/// </summary>
public class TaktEcChangeNotificationPush
{
    /// <summary>
    /// 公司编码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 通知投递记录 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeliveryId { get; set; }

    /// <summary>
    /// 通知单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcNotificationId { get; set; }

    /// <summary>
    /// 通知单号
    /// </summary>
    public string EcNotificationCode { get; set; } = string.Empty;

    /// <summary>
    /// 设变 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcTeId { get; set; }

    /// <summary>
    /// 设变单号
    /// </summary>
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 设变标题
    /// </summary>
    public string? EcTitle { get; set; }

    /// <summary>
    /// 目标部门编码
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 优先级（1普通 2高 3紧急）
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    /// 推送时间
    /// </summary>
    public DateTime PushedAt { get; set; }
}

/// <summary>
/// 执行任务分配推送模型
/// </summary>
public class TaktEcExecutionTaskAssignedPush
{
    /// <summary>
    /// 公司编码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TaskId { get; set; }

    /// <summary>
    /// 设变单号
    /// </summary>
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务标题
    /// </summary>
    public string TaskTitle { get; set; } = string.Empty;

    /// <summary>
    /// 截止日期
    /// </summary>
    public DateTime? DueDate { get; set; }
}

/// <summary>
/// 任务进度推送模型（ReportProgress）
/// </summary>
public class TaktEcExecutionTaskProgressPush
{
    /// <summary>
    /// 公司编码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TaskId { get; set; }

    /// <summary>
    /// 设变单号
    /// </summary>
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务状态
    /// </summary>
    public int TaskStatus { get; set; }

    /// <summary>
    /// 进度百分比 0-100
    /// </summary>
    public int ProgressPercent { get; set; }

    /// <summary>
    /// 进度说明
    /// </summary>
    public string? ProgressRemark { get; set; }

    /// <summary>
    /// 上报人用户名
    /// </summary>
    public string? ReporterUserName { get; set; }

    /// <summary>
    /// 上报时间
    /// </summary>
    public DateTime ReportedAt { get; set; }
}

/// <summary>
/// 变更闭环完成推送
/// </summary>
public class TaktEcChangeClosedPush
{
    /// <summary>
    /// 公司编码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 设变 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcTeId { get; set; }

    /// <summary>
    /// 设变单号
    /// </summary>
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 通知单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcNotificationId { get; set; }

    /// <summary>
    /// 完成时间
    /// </summary>
    public DateTime ClosedAt { get; set; }
}

/// <summary>
/// 任务超时/阻塞预警推送
/// </summary>
public class TaktEcExecutionTaskAlertPush
{
    /// <summary>
    /// 公司编码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TaskId { get; set; }

    /// <summary>
    /// 设变单号
    /// </summary>
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 预警类型（overdue/blocked）
    /// </summary>
    public string AlertType { get; set; } = string.Empty;

    /// <summary>
    /// 预警说明
    /// </summary>
    public string Message { get; set; } = string.Empty;
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcChangeFlowDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：工程变更通知与执行监测流程 DTO
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Newtonsoft.Json;

namespace Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 通知确认请求
/// </summary>
public class TaktEcNotificationConfirmDto
{
    /// <summary>
    /// 投递记录 ID（与 notificationId+ecDeptGroupCode 二选一）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeliveryId { get; set; }

    /// <summary>
    /// 通知单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcNotificationId { get; set; }

    /// <summary>
    /// 设变部门编码（选项 TaktDepts/options；DictValue=DeptCode）
    /// </summary>
    public string? EcDeptGroupCode { get; set; }
}

/// <summary>
/// 执行任务进度上报
/// </summary>
public class TaktEcExecutionTaskProgressReportDto
{
    /// <summary>
    /// 任务 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TaskId { get; set; }

    /// <summary>
    /// 任务状态（0待执行 1执行中 2已完成 3阻塞）
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
}

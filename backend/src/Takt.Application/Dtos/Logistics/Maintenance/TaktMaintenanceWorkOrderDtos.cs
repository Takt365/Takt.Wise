// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Maintenance
// 文件名称：TaktMaintenanceWorkOrderDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：MaintenanceWorkOrder 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMaintenanceWorkOrder 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Maintenance;

// ========================================
// MaintenanceWorkOrder 响应 DTO
// ========================================

/// <summary>
/// 维护工单实体（由通知单转入或直接创建；执行领料、报工、完工；材料/人工成本汇总于头表 TotalCost 等字段）。FlowInstanceId 由业务在发起流程后写入；流程引擎通过 BusinessKey/BusinessType 与本模块对接。
/// 对应前端 TaktMaintenanceWorkOrderDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktMaintenanceWorkOrderDto : TaktApprovalDtoBase
{
    /// <summary>
    /// MaintenanceWorkOrderID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 维护工单号
    /// </summary>
    public string WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源维护通知单ID（直接建单可为空，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaintenanceNotificationId { get; set; }

    /// <summary>
    /// 来源维护通知单名称（填充字段）
    /// </summary>
    public string? MaintenanceNotificationName { get; set; }

    /// <summary>
    /// 来源通知单号（冗余）
    /// </summary>
    public string? NotificationCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentId { get; set; }

    /// <summary>
    /// 设备编码（冗余）
    /// </summary>
    public string EquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备名称（冗余）
    /// </summary>
    public string EquipmentName { get; set; } = string.Empty;

    /// <summary>
    /// 维护类别（字典 logistics_maintenance_category）
    /// </summary>
    public int MaintenanceCategory { get; set; } = 0;

    /// <summary>
    /// 维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
    /// </summary>
    public int MaintenanceType { get; set; } = 0;

    /// <summary>
    /// 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
    /// </summary>
    public int WorkOrderStatus { get; set; } = 0;

    /// <summary>
    /// 优先级（1=低，2=中，3=高，4=紧急）
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 工作中心
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 指派技师（人员编码）
    /// </summary>
    public string? AssignedTechnician { get; set; } = string.Empty;

    /// <summary>
    /// 维护单位
    /// </summary>
    public string? MaintenanceCompany { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 实际开始时间
    /// </summary>
    public DateTime? ActualStartTime { get; set; }

    /// <summary>
    /// 实际结束时间
    /// </summary>
    public DateTime? ActualEndTime { get; set; }

    /// <summary>
    /// 故障描述
    /// </summary>
    public string? FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 维护内容
    /// </summary>
    public string? MaintenanceContent { get; set; } = string.Empty;

    /// <summary>
    /// 处理方案
    /// </summary>
    public string? Solution { get; set; } = string.Empty;

    /// <summary>
    /// 结算成本中心ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostCenterId { get; set; }

    /// <summary>
    /// 结算成本中心名称（填充字段）
    /// </summary>
    public string? CostCenterName { get; set; }

    /// <summary>
    /// 结算成本中心编码（冗余）
    /// </summary>
    public string? CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本要素ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostElementId { get; set; }

    /// <summary>
    /// 成本要素名称（填充字段）
    /// </summary>
    public string? CostElementName { get; set; }

    /// <summary>
    /// 成本要素编码（冗余）
    /// </summary>
    public string? CostElementCode { get; set; } = string.Empty;

    /// <summary>
    /// 材料成本合计
    /// </summary>
    public decimal TotalMaterialCost { get; set; }

    /// <summary>
    /// 人工成本合计
    /// </summary>
    public decimal TotalLaborCost { get; set; }

    /// <summary>
    /// 其他成本合计
    /// </summary>
    public decimal TotalOtherCost { get; set; }

    /// <summary>
    /// 总成本
    /// </summary>
    public decimal TotalCost { get; set; }

    /// <summary>
    /// 结算状态（0=未结算，1=部分结算，2=已结算）
    /// </summary>
    public int SettlementStatus { get; set; } = 0;

    /// <summary>
    /// 结算时间
    /// </summary>
    public DateTime? SettlementTime { get; set; }

    /// <summary>
    /// 完工时间
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// 验收人（人员编码）
    /// </summary>
    public string? AcceptedByEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 验收时间
    /// </summary>
    public DateTime? AcceptedAt { get; set; }

    /// <summary>
    /// 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
    /// </summary>
    public int MaintenanceResult { get; set; } = 0;

    /// <summary>
    /// 下次维护日期
    /// </summary>
    public DateTime? NextMaintenanceDate { get; set; }

    /// <summary>
    /// 维护周期（天）
    /// </summary>
    public int MaintenanceCycleDays { get; set; } = 0;

    /// <summary>
    /// 维护图片（JSON格式，存储维护图片URL列表）
    /// </summary>
    public string? MaintenanceImages { get; set; } = string.Empty;

    /// <summary>
    /// 维护文档（JSON格式，存储维护文档ID列表）
    /// </summary>
    public string? MaintenanceDocuments { get; set; } = string.Empty;

    /// <summary>
    /// 验收总结
    /// </summary>
    public string? AcceptedSummary { get; set; } = string.Empty;

    /// <summary>
    /// 是否已归档至维护履历（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int IsHistoryArchived { get; set; } = 0;

    /// <summary>
    /// 来源维护通知单
    /// （主表：TaktMaintenanceNotification）
    /// </summary>
    public TaktMaintenanceNotificationDto? MaintenanceNotification { get; set; }

    /// <summary>
    /// 设备（主数据）
    /// （主表：TaktEquipment）
    /// </summary>
    public TaktEquipmentDto? Equipment { get; set; }

    /// <summary>
    /// 领料明细
    /// （子表：TaktMaintenanceWorkOrderMaterial）
    /// </summary>
    public List<TaktMaintenanceWorkOrderMaterialDto>? Materials { get; set; }

    /// <summary>
    /// 报工明细
    /// （子表：TaktMaintenanceWorkOrderLabor）
    /// </summary>
    public List<TaktMaintenanceWorkOrderLaborDto>? Labors { get; set; }

}

// ========================================
// MaintenanceWorkOrder 查询 DTO
// ========================================

/// <summary>
/// MaintenanceWorkOrder 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMaintenanceWorkOrderQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 维护工单号
    /// </summary>
    public string? WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源维护通知单ID（直接建单可为空，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaintenanceNotificationId { get; set; }

    /// <summary>
    /// 来源通知单号（冗余）
    /// </summary>
    public string? NotificationCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EquipmentId { get; set; }

    /// <summary>
    /// 设备编码（冗余）
    /// </summary>
    public string? EquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备名称（冗余）
    /// </summary>
    public string? EquipmentName { get; set; } = string.Empty;

    /// <summary>
    /// 维护类别（字典 logistics_maintenance_category）
    /// </summary>
    public int? MaintenanceCategory { get; set; }

    /// <summary>
    /// 维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
    /// </summary>
    public int? MaintenanceType { get; set; }

    /// <summary>
    /// 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
    /// </summary>
    public int? WorkOrderStatus { get; set; }

    /// <summary>
    /// 优先级（1=低，2=中，3=高，4=紧急）
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 工作中心
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 指派技师（人员编码）
    /// </summary>
    public string? AssignedTechnician { get; set; } = string.Empty;

    /// <summary>
    /// 维护单位
    /// </summary>
    public string? MaintenanceCompany { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始时间（范围查询-开始）
    /// </summary>
    public DateTime? PlannedStartTimeStart { get; set; }

    /// <summary>
    /// 计划开始时间（范围查询-结束）
    /// </summary>
    public DateTime? PlannedStartTimeEnd { get; set; }

    /// <summary>
    /// 计划结束时间（范围查询-开始）
    /// </summary>
    public DateTime? PlannedEndTimeStart { get; set; }

    /// <summary>
    /// 计划结束时间（范围查询-结束）
    /// </summary>
    public DateTime? PlannedEndTimeEnd { get; set; }

    /// <summary>
    /// 实际开始时间（范围查询-开始）
    /// </summary>
    public DateTime? ActualStartTimeStart { get; set; }

    /// <summary>
    /// 实际开始时间（范围查询-结束）
    /// </summary>
    public DateTime? ActualStartTimeEnd { get; set; }

    /// <summary>
    /// 实际结束时间（范围查询-开始）
    /// </summary>
    public DateTime? ActualEndTimeStart { get; set; }

    /// <summary>
    /// 实际结束时间（范围查询-结束）
    /// </summary>
    public DateTime? ActualEndTimeEnd { get; set; }

    /// <summary>
    /// 故障描述
    /// </summary>
    public string? FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 维护内容
    /// </summary>
    public string? MaintenanceContent { get; set; } = string.Empty;

    /// <summary>
    /// 处理方案
    /// </summary>
    public string? Solution { get; set; } = string.Empty;

    /// <summary>
    /// 结算成本中心ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostCenterId { get; set; }

    /// <summary>
    /// 结算成本中心编码（冗余）
    /// </summary>
    public string? CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本要素ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostElementId { get; set; }

    /// <summary>
    /// 成本要素编码（冗余）
    /// </summary>
    public string? CostElementCode { get; set; } = string.Empty;

    /// <summary>
    /// 材料成本合计
    /// </summary>
    public decimal? TotalMaterialCost { get; set; }

    /// <summary>
    /// 人工成本合计
    /// </summary>
    public decimal? TotalLaborCost { get; set; }

    /// <summary>
    /// 其他成本合计
    /// </summary>
    public decimal? TotalOtherCost { get; set; }

    /// <summary>
    /// 总成本
    /// </summary>
    public decimal? TotalCost { get; set; }

    /// <summary>
    /// 结算状态（0=未结算，1=部分结算，2=已结算）
    /// </summary>
    public int? SettlementStatus { get; set; }

    /// <summary>
    /// 结算时间（范围查询-开始）
    /// </summary>
    public DateTime? SettlementTimeStart { get; set; }

    /// <summary>
    /// 结算时间（范围查询-结束）
    /// </summary>
    public DateTime? SettlementTimeEnd { get; set; }

    /// <summary>
    /// 完工时间（范围查询-开始）
    /// </summary>
    public DateTime? CompletedAtStart { get; set; }

    /// <summary>
    /// 完工时间（范围查询-结束）
    /// </summary>
    public DateTime? CompletedAtEnd { get; set; }

    /// <summary>
    /// 验收人（人员编码）
    /// </summary>
    public string? AcceptedByEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 验收时间（范围查询-开始）
    /// </summary>
    public DateTime? AcceptedAtStart { get; set; }

    /// <summary>
    /// 验收时间（范围查询-结束）
    /// </summary>
    public DateTime? AcceptedAtEnd { get; set; }

    /// <summary>
    /// 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
    /// </summary>
    public int? MaintenanceResult { get; set; }

    /// <summary>
    /// 下次维护日期（范围查询-开始）
    /// </summary>
    public DateTime? NextMaintenanceDateStart { get; set; }

    /// <summary>
    /// 下次维护日期（范围查询-结束）
    /// </summary>
    public DateTime? NextMaintenanceDateEnd { get; set; }

    /// <summary>
    /// 维护周期（天）
    /// </summary>
    public int? MaintenanceCycleDays { get; set; }

    /// <summary>
    /// 维护图片（JSON格式，存储维护图片URL列表）
    /// </summary>
    public string? MaintenanceImages { get; set; } = string.Empty;

    /// <summary>
    /// 维护文档（JSON格式，存储维护文档ID列表）
    /// </summary>
    public string? MaintenanceDocuments { get; set; } = string.Empty;

    /// <summary>
    /// 验收总结
    /// </summary>
    public string? AcceptedSummary { get; set; } = string.Empty;

    /// <summary>
    /// 是否已归档至维护履历（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? IsHistoryArchived { get; set; }

    /// <summary>
    /// 审批状态（字典 sys_approval_status；与 TaktApprovalEntityBase.ApprovalStatus 一致）
    /// </summary>
    public TaktApprovalStatus? ApprovalStatus { get; set; }

    /// <summary>
    /// 发起人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InitiatorId { get; set; }

    /// <summary>
    /// 发起时间（范围查询-开始）
    /// </summary>
    public DateTime? InitiatedAtStart { get; set; }

    /// <summary>
    /// 发起时间（范围查询-结束）
    /// </summary>
    public DateTime? InitiatedAtEnd { get; set; }

    /// <summary>
    /// 最终审批人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApprovedBy { get; set; }

    /// <summary>
    /// 最终审批时间（范围查询-开始）
    /// </summary>
    public DateTime? ApprovedAtStart { get; set; }

    /// <summary>
    /// 最终审批时间（范围查询-结束）
    /// </summary>
    public DateTime? ApprovedAtEnd { get; set; }

    /// <summary>
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建MaintenanceWorkOrder DTO
// ========================================

/// <summary>
/// 创建MaintenanceWorkOrder DTO
/// </summary>
public class TaktMaintenanceWorkOrderCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 维护工单号
    /// </summary>
    [Required(ErrorMessage = "维护工单号不能为空")]
    public string WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源维护通知单ID（直接建单可为空，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaintenanceNotificationId { get; set; }

    /// <summary>
    /// 来源通知单号（冗余）
    /// </summary>
    public string? NotificationCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentId { get; set; }

    /// <summary>
    /// 设备编码（冗余）
    /// </summary>
    [Required(ErrorMessage = "设备编码（冗余）不能为空")]
    public string EquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备名称（冗余）
    /// </summary>
    public string EquipmentName { get; set; } = string.Empty;

    /// <summary>
    /// 维护类别（字典 logistics_maintenance_category）
    /// </summary>
    public int MaintenanceCategory { get; set; } = 0;

    /// <summary>
    /// 维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
    /// </summary>
    public int MaintenanceType { get; set; } = 0;

    /// <summary>
    /// 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
    /// </summary>
    public int WorkOrderStatus { get; set; } = 0;

    /// <summary>
    /// 优先级（1=低，2=中，3=高，4=紧急）
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 工作中心
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 指派技师（人员编码）
    /// </summary>
    public string? AssignedTechnician { get; set; } = string.Empty;

    /// <summary>
    /// 维护单位
    /// </summary>
    public string? MaintenanceCompany { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 实际开始时间
    /// </summary>
    public DateTime? ActualStartTime { get; set; }

    /// <summary>
    /// 实际结束时间
    /// </summary>
    public DateTime? ActualEndTime { get; set; }

    /// <summary>
    /// 故障描述
    /// </summary>
    public string? FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 维护内容
    /// </summary>
    public string? MaintenanceContent { get; set; } = string.Empty;

    /// <summary>
    /// 处理方案
    /// </summary>
    public string? Solution { get; set; } = string.Empty;

    /// <summary>
    /// 结算成本中心ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostCenterId { get; set; }

    /// <summary>
    /// 结算成本中心编码（冗余）
    /// </summary>
    public string? CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本要素ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostElementId { get; set; }

    /// <summary>
    /// 成本要素编码（冗余）
    /// </summary>
    public string? CostElementCode { get; set; } = string.Empty;

    /// <summary>
    /// 材料成本合计
    /// </summary>
    public decimal TotalMaterialCost { get; set; }

    /// <summary>
    /// 人工成本合计
    /// </summary>
    public decimal TotalLaborCost { get; set; }

    /// <summary>
    /// 其他成本合计
    /// </summary>
    public decimal TotalOtherCost { get; set; }

    /// <summary>
    /// 总成本
    /// </summary>
    public decimal TotalCost { get; set; }

    /// <summary>
    /// 结算状态（0=未结算，1=部分结算，2=已结算）
    /// </summary>
    public int SettlementStatus { get; set; } = 0;

    /// <summary>
    /// 结算时间
    /// </summary>
    public DateTime? SettlementTime { get; set; }

    /// <summary>
    /// 完工时间
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// 验收人（人员编码）
    /// </summary>
    public string? AcceptedByEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 验收时间
    /// </summary>
    public DateTime? AcceptedAt { get; set; }

    /// <summary>
    /// 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
    /// </summary>
    public int MaintenanceResult { get; set; } = 0;

    /// <summary>
    /// 下次维护日期
    /// </summary>
    public DateTime? NextMaintenanceDate { get; set; }

    /// <summary>
    /// 维护周期（天）
    /// </summary>
    public int MaintenanceCycleDays { get; set; } = 0;

    /// <summary>
    /// 维护图片（JSON格式，存储维护图片URL列表）
    /// </summary>
    public string? MaintenanceImages { get; set; } = string.Empty;

    /// <summary>
    /// 维护文档（JSON格式，存储维护文档ID列表）
    /// </summary>
    public string? MaintenanceDocuments { get; set; } = string.Empty;

    /// <summary>
    /// 验收总结
    /// </summary>
    public string? AcceptedSummary { get; set; } = string.Empty;

    /// <summary>
    /// 是否已归档至维护履历（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int IsHistoryArchived { get; set; } = 0;

    /// <summary>
    /// 领料明细（子表，级联保存）
    /// </summary>
    public List<TaktMaintenanceWorkOrderMaterialCreateDto>? Materials { get; set; }

    /// <summary>
    /// 报工明细（子表，级联保存）
    /// </summary>
    public List<TaktMaintenanceWorkOrderLaborCreateDto>? Labors { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新MaintenanceWorkOrder DTO
// ========================================

/// <summary>
/// 更新MaintenanceWorkOrder DTO
/// 继承 TaktMaintenanceWorkOrderCreateDto，添加 MaintenanceWorkOrderId 字段
/// </summary>
public class TaktMaintenanceWorkOrderUpdateDto : TaktMaintenanceWorkOrderCreateDto
{
    /// <summary>
    /// MaintenanceWorkOrderID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 领料明细（子表，级联保存）
    /// </summary>
    public new List<TaktMaintenanceWorkOrderMaterialUpdateDto>? Materials { get; set; }

    /// <summary>
    /// 报工明细（子表，级联保存）
    /// </summary>
    public new List<TaktMaintenanceWorkOrderLaborUpdateDto>? Labors { get; set; }

}

// ========================================
// MaintenanceWorkOrder 状态 DTO
// ========================================

/// <summary>
/// MaintenanceWorkOrder 状态更新 DTO
/// </summary>
public class TaktMaintenanceWorkOrderStatusDto
{
    /// <summary>
    /// MaintenanceWorkOrderID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
    /// </summary>
    [Required(ErrorMessage = "工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）不能为空")]
    public int WorkOrderStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// MaintenanceWorkOrder 导入模板行 DTO
/// </summary>
public class TaktMaintenanceWorkOrderTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 维护工单号
    /// </summary>
    public string? WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源维护通知单ID（直接建单可为空，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaintenanceNotificationId { get; set; }

    /// <summary>
    /// 来源通知单号（冗余）
    /// </summary>
    public string? NotificationCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EquipmentId { get; set; }

    /// <summary>
    /// 设备编码（冗余）
    /// </summary>
    public string? EquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备名称（冗余）
    /// </summary>
    public string? EquipmentName { get; set; } = string.Empty;

    /// <summary>
    /// 维护类别（字典 logistics_maintenance_category）
    /// </summary>
    public int? MaintenanceCategory { get; set; }

    /// <summary>
    /// 维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
    /// </summary>
    public int? MaintenanceType { get; set; }

    /// <summary>
    /// 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
    /// </summary>
    public int? WorkOrderStatus { get; set; }

    /// <summary>
    /// 优先级（1=低，2=中，3=高，4=紧急）
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 工作中心
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 指派技师（人员编码）
    /// </summary>
    public string? AssignedTechnician { get; set; } = string.Empty;

    /// <summary>
    /// 维护单位
    /// </summary>
    public string? MaintenanceCompany { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 实际开始时间
    /// </summary>
    public DateTime? ActualStartTime { get; set; }

    /// <summary>
    /// 实际结束时间
    /// </summary>
    public DateTime? ActualEndTime { get; set; }

    /// <summary>
    /// 故障描述
    /// </summary>
    public string? FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 维护内容
    /// </summary>
    public string? MaintenanceContent { get; set; } = string.Empty;

    /// <summary>
    /// 处理方案
    /// </summary>
    public string? Solution { get; set; } = string.Empty;

    /// <summary>
    /// 结算成本中心ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostCenterId { get; set; }

    /// <summary>
    /// 结算成本中心编码（冗余）
    /// </summary>
    public string? CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本要素ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostElementId { get; set; }

    /// <summary>
    /// 成本要素编码（冗余）
    /// </summary>
    public string? CostElementCode { get; set; } = string.Empty;

    /// <summary>
    /// 材料成本合计
    /// </summary>
    public decimal? TotalMaterialCost { get; set; }

    /// <summary>
    /// 人工成本合计
    /// </summary>
    public decimal? TotalLaborCost { get; set; }

    /// <summary>
    /// 其他成本合计
    /// </summary>
    public decimal? TotalOtherCost { get; set; }

    /// <summary>
    /// 总成本
    /// </summary>
    public decimal? TotalCost { get; set; }

    /// <summary>
    /// 结算状态（0=未结算，1=部分结算，2=已结算）
    /// </summary>
    public int? SettlementStatus { get; set; }

    /// <summary>
    /// 结算时间
    /// </summary>
    public DateTime? SettlementTime { get; set; }

    /// <summary>
    /// 完工时间
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// 验收人（人员编码）
    /// </summary>
    public string? AcceptedByEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 验收时间
    /// </summary>
    public DateTime? AcceptedAt { get; set; }

    /// <summary>
    /// 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
    /// </summary>
    public int? MaintenanceResult { get; set; }

    /// <summary>
    /// 下次维护日期
    /// </summary>
    public DateTime? NextMaintenanceDate { get; set; }

    /// <summary>
    /// 维护周期（天）
    /// </summary>
    public int? MaintenanceCycleDays { get; set; }

    /// <summary>
    /// 维护图片（JSON格式，存储维护图片URL列表）
    /// </summary>
    public string? MaintenanceImages { get; set; } = string.Empty;

    /// <summary>
    /// 维护文档（JSON格式，存储维护文档ID列表）
    /// </summary>
    public string? MaintenanceDocuments { get; set; } = string.Empty;

    /// <summary>
    /// 验收总结
    /// </summary>
    public string? AcceptedSummary { get; set; } = string.Empty;

    /// <summary>
    /// 是否已归档至维护履历（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? IsHistoryArchived { get; set; }

    /// <summary>
    /// 领料明细（子表，级联保存）
    /// </summary>
    public List<TaktMaintenanceWorkOrderMaterialCreateDto>? Materials { get; set; }

    /// <summary>
    /// 报工明细（子表，级联保存）
    /// </summary>
    public List<TaktMaintenanceWorkOrderLaborCreateDto>? Labors { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// MaintenanceWorkOrder 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMaintenanceWorkOrderImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 维护工单号
    /// </summary>
    public string? WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源维护通知单ID（直接建单可为空，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaintenanceNotificationId { get; set; }

    /// <summary>
    /// 来源通知单号（冗余）
    /// </summary>
    public string? NotificationCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EquipmentId { get; set; }

    /// <summary>
    /// 设备编码（冗余）
    /// </summary>
    public string? EquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备名称（冗余）
    /// </summary>
    public string? EquipmentName { get; set; } = string.Empty;

    /// <summary>
    /// 维护类别（字典 logistics_maintenance_category）
    /// </summary>
    public int? MaintenanceCategory { get; set; }

    /// <summary>
    /// 维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
    /// </summary>
    public int? MaintenanceType { get; set; }

    /// <summary>
    /// 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
    /// </summary>
    public int? WorkOrderStatus { get; set; }

    /// <summary>
    /// 优先级（1=低，2=中，3=高，4=紧急）
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 工作中心
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 指派技师（人员编码）
    /// </summary>
    public string? AssignedTechnician { get; set; } = string.Empty;

    /// <summary>
    /// 维护单位
    /// </summary>
    public string? MaintenanceCompany { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 实际开始时间
    /// </summary>
    public DateTime? ActualStartTime { get; set; }

    /// <summary>
    /// 实际结束时间
    /// </summary>
    public DateTime? ActualEndTime { get; set; }

    /// <summary>
    /// 故障描述
    /// </summary>
    public string? FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 维护内容
    /// </summary>
    public string? MaintenanceContent { get; set; } = string.Empty;

    /// <summary>
    /// 处理方案
    /// </summary>
    public string? Solution { get; set; } = string.Empty;

    /// <summary>
    /// 结算成本中心ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostCenterId { get; set; }

    /// <summary>
    /// 结算成本中心编码（冗余）
    /// </summary>
    public string? CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本要素ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostElementId { get; set; }

    /// <summary>
    /// 成本要素编码（冗余）
    /// </summary>
    public string? CostElementCode { get; set; } = string.Empty;

    /// <summary>
    /// 材料成本合计
    /// </summary>
    public decimal? TotalMaterialCost { get; set; }

    /// <summary>
    /// 人工成本合计
    /// </summary>
    public decimal? TotalLaborCost { get; set; }

    /// <summary>
    /// 其他成本合计
    /// </summary>
    public decimal? TotalOtherCost { get; set; }

    /// <summary>
    /// 总成本
    /// </summary>
    public decimal? TotalCost { get; set; }

    /// <summary>
    /// 结算状态（0=未结算，1=部分结算，2=已结算）
    /// </summary>
    public int? SettlementStatus { get; set; }

    /// <summary>
    /// 结算时间
    /// </summary>
    public DateTime? SettlementTime { get; set; }

    /// <summary>
    /// 完工时间
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// 验收人（人员编码）
    /// </summary>
    public string? AcceptedByEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 验收时间
    /// </summary>
    public DateTime? AcceptedAt { get; set; }

    /// <summary>
    /// 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
    /// </summary>
    public int? MaintenanceResult { get; set; }

    /// <summary>
    /// 下次维护日期
    /// </summary>
    public DateTime? NextMaintenanceDate { get; set; }

    /// <summary>
    /// 维护周期（天）
    /// </summary>
    public int? MaintenanceCycleDays { get; set; }

    /// <summary>
    /// 维护图片（JSON格式，存储维护图片URL列表）
    /// </summary>
    public string? MaintenanceImages { get; set; } = string.Empty;

    /// <summary>
    /// 维护文档（JSON格式，存储维护文档ID列表）
    /// </summary>
    public string? MaintenanceDocuments { get; set; } = string.Empty;

    /// <summary>
    /// 验收总结
    /// </summary>
    public string? AcceptedSummary { get; set; } = string.Empty;

    /// <summary>
    /// 是否已归档至维护履历（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? IsHistoryArchived { get; set; }

    /// <summary>
    /// 领料明细（子表，级联保存）
    /// </summary>
    public List<TaktMaintenanceWorkOrderMaterialCreateDto>? Materials { get; set; }

    /// <summary>
    /// 报工明细（子表，级联保存）
    /// </summary>
    public List<TaktMaintenanceWorkOrderLaborCreateDto>? Labors { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// MaintenanceWorkOrder 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMaintenanceWorkOrderExportDto
{
    /// <summary>
    /// MaintenanceWorkOrderID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 维护工单号
    /// </summary>
    public string WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源维护通知单ID（直接建单可为空，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaintenanceNotificationId { get; set; }

    /// <summary>
    /// 来源通知单号（冗余）
    /// </summary>
    public string? NotificationCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentId { get; set; }

    /// <summary>
    /// 设备编码（冗余）
    /// </summary>
    public string EquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备名称（冗余）
    /// </summary>
    public string EquipmentName { get; set; } = string.Empty;

    /// <summary>
    /// 维护类别（字典 logistics_maintenance_category）
    /// </summary>
    public int MaintenanceCategory { get; set; } = 0;

    /// <summary>
    /// 维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
    /// </summary>
    public int MaintenanceType { get; set; } = 0;

    /// <summary>
    /// 工单状态（字典 sys_ticket_status；0=新建，1=已分配，2=处理中，3=待确认，4=已完成，5=已关闭，6=已取消）
    /// </summary>
    public int WorkOrderStatus { get; set; } = 0;

    /// <summary>
    /// 优先级（1=低，2=中，3=高，4=紧急）
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 工作中心
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 指派技师（人员编码）
    /// </summary>
    public string? AssignedTechnician { get; set; } = string.Empty;

    /// <summary>
    /// 维护单位
    /// </summary>
    public string? MaintenanceCompany { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 实际开始时间
    /// </summary>
    public DateTime? ActualStartTime { get; set; }

    /// <summary>
    /// 实际结束时间
    /// </summary>
    public DateTime? ActualEndTime { get; set; }

    /// <summary>
    /// 故障描述
    /// </summary>
    public string? FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 维护内容
    /// </summary>
    public string? MaintenanceContent { get; set; } = string.Empty;

    /// <summary>
    /// 处理方案
    /// </summary>
    public string? Solution { get; set; } = string.Empty;

    /// <summary>
    /// 结算成本中心ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostCenterId { get; set; }

    /// <summary>
    /// 结算成本中心编码（冗余）
    /// </summary>
    public string? CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本要素ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostElementId { get; set; }

    /// <summary>
    /// 成本要素编码（冗余）
    /// </summary>
    public string? CostElementCode { get; set; } = string.Empty;

    /// <summary>
    /// 材料成本合计
    /// </summary>
    public decimal TotalMaterialCost { get; set; }

    /// <summary>
    /// 人工成本合计
    /// </summary>
    public decimal TotalLaborCost { get; set; }

    /// <summary>
    /// 其他成本合计
    /// </summary>
    public decimal TotalOtherCost { get; set; }

    /// <summary>
    /// 总成本
    /// </summary>
    public decimal TotalCost { get; set; }

    /// <summary>
    /// 结算状态（0=未结算，1=部分结算，2=已结算）
    /// </summary>
    public int SettlementStatus { get; set; } = 0;

    /// <summary>
    /// 结算时间
    /// </summary>
    public DateTime? SettlementTime { get; set; }

    /// <summary>
    /// 完工时间
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// 验收人（人员编码）
    /// </summary>
    public string? AcceptedByEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 验收时间
    /// </summary>
    public DateTime? AcceptedAt { get; set; }

    /// <summary>
    /// 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
    /// </summary>
    public int MaintenanceResult { get; set; } = 0;

    /// <summary>
    /// 下次维护日期
    /// </summary>
    public DateTime? NextMaintenanceDate { get; set; }

    /// <summary>
    /// 维护周期（天）
    /// </summary>
    public int MaintenanceCycleDays { get; set; } = 0;

    /// <summary>
    /// 维护图片（JSON格式，存储维护图片URL列表）
    /// </summary>
    public string? MaintenanceImages { get; set; } = string.Empty;

    /// <summary>
    /// 维护文档（JSON格式，存储维护文档ID列表）
    /// </summary>
    public string? MaintenanceDocuments { get; set; } = string.Empty;

    /// <summary>
    /// 验收总结
    /// </summary>
    public string? AcceptedSummary { get; set; } = string.Empty;

    /// <summary>
    /// 是否已归档至维护履历（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int IsHistoryArchived { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

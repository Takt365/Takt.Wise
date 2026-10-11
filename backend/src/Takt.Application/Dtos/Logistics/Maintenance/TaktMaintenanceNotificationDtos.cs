// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Maintenance
// 文件名称：TaktMaintenanceNotificationDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：MaintenanceNotification 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMaintenanceNotification 生成，请按需审阅）
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
// MaintenanceNotification 响应 DTO
// ========================================

/// <summary>
/// 维护通知单实体（流程起点：发现异常 → 开通知单 → 转/建维护工单）。FlowInstanceId 由业务在发起流程后写入；流程引擎通过 BusinessKey/BusinessType 与本模块对接。
/// 对应前端 TaktMaintenanceNotificationDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktMaintenanceNotificationDto : TaktApprovalDtoBase
{
    /// <summary>
    /// MaintenanceNotificationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceNotificationId { get; set; }

    /// <summary>
    /// 通知单号
    /// </summary>
    public string NotificationCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentId { get; set; }

    /// <summary>
    /// 设备编码（冗余，便于查询）
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
    /// 优先级（1=低，2=中，3=高，4=紧急）
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 通知单状态（0=新建，1=已转工单，2=已关闭，3=已取消）
    /// </summary>
    public int NotificationStatus { get; set; } = 0;

    /// <summary>
    /// 异常/故障描述
    /// </summary>
    public string FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 发现时间
    /// </summary>
    public DateTime DiscoveredAt { get; set; }

    /// <summary>
    /// 故障开始时间
    /// </summary>
    public DateTime? BreakdownStartTime { get; set; }

    /// <summary>
    /// 故障结束时间
    /// </summary>
    public DateTime? BreakdownEndTime { get; set; }

    /// <summary>
    /// 报告人（人员编码）
    /// </summary>
    public string? ReportedByEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 责任成本中心ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostCenterId { get; set; }

    /// <summary>
    /// 责任成本中心名称（填充字段）
    /// </summary>
    public string? CostCenterName { get; set; }

    /// <summary>
    /// 责任成本中心编码（冗余）
    /// </summary>
    public string? CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联维护工单ID（转工单后回填，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 关联维护工单名称（填充字段）
    /// </summary>
    public string? MaintenanceWorkOrderName { get; set; }

    /// <summary>
    /// 关联维护工单号（冗余）
    /// </summary>
    public string? MaintenanceWorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 通知图片（JSON格式，存储图片URL列表）
    /// </summary>
    public string? NotificationImages { get; set; } = string.Empty;

    /// <summary>
    /// 设备（主数据）
    /// （主表：TaktEquipment）
    /// </summary>
    public TaktEquipmentDto? Equipment { get; set; }

    /// <summary>
    /// 关联维护工单
    /// （主表：TaktMaintenanceWorkOrder）
    /// </summary>
    public TaktMaintenanceWorkOrderDto? MaintenanceWorkOrder { get; set; }

}

// ========================================
// MaintenanceNotification 查询 DTO
// ========================================

/// <summary>
/// MaintenanceNotification 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMaintenanceNotificationQueryDto : TaktPagedQuery
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
    /// 通知单号
    /// </summary>
    public string? NotificationCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EquipmentId { get; set; }

    /// <summary>
    /// 设备编码（冗余，便于查询）
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
    /// 优先级（1=低，2=中，3=高，4=紧急）
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 通知单状态（0=新建，1=已转工单，2=已关闭，3=已取消）
    /// </summary>
    public int? NotificationStatus { get; set; }

    /// <summary>
    /// 异常/故障描述
    /// </summary>
    public string? FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 发现时间（范围查询-开始）
    /// </summary>
    public DateTime? DiscoveredAtStart { get; set; }

    /// <summary>
    /// 发现时间（范围查询-结束）
    /// </summary>
    public DateTime? DiscoveredAtEnd { get; set; }

    /// <summary>
    /// 故障开始时间（范围查询-开始）
    /// </summary>
    public DateTime? BreakdownStartTimeStart { get; set; }

    /// <summary>
    /// 故障开始时间（范围查询-结束）
    /// </summary>
    public DateTime? BreakdownStartTimeEnd { get; set; }

    /// <summary>
    /// 故障结束时间（范围查询-开始）
    /// </summary>
    public DateTime? BreakdownEndTimeStart { get; set; }

    /// <summary>
    /// 故障结束时间（范围查询-结束）
    /// </summary>
    public DateTime? BreakdownEndTimeEnd { get; set; }

    /// <summary>
    /// 报告人（人员编码）
    /// </summary>
    public string? ReportedByEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 责任成本中心ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostCenterId { get; set; }

    /// <summary>
    /// 责任成本中心编码（冗余）
    /// </summary>
    public string? CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联维护工单ID（转工单后回填，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 关联维护工单号（冗余）
    /// </summary>
    public string? MaintenanceWorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 通知图片（JSON格式，存储图片URL列表）
    /// </summary>
    public string? NotificationImages { get; set; } = string.Empty;

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
// 创建MaintenanceNotification DTO
// ========================================

/// <summary>
/// 创建MaintenanceNotification DTO
/// </summary>
public class TaktMaintenanceNotificationCreateDto
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
    /// 通知单号
    /// </summary>
    [Required(ErrorMessage = "通知单号不能为空")]
    public string NotificationCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentId { get; set; }

    /// <summary>
    /// 设备编码（冗余，便于查询）
    /// </summary>
    [Required(ErrorMessage = "设备编码（冗余，便于查询）不能为空")]
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
    /// 优先级（1=低，2=中，3=高，4=紧急）
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 通知单状态（0=新建，1=已转工单，2=已关闭，3=已取消）
    /// </summary>
    public int NotificationStatus { get; set; } = 0;

    /// <summary>
    /// 异常/故障描述
    /// </summary>
    [Required(ErrorMessage = "异常/故障描述不能为空")]
    public string FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 发现时间
    /// </summary>
    public DateTime DiscoveredAt { get; set; }

    /// <summary>
    /// 故障开始时间
    /// </summary>
    public DateTime? BreakdownStartTime { get; set; }

    /// <summary>
    /// 故障结束时间
    /// </summary>
    public DateTime? BreakdownEndTime { get; set; }

    /// <summary>
    /// 报告人（人员编码）
    /// </summary>
    public string? ReportedByEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 责任成本中心ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostCenterId { get; set; }

    /// <summary>
    /// 责任成本中心编码（冗余）
    /// </summary>
    public string? CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联维护工单ID（转工单后回填，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 关联维护工单号（冗余）
    /// </summary>
    public string? MaintenanceWorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 通知图片（JSON格式，存储图片URL列表）
    /// </summary>
    public string? NotificationImages { get; set; } = string.Empty;

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
// 更新MaintenanceNotification DTO
// ========================================

/// <summary>
/// 更新MaintenanceNotification DTO
/// 继承 TaktMaintenanceNotificationCreateDto，添加 MaintenanceNotificationId 字段
/// </summary>
public class TaktMaintenanceNotificationUpdateDto : TaktMaintenanceNotificationCreateDto
{
    /// <summary>
    /// MaintenanceNotificationID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceNotificationId { get; set; }

}

// ========================================
// MaintenanceNotification 状态 DTO
// ========================================

/// <summary>
/// MaintenanceNotification 状态更新 DTO
/// </summary>
public class TaktMaintenanceNotificationStatusDto
{
    /// <summary>
    /// MaintenanceNotificationID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceNotificationId { get; set; }

    /// <summary>
    /// 通知单状态（0=新建，1=已转工单，2=已关闭，3=已取消）
    /// </summary>
    [Required(ErrorMessage = "通知单状态（0=新建，1=已转工单，2=已关闭，3=已取消）不能为空")]
    public int NotificationStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// MaintenanceNotification 导入模板行 DTO
/// </summary>
public class TaktMaintenanceNotificationTemplateDto
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
    /// 通知单号
    /// </summary>
    public string? NotificationCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EquipmentId { get; set; }

    /// <summary>
    /// 设备编码（冗余，便于查询）
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
    /// 优先级（1=低，2=中，3=高，4=紧急）
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 通知单状态（0=新建，1=已转工单，2=已关闭，3=已取消）
    /// </summary>
    public int? NotificationStatus { get; set; }

    /// <summary>
    /// 异常/故障描述
    /// </summary>
    public string? FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 发现时间
    /// </summary>
    public DateTime? DiscoveredAt { get; set; }

    /// <summary>
    /// 故障开始时间
    /// </summary>
    public DateTime? BreakdownStartTime { get; set; }

    /// <summary>
    /// 故障结束时间
    /// </summary>
    public DateTime? BreakdownEndTime { get; set; }

    /// <summary>
    /// 报告人（人员编码）
    /// </summary>
    public string? ReportedByEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 责任成本中心ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostCenterId { get; set; }

    /// <summary>
    /// 责任成本中心编码（冗余）
    /// </summary>
    public string? CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联维护工单ID（转工单后回填，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 关联维护工单号（冗余）
    /// </summary>
    public string? MaintenanceWorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 通知图片（JSON格式，存储图片URL列表）
    /// </summary>
    public string? NotificationImages { get; set; } = string.Empty;

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
/// MaintenanceNotification 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMaintenanceNotificationImportDto
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
    /// 通知单号
    /// </summary>
    public string? NotificationCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EquipmentId { get; set; }

    /// <summary>
    /// 设备编码（冗余，便于查询）
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
    /// 优先级（1=低，2=中，3=高，4=紧急）
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 通知单状态（0=新建，1=已转工单，2=已关闭，3=已取消）
    /// </summary>
    public int? NotificationStatus { get; set; }

    /// <summary>
    /// 异常/故障描述
    /// </summary>
    public string? FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 发现时间
    /// </summary>
    public DateTime? DiscoveredAt { get; set; }

    /// <summary>
    /// 故障开始时间
    /// </summary>
    public DateTime? BreakdownStartTime { get; set; }

    /// <summary>
    /// 故障结束时间
    /// </summary>
    public DateTime? BreakdownEndTime { get; set; }

    /// <summary>
    /// 报告人（人员编码）
    /// </summary>
    public string? ReportedByEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 责任成本中心ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostCenterId { get; set; }

    /// <summary>
    /// 责任成本中心编码（冗余）
    /// </summary>
    public string? CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联维护工单ID（转工单后回填，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 关联维护工单号（冗余）
    /// </summary>
    public string? MaintenanceWorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 通知图片（JSON格式，存储图片URL列表）
    /// </summary>
    public string? NotificationImages { get; set; } = string.Empty;

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
/// MaintenanceNotification 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMaintenanceNotificationExportDto
{
    /// <summary>
    /// MaintenanceNotificationID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceNotificationId { get; set; }

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
    /// 通知单号
    /// </summary>
    public string NotificationCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentId { get; set; }

    /// <summary>
    /// 设备编码（冗余，便于查询）
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
    /// 优先级（1=低，2=中，3=高，4=紧急）
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 通知单状态（0=新建，1=已转工单，2=已关闭，3=已取消）
    /// </summary>
    public int NotificationStatus { get; set; } = 0;

    /// <summary>
    /// 异常/故障描述
    /// </summary>
    public string FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 发现时间
    /// </summary>
    public DateTime DiscoveredAt { get; set; }

    /// <summary>
    /// 故障开始时间
    /// </summary>
    public DateTime? BreakdownStartTime { get; set; }

    /// <summary>
    /// 故障结束时间
    /// </summary>
    public DateTime? BreakdownEndTime { get; set; }

    /// <summary>
    /// 报告人（人员编码）
    /// </summary>
    public string? ReportedByEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 责任成本中心ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostCenterId { get; set; }

    /// <summary>
    /// 责任成本中心编码（冗余）
    /// </summary>
    public string? CostCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联维护工单ID（转工单后回填，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 关联维护工单号（冗余）
    /// </summary>
    public string? MaintenanceWorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 通知图片（JSON格式，存储图片URL列表）
    /// </summary>
    public string? NotificationImages { get; set; } = string.Empty;

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

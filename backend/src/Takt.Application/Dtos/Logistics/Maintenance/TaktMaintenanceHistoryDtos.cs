// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Maintenance
// 文件名称：TaktMaintenanceHistoryDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：MaintenanceHistory 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMaintenanceHistory 生成，请按需审阅）
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
// MaintenanceHistory 响应 DTO
// ========================================

/// <summary>
/// 设备维护履历实体（TaktEquipment 子表；数据来源于 TaktMaintenanceWorkOrder 完工归档，只读展示）
/// 对应前端 TaktMaintenanceHistoryDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktMaintenanceHistoryDto : TaktCompanyDtoBase
{
    /// <summary>
    /// MaintenanceHistoryID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceHistoryId { get; set; }

    /// <summary>
    /// 来源维护工单ID（一工单一条履历，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 来源维护工单名称（填充字段）
    /// </summary>
    public string? MaintenanceWorkOrderName { get; set; }

    /// <summary>
    /// 来源维护工单号（冗余）
    /// </summary>
    public string WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentId { get; set; }

    /// <summary>
    /// 设备名称（填充字段）
    /// </summary>
    public string? EquipmentName { get; set; }

    /// <summary>
    /// 设备编码（冗余字段,便于查询）
    /// </summary>
    public string EquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
    /// </summary>
    public int MaintenanceType { get; set; } = 0;

    /// <summary>
    /// 维护类别（字典 logistics_maintenance_category）
    /// </summary>
    public int MaintenanceCategory { get; set; } = 0;

    /// <summary>
    /// 维护单位
    /// </summary>
    public string? MaintenanceCompany { get; set; } = string.Empty;

    /// <summary>
    /// 维护技师（人员编码）
    /// </summary>
    public string? MaintenanceTechnician { get; set; } = string.Empty;

    /// <summary>
    /// 维护日期（归档基准日，通常取工单完工时间）
    /// </summary>
    public DateTime MaintenanceDate { get; set; }

    /// <summary>
    /// 维护开始时间
    /// </summary>
    public DateTime? MaintenanceStartTime { get; set; }

    /// <summary>
    /// 维护结束时间
    /// </summary>
    public DateTime? MaintenanceEndTime { get; set; }

    /// <summary>
    /// 维护内容描述
    /// </summary>
    public string? MaintenanceContent { get; set; } = string.Empty;

    /// <summary>
    /// 故障描述
    /// </summary>
    public string? FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 处理方案
    /// </summary>
    public string? Solution { get; set; } = string.Empty;

    /// <summary>
    /// 使用配件（JSON，由工单领料明细汇总）
    /// </summary>
    public string? UsedParts { get; set; } = string.Empty;

    /// <summary>
    /// 维护费用（工单总成本快照）
    /// </summary>
    public decimal MaintenanceCost { get; set; }

    /// <summary>
    /// 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
    /// </summary>
    public int MaintenanceResult { get; set; } = 0;

    /// <summary>
    /// 履历状态（固定为 2=已完成，归档写入）
    /// </summary>
    public int MaintenanceStatus { get; set; } = 0;

    /// <summary>
    /// 下次维护日期
    /// </summary>
    public DateTime? NextMaintenanceDate { get; set; }

    /// <summary>
    /// 维护周期（天）
    /// </summary>
    public int MaintenanceCycleDays { get; set; } = 0;

    /// <summary>
    /// 维护文档（JSON格式，存储维护文档ID列表）
    /// </summary>
    public string? MaintenanceDocuments { get; set; } = string.Empty;

    /// <summary>
    /// 维护图片（JSON格式，存储维护图片URL列表）
    /// </summary>
    public string? MaintenanceImages { get; set; } = string.Empty;

    /// <summary>
    /// 验收总结
    /// </summary>
    public string? AcceptedSummary { get; set; } = string.Empty;

    /// <summary>
    /// 验收人（人员编码）
    /// </summary>
    public string? AcceptedByEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 验收时间
    /// </summary>
    public DateTime? AcceptedAt { get; set; }

    /// <summary>
    /// 归档时间
    /// </summary>
    public DateTime ArchivedAt { get; set; }

    /// <summary>
    /// 设备（主表）
    /// （主表：TaktEquipment）
    /// </summary>
    public TaktEquipmentDto? Equipment { get; set; }

    /// <summary>
    /// 来源维护工单
    /// （主表：TaktMaintenanceWorkOrder）
    /// </summary>
    public TaktMaintenanceWorkOrderDto? MaintenanceWorkOrder { get; set; }

}

// ========================================
// MaintenanceHistory 查询 DTO
// ========================================

/// <summary>
/// MaintenanceHistory 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMaintenanceHistoryQueryDto : TaktPagedQuery
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
    /// 来源维护工单ID（一工单一条履历，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 来源维护工单号（冗余）
    /// </summary>
    public string? WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EquipmentId { get; set; }

    /// <summary>
    /// 设备编码（冗余字段,便于查询）
    /// </summary>
    public string? EquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
    /// </summary>
    public int? MaintenanceType { get; set; }

    /// <summary>
    /// 维护类别（字典 logistics_maintenance_category）
    /// </summary>
    public int? MaintenanceCategory { get; set; }

    /// <summary>
    /// 维护单位
    /// </summary>
    public string? MaintenanceCompany { get; set; } = string.Empty;

    /// <summary>
    /// 维护技师（人员编码）
    /// </summary>
    public string? MaintenanceTechnician { get; set; } = string.Empty;

    /// <summary>
    /// 维护日期（归档基准日，通常取工单完工时间）（范围查询-开始）
    /// </summary>
    public DateTime? MaintenanceDateStart { get; set; }

    /// <summary>
    /// 维护日期（归档基准日，通常取工单完工时间）（范围查询-结束）
    /// </summary>
    public DateTime? MaintenanceDateEnd { get; set; }

    /// <summary>
    /// 维护开始时间（范围查询-开始）
    /// </summary>
    public DateTime? MaintenanceStartTimeStart { get; set; }

    /// <summary>
    /// 维护开始时间（范围查询-结束）
    /// </summary>
    public DateTime? MaintenanceStartTimeEnd { get; set; }

    /// <summary>
    /// 维护结束时间（范围查询-开始）
    /// </summary>
    public DateTime? MaintenanceEndTimeStart { get; set; }

    /// <summary>
    /// 维护结束时间（范围查询-结束）
    /// </summary>
    public DateTime? MaintenanceEndTimeEnd { get; set; }

    /// <summary>
    /// 维护内容描述
    /// </summary>
    public string? MaintenanceContent { get; set; } = string.Empty;

    /// <summary>
    /// 故障描述
    /// </summary>
    public string? FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 处理方案
    /// </summary>
    public string? Solution { get; set; } = string.Empty;

    /// <summary>
    /// 使用配件（JSON，由工单领料明细汇总）
    /// </summary>
    public string? UsedParts { get; set; } = string.Empty;

    /// <summary>
    /// 维护费用（工单总成本快照）
    /// </summary>
    public decimal? MaintenanceCost { get; set; }

    /// <summary>
    /// 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
    /// </summary>
    public int? MaintenanceResult { get; set; }

    /// <summary>
    /// 履历状态（固定为 2=已完成，归档写入）
    /// </summary>
    public int? MaintenanceStatus { get; set; }

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
    /// 维护文档（JSON格式，存储维护文档ID列表）
    /// </summary>
    public string? MaintenanceDocuments { get; set; } = string.Empty;

    /// <summary>
    /// 维护图片（JSON格式，存储维护图片URL列表）
    /// </summary>
    public string? MaintenanceImages { get; set; } = string.Empty;

    /// <summary>
    /// 验收总结
    /// </summary>
    public string? AcceptedSummary { get; set; } = string.Empty;

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
    /// 归档时间（范围查询-开始）
    /// </summary>
    public DateTime? ArchivedAtStart { get; set; }

    /// <summary>
    /// 归档时间（范围查询-结束）
    /// </summary>
    public DateTime? ArchivedAtEnd { get; set; }

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
// 创建MaintenanceHistory DTO
// ========================================

/// <summary>
/// 创建MaintenanceHistory DTO
/// </summary>
public class TaktMaintenanceHistoryCreateDto
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
    /// 来源维护工单ID（一工单一条履历，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 来源维护工单号（冗余）
    /// </summary>
    [Required(ErrorMessage = "来源维护工单号（冗余）不能为空")]
    public string WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentId { get; set; }

    /// <summary>
    /// 设备编码（冗余字段,便于查询）
    /// </summary>
    [Required(ErrorMessage = "设备编码（冗余字段,便于查询）不能为空")]
    public string EquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
    /// </summary>
    public int MaintenanceType { get; set; } = 0;

    /// <summary>
    /// 维护类别（字典 logistics_maintenance_category）
    /// </summary>
    public int MaintenanceCategory { get; set; } = 0;

    /// <summary>
    /// 维护单位
    /// </summary>
    public string? MaintenanceCompany { get; set; } = string.Empty;

    /// <summary>
    /// 维护技师（人员编码）
    /// </summary>
    public string? MaintenanceTechnician { get; set; } = string.Empty;

    /// <summary>
    /// 维护日期（归档基准日，通常取工单完工时间）
    /// </summary>
    public DateTime MaintenanceDate { get; set; }

    /// <summary>
    /// 维护开始时间
    /// </summary>
    public DateTime? MaintenanceStartTime { get; set; }

    /// <summary>
    /// 维护结束时间
    /// </summary>
    public DateTime? MaintenanceEndTime { get; set; }

    /// <summary>
    /// 维护内容描述
    /// </summary>
    public string? MaintenanceContent { get; set; } = string.Empty;

    /// <summary>
    /// 故障描述
    /// </summary>
    public string? FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 处理方案
    /// </summary>
    public string? Solution { get; set; } = string.Empty;

    /// <summary>
    /// 使用配件（JSON，由工单领料明细汇总）
    /// </summary>
    public string? UsedParts { get; set; } = string.Empty;

    /// <summary>
    /// 维护费用（工单总成本快照）
    /// </summary>
    public decimal MaintenanceCost { get; set; }

    /// <summary>
    /// 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
    /// </summary>
    public int MaintenanceResult { get; set; } = 0;

    /// <summary>
    /// 履历状态（固定为 2=已完成，归档写入）
    /// </summary>
    public int MaintenanceStatus { get; set; } = 0;

    /// <summary>
    /// 下次维护日期
    /// </summary>
    public DateTime? NextMaintenanceDate { get; set; }

    /// <summary>
    /// 维护周期（天）
    /// </summary>
    public int MaintenanceCycleDays { get; set; } = 0;

    /// <summary>
    /// 维护文档（JSON格式，存储维护文档ID列表）
    /// </summary>
    public string? MaintenanceDocuments { get; set; } = string.Empty;

    /// <summary>
    /// 维护图片（JSON格式，存储维护图片URL列表）
    /// </summary>
    public string? MaintenanceImages { get; set; } = string.Empty;

    /// <summary>
    /// 验收总结
    /// </summary>
    public string? AcceptedSummary { get; set; } = string.Empty;

    /// <summary>
    /// 验收人（人员编码）
    /// </summary>
    public string? AcceptedByEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 验收时间
    /// </summary>
    public DateTime? AcceptedAt { get; set; }

    /// <summary>
    /// 归档时间
    /// </summary>
    public DateTime ArchivedAt { get; set; }

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
// 更新MaintenanceHistory DTO
// ========================================

/// <summary>
/// 更新MaintenanceHistory DTO
/// 继承 TaktMaintenanceHistoryCreateDto，添加 MaintenanceHistoryId 字段
/// </summary>
public class TaktMaintenanceHistoryUpdateDto : TaktMaintenanceHistoryCreateDto
{
    /// <summary>
    /// MaintenanceHistoryID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceHistoryId { get; set; }

}

// ========================================
// MaintenanceHistory 状态 DTO
// ========================================

/// <summary>
/// MaintenanceHistory 状态更新 DTO
/// </summary>
public class TaktMaintenanceHistoryStatusDto
{
    /// <summary>
    /// MaintenanceHistoryID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceHistoryId { get; set; }

    /// <summary>
    /// 履历状态（固定为 2=已完成，归档写入）
    /// </summary>
    [Required(ErrorMessage = "履历状态（固定为 2=已完成，归档写入）不能为空")]
    public int MaintenanceStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// MaintenanceHistory 导入模板行 DTO
/// </summary>
public class TaktMaintenanceHistoryTemplateDto
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
    /// 来源维护工单ID（一工单一条履历，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 来源维护工单号（冗余）
    /// </summary>
    public string? WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EquipmentId { get; set; }

    /// <summary>
    /// 设备编码（冗余字段,便于查询）
    /// </summary>
    public string? EquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
    /// </summary>
    public int? MaintenanceType { get; set; }

    /// <summary>
    /// 维护类别（字典 logistics_maintenance_category）
    /// </summary>
    public int? MaintenanceCategory { get; set; }

    /// <summary>
    /// 维护单位
    /// </summary>
    public string? MaintenanceCompany { get; set; } = string.Empty;

    /// <summary>
    /// 维护技师（人员编码）
    /// </summary>
    public string? MaintenanceTechnician { get; set; } = string.Empty;

    /// <summary>
    /// 维护日期（归档基准日，通常取工单完工时间）
    /// </summary>
    public DateTime? MaintenanceDate { get; set; }

    /// <summary>
    /// 维护开始时间
    /// </summary>
    public DateTime? MaintenanceStartTime { get; set; }

    /// <summary>
    /// 维护结束时间
    /// </summary>
    public DateTime? MaintenanceEndTime { get; set; }

    /// <summary>
    /// 维护内容描述
    /// </summary>
    public string? MaintenanceContent { get; set; } = string.Empty;

    /// <summary>
    /// 故障描述
    /// </summary>
    public string? FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 处理方案
    /// </summary>
    public string? Solution { get; set; } = string.Empty;

    /// <summary>
    /// 使用配件（JSON，由工单领料明细汇总）
    /// </summary>
    public string? UsedParts { get; set; } = string.Empty;

    /// <summary>
    /// 维护费用（工单总成本快照）
    /// </summary>
    public decimal? MaintenanceCost { get; set; }

    /// <summary>
    /// 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
    /// </summary>
    public int? MaintenanceResult { get; set; }

    /// <summary>
    /// 履历状态（固定为 2=已完成，归档写入）
    /// </summary>
    public int? MaintenanceStatus { get; set; }

    /// <summary>
    /// 下次维护日期
    /// </summary>
    public DateTime? NextMaintenanceDate { get; set; }

    /// <summary>
    /// 维护周期（天）
    /// </summary>
    public int? MaintenanceCycleDays { get; set; }

    /// <summary>
    /// 维护文档（JSON格式，存储维护文档ID列表）
    /// </summary>
    public string? MaintenanceDocuments { get; set; } = string.Empty;

    /// <summary>
    /// 维护图片（JSON格式，存储维护图片URL列表）
    /// </summary>
    public string? MaintenanceImages { get; set; } = string.Empty;

    /// <summary>
    /// 验收总结
    /// </summary>
    public string? AcceptedSummary { get; set; } = string.Empty;

    /// <summary>
    /// 验收人（人员编码）
    /// </summary>
    public string? AcceptedByEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 验收时间
    /// </summary>
    public DateTime? AcceptedAt { get; set; }

    /// <summary>
    /// 归档时间
    /// </summary>
    public DateTime? ArchivedAt { get; set; }

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
/// MaintenanceHistory 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMaintenanceHistoryImportDto
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
    /// 来源维护工单ID（一工单一条履历，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 来源维护工单号（冗余）
    /// </summary>
    public string? WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EquipmentId { get; set; }

    /// <summary>
    /// 设备编码（冗余字段,便于查询）
    /// </summary>
    public string? EquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
    /// </summary>
    public int? MaintenanceType { get; set; }

    /// <summary>
    /// 维护类别（字典 logistics_maintenance_category）
    /// </summary>
    public int? MaintenanceCategory { get; set; }

    /// <summary>
    /// 维护单位
    /// </summary>
    public string? MaintenanceCompany { get; set; } = string.Empty;

    /// <summary>
    /// 维护技师（人员编码）
    /// </summary>
    public string? MaintenanceTechnician { get; set; } = string.Empty;

    /// <summary>
    /// 维护日期（归档基准日，通常取工单完工时间）
    /// </summary>
    public DateTime? MaintenanceDate { get; set; }

    /// <summary>
    /// 维护开始时间
    /// </summary>
    public DateTime? MaintenanceStartTime { get; set; }

    /// <summary>
    /// 维护结束时间
    /// </summary>
    public DateTime? MaintenanceEndTime { get; set; }

    /// <summary>
    /// 维护内容描述
    /// </summary>
    public string? MaintenanceContent { get; set; } = string.Empty;

    /// <summary>
    /// 故障描述
    /// </summary>
    public string? FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 处理方案
    /// </summary>
    public string? Solution { get; set; } = string.Empty;

    /// <summary>
    /// 使用配件（JSON，由工单领料明细汇总）
    /// </summary>
    public string? UsedParts { get; set; } = string.Empty;

    /// <summary>
    /// 维护费用（工单总成本快照）
    /// </summary>
    public decimal? MaintenanceCost { get; set; }

    /// <summary>
    /// 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
    /// </summary>
    public int? MaintenanceResult { get; set; }

    /// <summary>
    /// 履历状态（固定为 2=已完成，归档写入）
    /// </summary>
    public int? MaintenanceStatus { get; set; }

    /// <summary>
    /// 下次维护日期
    /// </summary>
    public DateTime? NextMaintenanceDate { get; set; }

    /// <summary>
    /// 维护周期（天）
    /// </summary>
    public int? MaintenanceCycleDays { get; set; }

    /// <summary>
    /// 维护文档（JSON格式，存储维护文档ID列表）
    /// </summary>
    public string? MaintenanceDocuments { get; set; } = string.Empty;

    /// <summary>
    /// 维护图片（JSON格式，存储维护图片URL列表）
    /// </summary>
    public string? MaintenanceImages { get; set; } = string.Empty;

    /// <summary>
    /// 验收总结
    /// </summary>
    public string? AcceptedSummary { get; set; } = string.Empty;

    /// <summary>
    /// 验收人（人员编码）
    /// </summary>
    public string? AcceptedByEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 验收时间
    /// </summary>
    public DateTime? AcceptedAt { get; set; }

    /// <summary>
    /// 归档时间
    /// </summary>
    public DateTime? ArchivedAt { get; set; }

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
/// MaintenanceHistory 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMaintenanceHistoryExportDto
{
    /// <summary>
    /// MaintenanceHistoryID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceHistoryId { get; set; }

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
    /// 来源维护工单ID（一工单一条履历，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 来源维护工单号（冗余）
    /// </summary>
    public string WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EquipmentId { get; set; }

    /// <summary>
    /// 设备编码（冗余字段,便于查询）
    /// </summary>
    public string EquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
    /// </summary>
    public int MaintenanceType { get; set; } = 0;

    /// <summary>
    /// 维护类别（字典 logistics_maintenance_category）
    /// </summary>
    public int MaintenanceCategory { get; set; } = 0;

    /// <summary>
    /// 维护单位
    /// </summary>
    public string? MaintenanceCompany { get; set; } = string.Empty;

    /// <summary>
    /// 维护技师（人员编码）
    /// </summary>
    public string? MaintenanceTechnician { get; set; } = string.Empty;

    /// <summary>
    /// 维护日期（归档基准日，通常取工单完工时间）
    /// </summary>
    public DateTime MaintenanceDate { get; set; }

    /// <summary>
    /// 维护开始时间
    /// </summary>
    public DateTime? MaintenanceStartTime { get; set; }

    /// <summary>
    /// 维护结束时间
    /// </summary>
    public DateTime? MaintenanceEndTime { get; set; }

    /// <summary>
    /// 维护内容描述
    /// </summary>
    public string? MaintenanceContent { get; set; } = string.Empty;

    /// <summary>
    /// 故障描述
    /// </summary>
    public string? FaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// 处理方案
    /// </summary>
    public string? Solution { get; set; } = string.Empty;

    /// <summary>
    /// 使用配件（JSON，由工单领料明细汇总）
    /// </summary>
    public string? UsedParts { get; set; } = string.Empty;

    /// <summary>
    /// 维护费用（工单总成本快照）
    /// </summary>
    public decimal MaintenanceCost { get; set; }

    /// <summary>
    /// 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
    /// </summary>
    public int MaintenanceResult { get; set; } = 0;

    /// <summary>
    /// 履历状态（固定为 2=已完成，归档写入）
    /// </summary>
    public int MaintenanceStatus { get; set; } = 0;

    /// <summary>
    /// 下次维护日期
    /// </summary>
    public DateTime? NextMaintenanceDate { get; set; }

    /// <summary>
    /// 维护周期（天）
    /// </summary>
    public int MaintenanceCycleDays { get; set; } = 0;

    /// <summary>
    /// 维护文档（JSON格式，存储维护文档ID列表）
    /// </summary>
    public string? MaintenanceDocuments { get; set; } = string.Empty;

    /// <summary>
    /// 维护图片（JSON格式，存储维护图片URL列表）
    /// </summary>
    public string? MaintenanceImages { get; set; } = string.Empty;

    /// <summary>
    /// 验收总结
    /// </summary>
    public string? AcceptedSummary { get; set; } = string.Empty;

    /// <summary>
    /// 验收人（人员编码）
    /// </summary>
    public string? AcceptedByEmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 验收时间
    /// </summary>
    public DateTime? AcceptedAt { get; set; }

    /// <summary>
    /// 归档时间
    /// </summary>
    public DateTime ArchivedAt { get; set; }

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

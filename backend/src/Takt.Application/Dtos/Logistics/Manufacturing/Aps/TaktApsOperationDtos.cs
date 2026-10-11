// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Aps
// 文件名称：TaktApsOperationDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：ApsOperation 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktApsOperation 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Aps;

// ========================================
// ApsOperation 响应 DTO
// ========================================

/// <summary>
/// APS 工序排程（APS_Order → Operation，关联 RoutingItem 与 WC/Resource）
/// 对应前端 TaktApsOperationDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktApsOperationDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ApsOperationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApsOperationId { get; set; }

    /// <summary>
    /// APS 订单 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApsOrderId { get; set; }

    /// <summary>
    /// APS 订单 名称（填充字段）
    /// </summary>
    public string? ApsOrderName { get; set; }

    /// <summary>
    /// APS 订单编码（冗余）
    /// </summary>
    public string ApsOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（工序序号）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 工艺路线工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemId { get; set; }

    /// <summary>
    /// 工艺路线工序 名称（填充字段）
    /// </summary>
    public string? RoutingItemName { get; set; }

    /// <summary>
    /// 工序编码
    /// </summary>
    public string ProcessCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序名称
    /// </summary>
    public string? ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心资源 ID（选项 TaktWorkCenterResources/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WorkCenterResourceId { get; set; }

    /// <summary>
    /// 工作中心资源 名称（填充字段）
    /// </summary>
    public string? WorkCenterResourceName { get; set; }

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 计划工时（分钟）
    /// </summary>
    public decimal PlannedDurationMinutes { get; set; }

    /// <summary>
    /// 换型时间（分钟）
    /// </summary>
    public decimal ChangeoverMinutes { get; set; }

    /// <summary>
    /// 工序状态（字典 aps_operation_status；0=待排程，1=已排程，2=执行中，3=已完成）
    /// </summary>
    public int OperationStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

}

// ========================================
// ApsOperation 查询 DTO
// ========================================

/// <summary>
/// ApsOperation 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktApsOperationQueryDto : TaktPagedQuery
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
    /// APS 订单 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOrderId { get; set; }

    /// <summary>
    /// APS 订单编码（冗余）
    /// </summary>
    public string? ApsOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（工序序号）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 工艺路线工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemId { get; set; }

    /// <summary>
    /// 工序编码
    /// </summary>
    public string? ProcessCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序名称
    /// </summary>
    public string? ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心资源 ID（选项 TaktWorkCenterResources/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WorkCenterResourceId { get; set; }

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
    /// 计划工时（分钟）
    /// </summary>
    public decimal? PlannedDurationMinutes { get; set; }

    /// <summary>
    /// 换型时间（分钟）
    /// </summary>
    public decimal? ChangeoverMinutes { get; set; }

    /// <summary>
    /// 工序状态（字典 aps_operation_status；0=待排程，1=已排程，2=执行中，3=已完成）
    /// </summary>
    public int? OperationStatus { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
// 创建ApsOperation DTO
// ========================================

/// <summary>
/// 创建ApsOperation DTO
/// </summary>
public class TaktApsOperationCreateDto
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
    /// APS 订单 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApsOrderId { get; set; }

    /// <summary>
    /// APS 订单编码（冗余）
    /// </summary>
    public string ApsOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（工序序号）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 工艺路线工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemId { get; set; }

    /// <summary>
    /// 工序编码
    /// </summary>
    [Required(ErrorMessage = "工序编码不能为空")]
    public string ProcessCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序名称
    /// </summary>
    public string? ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心资源 ID（选项 TaktWorkCenterResources/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WorkCenterResourceId { get; set; }

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 计划工时（分钟）
    /// </summary>
    public decimal PlannedDurationMinutes { get; set; }

    /// <summary>
    /// 换型时间（分钟）
    /// </summary>
    public decimal ChangeoverMinutes { get; set; }

    /// <summary>
    /// 工序状态（字典 aps_operation_status；0=待排程，1=已排程，2=执行中，3=已完成）
    /// </summary>
    public int OperationStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

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
// 更新ApsOperation DTO
// ========================================

/// <summary>
/// 更新ApsOperation DTO
/// 继承 TaktApsOperationCreateDto，添加 ApsOperationId 字段
/// </summary>
public class TaktApsOperationUpdateDto : TaktApsOperationCreateDto
{
    /// <summary>
    /// ApsOperationID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApsOperationId { get; set; }

}

// ========================================
// ApsOperation 状态 DTO
// ========================================

/// <summary>
/// ApsOperation 状态更新 DTO
/// </summary>
public class TaktApsOperationStatusDto
{
    /// <summary>
    /// ApsOperationID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApsOperationId { get; set; }

    /// <summary>
    /// 工序状态（字典 aps_operation_status；0=待排程，1=已排程，2=执行中，3=已完成）
    /// </summary>
    [Required(ErrorMessage = "工序状态（字典 aps_operation_status；0=待排程，1=已排程，2=执行中，3=已完成）不能为空")]
    public int OperationStatus { get; set; } = 0;
}

// ========================================
// ApsOperation 作废 DTO
// ========================================

/// <summary>
/// ApsOperation 作废/撤销作废 DTO
/// </summary>
public class TaktApsOperationObsoleteDto
{
    /// <summary>
    /// ApsOperationID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApsOperationId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ApsOperation 导入模板行 DTO
/// </summary>
public class TaktApsOperationTemplateDto
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
    /// APS 订单 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOrderId { get; set; }

    /// <summary>
    /// APS 订单编码（冗余）
    /// </summary>
    public string? ApsOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（工序序号）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 工艺路线工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemId { get; set; }

    /// <summary>
    /// 工序编码
    /// </summary>
    public string? ProcessCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序名称
    /// </summary>
    public string? ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心资源 ID（选项 TaktWorkCenterResources/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WorkCenterResourceId { get; set; }

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 计划工时（分钟）
    /// </summary>
    public decimal? PlannedDurationMinutes { get; set; }

    /// <summary>
    /// 换型时间（分钟）
    /// </summary>
    public decimal? ChangeoverMinutes { get; set; }

    /// <summary>
    /// 工序状态（字典 aps_operation_status；0=待排程，1=已排程，2=执行中，3=已完成）
    /// </summary>
    public int? OperationStatus { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
/// ApsOperation 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktApsOperationImportDto
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
    /// APS 订单 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOrderId { get; set; }

    /// <summary>
    /// APS 订单编码（冗余）
    /// </summary>
    public string? ApsOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（工序序号）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 工艺路线工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemId { get; set; }

    /// <summary>
    /// 工序编码
    /// </summary>
    public string? ProcessCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序名称
    /// </summary>
    public string? ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心资源 ID（选项 TaktWorkCenterResources/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WorkCenterResourceId { get; set; }

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 计划工时（分钟）
    /// </summary>
    public decimal? PlannedDurationMinutes { get; set; }

    /// <summary>
    /// 换型时间（分钟）
    /// </summary>
    public decimal? ChangeoverMinutes { get; set; }

    /// <summary>
    /// 工序状态（字典 aps_operation_status；0=待排程，1=已排程，2=执行中，3=已完成）
    /// </summary>
    public int? OperationStatus { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
/// ApsOperation 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktApsOperationExportDto
{
    /// <summary>
    /// ApsOperationID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApsOperationId { get; set; }

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
    /// APS 订单 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApsOrderId { get; set; }

    /// <summary>
    /// APS 订单编码（冗余）
    /// </summary>
    public string ApsOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（工序序号）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 工艺路线工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemId { get; set; }

    /// <summary>
    /// 工序编码
    /// </summary>
    public string ProcessCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序名称
    /// </summary>
    public string? ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心资源 ID（选项 TaktWorkCenterResources/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? WorkCenterResourceId { get; set; }

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 计划工时（分钟）
    /// </summary>
    public decimal PlannedDurationMinutes { get; set; }

    /// <summary>
    /// 换型时间（分钟）
    /// </summary>
    public decimal ChangeoverMinutes { get; set; }

    /// <summary>
    /// 工序状态（字典 aps_operation_status；0=待排程，1=已排程，2=执行中，3=已完成）
    /// </summary>
    public int OperationStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

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

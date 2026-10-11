// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Aps
// 文件名称：TaktApsOrderDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：ApsOrder 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktApsOrder 生成，请按需审阅）
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
// ApsOrder 响应 DTO
// ========================================

/// <summary>
/// APS 排程订单（Planned Order 释放后进入 APS 排程）
/// 对应前端 TaktApsOrderDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktApsOrderDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ApsOrderID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApsOrderId { get; set; }

    /// <summary>
    /// APS 订单编码
    /// </summary>
    public string ApsOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源计划订单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PlannedOrderId { get; set; }

    /// <summary>
    /// 来源计划订单 名称（填充字段）
    /// </summary>
    public string? PlannedOrderName { get; set; }

    /// <summary>
    /// 来源计划订单编码（冗余）
    /// </summary>
    public string? PlannedOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 订单数量
    /// </summary>
    public decimal OrderQuantity { get; set; }

    /// <summary>
    /// 计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线编码（选项 TaktRoutings/options；DictValue=RoutingCode）
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// APS 订单状态（字典 aps_order_status；0=待排程，1=已排程，2=已释放，3=已完成）
    /// </summary>
    public int OrderStatus { get; set; } = 0;

    /// <summary>
    /// 关联 APS 排程批次 ID（可选）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsScheduleId { get; set; }

    /// <summary>
    /// 关联 APS 排程批次 名称（填充字段）
    /// </summary>
    public string? ApsScheduleName { get; set; }

    /// <summary>
    /// APS 工序排程列表
    /// （子表：TaktApsOperation）
    /// </summary>
    public List<TaktApsOperationDto>? Operations { get; set; }

}

// ========================================
// ApsOrder 查询 DTO
// ========================================

/// <summary>
/// ApsOrder 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktApsOrderQueryDto : TaktPagedQuery
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
    /// APS 订单编码
    /// </summary>
    public string? ApsOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源计划订单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PlannedOrderId { get; set; }

    /// <summary>
    /// 来源计划订单编码（冗余）
    /// </summary>
    public string? PlannedOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 订单数量
    /// </summary>
    public decimal? OrderQuantity { get; set; }

    /// <summary>
    /// 计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线编码（选项 TaktRoutings/options；DictValue=RoutingCode）
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

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
    /// APS 订单状态（字典 aps_order_status；0=待排程，1=已排程，2=已释放，3=已完成）
    /// </summary>
    public int? OrderStatus { get; set; }

    /// <summary>
    /// 关联 APS 排程批次 ID（可选）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsScheduleId { get; set; }

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
// 创建ApsOrder DTO
// ========================================

/// <summary>
/// 创建ApsOrder DTO
/// </summary>
public class TaktApsOrderCreateDto
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
    /// APS 订单编码
    /// </summary>
    [Required(ErrorMessage = "APS 订单编码不能为空")]
    public string ApsOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源计划订单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PlannedOrderId { get; set; }

    /// <summary>
    /// 来源计划订单编码（冗余）
    /// </summary>
    public string? PlannedOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 订单数量
    /// </summary>
    public decimal OrderQuantity { get; set; }

    /// <summary>
    /// 计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    [Required(ErrorMessage = "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）不能为空")]
    public string UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线编码（选项 TaktRoutings/options；DictValue=RoutingCode）
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// APS 订单状态（字典 aps_order_status；0=待排程，1=已排程，2=已释放，3=已完成）
    /// </summary>
    public int OrderStatus { get; set; } = 0;

    /// <summary>
    /// 关联 APS 排程批次 ID（可选）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsScheduleId { get; set; }

    /// <summary>
    /// APS 工序排程列表（子表，级联保存）
    /// </summary>
    public List<TaktApsOperationCreateDto>? Operations { get; set; }

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
// 更新ApsOrder DTO
// ========================================

/// <summary>
/// 更新ApsOrder DTO
/// 继承 TaktApsOrderCreateDto，添加 ApsOrderId 字段
/// </summary>
public class TaktApsOrderUpdateDto : TaktApsOrderCreateDto
{
    /// <summary>
    /// ApsOrderID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApsOrderId { get; set; }

    /// <summary>
    /// APS 工序排程列表（子表，级联保存）
    /// </summary>
    public new List<TaktApsOperationUpdateDto>? Operations { get; set; }

}

// ========================================
// ApsOrder 状态 DTO
// ========================================

/// <summary>
/// ApsOrder 状态更新 DTO
/// </summary>
public class TaktApsOrderStatusDto
{
    /// <summary>
    /// ApsOrderID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApsOrderId { get; set; }

    /// <summary>
    /// APS 订单状态（字典 aps_order_status；0=待排程，1=已排程，2=已释放，3=已完成）
    /// </summary>
    [Required(ErrorMessage = "APS 订单状态（字典 aps_order_status；0=待排程，1=已排程，2=已释放，3=已完成）不能为空")]
    public int OrderStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ApsOrder 导入模板行 DTO
/// </summary>
public class TaktApsOrderTemplateDto
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
    /// APS 订单编码
    /// </summary>
    public string? ApsOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源计划订单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PlannedOrderId { get; set; }

    /// <summary>
    /// 来源计划订单编码（冗余）
    /// </summary>
    public string? PlannedOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 订单数量
    /// </summary>
    public decimal? OrderQuantity { get; set; }

    /// <summary>
    /// 计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线编码（选项 TaktRoutings/options；DictValue=RoutingCode）
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// APS 订单状态（字典 aps_order_status；0=待排程，1=已排程，2=已释放，3=已完成）
    /// </summary>
    public int? OrderStatus { get; set; }

    /// <summary>
    /// 关联 APS 排程批次 ID（可选）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsScheduleId { get; set; }

    /// <summary>
    /// APS 工序排程列表（子表，级联保存）
    /// </summary>
    public List<TaktApsOperationCreateDto>? Operations { get; set; }

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
/// ApsOrder 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktApsOrderImportDto
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
    /// APS 订单编码
    /// </summary>
    public string? ApsOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源计划订单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PlannedOrderId { get; set; }

    /// <summary>
    /// 来源计划订单编码（冗余）
    /// </summary>
    public string? PlannedOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 订单数量
    /// </summary>
    public decimal? OrderQuantity { get; set; }

    /// <summary>
    /// 计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线编码（选项 TaktRoutings/options；DictValue=RoutingCode）
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// APS 订单状态（字典 aps_order_status；0=待排程，1=已排程，2=已释放，3=已完成）
    /// </summary>
    public int? OrderStatus { get; set; }

    /// <summary>
    /// 关联 APS 排程批次 ID（可选）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsScheduleId { get; set; }

    /// <summary>
    /// APS 工序排程列表（子表，级联保存）
    /// </summary>
    public List<TaktApsOperationCreateDto>? Operations { get; set; }

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
/// ApsOrder 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktApsOrderExportDto
{
    /// <summary>
    /// ApsOrderID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApsOrderId { get; set; }

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
    /// APS 订单编码
    /// </summary>
    public string ApsOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源计划订单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PlannedOrderId { get; set; }

    /// <summary>
    /// 来源计划订单编码（冗余）
    /// </summary>
    public string? PlannedOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 订单数量
    /// </summary>
    public decimal OrderQuantity { get; set; }

    /// <summary>
    /// 计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线编码（选项 TaktRoutings/options；DictValue=RoutingCode）
    /// </summary>
    public string? RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划开始时间
    /// </summary>
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// APS 订单状态（字典 aps_order_status；0=待排程，1=已排程，2=已释放，3=已完成）
    /// </summary>
    public int OrderStatus { get; set; } = 0;

    /// <summary>
    /// 关联 APS 排程批次 ID（可选）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsScheduleId { get; set; }

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

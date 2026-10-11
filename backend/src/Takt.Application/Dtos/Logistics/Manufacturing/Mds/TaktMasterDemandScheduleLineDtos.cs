// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Mds
// 文件名称：TaktMasterDemandScheduleLineDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：MasterDemandScheduleLine 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMasterDemandScheduleLine 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Mds;

// ========================================
// MasterDemandScheduleLine 响应 DTO
// ========================================

/// <summary>
/// 主需求计划 MDS 行（物料 + 时间桶 + 需求来源）
/// 对应前端 TaktMasterDemandScheduleLineDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktMasterDemandScheduleLineDto : TaktCompanyDtoBase
{
    /// <summary>
    /// MasterDemandScheduleLineID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MasterDemandScheduleLineId { get; set; }

    /// <summary>
    /// MDS 头表 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MasterDemandScheduleId { get; set; }

    /// <summary>
    /// MDS 头表 名称（填充字段）
    /// </summary>
    public string? MasterDemandScheduleName { get; set; }

    /// <summary>
    /// MDS 编码（冗余）
    /// </summary>
    public string MdsCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 需求来源（字典 mds_demand_source；0=销售订单，1=预测，2=手工）
    /// </summary>
    public int DemandSourceType { get; set; } = 0;

    /// <summary>
    /// 来源销售订单 ID（可选）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesOrderId { get; set; }

    /// <summary>
    /// 来源销售订单 名称（填充字段）
    /// </summary>
    public string? SalesOrderName { get; set; }

    /// <summary>
    /// 来源销售订单行号（可选；与 SalesOrderId 成对）
    /// </summary>
    public int? SalesOrderLineNumber { get; set; }

    /// <summary>
    /// 来源销售预测 ID（可选；预测/计划类需求）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesForecastId { get; set; }

    /// <summary>
    /// 来源销售预测 名称（填充字段）
    /// </summary>
    public string? SalesForecastName { get; set; }

    /// <summary>
    /// 来源销售预测行号（可选；与 SalesForecastId 成对）
    /// </summary>
    public int? SalesForecastLineNumber { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 时间桶开始
    /// </summary>
    public DateTime BucketStart { get; set; }

    /// <summary>
    /// 时间桶结束
    /// </summary>
    public DateTime BucketEnd { get; set; }

    /// <summary>
    /// 需求数量（基本单位）
    /// </summary>
    public decimal DemandQuantity { get; set; }

    /// <summary>
    /// 计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

}

// ========================================
// MasterDemandScheduleLine 查询 DTO
// ========================================

/// <summary>
/// MasterDemandScheduleLine 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMasterDemandScheduleLineQueryDto : TaktPagedQuery
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
    /// MDS 头表 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterDemandScheduleId { get; set; }

    /// <summary>
    /// MDS 编码（冗余）
    /// </summary>
    public string? MdsCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 需求来源（字典 mds_demand_source；0=销售订单，1=预测，2=手工）
    /// </summary>
    public int? DemandSourceType { get; set; }

    /// <summary>
    /// 来源销售订单 ID（可选）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesOrderId { get; set; }

    /// <summary>
    /// 来源销售订单行号（可选；与 SalesOrderId 成对）
    /// </summary>
    public int? SalesOrderLineNumber { get; set; }

    /// <summary>
    /// 来源销售预测 ID（可选；预测/计划类需求）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesForecastId { get; set; }

    /// <summary>
    /// 来源销售预测行号（可选；与 SalesForecastId 成对）
    /// </summary>
    public int? SalesForecastLineNumber { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 时间桶开始（范围查询-开始）
    /// </summary>
    public DateTime? BucketStartStart { get; set; }

    /// <summary>
    /// 时间桶开始（范围查询-结束）
    /// </summary>
    public DateTime? BucketStartEnd { get; set; }

    /// <summary>
    /// 时间桶结束（范围查询-开始）
    /// </summary>
    public DateTime? BucketEndStart { get; set; }

    /// <summary>
    /// 时间桶结束（范围查询-结束）
    /// </summary>
    public DateTime? BucketEndEnd { get; set; }

    /// <summary>
    /// 需求数量（基本单位）
    /// </summary>
    public decimal? DemandQuantity { get; set; }

    /// <summary>
    /// 计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? UnitOfMeasure { get; set; } = string.Empty;

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
// 创建MasterDemandScheduleLine DTO
// ========================================

/// <summary>
/// 创建MasterDemandScheduleLine DTO
/// </summary>
public class TaktMasterDemandScheduleLineCreateDto
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
    /// MDS 头表 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MasterDemandScheduleId { get; set; }

    /// <summary>
    /// MDS 编码（冗余）
    /// </summary>
    [Required(ErrorMessage = "MDS 编码（冗余）不能为空")]
    public string MdsCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 需求来源（字典 mds_demand_source；0=销售订单，1=预测，2=手工）
    /// </summary>
    public int DemandSourceType { get; set; } = 0;

    /// <summary>
    /// 来源销售订单 ID（可选）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesOrderId { get; set; }

    /// <summary>
    /// 来源销售订单行号（可选；与 SalesOrderId 成对）
    /// </summary>
    public int? SalesOrderLineNumber { get; set; }

    /// <summary>
    /// 来源销售预测 ID（可选；预测/计划类需求）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesForecastId { get; set; }

    /// <summary>
    /// 来源销售预测行号（可选；与 SalesForecastId 成对）
    /// </summary>
    public int? SalesForecastLineNumber { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 时间桶开始
    /// </summary>
    public DateTime BucketStart { get; set; }

    /// <summary>
    /// 时间桶结束
    /// </summary>
    public DateTime BucketEnd { get; set; }

    /// <summary>
    /// 需求数量（基本单位）
    /// </summary>
    public decimal DemandQuantity { get; set; }

    /// <summary>
    /// 计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    [Required(ErrorMessage = "计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）不能为空")]
    public string UnitOfMeasure { get; set; } = string.Empty;

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
// 更新MasterDemandScheduleLine DTO
// ========================================

/// <summary>
/// 更新MasterDemandScheduleLine DTO
/// 继承 TaktMasterDemandScheduleLineCreateDto，添加 MasterDemandScheduleLineId 字段
/// </summary>
public class TaktMasterDemandScheduleLineUpdateDto : TaktMasterDemandScheduleLineCreateDto
{
    /// <summary>
    /// MasterDemandScheduleLineID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MasterDemandScheduleLineId { get; set; }

}

// ========================================
// MasterDemandScheduleLine 作废 DTO
// ========================================

/// <summary>
/// MasterDemandScheduleLine 作废/撤销作废 DTO
/// </summary>
public class TaktMasterDemandScheduleLineObsoleteDto
{
    /// <summary>
    /// MasterDemandScheduleLineID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MasterDemandScheduleLineId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// MasterDemandScheduleLine 导入模板行 DTO
/// </summary>
public class TaktMasterDemandScheduleLineTemplateDto
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
    /// MDS 头表 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterDemandScheduleId { get; set; }

    /// <summary>
    /// MDS 编码（冗余）
    /// </summary>
    public string? MdsCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 需求来源（字典 mds_demand_source；0=销售订单，1=预测，2=手工）
    /// </summary>
    public int? DemandSourceType { get; set; }

    /// <summary>
    /// 来源销售订单 ID（可选）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesOrderId { get; set; }

    /// <summary>
    /// 来源销售订单行号（可选；与 SalesOrderId 成对）
    /// </summary>
    public int? SalesOrderLineNumber { get; set; }

    /// <summary>
    /// 来源销售预测 ID（可选；预测/计划类需求）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesForecastId { get; set; }

    /// <summary>
    /// 来源销售预测行号（可选；与 SalesForecastId 成对）
    /// </summary>
    public int? SalesForecastLineNumber { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 时间桶开始
    /// </summary>
    public DateTime? BucketStart { get; set; }

    /// <summary>
    /// 时间桶结束
    /// </summary>
    public DateTime? BucketEnd { get; set; }

    /// <summary>
    /// 需求数量（基本单位）
    /// </summary>
    public decimal? DemandQuantity { get; set; }

    /// <summary>
    /// 计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? UnitOfMeasure { get; set; } = string.Empty;

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
/// MasterDemandScheduleLine 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMasterDemandScheduleLineImportDto
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
    /// MDS 头表 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MasterDemandScheduleId { get; set; }

    /// <summary>
    /// MDS 编码（冗余）
    /// </summary>
    public string? MdsCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 需求来源（字典 mds_demand_source；0=销售订单，1=预测，2=手工）
    /// </summary>
    public int? DemandSourceType { get; set; }

    /// <summary>
    /// 来源销售订单 ID（可选）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesOrderId { get; set; }

    /// <summary>
    /// 来源销售订单行号（可选；与 SalesOrderId 成对）
    /// </summary>
    public int? SalesOrderLineNumber { get; set; }

    /// <summary>
    /// 来源销售预测 ID（可选；预测/计划类需求）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesForecastId { get; set; }

    /// <summary>
    /// 来源销售预测行号（可选；与 SalesForecastId 成对）
    /// </summary>
    public int? SalesForecastLineNumber { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 时间桶开始
    /// </summary>
    public DateTime? BucketStart { get; set; }

    /// <summary>
    /// 时间桶结束
    /// </summary>
    public DateTime? BucketEnd { get; set; }

    /// <summary>
    /// 需求数量（基本单位）
    /// </summary>
    public decimal? DemandQuantity { get; set; }

    /// <summary>
    /// 计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? UnitOfMeasure { get; set; } = string.Empty;

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
/// MasterDemandScheduleLine 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMasterDemandScheduleLineExportDto
{
    /// <summary>
    /// MasterDemandScheduleLineID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MasterDemandScheduleLineId { get; set; }

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
    /// MDS 头表 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MasterDemandScheduleId { get; set; }

    /// <summary>
    /// MDS 编码（冗余）
    /// </summary>
    public string MdsCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 需求来源（字典 mds_demand_source；0=销售订单，1=预测，2=手工）
    /// </summary>
    public int DemandSourceType { get; set; } = 0;

    /// <summary>
    /// 来源销售订单 ID（可选）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesOrderId { get; set; }

    /// <summary>
    /// 来源销售订单行号（可选；与 SalesOrderId 成对）
    /// </summary>
    public int? SalesOrderLineNumber { get; set; }

    /// <summary>
    /// 来源销售预测 ID（可选；预测/计划类需求）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesForecastId { get; set; }

    /// <summary>
    /// 来源销售预测行号（可选；与 SalesForecastId 成对）
    /// </summary>
    public int? SalesForecastLineNumber { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 时间桶开始
    /// </summary>
    public DateTime BucketStart { get; set; }

    /// <summary>
    /// 时间桶结束
    /// </summary>
    public DateTime BucketEnd { get; set; }

    /// <summary>
    /// 需求数量（基本单位）
    /// </summary>
    public decimal DemandQuantity { get; set; }

    /// <summary>
    /// 计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string UnitOfMeasure { get; set; } = string.Empty;

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

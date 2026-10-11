// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Mrp
// 文件名称：TaktProductionPlanItemDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：ProductionPlanItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktProductionPlanItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Mrp;

// ========================================
// ProductionPlanItem 响应 DTO
// ========================================

/// <summary>
/// Takt生产计划明细实体
/// 对应前端 TaktProductionPlanItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktProductionPlanItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ProductionPlanItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionPlanItemId { get; set; }

    /// <summary>
    /// 生产计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionPlanId { get; set; }

    /// <summary>
    /// 生产计划名称（填充字段）
    /// </summary>
    public string? ProductionPlanName { get; set; }

    /// <summary>
    /// 生产计划编码（冗余字段，便于查询）
    /// </summary>
    public string ProductionPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 来源销售计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesForecastId { get; set; }

    /// <summary>
    /// 来源销售计划名称（填充字段）
    /// </summary>
    public string? SalesForecastName { get; set; }

    /// <summary>
    /// 来源销售计划编码
    /// </summary>
    public string? SalesForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源销售计划行号
    /// </summary>
    public int? SalesForecastLineNumber { get; set; }

    /// <summary>
    /// 来源 MRP 明细 ID（MRP 需求追溯，关联 TaktMaterialRequirementsPlanningItem.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialRequirementsPlanningItemId { get; set; }

    /// <summary>
    /// 来源 MRP 明细 名称（填充字段）
    /// </summary>
    public string? MaterialRequirementsPlanningItemName { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格（回填：随物料）
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（关联 TaktModelDestination.ModelCode，与物料机种主数据对齐）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称（冗余字段，便于查询展示）
    /// </summary>
    public string? ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string PlanUnit { get; set; } = string.Empty;

    /// <summary>
    /// 计划数量（基本单位数量）
    /// </summary>
    public decimal PlanQuantity { get; set; }

    /// <summary>
    /// 计划开工日期
    /// </summary>
    public DateTime? PlannedStartDate { get; set; }

    /// <summary>
    /// 计划完工日期
    /// </summary>
    public DateTime? PlannedEndDate { get; set; }

    /// <summary>
    /// 已转工单/采购数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 预计单位成本
    /// </summary>
    public decimal EstimatedUnitCost { get; set; }

    /// <summary>
    /// 预计金额
    /// </summary>
    public decimal EstimatedAmount { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

}

// ========================================
// ProductionPlanItem 查询 DTO
// ========================================

/// <summary>
/// ProductionPlanItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktProductionPlanItemQueryDto : TaktPagedQuery
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
    /// 生产计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProductionPlanId { get; set; }

    /// <summary>
    /// 生产计划编码（冗余字段，便于查询）
    /// </summary>
    public string? ProductionPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 来源销售计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesForecastId { get; set; }

    /// <summary>
    /// 来源销售计划编码
    /// </summary>
    public string? SalesForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源销售计划行号
    /// </summary>
    public int? SalesForecastLineNumber { get; set; }

    /// <summary>
    /// 来源 MRP 明细 ID（MRP 需求追溯，关联 TaktMaterialRequirementsPlanningItem.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialRequirementsPlanningItemId { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格（回填：随物料）
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（关联 TaktModelDestination.ModelCode，与物料机种主数据对齐）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称（冗余字段，便于查询展示）
    /// </summary>
    public string? ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? PlanUnit { get; set; } = string.Empty;

    /// <summary>
    /// 计划数量（基本单位数量）
    /// </summary>
    public decimal? PlanQuantity { get; set; }

    /// <summary>
    /// 计划开工日期（范围查询-开始）
    /// </summary>
    public DateTime? PlannedStartDateStart { get; set; }

    /// <summary>
    /// 计划开工日期（范围查询-结束）
    /// </summary>
    public DateTime? PlannedStartDateEnd { get; set; }

    /// <summary>
    /// 计划完工日期（范围查询-开始）
    /// </summary>
    public DateTime? PlannedEndDateStart { get; set; }

    /// <summary>
    /// 计划完工日期（范围查询-结束）
    /// </summary>
    public DateTime? PlannedEndDateEnd { get; set; }

    /// <summary>
    /// 已转工单/采购数量（基本单位数量）
    /// </summary>
    public decimal? ConvertedQuantity { get; set; }

    /// <summary>
    /// 预计单位成本
    /// </summary>
    public decimal? EstimatedUnitCost { get; set; }

    /// <summary>
    /// 预计金额
    /// </summary>
    public decimal? EstimatedAmount { get; set; }

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
// 创建ProductionPlanItem DTO
// ========================================

/// <summary>
/// 创建ProductionPlanItem DTO
/// </summary>
public class TaktProductionPlanItemCreateDto
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
    /// 生产计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionPlanId { get; set; }

    /// <summary>
    /// 生产计划编码（冗余字段，便于查询）
    /// </summary>
    public string ProductionPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 来源销售计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesForecastId { get; set; }

    /// <summary>
    /// 来源销售计划编码
    /// </summary>
    public string? SalesForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源销售计划行号
    /// </summary>
    public int? SalesForecastLineNumber { get; set; }

    /// <summary>
    /// 来源 MRP 明细 ID（MRP 需求追溯，关联 TaktMaterialRequirementsPlanningItem.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialRequirementsPlanningItemId { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    [Required(ErrorMessage = "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）不能为空")]
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格（回填：随物料）
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（关联 TaktModelDestination.ModelCode，与物料机种主数据对齐）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称（冗余字段，便于查询展示）
    /// </summary>
    public string? ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    [Required(ErrorMessage = "计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）不能为空")]
    public string PlanUnit { get; set; } = string.Empty;

    /// <summary>
    /// 计划数量（基本单位数量）
    /// </summary>
    public decimal PlanQuantity { get; set; }

    /// <summary>
    /// 计划开工日期
    /// </summary>
    public DateTime? PlannedStartDate { get; set; }

    /// <summary>
    /// 计划完工日期
    /// </summary>
    public DateTime? PlannedEndDate { get; set; }

    /// <summary>
    /// 已转工单/采购数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 预计单位成本
    /// </summary>
    public decimal EstimatedUnitCost { get; set; }

    /// <summary>
    /// 预计金额
    /// </summary>
    public decimal EstimatedAmount { get; set; }

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
// 更新ProductionPlanItem DTO
// ========================================

/// <summary>
/// 更新ProductionPlanItem DTO
/// 继承 TaktProductionPlanItemCreateDto，添加 ProductionPlanItemId 字段
/// </summary>
public class TaktProductionPlanItemUpdateDto : TaktProductionPlanItemCreateDto
{
    /// <summary>
    /// ProductionPlanItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionPlanItemId { get; set; }

}

// ========================================
// ProductionPlanItem 作废 DTO
// ========================================

/// <summary>
/// ProductionPlanItem 作废/撤销作废 DTO
/// </summary>
public class TaktProductionPlanItemObsoleteDto
{
    /// <summary>
    /// ProductionPlanItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionPlanItemId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ProductionPlanItem 导入模板行 DTO
/// </summary>
public class TaktProductionPlanItemTemplateDto
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
    /// 生产计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProductionPlanId { get; set; }

    /// <summary>
    /// 生产计划编码（冗余字段，便于查询）
    /// </summary>
    public string? ProductionPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 来源销售计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesForecastId { get; set; }

    /// <summary>
    /// 来源销售计划编码
    /// </summary>
    public string? SalesForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源销售计划行号
    /// </summary>
    public int? SalesForecastLineNumber { get; set; }

    /// <summary>
    /// 来源 MRP 明细 ID（MRP 需求追溯，关联 TaktMaterialRequirementsPlanningItem.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialRequirementsPlanningItemId { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格（回填：随物料）
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（关联 TaktModelDestination.ModelCode，与物料机种主数据对齐）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称（冗余字段，便于查询展示）
    /// </summary>
    public string? ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? PlanUnit { get; set; } = string.Empty;

    /// <summary>
    /// 计划数量（基本单位数量）
    /// </summary>
    public decimal? PlanQuantity { get; set; }

    /// <summary>
    /// 计划开工日期
    /// </summary>
    public DateTime? PlannedStartDate { get; set; }

    /// <summary>
    /// 计划完工日期
    /// </summary>
    public DateTime? PlannedEndDate { get; set; }

    /// <summary>
    /// 已转工单/采购数量（基本单位数量）
    /// </summary>
    public decimal? ConvertedQuantity { get; set; }

    /// <summary>
    /// 预计单位成本
    /// </summary>
    public decimal? EstimatedUnitCost { get; set; }

    /// <summary>
    /// 预计金额
    /// </summary>
    public decimal? EstimatedAmount { get; set; }

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
/// ProductionPlanItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktProductionPlanItemImportDto
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
    /// 生产计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProductionPlanId { get; set; }

    /// <summary>
    /// 生产计划编码（冗余字段，便于查询）
    /// </summary>
    public string? ProductionPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 来源销售计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesForecastId { get; set; }

    /// <summary>
    /// 来源销售计划编码
    /// </summary>
    public string? SalesForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源销售计划行号
    /// </summary>
    public int? SalesForecastLineNumber { get; set; }

    /// <summary>
    /// 来源 MRP 明细 ID（MRP 需求追溯，关联 TaktMaterialRequirementsPlanningItem.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialRequirementsPlanningItemId { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格（回填：随物料）
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（关联 TaktModelDestination.ModelCode，与物料机种主数据对齐）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称（冗余字段，便于查询展示）
    /// </summary>
    public string? ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? PlanUnit { get; set; } = string.Empty;

    /// <summary>
    /// 计划数量（基本单位数量）
    /// </summary>
    public decimal? PlanQuantity { get; set; }

    /// <summary>
    /// 计划开工日期
    /// </summary>
    public DateTime? PlannedStartDate { get; set; }

    /// <summary>
    /// 计划完工日期
    /// </summary>
    public DateTime? PlannedEndDate { get; set; }

    /// <summary>
    /// 已转工单/采购数量（基本单位数量）
    /// </summary>
    public decimal? ConvertedQuantity { get; set; }

    /// <summary>
    /// 预计单位成本
    /// </summary>
    public decimal? EstimatedUnitCost { get; set; }

    /// <summary>
    /// 预计金额
    /// </summary>
    public decimal? EstimatedAmount { get; set; }

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
/// ProductionPlanItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktProductionPlanItemExportDto
{
    /// <summary>
    /// ProductionPlanItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionPlanItemId { get; set; }

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
    /// 生产计划ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionPlanId { get; set; }

    /// <summary>
    /// 生产计划编码（冗余字段，便于查询）
    /// </summary>
    public string ProductionPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 来源销售计划ID（MRP 需求追溯，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesForecastId { get; set; }

    /// <summary>
    /// 来源销售计划编码
    /// </summary>
    public string? SalesForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源销售计划行号
    /// </summary>
    public int? SalesForecastLineNumber { get; set; }

    /// <summary>
    /// 来源 MRP 明细 ID（MRP 需求追溯，关联 TaktMaterialRequirementsPlanningItem.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialRequirementsPlanningItemId { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格（回填：随物料）
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（关联 TaktModelDestination.ModelCode，与物料机种主数据对齐）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称（冗余字段，便于查询展示）
    /// </summary>
    public string? ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string PlanUnit { get; set; } = string.Empty;

    /// <summary>
    /// 计划数量（基本单位数量）
    /// </summary>
    public decimal PlanQuantity { get; set; }

    /// <summary>
    /// 计划开工日期
    /// </summary>
    public DateTime? PlannedStartDate { get; set; }

    /// <summary>
    /// 计划完工日期
    /// </summary>
    public DateTime? PlannedEndDate { get; set; }

    /// <summary>
    /// 已转工单/采购数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 预计单位成本
    /// </summary>
    public decimal EstimatedUnitCost { get; set; }

    /// <summary>
    /// 预计金额
    /// </summary>
    public decimal EstimatedAmount { get; set; }

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

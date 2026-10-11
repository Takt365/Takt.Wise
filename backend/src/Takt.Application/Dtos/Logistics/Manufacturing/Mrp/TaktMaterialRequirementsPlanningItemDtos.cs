// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Mrp
// 文件名称：TaktMaterialRequirementsPlanningItemDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：MaterialRequirementsPlanningItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMaterialRequirementsPlanningItem 生成，请按需审阅）
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
// MaterialRequirementsPlanningItem 响应 DTO
// ========================================

/// <summary>
/// 物料需求计划 MRP 明细行（物料 + 需求日期 + 净需求数量）
/// 对应前端 TaktMaterialRequirementsPlanningItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktMaterialRequirementsPlanningItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// MaterialRequirementsPlanningItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialRequirementsPlanningItemId { get; set; }

    /// <summary>
    /// MRP 头表 ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialRequirementsPlanningId { get; set; }

    /// <summary>
    /// MRP 头表 名称（填充字段）
    /// </summary>
    public string? MaterialRequirementsPlanningName { get; set; }

    /// <summary>
    /// MRP 编码（冗余字段，便于查询）
    /// </summary>
    public string MaterialRequirementsPlanningCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

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
    /// 机种编码（关联 TaktModelDestination.ModelCode，可选）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称（冗余）
    /// </summary>
    public string? ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 父项物料编码（BOM 展开上级，可选）
    /// </summary>
    public string? ParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// BOM 层级（1=顶层成品）
    /// </summary>
    public int BomLevel { get; set; } = 0;

    /// <summary>
    /// 需求日期
    /// </summary>
    public DateTime RequirementDate { get; set; }

    /// <summary>
    /// 计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string PlanUnit { get; set; } = string.Empty;

    /// <summary>
    /// 毛需求数量（基本单位数量）
    /// </summary>
    public decimal GrossRequirement { get; set; }

    /// <summary>
    /// 计划接收数量（在途/已订未收等，运算快照）
    /// </summary>
    public decimal ScheduledReceipts { get; set; }

    /// <summary>
    /// 现有库存数量（运算快照，来源 TaktMaterialPlant.CurrentStock）
    /// </summary>
    public decimal OnHandQuantity { get; set; }

    /// <summary>
    /// 预计可用库存（运算后 POH 快照）
    /// </summary>
    public decimal ProjectedOnHand { get; set; }

    /// <summary>
    /// 净需求数量（基本单位数量）
    /// </summary>
    public decimal NetRequirement { get; set; }

    /// <summary>
    /// 供应类型（字典 logistics_procurement_type；0=自制，1=外购，2=委外）
    /// </summary>
    public int ProcurementType { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

}

// ========================================
// MaterialRequirementsPlanningItem 查询 DTO
// ========================================

/// <summary>
/// MaterialRequirementsPlanningItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMaterialRequirementsPlanningItemQueryDto : TaktPagedQuery
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
    /// MRP 头表 ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialRequirementsPlanningId { get; set; }

    /// <summary>
    /// MRP 编码（冗余字段，便于查询）
    /// </summary>
    public string? MaterialRequirementsPlanningCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

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
    /// 机种编码（关联 TaktModelDestination.ModelCode，可选）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称（冗余）
    /// </summary>
    public string? ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 父项物料编码（BOM 展开上级，可选）
    /// </summary>
    public string? ParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// BOM 层级（1=顶层成品）
    /// </summary>
    public int? BomLevel { get; set; }

    /// <summary>
    /// 需求日期（范围查询-开始）
    /// </summary>
    public DateTime? RequirementDateStart { get; set; }

    /// <summary>
    /// 需求日期（范围查询-结束）
    /// </summary>
    public DateTime? RequirementDateEnd { get; set; }

    /// <summary>
    /// 计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? PlanUnit { get; set; } = string.Empty;

    /// <summary>
    /// 毛需求数量（基本单位数量）
    /// </summary>
    public decimal? GrossRequirement { get; set; }

    /// <summary>
    /// 计划接收数量（在途/已订未收等，运算快照）
    /// </summary>
    public decimal? ScheduledReceipts { get; set; }

    /// <summary>
    /// 现有库存数量（运算快照，来源 TaktMaterialPlant.CurrentStock）
    /// </summary>
    public decimal? OnHandQuantity { get; set; }

    /// <summary>
    /// 预计可用库存（运算后 POH 快照）
    /// </summary>
    public decimal? ProjectedOnHand { get; set; }

    /// <summary>
    /// 净需求数量（基本单位数量）
    /// </summary>
    public decimal? NetRequirement { get; set; }

    /// <summary>
    /// 供应类型（字典 logistics_procurement_type；0=自制，1=外购，2=委外）
    /// </summary>
    public int? ProcurementType { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是）
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
// 创建MaterialRequirementsPlanningItem DTO
// ========================================

/// <summary>
/// 创建MaterialRequirementsPlanningItem DTO
/// </summary>
public class TaktMaterialRequirementsPlanningItemCreateDto
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
    /// MRP 头表 ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialRequirementsPlanningId { get; set; }

    /// <summary>
    /// MRP 编码（冗余字段，便于查询）
    /// </summary>
    public string MaterialRequirementsPlanningCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

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
    /// 机种编码（关联 TaktModelDestination.ModelCode，可选）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称（冗余）
    /// </summary>
    public string? ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 父项物料编码（BOM 展开上级，可选）
    /// </summary>
    public string? ParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// BOM 层级（1=顶层成品）
    /// </summary>
    public int BomLevel { get; set; } = 0;

    /// <summary>
    /// 需求日期
    /// </summary>
    public DateTime RequirementDate { get; set; }

    /// <summary>
    /// 计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    [Required(ErrorMessage = "计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）不能为空")]
    public string PlanUnit { get; set; } = string.Empty;

    /// <summary>
    /// 毛需求数量（基本单位数量）
    /// </summary>
    public decimal GrossRequirement { get; set; }

    /// <summary>
    /// 计划接收数量（在途/已订未收等，运算快照）
    /// </summary>
    public decimal ScheduledReceipts { get; set; }

    /// <summary>
    /// 现有库存数量（运算快照，来源 TaktMaterialPlant.CurrentStock）
    /// </summary>
    public decimal OnHandQuantity { get; set; }

    /// <summary>
    /// 预计可用库存（运算后 POH 快照）
    /// </summary>
    public decimal ProjectedOnHand { get; set; }

    /// <summary>
    /// 净需求数量（基本单位数量）
    /// </summary>
    public decimal NetRequirement { get; set; }

    /// <summary>
    /// 供应类型（字典 logistics_procurement_type；0=自制，1=外购，2=委外）
    /// </summary>
    public int ProcurementType { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是）
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
// 更新MaterialRequirementsPlanningItem DTO
// ========================================

/// <summary>
/// 更新MaterialRequirementsPlanningItem DTO
/// 继承 TaktMaterialRequirementsPlanningItemCreateDto，添加 MaterialRequirementsPlanningItemId 字段
/// </summary>
public class TaktMaterialRequirementsPlanningItemUpdateDto : TaktMaterialRequirementsPlanningItemCreateDto
{
    /// <summary>
    /// MaterialRequirementsPlanningItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialRequirementsPlanningItemId { get; set; }

}

// ========================================
// MaterialRequirementsPlanningItem 作废 DTO
// ========================================

/// <summary>
/// MaterialRequirementsPlanningItem 作废/撤销作废 DTO
/// </summary>
public class TaktMaterialRequirementsPlanningItemObsoleteDto
{
    /// <summary>
    /// MaterialRequirementsPlanningItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialRequirementsPlanningItemId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// MaterialRequirementsPlanningItem 导入模板行 DTO
/// </summary>
public class TaktMaterialRequirementsPlanningItemTemplateDto
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
    /// MRP 头表 ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialRequirementsPlanningId { get; set; }

    /// <summary>
    /// MRP 编码（冗余字段，便于查询）
    /// </summary>
    public string? MaterialRequirementsPlanningCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

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
    /// 机种编码（关联 TaktModelDestination.ModelCode，可选）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称（冗余）
    /// </summary>
    public string? ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 父项物料编码（BOM 展开上级，可选）
    /// </summary>
    public string? ParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// BOM 层级（1=顶层成品）
    /// </summary>
    public int? BomLevel { get; set; }

    /// <summary>
    /// 需求日期
    /// </summary>
    public DateTime? RequirementDate { get; set; }

    /// <summary>
    /// 计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? PlanUnit { get; set; } = string.Empty;

    /// <summary>
    /// 毛需求数量（基本单位数量）
    /// </summary>
    public decimal? GrossRequirement { get; set; }

    /// <summary>
    /// 计划接收数量（在途/已订未收等，运算快照）
    /// </summary>
    public decimal? ScheduledReceipts { get; set; }

    /// <summary>
    /// 现有库存数量（运算快照，来源 TaktMaterialPlant.CurrentStock）
    /// </summary>
    public decimal? OnHandQuantity { get; set; }

    /// <summary>
    /// 预计可用库存（运算后 POH 快照）
    /// </summary>
    public decimal? ProjectedOnHand { get; set; }

    /// <summary>
    /// 净需求数量（基本单位数量）
    /// </summary>
    public decimal? NetRequirement { get; set; }

    /// <summary>
    /// 供应类型（字典 logistics_procurement_type；0=自制，1=外购，2=委外）
    /// </summary>
    public int? ProcurementType { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是）
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
/// MaterialRequirementsPlanningItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMaterialRequirementsPlanningItemImportDto
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
    /// MRP 头表 ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MaterialRequirementsPlanningId { get; set; }

    /// <summary>
    /// MRP 编码（冗余字段，便于查询）
    /// </summary>
    public string? MaterialRequirementsPlanningCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

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
    /// 机种编码（关联 TaktModelDestination.ModelCode，可选）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称（冗余）
    /// </summary>
    public string? ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 父项物料编码（BOM 展开上级，可选）
    /// </summary>
    public string? ParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// BOM 层级（1=顶层成品）
    /// </summary>
    public int? BomLevel { get; set; }

    /// <summary>
    /// 需求日期
    /// </summary>
    public DateTime? RequirementDate { get; set; }

    /// <summary>
    /// 计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? PlanUnit { get; set; } = string.Empty;

    /// <summary>
    /// 毛需求数量（基本单位数量）
    /// </summary>
    public decimal? GrossRequirement { get; set; }

    /// <summary>
    /// 计划接收数量（在途/已订未收等，运算快照）
    /// </summary>
    public decimal? ScheduledReceipts { get; set; }

    /// <summary>
    /// 现有库存数量（运算快照，来源 TaktMaterialPlant.CurrentStock）
    /// </summary>
    public decimal? OnHandQuantity { get; set; }

    /// <summary>
    /// 预计可用库存（运算后 POH 快照）
    /// </summary>
    public decimal? ProjectedOnHand { get; set; }

    /// <summary>
    /// 净需求数量（基本单位数量）
    /// </summary>
    public decimal? NetRequirement { get; set; }

    /// <summary>
    /// 供应类型（字典 logistics_procurement_type；0=自制，1=外购，2=委外）
    /// </summary>
    public int? ProcurementType { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是）
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
/// MaterialRequirementsPlanningItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMaterialRequirementsPlanningItemExportDto
{
    /// <summary>
    /// MaterialRequirementsPlanningItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialRequirementsPlanningItemId { get; set; }

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
    /// MRP 头表 ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialRequirementsPlanningId { get; set; }

    /// <summary>
    /// MRP 编码（冗余字段，便于查询）
    /// </summary>
    public string MaterialRequirementsPlanningCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

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
    /// 机种编码（关联 TaktModelDestination.ModelCode，可选）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称（冗余）
    /// </summary>
    public string? ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 父项物料编码（BOM 展开上级，可选）
    /// </summary>
    public string? ParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// BOM 层级（1=顶层成品）
    /// </summary>
    public int BomLevel { get; set; } = 0;

    /// <summary>
    /// 需求日期
    /// </summary>
    public DateTime RequirementDate { get; set; }

    /// <summary>
    /// 计划单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string PlanUnit { get; set; } = string.Empty;

    /// <summary>
    /// 毛需求数量（基本单位数量）
    /// </summary>
    public decimal GrossRequirement { get; set; }

    /// <summary>
    /// 计划接收数量（在途/已订未收等，运算快照）
    /// </summary>
    public decimal ScheduledReceipts { get; set; }

    /// <summary>
    /// 现有库存数量（运算快照，来源 TaktMaterialPlant.CurrentStock）
    /// </summary>
    public decimal OnHandQuantity { get; set; }

    /// <summary>
    /// 预计可用库存（运算后 POH 快照）
    /// </summary>
    public decimal ProjectedOnHand { get; set; }

    /// <summary>
    /// 净需求数量（基本单位数量）
    /// </summary>
    public decimal NetRequirement { get; set; }

    /// <summary>
    /// 供应类型（字典 logistics_procurement_type；0=自制，1=外购，2=委外）
    /// </summary>
    public int ProcurementType { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是）
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

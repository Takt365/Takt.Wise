// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Bom
// 文件名称：TaktBillOfMaterialItemDtos.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：BillOfMaterialItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktBillOfMaterialItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Bom;

// ========================================
// BillOfMaterialItem 响应 DTO
// ========================================

/// <summary>
/// Takt物料清单明细实体（扁平BOM行：一头多行，每行一个直接子件；多层BOM通过子件物料关联其BOM头递归展开）
/// 对应前端 TaktBillOfMaterialItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktBillOfMaterialItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// BillOfMaterialItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialItemId { get; set; }

    /// <summary>
    /// 物料清单ID（关联BOM头，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialId { get; set; }

    /// <summary>
    /// 物料清单名称（填充字段）
    /// </summary>
    public string? BillOfMaterialName { get; set; }

    /// <summary>
    /// BOM编码（冗余，便于查询）
    /// </summary>
    public string BomCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号，步长10：10/20/30…）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 子项物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 子项物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 用量（quantity）
    /// </summary>
    public decimal UsageQuantity { get; set; }

    /// <summary>
    /// 单位（字典 logistics_materials_unit_of_measure_code）
    /// </summary>
    public string MaterialUnit { get; set; } = string.Empty;

    /// <summary>
    /// 损耗率（0-100，scrap_rate）
    /// </summary>
    public decimal ScrapRate { get; set; }

    /// <summary>
    /// 实际用量（用量 × (1 + 损耗率/100)）
    /// </summary>
    public decimal ActualUsageQuantity { get; set; }

    /// <summary>
    /// 工序号（operation_seq）
    /// </summary>
    public int OperationSeq { get; set; } = 0;

    /// <summary>
    /// 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 位号（position，PCB位号等）
    /// </summary>
    public string? Position { get; set; } = string.Empty;

    /// <summary>
    /// 替代组号（substitute_group）
    /// </summary>
    public string? SubstituteGroup { get; set; } = string.Empty;

    /// <summary>
    /// 替代优先级（组内越小越优先）
    /// </summary>
    public int SubstitutePriority { get; set; } = 0;

    /// <summary>
    /// 是否可选件（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int IsOptional { get; set; } = 0;

    /// <summary>
    /// 是否虚拟件（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int IsPhantom { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 物料清单（BOM头）
    /// （主表：TaktBillOfMaterial）
    /// </summary>
    public TaktBillOfMaterialDto? Bom { get; set; }

    /// <summary>
    /// 替代料明细（一行主件可维护多条替代物料）
    /// （子表：TaktBillOfMaterialSubstitute）
    /// </summary>
    public List<TaktBillOfMaterialSubstituteDto>? Substitutes { get; set; }

}

// ========================================
// BillOfMaterialItem 查询 DTO
// ========================================

/// <summary>
/// BillOfMaterialItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktBillOfMaterialItemQueryDto : TaktPagedQuery
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
    /// 物料清单ID（关联BOM头，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? BillOfMaterialId { get; set; }

    /// <summary>
    /// BOM编码（冗余，便于查询）
    /// </summary>
    public string? BomCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号，步长10：10/20/30…）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 子项物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 子项物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 用量（quantity）
    /// </summary>
    public decimal? UsageQuantity { get; set; }

    /// <summary>
    /// 单位（字典 logistics_materials_unit_of_measure_code）
    /// </summary>
    public string? MaterialUnit { get; set; } = string.Empty;

    /// <summary>
    /// 损耗率（0-100，scrap_rate）
    /// </summary>
    public decimal? ScrapRate { get; set; }

    /// <summary>
    /// 实际用量（用量 × (1 + 损耗率/100)）
    /// </summary>
    public decimal? ActualUsageQuantity { get; set; }

    /// <summary>
    /// 工序号（operation_seq）
    /// </summary>
    public int? OperationSeq { get; set; }

    /// <summary>
    /// 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 位号（position，PCB位号等）
    /// </summary>
    public string? Position { get; set; } = string.Empty;

    /// <summary>
    /// 替代组号（substitute_group）
    /// </summary>
    public string? SubstituteGroup { get; set; } = string.Empty;

    /// <summary>
    /// 替代优先级（组内越小越优先）
    /// </summary>
    public int? SubstitutePriority { get; set; }

    /// <summary>
    /// 是否可选件（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? IsOptional { get; set; }

    /// <summary>
    /// 是否虚拟件（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? IsPhantom { get; set; }

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
// 创建BillOfMaterialItem DTO
// ========================================

/// <summary>
/// 创建BillOfMaterialItem DTO
/// </summary>
public class TaktBillOfMaterialItemCreateDto
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
    /// 物料清单ID（关联BOM头，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialId { get; set; }

    /// <summary>
    /// BOM编码（冗余，便于查询）
    /// </summary>
    [Required(ErrorMessage = "BOM编码（冗余，便于查询）不能为空")]
    public string BomCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号，步长10：10/20/30…）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 子项物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "子项物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 子项物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 用量（quantity）
    /// </summary>
    public decimal UsageQuantity { get; set; }

    /// <summary>
    /// 单位（字典 logistics_materials_unit_of_measure_code）
    /// </summary>
    [Required(ErrorMessage = "单位（字典 logistics_materials_unit_of_measure_code）不能为空")]
    public string MaterialUnit { get; set; } = string.Empty;

    /// <summary>
    /// 损耗率（0-100，scrap_rate）
    /// </summary>
    public decimal ScrapRate { get; set; }

    /// <summary>
    /// 实际用量（用量 × (1 + 损耗率/100)）
    /// </summary>
    public decimal ActualUsageQuantity { get; set; }

    /// <summary>
    /// 工序号（operation_seq）
    /// </summary>
    public int OperationSeq { get; set; } = 0;

    /// <summary>
    /// 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 位号（position，PCB位号等）
    /// </summary>
    public string? Position { get; set; } = string.Empty;

    /// <summary>
    /// 替代组号（substitute_group）
    /// </summary>
    public string? SubstituteGroup { get; set; } = string.Empty;

    /// <summary>
    /// 替代优先级（组内越小越优先）
    /// </summary>
    public int SubstitutePriority { get; set; } = 0;

    /// <summary>
    /// 是否可选件（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int IsOptional { get; set; } = 0;

    /// <summary>
    /// 是否虚拟件（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int IsPhantom { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 替代料明细（一行主件可维护多条替代物料）（子表，级联保存）
    /// </summary>
    public List<TaktBillOfMaterialSubstituteCreateDto>? Substitutes { get; set; }

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
// 更新BillOfMaterialItem DTO
// ========================================

/// <summary>
/// 更新BillOfMaterialItem DTO
/// 继承 TaktBillOfMaterialItemCreateDto，添加 BillOfMaterialItemId 字段
/// </summary>
public class TaktBillOfMaterialItemUpdateDto : TaktBillOfMaterialItemCreateDto
{
    /// <summary>
    /// BillOfMaterialItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialItemId { get; set; }

    /// <summary>
    /// 替代料明细（一行主件可维护多条替代物料）（子表，级联保存）
    /// </summary>
    public new List<TaktBillOfMaterialSubstituteUpdateDto>? Substitutes { get; set; }

}

// ========================================
// BillOfMaterialItem 作废 DTO
// ========================================

/// <summary>
/// BillOfMaterialItem 作废/撤销作废 DTO
/// </summary>
public class TaktBillOfMaterialItemObsoleteDto
{
    /// <summary>
    /// BillOfMaterialItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialItemId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// BillOfMaterialItem 导入模板行 DTO
/// </summary>
public class TaktBillOfMaterialItemTemplateDto
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
    /// 物料清单ID（关联BOM头，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? BillOfMaterialId { get; set; }

    /// <summary>
    /// BOM编码（冗余，便于查询）
    /// </summary>
    public string? BomCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号，步长10：10/20/30…）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 子项物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 子项物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 用量（quantity）
    /// </summary>
    public decimal? UsageQuantity { get; set; }

    /// <summary>
    /// 单位（字典 logistics_materials_unit_of_measure_code）
    /// </summary>
    public string? MaterialUnit { get; set; } = string.Empty;

    /// <summary>
    /// 损耗率（0-100，scrap_rate）
    /// </summary>
    public decimal? ScrapRate { get; set; }

    /// <summary>
    /// 实际用量（用量 × (1 + 损耗率/100)）
    /// </summary>
    public decimal? ActualUsageQuantity { get; set; }

    /// <summary>
    /// 工序号（operation_seq）
    /// </summary>
    public int? OperationSeq { get; set; }

    /// <summary>
    /// 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 位号（position，PCB位号等）
    /// </summary>
    public string? Position { get; set; } = string.Empty;

    /// <summary>
    /// 替代组号（substitute_group）
    /// </summary>
    public string? SubstituteGroup { get; set; } = string.Empty;

    /// <summary>
    /// 替代优先级（组内越小越优先）
    /// </summary>
    public int? SubstitutePriority { get; set; }

    /// <summary>
    /// 是否可选件（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? IsOptional { get; set; }

    /// <summary>
    /// 是否虚拟件（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? IsPhantom { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

    /// <summary>
    /// 替代料明细（一行主件可维护多条替代物料）（子表，级联保存）
    /// </summary>
    public List<TaktBillOfMaterialSubstituteCreateDto>? Substitutes { get; set; }

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
/// BillOfMaterialItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktBillOfMaterialItemImportDto
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
    /// 物料清单ID（关联BOM头，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? BillOfMaterialId { get; set; }

    /// <summary>
    /// BOM编码（冗余，便于查询）
    /// </summary>
    public string? BomCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号，步长10：10/20/30…）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 子项物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 子项物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 用量（quantity）
    /// </summary>
    public decimal? UsageQuantity { get; set; }

    /// <summary>
    /// 单位（字典 logistics_materials_unit_of_measure_code）
    /// </summary>
    public string? MaterialUnit { get; set; } = string.Empty;

    /// <summary>
    /// 损耗率（0-100，scrap_rate）
    /// </summary>
    public decimal? ScrapRate { get; set; }

    /// <summary>
    /// 实际用量（用量 × (1 + 损耗率/100)）
    /// </summary>
    public decimal? ActualUsageQuantity { get; set; }

    /// <summary>
    /// 工序号（operation_seq）
    /// </summary>
    public int? OperationSeq { get; set; }

    /// <summary>
    /// 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 位号（position，PCB位号等）
    /// </summary>
    public string? Position { get; set; } = string.Empty;

    /// <summary>
    /// 替代组号（substitute_group）
    /// </summary>
    public string? SubstituteGroup { get; set; } = string.Empty;

    /// <summary>
    /// 替代优先级（组内越小越优先）
    /// </summary>
    public int? SubstitutePriority { get; set; }

    /// <summary>
    /// 是否可选件（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? IsOptional { get; set; }

    /// <summary>
    /// 是否虚拟件（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int? IsPhantom { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

    /// <summary>
    /// 替代料明细（一行主件可维护多条替代物料）（子表，级联保存）
    /// </summary>
    public List<TaktBillOfMaterialSubstituteCreateDto>? Substitutes { get; set; }

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
/// BillOfMaterialItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktBillOfMaterialItemExportDto
{
    /// <summary>
    /// BillOfMaterialItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialItemId { get; set; }

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
    /// 物料清单ID（关联BOM头，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialId { get; set; }

    /// <summary>
    /// BOM编码（冗余，便于查询）
    /// </summary>
    public string BomCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号，步长10：10/20/30…）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 子项物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 子项物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 用量（quantity）
    /// </summary>
    public decimal UsageQuantity { get; set; }

    /// <summary>
    /// 单位（字典 logistics_materials_unit_of_measure_code）
    /// </summary>
    public string MaterialUnit { get; set; } = string.Empty;

    /// <summary>
    /// 损耗率（0-100，scrap_rate）
    /// </summary>
    public decimal ScrapRate { get; set; }

    /// <summary>
    /// 实际用量（用量 × (1 + 损耗率/100)）
    /// </summary>
    public decimal ActualUsageQuantity { get; set; }

    /// <summary>
    /// 工序号（operation_seq）
    /// </summary>
    public int OperationSeq { get; set; } = 0;

    /// <summary>
    /// 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    public string? WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 位号（position，PCB位号等）
    /// </summary>
    public string? Position { get; set; } = string.Empty;

    /// <summary>
    /// 替代组号（substitute_group）
    /// </summary>
    public string? SubstituteGroup { get; set; } = string.Empty;

    /// <summary>
    /// 替代优先级（组内越小越优先）
    /// </summary>
    public int SubstitutePriority { get; set; } = 0;

    /// <summary>
    /// 是否可选件（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int IsOptional { get; set; } = 0;

    /// <summary>
    /// 是否虚拟件（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    public int IsPhantom { get; set; } = 0;

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

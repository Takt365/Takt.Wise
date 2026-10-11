// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Bom
// 文件名称：TaktBillOfMaterialSubstituteDtos.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：BillOfMaterialSubstitute 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktBillOfMaterialSubstitute 生成，请按需审阅）
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
// BillOfMaterialSubstitute 响应 DTO
// ========================================

/// <summary>
/// BOM替代料实体（挂载于物料清单明细行，一行主件可维护多条替代物料）
/// 对应前端 TaktBillOfMaterialSubstituteDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktBillOfMaterialSubstituteDto : TaktCompanyDtoBase
{
    /// <summary>
    /// BillOfMaterialSubstituteID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialSubstituteId { get; set; }

    /// <summary>
    /// 物料清单明细ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialItemId { get; set; }

    /// <summary>
    /// 物料清单明细名称（填充字段）
    /// </summary>
    public string? BillOfMaterialItemName { get; set; }

    /// <summary>
    /// 物料清单ID（冗余，便于按BOM头查询，序列化为string以避免Javascript精度问题）
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
    /// 主件物料编码（冗余，对应明细行子项物料编码）
    /// </summary>
    public string PrimaryMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 替代行号（步长10：10/20/30…）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 替代物料ID（选项 TaktMaterialPlants/options；DictValue=Id，ExtValue=PlantCode）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SubstituteMaterialId { get; set; }

    /// <summary>
    /// 替代物料名称（填充字段）
    /// </summary>
    public string? SubstituteMaterialName { get; set; }

    /// <summary>
    /// 替代物料编码（冗余）
    /// </summary>
    public string SubstituteMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 替代组号（与明细行 substitute_group 对齐，便于组内检索）
    /// </summary>
    public string? SubstituteGroup { get; set; } = string.Empty;

    /// <summary>
    /// 替代优先级（越小越优先）
    /// </summary>
    public int SubstitutePriority { get; set; } = 0;

    /// <summary>
    /// 替代用量
    /// </summary>
    public decimal UsageQuantity { get; set; }

    /// <summary>
    /// 单位（字典 logistics_materials_unit_of_measure_code）
    /// </summary>
    public string MaterialUnit { get; set; } = string.Empty;

    /// <summary>
    /// 替代比例（相对主件用量，默认1表示等量替代）
    /// </summary>
    public decimal UsageRatio { get; set; }

    /// <summary>
    /// 是否启用（0=否，1=是，字典 sys_yes_no）
    /// </summary>
    public int IsEnabled { get; set; } = 0;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// 失效日期（为空表示永久有效）
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 物料清单明细（主表）
    /// （主表：TaktBillOfMaterialItem）
    /// </summary>
    public TaktBillOfMaterialItemDto? BillOfMaterialItem { get; set; }

    /// <summary>
    /// 替代物料（工厂物料主数据）
    /// （主表：TaktMaterialPlant）
    /// </summary>
    public TaktMaterialPlantDto? SubstituteMaterialPlant { get; set; }

}

// ========================================
// BillOfMaterialSubstitute 查询 DTO
// ========================================

/// <summary>
/// BillOfMaterialSubstitute 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktBillOfMaterialSubstituteQueryDto : TaktPagedQuery
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
    /// 物料清单明细ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? BillOfMaterialItemId { get; set; }

    /// <summary>
    /// 物料清单ID（冗余，便于按BOM头查询，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? BillOfMaterialId { get; set; }

    /// <summary>
    /// BOM编码（冗余，便于查询）
    /// </summary>
    public string? BomCode { get; set; } = string.Empty;

    /// <summary>
    /// 主件物料编码（冗余，对应明细行子项物料编码）
    /// </summary>
    public string? PrimaryMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 替代行号（步长10：10/20/30…）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 替代物料ID（选项 TaktMaterialPlants/options；DictValue=Id，ExtValue=PlantCode）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SubstituteMaterialId { get; set; }

    /// <summary>
    /// 替代物料编码（冗余）
    /// </summary>
    public string? SubstituteMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 替代组号（与明细行 substitute_group 对齐，便于组内检索）
    /// </summary>
    public string? SubstituteGroup { get; set; } = string.Empty;

    /// <summary>
    /// 替代优先级（越小越优先）
    /// </summary>
    public int? SubstitutePriority { get; set; }

    /// <summary>
    /// 替代用量
    /// </summary>
    public decimal? UsageQuantity { get; set; }

    /// <summary>
    /// 单位（字典 logistics_materials_unit_of_measure_code）
    /// </summary>
    public string? MaterialUnit { get; set; } = string.Empty;

    /// <summary>
    /// 替代比例（相对主件用量，默认1表示等量替代）
    /// </summary>
    public decimal? UsageRatio { get; set; }

    /// <summary>
    /// 是否启用（0=否，1=是，字典 sys_yes_no）
    /// </summary>
    public int? IsEnabled { get; set; }

    /// <summary>
    /// 生效日期（范围查询-开始）
    /// </summary>
    public DateTime? EffectiveDateStart { get; set; }

    /// <summary>
    /// 生效日期（范围查询-结束）
    /// </summary>
    public DateTime? EffectiveDateEnd { get; set; }

    /// <summary>
    /// 失效日期（为空表示永久有效）（范围查询-开始）
    /// </summary>
    public DateTime? ExpiryDateStart { get; set; }

    /// <summary>
    /// 失效日期（为空表示永久有效）（范围查询-结束）
    /// </summary>
    public DateTime? ExpiryDateEnd { get; set; }

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
// 创建BillOfMaterialSubstitute DTO
// ========================================

/// <summary>
/// 创建BillOfMaterialSubstitute DTO
/// </summary>
public class TaktBillOfMaterialSubstituteCreateDto
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
    /// 物料清单明细ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialItemId { get; set; }

    /// <summary>
    /// 物料清单ID（冗余，便于按BOM头查询，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialId { get; set; }

    /// <summary>
    /// BOM编码（冗余，便于查询）
    /// </summary>
    [Required(ErrorMessage = "BOM编码（冗余，便于查询）不能为空")]
    public string BomCode { get; set; } = string.Empty;

    /// <summary>
    /// 主件物料编码（冗余，对应明细行子项物料编码）
    /// </summary>
    [Required(ErrorMessage = "主件物料编码（冗余，对应明细行子项物料编码）不能为空")]
    public string PrimaryMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 替代行号（步长10：10/20/30…）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 替代物料ID（选项 TaktMaterialPlants/options；DictValue=Id，ExtValue=PlantCode）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SubstituteMaterialId { get; set; }

    /// <summary>
    /// 替代物料编码（冗余）
    /// </summary>
    public string SubstituteMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 替代组号（与明细行 substitute_group 对齐，便于组内检索）
    /// </summary>
    public string? SubstituteGroup { get; set; } = string.Empty;

    /// <summary>
    /// 替代优先级（越小越优先）
    /// </summary>
    public int SubstitutePriority { get; set; } = 0;

    /// <summary>
    /// 替代用量
    /// </summary>
    public decimal UsageQuantity { get; set; }

    /// <summary>
    /// 单位（字典 logistics_materials_unit_of_measure_code）
    /// </summary>
    [Required(ErrorMessage = "单位（字典 logistics_materials_unit_of_measure_code）不能为空")]
    public string MaterialUnit { get; set; } = string.Empty;

    /// <summary>
    /// 替代比例（相对主件用量，默认1表示等量替代）
    /// </summary>
    public decimal UsageRatio { get; set; }

    /// <summary>
    /// 是否启用（0=否，1=是，字典 sys_yes_no）
    /// </summary>
    public int IsEnabled { get; set; } = 0;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// 失效日期（为空表示永久有效）
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

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
// 更新BillOfMaterialSubstitute DTO
// ========================================

/// <summary>
/// 更新BillOfMaterialSubstitute DTO
/// 继承 TaktBillOfMaterialSubstituteCreateDto，添加 BillOfMaterialSubstituteId 字段
/// </summary>
public class TaktBillOfMaterialSubstituteUpdateDto : TaktBillOfMaterialSubstituteCreateDto
{
    /// <summary>
    /// BillOfMaterialSubstituteID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialSubstituteId { get; set; }

}

// ========================================
// BillOfMaterialSubstitute 作废 DTO
// ========================================

/// <summary>
/// BillOfMaterialSubstitute 作废/撤销作废 DTO
/// </summary>
public class TaktBillOfMaterialSubstituteObsoleteDto
{
    /// <summary>
    /// BillOfMaterialSubstituteID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialSubstituteId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// BillOfMaterialSubstitute 导入模板行 DTO
/// </summary>
public class TaktBillOfMaterialSubstituteTemplateDto
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
    /// 物料清单明细ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? BillOfMaterialItemId { get; set; }

    /// <summary>
    /// 物料清单ID（冗余，便于按BOM头查询，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? BillOfMaterialId { get; set; }

    /// <summary>
    /// BOM编码（冗余，便于查询）
    /// </summary>
    public string? BomCode { get; set; } = string.Empty;

    /// <summary>
    /// 主件物料编码（冗余，对应明细行子项物料编码）
    /// </summary>
    public string? PrimaryMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 替代行号（步长10：10/20/30…）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 替代物料ID（选项 TaktMaterialPlants/options；DictValue=Id，ExtValue=PlantCode）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SubstituteMaterialId { get; set; }

    /// <summary>
    /// 替代物料编码（冗余）
    /// </summary>
    public string? SubstituteMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 替代组号（与明细行 substitute_group 对齐，便于组内检索）
    /// </summary>
    public string? SubstituteGroup { get; set; } = string.Empty;

    /// <summary>
    /// 替代优先级（越小越优先）
    /// </summary>
    public int? SubstitutePriority { get; set; }

    /// <summary>
    /// 替代用量
    /// </summary>
    public decimal? UsageQuantity { get; set; }

    /// <summary>
    /// 单位（字典 logistics_materials_unit_of_measure_code）
    /// </summary>
    public string? MaterialUnit { get; set; } = string.Empty;

    /// <summary>
    /// 替代比例（相对主件用量，默认1表示等量替代）
    /// </summary>
    public decimal? UsageRatio { get; set; }

    /// <summary>
    /// 是否启用（0=否，1=是，字典 sys_yes_no）
    /// </summary>
    public int? IsEnabled { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// 失效日期（为空表示永久有效）
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

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
/// BillOfMaterialSubstitute 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktBillOfMaterialSubstituteImportDto
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
    /// 物料清单明细ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? BillOfMaterialItemId { get; set; }

    /// <summary>
    /// 物料清单ID（冗余，便于按BOM头查询，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? BillOfMaterialId { get; set; }

    /// <summary>
    /// BOM编码（冗余，便于查询）
    /// </summary>
    public string? BomCode { get; set; } = string.Empty;

    /// <summary>
    /// 主件物料编码（冗余，对应明细行子项物料编码）
    /// </summary>
    public string? PrimaryMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 替代行号（步长10：10/20/30…）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 替代物料ID（选项 TaktMaterialPlants/options；DictValue=Id，ExtValue=PlantCode）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SubstituteMaterialId { get; set; }

    /// <summary>
    /// 替代物料编码（冗余）
    /// </summary>
    public string? SubstituteMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 替代组号（与明细行 substitute_group 对齐，便于组内检索）
    /// </summary>
    public string? SubstituteGroup { get; set; } = string.Empty;

    /// <summary>
    /// 替代优先级（越小越优先）
    /// </summary>
    public int? SubstitutePriority { get; set; }

    /// <summary>
    /// 替代用量
    /// </summary>
    public decimal? UsageQuantity { get; set; }

    /// <summary>
    /// 单位（字典 logistics_materials_unit_of_measure_code）
    /// </summary>
    public string? MaterialUnit { get; set; } = string.Empty;

    /// <summary>
    /// 替代比例（相对主件用量，默认1表示等量替代）
    /// </summary>
    public decimal? UsageRatio { get; set; }

    /// <summary>
    /// 是否启用（0=否，1=是，字典 sys_yes_no）
    /// </summary>
    public int? IsEnabled { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// 失效日期（为空表示永久有效）
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

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
/// BillOfMaterialSubstitute 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktBillOfMaterialSubstituteExportDto
{
    /// <summary>
    /// BillOfMaterialSubstituteID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialSubstituteId { get; set; }

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
    /// 物料清单明细ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialItemId { get; set; }

    /// <summary>
    /// 物料清单ID（冗余，便于按BOM头查询，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialId { get; set; }

    /// <summary>
    /// BOM编码（冗余，便于查询）
    /// </summary>
    public string BomCode { get; set; } = string.Empty;

    /// <summary>
    /// 主件物料编码（冗余，对应明细行子项物料编码）
    /// </summary>
    public string PrimaryMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 替代行号（步长10：10/20/30…）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 替代物料ID（选项 TaktMaterialPlants/options；DictValue=Id，ExtValue=PlantCode）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SubstituteMaterialId { get; set; }

    /// <summary>
    /// 替代物料编码（冗余）
    /// </summary>
    public string SubstituteMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 替代组号（与明细行 substitute_group 对齐，便于组内检索）
    /// </summary>
    public string? SubstituteGroup { get; set; } = string.Empty;

    /// <summary>
    /// 替代优先级（越小越优先）
    /// </summary>
    public int SubstitutePriority { get; set; } = 0;

    /// <summary>
    /// 替代用量
    /// </summary>
    public decimal UsageQuantity { get; set; }

    /// <summary>
    /// 单位（字典 logistics_materials_unit_of_measure_code）
    /// </summary>
    public string MaterialUnit { get; set; } = string.Empty;

    /// <summary>
    /// 替代比例（相对主件用量，默认1表示等量替代）
    /// </summary>
    public decimal UsageRatio { get; set; }

    /// <summary>
    /// 是否启用（0=否，1=是，字典 sys_yes_no）
    /// </summary>
    public int IsEnabled { get; set; } = 0;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// 失效日期（为空表示永久有效）
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

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

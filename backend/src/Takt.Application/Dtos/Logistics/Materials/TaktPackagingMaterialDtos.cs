// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktPackagingMaterialDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：PackagingMaterial 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPackagingMaterial 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Materials;

// ========================================
// PackagingMaterial 响应 DTO
// ========================================

/// <summary>
/// Takt包装物料实体
/// 对应前端 TaktPackagingMaterialDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPackagingMaterialDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PackagingMaterialID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PackagingMaterialId { get; set; }

    /// <summary>
    /// 包装物料编码
    /// </summary>
    public string PackagingMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：随物料）
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 海关商品编码（HS Code）
    /// </summary>
    public string? HsCode { get; set; } = string.Empty;

    /// <summary>
    /// 商品名称（HS Name；海关申报完整品名，可超默认短串）
    /// </summary>
    public string? HsName { get; set; } = string.Empty;

    /// <summary>
    /// 附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）
    /// </summary>
    public string? AdditionalCode { get; set; } = string.Empty;

    /// <summary>
    /// 原产国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）
    /// </summary>
    public string? OriginCountryRegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 原产国/地区名称
    /// </summary>
    public string? OriginCountryRegionName { get; set; } = string.Empty;

    /// <summary>
    /// 目的国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）
    /// </summary>
    public string? DestinationCountryRegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 目的国/地区名称
    /// </summary>
    public string? DestinationCountryRegionName { get; set; } = string.Empty;

    /// <summary>
    /// 监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）
    /// </summary>
    public string? RegulatoryConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率/协定税率标识（记录适用的关税税率类型，便于成本核算）
    /// </summary>
    public string? TariffRateType { get; set; } = string.Empty;

    /// <summary>
    /// 毛重（包含包装物的总重量，单位：千克）
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重（不含包装物的净重量，单位：千克）
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等；默认 KG）
    /// </summary>
    public string WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 业务量/容积（一个包装单位的体积，单位：立方米）
    /// </summary>
    public decimal? BusinessVolume { get; set; }

    /// <summary>
    /// 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等；默认 M3）
    /// </summary>
    public string VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 大小/量纲（尺寸量纲或大小规格）
    /// </summary>
    public string? SizeDimension { get; set; } = string.Empty;

    /// <summary>
    /// 包装类型（字典 logistics_materials_material_type；DictValue=VERP 等；默认 VERP）
    /// </summary>
    public string PackagingType { get; set; } = string.Empty;

    /// <summary>
    /// 包装单位（字典 logistics_materials_unit_of_measure_code；DictValue=CAR/CT 等；默认 CAR）
    /// </summary>
    public string PackingUnit { get; set; } = string.Empty;

    /// <summary>
    /// 每包装数量（一个包装包含的基本单位数量）
    /// </summary>
    public decimal? QuantityPerPacking { get; set; }

    /// <summary>
    /// 包装规格（含多段规格说明，可超默认短串）
    /// </summary>
    public string? PackagingSpec { get; set; } = string.Empty;

    /// <summary>
    /// 包装描述（超长说明，可超默认短串）
    /// </summary>
    public string? PackagingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

}

// ========================================
// PackagingMaterial 查询 DTO
// ========================================

/// <summary>
/// PackagingMaterial 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPackagingMaterialQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码
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
    /// 包装物料编码
    /// </summary>
    public string? PackagingMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：随物料）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 海关商品编码（HS Code）
    /// </summary>
    public string? HsCode { get; set; } = string.Empty;

    /// <summary>
    /// 商品名称（HS Name；海关申报完整品名，可超默认短串）
    /// </summary>
    public string? HsName { get; set; } = string.Empty;

    /// <summary>
    /// 附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）
    /// </summary>
    public string? AdditionalCode { get; set; } = string.Empty;

    /// <summary>
    /// 原产国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）
    /// </summary>
    public string? OriginCountryRegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 原产国/地区名称
    /// </summary>
    public string? OriginCountryRegionName { get; set; } = string.Empty;

    /// <summary>
    /// 目的国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）
    /// </summary>
    public string? DestinationCountryRegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 目的国/地区名称
    /// </summary>
    public string? DestinationCountryRegionName { get; set; } = string.Empty;

    /// <summary>
    /// 监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）
    /// </summary>
    public string? RegulatoryConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率/协定税率标识（记录适用的关税税率类型，便于成本核算）
    /// </summary>
    public string? TariffRateType { get; set; } = string.Empty;

    /// <summary>
    /// 毛重（包含包装物的总重量，单位：千克）
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重（不含包装物的净重量，单位：千克）
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等；默认 KG）
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 业务量/容积（一个包装单位的体积，单位：立方米）
    /// </summary>
    public decimal? BusinessVolume { get; set; }

    /// <summary>
    /// 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等；默认 M3）
    /// </summary>
    public string? VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 大小/量纲（尺寸量纲或大小规格）
    /// </summary>
    public string? SizeDimension { get; set; } = string.Empty;

    /// <summary>
    /// 包装类型（字典 logistics_materials_material_type；DictValue=VERP 等；默认 VERP）
    /// </summary>
    public string? PackagingType { get; set; } = string.Empty;

    /// <summary>
    /// 包装单位（字典 logistics_materials_unit_of_measure_code；DictValue=CAR/CT 等；默认 CAR）
    /// </summary>
    public string? PackingUnit { get; set; } = string.Empty;

    /// <summary>
    /// 每包装数量（一个包装包含的基本单位数量）
    /// </summary>
    public decimal? QuantityPerPacking { get; set; }

    /// <summary>
    /// 包装规格（含多段规格说明，可超默认短串）
    /// </summary>
    public string? PackagingSpec { get; set; } = string.Empty;

    /// <summary>
    /// 包装描述（超长说明，可超默认短串）
    /// </summary>
    public string? PackagingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

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
// 创建PackagingMaterial DTO
// ========================================

/// <summary>
/// 创建PackagingMaterial DTO
/// </summary>
public class TaktPackagingMaterialCreateDto
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
    /// 包装物料编码
    /// </summary>
    [Required(ErrorMessage = "包装物料编码不能为空")]
    public string PackagingMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：随物料）
    /// </summary>
    [Required(ErrorMessage = "物料描述（回填：随物料）不能为空")]
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 海关商品编码（HS Code）
    /// </summary>
    public string? HsCode { get; set; } = string.Empty;

    /// <summary>
    /// 商品名称（HS Name；海关申报完整品名，可超默认短串）
    /// </summary>
    public string? HsName { get; set; } = string.Empty;

    /// <summary>
    /// 附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）
    /// </summary>
    public string? AdditionalCode { get; set; } = string.Empty;

    /// <summary>
    /// 原产国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）
    /// </summary>
    public string? OriginCountryRegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 原产国/地区名称
    /// </summary>
    public string? OriginCountryRegionName { get; set; } = string.Empty;

    /// <summary>
    /// 目的国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）
    /// </summary>
    public string? DestinationCountryRegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 目的国/地区名称
    /// </summary>
    public string? DestinationCountryRegionName { get; set; } = string.Empty;

    /// <summary>
    /// 监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）
    /// </summary>
    public string? RegulatoryConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率/协定税率标识（记录适用的关税税率类型，便于成本核算）
    /// </summary>
    public string? TariffRateType { get; set; } = string.Empty;

    /// <summary>
    /// 毛重（包含包装物的总重量，单位：千克）
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重（不含包装物的净重量，单位：千克）
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等；默认 KG）
    /// </summary>
    [Required(ErrorMessage = "重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等；默认 KG）不能为空")]
    public string WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 业务量/容积（一个包装单位的体积，单位：立方米）
    /// </summary>
    public decimal? BusinessVolume { get; set; }

    /// <summary>
    /// 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等；默认 M3）
    /// </summary>
    [Required(ErrorMessage = "体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等；默认 M3）不能为空")]
    public string VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 大小/量纲（尺寸量纲或大小规格）
    /// </summary>
    public string? SizeDimension { get; set; } = string.Empty;

    /// <summary>
    /// 包装类型（字典 logistics_materials_material_type；DictValue=VERP 等；默认 VERP）
    /// </summary>
    [Required(ErrorMessage = "包装类型（字典 logistics_materials_material_type；DictValue=VERP 等；默认 VERP）不能为空")]
    public string PackagingType { get; set; } = string.Empty;

    /// <summary>
    /// 包装单位（字典 logistics_materials_unit_of_measure_code；DictValue=CAR/CT 等；默认 CAR）
    /// </summary>
    [Required(ErrorMessage = "包装单位（字典 logistics_materials_unit_of_measure_code；DictValue=CAR/CT 等；默认 CAR）不能为空")]
    public string PackingUnit { get; set; } = string.Empty;

    /// <summary>
    /// 每包装数量（一个包装包含的基本单位数量）
    /// </summary>
    public decimal? QuantityPerPacking { get; set; }

    /// <summary>
    /// 包装规格（含多段规格说明，可超默认短串）
    /// </summary>
    public string? PackagingSpec { get; set; } = string.Empty;

    /// <summary>
    /// 包装描述（超长说明，可超默认短串）
    /// </summary>
    public string? PackagingDescription { get; set; } = string.Empty;

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
// 更新PackagingMaterial DTO
// ========================================

/// <summary>
/// 更新PackagingMaterial DTO
/// 继承 TaktPackagingMaterialCreateDto，添加 PackagingMaterialId 字段
/// </summary>
public class TaktPackagingMaterialUpdateDto : TaktPackagingMaterialCreateDto
{
    /// <summary>
    /// PackagingMaterialID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PackagingMaterialId { get; set; }

}

// ========================================
// PackagingMaterial 排序 DTO
// ========================================

/// <summary>
/// PackagingMaterial 排序更新 DTO
/// </summary>
public class TaktPackagingMaterialSortDto
{
    /// <summary>
    /// PackagingMaterialID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PackagingMaterialId { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [Required(ErrorMessage = "排序号（越小越靠前）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PackagingMaterial 导入模板行 DTO
/// </summary>
public class TaktPackagingMaterialTemplateDto
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
    /// 包装物料编码
    /// </summary>
    public string? PackagingMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：随物料）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 海关商品编码（HS Code）
    /// </summary>
    public string? HsCode { get; set; } = string.Empty;

    /// <summary>
    /// 商品名称（HS Name；海关申报完整品名，可超默认短串）
    /// </summary>
    public string? HsName { get; set; } = string.Empty;

    /// <summary>
    /// 附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）
    /// </summary>
    public string? AdditionalCode { get; set; } = string.Empty;

    /// <summary>
    /// 原产国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）
    /// </summary>
    public string? OriginCountryRegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 原产国/地区名称
    /// </summary>
    public string? OriginCountryRegionName { get; set; } = string.Empty;

    /// <summary>
    /// 目的国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）
    /// </summary>
    public string? DestinationCountryRegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 目的国/地区名称
    /// </summary>
    public string? DestinationCountryRegionName { get; set; } = string.Empty;

    /// <summary>
    /// 监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）
    /// </summary>
    public string? RegulatoryConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率/协定税率标识（记录适用的关税税率类型，便于成本核算）
    /// </summary>
    public string? TariffRateType { get; set; } = string.Empty;

    /// <summary>
    /// 毛重（包含包装物的总重量，单位：千克）
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重（不含包装物的净重量，单位：千克）
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等；默认 KG）
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 业务量/容积（一个包装单位的体积，单位：立方米）
    /// </summary>
    public decimal? BusinessVolume { get; set; }

    /// <summary>
    /// 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等；默认 M3）
    /// </summary>
    public string? VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 大小/量纲（尺寸量纲或大小规格）
    /// </summary>
    public string? SizeDimension { get; set; } = string.Empty;

    /// <summary>
    /// 包装类型（字典 logistics_materials_material_type；DictValue=VERP 等；默认 VERP）
    /// </summary>
    public string? PackagingType { get; set; } = string.Empty;

    /// <summary>
    /// 包装单位（字典 logistics_materials_unit_of_measure_code；DictValue=CAR/CT 等；默认 CAR）
    /// </summary>
    public string? PackingUnit { get; set; } = string.Empty;

    /// <summary>
    /// 每包装数量（一个包装包含的基本单位数量）
    /// </summary>
    public decimal? QuantityPerPacking { get; set; }

    /// <summary>
    /// 包装规格（含多段规格说明，可超默认短串）
    /// </summary>
    public string? PackagingSpec { get; set; } = string.Empty;

    /// <summary>
    /// 包装描述（超长说明，可超默认短串）
    /// </summary>
    public string? PackagingDescription { get; set; } = string.Empty;

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
/// PackagingMaterial 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPackagingMaterialImportDto
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
    /// 包装物料编码
    /// </summary>
    public string? PackagingMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：随物料）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 海关商品编码（HS Code）
    /// </summary>
    public string? HsCode { get; set; } = string.Empty;

    /// <summary>
    /// 商品名称（HS Name；海关申报完整品名，可超默认短串）
    /// </summary>
    public string? HsName { get; set; } = string.Empty;

    /// <summary>
    /// 附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）
    /// </summary>
    public string? AdditionalCode { get; set; } = string.Empty;

    /// <summary>
    /// 原产国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）
    /// </summary>
    public string? OriginCountryRegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 原产国/地区名称
    /// </summary>
    public string? OriginCountryRegionName { get; set; } = string.Empty;

    /// <summary>
    /// 目的国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）
    /// </summary>
    public string? DestinationCountryRegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 目的国/地区名称
    /// </summary>
    public string? DestinationCountryRegionName { get; set; } = string.Empty;

    /// <summary>
    /// 监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）
    /// </summary>
    public string? RegulatoryConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率/协定税率标识（记录适用的关税税率类型，便于成本核算）
    /// </summary>
    public string? TariffRateType { get; set; } = string.Empty;

    /// <summary>
    /// 毛重（包含包装物的总重量，单位：千克）
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重（不含包装物的净重量，单位：千克）
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等；默认 KG）
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 业务量/容积（一个包装单位的体积，单位：立方米）
    /// </summary>
    public decimal? BusinessVolume { get; set; }

    /// <summary>
    /// 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等；默认 M3）
    /// </summary>
    public string? VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 大小/量纲（尺寸量纲或大小规格）
    /// </summary>
    public string? SizeDimension { get; set; } = string.Empty;

    /// <summary>
    /// 包装类型（字典 logistics_materials_material_type；DictValue=VERP 等；默认 VERP）
    /// </summary>
    public string? PackagingType { get; set; } = string.Empty;

    /// <summary>
    /// 包装单位（字典 logistics_materials_unit_of_measure_code；DictValue=CAR/CT 等；默认 CAR）
    /// </summary>
    public string? PackingUnit { get; set; } = string.Empty;

    /// <summary>
    /// 每包装数量（一个包装包含的基本单位数量）
    /// </summary>
    public decimal? QuantityPerPacking { get; set; }

    /// <summary>
    /// 包装规格（含多段规格说明，可超默认短串）
    /// </summary>
    public string? PackagingSpec { get; set; } = string.Empty;

    /// <summary>
    /// 包装描述（超长说明，可超默认短串）
    /// </summary>
    public string? PackagingDescription { get; set; } = string.Empty;

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
/// PackagingMaterial 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPackagingMaterialExportDto
{
    /// <summary>
    /// PackagingMaterialID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PackagingMaterialId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 包装物料编码
    /// </summary>
    public string PackagingMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：随物料）
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 海关商品编码（HS Code）
    /// </summary>
    public string? HsCode { get; set; } = string.Empty;

    /// <summary>
    /// 商品名称（HS Name；海关申报完整品名，可超默认短串）
    /// </summary>
    public string? HsName { get; set; } = string.Empty;

    /// <summary>
    /// 附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）
    /// </summary>
    public string? AdditionalCode { get; set; } = string.Empty;

    /// <summary>
    /// 原产国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）
    /// </summary>
    public string? OriginCountryRegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 原产国/地区名称
    /// </summary>
    public string? OriginCountryRegionName { get; set; } = string.Empty;

    /// <summary>
    /// 目的国/地区编码（字典 sys_country_code；DictValue=ISO 3166-1 alpha-2）
    /// </summary>
    public string? DestinationCountryRegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 目的国/地区名称
    /// </summary>
    public string? DestinationCountryRegionName { get; set; } = string.Empty;

    /// <summary>
    /// 监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）
    /// </summary>
    public string? RegulatoryConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率/协定税率标识（记录适用的关税税率类型，便于成本核算）
    /// </summary>
    public string? TariffRateType { get; set; } = string.Empty;

    /// <summary>
    /// 毛重（包含包装物的总重量，单位：千克）
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重（不含包装物的净重量，单位：千克）
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（字典 logistics_materials_unit_of_measure_code；DictValue=KG/G/T 等；默认 KG）
    /// </summary>
    public string WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 业务量/容积（一个包装单位的体积，单位：立方米）
    /// </summary>
    public decimal? BusinessVolume { get; set; }

    /// <summary>
    /// 体积单位（字典 logistics_materials_unit_of_measure_code；DictValue=M3/L/ML 等；默认 M3）
    /// </summary>
    public string VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 大小/量纲（尺寸量纲或大小规格）
    /// </summary>
    public string? SizeDimension { get; set; } = string.Empty;

    /// <summary>
    /// 包装类型（字典 logistics_materials_material_type；DictValue=VERP 等；默认 VERP）
    /// </summary>
    public string PackagingType { get; set; } = string.Empty;

    /// <summary>
    /// 包装单位（字典 logistics_materials_unit_of_measure_code；DictValue=CAR/CT 等；默认 CAR）
    /// </summary>
    public string PackingUnit { get; set; } = string.Empty;

    /// <summary>
    /// 每包装数量（一个包装包含的基本单位数量）
    /// </summary>
    public decimal? QuantityPerPacking { get; set; }

    /// <summary>
    /// 包装规格（含多段规格说明，可超默认短串）
    /// </summary>
    public string? PackagingSpec { get; set; } = string.Empty;

    /// <summary>
    /// 包装描述（超长说明，可超默认短串）
    /// </summary>
    public string? PackagingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

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

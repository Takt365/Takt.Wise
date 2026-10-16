// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Quality
// 文件名称：TaktInspectionStandard.cs
// 功能描述：检验标准实体，定义IQC/IPQC/FQC的检验项目和标准
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Quality.Operation;

/// <summary>
/// 检验标准实体（IQC/IPQC/FQC通用）
/// </summary>
[SugarTable("takt_logistics_quality_inspection_standard", "检验标准表")]
[SugarIndex("ix_inspection_standard_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_inspection_standard_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_inspection_standard_is_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(StandardCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_quality_inspection_standard_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_inspection_standard_inspection_type", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(InspectionType), OrderByType.Asc, false)]
public class TaktInspectionStandard : TaktCompanyEntityBase
{
    /// <summary>
    /// 检验标准编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 检验标准编码规则生成并展示，非手输；单据类型菜单：检验标准）
    /// </summary>
    [SugarColumn(ColumnName = "standard_code", ColumnDescription = "检验标准编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string StandardCode { get; set; } = string.Empty;
    /// <summary>
    /// 检验标准名称
    /// </summary>
    [SugarColumn(ColumnName = "standard_name", ColumnDescription = "检验标准名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string StandardName { get; set; } = string.Empty;
    /// <summary>
    /// 检验类型（字典 logistics_quality_inspection_type）
    /// </summary>
    [SugarColumn(ColumnName = "inspection_type", ColumnDescription = "检验类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int InspectionType { get; set; } = 0;
    /// <summary>
    /// 物料类别编码
    /// </summary>
    [SugarColumn(ColumnName = "material_category_code", ColumnDescription = "物料类别编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCategoryCode { get; set; } = string.Empty;
    /// <summary>
    /// 物料类别名称
    /// </summary>
    [SugarColumn(ColumnName = "material_category_name", ColumnDescription = "物料类别名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string MaterialCategoryName { get; set; } = string.Empty;
    /// <summary>
    /// 抽样方案编码（选项 TaktSamplingSchemes/options；DictValue=SamplingSchemeCode）
    /// </summary>
    [SugarColumn(ColumnName = "sampling_scheme_code", ColumnDescription = "抽样方案编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? SamplingSchemeCode { get; set; }
    /// <summary>
    /// 抽样方案名称
    /// </summary>
    [SugarColumn(ColumnName = "sampling_scheme_name", ColumnDescription = "抽样方案名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? SamplingSchemeName { get; set; }
    /// <summary>
    /// 检验标准描述
    /// </summary>
    [SugarColumn(ColumnName = "standard_description", ColumnDescription = "检验标准描述", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? StandardDescription { get; set; }
    /// <summary>
    /// 检验标准状态（字典 logistics_quality_standard_status）
    /// </summary>
    [SugarColumn(ColumnName = "standard_status", ColumnDescription = "检验标准状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int StandardStatus { get; set; } = 0;

    /// <summary>
    /// 检验标准明细列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktInspectionStandardItem.InspectionStandardId))]
    public List<TaktInspectionStandardItem>? Items { get; set; }
}

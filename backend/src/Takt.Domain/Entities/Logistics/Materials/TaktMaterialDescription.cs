// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Materials
// 文件名称：TaktMaterialDescription.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt物料多语言描述实体
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Materials;

/// <summary>
/// Takt物料多语言描述实体（租户级）
/// 特例：继承组合2：无关联工厂、有语言
/// <para>业务唯一键（新增/更新匹配）：TenantCode+MaterialCode+CultureCode。</para>
/// </summary>
[SugarTable("takt_logistics_materials_material_description", "物料描述表")]
[SugarIndex("ix_takt_logistics_materials_material_description_tenant", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_material_description_unique", nameof(TenantCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, nameof(CultureCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_materials_material_description_material_code", nameof(TenantCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, false)]
public class TaktMaterialDescription : TaktTenantCultureEntityBase
{
    /// <summary>
    /// 物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;
    /// <summary>
    /// 物料描述
    /// </summary>
    [SugarColumn(ColumnName = "material_description", ColumnDescription = "物料描述", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    [SugarColumn(ColumnName = "material_specification", ColumnDescription = "物料规格", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? MaterialSpecification { get; set; }

    /// <summary>
    /// 物料型号
    /// </summary>
    [SugarColumn(ColumnName = "material_model", ColumnDescription = "物料型号", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? MaterialModel { get; set; }

    /// <summary>
    /// 物料长描述
    /// </summary>
    [SugarColumn(ColumnName = "material_long_description", ColumnDescription = "物料长描述", ColumnDataType = "nvarchar", Length = 255, IsNullable = true)]
    public string? MaterialLongDescription { get; set; }

}

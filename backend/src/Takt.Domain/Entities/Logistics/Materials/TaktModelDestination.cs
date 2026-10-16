// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Materials
// 文件名称：TaktModelDestination.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt型号目的地实体（组合4仅租户）；机型/型号与出货目的地关联
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Materials;

/// <summary>
/// Takt型号目的地实体（租户级；物料编码/名称、机种编码/名称、仕向地编码/名称）
/// 特例：继承组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase）
/// <para>业务唯一键（新增/更新匹配）：TenantCode+MaterialCode+ModelCode；DestinationCode 为业务字段，不参与唯一匹配。</para>
/// </summary>
[SugarTable("takt_logistics_materials_model_destination", "型号目的地表")]
[SugarIndex("ix_model_destination_tenant", nameof(TenantCode), OrderByType.Asc, false)]
[SugarIndex("ix_model_destination_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_model_destination_unique", nameof(TenantCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, nameof(ModelCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_materials_model_destination_order_num", nameof(TenantCode), OrderByType.Asc, nameof(SortOrder), OrderByType.Asc, false)]
public class TaktModelDestination : TaktTenantCoreEntityBase
{
    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    [SugarColumn(ColumnName = "material_description", ColumnDescription = "物料描述", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（40；前端表单选择编码规则后自动通过 TaktNumbering 机种编码规则生成并展示，非手输；单据类型菜单：机种仕向）
    /// </summary>
    [SugarColumn(ColumnName = "model_code", ColumnDescription = "机种编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称（回填：按 ModelCode 取物料描述表 culture_code=Z1）
    /// </summary>
    [SugarColumn(ColumnName = "model_name", ColumnDescription = "机种名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 仕向地编码（40；前端表单选择编码规则后自动通过 TaktNumbering 仕向地编码规则生成并展示，非手输；单据类型菜单：机种仕向）
    /// </summary>
    [SugarColumn(ColumnName = "destination_code", ColumnDescription = "仕向地编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string DestinationCode { get; set; } = string.Empty;

    /// <summary>
    /// 仕向地名称（80）
    /// </summary>
    [SugarColumn(ColumnName = "destination_name", ColumnDescription = "仕向地名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string DestinationName { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
}

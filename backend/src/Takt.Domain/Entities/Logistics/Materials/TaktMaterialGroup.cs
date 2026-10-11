// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Materials
// 文件名称：TaktMaterialGroup.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt物料组主数据实体（material_group），定义物料组层级与组织归属（与 TaktGeneralMaterial.MaterialGroup 对齐）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Materials;

/// <summary>
/// Takt物料组主数据实体（租户级）
/// 组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase；仅租户）
/// </summary>
[SugarTable("takt_logistics_materials_material_group", "物料组主数据表")]
[SugarIndex("ix_material_group_tenant", nameof(TenantCode), OrderByType.Asc, false)]
[SugarIndex("ix_material_group_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_materials_material_group_unique", nameof(TenantCode), OrderByType.Asc, nameof(MaterialGroupCode), OrderByType.Asc, true)]
public class TaktMaterialGroup : TaktTenantCoreEntityBase
{
    /// <summary>
    /// 物料组编码（与员工编码同长 6；租户内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "material_group_code", ColumnDescription = "物料组编码", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string MaterialGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料组名称（group_name）
    /// </summary>
    [SugarColumn(ColumnName = "material_group_name", ColumnDescription = "物料组名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string MaterialGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（sort；越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 物料组描述（description）
    /// </summary>
    [SugarColumn(ColumnName = "material_group_description", ColumnDescription = "物料组描述", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? MaterialGroupDescription { get; set; }
}

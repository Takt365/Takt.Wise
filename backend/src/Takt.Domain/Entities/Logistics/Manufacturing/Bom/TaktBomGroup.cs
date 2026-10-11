// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Bom
// 文件名称：TaktBomGroup.cs
// 创建时间：2026-10-08
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM组主数据实体，按类别（BOM、工艺）定义制造主数据业务组织分组
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM组主数据实体（公司级；按类别区分的 BOM/工艺业务组织分组）
/// </summary>
[SugarTable("takt_logistics_manufacturing_bom_group", "BOM组主数据表")]
[SugarIndex("ix_bom_group_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_bom_group_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_bom_group_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(BomGroupCategory), OrderByType.Asc, nameof(BomGroupCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_bom_group_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_bom_group_category", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(BomGroupCategory), OrderByType.Asc, false)]
public class TaktBomGroup : TaktCompanyEntityBase
{
    /// <summary>
    /// BOM组编码（与员工编码同长 6）
    /// </summary>
    [SugarColumn(ColumnName = "bom_group_code", ColumnDescription = "BOM组编码", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string BomGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// BOM组名称
    /// </summary>
    [SugarColumn(ColumnName = "bom_group_name", ColumnDescription = "BOM组名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string BomGroupName { get; set; } = string.Empty;

    /// <summary>
    /// BOM组描述
    /// </summary>
    [SugarColumn(ColumnName = "bom_group_description", ColumnDescription = "BOM组描述", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? BomGroupDescription { get; set; }

    /// <summary>
    /// BOM组类别（字典 logistics_manufacturing_bom_group_category；1=BOM 2=工艺）
    /// </summary>
    [SugarColumn(ColumnName = "bom_group_category", ColumnDescription = "BOM组类别", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int BomGroupCategory { get; set; } = 1;

    /// <summary>
    /// 联系电话
    /// </summary>
    [SugarColumn(ColumnName = "contact_phone", ColumnDescription = "联系电话", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? ContactPhone { get; set; }

    /// <summary>
    /// 联系邮箱
    /// </summary>
    [SugarColumn(ColumnName = "contact_email", ColumnDescription = "联系邮箱", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? ContactEmail { get; set; }

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    [SugarColumn(ColumnName = "is_built_in", ColumnDescription = "内置", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// BOM组状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "group_status", ColumnDescription = "BOM组状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int GroupStatus { get; set; } = 1;
}

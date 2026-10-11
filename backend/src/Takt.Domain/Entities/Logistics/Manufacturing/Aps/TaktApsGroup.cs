// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Aps
// 文件名称：TaktApsGroup.cs
// 创建时间：2026-10-08
// 创建人：Takt365(Cursor AI)
// 功能描述：APS组主数据实体，按类别（计划员、发布员、设备操作员、设备维护员）定义计划业务组织分组
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Aps;

/// <summary>
/// APS组主数据实体（公司级；按计划类别区分的业务组织分组）
/// </summary>
[SugarTable("takt_logistics_manufacturing_aps_group", "APS组主数据表")]
[SugarIndex("ix_aps_group_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_aps_group_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_aps_group_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(ApsGroupCategory), OrderByType.Asc, nameof(ApsGroupCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_aps_group_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_aps_group_category", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ApsGroupCategory), OrderByType.Asc, false)]
public class TaktApsGroup : TaktCompanyEntityBase
{
    /// <summary>
    /// APS组编码（与员工编码同长 6）
    /// </summary>
    [SugarColumn(ColumnName = "aps_group_code", ColumnDescription = "APS组编码", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string ApsGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// APS组名称
    /// </summary>
    [SugarColumn(ColumnName = "aps_group_name", ColumnDescription = "APS组名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string ApsGroupName { get; set; } = string.Empty;

    /// <summary>
    /// APS组描述
    /// </summary>
    [SugarColumn(ColumnName = "aps_group_description", ColumnDescription = "APS组描述", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? ApsGroupDescription { get; set; }

    /// <summary>
    /// APS组类别（字典 logistics_manufacturing_aps_group_category；1=计划员 2=发布员 3=设备操作员 4=设备维护员）
    /// </summary>
    [SugarColumn(ColumnName = "aps_group_category", ColumnDescription = "APS组类别", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ApsGroupCategory { get; set; } = 1;

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
    /// APS组状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "group_status", ColumnDescription = "APS组状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int GroupStatus { get; set; } = 1;
}

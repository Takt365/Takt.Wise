// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Sop
// 文件名称：TaktSopGroup.cs
// 创建时间：2026-10-08
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP组主数据实体，按类别（工位、文档、版本、确认、执行、扫码、ESD、安灯）定义 SOP 业务组织分组
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP组主数据实体（公司级；按 SOP 业务类别区分的组织分组）
/// </summary>
[SugarTable("takt_logistics_manufacturing_sop_sop_group", "SOP组主数据表")]
[SugarIndex("ix_sop_group_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sop_group_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_sop_sop_group_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(SopGroupCategory), OrderByType.Asc, nameof(SopGroupCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_sop_sop_group_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_sop_sop_group_category", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SopGroupCategory), OrderByType.Asc, false)]
public class TaktSopGroup : TaktCompanyEntityBase
{
    /// <summary>
    /// SOP组编码（与员工编码同长 6）
    /// </summary>
    [SugarColumn(ColumnName = "sop_group_code", ColumnDescription = "SOP组编码", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string SopGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// SOP组名称
    /// </summary>
    [SugarColumn(ColumnName = "sop_group_name", ColumnDescription = "SOP组名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string SopGroupName { get; set; } = string.Empty;

    /// <summary>
    /// SOP组描述
    /// </summary>
    [SugarColumn(ColumnName = "sop_group_description", ColumnDescription = "SOP组描述", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? SopGroupDescription { get; set; }

    /// <summary>
    /// SOP组类别（字典 logistics_manufacturing_sop_group_category；1=工位 2=文档 3=版本 4=确认 5=执行 6=扫码 7=ESD 8=安灯）
    /// </summary>
    [SugarColumn(ColumnName = "sop_group_category", ColumnDescription = "SOP组类别", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int SopGroupCategory { get; set; } = 1;

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
    /// SOP组状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "group_status", ColumnDescription = "SOP组状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int GroupStatus { get; set; } = 1;
}

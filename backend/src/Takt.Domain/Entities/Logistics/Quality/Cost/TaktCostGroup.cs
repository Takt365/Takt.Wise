// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Quality.Cost
// 文件名称：TaktCostGroup.cs
// 创建时间：2026-10-08
// 创建人：Takt365(Cursor AI)
// 功能描述：成本组主数据实体，按类别（记录组、废弃组、对应组）定义品质成本业务组织分组
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Quality.Cost;

/// <summary>
/// 成本组主数据实体（公司级；按品质成本业务类别区分的组织分组）
/// </summary>
[SugarTable("takt_logistics_quality_cost_cost_group", "成本组主数据表")]
[SugarIndex("ix_cost_group_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_cost_group_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_cost_cost_group_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(CostGroupCategory), OrderByType.Asc, nameof(CostGroupCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_quality_cost_cost_group_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_cost_cost_group_category", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CostGroupCategory), OrderByType.Asc, false)]
public class TaktCostGroup : TaktCompanyEntityBase
{
    /// <summary>
    /// 成本组编码（与员工编码同长 6）
    /// </summary>
    [SugarColumn(ColumnName = "cost_group_code", ColumnDescription = "成本组编码", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string CostGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本组名称
    /// </summary>
    [SugarColumn(ColumnName = "cost_group_name", ColumnDescription = "成本组名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string CostGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 成本组描述
    /// </summary>
    [SugarColumn(ColumnName = "cost_group_description", ColumnDescription = "成本组描述", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? CostGroupDescription { get; set; }

    /// <summary>
    /// 成本组类别（字典 logistics_quality_cost_group_category；1=记录组 2=废弃组 3=对应组）
    /// </summary>
    [SugarColumn(ColumnName = "cost_group_category", ColumnDescription = "成本组类别", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int CostGroupCategory { get; set; } = 1;

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
    /// 成本组状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "group_status", ColumnDescription = "成本组状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int GroupStatus { get; set; } = 1;
}

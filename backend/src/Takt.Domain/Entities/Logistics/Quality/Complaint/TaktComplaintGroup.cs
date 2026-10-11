// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Quality.Complaint
// 文件名称：TaktComplaintGroup.cs
// 创建时间：2026-10-08
// 创建人：Takt365(Cursor AI)
// 功能描述：客诉组主数据实体，按类别（责任组、处理组、改善组、评价组、调查组）定义客诉业务组织分组
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Quality.Complaint;

/// <summary>
/// 客诉组主数据实体（公司级；按客诉业务类别区分的组织分组）
/// </summary>
[SugarTable("takt_logistics_quality_complaint_complaint_group", "客诉组主数据表")]
[SugarIndex("ix_complaint_group_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_complaint_group_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_complaint_complaint_group_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(ComplaintGroupCategory), OrderByType.Asc, nameof(ComplaintGroupCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_quality_complaint_complaint_group_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_complaint_complaint_group_category", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ComplaintGroupCategory), OrderByType.Asc, false)]
public class TaktComplaintGroup : TaktCompanyEntityBase
{
    /// <summary>
    /// 客诉组编码（与员工编码同长 6）
    /// </summary>
    [SugarColumn(ColumnName = "complaint_group_code", ColumnDescription = "客诉组编码", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string ComplaintGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 客诉组名称
    /// </summary>
    [SugarColumn(ColumnName = "complaint_group_name", ColumnDescription = "客诉组名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string ComplaintGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 客诉组描述
    /// </summary>
    [SugarColumn(ColumnName = "complaint_group_description", ColumnDescription = "客诉组描述", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? ComplaintGroupDescription { get; set; }

    /// <summary>
    /// 客诉组类别（字典 logistics_quality_complaint_group_category；1=责任组 2=处理组 3=改善组 4=评价组 5=调查组）
    /// </summary>
    [SugarColumn(ColumnName = "complaint_group_category", ColumnDescription = "客诉组类别", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ComplaintGroupCategory { get; set; } = 1;

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
    /// 客诉组状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "group_status", ColumnDescription = "客诉组状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int GroupStatus { get; set; } = 1;
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Organization
// 文件名称：TaktEmployeePost.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：员工-岗位关联实体，记录员工与岗位的真实组织关系
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities.HumanResource.Personnel;

namespace Takt.Domain.Entities.HumanResource.Organization;

/// <summary>
/// 员工-岗位关联实体
/// 记录员工与岗位的真实组织关系（不包含代理）
/// </summary>
[SugarTable("takt_human_resource_organization_employee_post", "员工-岗位关联表")]
[SugarIndex("ix_employee_post_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_employee_post_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_employee_post_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, nameof(PostId), OrderByType.Asc, true)]
[SugarIndex("ix_employee_post_employee", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, false)]
[SugarIndex("ix_employee_post_post", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PostId), OrderByType.Asc, false)]
public class TaktEmployeePost : TaktCompanyEntityBase
{
    /// <summary>
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "employee_id", ColumnDescription = "员工ID", ColumnDataType = "bigint", IsNullable = false)]
    public long EmployeeId { get; set; }
    /// <summary>
    /// 岗位（选项 TaktPosts/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "post_id", ColumnDescription = "岗位ID", ColumnDataType = "bigint", IsNullable = false)]
    public long PostId { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 员工（多对一）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(EmployeeId))]
    public TaktEmployee? Employee { get; set; }

    /// <summary>
    /// 岗位（多对一）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(PostId))]
    public TaktPost? Post { get; set; }
}

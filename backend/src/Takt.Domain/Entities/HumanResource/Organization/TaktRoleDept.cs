// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Organization
// 文件名称：TaktRoleDept.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：角色-部门关联实体，用于自定义数据权限范围（DataScope=4）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities.Identity;

namespace Takt.Domain.Entities.HumanResource.Organization;

/// <summary>
/// 角色-部门关联实体
/// 当角色 DataScope=4（字典 sys_data_scope 自定义数据范围）时，定义角色可访问的部门数据范围
/// </summary>
[SugarTable("takt_human_resource_organization_role_dept", "角色-部门关联表")]
[SugarIndex("ix_role_dept_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_role_dept_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_role_dept_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(RoleId), OrderByType.Asc, nameof(DeptId), OrderByType.Asc, true)]
[SugarIndex("ix_role_dept_dept", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(DeptId), OrderByType.Asc, false)]
[SugarIndex("ix_role_dept_role", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(RoleId), OrderByType.Asc, false)]
public class TaktRoleDept : TaktCompanyEntityBase
{
    /// <summary>
    /// 角色（选项 TaktRoles/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "role_id", ColumnDescription = "角色ID", ColumnDataType = "bigint", IsNullable = false)]
    public long RoleId { get; set; }
    /// <summary>
    /// 部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
    /// </summary>
    [SugarColumn(ColumnName = "dept_id", ColumnDescription = "部门ID", ColumnDataType = "bigint", IsNullable = false)]
    public long DeptId { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 角色（多对一）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(RoleId), nameof(TaktRole.Id))]
    public TaktRole Role { get; set; } = null!;
    /// <summary>
    /// 部门（多对一）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(DeptId), nameof(TaktDept.Id))]
    public TaktDept Dept { get; set; } = null!;
}

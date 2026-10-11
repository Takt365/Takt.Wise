// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Organization
// 文件名称：TaktEmployeeDept.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：员工-部门关联实体，记录员工与部门的真实组织关系
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities.HumanResource.Personnel;

namespace Takt.Domain.Entities.HumanResource.Organization;

/// <summary>
/// 员工-部门关联实体
/// 记录员工与部门的真实组织关系（不包含代理）
/// </summary>
[SugarTable("takt_human_resource_organization_employee_dept", "员工-部门关联表")]
[SugarIndex("ix_employee_dept_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_employee_dept_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_employee_dept_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, nameof(DeptId), OrderByType.Asc, true)]
[SugarIndex("ix_employee_dept_dept", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(DeptId), OrderByType.Asc, false)]
[SugarIndex("ix_employee_dept_employee", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, false)]
public class TaktEmployeeDept : TaktCompanyEntityBase
{
    /// <summary>
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "employee_id", ColumnDescription = "员工ID", ColumnDataType = "bigint", IsNullable = false)]
    public long EmployeeId { get; set; }
    /// <summary>
    /// 部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
    /// </summary>
    [SugarColumn(ColumnName = "dept_id", ColumnDescription = "部门ID", ColumnDataType = "bigint", IsNullable = false)]
    public long DeptId { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 员工（多对一）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(EmployeeId))]
    public TaktEmployee? Employee { get; set; }

    /// <summary>
    /// 部门（多对一）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(DeptId))]
    public TaktDept? Dept { get; set; }
}

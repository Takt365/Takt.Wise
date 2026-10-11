// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Personnel
// 文件名称：TaktEmployeeReassignment.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：员工调动记录实体（人事-调动管理）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Personnel;

/// <summary>
/// 员工调动记录（审批单；审批态见基类 ApprovalStatus，字典 sys_approval_status）
/// </summary>
[SugarTable("takt_human_resource_personnel_employee_reassignment", "员工调动表")]
[SugarIndex("ix_employee_reassignment_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_employee_reassignment_employee", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, false)]
[SugarIndex("ix_employee_reassignment_approval", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ApprovalStatus), OrderByType.Asc, false)]
public class TaktEmployeeReassignment : TaktApprovalEntityBase
{
    /// <summary>
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "employee_id", ColumnDescription = "员工ID", ColumnDataType = "bigint", IsNullable = false)]
    public long EmployeeId { get; set; }
    /// <summary>
    /// 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    [SugarColumn(ColumnName = "employee_code", ColumnDescription = "员工编码", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string EmployeeCode { get; set; } = string.Empty;
    /// <summary>
    /// 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    [SugarColumn(ColumnName = "employee_name", ColumnDescription = "员工姓名", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string EmployeeName { get; set; } = string.Empty;
    /// <summary>
    /// 调动类型（字典 humanresource_personnel_reassignment_type；0=转岗 1=调岗）
    /// </summary>
    [SugarColumn(ColumnName = "reassignment_type", ColumnDescription = "调动类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ReassignmentType { get; set; }
    /// <summary>
    /// 调出部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
    /// </summary>
    [SugarColumn(ColumnName = "from_dept_id", ColumnDescription = "调出部门ID", ColumnDataType = "bigint", IsNullable = false)]
    public long FromDeptId { get; set; }
    /// <summary>
    /// 调出部门名称
    /// </summary>
    [SugarColumn(ColumnName = "from_dept_name", ColumnDescription = "调出部门名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string FromDeptName { get; set; } = string.Empty;
    /// <summary>
    /// 调出岗位（选项 TaktPosts/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "from_post_id", ColumnDescription = "调出岗位ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? FromPostId { get; set; }
    /// <summary>
    /// 调出岗位名称
    /// </summary>
    [SugarColumn(ColumnName = "from_post_name", ColumnDescription = "调出岗位名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? FromPostName { get; set; }
    /// <summary>
    /// 调入部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
    /// </summary>
    [SugarColumn(ColumnName = "to_dept_id", ColumnDescription = "调入部门ID", ColumnDataType = "bigint", IsNullable = false)]
    public long ToDeptId { get; set; }
    /// <summary>
    /// 调入部门名称
    /// </summary>
    [SugarColumn(ColumnName = "to_dept_name", ColumnDescription = "调入部门名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string ToDeptName { get; set; } = string.Empty;
    /// <summary>
    /// 调入岗位（选项 TaktPosts/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "to_post_id", ColumnDescription = "调入岗位ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? ToPostId { get; set; }
    /// <summary>
    /// 调入岗位名称
    /// </summary>
    [SugarColumn(ColumnName = "to_post_name", ColumnDescription = "调入岗位名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? ToPostName { get; set; }
    /// <summary>
    /// 生效日期
    /// </summary>
    [SugarColumn(ColumnName = "effective_date", ColumnDescription = "生效日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? EffectiveDate { get; set; }
    /// <summary>
    /// 调动原因
    /// </summary>
    [SugarColumn(ColumnName = "reason", ColumnDescription = "调动原因", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? Reason { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 员工主档（多对一）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(EmployeeId))]
    public TaktEmployee? Employee { get; set; }
}

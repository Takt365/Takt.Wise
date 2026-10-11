// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Personnel
// 文件名称：TaktEmployeeJoined.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：员工入职上岗办理记录（Joined=实际上班，主子表之子表）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Personnel;

/// <summary>
/// 员工入职上岗办理记录（审批单，Joined=实际上班；审批态见基类 ApprovalStatus，字典 sys_approval_status）
/// </summary>
[SugarTable("takt_human_resource_personnel_employee_joined", "员工入职上岗表")]
[SugarIndex("ix_employee_joined_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_employee_joined_employee", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, false)]
[SugarIndex("ix_employee_joined_approval", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ApprovalStatus), OrderByType.Asc, false)]
public class TaktEmployeeJoined : TaktApprovalEntityBase
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
    /// 入职待办（选项 TaktEmployeeOnboardings/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "onboarding_id", ColumnDescription = "入职待办ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? OnboardingId { get; set; }
    /// <summary>
    /// 实际上岗日期（JoinedDate：我去上班）
    /// </summary>
    [SugarColumn(ColumnName = "joined_date", ColumnDescription = "实际上岗日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime JoinedDate { get; set; }
    /// <summary>
    /// 试用期结束日期
    /// </summary>
    [SugarColumn(ColumnName = "probation_end_date", ColumnDescription = "试用期结束日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ProbationEndDate { get; set; }
    /// <summary>
    /// 转正日期
    /// </summary>
    [SugarColumn(ColumnName = "regular_date", ColumnDescription = "转正日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? RegularDate { get; set; }
    /// <summary>
    /// 上岗部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
    /// </summary>
    [SugarColumn(ColumnName = "dept_id", ColumnDescription = "上岗部门ID", ColumnDataType = "bigint", IsNullable = false)]
    public long DeptId { get; set; }
    /// <summary>
    /// 上岗部门名称
    /// </summary>
    [SugarColumn(ColumnName = "dept_name", ColumnDescription = "上岗部门名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string DeptName { get; set; } = string.Empty;
    /// <summary>
    /// 上岗岗位（选项 TaktPosts/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "post_id", ColumnDescription = "上岗岗位ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? PostId { get; set; }
    /// <summary>
    /// 上岗岗位名称
    /// </summary>
    [SugarColumn(ColumnName = "post_name", ColumnDescription = "上岗岗位名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? PostName { get; set; }
    /// <summary>
    /// 职务/职称
    /// </summary>
    [SugarColumn(ColumnName = "job_title", ColumnDescription = "职务", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? JobTitle { get; set; }
    /// <summary>
    /// 工作性质（字典 humanresource_personnel_employee_work_nature；0=全职 1=兼职 2=实习 3=外包 4=其他）
    /// </summary>
    [SugarColumn(ColumnName = "work_nature", ColumnDescription = "工作性质", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int WorkNature { get; set; }
    /// <summary>
    /// 任职类型（字典 humanresource_personnel_employee_employment_type；0=主职 1=兼职 2=借调 3=挂职）
    /// </summary>
    [SugarColumn(ColumnName = "employment_type", ColumnDescription = "任职类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int EmploymentType { get; set; }
    /// <summary>
    /// 直属上级（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "direct_manager_id", ColumnDescription = "直属上级员工ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? DirectManagerId { get; set; }
    /// <summary>
    /// 直属上级姓名
    /// </summary>
    [SugarColumn(ColumnName = "direct_manager_name", ColumnDescription = "直属上级姓名", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? DirectManagerName { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 员工主档（多对一）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(EmployeeId))]
    public TaktEmployee? Employee { get; set; }

    /// <summary>
    /// 直属上级员工（多对一）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(DirectManagerId))]
    public TaktEmployee? DirectManager { get; set; }
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Attendance
// 文件名称：TaktLeave.cs
// 创建时间：2026-06-04
// 创建人：Takt365(Cursor AI)
// 功能描述：请假实体，与工作流请假流程关联；BusinessType=Leave、BusinessKey=本表 Id
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Attendance;

/// <summary>
/// 请假实体。FlowInstanceId 由业务在发起流程后写入；流程引擎通过 BusinessKey/BusinessType 与请假模块对接。
/// </summary>
[SugarTable("takt_human_resource_attendance_leave", "请假信息表")]
[SugarIndex("ix_leave_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_leave_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_leave_employee_start_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, nameof(StartDate), OrderByType.Asc, false)]
[SugarIndex("ix_leave_employee_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, false)]
[SugarIndex("ix_leave_dept_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(DeptId), OrderByType.Asc, false)]
[SugarIndex("ix_leave_flow_instance_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FlowInstanceId), OrderByType.Asc, false)]
[SugarIndex("ix_leave_leave_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(LeaveStatus), OrderByType.Asc, false)]
[SugarIndex("ix_leave_start_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(StartDate), OrderByType.Asc, false)]
public class TaktLeave : TaktApprovalEntityBase
{
    /// <summary>
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "employee_id", ColumnDescription = "员工ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }
    /// <summary>
    /// 员工姓名（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "employee_name", ColumnDescription = "员工姓名", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string EmployeeName { get; set; } = string.Empty;
    /// <summary>
    /// 部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "dept_id", ColumnDescription = "部门ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }
    /// <summary>
    /// 部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    [SugarColumn(ColumnName = "dept_name", ColumnDescription = "部门名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? DeptName { get; set; }
    /// <summary>
    /// 请假类型（字典 sys_leave_type；列存 DictValue）
    /// </summary>
    [SugarColumn(ColumnName = "leave_type", ColumnDescription = "请假类型", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string LeaveType { get; set; } = string.Empty;
    /// <summary>
    /// 开始日期
    /// </summary>
    [SugarColumn(ColumnName = "start_date", ColumnDescription = "开始日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime StartDate { get; set; }
    /// <summary>
    /// 结束日期
    /// </summary>
    [SugarColumn(ColumnName = "end_date", ColumnDescription = "结束日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime EndDate { get; set; }
    /// <summary>
    /// 请假事由
    /// </summary>
    [SugarColumn(ColumnName = "reason", ColumnDescription = "请假事由", ColumnDataType = "nvarchar", Length = 500, IsNullable = false, DefaultValue = "")]
    public string Reason { get; set; } = string.Empty;
    /// <summary>
    /// 证明附件（JSON 列表，由 TaktFile 统一上传）
    /// </summary>
    [SugarColumn(ColumnName = "attachments", ColumnDescription = "证明附件JSON", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? Attachments { get; set; }
    /// <summary>
    /// 经办人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "handling_by", ColumnDescription = "经办人ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long HandlingBy { get; set; }
    /// <summary>
    /// 经办人名称（冗余：按 HandlingBy 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "handling_by_name", ColumnDescription = "经办人名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? HandlingByName { get; set; }
    /// <summary>
    /// 经办时间
    /// </summary>
    [SugarColumn(ColumnName = "handling_at", ColumnDescription = "经办时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? HandlingAt { get; set; }
    /// <summary>
    /// 经办备注
    /// </summary>
    [SugarColumn(ColumnName = "handling_comment", ColumnDescription = "经办备注", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? HandlingComment { get; set; }
    /// <summary>
    /// 请假状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
    /// </summary>
    [SugarColumn(ColumnName = "leave_status", ColumnDescription = "请假状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LeaveStatus { get; set; }
}

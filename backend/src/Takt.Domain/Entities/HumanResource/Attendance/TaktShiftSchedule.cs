// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Attendance
// 文件名称：TaktShiftSchedule.cs
// 创建时间：2026-06-04
// 创建人：Takt365(Cursor AI)
// 功能描述：排班计划（按排班类别区分部门/人员）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Attendance;

/// <summary>
/// 排班计划（ScheduleType=0 部门排班时 DeptId 必填；ScheduleType=1 人员排班时 EmployeeId 必填）
/// </summary>
[SugarTable("takt_human_resource_attendance_shift_schedule", "排班信息表")]
[SugarIndex("ix_shift_schedule_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_shift_schedule_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_shift_schedule_schedule_type", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ScheduleType), OrderByType.Asc, false)]
[SugarIndex("ix_shift_schedule_employee_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, false)]
[SugarIndex("ix_shift_schedule_dept_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(DeptId), OrderByType.Asc, false)]
[SugarIndex("ix_shift_schedule_schedule_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ScheduleDate), OrderByType.Asc, false)]
[SugarIndex("ix_shift_schedule_shift_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ShiftId), OrderByType.Asc, false)]
public class TaktShiftSchedule : TaktCompanyEntityBase
{
    /// <summary>
    /// 排班类别（字典 humanresource_attendance_schedule_type；0=部门 1=人员）
    /// </summary>
    [SugarColumn(ColumnName = "schedule_type", ColumnDescription = "排班类别", ColumnDataType = "tinyint", IsNullable = false)]
    public int ScheduleType { get; set; }
    /// <summary>
    /// 部门（选项 TaktDepts/tree-options；DictValue=Id；ScheduleType=0 时必填）
    /// </summary>
    [SugarColumn(ColumnName = "dept_id", ColumnDescription = "部门ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }
    /// <summary>
    /// 部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    [SugarColumn(ColumnName = "dept_name", ColumnDescription = "部门名称", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? DeptName { get; set; }
    /// <summary>
    /// 员工（选项 TaktEmployees/options；DictValue=Id；ScheduleType=1 时必填）
    /// </summary>
    [SugarColumn(ColumnName = "employee_id", ColumnDescription = "员工ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }
    /// <summary>
    /// 员工名称（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "employee_name", ColumnDescription = "员工名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? EmployeeName { get; set; }
    /// <summary>
    /// 排班日期
    /// </summary>
    [SugarColumn(ColumnName = "schedule_date", ColumnDescription = "排班日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime ScheduleDate { get; set; }
    /// <summary>
    /// 班次（选项 TaktWorkShifts/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "shift_id", ColumnDescription = "班次ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ShiftId { get; set; }
}

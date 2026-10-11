// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Attendance
// 文件名称：TaktOvertimeItem.cs
// 创建时间：2026-06-04
// 创建人：Takt365(Cursor AI)
// 功能描述：加班申请明细（记录每个人员的加班信息）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Attendance;

/// <summary>
/// 加班申请明细（一次申请可包含多个人员）
/// </summary>
[SugarTable("takt_human_resource_attendance_overtime_item", "加班明细表")]
[SugarIndex("ix_overtime_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_overtime_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_overtime_item_request_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OvertimeId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
[SugarIndex("ix_overtime_item_overtime_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OvertimeId), OrderByType.Asc, false)]
[SugarIndex("ix_overtime_item_employee_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, false)]
public class TaktOvertimeItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 加班申请单（关联 TaktOvertime.Id，主子表关系）
    /// </summary>
    [SugarColumn(ColumnName = "overtime_id", ColumnDescription = "加班申请单ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OvertimeId { get; set; }
    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; }
    /// <summary>
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "employee_id", ColumnDescription = "员工ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }
    /// <summary>
    /// 员工姓名
    /// </summary>
    [SugarColumn(ColumnName = "employee_name", ColumnDescription = "员工姓名", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string EmployeeName { get; set; } = string.Empty;
    /// <summary>
    /// 计划加班小时数
    /// </summary>
    [SugarColumn(ColumnName = "planned_hours", ColumnDescription = "计划小时数", ColumnDataType = "decimal", Length = 8, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal PlannedHours { get; set; }
    /// <summary>
    /// 实际加班开始时间
    /// </summary>
    [SugarColumn(ColumnName = "actual_start_time", ColumnDescription = "实际开始时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ActualStartTime { get; set; }
    /// <summary>
    /// 实际加班结束时间
    /// </summary>
    [SugarColumn(ColumnName = "actual_end_time", ColumnDescription = "实际结束时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ActualEndTime { get; set; }
    /// <summary>
    /// 实际加班小时数
    /// </summary>
    [SugarColumn(ColumnName = "actual_hours", ColumnDescription = "实际小时数", ColumnDataType = "decimal", Length = 8, DecimalDigits = 2, IsNullable = true)]
    public decimal? ActualHours { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;


// ========================================
    // 导航属性区域
    // ========================================
    /// <summary>
    /// 加班主表
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(OvertimeId))]
    public TaktOvertime? Overtime { get; set; }
}

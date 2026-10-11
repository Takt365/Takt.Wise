// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Attendance
// 文件名称：TaktOvertime.cs
// 创建时间：2026-06-04
// 创建人：Takt365(Cursor AI)
// 功能描述：加班申请/登记主表
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Attendance;

/// <summary>
/// 加班申请（时长与状态由业务维护，可与工作流扩展对接）
/// </summary>
[SugarTable("takt_human_resource_attendance_overtime", "加班信息表")]
[SugarIndex("ix_overtime_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_overtime_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_overtime_dept_date_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(DeptId), OrderByType.Asc, nameof(OvertimeDate), OrderByType.Asc, true)]
[SugarIndex("ix_overtime_dept_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(DeptId), OrderByType.Asc, false)]
[SugarIndex("ix_overtime_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OvertimeDate), OrderByType.Asc, false)]
[SugarIndex("ix_overtime_type", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OvertimeType), OrderByType.Asc, false)]
[SugarIndex("ix_overtime_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OvertimeStatus), OrderByType.Asc, false)]
[SugarIndex("ix_overtime_flow_instance_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FlowInstanceId), OrderByType.Asc, false)]
public class TaktOvertime : TaktApprovalEntityBase
{
    /// <summary>
    /// 部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "dept_id", ColumnDescription = "部门ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DeptId { get; set; }
    /// <summary>
    /// 部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    [SugarColumn(ColumnName = "dept_name", ColumnDescription = "部门名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? DeptName { get; set; }
    /// <summary>
    /// 加班归属日期
    /// </summary>
    [SugarColumn(ColumnName = "overtime_date", ColumnDescription = "加班日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime OvertimeDate { get; set; }
    /// <summary>
    /// 计划加班开始时间
    /// </summary>
    [SugarColumn(ColumnName = "planned_start_time", ColumnDescription = "计划开始时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime PlannedStartTime { get; set; }
    /// <summary>
    /// 计划加班结束时间
    /// </summary>
    [SugarColumn(ColumnName = "planned_end_time", ColumnDescription = "计划结束时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime PlannedEndTime { get; set; }
    /// <summary>
    /// 加班总人数
    /// </summary>
    [SugarColumn(ColumnName = "total_employees", ColumnDescription = "加班总人数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TotalEmployees { get; set; }
    /// <summary>
    /// 计划加班总小时数
    /// </summary>
    [SugarColumn(ColumnName = "total_planned_hours", ColumnDescription = "计划总小时数", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TotalPlannedHours { get; set; }
    /// <summary>
    /// 实际加班总小时数
    /// </summary>
    [SugarColumn(ColumnName = "total_actual_hours", ColumnDescription = "实际总小时数", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TotalActualHours { get; set; }
    /// <summary>
    /// 加班类型（字典 humanresource_attendance_overtime_type；0=工作日加班 1=休息日加班 2=法定节假日加班）
    /// </summary>
    [SugarColumn(ColumnName = "overtime_type", ColumnDescription = "加班类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int OvertimeType { get; set; }
    /// <summary>
    /// 加班原因
    /// </summary>
    [SugarColumn(ColumnName = "reason", ColumnDescription = "加班原因", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? Reason { get; set; }
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
    /// 加班状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
    /// </summary>
    [SugarColumn(ColumnName = "overtime_status", ColumnDescription = "加班状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int OvertimeStatus { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================
    /// <summary>
    /// 加班明细列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktOvertimeItem.OvertimeId))]
    public List<TaktOvertimeItem>? Items { get; set; }
}

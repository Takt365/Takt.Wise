// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcExecutionTask.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：工程变更执行任务（通知确认后创建，部门执行监测）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 完成时间
/// </summary>
[SugarTable("takt_logistics_manufacturing_ec_execution_task", "工程变更执行任务表")]
[SugarIndex("ix_ec_execution_task_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_ec_execution_task_notification_dept_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EcNotificationId), OrderByType.Asc, nameof(DeptCode), OrderByType.Asc, true)]
[SugarIndex("ix_ec_execution_task_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_ec_execution_task_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(TaskStatus), OrderByType.Asc, false)]
[SugarIndex("ix_ec_execution_task_due_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(DueDate), OrderByType.Asc, false)]
public class TaktEcExecutionTask : TaktCompanyEntityBase
{
    /// <summary>
    /// 通知单 ID
    /// </summary>
    [SugarColumn(ColumnName = "ec_notification_id", ColumnDescription = "通知单ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcNotificationId { get; set; }

    /// <summary>
    /// 技术课主表 ID（TaktEcGijutsu 主键）
    /// </summary>
    [SugarColumn(ColumnName = "ec_gijutsu_id", ColumnDescription = "技术课主表ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcGijutsuId { get; set; }
    /// <summary>
    /// 设变单号（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "ec_code", ColumnDescription = "设变单号", ColumnDataType = "varchar", Length = 40, IsNullable = false)]
    public string EcCode { get; set; } = string.Empty;
    /// <summary>
    /// 关联设变部门行 ID（TaktEcSeikan/Mp 等 8 张部门执行表主键）
    /// </summary>
    [SugarColumn(ColumnName = "ec_exec_id", ColumnDescription = "设变部门行ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcExecId { get; set; }
    /// <summary>
    /// 设变明细 ID（可选）
    /// </summary>
    [SugarColumn(ColumnName = "ec_detail_id", ColumnDescription = "设变明细ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcDetailId { get; set; }
    /// <summary>
    /// 责任部门编码（TaktDept.DeptCode，如 D0710）
    /// </summary>
    [SugarColumn(ColumnName = "dept_code", ColumnDescription = "责任部门编码", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string DeptCode { get; set; } = string.Empty;
    /// <summary>
    /// 责任部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
    /// </summary>
    [SugarColumn(ColumnName = "dept_name", ColumnDescription = "责任部门名称", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? DeptName { get; set; }
    /// <summary>
    /// 任务标题
    /// </summary>
    [SugarColumn(ColumnName = "task_title", ColumnDescription = "任务标题", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string TaskTitle { get; set; } = string.Empty;
    /// <summary>
    /// 任务状态（0待执行 1执行中 2已完成 3阻塞 4超时）
    /// </summary>
    [SugarColumn(ColumnName = "task_status", ColumnDescription = "任务状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TaskStatus { get; set; }
    /// <summary>
    /// 进度百分比 0-100
    /// </summary>
    [SugarColumn(ColumnName = "progress_percent", ColumnDescription = "进度百分比", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ProgressPercent { get; set; }
    /// <summary>
    /// 截止日期
    /// </summary>
    [SugarColumn(ColumnName = "due_date", ColumnDescription = "截止日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? DueDate { get; set; }
    /// <summary>
    /// 最近进度说明
    /// </summary>
    [SugarColumn(ColumnName = "last_progress_remark", ColumnDescription = "最近进度说明", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? LastProgressRemark { get; set; }
    /// <summary>
    /// 完成时间
    /// </summary>
    [SugarColumn(ColumnName = "completed_at", ColumnDescription = "完成时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? CompletedAt { get; set; }
}

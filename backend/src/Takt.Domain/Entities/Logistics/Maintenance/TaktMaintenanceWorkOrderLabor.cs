// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Maintenance
// 文件名称：TaktMaintenanceWorkOrderLabor.cs
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：维护工单报工明细实体，记录维护人员工时与人工成本
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Maintenance;

/// <summary>
/// 维护工单报工明细实体（主子表：挂载于维护工单）
/// </summary>
[SugarTable("takt_logistics_maintenance_work_order_labor", "维护工单报工表")]
[SugarIndex("ix_maintenance_work_order_labor_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_maintenance_work_order_labor_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_maintenance_work_order_labor_order_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaintenanceWorkOrderId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, nameof(EmployeeCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_maintenance_work_order_labor_work_order_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaintenanceWorkOrderId), OrderByType.Asc, false)]
public class TaktMaintenanceWorkOrderLabor : TaktCompanyEntityBase
{
    /// <summary>
    /// 维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_work_order_id", ColumnDescription = "维护工单ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 维护工单号（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "work_order_code", ColumnDescription = "维护工单号", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（步长10：10/20/30…）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "10")]
    public int LineNumber { get; set; } = 10;

    /// <summary>
    /// 员工ID（序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "employee_id", ColumnDescription = "员工ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工编码
    /// </summary>
    [SugarColumn(ColumnName = "employee_code", ColumnDescription = "员工编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string EmployeeCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工姓名（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "employee_name", ColumnDescription = "员工姓名", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? EmployeeName { get; set; }

    /// <summary>
    /// 报工日期
    /// </summary>
    [SugarColumn(ColumnName = "work_date", ColumnDescription = "报工日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime WorkDate { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    [SugarColumn(ColumnName = "start_time", ColumnDescription = "开始时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    [SugarColumn(ColumnName = "end_time", ColumnDescription = "结束时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 工时（小时）
    /// </summary>
    [SugarColumn(ColumnName = "work_hours", ColumnDescription = "工时", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal WorkHours { get; set; } = 0;

    /// <summary>
    /// 小时费率
    /// </summary>
    [SugarColumn(ColumnName = "hourly_rate", ColumnDescription = "小时费率", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal HourlyRate { get; set; } = 0;

    /// <summary>
    /// 人工成本
    /// </summary>
    [SugarColumn(ColumnName = "labor_cost", ColumnDescription = "人工成本", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal LaborCost { get; set; } = 0;

    /// <summary>
    /// 作业描述
    /// </summary>
    [SugarColumn(ColumnName = "operation_description", ColumnDescription = "作业描述", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? OperationDescription { get; set; }

    /// <summary>
    /// 报工确认状态（0=待确认，1=已确认）
    /// </summary>
    [SugarColumn(ColumnName = "confirmation_status", ColumnDescription = "报工确认状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ConfirmationStatus { get; set; } = 0;

    /// <summary>
    /// 确认时间
    /// </summary>
    [SugarColumn(ColumnName = "confirmed_at", ColumnDescription = "确认时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ConfirmedAt { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 维护工单（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(MaintenanceWorkOrderId))]
    public TaktMaintenanceWorkOrder? MaintenanceWorkOrder { get; set; }
}

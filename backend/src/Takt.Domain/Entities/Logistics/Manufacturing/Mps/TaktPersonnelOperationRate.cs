#nullable enable
// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Mps
// 文件名称：TaktPersonnelOperationRate.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：人员稼动率实体（生产线人员作业效率记录）
// 计算公式：人员稼动率(%) = 在岗作业时间 ÷ 出勤时间 × 100%（在岗作业率）
// 辅助关系：出勤时间为计划工作时间（含休息、待命）；空闲时间为等料、设备调试等非作业时间
// 良品率(%) = 合格品数量 ÷ 实际产量 × 100%
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Mps;

/// <summary>
/// 人员稼动率实体（生产线人员作业效率记录）
/// 人员稼动率(%) = 在岗作业时间 ÷ 出勤时间 × 100%（在岗作业率）。
/// </summary>
[SugarTable("takt_logistics_manufacturing_mps_personnel_operation_rate", "人员稼动率表")]
[SugarIndex("ix_personnel_operation_rate_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_personnel_operation_rate_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_mps_personnel_operation_rate_por_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(TeamCode), OrderByType.Asc, nameof(TimeCategory), OrderByType.Asc, nameof(StartDate), OrderByType.Asc, nameof(ShiftNo), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_mps_personnel_operation_rate_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_mps_personnel_operation_rate_team_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(TeamCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_mps_personnel_operation_rate_start_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(StartDate), OrderByType.Asc, false)]
public class TaktPersonnelOperationRate : TaktCompanyEntityBase
{

    /// <summary>
    /// 时间类别（1=天，2=周，3=月）
    /// </summary>
    [SugarColumn(ColumnName = "time_category", ColumnDescription = "时间类别", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int TimeCategory { get; set; } = 1;

    /// <summary>
    /// 开始日期
    /// </summary>
    [SugarColumn(ColumnName = "start_date", ColumnDescription = "开始日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 结束日期
    /// </summary>
    [SugarColumn(ColumnName = "end_date", ColumnDescription = "结束日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 周数（1-53）
    /// </summary>
    [SugarColumn(ColumnName = "week_number", ColumnDescription = "周数", ColumnDataType = "int", IsNullable = true)]
    public int? WeekNumber { get; set; }

    /// <summary>
    /// 月份（1-12）
    /// </summary>
    [SugarColumn(ColumnName = "month_number", ColumnDescription = "月份", ColumnDataType = "int", IsNullable = true)]
    public int? MonthNumber { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "team_code", ColumnDescription = "生产班组", ColumnDataType = "nvarchar", Length = 8, IsNullable = false)]
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组名称
    /// </summary>
    [SugarColumn(ColumnName = "team_code_name", ColumnDescription = "生产班组名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? TeamCodeName { get; set; }

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    [SugarColumn(ColumnName = "shift_no", ColumnDescription = "班次", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ShiftNo { get; set; } = 1;

    /// <summary>
    /// 计划直接人员数量
    /// </summary>
    [SugarColumn(ColumnName = "planned_direct_personnel_count", ColumnDescription = "计划直接人员数量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PlannedDirectPersonnelCount { get; set; } = 0;

    /// <summary>
    /// 实际直接人员数量
    /// </summary>
    [SugarColumn(ColumnName = "actual_direct_personnel_count", ColumnDescription = "实际直接人员数量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ActualDirectPersonnelCount { get; set; } = 0;

    /// <summary>
    /// 计划间接人员数量
    /// </summary>
    [SugarColumn(ColumnName = "planned_indirect_personnel_count", ColumnDescription = "计划间接人员数量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PlannedIndirectPersonnelCount { get; set; } = 0;

    /// <summary>
    /// 实际间接人员数量
    /// </summary>
    [SugarColumn(ColumnName = "actual_indirect_personnel_count", ColumnDescription = "实际间接人员数量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ActualIndirectPersonnelCount { get; set; } = 0;

    /// <summary>
    /// 出勤时间（分钟）。员工在公司的计划工作时间，含休息、待命等。
    /// </summary>
    [SugarColumn(ColumnName = "planned_work_time", ColumnDescription = "出勤时间(分钟)", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal PlannedWorkTime { get; set; } = 0;

    /// <summary>
    /// 在岗作业时间（分钟）。员工实际在工位上执行生产任务的时间。
    /// </summary>
    [SugarColumn(ColumnName = "actual_work_time", ColumnDescription = "在岗作业时间(分钟)", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ActualWorkTime { get; set; } = 0;

    /// <summary>
    /// 休息时间（分钟）
    /// </summary>
    [SugarColumn(ColumnName = "break_time", ColumnDescription = "休息时间(分钟)", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal BreakTime { get; set; } = 0;

    /// <summary>
    /// 空闲时间（分钟）。等料、设备调试等非作业时间。
    /// </summary>
    [SugarColumn(ColumnName = "idle_time", ColumnDescription = "空闲时间(分钟)", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal IdleTime { get; set; } = 0;

    /// <summary>
    /// 人员稼动率（%）。计算公式：在岗作业时间 ÷ 出勤时间 × 100%（在岗作业率）。
    /// </summary>
    [SugarColumn(ColumnName = "personnel_operation_rate", ColumnDescription = "人员稼动率(%)", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal PersonnelOperationRate { get; set; } = 0;

    /// <summary>
    /// 计划产量
    /// </summary>
    [SugarColumn(ColumnName = "planned_output", ColumnDescription = "计划产量", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal PlannedOutput { get; set; } = 0;

    /// <summary>
    /// 实际产量
    /// </summary>
    [SugarColumn(ColumnName = "actual_output", ColumnDescription = "实际产量", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ActualOutput { get; set; } = 0;

    /// <summary>
    /// 合格品数量
    /// </summary>
    [SugarColumn(ColumnName = "qualified_quantity", ColumnDescription = "合格品数量", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal QualifiedQuantity { get; set; } = 0;

    /// <summary>
    /// 不良品数量
    /// </summary>
    [SugarColumn(ColumnName = "defective_quantity", ColumnDescription = "不良品数量", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal DefectiveQuantity { get; set; } = 0;

    /// <summary>
    /// 良品率（%）
    /// </summary>
    [SugarColumn(ColumnName = "yield_rate", ColumnDescription = "良品率(%)", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal YieldRate { get; set; } = 0;

    /// <summary>
    /// 工作效率（%）
    /// </summary>
    [SugarColumn(ColumnName = "work_efficiency", ColumnDescription = "工作效率(%)", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal WorkEfficiency { get; set; } = 0;

    /// <summary>
    /// 空闲原因类型（1=缺料，2=设备故障，3=换型调试，4=人员调配，5=其他）
    /// </summary>
    [SugarColumn(ColumnName = "idle_reason_type", ColumnDescription = "空闲原因类型", ColumnDataType = "int", IsNullable = true)]
    public int? IdleReasonType { get; set; }

    /// <summary>
    /// 空闲原因描述
    /// </summary>
    [SugarColumn(ColumnName = "idle_reason", ColumnDescription = "空闲原因描述", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? IdleReason { get; set; }

    /// <summary>
    /// 加班时间（分钟）
    /// </summary>
    [SugarColumn(ColumnName = "overtime_hours", ColumnDescription = "加班时间(分钟)", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal OvertimeHours { get; set; } = 0;

    /// <summary>
    /// 班组长（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "team_leader", ColumnDescription = "班组长", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? TeamLeader { get; set; }

    /// <summary>
    /// 主管（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "supervisor", ColumnDescription = "主管", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? Supervisor { get; set; }

    /// <summary>
    /// 状态（0=正常，1=停用）
    /// </summary>
    [SugarColumn(ColumnName = "rate_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int RateStatus { get; set; } = 0;
}

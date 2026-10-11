// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Performance
// 文件名称：TaktPerfObjective.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：绩效目标实体（Perf 标识），对应菜单 performance/objective
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Performance;

/// <summary>
/// 员工绩效目标（审批态见基类 ApprovalStatus，字典 sys_approval_status）
/// </summary>
[SugarTable("takt_human_resource_perf_objective", "绩效目标表")]
[SugarIndex("ix_perf_objective_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_perf_objective_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_perf_objective_employee_period", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, nameof(ObjectivePeriod), OrderByType.Asc, false)]
public class TaktPerfObjective : TaktApprovalEntityBase
{
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
    /// 方案指标（选项 TaktPerfSchemes/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "scheme_metric_id", ColumnDescription = "方案指标ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SchemeMetricId { get; set; }
    /// <summary>
    /// 目标周期（如 2026-Q1、2026-Annual）
    /// </summary>
    [SugarColumn(ColumnName = "objective_period", ColumnDescription = "目标周期", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string ObjectivePeriod { get; set; } = string.Empty;
    /// <summary>
    /// 目标描述
    /// </summary>
    [SugarColumn(ColumnName = "objective_description", ColumnDescription = "目标描述", ColumnDataType = "nvarchar", Length = 500, IsNullable = false)]
    public string ObjectiveDescription { get; set; } = string.Empty;
    /// <summary>
    /// 目标值
    /// </summary>
    [SugarColumn(ColumnName = "target_value", ColumnDescription = "目标值", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TargetValue { get; set; }
    /// <summary>
    /// 实际完成值
    /// </summary>
    [SugarColumn(ColumnName = "actual_value", ColumnDescription = "实际完成值", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ActualValue { get; set; }
    /// <summary>
    /// 完成百分比（%）
    /// </summary>
    [SugarColumn(ColumnName = "completion_percentage", ColumnDescription = "完成百分比", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal CompletionPercentage { get; set; }
    /// <summary>
    /// 目标权重（%）
    /// </summary>
    [SugarColumn(ColumnName = "objective_weight", ColumnDescription = "目标权重", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ObjectiveWeight { get; set; }
    /// <summary>
    /// 开始日期
    /// </summary>
    [SugarColumn(ColumnName = "start_date", ColumnDescription = "开始日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime StartDate { get; set; }
    /// <summary>
    /// 截止日期
    /// </summary>
    [SugarColumn(ColumnName = "due_date", ColumnDescription = "截止日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime DueDate { get; set; }
    /// <summary>
    /// 目标达成说明
    /// </summary>
    [SugarColumn(ColumnName = "achievement_notes", ColumnDescription = "目标达成说明", ColumnDataType = "nvarchar", Length = 1000, IsNullable = false)]
    public string AchievementNotes { get; set; } = string.Empty;
    /// <summary>
    /// 业务状态（字典 humanresource_performance_objective_status；0=待确认 1=进行中 2=已完成）
    /// </summary>
    [SugarColumn(ColumnName = "objective_status", ColumnDescription = "业务状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ObjectiveStatus { get; set; }
}

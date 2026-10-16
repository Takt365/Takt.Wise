// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Performance
// 文件名称：TaktPerfCycle.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：绩效周期实体（Perf 标识），对应菜单 performance/cycle-schedule
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Performance;

/// <summary>
/// 绩效考核周期日程安排
/// </summary>
[SugarTable("takt_human_resource_perf_cycle", "绩效周期日程表")]
[SugarIndex("ix_perf_cycle_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_perf_cycle_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_perf_cycle_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CycleCode), OrderByType.Asc, true)]
public class TaktPerfCycle : TaktCompanyEntityBase
{
    /// <summary>
    /// 周期编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 周期编码规则生成并展示，非手输；单据类型菜单：绩效周期）
    /// </summary>
    [SugarColumn(ColumnName = "cycle_code", ColumnDescription = "周期编码", ColumnDataType = "nvarchar", Length = 64, IsNullable = false)]
    public string CycleCode { get; set; } = string.Empty;
    /// <summary>
    /// 周期名称
    /// </summary>
    [SugarColumn(ColumnName = "cycle_name", ColumnDescription = "周期名称", ColumnDataType = "nvarchar", Length = 128, IsNullable = false)]
    public string CycleName { get; set; } = string.Empty;
    /// <summary>
    /// 周期类型（字典 humanresource_performance_cycle；列存 DictValue：MONTH/QUARTER/HALFYEAR/YEAR）
    /// </summary>
    [SugarColumn(ColumnName = "cycle_type", ColumnDescription = "周期类型", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string CycleType { get; set; } = string.Empty;
    /// <summary>
    /// 周期年度
    /// </summary>
    [SugarColumn(ColumnName = "cycle_year", ColumnDescription = "周期年度", ColumnDataType = "int", IsNullable = false)]
    public int CycleYear { get; set; }
    /// <summary>
    /// 周期序号
    /// </summary>
    [SugarColumn(ColumnName = "cycle_sequence", ColumnDescription = "周期序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CycleSequence { get; set; }
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
    /// 目标设定截止日期
    /// </summary>
    [SugarColumn(ColumnName = "goal_setting_due_date", ColumnDescription = "目标设定截止日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime GoalSettingDueDate { get; set; }
    /// <summary>
    /// 自评截止日期
    /// </summary>
    [SugarColumn(ColumnName = "self_evaluation_due_date", ColumnDescription = "自评截止日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime SelfEvaluationDueDate { get; set; }
    /// <summary>
    /// 主管评审截止日期
    /// </summary>
    [SugarColumn(ColumnName = "supervisor_review_due_date", ColumnDescription = "主管评审截止日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime SupervisorReviewDueDate { get; set; }
    /// <summary>
    /// 面谈截止日期
    /// </summary>
    [SugarColumn(ColumnName = "interview_due_date", ColumnDescription = "面谈截止日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime InterviewDueDate { get; set; }
    /// <summary>
    /// 结果确认截止日期
    /// </summary>
    [SugarColumn(ColumnName = "result_confirmation_due_date", ColumnDescription = "结果确认截止日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime ResultConfirmationDueDate { get; set; }
    /// <summary>
    /// 适用部门
    /// </summary>
    [SugarColumn(ColumnName = "applicable_department", ColumnDescription = "适用部门", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string ApplicableDepartment { get; set; } = string.Empty;
    /// <summary>
    /// 周期说明
    /// </summary>
    [SugarColumn(ColumnName = "perf_cycle_description", ColumnDescription = "周期说明", ColumnDataType = "nvarchar", Length = 500, IsNullable = false)]
    public string PerfCycleDescription { get; set; } = string.Empty;
    /// <summary>
    /// 状态（字典 humanresource_performance_cycle_schedule_status；0=待启动 1=目标设定中 2=进行中 3=评审中 4=已完成 5=已归档）
    /// </summary>
    [SugarColumn(ColumnName = "cycle_schedule_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CycleScheduleStatus { get; set; }
}

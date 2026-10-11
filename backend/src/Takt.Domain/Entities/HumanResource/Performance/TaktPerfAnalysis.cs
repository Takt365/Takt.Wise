// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Performance
// 文件名称：TaktPerfAnalysis.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：分析改进实体（Perf 标识），对应菜单 performance/analysis
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Performance;

/// <summary>
/// 分析改进（审批态见基类 ApprovalStatus，字典 sys_approval_status）
/// </summary>
[SugarTable("takt_human_resource_perf_analysis", "分析改进表")]
[SugarIndex("ix_perf_analysis_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_perf_analysis_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_perf_analysis_employee", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, false)]
public class TaktPerfAnalysis : TaktApprovalEntityBase
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
    /// 考核评估（选项 TaktPerfAssessments/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "assessment_id", ColumnDescription = "考核评估ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssessmentId { get; set; }
    /// <summary>
    /// 改进计划标题
    /// </summary>
    [SugarColumn(ColumnName = "plan_title", ColumnDescription = "改进计划标题", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string PlanTitle { get; set; } = string.Empty;
    /// <summary>
    /// 改进领域
    /// </summary>
    [SugarColumn(ColumnName = "improvement_area", ColumnDescription = "改进领域", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string ImprovementArea { get; set; } = string.Empty;
    /// <summary>
    /// 当前状况描述
    /// </summary>
    [SugarColumn(ColumnName = "current_situation", ColumnDescription = "当前状况描述", ColumnDataType = "nvarchar", Length = 1000, IsNullable = false)]
    public string CurrentSituation { get; set; } = string.Empty;
    /// <summary>
    /// 改进目标
    /// </summary>
    [SugarColumn(ColumnName = "improvement_goal", ColumnDescription = "改进目标", ColumnDataType = "nvarchar", Length = 500, IsNullable = false)]
    public string ImprovementGoal { get; set; } = string.Empty;
    /// <summary>
    /// 改进措施
    /// </summary>
    [SugarColumn(ColumnName = "improvement_actions", ColumnDescription = "改进措施", ColumnDataType = "nvarchar", Length = 1000, IsNullable = false)]
    public string ImprovementActions { get; set; } = string.Empty;
    /// <summary>
    /// 计划制定日期
    /// </summary>
    [SugarColumn(ColumnName = "plan_date", ColumnDescription = "计划制定日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime PlanDate { get; set; }
    /// <summary>
    /// 目标完成日期
    /// </summary>
    [SugarColumn(ColumnName = "target_completion_date", ColumnDescription = "目标完成日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime TargetCompletionDate { get; set; }
    /// <summary>
    /// 进度百分比（%）
    /// </summary>
    [SugarColumn(ColumnName = "progress_percentage", ColumnDescription = "进度百分比", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ProgressPercentage { get; set; }
    /// <summary>
    /// 改进结果说明
    /// </summary>
    [SugarColumn(ColumnName = "result_description", ColumnDescription = "改进结果说明", ColumnDataType = "nvarchar", Length = 1000, IsNullable = false)]
    public string ResultDescription { get; set; } = string.Empty;
    /// <summary>
    /// 指导老师（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "mentor_id", ColumnDescription = "指导老师ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MentorId { get; set; }
    /// <summary>
    /// 指导老师名称（冗余：按 MentorId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "mentor_name", ColumnDescription = "指导老师名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? MentorName { get; set; }
    /// <summary>
    /// 业务状态（字典 humanresource_performance_improvement_status；0=待审批 1=进行中 2=已完成 3=已关闭）
    /// </summary>
    [SugarColumn(ColumnName = "improvement_status", ColumnDescription = "业务状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ImprovementStatus { get; set; }
}

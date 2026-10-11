// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Performance
// 文件名称：TaktPerfAssessment.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：绩效考核实体（Perf 标识），对应菜单 performance/assessment
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Performance;

/// <summary>
/// 员工绩效考核
/// </summary>
[SugarTable("takt_human_resource_perf_assessment", "绩效考核表")]
[SugarIndex("ix_perf_assessment_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_perf_assessment_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_perf_assessment_employee_period", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, nameof(AssessmentPeriod), OrderByType.Asc, false)]
public class TaktPerfAssessment : TaktCompanyEntityBase
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
    /// 考核周期（如 2026-Q1、2026-Annual）
    /// </summary>
    [SugarColumn(ColumnName = "assessment_period", ColumnDescription = "考核周期", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string AssessmentPeriod { get; set; } = string.Empty;
    /// <summary>
    /// 考核日期
    /// </summary>
    [SugarColumn(ColumnName = "assessment_date", ColumnDescription = "考核日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime AssessmentDate { get; set; }
    /// <summary>
    /// 方案指标（选项 TaktPerfSchemes/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "scheme_metric_id", ColumnDescription = "方案指标ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SchemeMetricId { get; set; }
    /// <summary>
    /// 自评分数
    /// </summary>
    [SugarColumn(ColumnName = "self_score", ColumnDescription = "自评分数", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal SelfScore { get; set; }
    /// <summary>
    /// 自评说明
    /// </summary>
    [SugarColumn(ColumnName = "self_evaluation_notes", ColumnDescription = "自评说明", ColumnDataType = "nvarchar", Length = 1000, IsNullable = false)]
    public string SelfEvaluationNotes { get; set; } = string.Empty;
    /// <summary>
    /// 主管评分
    /// </summary>
    [SugarColumn(ColumnName = "supervisor_score", ColumnDescription = "主管评分", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal SupervisorScore { get; set; }
    /// <summary>
    /// 主管评语
    /// </summary>
    [SugarColumn(ColumnName = "supervisor_comments", ColumnDescription = "主管评语", ColumnDataType = "nvarchar", Length = 1000, IsNullable = false)]
    public string SupervisorComments { get; set; } = string.Empty;
    /// <summary>
    /// 综合得分
    /// </summary>
    [SugarColumn(ColumnName = "final_score", ColumnDescription = "综合得分", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal FinalScore { get; set; }
    /// <summary>
    /// 绩效等级（字典 humanresource_performance_grade；列存 DictValue：A/B/C/D/E）
    /// </summary>
    [SugarColumn(ColumnName = "performance_grade", ColumnDescription = "绩效等级", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string PerformanceGrade { get; set; } = string.Empty;
    /// <summary>
    /// 评审人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "reviewer_id", ColumnDescription = "评审人ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ReviewerId { get; set; }
    /// <summary>
    /// 评审人名称（冗余：按 ReviewerId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "reviewer_name", ColumnDescription = "评审人名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? ReviewerName { get; set; }
    /// <summary>
    /// 面谈日期
    /// </summary>
    [SugarColumn(ColumnName = "interview_date", ColumnDescription = "面谈日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime InterviewDate { get; set; }
    /// <summary>
    /// 面谈记录
    /// </summary>
    [SugarColumn(ColumnName = "interview_notes", ColumnDescription = "面谈记录", ColumnDataType = "nvarchar", Length = 1000, IsNullable = false)]
    public string InterviewNotes { get; set; } = string.Empty;
    /// <summary>
    /// 状态（字典 humanresource_performance_assessment_status；0=待自评 1=自评中 2=待主管评审 3=评审中 4=已完成 5=已确认）
    /// </summary>
    [SugarColumn(ColumnName = "assessment_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int AssessmentStatus { get; set; }
}

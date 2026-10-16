// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Performance
// 文件名称：TaktPerfScheme.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：绩效方案指标实体（Perf 标识），对应菜单 performance/scheme-metric
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Performance;

/// <summary>
/// 绩效方案指标（方案维度 + 指标维度合一，每行表示某方案下的一条指标）
/// </summary>
[SugarTable("takt_human_resource_perf_scheme", "绩效方案指标表")]
[SugarIndex("ix_perf_scheme_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_perf_scheme_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_perf_scheme_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SchemeCode), OrderByType.Asc, nameof(MetricCode), OrderByType.Asc, true)]
public class TaktPerfScheme : TaktCompanyEntityBase
{
    /// <summary>
    /// 方案编码（前端表单选择编码规则后自动通过 TaktNumbering 方案编码规则生成并展示，非手输；单据类型菜单：绩效方案）
    /// </summary>
    [SugarColumn(ColumnName = "scheme_code", ColumnDescription = "方案编码", ColumnDataType = "nvarchar", Length = 64, IsNullable = false)]
    public string SchemeCode { get; set; } = string.Empty;
    /// <summary>
    /// 方案名称
    /// </summary>
    [SugarColumn(ColumnName = "scheme_name", ColumnDescription = "方案名称", ColumnDataType = "nvarchar", Length = 128, IsNullable = false)]
    public string SchemeName { get; set; } = string.Empty;
    /// <summary>
    /// 适用部门
    /// </summary>
    [SugarColumn(ColumnName = "applicable_department", ColumnDescription = "适用部门", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string ApplicableDepartment { get; set; } = string.Empty;
    /// <summary>
    /// 考核周期类型（字典 humanresource_performance_cycle；列存 DictValue：MONTH/QUARTER/HALFYEAR/YEAR）
    /// </summary>
    [SugarColumn(ColumnName = "cycle_type", ColumnDescription = "考核周期类型", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string CycleType { get; set; } = string.Empty;
    /// <summary>
    /// 评分标准（字典 humanresource_performance_scoring_standard；列存 DictValue：PERCENT/FIVE/GRADE）
    /// </summary>
    [SugarColumn(ColumnName = "scoring_standard", ColumnDescription = "评分标准", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string ScoringStandard { get; set; } = string.Empty;
    /// <summary>
    /// 自评权重（%）
    /// </summary>
    [SugarColumn(ColumnName = "self_evaluation_weight", ColumnDescription = "自评权重", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal SelfEvaluationWeight { get; set; }
    /// <summary>
    /// 主管评分权重（%）
    /// </summary>
    [SugarColumn(ColumnName = "supervisor_weight", ColumnDescription = "主管评分权重", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal SupervisorWeight { get; set; }
    /// <summary>
    /// 指标编码
    /// </summary>
    [SugarColumn(ColumnName = "metric_code", ColumnDescription = "指标编码", ColumnDataType = "nvarchar", Length = 64, IsNullable = false)]
    public string MetricCode { get; set; } = string.Empty;
    /// <summary>
    /// 指标名称
    /// </summary>
    [SugarColumn(ColumnName = "metric_name", ColumnDescription = "指标名称", ColumnDataType = "nvarchar", Length = 128, IsNullable = false)]
    public string MetricName { get; set; } = string.Empty;
    /// <summary>
    /// 指标类别（字典 humanresource_performance_metric_category；列存 DictValue：PERF/CAPABILITY/ATTITUDE/MANAGEMENT/INNOVATION/QUALITY/EFFICIENCY/SAFETY）
    /// </summary>
    [SugarColumn(ColumnName = "category", ColumnDescription = "指标类别", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string Category { get; set; } = string.Empty;
    /// <summary>
    /// 指标类型（字典 humanresource_performance_metric_type；列存 DictValue：QUANT/QUAL）
    /// </summary>
    [SugarColumn(ColumnName = "metric_type", ColumnDescription = "指标类型", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string MetricType { get; set; } = string.Empty;
    /// <summary>
    /// 评分标准说明
    /// </summary>
    [SugarColumn(ColumnName = "scoring_criteria", ColumnDescription = "评分标准说明", ColumnDataType = "nvarchar", Length = 1000, IsNullable = false)]
    public string ScoringCriteria { get; set; } = string.Empty;
    /// <summary>
    /// 标准权重（%）
    /// </summary>
    [SugarColumn(ColumnName = "standard_weight", ColumnDescription = "标准权重", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal StandardWeight { get; set; }
    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; }
    /// <summary>
    /// 状态（字典 humanresource_performance_scheme_metric_status；0=启用 1=停用）
    /// </summary>
    [SugarColumn(ColumnName = "scheme_metric_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SchemeMetricStatus { get; set; }
}

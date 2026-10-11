// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Quality.Cost
// 文件名称：TaktQualityIssue.cs
// 创建时间：2026-05-07
// 创建人：Takt365(Qoder AI)
// 功能描述:品质问题应对主表,用于记录质量问题的基础信息(年月日、机种、批次)
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Newtonsoft.Json;
using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Quality.Cost;

/// <summary>
/// 品质问题应对主表,用于记录质量问题的基础信息(年月日、机种、批次)及汇总数据
/// </summary>
[SugarTable("takt_logistics_quality_issue", "品质问题应对主表")]
[SugarIndex("ix_quality_issue_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_quality_issue_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_issue_qf_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(QualityIssueCode), OrderByType.Asc, nameof(IssueDate), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_quality_issue_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktQualityIssue : TaktCompanyEntityBase
{

    /// <summary>
    /// 品质问题编码（唯一，如：QF-2026-0001）
    /// </summary>
    [SugarColumn(ColumnName = "quality_issue_code", ColumnDescription = "品质问题编码", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string QualityIssueCode { get; set; } = string.Empty;

    // ==================== 基础日期与产品信息 ====================
    
    /// <summary>
    /// 问题日期
    /// </summary>
    [SugarColumn(ColumnName = "issue_date", ColumnDescription = "问题日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime IssueDate { get; set; }

    /// <summary>
    /// 机种/产品型号
    /// </summary>
    [SugarColumn(ColumnName = "model", ColumnDescription = "机种", Length = 40, ColumnDataType = "nvarchar", IsNullable = false)]
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// 批次号/Lot No
    /// </summary>
    [SugarColumn(ColumnName = "lot", ColumnDescription = "批次号", Length = 30, ColumnDataType = "nvarchar", IsNullable = false)]
    public string Lot { get; set; } = string.Empty;

    // ==================== 汇总信息 ====================

    /// <summary>
    /// 品质问题应对摘要(汇总说明)
    /// </summary>
    [SugarColumn(ColumnName = "quality_problems_response", ColumnDescription = "品质问题应对", Length = 255, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? QualityProblemsResponse { get; set; }

    /// <summary>
    /// 不良改修应对摘要(汇总说明)
    /// </summary>
    [SugarColumn(ColumnName = "rework_due_to_defects", ColumnDescription = "不良改修应对", Length = 255, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? ReworkDueToDefects { get; set; }

    /// <summary>
    /// 是否需要不良改修应对(Y/N)
    /// </summary>
    [SugarColumn(ColumnName = "need_rework", ColumnDescription = "是否需要不良改修应对", Length = 1, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? NeedRework { get; set; }

    /// <summary>
    /// 总时间(分钟,自动计算 = 各子表时间合计)
    /// </summary>
    [SugarColumn(ColumnName = "total_time_minutes", ColumnDescription = "总时间", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TotalTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 总费用(元,自动计算 = 各子表费用合计)
    /// </summary>
    [SugarColumn(ColumnName = "total_cost", ColumnDescription = "总费用", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TotalCost { get; set; } = 0;

    /// <summary>
    /// 成本币种（CNY/USD/JPY等）
    /// </summary>
    [SugarColumn(ColumnName = "currency_code", ColumnDescription = "成本币种", Length = 3, ColumnDataType = "nvarchar", IsNullable = false, DefaultValue = "CNY")]
    public string CurrencyCode { get; set; } = "CNY";

    // ==================== 导航关系 ====================

    /// <summary>
    /// 会议/调查/试验费用明细列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktQualityIssueMeeting.QualityIssueId))]
    public List<TaktQualityIssueMeeting>? MeetingItems { get; set; }

    /// <summary>
    /// 组装不良改修应对明细列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktQualityIssueAssyRework.QualityIssueId))]
    public List<TaktQualityIssueAssyRework>? AssyReworkItems { get; set; }

    /// <summary>
    /// PCBA不良改修应对明细列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktQualityIssuePcbaRework.QualityIssueId))]
    public List<TaktQualityIssuePcbaRework>? PcbaReworkItems { get; set; }
}

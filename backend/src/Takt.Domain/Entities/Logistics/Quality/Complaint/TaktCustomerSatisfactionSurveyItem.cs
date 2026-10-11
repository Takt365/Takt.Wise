// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Quality.Complaint
// 文件名称：TaktCustomerSatisfactionSurveyItem.cs
// 创建时间：2026-05-11
// 创建人：Takt365(Cursor AI)
// 功能描述：客户满意度调查项目明细实体，记录具体的调查项目和评分
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Quality.Complaint;

/// <summary>
/// 客户满意度调查项目明细实体
/// </summary>
[SugarTable("takt_logistics_quality_customer_satisfaction_survey_item", "客户满意度调查项目明细表")]
[SugarIndex("ix_customer_satisfaction_survey_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_customer_satisfaction_survey_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_customer_satisfaction_survey_item_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SurveyId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_quality_customer_satisfaction_survey_item_category_type", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CategoryType), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_customer_satisfaction_survey_item_customer_satisfaction_survey_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CustomerSatisfactionSurveyCode), OrderByType.Asc, false)]
public class TaktCustomerSatisfactionSurveyItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 调查表 ID（选项 TaktCustomerSatisfactionSurveys/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "survey_id", ColumnDescription = "调查表ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SurveyId { get; set; }

    /// <summary>
    /// 调查表编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "customer_satisfaction_survey_code", ColumnDescription = "调查表编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string CustomerSatisfactionSurveyCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 调查类别类型（字典 logistics_quality_satisfaction_category）
    /// </summary>
    [SugarColumn(ColumnName = "category_type", ColumnDescription = "调查类别", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CategoryType { get; set; } = 0;

    /// <summary>
    /// 调查项目名称
    /// </summary>
    [SugarColumn(ColumnName = "item_name", ColumnDescription = "调查项目", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 调查项目说明
    /// </summary>
    [SugarColumn(ColumnName = "item_description", ColumnDescription = "项目说明", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? ItemDescription { get; set; }

    /// <summary>
    /// 权重（%）
    /// </summary>
    [SugarColumn(ColumnName = "weight", ColumnDescription = "权重", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int Weight { get; set; } = 0;

    /// <summary>
    /// 评分（0-100分）
    /// </summary>
    [SugarColumn(ColumnName = "score", ColumnDescription = "评分", ColumnDataType = "int", IsNullable = true)]
    public int? Score { get; set; }

    /// <summary>
    /// 满意度等级（字典 logistics_quality_satisfaction_level）
    /// </summary>
    [SugarColumn(ColumnName = "satisfaction_level", ColumnDescription = "满意度等级", ColumnDataType = "int", IsNullable = true)]
    public int? SatisfactionLevel { get; set; }

    /// <summary>
    /// 客户反馈/意见
    /// </summary>
    [SugarColumn(ColumnName = "customer_feedback", ColumnDescription = "客户反馈", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? CustomerFeedback { get; set; }

    /// <summary>
    /// 改进建议
    /// </summary>
    [SugarColumn(ColumnName = "improvement_suggestion", ColumnDescription = "改进建议", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? ImprovementSuggestion { get; set; }

    /// <summary>
    /// 跟进措施
    /// </summary>
    [SugarColumn(ColumnName = "follow_up_action", ColumnDescription = "跟进措施", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? FollowUpAction { get; set; }

    /// <summary>
    /// 跟进状态（字典 logistics_quality_follow_up_status）
    /// </summary>
    [SugarColumn(ColumnName = "follow_up_status", ColumnDescription = "跟进状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int FollowUpStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 调查表主表
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(SurveyId))]
    public TaktCustomerSatisfactionSurvey? Survey { get; set; }
}

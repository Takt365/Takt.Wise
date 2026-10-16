// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Quality.Complaint
// 文件名称：TaktCustomerSatisfactionSurvey.cs
// 创建时间：2026-05-11
// 创建人：Takt365(Cursor AI)
// 功能描述：客户满意度调查表主表实体，记录客户满意度调查的基本信息
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Quality.Complaint;

/// <summary>
/// 客户满意度调查表主表实体
/// </summary>
[SugarTable("takt_logistics_quality_customer_satisfaction_survey", "客户满意度调查表")]
[SugarIndex("ix_customer_satisfaction_survey_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_customer_satisfaction_survey_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_customer_satisfaction_survey_survey_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(CustomerSatisfactionSurveyCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_quality_customer_satisfaction_survey_related_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_customer_satisfaction_survey_customer_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CustomerId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_customer_satisfaction_survey_overall_satisfaction", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OverallSatisfaction), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_customer_satisfaction_survey_survey_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SurveyDate), OrderByType.Asc, false)]
public class TaktCustomerSatisfactionSurvey : TaktCompanyEntityBase
{
    /// <summary>
    /// 调查表编码（租户+公司+工厂内唯一；前端表单选择编码规则后自动通过 TaktNumbering 调查表编码规则生成并展示，非手输；单据类型菜单：客户满意度调查）
    /// </summary>
    [SugarColumn(ColumnName = "customer_satisfaction_survey_code", ColumnDescription = "调查表编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string CustomerSatisfactionSurveyCode { get; set; } = string.Empty;
    /// <summary>
    /// 客户 ID（选项 TaktCustomers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "customer_id", ColumnDescription = "客户ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerId { get; set; }
    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    [SugarColumn(ColumnName = "customer_name1", ColumnDescription = "客户名称1", ColumnDataType = "nvarchar", Length = 140, IsNullable = false)]
    public string CustomerName1 { get; set; } = string.Empty;
    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    [SugarColumn(ColumnName = "customer_code", ColumnDescription = "客户编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? CustomerCode { get; set; }
    /// <summary>
    /// 调查日期
    /// </summary>
    [SugarColumn(ColumnName = "survey_date", ColumnDescription = "调查日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime SurveyDate { get; set; } = DateTime.Today;
    /// <summary>
    /// 调查方式（字典 logistics_quality_survey_method）
    /// </summary>
    [SugarColumn(ColumnName = "survey_method", ColumnDescription = "调查方式", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SurveyMethod { get; set; } = 0;
    /// <summary>
    /// 调查类型（字典 logistics_quality_survey_type）
    /// </summary>
    [SugarColumn(ColumnName = "survey_type", ColumnDescription = "调查类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SurveyType { get; set; } = 0;
    /// <summary>
    /// 调查周期（字典 logistics_quality_period）
    /// </summary>
    [SugarColumn(ColumnName = "survey_period", ColumnDescription = "调查周期", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int SurveyPeriod { get; set; } = 1;
    /// <summary>
    /// 调查组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=5；DictValue=ComplaintGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "surveyor_by_group_code", ColumnDescription = "调查组编码", ColumnDataType = "varchar", Length = 6, IsNullable = true)]
    public string? SurveyorByGroupCode { get; set; }
    /// <summary>
    /// 调查组名称（冗余：按 SurveyorByGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "surveyor_by_group_name", ColumnDescription = "调查组名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? SurveyorByGroupName { get; set; }
    /// <summary>
    /// 客户联系人
    /// </summary>
    [SugarColumn(ColumnName = "customer_contact", ColumnDescription = "客户联系人", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? CustomerContact { get; set; }
    /// <summary>
    /// 客户联系电话
    /// </summary>
    [SugarColumn(ColumnName = "customer_phone", ColumnDescription = "客户联系电话", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? CustomerPhone { get; set; }
    /// <summary>
    /// 整体满意度（字典 logistics_quality_satisfaction_level）
    /// </summary>
    [SugarColumn(ColumnName = "overall_satisfaction", ColumnDescription = "整体满意度", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int OverallSatisfaction { get; set; } = 0;
    /// <summary>
    /// 综合评分（0-100分）
    /// </summary>
    [SugarColumn(ColumnName = "total_score", ColumnDescription = "综合评分", ColumnDataType = "int", IsNullable = true)]
    public int? TotalScore { get; set; }
    /// <summary>
    /// 产品质量评分（0-100分）
    /// </summary>
    [SugarColumn(ColumnName = "quality_score", ColumnDescription = "产品质量评分", ColumnDataType = "int", IsNullable = true)]
    public int? QualityScore { get; set; }
    /// <summary>
    /// 交付准时率评分（0-100分）
    /// </summary>
    [SugarColumn(ColumnName = "delivery_score", ColumnDescription = "交付准时率评分", ColumnDataType = "int", IsNullable = true)]
    public int? DeliveryScore { get; set; }
    /// <summary>
    /// 服务质量评分（0-100分）
    /// </summary>
    [SugarColumn(ColumnName = "service_score", ColumnDescription = "服务质量评分", ColumnDataType = "int", IsNullable = true)]
    public int? ServiceScore { get; set; }
    /// <summary>
    /// 价格竞争力评分（0-100分）
    /// </summary>
    [SugarColumn(ColumnName = "price_score", ColumnDescription = "价格竞争力评分", ColumnDataType = "int", IsNullable = true)]
    public int? PriceScore { get; set; }
    /// <summary>
    /// 技术支持评分（0-100分）
    /// </summary>
    [SugarColumn(ColumnName = "technical_score", ColumnDescription = "技术支持评分", ColumnDataType = "int", IsNullable = true)]
    public int? TechnicalScore { get; set; }
    /// <summary>
    /// 客户主要表扬
    /// </summary>
    [SugarColumn(ColumnName = "customer_praise", ColumnDescription = "客户主要表扬", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? CustomerPraise { get; set; }
    /// <summary>
    /// 客户主要意见/建议
    /// </summary>
    [SugarColumn(ColumnName = "customer_feedback", ColumnDescription = "客户意见", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? CustomerFeedback { get; set; }
    /// <summary>
    /// 改进计划/措施
    /// </summary>
    [SugarColumn(ColumnName = "improvement_plan", ColumnDescription = "改进计划", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? ImprovementPlan { get; set; }
    /// <summary>
    /// 关联客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "related_complaint_id", ColumnDescription = "关联客诉ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RelatedComplaintId { get; set; }
    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    [SugarColumn(ColumnName = "attachments", ColumnDescription = "附件JSON", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? Attachments { get; set; }
    /// <summary>
    /// 调查状态（字典 logistics_quality_survey_status）
    /// </summary>
    [SugarColumn(ColumnName = "survey_status", ColumnDescription = "调查状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SurveyStatus { get; set; } = 0;
    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 跟进状态（字典 logistics_quality_follow_up_status）
    /// </summary>
    [SugarColumn(ColumnName = "follow_up_status", ColumnDescription = "跟进状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int FollowUpStatus { get; set; } = 0;

    /// <summary>
    /// 调查项目明细列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktCustomerSatisfactionSurveyItem.SurveyId))]
    public List<TaktCustomerSatisfactionSurveyItem>? Items { get; set; }
}

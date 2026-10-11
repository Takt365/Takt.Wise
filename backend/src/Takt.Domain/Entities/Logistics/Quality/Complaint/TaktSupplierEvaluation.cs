// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Quality.Complaint
// 文件名称：TaktSupplierEvaluation.cs
// 创建时间：2026-05-11
// 创建人：Takt365(Cursor AI)
// 功能描述：供应商评价考核主表实体，记录供应商评价考核的基本信息
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Quality.Complaint;

/// <summary>
/// 供应商评价考核主表实体
/// </summary>
[SugarTable("takt_logistics_quality_supplier_evaluation", "供应商评价考核表")]
[SugarIndex("ix_supplier_evaluation_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_supplier_evaluation_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_supplier_evaluation_evaluation_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(SupplierEvaluationCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_quality_supplier_evaluation_related_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_supplier_evaluation_evaluation_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EvaluationDate), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_supplier_evaluation_overall_rating", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OverallRating), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_supplier_evaluation_supplier_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SupplierId), OrderByType.Asc, false)]
public class TaktSupplierEvaluation : TaktCompanyEntityBase
{
    /// <summary>
    /// 评价表编码（组合唯一索引）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_evaluation_code", ColumnDescription = "评价表编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string SupplierEvaluationCode { get; set; } = string.Empty;
    /// <summary>
    /// 供应商 ID（选项 TaktSuppliers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_id", ColumnDescription = "供应商ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SupplierId { get; set; }
    /// <summary>
    /// 供应商名称
    /// </summary>
    [SugarColumn(ColumnName = "supplier_name1", ColumnDescription = "供应商名称1", ColumnDataType = "nvarchar", Length = 140, IsNullable = false)]
    public string SupplierName1 { get; set; } = string.Empty;
    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_code", ColumnDescription = "供应商编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? SupplierCode { get; set; }
    /// <summary>
    /// 评价日期
    /// </summary>
    [SugarColumn(ColumnName = "evaluation_date", ColumnDescription = "评价日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime EvaluationDate { get; set; } = DateTime.Today;
    /// <summary>
    /// 评价周期（字典 logistics_quality_period）
    /// </summary>
    [SugarColumn(ColumnName = "evaluation_period", ColumnDescription = "评价周期", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int EvaluationPeriod { get; set; } = 1;
    /// <summary>
    /// 评价类型（0=常规评价，1=准入评价，2=年度评审，3=专项评价）
    /// </summary>
    [SugarColumn(ColumnName = "evaluation_type", ColumnDescription = "评价类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int EvaluationType { get; set; } = 0;
    /// <summary>
    /// 评价组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=4；DictValue=ComplaintGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "evaluator_by_group_code", ColumnDescription = "评价组编码", ColumnDataType = "varchar", Length = 6, IsNullable = true)]
    public string? EvaluatorByGroupCode { get; set; }
    /// <summary>
    /// 评价组名称（冗余：按 EvaluatorByGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "evaluator_by_group_name", ColumnDescription = "评价组名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? EvaluatorByGroupName { get; set; }
    /// <summary>
    /// 评价部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "evaluation_dept_id", ColumnDescription = "评价部门ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EvaluationDeptId { get; set; }
    /// <summary>
    /// 评价部门名称（冗余：按 EvaluationDeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    [SugarColumn(ColumnName = "evaluation_dept_name", ColumnDescription = "评价部门名称", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? EvaluationDeptName { get; set; }
    /// <summary>
    /// 总体评级（字典 logistics_quality_supplier_rating）
    /// </summary>
    [SugarColumn(ColumnName = "overall_rating", ColumnDescription = "总体评级", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int OverallRating { get; set; } = 0;
    /// <summary>
    /// 综合评分（0-100分）
    /// </summary>
    [SugarColumn(ColumnName = "total_score", ColumnDescription = "综合评分", ColumnDataType = "int", IsNullable = true)]
    public int? TotalScore { get; set; }
    /// <summary>
    /// 质量评分（0-100分）
    /// </summary>
    [SugarColumn(ColumnName = "quality_score", ColumnDescription = "质量评分", ColumnDataType = "int", IsNullable = true)]
    public int? QualityScore { get; set; }
    /// <summary>
    /// 交付评分（0-100分）
    /// </summary>
    [SugarColumn(ColumnName = "delivery_score", ColumnDescription = "交付评分", ColumnDataType = "int", IsNullable = true)]
    public int? DeliveryScore { get; set; }
    /// <summary>
    /// 价格评分（0-100分）
    /// </summary>
    [SugarColumn(ColumnName = "price_score", ColumnDescription = "价格评分", ColumnDataType = "int", IsNullable = true)]
    public int? PriceScore { get; set; }
    /// <summary>
    /// 服务评分（0-100分）
    /// </summary>
    [SugarColumn(ColumnName = "service_score", ColumnDescription = "服务评分", ColumnDataType = "int", IsNullable = true)]
    public int? ServiceScore { get; set; }
    /// <summary>
    /// 技术能力评分（0-100分）
    /// </summary>
    [SugarColumn(ColumnName = "technical_score", ColumnDescription = "技术能力评分", ColumnDataType = "int", IsNullable = true)]
    public int? TechnicalScore { get; set; }
    /// <summary>
    /// 主要优点
    /// </summary>
    [SugarColumn(ColumnName = "main_strengths", ColumnDescription = "主要优点", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? MainStrengths { get; set; }
    /// <summary>
    /// 主要问题/不足
    /// </summary>
    [SugarColumn(ColumnName = "main_issues", ColumnDescription = "主要问题", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? MainIssues { get; set; }
    /// <summary>
    /// 改进要求/建议
    /// </summary>
    [SugarColumn(ColumnName = "improvement_requirements", ColumnDescription = "改进要求", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? ImprovementRequirements { get; set; }
    /// <summary>
    /// 考核结论（字典 logistics_quality_evaluation_conclusion）
    /// </summary>
    [SugarColumn(ColumnName = "evaluation_conclusion", ColumnDescription = "考核结论", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int EvaluationConclusion { get; set; } = 0;
    /// <summary>
    /// 整改期限（要求完成日期）
    /// </summary>
    [SugarColumn(ColumnName = "rectification_deadline", ColumnDescription = "整改期限", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? RectificationDeadline { get; set; }
    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    [SugarColumn(ColumnName = "attachments", ColumnDescription = "附件JSON", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? Attachments { get; set; }
    /// <summary>
    /// 评价状态（字典 logistics_quality_evaluation_status）
    /// </summary>
    [SugarColumn(ColumnName = "evaluation_status", ColumnDescription = "评价状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int EvaluationStatus { get; set; } = 0;
    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 整改跟进状态（字典 logistics_quality_rectification_status）
    /// </summary>
    [SugarColumn(ColumnName = "rectification_status", ColumnDescription = "整改跟进状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int RectificationStatus { get; set; } = 0;

    /// <summary>
    /// 评价项目明细列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktSupplierEvaluationItem.EvaluationId))]
    public List<TaktSupplierEvaluationItem>? Items { get; set; }
}

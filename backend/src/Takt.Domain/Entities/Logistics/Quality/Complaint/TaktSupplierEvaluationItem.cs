// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Quality.Complaint
// 文件名称：TaktSupplierEvaluationItem.cs
// 创建时间：2026-05-11
// 创建人：Takt365(Cursor AI)
// 功能描述：供应商评价考核项目明细实体，记录具体的评价项目和评分
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Quality.Complaint;

/// <summary>
/// 供应商评价考核项目明细实体
/// </summary>
[SugarTable("takt_logistics_quality_supplier_evaluation_item", "供应商评价考核项目明细表")]
[SugarIndex("ix_supplier_evaluation_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_supplier_evaluation_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_supplier_evaluation_item_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EvaluationId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_quality_supplier_evaluation_item_category_type", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CategoryType), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_supplier_evaluation_item_evaluation_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EvaluationId), OrderByType.Asc, false)]
public class TaktSupplierEvaluationItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 评价表 ID（选项 TaktSupplierEvaluations/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "evaluation_id", ColumnDescription = "评价表ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EvaluationId { get; set; }

    /// <summary>
    /// 评价表编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_evaluation_code", ColumnDescription = "评价表编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string SupplierEvaluationCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 评价类别类型（字典 logistics_quality_evaluation_category）
    /// </summary>
    [SugarColumn(ColumnName = "category_type", ColumnDescription = "评价类别", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CategoryType { get; set; } = 0;

    /// <summary>
    /// 评价项目名称
    /// </summary>
    [SugarColumn(ColumnName = "item_name", ColumnDescription = "评价项目", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 评价项目说明
    /// </summary>
    [SugarColumn(ColumnName = "item_description", ColumnDescription = "项目说明", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? ItemDescription { get; set; }

    /// <summary>
    /// 权重（%）
    /// </summary>
    [SugarColumn(ColumnName = "weight", ColumnDescription = "权重", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int Weight { get; set; } = 0;

    /// <summary>
    /// 评分标准
    /// </summary>
    [SugarColumn(ColumnName = "scoring_standard", ColumnDescription = "评分标准", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? ScoringStandard { get; set; }

    /// <summary>
    /// 评分（0-100分）
    /// </summary>
    [SugarColumn(ColumnName = "score", ColumnDescription = "评分", ColumnDataType = "int", IsNullable = true)]
    public int? Score { get; set; }

    /// <summary>
    /// 评级（字典 logistics_quality_supplier_rating）
    /// </summary>
    [SugarColumn(ColumnName = "rating_level", ColumnDescription = "评级", ColumnDataType = "int", IsNullable = true)]
    public int? RatingLevel { get; set; }

    /// <summary>
    /// 评价说明/事实依据
    /// </summary>
    [SugarColumn(ColumnName = "evaluation_comment", ColumnDescription = "评价说明", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? EvaluationComment { get; set; }

    /// <summary>
    /// 存在问题
    /// </summary>
    [SugarColumn(ColumnName = "existing_issues", ColumnDescription = "存在问题", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? ExistingIssues { get; set; }

    /// <summary>
    /// 改进要求
    /// </summary>
    [SugarColumn(ColumnName = "improvement_requirement", ColumnDescription = "改进要求", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? ImprovementRequirement { get; set; }

    /// <summary>
    /// 整改要求（0=无需整改，1=限期整改，2=重点整改）
    /// </summary>
    [SugarColumn(ColumnName = "rectification_required", ColumnDescription = "整改要求", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int RectificationRequired { get; set; } = 0;

    /// <summary>
    /// 整改期限
    /// </summary>
    [SugarColumn(ColumnName = "rectification_deadline", ColumnDescription = "整改期限", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? RectificationDeadline { get; set; }

    /// <summary>
    /// 整改状态（字典 logistics_quality_rectification_status）
    /// </summary>
    [SugarColumn(ColumnName = "rectification_status", ColumnDescription = "整改状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int RectificationStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 评价表主表
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(EvaluationId))]
    public TaktSupplierEvaluation? Evaluation { get; set; }
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Quality
// 文件名称：TaktInspectionStandardItem.cs
// 功能描述：检验标准明细实体
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Quality.Operation;

/// <summary>
/// 检验标准明细实体
/// </summary>
[SugarTable("takt_logistics_quality_inspection_standard_item", "检验标准明细表")]
[SugarIndex("ix_inspection_standard_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_inspection_standard_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_inspection_standard_item_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(InspectionStandardId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, nameof(ItemCode), OrderByType.Asc, nameof(ItemType), OrderByType.Asc, true)]
public class TaktInspectionStandardItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 检验标准 ID（选项 TaktInspectionStandards/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "inspection_standard_id", ColumnDescription = "检验标准ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InspectionStandardId { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 检验项目编码（前端表单选择编码规则后自动通过 TaktNumbering 检验项目编码规则生成并展示，非手输；单据类型菜单：检验标准）
    /// </summary>
    [SugarColumn(ColumnName = "item_code", ColumnDescription = "检验项目编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string ItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验项目名称
    /// </summary>
    [SugarColumn(ColumnName = "item_name", ColumnDescription = "检验项目名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string ItemName { get; set; } = string.Empty;

    /// <summary>
    /// 检验项目类型（字典 logistics_quality_inspection_item_type）
    /// </summary>
    [SugarColumn(ColumnName = "item_type", ColumnDescription = "检验项目类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ItemType { get; set; } = 0;

    /// <summary>
    /// 缺点等级（字典 logistics_quality_defect_severity_code；CR/MA/MI）
    /// </summary>
    [SugarColumn(ColumnName = "defect_level", ColumnDescription = "缺点等级", ColumnDataType = "nvarchar", Length = 2, IsNullable = false)]
    public string DefectLevel { get; set; } = string.Empty;

    /// <summary>
    /// 检验方式（字典 logistics_quality_inspection_mode）
    /// </summary>
    [SugarColumn(ColumnName = "inspection_mode", ColumnDescription = "检验方式", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int InspectionMode { get; set; } = 1;

    /// <summary>
    /// 检验标准值
    /// </summary>
    [SugarColumn(ColumnName = "standard_value", ColumnDescription = "检验标准值", ColumnDataType = "nvarchar", Length = 500, IsNullable = false)]
    public string StandardValue { get; set; } = string.Empty;

    /// <summary>
    /// 检验上限值
    /// </summary>
    [SugarColumn(ColumnName = "upper_limit", ColumnDescription = "检验上限值", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string UpperLimit { get; set; } = string.Empty;

    /// <summary>
    /// 检验下限值
    /// </summary>
    [SugarColumn(ColumnName = "lower_limit", ColumnDescription = "检验下限值", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string LowerLimit { get; set; } = string.Empty;

    /// <summary>
    /// 检验工具（手输名称）
    /// </summary>
    [SugarColumn(ColumnName = "inspection_tool", ColumnDescription = "检验工具", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string InspectionTool { get; set; } = string.Empty;

    /// <summary>
    /// 检验方法说明
    /// </summary>
    [SugarColumn(ColumnName = "inspection_method_description", ColumnDescription = "检验方法说明", ColumnDataType = "nvarchar", Length = 70, IsNullable = false)]
    public string InspectionMethodDescription { get; set; } = string.Empty;

    /// <summary>
    /// 接收标准（AC值）
    /// </summary>
    [SugarColumn(ColumnName = "acceptance_criteria", ColumnDescription = "接收标准(AC值)", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string AcceptanceCriteria { get; set; } = string.Empty;

    /// <summary>
    /// 拒收标准（RE值）
    /// </summary>
    [SugarColumn(ColumnName = "rejection_criteria", ColumnDescription = "拒收标准(RE值)", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string RejectionCriteria { get; set; } = string.Empty;

    /// <summary>
    /// 是否合格判定项目（字典 sys_yes_no）
    /// </summary>
    [SugarColumn(ColumnName = "is_qualified_basis", ColumnDescription = "是否合格判定项目", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int IsQualifiedBasis { get; set; } = 1;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 检验标准（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(InspectionStandardId))]
    public TaktInspectionStandard? Standard { get; set; }
}

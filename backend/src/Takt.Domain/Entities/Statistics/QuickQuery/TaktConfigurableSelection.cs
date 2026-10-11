// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Statistics.QuickQuery
// 文件名称：TaktConfigurableSelection.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：定制报表筛选条件（WHERE）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.Statistics.QuickQuery;

/// <summary>
/// 定制报表筛选条件
/// </summary>
/// <remarks>
/// 运行前由用户填写筛选值；服务层将合法值编译为参数化 WHERE 条件。
/// </remarks>
[SugarTable("takt_statistics_quick_query_configurable_selection", "定制报表筛选表")]
[SugarIndex("ix_configurable_selection_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_configurable_selection_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_configurable_selection_report_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ConfigurableId), OrderByType.Asc, false)]
public class TaktConfigurableSelection : TaktCompanyEntityBase
{
    /// <summary>
    /// 关联定制报表主表 ID（选项 TaktConfigurables/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "configurable_id", ColumnDescription = "定制报表主表ID", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableId { get; set; }

    /// <summary>
    /// 数据源别名
    /// </summary>
    [SugarColumn(ColumnName = "source_alias", ColumnDescription = "数据源别名", ColumnDataType = "varchar", Length = 10, IsNullable = false)]
    public string SourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 列名
    /// </summary>
    [SugarColumn(ColumnName = "column_name", ColumnDescription = "列名", ColumnDataType = "varchar", Length = 128, IsNullable = false)]
    public string ColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 显示名称（筛选项标签）
    /// </summary>
    [SugarColumn(ColumnName = "display_name", ColumnDescription = "显示名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// 比较运算符（字典 code_generator_query_type；1=等于 2=不等于 3=大于 4=大于等于 5=小于 6=小于等于 7=模糊 8=区间）
    /// </summary>
    [SugarColumn(ColumnName = "filter_operator", ColumnDescription = "比较运算符", ColumnDataType = "int", IsNullable = false, DefaultValue = "7")]
    public int FilterOperator { get; set; } = 7;

    /// <summary>
    /// 默认值（单值或 IN 列表逗号分隔）
    /// </summary>
    [SugarColumn(ColumnName = "default_value", ColumnDescription = "默认值", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? DefaultValue { get; set; }

    /// <summary>
    /// 区间结束值（BETWEEN 时使用）
    /// </summary>
    [SugarColumn(ColumnName = "default_value_to", ColumnDescription = "区间结束值", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? DefaultValueTo { get; set; }

    /// <summary>
    /// 是否必填（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_required", ColumnDescription = "是否必填", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsRequired { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）（筛选项展示顺序）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 关联的定制报表主表
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    [Navigate(NavigateType.OneToOne, nameof(ConfigurableId))]
    public TaktConfigurable? Configurable { get; set; }
}

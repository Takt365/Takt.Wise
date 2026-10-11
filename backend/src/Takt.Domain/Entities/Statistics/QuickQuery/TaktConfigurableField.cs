// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Statistics.QuickQuery
// 文件名称：TaktConfigurableField.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：定制报表输出字段（SELECT 子句）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.Statistics.QuickQuery;

/// <summary>
/// 定制报表输出字段定义
/// </summary>
[SugarTable("takt_statistics_quick_query_configurable_field", "定制报表输出字段表")]
[SugarIndex("ix_configurable_field_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_configurable_field_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_configurable_field_report_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ConfigurableId), OrderByType.Asc, false)]
public class TaktConfigurableField : TaktCompanyEntityBase
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
    /// 显示名称（表头/Excel 列标题）
    /// </summary>
    [SugarColumn(ColumnName = "display_name", ColumnDescription = "显示名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// 输出别名（SELECT AS，为空时使用 display_name）
    /// </summary>
    [SugarColumn(ColumnName = "output_alias", ColumnDescription = "输出别名", ColumnDataType = "varchar", Length = 128, IsNullable = true)]
    public string? OutputAlias { get; set; }

    /// <summary>
    /// 聚合函数（字典 sys_configurable_aggregate_func；0=无 1=COUNT 2=SUM 3=AVG 4=MIN 5=MAX）
    /// </summary>
    [SugarColumn(ColumnName = "aggregate_func", ColumnDescription = "聚合函数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int AggregateFunc { get; set; } = 0;

    /// <summary>
    /// 是否输出（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_visible", ColumnDescription = "是否输出", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int IsVisible { get; set; } = 1;

    /// <summary>
    /// 排序号（回填）（SELECT 列顺序）
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

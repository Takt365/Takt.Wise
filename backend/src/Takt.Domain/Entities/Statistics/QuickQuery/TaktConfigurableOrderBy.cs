// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Statistics.QuickQuery
// 文件名称：TaktConfigurableOrderBy.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：定制报表排序字段（ORDER BY 子句）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.Statistics.QuickQuery;

/// <summary>
/// 定制报表排序字段定义
/// </summary>
[SugarTable("takt_statistics_quick_query_configurable_order_by", "定制报表排序表")]
[SugarIndex("ix_configurable_order_by_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_configurable_order_by_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_configurable_order_by_report_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ConfigurableId), OrderByType.Asc, false)]
public class TaktConfigurableOrderBy : TaktCompanyEntityBase
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
    /// 排序方向（字典 sys_sort_type；ASC=升序 DESC=降序）
    /// </summary>
    [SugarColumn(ColumnName = "sort_direction", ColumnDescription = "排序方向", ColumnDataType = "varchar", Length = 4, IsNullable = false, DefaultValue = "ASC")]
    public string SortDirection { get; set; } = "ASC";

    /// <summary>
    /// 排序号（回填）（ORDER BY 优先级）
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

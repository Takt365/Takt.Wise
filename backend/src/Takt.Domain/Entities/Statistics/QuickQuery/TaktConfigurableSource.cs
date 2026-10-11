// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Statistics.QuickQuery
// 文件名称：TaktConfigurableSource.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：定制报表数据源表（FROM 子句）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.Statistics.QuickQuery;

/// <summary>
/// 定制报表数据源（单表及别名）
/// </summary>
[SugarTable("takt_statistics_quick_query_configurable_source", "定制报表数据源表")]
[SugarIndex("ix_configurable_source_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_configurable_source_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_configurable_source_report_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ConfigurableId), OrderByType.Asc, false)]
public class TaktConfigurableSource : TaktCompanyEntityBase
{
    /// <summary>
    /// 关联定制报表主表 ID（选项 TaktConfigurables/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "configurable_id", ColumnDescription = "定制报表主表ID", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableId { get; set; }

    /// <summary>
    /// 数据源别名（如 A、B、C，用于 JOIN 与字段引用）
    /// </summary>
    [SugarColumn(ColumnName = "source_alias", ColumnDescription = "数据源别名", ColumnDataType = "varchar", Length = 10, IsNullable = false)]
    public string SourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 物理表名（选项 TaktDatabaseInfos/tables；DictValue=TableName；运行时 takt_ 前缀白名单校验）
    /// </summary>
    [SugarColumn(ColumnName = "table_name", ColumnDescription = "物理表名", ColumnDataType = "varchar", Length = 128, IsNullable = false)]
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 是否主表（字典 sys_yes_no；0=否 1=是；驱动 FROM 的第一张表）
    /// </summary>
    [SugarColumn(ColumnName = "is_primary", ColumnDescription = "是否主表", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsPrimary { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）（多表 FROM 顺序）
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

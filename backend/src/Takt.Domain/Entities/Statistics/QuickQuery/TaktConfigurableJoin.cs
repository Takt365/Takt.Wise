// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Statistics.QuickQuery
// 文件名称：TaktConfigurableJoin.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：定制报表多表关联（JOIN 子句）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.Statistics.QuickQuery;

/// <summary>
/// 定制报表多表关联定义
/// </summary>
[SugarTable("takt_statistics_quick_query_configurable_join", "定制报表关联表")]
[SugarIndex("ix_configurable_join_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_configurable_join_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_configurable_join_report_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ConfigurableId), OrderByType.Asc, false)]
public class TaktConfigurableJoin : TaktCompanyEntityBase
{
    /// <summary>
    /// 关联定制报表主表 ID（选项 TaktConfigurables/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "configurable_id", ColumnDescription = "定制报表主表ID", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableId { get; set; }

    /// <summary>
    /// 关联类型（字典 sys_configurable_join_type；1=内连接 2=左连接 3=右连接 4=全连接）
    /// </summary>
    [SugarColumn(ColumnName = "join_type", ColumnDescription = "关联类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int JoinType { get; set; } = 1;

    /// <summary>
    /// 左表数据源别名
    /// </summary>
    [SugarColumn(ColumnName = "left_source_alias", ColumnDescription = "左表别名", ColumnDataType = "varchar", Length = 10, IsNullable = false)]
    public string LeftSourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 左表关联列名
    /// </summary>
    [SugarColumn(ColumnName = "left_column_name", ColumnDescription = "左表关联列", ColumnDataType = "varchar", Length = 128, IsNullable = false)]
    public string LeftColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 右表数据源别名
    /// </summary>
    [SugarColumn(ColumnName = "right_source_alias", ColumnDescription = "右表别名", ColumnDataType = "varchar", Length = 10, IsNullable = false)]
    public string RightSourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 右表关联列名
    /// </summary>
    [SugarColumn(ColumnName = "right_column_name", ColumnDescription = "右表关联列", ColumnDataType = "varchar", Length = 128, IsNullable = false)]
    public string RightColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（JOIN 应用顺序）
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

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Statistics.QuickQuery
// 文件名称：TaktConfigurable.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：定制报表主表（快速查询定义头）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Shared.Constants;

namespace Takt.Domain.Entities.Statistics.QuickQuery;

/// <summary>
/// 定制报表主实体（快速查询定义）
/// </summary>
/// <remarks>
/// 主子表结构承载多表 JOIN、筛选条件、分组与排序定义；
/// 运行时由应用服务编译为 SqlSugar Queryable 并通过 ITaktStatQueryExecutor 执行，Excel 导出走 TaktExcelHelper。
/// </remarks>
[SugarTable("takt_statistics_quick_query_configurable", "定制报表主表")]
[SugarIndex("ix_configurable_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_configurable_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_configurable_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ConfigurableCode), OrderByType.Asc, true)]
[SugarIndex("ix_configurable_domain", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ConfigurableDomain), OrderByType.Asc, false)]
public class TaktConfigurable : TaktCompanyEntityBase
{
    /// <summary>
    /// 定制报表编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 定制报表编码规则生成并展示，非手输；单据类型菜单：定制报表）
    /// </summary>
    [SugarColumn(ColumnName = "configurable_code", ColumnDescription = "定制报表编码", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
    public string ConfigurableCode { get; set; } = string.Empty;
    /// <summary>
    /// 定制报表名称
    /// </summary>
    [SugarColumn(ColumnName = "configurable_name", ColumnDescription = "定制报表名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string ConfigurableName { get; set; } = string.Empty;
    /// <summary>
    /// 定制报表业务域（TaktModule；0=仪表盘 1=身份认证 2=日常事务 3=财务核算 4=后勤管理 5=人力资源 6=工作流 7=代码管理 8=基础设置 9=统计看板 10=实体；与一级菜单映射）
    /// </summary>
    [SugarColumn(ColumnName = "configurable_domain", ColumnDescription = "定制报表业务域", ColumnDataType = "int", IsNullable = false, DefaultValue = "9")]
    public int ConfigurableDomain { get; set; } = 9;
    /// <summary>
    /// 定制报表子分类（与菜单末级路由段对齐，如 management、controlling、material）
    /// </summary>
    [SugarColumn(ColumnName = "configurable_sub_category", ColumnDescription = "定制报表子分类", ColumnDataType = "varchar", Length = 50, IsNullable = true)]
    public string? ConfigurableSubCategory { get; set; }
    /// <summary>
    /// 是否去重行（字典 sys_yes_no；0=否 1=是；SELECT DISTINCT）
    /// </summary>
    [SugarColumn(ColumnName = "distinct_rows", ColumnDescription = "是否去重", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int DistinctRows { get; set; } = 1;
    /// <summary>
    /// 单次导出最大行数（TaktConfigurableConstants；DefaultRowLimit=500，MaxRowLimit=50000）
    /// </summary>
    [SugarColumn(ColumnName = "max_export_rows", ColumnDescription = "导出最大行数", ColumnDataType = "int", IsNullable = false, DefaultValue = "500")]
    public int MaxExportRows { get; set; } = TaktConfigurableConstants.DefaultRowLimit;
    /// <summary>
    /// 单次查询最大行数（TaktConfigurableConstants；DefaultRowLimit=500，MaxRowLimit=50000）
    /// </summary>
    [SugarColumn(ColumnName = "max_query_rows", ColumnDescription = "查询最大行数", ColumnDataType = "int", IsNullable = false, DefaultValue = "500")]
    public int MaxQueryRows { get; set; } = TaktConfigurableConstants.DefaultRowLimit;
    /// <summary>
    /// 公开（字典 sys_public_type；0=公开 1=私有）
    /// </summary>
    [SugarColumn(ColumnName = "is_public", ColumnDescription = "公开", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsPublic { get; set; } = 0;
    /// <summary>
    /// 定制报表描述
    /// </summary>
    [SugarColumn(ColumnName = "configurable_description", ColumnDescription = "定制报表描述", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? ConfigurableDescription { get; set; }
    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; }
    /// <summary>
    /// 定制报表状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
    /// </summary>
    [SugarColumn(ColumnName = "configurable_status", ColumnDescription = "定制报表状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ConfigurableStatus { get; set; } = 1;

    // ========================================
    // 导航属性区域（主子表 OneToMany）
    // ========================================

    /// <summary>
    /// 数据源表列表（FROM）
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    [Navigate(NavigateType.OneToMany, nameof(TaktConfigurableSource.ConfigurableId))]
    public List<TaktConfigurableSource>? Sources { get; set; }

    /// <summary>
    /// 多表关联列表（JOIN）
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    [Navigate(NavigateType.OneToMany, nameof(TaktConfigurableJoin.ConfigurableId))]
    public List<TaktConfigurableJoin>? Joins { get; set; }

    /// <summary>
    /// 输出字段列表（SELECT）
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    [Navigate(NavigateType.OneToMany, nameof(TaktConfigurableField.ConfigurableId))]
    public List<TaktConfigurableField>? Fields { get; set; }

    /// <summary>
    /// 筛选条件列表（WHERE）
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    [Navigate(NavigateType.OneToMany, nameof(TaktConfigurableSelection.ConfigurableId))]
    public List<TaktConfigurableSelection>? Selections { get; set; }

    /// <summary>
    /// 分组字段列表（GROUP BY）
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    [Navigate(NavigateType.OneToMany, nameof(TaktConfigurableGroupBy.ConfigurableId))]
    public List<TaktConfigurableGroupBy>? GroupBys { get; set; }

    /// <summary>
    /// 排序字段列表（ORDER BY）
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    [Navigate(NavigateType.OneToMany, nameof(TaktConfigurableOrderBy.ConfigurableId))]
    public List<TaktConfigurableOrderBy>? OrderBys { get; set; }
}

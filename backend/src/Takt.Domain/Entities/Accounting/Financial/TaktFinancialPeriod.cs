// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Accounting.Financial
// 文件名称：TaktFinancialPeriod.cs
// 创建时间：2026-07-06
// 创建人：Takt365(Cursor AI)
// 功能描述：财务期间实体（租户级）；按国家代码维护 FY 编码、会计期间、自然年月与财季
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Accounting.Financial;

/// <summary>
/// 财务期间（租户级主数据；按 CountryCode 区分各国财年规则）
/// 特例：继承组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase）
/// </summary>
[SugarTable("takt_accounting_financial_period", "财务期间表")]
[SugarIndex("ix_financial_period_tenant", nameof(TenantCode), OrderByType.Asc, false)]
[SugarIndex("ix_financial_period_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_accounting_financial_period_period_unique", nameof(TenantCode), OrderByType.Asc, nameof(CountryCode), OrderByType.Asc, nameof(PeriodCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_accounting_financial_period_fy_code", nameof(TenantCode), OrderByType.Asc, nameof(CountryCode), OrderByType.Asc, nameof(FinancialYearCode), OrderByType.Asc, false)]
public class TaktFinancialPeriod : TaktTenantCoreEntityBase
{
    /// <summary>
    /// 国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    [SugarColumn(ColumnName = "country_code", ColumnDescription = "国家代码", ColumnDataType = "nvarchar", Length = 2, IsNullable = false)]
    public string CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 财务年度编码（如 FY2000、FY2027；日本/香港 FY2027=2026/4/1～2027/3/31；中国 FY2027=2027/1/1～2027/12/31）
    /// </summary>
    [SugarColumn(ColumnName = "financial_year_code", ColumnDescription = "财务年度编码", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string FinancialYearCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计期间编码（YYYYMM，如 201101、202704）
    /// </summary>
    [SugarColumn(ColumnName = "period_code", ColumnDescription = "会计期间编码", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string PeriodCode { get; set; } = string.Empty;

    /// <summary>
    /// 自然年（日历年份）
    /// </summary>
    [SugarColumn(ColumnName = "calendar_year", ColumnDescription = "自然年", ColumnDataType = "int", IsNullable = false)]
    public int CalendarYear { get; set; }

    /// <summary>
    /// 自然月（1～12）
    /// </summary>
    [SugarColumn(ColumnName = "calendar_month", ColumnDescription = "自然月", ColumnDataType = "int", IsNullable = false)]
    public int CalendarMonth { get; set; }

    /// <summary>
    /// 财季编码（随国家财年规则变化；日本/香港 Q1=4～6 月；中国 Q1=1～3 月；美国 Q1=10～12 月）
    /// </summary>
    [SugarColumn(ColumnName = "financial_quarter_code", ColumnDescription = "财季编码", ColumnDataType = "varchar", Length = 2, IsNullable = false)]
    public string FinancialQuarterCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否内置（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    [SugarColumn(ColumnName = "is_built_in", ColumnDescription = "是否内置", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int IsBuiltIn { get; set; } = 1;
}

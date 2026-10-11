// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Accounting.Financial
// 文件名称：TaktProfitLoss.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：利润表行实体（对齐中国利润表列报与 IAS 1 Statement of Profit or Loss and OCI：本期/上期/本年累计、收入费用利润层次）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Accounting.Financial;

/// <summary>
/// 利润表（及综合收益）行实体（CAS 利润表列报 / IAS 1 Statement of Profit or Loss and OCI）
/// 列报层次：收入→成本费用→营业利润→利润总额→所得税→净利润→其他综合收益→综合收益总额。
/// 唯一键：租户 + 公司 + 工厂 + 期间 + 报表项目编码
/// </summary>
[SugarTable("takt_accounting_financial_profit_loss", "利润表")]
[SugarIndex("ix_profit_loss_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_profit_loss_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_accounting_financial_profit_loss_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(PeriodCode), OrderByType.Asc, nameof(StatementLineCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_accounting_financial_profit_loss_period", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PeriodCode), OrderByType.Desc, false)]
public class TaktProfitLoss : TaktCompanyEntityBase
{
    /// <summary>
    /// 会计期间编码（YYYYMM；利润表报告期）
    /// </summary>
    [SugarColumn(ColumnName = "period_code", ColumnDescription = "会计期间", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string PeriodCode { get; set; } = string.Empty;
    /// <summary>
    /// 报表项目编码（利润表/综合收益表行项目）
    /// </summary>
    [SugarColumn(ColumnName = "statement_line_code", ColumnDescription = "报表项目编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string StatementLineCode { get; set; } = string.Empty;
    /// <summary>
    /// 报表项目名称（如「营业收入」「营业成本」「净利润」「其他综合收益」）
    /// </summary>
    [SugarColumn(ColumnName = "statement_line_name", ColumnDescription = "报表项目名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string StatementLineName { get; set; } = string.Empty;
    /// <summary>
    /// 会计科目编码（可选；选项 TaktAccountTitles/options）
    /// </summary>
    [SugarColumn(ColumnName = "account_title_code", ColumnDescription = "会计科目编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? AccountTitleCode { get; set; }
    /// <summary>
    /// 会计科目名称（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "account_title_name", ColumnDescription = "会计科目名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? AccountTitleName { get; set; }
    /// <summary>
    /// 行类别（字典 accounting_financial_profit_loss_line_category；1=营业收入，2=营业成本，3=税金及附加，4=期间费用，5=其他收益损失，6=营业利润，7=营业外收支，8=利润总额，9=所得税费用，10=净利润，11=其他综合收益OCI，12=综合收益总额）
    /// </summary>
    [SugarColumn(ColumnName = "line_category", ColumnDescription = "行类别", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int LineCategory { get; set; } = 1;
    /// <summary>
    /// 是否合计/小计行（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    [SugarColumn(ColumnName = "is_total_line", ColumnDescription = "是否合计行", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsTotalLine { get; set; } = 0;
    /// <summary>
    /// 本期金额（收入类为正列报；成本费用类按公司政策为正数列报或负数列报，须与 IsExpense 一致）
    /// </summary>
    [SugarColumn(ColumnName = "period_amount", ColumnDescription = "本期金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal PeriodAmount { get; set; } = 0;
    /// <summary>
    /// 上期金额（比较信息；CAS/IAS 1）
    /// </summary>
    [SugarColumn(ColumnName = "prior_period_amount", ColumnDescription = "上期金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal PriorPeriodAmount { get; set; } = 0;
    /// <summary>
    /// 本年累计金额（中国利润表常见列；自财年期初至本期末）
    /// </summary>
    [SugarColumn(ColumnName = "year_to_date_amount", ColumnDescription = "本年累计金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal YearToDateAmount { get; set; } = 0;
    /// <summary>
    /// 是否费用/成本性质（字典 sys_yes_no；1=费用成本，计算营业利润时作减项；0=收入或其他加项）
    /// </summary>
    [SugarColumn(ColumnName = "is_expense", ColumnDescription = "是否费用性质", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsExpense { get; set; } = 0;
    /// <summary>
    /// 币种（字典 accounting_financial_currency_code）
    /// </summary>
    [SugarColumn(ColumnName = "currency_code", ColumnDescription = "币种", ColumnDataType = "varchar", Length = 3, IsNullable = false, DefaultValue = "CNY")]
    public string CurrencyCode { get; set; } = "CNY";
    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    [SugarColumn(ColumnName = "profit_loss_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ProfitLossStatus { get; set; } = 1;
}

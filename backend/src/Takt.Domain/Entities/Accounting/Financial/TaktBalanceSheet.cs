// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Accounting.Financial
// 文件名称：TaktBalanceSheet.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：资产负债表行实体（对齐中国《企业会计准则——财务报表列报》与 IAS 1 Statement of Financial Position：流动/非流动分类、比较期、资产=负债+权益）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Accounting.Financial;

/// <summary>
/// 资产负债表行实体（CAS 财务报表列报 / IAS 1 Statement of Financial Position）
/// 列报原则：资产与负债按流动/非流动分类；所有者权益单独列示；期末列报金额参与「资产=负债+权益」勾稽。
/// 唯一键：租户 + 公司 + 工厂 + 期间 + 报表项目编码
/// </summary>
[SugarTable("takt_accounting_financial_balance_sheet", "资产负债表")]
[SugarIndex("ix_balance_sheet_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_balance_sheet_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_accounting_financial_balance_sheet_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(PeriodCode), OrderByType.Asc, nameof(StatementLineCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_accounting_financial_balance_sheet_period", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PeriodCode), OrderByType.Desc, false)]
public class TaktBalanceSheet : TaktCompanyEntityBase
{
    /// <summary>
    /// 会计期间编码（YYYYMM；资产负债表日所属报告期）
    /// </summary>
    [SugarColumn(ColumnName = "period_code", ColumnDescription = "会计期间", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string PeriodCode { get; set; } = string.Empty;
    /// <summary>
    /// 报表项目编码（资产负债表行项目；可与总账科目多对一映射）
    /// </summary>
    [SugarColumn(ColumnName = "statement_line_code", ColumnDescription = "报表项目编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string StatementLineCode { get; set; } = string.Empty;
    /// <summary>
    /// 报表项目名称（如「货币资金」「应付账款」「未分配利润」）
    /// </summary>
    [SugarColumn(ColumnName = "statement_line_name", ColumnDescription = "报表项目名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string StatementLineName { get; set; } = string.Empty;
    /// <summary>
    /// 会计科目编码（可选；选项 TaktAccountTitles/options，用于追溯总账）
    /// </summary>
    [SugarColumn(ColumnName = "account_title_code", ColumnDescription = "会计科目编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? AccountTitleCode { get; set; }
    /// <summary>
    /// 会计科目名称（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "account_title_name", ColumnDescription = "会计科目名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? AccountTitleName { get; set; }
    /// <summary>
    /// 行类别（字典 accounting_financial_balance_sheet_line_category；1=流动资产，2=非流动资产，3=流动负债，4=非流动负债，5=所有者权益；对齐 CAS/IAS 1 流动非流动列报）
    /// </summary>
    [SugarColumn(ColumnName = "line_category", ColumnDescription = "行类别", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int LineCategory { get; set; } = 1;
    /// <summary>
    /// 余额方向（0=借方余额为正列报，1=贷方余额为正列报；资产多为借方，负债权益多为贷方）
    /// </summary>
    [SugarColumn(ColumnName = "balance_direction", ColumnDescription = "余额方向", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int BalanceDirection { get; set; } = 0;
    /// <summary>
    /// 是否合计/小计行（字典 sys_yes_no；1=是，0=否；合计行一般不直接来自单一科目发生额）
    /// </summary>
    [SugarColumn(ColumnName = "is_total_line", ColumnDescription = "是否合计行", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsTotalLine { get; set; } = 0;
    /// <summary>
    /// 期初余额（总账口径）
    /// </summary>
    [SugarColumn(ColumnName = "opening_balance", ColumnDescription = "期初余额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal OpeningBalance { get; set; } = 0;
    /// <summary>
    /// 本期借方发生额
    /// </summary>
    [SugarColumn(ColumnName = "debit_amount", ColumnDescription = "借方发生额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal DebitAmount { get; set; } = 0;
    /// <summary>
    /// 本期贷方发生额
    /// </summary>
    [SugarColumn(ColumnName = "credit_amount", ColumnDescription = "贷方发生额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal CreditAmount { get; set; } = 0;
    /// <summary>
    /// 期末余额（总账口径；借方余额科目≈期初+借方−贷方，贷方余额科目≈期初+贷方−借方）
    /// </summary>
    [SugarColumn(ColumnName = "closing_balance", ColumnDescription = "期末余额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ClosingBalance { get; set; } = 0;
    /// <summary>
    /// 期末列报金额（按余额方向调整后的报表数列；CAS/IAS 1 比较列报用）
    /// </summary>
    [SugarColumn(ColumnName = "presentation_amount", ColumnDescription = "期末列报金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal PresentationAmount { get; set; } = 0;
    /// <summary>
    /// 上期列报金额（比较信息；IAS 1 / CAS 要求列示比较期）
    /// </summary>
    [SugarColumn(ColumnName = "prior_period_amount", ColumnDescription = "上期列报金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal PriorPeriodAmount { get; set; } = 0;
    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；报告货币）
    /// </summary>
    [SugarColumn(ColumnName = "currency_code", ColumnDescription = "币种", ColumnDataType = "varchar", Length = 3, IsNullable = false, DefaultValue = "CNY")]
    public string CurrencyCode { get; set; } = "CNY";
    /// <summary>
    /// 排序号（回填）（越小越靠前；应与报表印刷顺序一致）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=停用）
    /// </summary>
    [SugarColumn(ColumnName = "balance_sheet_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int BalanceSheetStatus { get; set; } = 1;
}

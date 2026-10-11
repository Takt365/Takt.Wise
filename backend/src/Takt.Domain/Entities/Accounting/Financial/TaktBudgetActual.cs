// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Accounting.Financial
// 文件名称：TaktBudgetActual.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：预算实绩实体（对齐管理会计预算控制惯例与 IFRS 实务披露：本期/本年累计预算实绩差异；非法定报表主表但通用）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Accounting.Financial;

/// <summary>
/// 预算实绩实体（管理会计 Budget vs Actual；CAS 全面预算管理实务 / 国际通用管理会计）
/// 差异约定：差异金额 = 实绩 − 预算；差异率 = 差异 / |预算|（预算为 0 时为 0）。
/// 唯一键：租户 + 公司 + 工厂 + 期间 + 成本中心 + 预算项
/// </summary>
[SugarTable("takt_accounting_financial_budget_actual", "预算实绩表")]
[SugarIndex("ix_budget_actual_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_budget_actual_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_accounting_financial_budget_actual_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(PeriodCode), OrderByType.Asc, nameof(CostCenterCode), OrderByType.Asc, nameof(BudgetItemCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_accounting_financial_budget_actual_period", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PeriodCode), OrderByType.Desc, false)]
public class TaktBudgetActual : TaktCompanyEntityBase
{
    /// <summary>
    /// 会计期间编码（YYYYMM）
    /// </summary>
    [SugarColumn(ColumnName = "period_code", ColumnDescription = "会计期间", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string PeriodCode { get; set; } = string.Empty;
    /// <summary>
    /// 成本中心编码（选项 TaktCostCenters/options；空串表示公司级）
    /// </summary>
    [SugarColumn(ColumnName = "cost_center_code", ColumnDescription = "成本中心编码", ColumnDataType = "nvarchar", Length = 6, IsNullable = false, DefaultValue = "")]
    public string CostCenterCode { get; set; } = string.Empty;
    /// <summary>
    /// 成本中心名称（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "cost_center_name", ColumnDescription = "成本中心名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? CostCenterName { get; set; }
    /// <summary>
    /// 预算项编码
    /// </summary>
    [SugarColumn(ColumnName = "budget_item_code", ColumnDescription = "预算项编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string BudgetItemCode { get; set; } = string.Empty;
    /// <summary>
    /// 预算项名称
    /// </summary>
    [SugarColumn(ColumnName = "budget_item_name", ColumnDescription = "预算项名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string BudgetItemName { get; set; } = string.Empty;
    /// <summary>
    /// 会计科目编码（可选；选项 TaktAccountTitles/options）
    /// </summary>
    [SugarColumn(ColumnName = "account_title_code", ColumnDescription = "会计科目编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? AccountTitleCode { get; set; }
    /// <summary>
    /// 预算类型（字典 accounting_financial_budget_type；1=经营预算，2=资本预算，3=财务预算）
    /// </summary>
    [SugarColumn(ColumnName = "budget_type", ColumnDescription = "预算类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int BudgetType { get; set; } = 1;
    /// <summary>
    /// 计量类型（字典 accounting_financial_budget_measure_type；1=金额，2=数量）
    /// </summary>
    [SugarColumn(ColumnName = "measure_type", ColumnDescription = "计量类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int MeasureType { get; set; } = 1;
    /// <summary>
    /// 本期预算金额（或数量，视 MeasureType）
    /// </summary>
    [SugarColumn(ColumnName = "budget_amount", ColumnDescription = "本期预算", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal BudgetAmount { get; set; } = 0;
    /// <summary>
    /// 本期实绩金额（或数量）
    /// </summary>
    [SugarColumn(ColumnName = "actual_amount", ColumnDescription = "本期实绩", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ActualAmount { get; set; } = 0;
    /// <summary>
    /// 本期差异金额（= 实绩 − 预算）
    /// </summary>
    [SugarColumn(ColumnName = "variance_amount", ColumnDescription = "本期差异", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal VarianceAmount { get; set; } = 0;
    /// <summary>
    /// 本期差异率（= 差异 / |预算|；预算为 0 时为 0；小数比率如 0.05=5%）
    /// </summary>
    [SugarColumn(ColumnName = "variance_percent", ColumnDescription = "本期差异率", ColumnDataType = "decimal", Length = 18, DecimalDigits = 6, IsNullable = false, DefaultValue = "0")]
    public decimal VariancePercent { get; set; } = 0;
    /// <summary>
    /// 上年同期实绩（比较分析）
    /// </summary>
    [SugarColumn(ColumnName = "prior_period_actual", ColumnDescription = "上年同期实绩", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal PriorPeriodActual { get; set; } = 0;
    /// <summary>
    /// 本年累计预算
    /// </summary>
    [SugarColumn(ColumnName = "ytd_budget_amount", ColumnDescription = "本年累计预算", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal YtdBudgetAmount { get; set; } = 0;
    /// <summary>
    /// 本年累计实绩
    /// </summary>
    [SugarColumn(ColumnName = "ytd_actual_amount", ColumnDescription = "本年累计实绩", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal YtdActualAmount { get; set; } = 0;
    /// <summary>
    /// 本年累计差异（= 本年累计实绩 − 本年累计预算）
    /// </summary>
    [SugarColumn(ColumnName = "ytd_variance_amount", ColumnDescription = "本年累计差异", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal YtdVarianceAmount { get; set; } = 0;
    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；数量计量时可仍存报告币）
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
    [SugarColumn(ColumnName = "budget_actual_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int BudgetActualStatus { get; set; } = 1;
}

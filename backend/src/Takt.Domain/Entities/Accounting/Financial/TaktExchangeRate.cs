// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Accounting.Financial
// 文件名称：TaktExchangeRate.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Cursor AI)
// 功能描述：汇率实体（租户级主数据；租户内各公司共用；源币种/目标币种/汇率类型/生效区间）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Accounting.Financial;

/// <summary>
/// 汇率实体（租户级主数据；租户内各公司共用同一套汇率；维护自币种至目标币种的折算汇率及生效区间）
/// 特例：继承组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase）
/// </summary>
[SugarTable("takt_accounting_financial_exchange_rate", "汇率表")]
[SugarIndex("ix_exchange_rate_tenant", nameof(TenantCode), OrderByType.Asc, false)]
[SugarIndex("ix_exchange_rate_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_accounting_financial_exchange_rate_unique", nameof(TenantCode), OrderByType.Asc, nameof(FromCurrencyCode), OrderByType.Asc, nameof(ToCurrencyCode), OrderByType.Asc, nameof(ExchangeRateType), OrderByType.Asc, nameof(ValidFrom), OrderByType.Asc, true)]
[SugarIndex("ix_takt_accounting_financial_exchange_rate_from_currency", nameof(TenantCode), OrderByType.Asc, nameof(FromCurrencyCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_accounting_financial_exchange_rate_to_currency", nameof(TenantCode), OrderByType.Asc, nameof(ToCurrencyCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_accounting_financial_exchange_rate_valid_from", nameof(TenantCode), OrderByType.Asc, nameof(ValidFrom), OrderByType.Desc, false)]
public class TaktExchangeRate : TaktTenantCoreEntityBase
{
    /// <summary>
    /// 源币种（字典 accounting_financial_currency_code；ISO 4217，如 USD、CNY）
    /// </summary>
    [SugarColumn(ColumnName = "from_currency_code", ColumnDescription = "源币种", ColumnDataType = "varchar", Length = 3, IsNullable = false)]
    public string FromCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY、USD）
    /// </summary>
    [SugarColumn(ColumnName = "to_currency_code", ColumnDescription = "目标币种", ColumnDataType = "varchar", Length = 3, IsNullable = false)]
    public string ToCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 汇率类型（字典 accounting_financial_exchange_rate_type；M=平均汇率，B=银行买入价，G=历史汇率，P=计划汇率，Z=自定义汇率，E=期末汇率，K=现金余额汇率，X=平均买入价）
    /// </summary>
    [SugarColumn(ColumnName = "exchange_rate_type", ColumnDescription = "汇率类型", ColumnDataType = "varchar", Length = 1, IsNullable = false, DefaultValue = "M")]
    public string ExchangeRateType { get; set; } = "M";

    /// <summary>
    /// 汇率（decimal，精度 6 位小数；直接标价：1 单位源币种 = ExchangeRate 单位目标币种）
    /// </summary>
    [SugarColumn(ColumnName = "exchange_rate", ColumnDescription = "汇率", ColumnDataType = "decimal", Length = 18, DecimalDigits = 6, IsNullable = false, DefaultValue = "1")]
    public decimal ExchangeRate { get; set; } = 1;

    /// <summary>
    /// 源币种换算基数（配合 RatioTo 支持间接标价；默认 1）
    /// </summary>
    [SugarColumn(ColumnName = "ratio_from", ColumnDescription = "源币种基数", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int RatioFrom { get; set; } = 1;

    /// <summary>
    /// 目标币种换算基数（配合 RatioFrom 支持间接标价；默认 1）
    /// </summary>
    [SugarColumn(ColumnName = "ratio_to", ColumnDescription = "目标币种基数", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int RatioTo { get; set; } = 1;

    /// <summary>
    /// 生效日期
    /// </summary>
    [SugarColumn(ColumnName = "valid_from", ColumnDescription = "生效日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime ValidFrom { get; set; } = DateTime.Today;

    /// <summary>
    /// 失效日期
    /// </summary>
    [SugarColumn(ColumnName = "valid_to", ColumnDescription = "失效日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime ValidTo { get; set; } = new DateTime(9999, 12, 31, 23, 59, 59);

    /// <summary>
    /// 汇率状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "exchange_rate_status", ColumnDescription = "汇率状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ExchangeRateStatus { get; set; } = 1;
}

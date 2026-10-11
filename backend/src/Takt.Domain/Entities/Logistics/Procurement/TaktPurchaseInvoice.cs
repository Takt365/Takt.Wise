// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Procurement
// 文件名称：TaktPurchaseInvoice.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt采购发票主表实体（字段按 RBKP 业务清单顺序；明细见 TaktPurchaseInvoiceItem）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Procurement;

/// <summary>
/// Takt采购发票主表实体（公司级；字段按 RBKP 业务清单）
/// </summary>
[SugarTable("takt_logistics_procurement_purchase_invoice", "采购发票表")]
[SugarIndex("ix_purchase_invoice_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_purchase_invoice_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_procurement_purchase_invoice_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FiscalYear), OrderByType.Asc, nameof(PurchaseInvoiceCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_procurement_purchase_invoice_supplier_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SupplierCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_procurement_purchase_invoice_posting_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PostingDate), OrderByType.Desc, false)]
public class TaktPurchaseInvoice : TaktCompanyEntityBase
{
    /// <summary>
    /// 发票凭证编号
    /// </summary>
    [SugarColumn(ColumnName = "purchase_invoice_code", ColumnDescription = "发票凭证编号", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string PurchaseInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计年度
    /// </summary>
    [SugarColumn(ColumnName = "fiscal_year", ColumnDescription = "会计年度", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string FiscalYear { get; set; } = string.Empty;

    /// <summary>
    /// 凭证类型（字典 logistics_purchase_invoice_document_type）
    /// </summary>
    [SugarColumn(ColumnName = "document_type", ColumnDescription = "凭证类型", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? DocumentType { get; set; }

    /// <summary>
    /// 凭证日期
    /// </summary>
    [SugarColumn(ColumnName = "document_date", ColumnDescription = "凭证日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime DocumentDate { get; set; }

    /// <summary>
    /// 过帐日期
    /// </summary>
    [SugarColumn(ColumnName = "posting_date", ColumnDescription = "过帐日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime PostingDate { get; set; }

    /// <summary>
    /// 交易类型（字典 logistics_purchase_invoice_transaction_event_type）
    /// </summary>
    [SugarColumn(ColumnName = "transaction_event_type", ColumnDescription = "交易类型", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? TransactionEventType { get; set; }

    /// <summary>
    /// 参照
    /// </summary>
    [SugarColumn(ColumnName = "reference_code", ColumnDescription = "参照", ColumnDataType = "nvarchar", Length = 16, IsNullable = true)]
    public string? ReferenceCode { get; set; }

    /// <summary>
    /// 出票方（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_code", ColumnDescription = "出票方", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 货币（字典 accounting_financial_currency_code）
    /// </summary>
    [SugarColumn(ColumnName = "currency_code", ColumnDescription = "货币", ColumnDataType = "nvarchar", Length = 3, IsNullable = false, DefaultValue = "CNY")]
    public string CurrencyCode { get; set; } = "CNY";

    /// <summary>
    /// 汇率
    /// </summary>
    [SugarColumn(ColumnName = "exchange_rate", ColumnDescription = "汇率", ColumnDataType = "decimal", Length = 9, DecimalDigits = 5, IsNullable = true)]
    public decimal? ExchangeRate { get; set; }

    /// <summary>
    /// 总发票金额
    /// </summary>
    [SugarColumn(ColumnName = "gross_amount", ColumnDescription = "总发票金额", ColumnDataType = "decimal", Length = 13, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal GrossAmount { get; set; } = 0;

    /// <summary>
    /// 增值税金额
    /// </summary>
    [SugarColumn(ColumnName = "vat_amount", ColumnDescription = "增值税金额", ColumnDataType = "decimal", Length = 13, DecimalDigits = 2, IsNullable = true)]
    public decimal? VatAmount { get; set; }

    /// <summary>
    /// 税务代码
    /// </summary>
    [SugarColumn(ColumnName = "tax_jurisdiction_code", ColumnDescription = "税务代码", ColumnDataType = "nvarchar", Length = 15, IsNullable = true)]
    public string? TaxJurisdictionCode { get; set; }

    /// <summary>
    /// 天数 1（现金折扣天数）
    /// </summary>
    [SugarColumn(ColumnName = "cash_discount_days1", ColumnDescription = "天数 1", ColumnDataType = "int", IsNullable = true)]
    public int? CashDiscountDays1 { get; set; }

    /// <summary>
    /// 发票
    /// </summary>
    [SugarColumn(ColumnName = "invoice_flag", ColumnDescription = "发票", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? InvoiceFlag { get; set; }

    /// <summary>
    /// 凭证抬头文本
    /// </summary>
    [SugarColumn(ColumnName = "header_text", ColumnDescription = "凭证抬头文本", ColumnDataType = "nvarchar", Length = 25, IsNullable = true)]
    public string? HeaderText { get; set; }

    /// <summary>
    /// 冲销者（冲销凭证编号）
    /// </summary>
    [SugarColumn(ColumnName = "reversal_document_code", ColumnDescription = "冲销者", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? ReversalDocumentCode { get; set; }

    /// <summary>
    /// 年（冲销会计年度）
    /// </summary>
    [SugarColumn(ColumnName = "reversal_fiscal_year", ColumnDescription = "年", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? ReversalFiscalYear { get; set; }

    /// <summary>
    /// 税码
    /// </summary>
    [SugarColumn(ColumnName = "tax_code", ColumnDescription = "税码", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? TaxCode { get; set; }

    /// <summary>
    /// 供货国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    [SugarColumn(ColumnName = "supplying_country", ColumnDescription = "供货国家", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? SupplyingCountry { get; set; }

    /// <summary>
    /// 税率（税务汇率）
    /// </summary>
    [SugarColumn(ColumnName = "tax_exchange_rate", ColumnDescription = "税率", ColumnDataType = "decimal", Length = 9, DecimalDigits = 5, IsNullable = true)]
    public decimal? TaxExchangeRate { get; set; }

    /// <summary>
    /// 付款基准日期
    /// </summary>
    [SugarColumn(ColumnName = "baseline_date", ColumnDescription = "付款基准日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? BaselineDate { get; set; }

    /// <summary>
    /// 换算日期
    /// </summary>
    [SugarColumn(ColumnName = "exchange_rate_date", ColumnDescription = "换算日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ExchangeRateDate { get; set; }

    /// <summary>
    /// 事务代码
    /// </summary>
    [SugarColumn(ColumnName = "transaction_code", ColumnDescription = "事务代码", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? TransactionCode { get; set; }

    /// <summary>
    /// 过账人（当前登录用户对应的 EmployeeCode）
    /// </summary>
    [SugarColumn(ColumnName = "posted_by", ColumnDescription = "过账人", ColumnDataType = "nvarchar", Length = 6, IsNullable = true)]
    public string? PostedBy { get; set; }

    /// <summary>
    /// 采购发票明细列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktPurchaseInvoiceItem.PurchaseInvoiceId))]
    public List<TaktPurchaseInvoiceItem>? Items { get; set; }
}

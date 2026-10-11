// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Sales
// 文件名称：TaktSalesInvoice.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt销售发票主表实体（必要字段按开票凭证抬头清单；明细见 TaktSalesInvoiceItem）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Sales;

/// <summary>
/// Takt销售发票主表实体（公司级）
/// </summary>
[SugarTable("takt_logistics_sales_invoice", "销售发票表")]
[SugarIndex("ix_sales_invoice_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sales_invoice_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_invoice_billing_doc_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(BillingDocumentCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_sales_invoice_customer_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CustomerCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_invoice_billing_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(BillingDate), OrderByType.Desc, false)]
public class TaktSalesInvoice : TaktCompanyEntityBase
{
    /// <summary>
    /// 开票凭证
    /// </summary>
    [SugarColumn(ColumnName = "billing_document_code", ColumnDescription = "开票凭证", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string BillingDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 开票类型
    /// </summary>
    [SugarColumn(ColumnName = "billing_type", ColumnDescription = "开票类型", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? BillingType { get; set; }

    /// <summary>
    /// 出具发票类别
    /// </summary>
    [SugarColumn(ColumnName = "billing_category", ColumnDescription = "出具发票类别", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? BillingCategory { get; set; }

    /// <summary>
    /// SD 凭证类别
    /// </summary>
    [SugarColumn(ColumnName = "document_category", ColumnDescription = "SD 凭证类别", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? DocumentCategory { get; set; }

    /// <summary>
    /// 凭证货币（字典 accounting_financial_currency_code）
    /// </summary>
    [SugarColumn(ColumnName = "currency_code", ColumnDescription = "凭证货币", ColumnDataType = "nvarchar", Length = 3, IsNullable = false, DefaultValue = "CNY")]
    public string CurrencyCode { get; set; } = "CNY";

    /// <summary>
    /// 销售组织
    /// </summary>
    [SugarColumn(ColumnName = "sales_organization", ColumnDescription = "销售组织", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? SalesOrganization { get; set; }

    /// <summary>
    /// 分销渠道
    /// </summary>
    [SugarColumn(ColumnName = "distribution_channel", ColumnDescription = "分销渠道", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? DistributionChannel { get; set; }

    /// <summary>
    /// 定价过程
    /// </summary>
    [SugarColumn(ColumnName = "pricing_procedure", ColumnDescription = "定价过程", ColumnDataType = "nvarchar", Length = 6, IsNullable = true)]
    public string? PricingProcedure { get; set; }

    /// <summary>
    /// 单据条件号
    /// </summary>
    [SugarColumn(ColumnName = "condition_code", ColumnDescription = "单据条件号", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? ConditionCode { get; set; }

    /// <summary>
    /// 装运条件（字典 logistics_sales_shipping_conditions）
    /// </summary>
    [SugarColumn(ColumnName = "shipping_conditions", ColumnDescription = "装运条件", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? ShippingConditions { get; set; }

    /// <summary>
    /// 出具发票日期
    /// </summary>
    [SugarColumn(ColumnName = "billing_date", ColumnDescription = "出具发票日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime BillingDate { get; set; }

    /// <summary>
    /// 客户组
    /// </summary>
    [SugarColumn(ColumnName = "customer_group", ColumnDescription = "客户组", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? CustomerGroup { get; set; }

    /// <summary>
    /// 国际贸易条件
    /// </summary>
    [SugarColumn(ColumnName = "incoterms1", ColumnDescription = "国际贸易条件", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? Incoterms1 { get; set; }

    /// <summary>
    /// 国际贸易条件(部分2)（最长 28，故 Length=28）
    /// </summary>
    [SugarColumn(ColumnName = "incoterms2", ColumnDescription = "国际贸易条件(部分2)", ColumnDataType = "nvarchar", Length = 28, IsNullable = true)]
    public string? Incoterms2 { get; set; }

    /// <summary>
    /// 过账状态
    /// </summary>
    [SugarColumn(ColumnName = "posting_status", ColumnDescription = "过账状态", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? PostingStatus { get; set; }

    /// <summary>
    /// 会计汇率
    /// </summary>
    [SugarColumn(ColumnName = "accounting_exchange_rate", ColumnDescription = "会计汇率", ColumnDataType = "decimal", Length = 9, DecimalDigits = 5, IsNullable = true)]
    public decimal? AccountingExchangeRate { get; set; }

    /// <summary>
    /// 付款条件
    /// </summary>
    [SugarColumn(ColumnName = "payment_terms", ColumnDescription = "付款条件", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? PaymentTerms { get; set; }

    /// <summary>
    /// 客户分配帐户组别
    /// </summary>
    [SugarColumn(ColumnName = "account_assignment_group", ColumnDescription = "客户分配帐户组别", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? AccountAssignmentGroup { get; set; }

    /// <summary>
    /// 目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    [SugarColumn(ColumnName = "country_code", ColumnDescription = "目的地国家", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? CountryCode { get; set; }

    /// <summary>
    /// 净价值
    /// </summary>
    [SugarColumn(ColumnName = "net_amount", ColumnDescription = "净价值", ColumnDataType = "decimal", Length = 15, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal NetAmount { get; set; } = 0;

    /// <summary>
    /// 付款方（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    [SugarColumn(ColumnName = "payer_code", ColumnDescription = "付款方", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? PayerCode { get; set; }

    /// <summary>
    /// 售达方（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    [SugarColumn(ColumnName = "customer_code", ColumnDescription = "售达方", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 统计货币（字典 accounting_financial_currency_code）
    /// </summary>
    [SugarColumn(ColumnName = "statistics_currency_code", ColumnDescription = "统计货币", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? StatisticsCurrencyCode { get; set; }

    /// <summary>
    /// 外贸数据编号
    /// </summary>
    [SugarColumn(ColumnName = "foreign_trade_code", ColumnDescription = "外贸数据编号", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? ForeignTradeCode { get; set; }

    /// <summary>
    /// 已取消的开票凭证
    /// </summary>
    [SugarColumn(ColumnName = "cancelled_billing_document", ColumnDescription = "已取消的开票凭证", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? CancelledBillingDocument { get; set; }

    /// <summary>
    /// 发票清单类型
    /// </summary>
    [SugarColumn(ColumnName = "invoice_list_type", ColumnDescription = "发票清单类型", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? InvoiceListType { get; set; }

    /// <summary>
    /// 产品组
    /// </summary>
    [SugarColumn(ColumnName = "division", ColumnDescription = "产品组", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? Division { get; set; }

    /// <summary>
    /// 定价的层次类型
    /// </summary>
    [SugarColumn(ColumnName = "hierarchy_type_pricing", ColumnDescription = "定价的层次类型", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? HierarchyTypePricing { get; set; }

    /// <summary>
    /// 贸易伙伴
    /// </summary>
    [SugarColumn(ColumnName = "trading_partner", ColumnDescription = "贸易伙伴", ColumnDataType = "nvarchar", Length = 6, IsNullable = true)]
    public string? TradingPartner { get; set; }

    /// <summary>
    /// 征税国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    [SugarColumn(ColumnName = "tax_departure_country", ColumnDescription = "征税国家", ColumnDataType = "nvarchar", Length = 3, IsNullable = true)]
    public string? TaxDepartureCountry { get; set; }

    /// <summary>
    /// 组织销售税编号
    /// </summary>
    [SugarColumn(ColumnName = "organization_sales_tax_number", ColumnDescription = "组织销售税编号", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? OrganizationSalesTaxNumber { get; set; }

    /// <summary>
    /// 国家销售税编号
    /// </summary>
    [SugarColumn(ColumnName = "country_sales_tax_number", ColumnDescription = "国家销售税编号", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? CountrySalesTaxNumber { get; set; }

    /// <summary>
    /// 参考（最长 16，故 Length=16）
    /// </summary>
    [SugarColumn(ColumnName = "reference_code", ColumnDescription = "参考", ColumnDataType = "nvarchar", Length = 16, IsNullable = true)]
    public string? ReferenceCode { get; set; }

    /// <summary>
    /// 已被取消
    /// </summary>
    [SugarColumn(ColumnName = "cancelled_flag", ColumnDescription = "已被取消", ColumnDataType = "nvarchar", Length = 1, IsNullable = true)]
    public string? CancelledFlag { get; set; }

    /// <summary>
    /// 换算日期
    /// </summary>
    [SugarColumn(ColumnName = "exchange_rate_date", ColumnDescription = "换算日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ExchangeRateDate { get; set; }

    /// <summary>
    /// 付款参考（最长 30，故 Length=30）
    /// </summary>
    [SugarColumn(ColumnName = "payment_reference", ColumnDescription = "付款参考", ColumnDataType = "nvarchar", Length = 30, IsNullable = true)]
    public string? PaymentReference { get; set; }

    /// <summary>
    /// 冲销原因
    /// </summary>
    [SugarColumn(ColumnName = "reversal_reason", ColumnDescription = "冲销原因", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? ReversalReason { get; set; }

    /// <summary>
    /// 过账人（当前登录用户对应的 EmployeeCode）
    /// </summary>
    [SugarColumn(ColumnName = "posted_by", ColumnDescription = "过账人", ColumnDataType = "nvarchar", Length = 6, IsNullable = true)]
    public string? PostedBy { get; set; }

    /// <summary>
    /// 销售发票明细列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktSalesInvoiceItem.SalesInvoiceId))]
    public List<TaktSalesInvoiceItem>? Items { get; set; }
}

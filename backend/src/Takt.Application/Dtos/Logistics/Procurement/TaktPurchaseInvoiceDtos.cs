// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Procurement
// 文件名称：TaktPurchaseInvoiceDtos.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Auto Generated)
// 功能描述：PurchaseInvoice 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPurchaseInvoice 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Procurement;

// ========================================
// PurchaseInvoice 响应 DTO
// ========================================

/// <summary>
/// Takt采购发票主表实体（公司级；字段按 RBKP 业务清单）
/// 对应前端 TaktPurchaseInvoiceDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPurchaseInvoiceDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PurchaseInvoiceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInvoiceId { get; set; }

    /// <summary>
    /// 发票凭证编号
    /// </summary>
    public string PurchaseInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计年度
    /// </summary>
    public string FiscalYear { get; set; } = string.Empty;

    /// <summary>
    /// 凭证类型（字典 logistics_purchase_invoice_document_type）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 凭证日期
    /// </summary>
    public DateTime DocumentDate { get; set; }

    /// <summary>
    /// 过帐日期
    /// </summary>
    public DateTime PostingDate { get; set; }

    /// <summary>
    /// 交易类型（字典 logistics_purchase_invoice_transaction_event_type）
    /// </summary>
    public string? TransactionEventType { get; set; } = string.Empty;

    /// <summary>
    /// 参照
    /// </summary>
    public string? ReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 出票方（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 货币（字典 accounting_financial_currency_code）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 汇率
    /// </summary>
    public decimal? ExchangeRate { get; set; }

    /// <summary>
    /// 总发票金额
    /// </summary>
    public decimal GrossAmount { get; set; }

    /// <summary>
    /// 增值税金额
    /// </summary>
    public decimal? VatAmount { get; set; }

    /// <summary>
    /// 税务代码
    /// </summary>
    public string? TaxJurisdictionCode { get; set; } = string.Empty;

    /// <summary>
    /// 天数 1（现金折扣天数）
    /// </summary>
    public int? CashDiscountDays1 { get; set; }

    /// <summary>
    /// 发票
    /// </summary>
    public string? InvoiceFlag { get; set; } = string.Empty;

    /// <summary>
    /// 凭证抬头文本
    /// </summary>
    public string? HeaderText { get; set; } = string.Empty;

    /// <summary>
    /// 冲销者（冲销凭证编号）
    /// </summary>
    public string? ReversalDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 年（冲销会计年度）
    /// </summary>
    public string? ReversalFiscalYear { get; set; } = string.Empty;

    /// <summary>
    /// 税码
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? SupplyingCountry { get; set; } = string.Empty;

    /// <summary>
    /// 税率（税务汇率）
    /// </summary>
    public decimal? TaxExchangeRate { get; set; }

    /// <summary>
    /// 付款基准日期
    /// </summary>
    public DateTime? BaselineDate { get; set; }

    /// <summary>
    /// 换算日期
    /// </summary>
    public DateTime? ExchangeRateDate { get; set; }

    /// <summary>
    /// 事务代码
    /// </summary>
    public string? TransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// 过账人（当前登录用户对应的 EmployeeCode）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// 采购发票明细列表（主子表关系）
    /// （子表：TaktPurchaseInvoiceItem）
    /// </summary>
    public List<TaktPurchaseInvoiceItemDto>? Items { get; set; }

}

// ========================================
// PurchaseInvoice 查询 DTO
// ========================================

/// <summary>
/// PurchaseInvoice 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPurchaseInvoiceQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 发票凭证编号
    /// </summary>
    public string? PurchaseInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计年度
    /// </summary>
    public string? FiscalYear { get; set; } = string.Empty;

    /// <summary>
    /// 凭证类型（字典 logistics_purchase_invoice_document_type）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 凭证日期（范围查询-开始）
    /// </summary>
    public DateTime? DocumentDateStart { get; set; }

    /// <summary>
    /// 凭证日期（范围查询-结束）
    /// </summary>
    public DateTime? DocumentDateEnd { get; set; }

    /// <summary>
    /// 过帐日期（范围查询-开始）
    /// </summary>
    public DateTime? PostingDateStart { get; set; }

    /// <summary>
    /// 过帐日期（范围查询-结束）
    /// </summary>
    public DateTime? PostingDateEnd { get; set; }

    /// <summary>
    /// 交易类型（字典 logistics_purchase_invoice_transaction_event_type）
    /// </summary>
    public string? TransactionEventType { get; set; } = string.Empty;

    /// <summary>
    /// 参照
    /// </summary>
    public string? ReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 出票方（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 货币（字典 accounting_financial_currency_code）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 汇率
    /// </summary>
    public decimal? ExchangeRate { get; set; }

    /// <summary>
    /// 总发票金额
    /// </summary>
    public decimal? GrossAmount { get; set; }

    /// <summary>
    /// 增值税金额
    /// </summary>
    public decimal? VatAmount { get; set; }

    /// <summary>
    /// 税务代码
    /// </summary>
    public string? TaxJurisdictionCode { get; set; } = string.Empty;

    /// <summary>
    /// 天数 1（现金折扣天数）
    /// </summary>
    public int? CashDiscountDays1 { get; set; }

    /// <summary>
    /// 发票
    /// </summary>
    public string? InvoiceFlag { get; set; } = string.Empty;

    /// <summary>
    /// 凭证抬头文本
    /// </summary>
    public string? HeaderText { get; set; } = string.Empty;

    /// <summary>
    /// 冲销者（冲销凭证编号）
    /// </summary>
    public string? ReversalDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 年（冲销会计年度）
    /// </summary>
    public string? ReversalFiscalYear { get; set; } = string.Empty;

    /// <summary>
    /// 税码
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? SupplyingCountry { get; set; } = string.Empty;

    /// <summary>
    /// 税率（税务汇率）
    /// </summary>
    public decimal? TaxExchangeRate { get; set; }

    /// <summary>
    /// 付款基准日期（范围查询-开始）
    /// </summary>
    public DateTime? BaselineDateStart { get; set; }

    /// <summary>
    /// 付款基准日期（范围查询-结束）
    /// </summary>
    public DateTime? BaselineDateEnd { get; set; }

    /// <summary>
    /// 换算日期（范围查询-开始）
    /// </summary>
    public DateTime? ExchangeRateDateStart { get; set; }

    /// <summary>
    /// 换算日期（范围查询-结束）
    /// </summary>
    public DateTime? ExchangeRateDateEnd { get; set; }

    /// <summary>
    /// 事务代码
    /// </summary>
    public string? TransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// 过账人（当前登录用户对应的 EmployeeCode）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建PurchaseInvoice DTO
// ========================================

/// <summary>
/// 创建PurchaseInvoice DTO
/// </summary>
public class TaktPurchaseInvoiceCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 发票凭证编号
    /// </summary>
    [Required(ErrorMessage = "发票凭证编号不能为空")]
    public string PurchaseInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计年度
    /// </summary>
    [Required(ErrorMessage = "会计年度不能为空")]
    public string FiscalYear { get; set; } = string.Empty;

    /// <summary>
    /// 凭证类型（字典 logistics_purchase_invoice_document_type）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 凭证日期
    /// </summary>
    public DateTime DocumentDate { get; set; }

    /// <summary>
    /// 过帐日期
    /// </summary>
    public DateTime PostingDate { get; set; }

    /// <summary>
    /// 交易类型（字典 logistics_purchase_invoice_transaction_event_type）
    /// </summary>
    public string? TransactionEventType { get; set; } = string.Empty;

    /// <summary>
    /// 参照
    /// </summary>
    public string? ReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 出票方（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    [Required(ErrorMessage = "出票方（选项 TaktSuppliers/options；DictValue=SupplierCode）不能为空")]
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 货币（字典 accounting_financial_currency_code）
    /// </summary>
    [Required(ErrorMessage = "货币（字典 accounting_financial_currency_code）不能为空")]
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 汇率
    /// </summary>
    public decimal? ExchangeRate { get; set; }

    /// <summary>
    /// 总发票金额
    /// </summary>
    public decimal GrossAmount { get; set; }

    /// <summary>
    /// 增值税金额
    /// </summary>
    public decimal? VatAmount { get; set; }

    /// <summary>
    /// 税务代码
    /// </summary>
    public string? TaxJurisdictionCode { get; set; } = string.Empty;

    /// <summary>
    /// 天数 1（现金折扣天数）
    /// </summary>
    public int? CashDiscountDays1 { get; set; }

    /// <summary>
    /// 发票
    /// </summary>
    public string? InvoiceFlag { get; set; } = string.Empty;

    /// <summary>
    /// 凭证抬头文本
    /// </summary>
    public string? HeaderText { get; set; } = string.Empty;

    /// <summary>
    /// 冲销者（冲销凭证编号）
    /// </summary>
    public string? ReversalDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 年（冲销会计年度）
    /// </summary>
    public string? ReversalFiscalYear { get; set; } = string.Empty;

    /// <summary>
    /// 税码
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? SupplyingCountry { get; set; } = string.Empty;

    /// <summary>
    /// 税率（税务汇率）
    /// </summary>
    public decimal? TaxExchangeRate { get; set; }

    /// <summary>
    /// 付款基准日期
    /// </summary>
    public DateTime? BaselineDate { get; set; }

    /// <summary>
    /// 换算日期
    /// </summary>
    public DateTime? ExchangeRateDate { get; set; }

    /// <summary>
    /// 事务代码
    /// </summary>
    public string? TransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// 过账人（当前登录用户对应的 EmployeeCode）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// 采购发票明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktPurchaseInvoiceItemCreateDto>? Items { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新PurchaseInvoice DTO
// ========================================

/// <summary>
/// 更新PurchaseInvoice DTO
/// 继承 TaktPurchaseInvoiceCreateDto，添加 PurchaseInvoiceId 字段
/// </summary>
public class TaktPurchaseInvoiceUpdateDto : TaktPurchaseInvoiceCreateDto
{
    /// <summary>
    /// PurchaseInvoiceID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInvoiceId { get; set; }

    /// <summary>
    /// 采购发票明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public new List<TaktPurchaseInvoiceItemUpdateDto>? Items { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PurchaseInvoice 导入模板行 DTO
/// </summary>
public class TaktPurchaseInvoiceTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 发票凭证编号
    /// </summary>
    public string? PurchaseInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计年度
    /// </summary>
    public string? FiscalYear { get; set; } = string.Empty;

    /// <summary>
    /// 凭证类型（字典 logistics_purchase_invoice_document_type）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 凭证日期
    /// </summary>
    public DateTime? DocumentDate { get; set; }

    /// <summary>
    /// 过帐日期
    /// </summary>
    public DateTime? PostingDate { get; set; }

    /// <summary>
    /// 交易类型（字典 logistics_purchase_invoice_transaction_event_type）
    /// </summary>
    public string? TransactionEventType { get; set; } = string.Empty;

    /// <summary>
    /// 参照
    /// </summary>
    public string? ReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 出票方（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 货币（字典 accounting_financial_currency_code）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 汇率
    /// </summary>
    public decimal? ExchangeRate { get; set; }

    /// <summary>
    /// 总发票金额
    /// </summary>
    public decimal? GrossAmount { get; set; }

    /// <summary>
    /// 增值税金额
    /// </summary>
    public decimal? VatAmount { get; set; }

    /// <summary>
    /// 税务代码
    /// </summary>
    public string? TaxJurisdictionCode { get; set; } = string.Empty;

    /// <summary>
    /// 天数 1（现金折扣天数）
    /// </summary>
    public int? CashDiscountDays1 { get; set; }

    /// <summary>
    /// 发票
    /// </summary>
    public string? InvoiceFlag { get; set; } = string.Empty;

    /// <summary>
    /// 凭证抬头文本
    /// </summary>
    public string? HeaderText { get; set; } = string.Empty;

    /// <summary>
    /// 冲销者（冲销凭证编号）
    /// </summary>
    public string? ReversalDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 年（冲销会计年度）
    /// </summary>
    public string? ReversalFiscalYear { get; set; } = string.Empty;

    /// <summary>
    /// 税码
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? SupplyingCountry { get; set; } = string.Empty;

    /// <summary>
    /// 税率（税务汇率）
    /// </summary>
    public decimal? TaxExchangeRate { get; set; }

    /// <summary>
    /// 付款基准日期
    /// </summary>
    public DateTime? BaselineDate { get; set; }

    /// <summary>
    /// 换算日期
    /// </summary>
    public DateTime? ExchangeRateDate { get; set; }

    /// <summary>
    /// 事务代码
    /// </summary>
    public string? TransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// 过账人（当前登录用户对应的 EmployeeCode）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// 采购发票明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktPurchaseInvoiceItemCreateDto>? Items { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// PurchaseInvoice 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPurchaseInvoiceImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 发票凭证编号
    /// </summary>
    public string? PurchaseInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计年度
    /// </summary>
    public string? FiscalYear { get; set; } = string.Empty;

    /// <summary>
    /// 凭证类型（字典 logistics_purchase_invoice_document_type）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 凭证日期
    /// </summary>
    public DateTime? DocumentDate { get; set; }

    /// <summary>
    /// 过帐日期
    /// </summary>
    public DateTime? PostingDate { get; set; }

    /// <summary>
    /// 交易类型（字典 logistics_purchase_invoice_transaction_event_type）
    /// </summary>
    public string? TransactionEventType { get; set; } = string.Empty;

    /// <summary>
    /// 参照
    /// </summary>
    public string? ReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 出票方（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 货币（字典 accounting_financial_currency_code）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 汇率
    /// </summary>
    public decimal? ExchangeRate { get; set; }

    /// <summary>
    /// 总发票金额
    /// </summary>
    public decimal? GrossAmount { get; set; }

    /// <summary>
    /// 增值税金额
    /// </summary>
    public decimal? VatAmount { get; set; }

    /// <summary>
    /// 税务代码
    /// </summary>
    public string? TaxJurisdictionCode { get; set; } = string.Empty;

    /// <summary>
    /// 天数 1（现金折扣天数）
    /// </summary>
    public int? CashDiscountDays1 { get; set; }

    /// <summary>
    /// 发票
    /// </summary>
    public string? InvoiceFlag { get; set; } = string.Empty;

    /// <summary>
    /// 凭证抬头文本
    /// </summary>
    public string? HeaderText { get; set; } = string.Empty;

    /// <summary>
    /// 冲销者（冲销凭证编号）
    /// </summary>
    public string? ReversalDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 年（冲销会计年度）
    /// </summary>
    public string? ReversalFiscalYear { get; set; } = string.Empty;

    /// <summary>
    /// 税码
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? SupplyingCountry { get; set; } = string.Empty;

    /// <summary>
    /// 税率（税务汇率）
    /// </summary>
    public decimal? TaxExchangeRate { get; set; }

    /// <summary>
    /// 付款基准日期
    /// </summary>
    public DateTime? BaselineDate { get; set; }

    /// <summary>
    /// 换算日期
    /// </summary>
    public DateTime? ExchangeRateDate { get; set; }

    /// <summary>
    /// 事务代码
    /// </summary>
    public string? TransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// 过账人（当前登录用户对应的 EmployeeCode）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// 采购发票明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktPurchaseInvoiceItemCreateDto>? Items { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// PurchaseInvoice 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPurchaseInvoiceExportDto
{
    /// <summary>
    /// PurchaseInvoiceID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchaseInvoiceId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 发票凭证编号
    /// </summary>
    public string PurchaseInvoiceCode { get; set; } = string.Empty;

    /// <summary>
    /// 会计年度
    /// </summary>
    public string FiscalYear { get; set; } = string.Empty;

    /// <summary>
    /// 凭证类型（字典 logistics_purchase_invoice_document_type）
    /// </summary>
    public string? DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// 凭证日期
    /// </summary>
    public DateTime DocumentDate { get; set; }

    /// <summary>
    /// 过帐日期
    /// </summary>
    public DateTime PostingDate { get; set; }

    /// <summary>
    /// 交易类型（字典 logistics_purchase_invoice_transaction_event_type）
    /// </summary>
    public string? TransactionEventType { get; set; } = string.Empty;

    /// <summary>
    /// 参照
    /// </summary>
    public string? ReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 出票方（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 货币（字典 accounting_financial_currency_code）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 汇率
    /// </summary>
    public decimal? ExchangeRate { get; set; }

    /// <summary>
    /// 总发票金额
    /// </summary>
    public decimal GrossAmount { get; set; }

    /// <summary>
    /// 增值税金额
    /// </summary>
    public decimal? VatAmount { get; set; }

    /// <summary>
    /// 税务代码
    /// </summary>
    public string? TaxJurisdictionCode { get; set; } = string.Empty;

    /// <summary>
    /// 天数 1（现金折扣天数）
    /// </summary>
    public int? CashDiscountDays1 { get; set; }

    /// <summary>
    /// 发票
    /// </summary>
    public string? InvoiceFlag { get; set; } = string.Empty;

    /// <summary>
    /// 凭证抬头文本
    /// </summary>
    public string? HeaderText { get; set; } = string.Empty;

    /// <summary>
    /// 冲销者（冲销凭证编号）
    /// </summary>
    public string? ReversalDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 年（冲销会计年度）
    /// </summary>
    public string? ReversalFiscalYear { get; set; } = string.Empty;

    /// <summary>
    /// 税码
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? SupplyingCountry { get; set; } = string.Empty;

    /// <summary>
    /// 税率（税务汇率）
    /// </summary>
    public decimal? TaxExchangeRate { get; set; }

    /// <summary>
    /// 付款基准日期
    /// </summary>
    public DateTime? BaselineDate { get; set; }

    /// <summary>
    /// 换算日期
    /// </summary>
    public DateTime? ExchangeRateDate { get; set; }

    /// <summary>
    /// 事务代码
    /// </summary>
    public string? TransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// 过账人（当前登录用户对应的 EmployeeCode）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

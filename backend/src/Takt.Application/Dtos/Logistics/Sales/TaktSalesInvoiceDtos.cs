// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Sales
// 文件名称：TaktSalesInvoiceDtos.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesInvoice 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSalesInvoice 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Sales;

// ========================================
// SalesInvoice 响应 DTO
// ========================================

/// <summary>
/// Takt销售发票主表实体（公司级）
/// 对应前端 TaktSalesInvoiceDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSalesInvoiceDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SalesInvoiceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesInvoiceId { get; set; }

    /// <summary>
    /// 开票凭证
    /// </summary>
    public string BillingDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 开票类型
    /// </summary>
    public string? BillingType { get; set; } = string.Empty;

    /// <summary>
    /// 出具发票类别
    /// </summary>
    public string? BillingCategory { get; set; } = string.Empty;

    /// <summary>
    /// SD 凭证类别
    /// </summary>
    public string? DocumentCategory { get; set; } = string.Empty;

    /// <summary>
    /// 凭证货币（字典 accounting_financial_currency_code）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售组织
    /// </summary>
    public string? SalesOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 分销渠道
    /// </summary>
    public string? DistributionChannel { get; set; } = string.Empty;

    /// <summary>
    /// 定价过程
    /// </summary>
    public string? PricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 单据条件号
    /// </summary>
    public string? ConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 装运条件（字典 logistics_sales_shipping_conditions）
    /// </summary>
    public string? ShippingConditions { get; set; } = string.Empty;

    /// <summary>
    /// 出具发票日期
    /// </summary>
    public DateTime BillingDate { get; set; }

    /// <summary>
    /// 客户组
    /// </summary>
    public string? CustomerGroup { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条件
    /// </summary>
    public string? Incoterms1 { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条件(部分2)（最长 28，故 Length=28）
    /// </summary>
    public string? Incoterms2 { get; set; } = string.Empty;

    /// <summary>
    /// 过账状态
    /// </summary>
    public string? PostingStatus { get; set; } = string.Empty;

    /// <summary>
    /// 会计汇率
    /// </summary>
    public decimal? AccountingExchangeRate { get; set; }

    /// <summary>
    /// 付款条件
    /// </summary>
    public string? PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 客户分配帐户组别
    /// </summary>
    public string? AccountAssignmentGroup { get; set; } = string.Empty;

    /// <summary>
    /// 目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 净价值
    /// </summary>
    public decimal NetAmount { get; set; }

    /// <summary>
    /// 付款方（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? PayerCode { get; set; } = string.Empty;

    /// <summary>
    /// 售达方（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 统计货币（字典 accounting_financial_currency_code）
    /// </summary>
    public string? StatisticsCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 外贸数据编号
    /// </summary>
    public string? ForeignTradeCode { get; set; } = string.Empty;

    /// <summary>
    /// 已取消的开票凭证
    /// </summary>
    public string? CancelledBillingDocument { get; set; } = string.Empty;

    /// <summary>
    /// 发票清单类型
    /// </summary>
    public string? InvoiceListType { get; set; } = string.Empty;

    /// <summary>
    /// 产品组
    /// </summary>
    public string? Division { get; set; } = string.Empty;

    /// <summary>
    /// 定价的层次类型
    /// </summary>
    public string? HierarchyTypePricing { get; set; } = string.Empty;

    /// <summary>
    /// 贸易伙伴
    /// </summary>
    public string? TradingPartner { get; set; } = string.Empty;

    /// <summary>
    /// 征税国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? TaxDepartureCountry { get; set; } = string.Empty;

    /// <summary>
    /// 组织销售税编号
    /// </summary>
    public string? OrganizationSalesTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 国家销售税编号
    /// </summary>
    public string? CountrySalesTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 参考（最长 16，故 Length=16）
    /// </summary>
    public string? ReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 已被取消
    /// </summary>
    public string? CancelledFlag { get; set; } = string.Empty;

    /// <summary>
    /// 换算日期
    /// </summary>
    public DateTime? ExchangeRateDate { get; set; }

    /// <summary>
    /// 付款参考（最长 30，故 Length=30）
    /// </summary>
    public string? PaymentReference { get; set; } = string.Empty;

    /// <summary>
    /// 冲销原因
    /// </summary>
    public string? ReversalReason { get; set; } = string.Empty;

    /// <summary>
    /// 过账人（当前登录用户对应的 EmployeeCode）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// 销售发票明细列表（主子表关系）
    /// （子表：TaktSalesInvoiceItem）
    /// </summary>
    public List<TaktSalesInvoiceItemDto>? Items { get; set; }

}

// ========================================
// SalesInvoice 查询 DTO
// ========================================

/// <summary>
/// SalesInvoice 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSalesInvoiceQueryDto : TaktPagedQuery
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
    /// 开票凭证
    /// </summary>
    public string? BillingDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 开票类型
    /// </summary>
    public string? BillingType { get; set; } = string.Empty;

    /// <summary>
    /// 出具发票类别
    /// </summary>
    public string? BillingCategory { get; set; } = string.Empty;

    /// <summary>
    /// SD 凭证类别
    /// </summary>
    public string? DocumentCategory { get; set; } = string.Empty;

    /// <summary>
    /// 凭证货币（字典 accounting_financial_currency_code）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售组织
    /// </summary>
    public string? SalesOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 分销渠道
    /// </summary>
    public string? DistributionChannel { get; set; } = string.Empty;

    /// <summary>
    /// 定价过程
    /// </summary>
    public string? PricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 单据条件号
    /// </summary>
    public string? ConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 装运条件（字典 logistics_sales_shipping_conditions）
    /// </summary>
    public string? ShippingConditions { get; set; } = string.Empty;

    /// <summary>
    /// 出具发票日期（范围查询-开始）
    /// </summary>
    public DateTime? BillingDateStart { get; set; }

    /// <summary>
    /// 出具发票日期（范围查询-结束）
    /// </summary>
    public DateTime? BillingDateEnd { get; set; }

    /// <summary>
    /// 客户组
    /// </summary>
    public string? CustomerGroup { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条件
    /// </summary>
    public string? Incoterms1 { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条件(部分2)（最长 28，故 Length=28）
    /// </summary>
    public string? Incoterms2 { get; set; } = string.Empty;

    /// <summary>
    /// 过账状态
    /// </summary>
    public string? PostingStatus { get; set; } = string.Empty;

    /// <summary>
    /// 会计汇率
    /// </summary>
    public decimal? AccountingExchangeRate { get; set; }

    /// <summary>
    /// 付款条件
    /// </summary>
    public string? PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 客户分配帐户组别
    /// </summary>
    public string? AccountAssignmentGroup { get; set; } = string.Empty;

    /// <summary>
    /// 目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 净价值
    /// </summary>
    public decimal? NetAmount { get; set; }

    /// <summary>
    /// 付款方（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? PayerCode { get; set; } = string.Empty;

    /// <summary>
    /// 售达方（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 统计货币（字典 accounting_financial_currency_code）
    /// </summary>
    public string? StatisticsCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 外贸数据编号
    /// </summary>
    public string? ForeignTradeCode { get; set; } = string.Empty;

    /// <summary>
    /// 已取消的开票凭证
    /// </summary>
    public string? CancelledBillingDocument { get; set; } = string.Empty;

    /// <summary>
    /// 发票清单类型
    /// </summary>
    public string? InvoiceListType { get; set; } = string.Empty;

    /// <summary>
    /// 产品组
    /// </summary>
    public string? Division { get; set; } = string.Empty;

    /// <summary>
    /// 定价的层次类型
    /// </summary>
    public string? HierarchyTypePricing { get; set; } = string.Empty;

    /// <summary>
    /// 贸易伙伴
    /// </summary>
    public string? TradingPartner { get; set; } = string.Empty;

    /// <summary>
    /// 征税国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? TaxDepartureCountry { get; set; } = string.Empty;

    /// <summary>
    /// 组织销售税编号
    /// </summary>
    public string? OrganizationSalesTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 国家销售税编号
    /// </summary>
    public string? CountrySalesTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 参考（最长 16，故 Length=16）
    /// </summary>
    public string? ReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 已被取消
    /// </summary>
    public string? CancelledFlag { get; set; } = string.Empty;

    /// <summary>
    /// 换算日期（范围查询-开始）
    /// </summary>
    public DateTime? ExchangeRateDateStart { get; set; }

    /// <summary>
    /// 换算日期（范围查询-结束）
    /// </summary>
    public DateTime? ExchangeRateDateEnd { get; set; }

    /// <summary>
    /// 付款参考（最长 30，故 Length=30）
    /// </summary>
    public string? PaymentReference { get; set; } = string.Empty;

    /// <summary>
    /// 冲销原因
    /// </summary>
    public string? ReversalReason { get; set; } = string.Empty;

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
// 创建SalesInvoice DTO
// ========================================

/// <summary>
/// 创建SalesInvoice DTO
/// </summary>
public class TaktSalesInvoiceCreateDto
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
    /// 开票凭证
    /// </summary>
    [Required(ErrorMessage = "开票凭证不能为空")]
    public string BillingDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 开票类型
    /// </summary>
    public string? BillingType { get; set; } = string.Empty;

    /// <summary>
    /// 出具发票类别
    /// </summary>
    public string? BillingCategory { get; set; } = string.Empty;

    /// <summary>
    /// SD 凭证类别
    /// </summary>
    public string? DocumentCategory { get; set; } = string.Empty;

    /// <summary>
    /// 凭证货币（字典 accounting_financial_currency_code）
    /// </summary>
    [Required(ErrorMessage = "凭证货币（字典 accounting_financial_currency_code）不能为空")]
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售组织
    /// </summary>
    public string? SalesOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 分销渠道
    /// </summary>
    public string? DistributionChannel { get; set; } = string.Empty;

    /// <summary>
    /// 定价过程
    /// </summary>
    public string? PricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 单据条件号
    /// </summary>
    public string? ConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 装运条件（字典 logistics_sales_shipping_conditions）
    /// </summary>
    public string? ShippingConditions { get; set; } = string.Empty;

    /// <summary>
    /// 出具发票日期
    /// </summary>
    public DateTime BillingDate { get; set; }

    /// <summary>
    /// 客户组
    /// </summary>
    public string? CustomerGroup { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条件
    /// </summary>
    public string? Incoterms1 { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条件(部分2)（最长 28，故 Length=28）
    /// </summary>
    public string? Incoterms2 { get; set; } = string.Empty;

    /// <summary>
    /// 过账状态
    /// </summary>
    public string? PostingStatus { get; set; } = string.Empty;

    /// <summary>
    /// 会计汇率
    /// </summary>
    public decimal? AccountingExchangeRate { get; set; }

    /// <summary>
    /// 付款条件
    /// </summary>
    public string? PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 客户分配帐户组别
    /// </summary>
    public string? AccountAssignmentGroup { get; set; } = string.Empty;

    /// <summary>
    /// 目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 净价值
    /// </summary>
    public decimal NetAmount { get; set; }

    /// <summary>
    /// 付款方（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? PayerCode { get; set; } = string.Empty;

    /// <summary>
    /// 售达方（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    [Required(ErrorMessage = "售达方（选项 TaktCustomers/options；DictValue=CustomerCode）不能为空")]
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 统计货币（字典 accounting_financial_currency_code）
    /// </summary>
    public string? StatisticsCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 外贸数据编号
    /// </summary>
    public string? ForeignTradeCode { get; set; } = string.Empty;

    /// <summary>
    /// 已取消的开票凭证
    /// </summary>
    public string? CancelledBillingDocument { get; set; } = string.Empty;

    /// <summary>
    /// 发票清单类型
    /// </summary>
    public string? InvoiceListType { get; set; } = string.Empty;

    /// <summary>
    /// 产品组
    /// </summary>
    public string? Division { get; set; } = string.Empty;

    /// <summary>
    /// 定价的层次类型
    /// </summary>
    public string? HierarchyTypePricing { get; set; } = string.Empty;

    /// <summary>
    /// 贸易伙伴
    /// </summary>
    public string? TradingPartner { get; set; } = string.Empty;

    /// <summary>
    /// 征税国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? TaxDepartureCountry { get; set; } = string.Empty;

    /// <summary>
    /// 组织销售税编号
    /// </summary>
    public string? OrganizationSalesTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 国家销售税编号
    /// </summary>
    public string? CountrySalesTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 参考（最长 16，故 Length=16）
    /// </summary>
    public string? ReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 已被取消
    /// </summary>
    public string? CancelledFlag { get; set; } = string.Empty;

    /// <summary>
    /// 换算日期
    /// </summary>
    public DateTime? ExchangeRateDate { get; set; }

    /// <summary>
    /// 付款参考（最长 30，故 Length=30）
    /// </summary>
    public string? PaymentReference { get; set; } = string.Empty;

    /// <summary>
    /// 冲销原因
    /// </summary>
    public string? ReversalReason { get; set; } = string.Empty;

    /// <summary>
    /// 过账人（当前登录用户对应的 EmployeeCode）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// 销售发票明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktSalesInvoiceItemCreateDto>? Items { get; set; }

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
// 更新SalesInvoice DTO
// ========================================

/// <summary>
/// 更新SalesInvoice DTO
/// 继承 TaktSalesInvoiceCreateDto，添加 SalesInvoiceId 字段
/// </summary>
public class TaktSalesInvoiceUpdateDto : TaktSalesInvoiceCreateDto
{
    /// <summary>
    /// SalesInvoiceID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesInvoiceId { get; set; }

    /// <summary>
    /// 销售发票明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public new List<TaktSalesInvoiceItemUpdateDto>? Items { get; set; }

}

// ========================================
// SalesInvoice 状态 DTO
// ========================================

/// <summary>
/// SalesInvoice 状态更新 DTO
/// </summary>
public class TaktSalesInvoiceStatusDto
{
    /// <summary>
    /// SalesInvoiceID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesInvoiceId { get; set; }

    /// <summary>
    /// 过账状态
    /// </summary>
    [Required(ErrorMessage = "过账状态不能为空")]
    public string PostingStatus { get; set; } = string.Empty;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SalesInvoice 导入模板行 DTO
/// </summary>
public class TaktSalesInvoiceTemplateDto
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
    /// 开票凭证
    /// </summary>
    public string? BillingDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 开票类型
    /// </summary>
    public string? BillingType { get; set; } = string.Empty;

    /// <summary>
    /// 出具发票类别
    /// </summary>
    public string? BillingCategory { get; set; } = string.Empty;

    /// <summary>
    /// SD 凭证类别
    /// </summary>
    public string? DocumentCategory { get; set; } = string.Empty;

    /// <summary>
    /// 凭证货币（字典 accounting_financial_currency_code）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售组织
    /// </summary>
    public string? SalesOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 分销渠道
    /// </summary>
    public string? DistributionChannel { get; set; } = string.Empty;

    /// <summary>
    /// 定价过程
    /// </summary>
    public string? PricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 单据条件号
    /// </summary>
    public string? ConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 装运条件（字典 logistics_sales_shipping_conditions）
    /// </summary>
    public string? ShippingConditions { get; set; } = string.Empty;

    /// <summary>
    /// 出具发票日期
    /// </summary>
    public DateTime? BillingDate { get; set; }

    /// <summary>
    /// 客户组
    /// </summary>
    public string? CustomerGroup { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条件
    /// </summary>
    public string? Incoterms1 { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条件(部分2)（最长 28，故 Length=28）
    /// </summary>
    public string? Incoterms2 { get; set; } = string.Empty;

    /// <summary>
    /// 过账状态
    /// </summary>
    public string? PostingStatus { get; set; } = string.Empty;

    /// <summary>
    /// 会计汇率
    /// </summary>
    public decimal? AccountingExchangeRate { get; set; }

    /// <summary>
    /// 付款条件
    /// </summary>
    public string? PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 客户分配帐户组别
    /// </summary>
    public string? AccountAssignmentGroup { get; set; } = string.Empty;

    /// <summary>
    /// 目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 净价值
    /// </summary>
    public decimal? NetAmount { get; set; }

    /// <summary>
    /// 付款方（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? PayerCode { get; set; } = string.Empty;

    /// <summary>
    /// 售达方（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 统计货币（字典 accounting_financial_currency_code）
    /// </summary>
    public string? StatisticsCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 外贸数据编号
    /// </summary>
    public string? ForeignTradeCode { get; set; } = string.Empty;

    /// <summary>
    /// 已取消的开票凭证
    /// </summary>
    public string? CancelledBillingDocument { get; set; } = string.Empty;

    /// <summary>
    /// 发票清单类型
    /// </summary>
    public string? InvoiceListType { get; set; } = string.Empty;

    /// <summary>
    /// 产品组
    /// </summary>
    public string? Division { get; set; } = string.Empty;

    /// <summary>
    /// 定价的层次类型
    /// </summary>
    public string? HierarchyTypePricing { get; set; } = string.Empty;

    /// <summary>
    /// 贸易伙伴
    /// </summary>
    public string? TradingPartner { get; set; } = string.Empty;

    /// <summary>
    /// 征税国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? TaxDepartureCountry { get; set; } = string.Empty;

    /// <summary>
    /// 组织销售税编号
    /// </summary>
    public string? OrganizationSalesTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 国家销售税编号
    /// </summary>
    public string? CountrySalesTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 参考（最长 16，故 Length=16）
    /// </summary>
    public string? ReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 已被取消
    /// </summary>
    public string? CancelledFlag { get; set; } = string.Empty;

    /// <summary>
    /// 换算日期
    /// </summary>
    public DateTime? ExchangeRateDate { get; set; }

    /// <summary>
    /// 付款参考（最长 30，故 Length=30）
    /// </summary>
    public string? PaymentReference { get; set; } = string.Empty;

    /// <summary>
    /// 冲销原因
    /// </summary>
    public string? ReversalReason { get; set; } = string.Empty;

    /// <summary>
    /// 过账人（当前登录用户对应的 EmployeeCode）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// 销售发票明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktSalesInvoiceItemCreateDto>? Items { get; set; }

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
/// SalesInvoice 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSalesInvoiceImportDto
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
    /// 开票凭证
    /// </summary>
    public string? BillingDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 开票类型
    /// </summary>
    public string? BillingType { get; set; } = string.Empty;

    /// <summary>
    /// 出具发票类别
    /// </summary>
    public string? BillingCategory { get; set; } = string.Empty;

    /// <summary>
    /// SD 凭证类别
    /// </summary>
    public string? DocumentCategory { get; set; } = string.Empty;

    /// <summary>
    /// 凭证货币（字典 accounting_financial_currency_code）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售组织
    /// </summary>
    public string? SalesOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 分销渠道
    /// </summary>
    public string? DistributionChannel { get; set; } = string.Empty;

    /// <summary>
    /// 定价过程
    /// </summary>
    public string? PricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 单据条件号
    /// </summary>
    public string? ConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 装运条件（字典 logistics_sales_shipping_conditions）
    /// </summary>
    public string? ShippingConditions { get; set; } = string.Empty;

    /// <summary>
    /// 出具发票日期
    /// </summary>
    public DateTime? BillingDate { get; set; }

    /// <summary>
    /// 客户组
    /// </summary>
    public string? CustomerGroup { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条件
    /// </summary>
    public string? Incoterms1 { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条件(部分2)（最长 28，故 Length=28）
    /// </summary>
    public string? Incoterms2 { get; set; } = string.Empty;

    /// <summary>
    /// 过账状态
    /// </summary>
    public string? PostingStatus { get; set; } = string.Empty;

    /// <summary>
    /// 会计汇率
    /// </summary>
    public decimal? AccountingExchangeRate { get; set; }

    /// <summary>
    /// 付款条件
    /// </summary>
    public string? PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 客户分配帐户组别
    /// </summary>
    public string? AccountAssignmentGroup { get; set; } = string.Empty;

    /// <summary>
    /// 目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 净价值
    /// </summary>
    public decimal? NetAmount { get; set; }

    /// <summary>
    /// 付款方（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? PayerCode { get; set; } = string.Empty;

    /// <summary>
    /// 售达方（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 统计货币（字典 accounting_financial_currency_code）
    /// </summary>
    public string? StatisticsCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 外贸数据编号
    /// </summary>
    public string? ForeignTradeCode { get; set; } = string.Empty;

    /// <summary>
    /// 已取消的开票凭证
    /// </summary>
    public string? CancelledBillingDocument { get; set; } = string.Empty;

    /// <summary>
    /// 发票清单类型
    /// </summary>
    public string? InvoiceListType { get; set; } = string.Empty;

    /// <summary>
    /// 产品组
    /// </summary>
    public string? Division { get; set; } = string.Empty;

    /// <summary>
    /// 定价的层次类型
    /// </summary>
    public string? HierarchyTypePricing { get; set; } = string.Empty;

    /// <summary>
    /// 贸易伙伴
    /// </summary>
    public string? TradingPartner { get; set; } = string.Empty;

    /// <summary>
    /// 征税国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? TaxDepartureCountry { get; set; } = string.Empty;

    /// <summary>
    /// 组织销售税编号
    /// </summary>
    public string? OrganizationSalesTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 国家销售税编号
    /// </summary>
    public string? CountrySalesTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 参考（最长 16，故 Length=16）
    /// </summary>
    public string? ReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 已被取消
    /// </summary>
    public string? CancelledFlag { get; set; } = string.Empty;

    /// <summary>
    /// 换算日期
    /// </summary>
    public DateTime? ExchangeRateDate { get; set; }

    /// <summary>
    /// 付款参考（最长 30，故 Length=30）
    /// </summary>
    public string? PaymentReference { get; set; } = string.Empty;

    /// <summary>
    /// 冲销原因
    /// </summary>
    public string? ReversalReason { get; set; } = string.Empty;

    /// <summary>
    /// 过账人（当前登录用户对应的 EmployeeCode）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// 销售发票明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktSalesInvoiceItemCreateDto>? Items { get; set; }

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
/// SalesInvoice 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSalesInvoiceExportDto
{
    /// <summary>
    /// SalesInvoiceID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesInvoiceId { get; set; }

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
    /// 开票凭证
    /// </summary>
    public string BillingDocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 开票类型
    /// </summary>
    public string? BillingType { get; set; } = string.Empty;

    /// <summary>
    /// 出具发票类别
    /// </summary>
    public string? BillingCategory { get; set; } = string.Empty;

    /// <summary>
    /// SD 凭证类别
    /// </summary>
    public string? DocumentCategory { get; set; } = string.Empty;

    /// <summary>
    /// 凭证货币（字典 accounting_financial_currency_code）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售组织
    /// </summary>
    public string? SalesOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 分销渠道
    /// </summary>
    public string? DistributionChannel { get; set; } = string.Empty;

    /// <summary>
    /// 定价过程
    /// </summary>
    public string? PricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 单据条件号
    /// </summary>
    public string? ConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 装运条件（字典 logistics_sales_shipping_conditions）
    /// </summary>
    public string? ShippingConditions { get; set; } = string.Empty;

    /// <summary>
    /// 出具发票日期
    /// </summary>
    public DateTime BillingDate { get; set; }

    /// <summary>
    /// 客户组
    /// </summary>
    public string? CustomerGroup { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条件
    /// </summary>
    public string? Incoterms1 { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条件(部分2)（最长 28，故 Length=28）
    /// </summary>
    public string? Incoterms2 { get; set; } = string.Empty;

    /// <summary>
    /// 过账状态
    /// </summary>
    public string? PostingStatus { get; set; } = string.Empty;

    /// <summary>
    /// 会计汇率
    /// </summary>
    public decimal? AccountingExchangeRate { get; set; }

    /// <summary>
    /// 付款条件
    /// </summary>
    public string? PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 客户分配帐户组别
    /// </summary>
    public string? AccountAssignmentGroup { get; set; } = string.Empty;

    /// <summary>
    /// 目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 净价值
    /// </summary>
    public decimal NetAmount { get; set; }

    /// <summary>
    /// 付款方（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? PayerCode { get; set; } = string.Empty;

    /// <summary>
    /// 售达方（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 统计货币（字典 accounting_financial_currency_code）
    /// </summary>
    public string? StatisticsCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 外贸数据编号
    /// </summary>
    public string? ForeignTradeCode { get; set; } = string.Empty;

    /// <summary>
    /// 已取消的开票凭证
    /// </summary>
    public string? CancelledBillingDocument { get; set; } = string.Empty;

    /// <summary>
    /// 发票清单类型
    /// </summary>
    public string? InvoiceListType { get; set; } = string.Empty;

    /// <summary>
    /// 产品组
    /// </summary>
    public string? Division { get; set; } = string.Empty;

    /// <summary>
    /// 定价的层次类型
    /// </summary>
    public string? HierarchyTypePricing { get; set; } = string.Empty;

    /// <summary>
    /// 贸易伙伴
    /// </summary>
    public string? TradingPartner { get; set; } = string.Empty;

    /// <summary>
    /// 征税国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? TaxDepartureCountry { get; set; } = string.Empty;

    /// <summary>
    /// 组织销售税编号
    /// </summary>
    public string? OrganizationSalesTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 国家销售税编号
    /// </summary>
    public string? CountrySalesTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 参考（最长 16，故 Length=16）
    /// </summary>
    public string? ReferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 已被取消
    /// </summary>
    public string? CancelledFlag { get; set; } = string.Empty;

    /// <summary>
    /// 换算日期
    /// </summary>
    public DateTime? ExchangeRateDate { get; set; }

    /// <summary>
    /// 付款参考（最长 30，故 Length=30）
    /// </summary>
    public string? PaymentReference { get; set; } = string.Empty;

    /// <summary>
    /// 冲销原因
    /// </summary>
    public string? ReversalReason { get; set; } = string.Empty;

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

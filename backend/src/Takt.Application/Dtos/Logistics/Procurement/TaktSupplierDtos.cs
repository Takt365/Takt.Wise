// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Procurement
// 文件名称：TaktSupplierDtos.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Auto Generated)
// 功能描述：Supplier 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSupplier 生成，请按需审阅）
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
// Supplier 响应 DTO
// ========================================

/// <summary>
/// Takt供货商实体
/// 对应前端 TaktSupplierDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSupplierDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SupplierID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SupplierId { get; set; }

    /// <summary>
    /// 供货商编码（唯一索引）
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商名称1
    /// </summary>
    public string SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 供货商名称2
    /// </summary>
    public string? SupplierName2 { get; set; } = string.Empty;

    /// <summary>
    /// 供货商简称
    /// </summary>
    public string? SupplierShortName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商类型（字典 logistics_procurement_supplier_category；0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）
    /// </summary>
    public int SupplierType { get; set; } = 0;

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    public string EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    public string IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 供货商标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? SupplierTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）
    /// </summary>
    public int TaxRate { get; set; } = 0;

    /// <summary>
    /// 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? RegistrationCountry { get; set; } = string.Empty;

    /// <summary>
    /// 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? RegistrationProvince { get; set; } = string.Empty;

    /// <summary>
    /// 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? RegistrationCity { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址1
    /// </summary>
    public string? RegistrationAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址2
    /// </summary>
    public string? RegistrationAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 供货商电话
    /// </summary>
    public string? SupplierPhone { get; set; } = string.Empty;

    /// <summary>
    /// 供货商传真
    /// </summary>
    public string? SupplierFax { get; set; } = string.Empty;

    /// <summary>
    /// 供货商邮箱
    /// </summary>
    public string? SupplierEmail { get; set; } = string.Empty;

    /// <summary>
    /// 供货商网站
    /// </summary>
    public string? SupplierWebsite { get; set; } = string.Empty;

    /// <summary>
    /// 联系人
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 联系人电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系人邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）
    /// </summary>
    public string ReconciliationAccount { get; set; } = string.Empty;

    /// <summary>
    /// 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 具有客户的清算（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int ClearingWithCustomer { get; set; } = 0;

    /// <summary>
    /// 付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int PaymentMethod { get; set; } = 0;

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
    /// </summary>
    public string PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 银行代码（选项 TaktBanks/options；DictValue=BankCode）
    /// </summary>
    public string BankCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行帐号
    /// </summary>
    public string BankAccount { get; set; } = string.Empty;

    /// <summary>
    /// 帐户持有人
    /// </summary>
    public string AccountHolder { get; set; } = string.Empty;

    /// <summary>
    /// 基于收货的发票验证（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int GrBasedInvoiceInspection { get; set; } = 0;

    /// <summary>
    /// 国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
    /// </summary>
    public string Incoterms1 { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条件2（地点说明）
    /// </summary>
    public string Incoterms2 { get; set; } = string.Empty;

    /// <summary>
    /// 自动产生的采购订单（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int AutomaticPurchaseOrder { get; set; } = 0;

    /// <summary>
    /// 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
    /// </summary>
    public int PricingDateControl { get; set; } = 0;

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    public string PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 计划交货时间（天）
    /// </summary>
    public int PlannedDeliveryTimeDays { get; set; } = 0;

    /// <summary>
    /// 评估收据结算（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int EvaluatedReceiptSettlement { get; set; } = 0;

    /// <summary>
    /// 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PurchasingOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 供货商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时）
    /// </summary>
    public int SupplierLevel { get; set; } = 0;

    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    public decimal EvaluationScore { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 供货商状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int SupplierStatus { get; set; } = 0;

}

// ========================================
// Supplier 查询 DTO
// ========================================

/// <summary>
/// Supplier 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSupplierQueryDto : TaktPagedQuery
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
    /// 供货商编码（唯一索引）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商名称1
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 供货商名称2
    /// </summary>
    public string? SupplierName2 { get; set; } = string.Empty;

    /// <summary>
    /// 供货商简称
    /// </summary>
    public string? SupplierShortName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商类型（字典 logistics_procurement_supplier_category；0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）
    /// </summary>
    public int? SupplierType { get; set; }

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    public string? EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    public string? IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 供货商标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? SupplierTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）
    /// </summary>
    public int? TaxRate { get; set; }

    /// <summary>
    /// 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? RegistrationCountry { get; set; } = string.Empty;

    /// <summary>
    /// 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? RegistrationProvince { get; set; } = string.Empty;

    /// <summary>
    /// 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? RegistrationCity { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址1
    /// </summary>
    public string? RegistrationAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址2
    /// </summary>
    public string? RegistrationAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 供货商电话
    /// </summary>
    public string? SupplierPhone { get; set; } = string.Empty;

    /// <summary>
    /// 供货商传真
    /// </summary>
    public string? SupplierFax { get; set; } = string.Empty;

    /// <summary>
    /// 供货商邮箱
    /// </summary>
    public string? SupplierEmail { get; set; } = string.Empty;

    /// <summary>
    /// 供货商网站
    /// </summary>
    public string? SupplierWebsite { get; set; } = string.Empty;

    /// <summary>
    /// 联系人
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 联系人电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系人邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）
    /// </summary>
    public string? ReconciliationAccount { get; set; } = string.Empty;

    /// <summary>
    /// 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 具有客户的清算（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? ClearingWithCustomer { get; set; }

    /// <summary>
    /// 付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int? PaymentMethod { get; set; }

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
    /// </summary>
    public string? PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 银行代码（选项 TaktBanks/options；DictValue=BankCode）
    /// </summary>
    public string? BankCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行帐号
    /// </summary>
    public string? BankAccount { get; set; } = string.Empty;

    /// <summary>
    /// 帐户持有人
    /// </summary>
    public string? AccountHolder { get; set; } = string.Empty;

    /// <summary>
    /// 基于收货的发票验证（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? GrBasedInvoiceInspection { get; set; }

    /// <summary>
    /// 国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
    /// </summary>
    public string? Incoterms1 { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条件2（地点说明）
    /// </summary>
    public string? Incoterms2 { get; set; } = string.Empty;

    /// <summary>
    /// 自动产生的采购订单（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? AutomaticPurchaseOrder { get; set; }

    /// <summary>
    /// 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
    /// </summary>
    public int? PricingDateControl { get; set; }

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 计划交货时间（天）
    /// </summary>
    public int? PlannedDeliveryTimeDays { get; set; }

    /// <summary>
    /// 评估收据结算（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? EvaluatedReceiptSettlement { get; set; }

    /// <summary>
    /// 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PurchasingOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 供货商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时）
    /// </summary>
    public int? SupplierLevel { get; set; }

    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    public decimal? EvaluationScore { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 供货商状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? SupplierStatus { get; set; }

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
// 创建Supplier DTO
// ========================================

/// <summary>
/// 创建Supplier DTO
/// </summary>
public class TaktSupplierCreateDto
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
    /// 供货商编码（唯一索引）
    /// </summary>
    [Required(ErrorMessage = "供货商编码（唯一索引）不能为空")]
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商名称1
    /// </summary>
    [Required(ErrorMessage = "供货商名称1不能为空")]
    public string SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 供货商名称2
    /// </summary>
    public string? SupplierName2 { get; set; } = string.Empty;

    /// <summary>
    /// 供货商简称
    /// </summary>
    public string? SupplierShortName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商类型（字典 logistics_procurement_supplier_category；0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）
    /// </summary>
    public int SupplierType { get; set; } = 0;

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    [Required(ErrorMessage = "企业性质（字典 sys_enterprise_nature）不能为空")]
    public string EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    [Required(ErrorMessage = "行业属性（字典 sys_industry_attribute）不能为空")]
    public string IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 供货商标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? SupplierTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）
    /// </summary>
    public int TaxRate { get; set; } = 0;

    /// <summary>
    /// 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? RegistrationCountry { get; set; } = string.Empty;

    /// <summary>
    /// 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? RegistrationProvince { get; set; } = string.Empty;

    /// <summary>
    /// 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? RegistrationCity { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址1
    /// </summary>
    public string? RegistrationAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址2
    /// </summary>
    public string? RegistrationAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 供货商电话
    /// </summary>
    public string? SupplierPhone { get; set; } = string.Empty;

    /// <summary>
    /// 供货商传真
    /// </summary>
    public string? SupplierFax { get; set; } = string.Empty;

    /// <summary>
    /// 供货商邮箱
    /// </summary>
    public string? SupplierEmail { get; set; } = string.Empty;

    /// <summary>
    /// 供货商网站
    /// </summary>
    public string? SupplierWebsite { get; set; } = string.Empty;

    /// <summary>
    /// 联系人
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 联系人电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系人邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    [Required(ErrorMessage = "结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）不能为空")]
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）
    /// </summary>
    [Required(ErrorMessage = "统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）不能为空")]
    public string ReconciliationAccount { get; set; } = string.Empty;

    /// <summary>
    /// 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    [Required(ErrorMessage = "客户（选项 TaktCustomers/options；DictValue=CustomerCode）不能为空")]
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 具有客户的清算（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int ClearingWithCustomer { get; set; } = 0;

    /// <summary>
    /// 付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int PaymentMethod { get; set; } = 0;

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
    /// </summary>
    [Required(ErrorMessage = "付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）不能为空")]
    public string PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 银行代码（选项 TaktBanks/options；DictValue=BankCode）
    /// </summary>
    [Required(ErrorMessage = "银行代码（选项 TaktBanks/options；DictValue=BankCode）不能为空")]
    public string BankCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行帐号
    /// </summary>
    [Required(ErrorMessage = "银行帐号不能为空")]
    public string BankAccount { get; set; } = string.Empty;

    /// <summary>
    /// 帐户持有人
    /// </summary>
    [Required(ErrorMessage = "帐户持有人不能为空")]
    public string AccountHolder { get; set; } = string.Empty;

    /// <summary>
    /// 基于收货的发票验证（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int GrBasedInvoiceInspection { get; set; } = 0;

    /// <summary>
    /// 国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
    /// </summary>
    [Required(ErrorMessage = "国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）不能为空")]
    public string Incoterms1 { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条件2（地点说明）
    /// </summary>
    [Required(ErrorMessage = "国际贸易条件2（地点说明）不能为空")]
    public string Incoterms2 { get; set; } = string.Empty;

    /// <summary>
    /// 自动产生的采购订单（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int AutomaticPurchaseOrder { get; set; } = 0;

    /// <summary>
    /// 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
    /// </summary>
    public int PricingDateControl { get; set; } = 0;

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    [Required(ErrorMessage = "采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）不能为空")]
    public string PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 计划交货时间（天）
    /// </summary>
    public int PlannedDeliveryTimeDays { get; set; } = 0;

    /// <summary>
    /// 评估收据结算（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int EvaluatedReceiptSettlement { get; set; } = 0;

    /// <summary>
    /// 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "采购组织（选项 TaktPlants/options；DictValue=PlantCode）不能为空")]
    public string PurchasingOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 供货商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时）
    /// </summary>
    public int SupplierLevel { get; set; } = 0;

    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    public decimal EvaluationScore { get; set; }

    /// <summary>
    /// 供货商状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int SupplierStatus { get; set; } = 0;

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
// 更新Supplier DTO
// ========================================

/// <summary>
/// 更新Supplier DTO
/// 继承 TaktSupplierCreateDto，添加 SupplierId 字段
/// </summary>
public class TaktSupplierUpdateDto : TaktSupplierCreateDto
{
    /// <summary>
    /// SupplierID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SupplierId { get; set; }

}

// ========================================
// Supplier 状态 DTO
// ========================================

/// <summary>
/// Supplier 状态更新 DTO
/// </summary>
public class TaktSupplierStatusDto
{
    /// <summary>
    /// SupplierID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SupplierId { get; set; }

    /// <summary>
    /// 供货商状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "供货商状态（字典 sys_normal_disable；1=启用，0=禁用）不能为空")]
    public int SupplierStatus { get; set; } = 0;
}

// ========================================
// Supplier 排序 DTO
// ========================================

/// <summary>
/// Supplier 排序更新 DTO
/// </summary>
public class TaktSupplierSortDto
{
    /// <summary>
    /// SupplierID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SupplierId { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [Required(ErrorMessage = "排序号（回填）（越小越靠前）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Supplier 导入模板行 DTO
/// </summary>
public class TaktSupplierTemplateDto
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
    /// 供货商编码（唯一索引）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商名称1
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 供货商名称2
    /// </summary>
    public string? SupplierName2 { get; set; } = string.Empty;

    /// <summary>
    /// 供货商简称
    /// </summary>
    public string? SupplierShortName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商类型（字典 logistics_procurement_supplier_category；0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）
    /// </summary>
    public int? SupplierType { get; set; }

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    public string? EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    public string? IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 供货商标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? SupplierTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）
    /// </summary>
    public int? TaxRate { get; set; }

    /// <summary>
    /// 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? RegistrationCountry { get; set; } = string.Empty;

    /// <summary>
    /// 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? RegistrationProvince { get; set; } = string.Empty;

    /// <summary>
    /// 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? RegistrationCity { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址1
    /// </summary>
    public string? RegistrationAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址2
    /// </summary>
    public string? RegistrationAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 供货商电话
    /// </summary>
    public string? SupplierPhone { get; set; } = string.Empty;

    /// <summary>
    /// 供货商传真
    /// </summary>
    public string? SupplierFax { get; set; } = string.Empty;

    /// <summary>
    /// 供货商邮箱
    /// </summary>
    public string? SupplierEmail { get; set; } = string.Empty;

    /// <summary>
    /// 供货商网站
    /// </summary>
    public string? SupplierWebsite { get; set; } = string.Empty;

    /// <summary>
    /// 联系人
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 联系人电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系人邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）
    /// </summary>
    public string? ReconciliationAccount { get; set; } = string.Empty;

    /// <summary>
    /// 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 具有客户的清算（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? ClearingWithCustomer { get; set; }

    /// <summary>
    /// 付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int? PaymentMethod { get; set; }

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
    /// </summary>
    public string? PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 银行代码（选项 TaktBanks/options；DictValue=BankCode）
    /// </summary>
    public string? BankCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行帐号
    /// </summary>
    public string? BankAccount { get; set; } = string.Empty;

    /// <summary>
    /// 帐户持有人
    /// </summary>
    public string? AccountHolder { get; set; } = string.Empty;

    /// <summary>
    /// 基于收货的发票验证（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? GrBasedInvoiceInspection { get; set; }

    /// <summary>
    /// 国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
    /// </summary>
    public string? Incoterms1 { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条件2（地点说明）
    /// </summary>
    public string? Incoterms2 { get; set; } = string.Empty;

    /// <summary>
    /// 自动产生的采购订单（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? AutomaticPurchaseOrder { get; set; }

    /// <summary>
    /// 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
    /// </summary>
    public int? PricingDateControl { get; set; }

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 计划交货时间（天）
    /// </summary>
    public int? PlannedDeliveryTimeDays { get; set; }

    /// <summary>
    /// 评估收据结算（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? EvaluatedReceiptSettlement { get; set; }

    /// <summary>
    /// 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PurchasingOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 供货商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时）
    /// </summary>
    public int? SupplierLevel { get; set; }

    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    public decimal? EvaluationScore { get; set; }

    /// <summary>
    /// 供货商状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? SupplierStatus { get; set; }

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
/// Supplier 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSupplierImportDto
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
    /// 供货商编码（唯一索引）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商名称1
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 供货商名称2
    /// </summary>
    public string? SupplierName2 { get; set; } = string.Empty;

    /// <summary>
    /// 供货商简称
    /// </summary>
    public string? SupplierShortName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商类型（字典 logistics_procurement_supplier_category；0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）
    /// </summary>
    public int? SupplierType { get; set; }

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    public string? EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    public string? IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 供货商标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? SupplierTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）
    /// </summary>
    public int? TaxRate { get; set; }

    /// <summary>
    /// 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? RegistrationCountry { get; set; } = string.Empty;

    /// <summary>
    /// 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? RegistrationProvince { get; set; } = string.Empty;

    /// <summary>
    /// 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? RegistrationCity { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址1
    /// </summary>
    public string? RegistrationAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址2
    /// </summary>
    public string? RegistrationAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 供货商电话
    /// </summary>
    public string? SupplierPhone { get; set; } = string.Empty;

    /// <summary>
    /// 供货商传真
    /// </summary>
    public string? SupplierFax { get; set; } = string.Empty;

    /// <summary>
    /// 供货商邮箱
    /// </summary>
    public string? SupplierEmail { get; set; } = string.Empty;

    /// <summary>
    /// 供货商网站
    /// </summary>
    public string? SupplierWebsite { get; set; } = string.Empty;

    /// <summary>
    /// 联系人
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 联系人电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系人邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）
    /// </summary>
    public string? ReconciliationAccount { get; set; } = string.Empty;

    /// <summary>
    /// 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 具有客户的清算（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? ClearingWithCustomer { get; set; }

    /// <summary>
    /// 付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int? PaymentMethod { get; set; }

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
    /// </summary>
    public string? PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 银行代码（选项 TaktBanks/options；DictValue=BankCode）
    /// </summary>
    public string? BankCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行帐号
    /// </summary>
    public string? BankAccount { get; set; } = string.Empty;

    /// <summary>
    /// 帐户持有人
    /// </summary>
    public string? AccountHolder { get; set; } = string.Empty;

    /// <summary>
    /// 基于收货的发票验证（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? GrBasedInvoiceInspection { get; set; }

    /// <summary>
    /// 国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
    /// </summary>
    public string? Incoterms1 { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条件2（地点说明）
    /// </summary>
    public string? Incoterms2 { get; set; } = string.Empty;

    /// <summary>
    /// 自动产生的采购订单（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? AutomaticPurchaseOrder { get; set; }

    /// <summary>
    /// 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
    /// </summary>
    public int? PricingDateControl { get; set; }

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 计划交货时间（天）
    /// </summary>
    public int? PlannedDeliveryTimeDays { get; set; }

    /// <summary>
    /// 评估收据结算（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? EvaluatedReceiptSettlement { get; set; }

    /// <summary>
    /// 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PurchasingOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 供货商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时）
    /// </summary>
    public int? SupplierLevel { get; set; }

    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    public decimal? EvaluationScore { get; set; }

    /// <summary>
    /// 供货商状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? SupplierStatus { get; set; }

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
/// Supplier 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSupplierExportDto
{
    /// <summary>
    /// SupplierID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SupplierId { get; set; }

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
    /// 供货商编码（唯一索引）
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商名称1
    /// </summary>
    public string SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 供货商名称2
    /// </summary>
    public string? SupplierName2 { get; set; } = string.Empty;

    /// <summary>
    /// 供货商简称
    /// </summary>
    public string? SupplierShortName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商类型（字典 logistics_procurement_supplier_category；0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）
    /// </summary>
    public int SupplierType { get; set; } = 0;

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    public string EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    public string IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 供货商标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? SupplierTaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
    /// </summary>
    public string? TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）
    /// </summary>
    public int TaxRate { get; set; } = 0;

    /// <summary>
    /// 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? RegistrationCountry { get; set; } = string.Empty;

    /// <summary>
    /// 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? RegistrationProvince { get; set; } = string.Empty;

    /// <summary>
    /// 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? RegistrationCity { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址1
    /// </summary>
    public string? RegistrationAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址2
    /// </summary>
    public string? RegistrationAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 供货商电话
    /// </summary>
    public string? SupplierPhone { get; set; } = string.Empty;

    /// <summary>
    /// 供货商传真
    /// </summary>
    public string? SupplierFax { get; set; } = string.Empty;

    /// <summary>
    /// 供货商邮箱
    /// </summary>
    public string? SupplierEmail { get; set; } = string.Empty;

    /// <summary>
    /// 供货商网站
    /// </summary>
    public string? SupplierWebsite { get; set; } = string.Empty;

    /// <summary>
    /// 联系人
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 联系人电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系人邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）
    /// </summary>
    public string ReconciliationAccount { get; set; } = string.Empty;

    /// <summary>
    /// 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 具有客户的清算（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int ClearingWithCustomer { get; set; } = 0;

    /// <summary>
    /// 付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int PaymentMethod { get; set; } = 0;

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
    /// </summary>
    public string PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 银行代码（选项 TaktBanks/options；DictValue=BankCode）
    /// </summary>
    public string BankCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行帐号
    /// </summary>
    public string BankAccount { get; set; } = string.Empty;

    /// <summary>
    /// 帐户持有人
    /// </summary>
    public string AccountHolder { get; set; } = string.Empty;

    /// <summary>
    /// 基于收货的发票验证（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int GrBasedInvoiceInspection { get; set; } = 0;

    /// <summary>
    /// 国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
    /// </summary>
    public string Incoterms1 { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条件2（地点说明）
    /// </summary>
    public string Incoterms2 { get; set; } = string.Empty;

    /// <summary>
    /// 自动产生的采购订单（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int AutomaticPurchaseOrder { get; set; } = 0;

    /// <summary>
    /// 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
    /// </summary>
    public int PricingDateControl { get; set; } = 0;

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    public string PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 计划交货时间（天）
    /// </summary>
    public int PlannedDeliveryTimeDays { get; set; } = 0;

    /// <summary>
    /// 评估收据结算（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int EvaluatedReceiptSettlement { get; set; } = 0;

    /// <summary>
    /// 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PurchasingOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 供货商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时）
    /// </summary>
    public int SupplierLevel { get; set; } = 0;

    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    public decimal EvaluationScore { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 供货商状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int SupplierStatus { get; set; } = 0;

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

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Sales
// 文件名称：TaktCustomerDtos.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Auto Generated)
// 功能描述：Customer 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktCustomer 生成，请按需审阅）
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
// Customer 响应 DTO
// ========================================

/// <summary>
/// Takt客户信息实体 <para>业务唯一键：TenantCode+CompanyCode+CustomerCode（PlantCode 为业务字段，不参与唯一）。</para>
/// 对应前端 TaktCustomerDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktCustomerDto : TaktCompanyDtoBase
{
    /// <summary>
    /// CustomerID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerId { get; set; }

    /// <summary>
    /// 客户编码（唯一索引）
    /// </summary>
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称1
    /// </summary>
    public string CustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称2
    /// </summary>
    public string? CustomerName2 { get; set; } = string.Empty;

    /// <summary>
    /// 客户简称
    /// </summary>
    public string? CustomerShortName { get; set; } = string.Empty;

    /// <summary>
    /// 客户类型（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
    /// </summary>
    public int CustomerType { get; set; } = 0;

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    public string EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    public string IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 客户标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? CustomerTaxNumber { get; set; } = string.Empty;

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
    /// 客户电话
    /// </summary>
    public string? CustomerPhone { get; set; } = string.Empty;

    /// <summary>
    /// 客户传真
    /// </summary>
    public string? CustomerFax { get; set; } = string.Empty;

    /// <summary>
    /// 客户邮箱
    /// </summary>
    public string? CustomerEmail { get; set; } = string.Empty;

    /// <summary>
    /// 客户网站
    /// </summary>
    public string? CustomerWebsite { get; set; } = string.Empty;

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
    /// 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string SalesOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 分销渠道
    /// </summary>
    public string DistributionChannel { get; set; } = string.Empty;

    /// <summary>
    /// 产品组
    /// </summary>
    public string ProductGroup { get; set; } = string.Empty;

    /// <summary>
    /// 客户组（字典 logistics_sales_customer_group；DictValue=Z1～Z4）
    /// </summary>
    public string CustomerGroup { get; set; } = string.Empty;

    /// <summary>
    /// 贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string TradingPartner { get; set; } = string.Empty;

    /// <summary>
    /// 帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）
    /// </summary>
    public string AccountAssignmentGroup { get; set; } = string.Empty;

    /// <summary>
    /// 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 尼尔森标识
    /// </summary>
    public string NielsenIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 中心记帐冻结（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int CentralPostingBlock { get; set; } = 0;

    /// <summary>
    /// 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）
    /// </summary>
    public string ReconciliationAccount { get; set; } = string.Empty;

    /// <summary>
    /// 总部（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string Headquarters { get; set; } = string.Empty;

    /// <summary>
    /// 具有供应商的清算（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int ClearingWithVendor { get; set; } = 0;

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
    /// </summary>
    public string PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int PaymentMethod { get; set; } = 0;

    /// <summary>
    /// 交货工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string DeliveringPlant { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
    /// </summary>
    public string Incoterms1 { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条件2（地点说明）
    /// </summary>
    public string Incoterms2 { get; set; } = string.Empty;

    /// <summary>
    /// 装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）
    /// </summary>
    public string ShippingConditions { get; set; } = string.Empty;

    /// <summary>
    /// 客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）
    /// </summary>
    public string CustomerPricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
    /// </summary>
    public int CreditLevel { get; set; } = 0;

    /// <summary>
    /// 信用额度（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal CreditAmount { get; set; }

    /// <summary>
    /// 折扣率（百分比；可选字典 logistics_sales_discount_rate_param 预设）
    /// </summary>
    public decimal DiscountRate { get; set; }

    /// <summary>
    /// 客户等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）
    /// </summary>
    public int CustomerLevel { get; set; } = 0;

    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    public decimal EvaluationScore { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 客户状态（字典 sys_normal_disable）
    /// </summary>
    public int CustomerStatus { get; set; } = 0;

}

// ========================================
// Customer 查询 DTO
// ========================================

/// <summary>
/// Customer 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktCustomerQueryDto : TaktPagedQuery
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
    /// 客户编码（唯一索引）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称1
    /// </summary>
    public string? CustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称2
    /// </summary>
    public string? CustomerName2 { get; set; } = string.Empty;

    /// <summary>
    /// 客户简称
    /// </summary>
    public string? CustomerShortName { get; set; } = string.Empty;

    /// <summary>
    /// 客户类型（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
    /// </summary>
    public int? CustomerType { get; set; }

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    public string? EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    public string? IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 客户标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? CustomerTaxNumber { get; set; } = string.Empty;

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
    /// 客户电话
    /// </summary>
    public string? CustomerPhone { get; set; } = string.Empty;

    /// <summary>
    /// 客户传真
    /// </summary>
    public string? CustomerFax { get; set; } = string.Empty;

    /// <summary>
    /// 客户邮箱
    /// </summary>
    public string? CustomerEmail { get; set; } = string.Empty;

    /// <summary>
    /// 客户网站
    /// </summary>
    public string? CustomerWebsite { get; set; } = string.Empty;

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
    /// 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? SalesOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 分销渠道
    /// </summary>
    public string? DistributionChannel { get; set; } = string.Empty;

    /// <summary>
    /// 产品组
    /// </summary>
    public string? ProductGroup { get; set; } = string.Empty;

    /// <summary>
    /// 客户组（字典 logistics_sales_customer_group；DictValue=Z1～Z4）
    /// </summary>
    public string? CustomerGroup { get; set; } = string.Empty;

    /// <summary>
    /// 贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? TradingPartner { get; set; } = string.Empty;

    /// <summary>
    /// 帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）
    /// </summary>
    public string? AccountAssignmentGroup { get; set; } = string.Empty;

    /// <summary>
    /// 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 尼尔森标识
    /// </summary>
    public string? NielsenIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 中心记帐冻结（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? CentralPostingBlock { get; set; }

    /// <summary>
    /// 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）
    /// </summary>
    public string? ReconciliationAccount { get; set; } = string.Empty;

    /// <summary>
    /// 总部（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? Headquarters { get; set; } = string.Empty;

    /// <summary>
    /// 具有供应商的清算（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? ClearingWithVendor { get; set; }

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
    /// </summary>
    public string? PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int? PaymentMethod { get; set; }

    /// <summary>
    /// 交货工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? DeliveringPlant { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
    /// </summary>
    public string? Incoterms1 { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条件2（地点说明）
    /// </summary>
    public string? Incoterms2 { get; set; } = string.Empty;

    /// <summary>
    /// 装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）
    /// </summary>
    public string? ShippingConditions { get; set; } = string.Empty;

    /// <summary>
    /// 客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）
    /// </summary>
    public string? CustomerPricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
    /// </summary>
    public int? CreditLevel { get; set; }

    /// <summary>
    /// 信用额度（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? CreditAmount { get; set; }

    /// <summary>
    /// 折扣率（百分比；可选字典 logistics_sales_discount_rate_param 预设）
    /// </summary>
    public decimal? DiscountRate { get; set; }

    /// <summary>
    /// 客户等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）
    /// </summary>
    public int? CustomerLevel { get; set; }

    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    public decimal? EvaluationScore { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 客户状态（字典 sys_normal_disable）
    /// </summary>
    public int? CustomerStatus { get; set; }

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
// 创建Customer DTO
// ========================================

/// <summary>
/// 创建Customer DTO
/// </summary>
public class TaktCustomerCreateDto
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
    /// 客户编码（唯一索引）
    /// </summary>
    [Required(ErrorMessage = "客户编码（唯一索引）不能为空")]
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称1
    /// </summary>
    [Required(ErrorMessage = "客户名称1不能为空")]
    public string CustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称2
    /// </summary>
    public string? CustomerName2 { get; set; } = string.Empty;

    /// <summary>
    /// 客户简称
    /// </summary>
    public string? CustomerShortName { get; set; } = string.Empty;

    /// <summary>
    /// 客户类型（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
    /// </summary>
    public int CustomerType { get; set; } = 0;

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
    /// 客户标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? CustomerTaxNumber { get; set; } = string.Empty;

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
    /// 客户电话
    /// </summary>
    public string? CustomerPhone { get; set; } = string.Empty;

    /// <summary>
    /// 客户传真
    /// </summary>
    public string? CustomerFax { get; set; } = string.Empty;

    /// <summary>
    /// 客户邮箱
    /// </summary>
    public string? CustomerEmail { get; set; } = string.Empty;

    /// <summary>
    /// 客户网站
    /// </summary>
    public string? CustomerWebsite { get; set; } = string.Empty;

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
    /// 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    [Required(ErrorMessage = "销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）不能为空")]
    public string SalesOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 分销渠道
    /// </summary>
    [Required(ErrorMessage = "分销渠道不能为空")]
    public string DistributionChannel { get; set; } = string.Empty;

    /// <summary>
    /// 产品组
    /// </summary>
    [Required(ErrorMessage = "产品组不能为空")]
    public string ProductGroup { get; set; } = string.Empty;

    /// <summary>
    /// 客户组（字典 logistics_sales_customer_group；DictValue=Z1～Z4）
    /// </summary>
    [Required(ErrorMessage = "客户组（字典 logistics_sales_customer_group；DictValue=Z1～Z4）不能为空")]
    public string CustomerGroup { get; set; } = string.Empty;

    /// <summary>
    /// 贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）不能为空")]
    public string TradingPartner { get; set; } = string.Empty;

    /// <summary>
    /// 帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）
    /// </summary>
    [Required(ErrorMessage = "帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）不能为空")]
    public string AccountAssignmentGroup { get; set; } = string.Empty;

    /// <summary>
    /// 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    [Required(ErrorMessage = "供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）不能为空")]
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 尼尔森标识
    /// </summary>
    [Required(ErrorMessage = "尼尔森标识不能为空")]
    public string NielsenIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 中心记帐冻结（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int CentralPostingBlock { get; set; } = 0;

    /// <summary>
    /// 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）
    /// </summary>
    [Required(ErrorMessage = "统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）不能为空")]
    public string ReconciliationAccount { get; set; } = string.Empty;

    /// <summary>
    /// 总部（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    [Required(ErrorMessage = "总部（选项 TaktCustomers/options；DictValue=CustomerCode）不能为空")]
    public string Headquarters { get; set; } = string.Empty;

    /// <summary>
    /// 具有供应商的清算（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int ClearingWithVendor { get; set; } = 0;

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
    /// </summary>
    [Required(ErrorMessage = "付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）不能为空")]
    public string PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int PaymentMethod { get; set; } = 0;

    /// <summary>
    /// 交货工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "交货工厂（选项 TaktPlants/options；DictValue=PlantCode）不能为空")]
    public string DeliveringPlant { get; set; } = string.Empty;

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
    /// 装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）
    /// </summary>
    [Required(ErrorMessage = "装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）不能为空")]
    public string ShippingConditions { get; set; } = string.Empty;

    /// <summary>
    /// 客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）
    /// </summary>
    [Required(ErrorMessage = "客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）不能为空")]
    public string CustomerPricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
    /// </summary>
    public int CreditLevel { get; set; } = 0;

    /// <summary>
    /// 信用额度（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal CreditAmount { get; set; }

    /// <summary>
    /// 折扣率（百分比；可选字典 logistics_sales_discount_rate_param 预设）
    /// </summary>
    public decimal DiscountRate { get; set; }

    /// <summary>
    /// 客户等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）
    /// </summary>
    public int CustomerLevel { get; set; } = 0;

    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    public decimal EvaluationScore { get; set; }

    /// <summary>
    /// 客户状态（字典 sys_normal_disable）
    /// </summary>
    public int CustomerStatus { get; set; } = 0;

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
// 更新Customer DTO
// ========================================

/// <summary>
/// 更新Customer DTO
/// 继承 TaktCustomerCreateDto，添加 CustomerId 字段
/// </summary>
public class TaktCustomerUpdateDto : TaktCustomerCreateDto
{
    /// <summary>
    /// CustomerID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerId { get; set; }

}

// ========================================
// Customer 状态 DTO
// ========================================

/// <summary>
/// Customer 状态更新 DTO
/// </summary>
public class TaktCustomerStatusDto
{
    /// <summary>
    /// CustomerID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerId { get; set; }

    /// <summary>
    /// 客户状态（字典 sys_normal_disable）
    /// </summary>
    [Required(ErrorMessage = "客户状态（字典 sys_normal_disable）不能为空")]
    public int CustomerStatus { get; set; } = 0;
}

// ========================================
// Customer 排序 DTO
// ========================================

/// <summary>
/// Customer 排序更新 DTO
/// </summary>
public class TaktCustomerSortDto
{
    /// <summary>
    /// CustomerID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerId { get; set; }

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
/// Customer 导入模板行 DTO
/// </summary>
public class TaktCustomerTemplateDto
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
    /// 客户编码（唯一索引）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称1
    /// </summary>
    public string? CustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称2
    /// </summary>
    public string? CustomerName2 { get; set; } = string.Empty;

    /// <summary>
    /// 客户简称
    /// </summary>
    public string? CustomerShortName { get; set; } = string.Empty;

    /// <summary>
    /// 客户类型（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
    /// </summary>
    public int? CustomerType { get; set; }

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    public string? EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    public string? IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 客户标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? CustomerTaxNumber { get; set; } = string.Empty;

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
    /// 客户电话
    /// </summary>
    public string? CustomerPhone { get; set; } = string.Empty;

    /// <summary>
    /// 客户传真
    /// </summary>
    public string? CustomerFax { get; set; } = string.Empty;

    /// <summary>
    /// 客户邮箱
    /// </summary>
    public string? CustomerEmail { get; set; } = string.Empty;

    /// <summary>
    /// 客户网站
    /// </summary>
    public string? CustomerWebsite { get; set; } = string.Empty;

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
    /// 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? SalesOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 分销渠道
    /// </summary>
    public string? DistributionChannel { get; set; } = string.Empty;

    /// <summary>
    /// 产品组
    /// </summary>
    public string? ProductGroup { get; set; } = string.Empty;

    /// <summary>
    /// 客户组（字典 logistics_sales_customer_group；DictValue=Z1～Z4）
    /// </summary>
    public string? CustomerGroup { get; set; } = string.Empty;

    /// <summary>
    /// 贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? TradingPartner { get; set; } = string.Empty;

    /// <summary>
    /// 帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）
    /// </summary>
    public string? AccountAssignmentGroup { get; set; } = string.Empty;

    /// <summary>
    /// 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 尼尔森标识
    /// </summary>
    public string? NielsenIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 中心记帐冻结（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? CentralPostingBlock { get; set; }

    /// <summary>
    /// 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）
    /// </summary>
    public string? ReconciliationAccount { get; set; } = string.Empty;

    /// <summary>
    /// 总部（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? Headquarters { get; set; } = string.Empty;

    /// <summary>
    /// 具有供应商的清算（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? ClearingWithVendor { get; set; }

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
    /// </summary>
    public string? PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int? PaymentMethod { get; set; }

    /// <summary>
    /// 交货工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? DeliveringPlant { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
    /// </summary>
    public string? Incoterms1 { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条件2（地点说明）
    /// </summary>
    public string? Incoterms2 { get; set; } = string.Empty;

    /// <summary>
    /// 装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）
    /// </summary>
    public string? ShippingConditions { get; set; } = string.Empty;

    /// <summary>
    /// 客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）
    /// </summary>
    public string? CustomerPricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
    /// </summary>
    public int? CreditLevel { get; set; }

    /// <summary>
    /// 信用额度（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? CreditAmount { get; set; }

    /// <summary>
    /// 折扣率（百分比；可选字典 logistics_sales_discount_rate_param 预设）
    /// </summary>
    public decimal? DiscountRate { get; set; }

    /// <summary>
    /// 客户等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）
    /// </summary>
    public int? CustomerLevel { get; set; }

    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    public decimal? EvaluationScore { get; set; }

    /// <summary>
    /// 客户状态（字典 sys_normal_disable）
    /// </summary>
    public int? CustomerStatus { get; set; }

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
/// Customer 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktCustomerImportDto
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
    /// 客户编码（唯一索引）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称1
    /// </summary>
    public string? CustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称2
    /// </summary>
    public string? CustomerName2 { get; set; } = string.Empty;

    /// <summary>
    /// 客户简称
    /// </summary>
    public string? CustomerShortName { get; set; } = string.Empty;

    /// <summary>
    /// 客户类型（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
    /// </summary>
    public int? CustomerType { get; set; }

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    public string? EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    public string? IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 客户标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? CustomerTaxNumber { get; set; } = string.Empty;

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
    /// 客户电话
    /// </summary>
    public string? CustomerPhone { get; set; } = string.Empty;

    /// <summary>
    /// 客户传真
    /// </summary>
    public string? CustomerFax { get; set; } = string.Empty;

    /// <summary>
    /// 客户邮箱
    /// </summary>
    public string? CustomerEmail { get; set; } = string.Empty;

    /// <summary>
    /// 客户网站
    /// </summary>
    public string? CustomerWebsite { get; set; } = string.Empty;

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
    /// 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? SalesOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 分销渠道
    /// </summary>
    public string? DistributionChannel { get; set; } = string.Empty;

    /// <summary>
    /// 产品组
    /// </summary>
    public string? ProductGroup { get; set; } = string.Empty;

    /// <summary>
    /// 客户组（字典 logistics_sales_customer_group；DictValue=Z1～Z4）
    /// </summary>
    public string? CustomerGroup { get; set; } = string.Empty;

    /// <summary>
    /// 贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? TradingPartner { get; set; } = string.Empty;

    /// <summary>
    /// 帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）
    /// </summary>
    public string? AccountAssignmentGroup { get; set; } = string.Empty;

    /// <summary>
    /// 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 尼尔森标识
    /// </summary>
    public string? NielsenIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 中心记帐冻结（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? CentralPostingBlock { get; set; }

    /// <summary>
    /// 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）
    /// </summary>
    public string? ReconciliationAccount { get; set; } = string.Empty;

    /// <summary>
    /// 总部（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? Headquarters { get; set; } = string.Empty;

    /// <summary>
    /// 具有供应商的清算（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? ClearingWithVendor { get; set; }

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
    /// </summary>
    public string? PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int? PaymentMethod { get; set; }

    /// <summary>
    /// 交货工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? DeliveringPlant { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
    /// </summary>
    public string? Incoterms1 { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条件2（地点说明）
    /// </summary>
    public string? Incoterms2 { get; set; } = string.Empty;

    /// <summary>
    /// 装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）
    /// </summary>
    public string? ShippingConditions { get; set; } = string.Empty;

    /// <summary>
    /// 客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）
    /// </summary>
    public string? CustomerPricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
    /// </summary>
    public int? CreditLevel { get; set; }

    /// <summary>
    /// 信用额度（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? CreditAmount { get; set; }

    /// <summary>
    /// 折扣率（百分比；可选字典 logistics_sales_discount_rate_param 预设）
    /// </summary>
    public decimal? DiscountRate { get; set; }

    /// <summary>
    /// 客户等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）
    /// </summary>
    public int? CustomerLevel { get; set; }

    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    public decimal? EvaluationScore { get; set; }

    /// <summary>
    /// 客户状态（字典 sys_normal_disable）
    /// </summary>
    public int? CustomerStatus { get; set; }

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
/// Customer 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktCustomerExportDto
{
    /// <summary>
    /// CustomerID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CustomerId { get; set; }

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
    /// 客户编码（唯一索引）
    /// </summary>
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称1
    /// </summary>
    public string CustomerName1 { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称2
    /// </summary>
    public string? CustomerName2 { get; set; } = string.Empty;

    /// <summary>
    /// 客户简称
    /// </summary>
    public string? CustomerShortName { get; set; } = string.Empty;

    /// <summary>
    /// 客户类型（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
    /// </summary>
    public int CustomerType { get; set; } = 0;

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    public string EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    public string IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 客户标识（税务登记证号/统一社会信用代码）
    /// </summary>
    public string? CustomerTaxNumber { get; set; } = string.Empty;

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
    /// 客户电话
    /// </summary>
    public string? CustomerPhone { get; set; } = string.Empty;

    /// <summary>
    /// 客户传真
    /// </summary>
    public string? CustomerFax { get; set; } = string.Empty;

    /// <summary>
    /// 客户邮箱
    /// </summary>
    public string? CustomerEmail { get; set; } = string.Empty;

    /// <summary>
    /// 客户网站
    /// </summary>
    public string? CustomerWebsite { get; set; } = string.Empty;

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
    /// 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string SalesOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 分销渠道
    /// </summary>
    public string DistributionChannel { get; set; } = string.Empty;

    /// <summary>
    /// 产品组
    /// </summary>
    public string ProductGroup { get; set; } = string.Empty;

    /// <summary>
    /// 客户组（字典 logistics_sales_customer_group；DictValue=Z1～Z4）
    /// </summary>
    public string CustomerGroup { get; set; } = string.Empty;

    /// <summary>
    /// 贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string TradingPartner { get; set; } = string.Empty;

    /// <summary>
    /// 帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）
    /// </summary>
    public string AccountAssignmentGroup { get; set; } = string.Empty;

    /// <summary>
    /// 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 尼尔森标识
    /// </summary>
    public string NielsenIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 中心记帐冻结（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int CentralPostingBlock { get; set; } = 0;

    /// <summary>
    /// 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）
    /// </summary>
    public string ReconciliationAccount { get; set; } = string.Empty;

    /// <summary>
    /// 总部（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string Headquarters { get; set; } = string.Empty;

    /// <summary>
    /// 具有供应商的清算（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int ClearingWithVendor { get; set; } = 0;

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
    /// </summary>
    public string PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    public int PaymentMethod { get; set; } = 0;

    /// <summary>
    /// 交货工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string DeliveringPlant { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
    /// </summary>
    public string Incoterms1 { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条件2（地点说明）
    /// </summary>
    public string Incoterms2 { get; set; } = string.Empty;

    /// <summary>
    /// 装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）
    /// </summary>
    public string ShippingConditions { get; set; } = string.Empty;

    /// <summary>
    /// 客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）
    /// </summary>
    public string CustomerPricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
    /// </summary>
    public int CreditLevel { get; set; } = 0;

    /// <summary>
    /// 信用额度（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal CreditAmount { get; set; }

    /// <summary>
    /// 折扣率（百分比；可选字典 logistics_sales_discount_rate_param 预设）
    /// </summary>
    public decimal DiscountRate { get; set; }

    /// <summary>
    /// 客户等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）
    /// </summary>
    public int CustomerLevel { get; set; } = 0;

    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    public decimal EvaluationScore { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 客户状态（字典 sys_normal_disable）
    /// </summary>
    public int CustomerStatus { get; set; } = 0;

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

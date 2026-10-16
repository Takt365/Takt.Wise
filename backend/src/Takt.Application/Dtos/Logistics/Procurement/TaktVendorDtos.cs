// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Procurement
// 文件名称：TaktVendorDtos.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：Vendor 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktVendor 生成，请按需审阅）
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
// Vendor 响应 DTO
// ========================================

/// <summary>
/// Takt经销商实体 <para>业务唯一键：TenantCode+CompanyCode+PurchasingOrganization+VendorCode。</para>
/// 对应前端 TaktVendorDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktVendorDto : TaktCompanyDtoBase
{
    /// <summary>
    /// VendorID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long VendorId { get; set; }

    /// <summary>
    /// 经销商（与公司、采购组织共同唯一）
    /// </summary>
    public string VendorCode { get; set; } = string.Empty;

    /// <summary>
    /// 账户组
    /// </summary>
    public string AccountGroup { get; set; } = string.Empty;

    /// <summary>
    /// 称谓（字典 logistics_sales_customer_title；1=公司 2=先生 3=夫人 4=先生和夫人）
    /// </summary>
    public int VendorTitle { get; set; } = 0;

    /// <summary>
    /// 名称 1
    /// </summary>
    public string VendorName1 { get; set; } = string.Empty;

    /// <summary>
    /// 名称 2
    /// </summary>
    public string? VendorName2 { get; set; } = string.Empty;

    /// <summary>
    /// 名称 3
    /// </summary>
    public string? VendorName3 { get; set; } = string.Empty;

    /// <summary>
    /// 名称 4
    /// </summary>
    public string? VendorName4 { get; set; } = string.Empty;

    /// <summary>
    /// 简称
    /// </summary>
    public string? VendorShortName { get; set; } = string.Empty;

    /// <summary>
    /// 国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? RegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? CityCode { get; set; } = string.Empty;

    /// <summary>
    /// 地址1
    /// </summary>
    public string? Addr1 { get; set; } = string.Empty;

    /// <summary>
    /// 地址2（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr2 { get; set; } = string.Empty;

    /// <summary>
    /// 邮政编码
    /// </summary>
    public string? PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// 电话号码1
    /// </summary>
    public string? Phone1 { get; set; } = string.Empty;

    /// <summary>
    /// 电话号码2
    /// </summary>
    public string? Phone2 { get; set; } = string.Empty;

    /// <summary>
    /// 传真号码
    /// </summary>
    public string? FaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 贸易伙伴
    /// </summary>
    public string? TradingPartner { get; set; } = string.Empty;

    /// <summary>
    /// 权限组
    /// </summary>
    public string? AuthorizationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 组代码
    /// </summary>
    public string? GroupKey { get; set; } = string.Empty;

    /// <summary>
    /// 经销商类型（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
    /// </summary>
    public int VendorType { get; set; } = 0;

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    public string EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    public string IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 信用信息号
    /// </summary>
    public string? CreditInfoNumber { get; set; } = string.Empty;

    /// <summary>
    /// 银行国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? BankCountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行代码（选项 TaktBanks/options；DictValue=BankCode）
    /// </summary>
    public string? BankCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行账户
    /// </summary>
    public string? BankAccount { get; set; } = string.Empty;

    /// <summary>
    /// 科目持有者
    /// </summary>
    public string? AccountHolder { get; set; } = string.Empty;

    /// <summary>
    /// 指令码
    /// </summary>
    public string? BankChargeBearerCode { get; set; } = string.Empty;

    /// <summary>
    /// 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）
    /// </summary>
    public string ReconciliationAccount { get; set; } = string.Empty;

    /// <summary>
    /// 总部
    /// </summary>
    public string? HeadOfficeAccountCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划组
    /// </summary>
    public string? PlanningGroup { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）
    /// </summary>
    public string PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 检查双重发票（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int DoubleInvoiceCheck { get; set; } = 0;

    /// <summary>
    /// 付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
    /// </summary>
    public int PaymentMethod { get; set; } = 0;

    /// <summary>
    /// 付款冻结（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int PaymentBlock { get; set; } = 0;

    /// <summary>
    /// 开户行（选项 TaktBanks/options；DictValue=BankCode）
    /// </summary>
    public string? HouseBank { get; set; } = string.Empty;

    /// <summary>
    /// 在客户和供应商之间清算（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int ClearingWithCustomer { get; set; } = 0;

    /// <summary>
    /// 会计职员
    /// </summary>
    public string? AccountingClerk { get; set; } = string.Empty;

    /// <summary>
    /// 我们带有经销商的帐目号
    /// </summary>
    public string? OurAccountAtVendor { get; set; } = string.Empty;

    /// <summary>
    /// 经销商联系人
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 经销商联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 经销商联系邮件（邮箱地址短字符串 40 不够）
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 代扣所得税国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? WithholdingTaxCountry { get; set; } = string.Empty;

    /// <summary>
    /// 预扣税类型（选项 TaktTaxCodes/options；DictValue=TaxCode；选中后回填预扣税代码）
    /// </summary>
    public string? WithholdingTaxType { get; set; } = string.Empty;

    /// <summary>
    /// 预扣税代码（由预扣税类型 WithholdingTaxType 回填；选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? WithholdingTaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PurchasingOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 订单货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string OrderCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
    /// </summary>
    public string Incoterms1 { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条款2（地点说明）
    /// </summary>
    public string Incoterms2 { get; set; } = string.Empty;

    /// <summary>
    /// 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
    /// </summary>
    public int PricingDateControl { get; set; } = 0;

    /// <summary>
    /// 基于收货的发票校验（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int GrBasedInvoiceInspection { get; set; } = 0;

    /// <summary>
    /// 自动评估GRR结算交货（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int EvaluatedReceiptSettlement { get; set; } = 0;

    /// <summary>
    /// 自动评估GR结算保留（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int ReturnErsFlag { get; set; } = 0;

    /// <summary>
    /// 自动建立采购订单（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int AutomaticPurchaseOrder { get; set; } = 0;

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    public string PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 税号1
    /// </summary>
    public string? TaxNumber1 { get; set; } = string.Empty;

    /// <summary>
    /// 税号2
    /// </summary>
    public string? TaxNumber2 { get; set; } = string.Empty;

    /// <summary>
    /// 税号3（来源主数据最长 60）
    /// </summary>
    public string? TaxNumber3 { get; set; } = string.Empty;

    /// <summary>
    /// 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
    /// </summary>
    public int CreditLevel { get; set; } = 0;

    /// <summary>
    /// 经销商等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）
    /// </summary>
    public int VendorLevel { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 经销商状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int VendorStatus { get; set; } = 0;

}

// ========================================
// Vendor 查询 DTO
// ========================================

/// <summary>
/// Vendor 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktVendorQueryDto : TaktPagedQuery
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
    /// 经销商（与公司、采购组织共同唯一）
    /// </summary>
    public string? VendorCode { get; set; } = string.Empty;

    /// <summary>
    /// 账户组
    /// </summary>
    public string? AccountGroup { get; set; } = string.Empty;

    /// <summary>
    /// 称谓（字典 logistics_sales_customer_title；1=公司 2=先生 3=夫人 4=先生和夫人）
    /// </summary>
    public int? VendorTitle { get; set; }

    /// <summary>
    /// 名称 1
    /// </summary>
    public string? VendorName1 { get; set; } = string.Empty;

    /// <summary>
    /// 名称 2
    /// </summary>
    public string? VendorName2 { get; set; } = string.Empty;

    /// <summary>
    /// 名称 3
    /// </summary>
    public string? VendorName3 { get; set; } = string.Empty;

    /// <summary>
    /// 名称 4
    /// </summary>
    public string? VendorName4 { get; set; } = string.Empty;

    /// <summary>
    /// 简称
    /// </summary>
    public string? VendorShortName { get; set; } = string.Empty;

    /// <summary>
    /// 国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? RegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? CityCode { get; set; } = string.Empty;

    /// <summary>
    /// 地址1
    /// </summary>
    public string? Addr1 { get; set; } = string.Empty;

    /// <summary>
    /// 地址2（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr2 { get; set; } = string.Empty;

    /// <summary>
    /// 邮政编码
    /// </summary>
    public string? PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// 电话号码1
    /// </summary>
    public string? Phone1 { get; set; } = string.Empty;

    /// <summary>
    /// 电话号码2
    /// </summary>
    public string? Phone2 { get; set; } = string.Empty;

    /// <summary>
    /// 传真号码
    /// </summary>
    public string? FaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 贸易伙伴
    /// </summary>
    public string? TradingPartner { get; set; } = string.Empty;

    /// <summary>
    /// 权限组
    /// </summary>
    public string? AuthorizationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 组代码
    /// </summary>
    public string? GroupKey { get; set; } = string.Empty;

    /// <summary>
    /// 经销商类型（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
    /// </summary>
    public int? VendorType { get; set; }

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    public string? EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    public string? IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 信用信息号
    /// </summary>
    public string? CreditInfoNumber { get; set; } = string.Empty;

    /// <summary>
    /// 银行国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? BankCountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行代码（选项 TaktBanks/options；DictValue=BankCode）
    /// </summary>
    public string? BankCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行账户
    /// </summary>
    public string? BankAccount { get; set; } = string.Empty;

    /// <summary>
    /// 科目持有者
    /// </summary>
    public string? AccountHolder { get; set; } = string.Empty;

    /// <summary>
    /// 指令码
    /// </summary>
    public string? BankChargeBearerCode { get; set; } = string.Empty;

    /// <summary>
    /// 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）
    /// </summary>
    public string? ReconciliationAccount { get; set; } = string.Empty;

    /// <summary>
    /// 总部
    /// </summary>
    public string? HeadOfficeAccountCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划组
    /// </summary>
    public string? PlanningGroup { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）
    /// </summary>
    public string? PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 检查双重发票（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? DoubleInvoiceCheck { get; set; }

    /// <summary>
    /// 付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
    /// </summary>
    public int? PaymentMethod { get; set; }

    /// <summary>
    /// 付款冻结（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? PaymentBlock { get; set; }

    /// <summary>
    /// 开户行（选项 TaktBanks/options；DictValue=BankCode）
    /// </summary>
    public string? HouseBank { get; set; } = string.Empty;

    /// <summary>
    /// 在客户和供应商之间清算（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? ClearingWithCustomer { get; set; }

    /// <summary>
    /// 会计职员
    /// </summary>
    public string? AccountingClerk { get; set; } = string.Empty;

    /// <summary>
    /// 我们带有经销商的帐目号
    /// </summary>
    public string? OurAccountAtVendor { get; set; } = string.Empty;

    /// <summary>
    /// 经销商联系人
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 经销商联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 经销商联系邮件（邮箱地址短字符串 40 不够）
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 代扣所得税国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? WithholdingTaxCountry { get; set; } = string.Empty;

    /// <summary>
    /// 预扣税类型（选项 TaktTaxCodes/options；DictValue=TaxCode；选中后回填预扣税代码）
    /// </summary>
    public string? WithholdingTaxType { get; set; } = string.Empty;

    /// <summary>
    /// 预扣税代码（由预扣税类型 WithholdingTaxType 回填；选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? WithholdingTaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PurchasingOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 订单货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string? OrderCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
    /// </summary>
    public string? Incoterms1 { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条款2（地点说明）
    /// </summary>
    public string? Incoterms2 { get; set; } = string.Empty;

    /// <summary>
    /// 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
    /// </summary>
    public int? PricingDateControl { get; set; }

    /// <summary>
    /// 基于收货的发票校验（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? GrBasedInvoiceInspection { get; set; }

    /// <summary>
    /// 自动评估GRR结算交货（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? EvaluatedReceiptSettlement { get; set; }

    /// <summary>
    /// 自动评估GR结算保留（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? ReturnErsFlag { get; set; }

    /// <summary>
    /// 自动建立采购订单（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? AutomaticPurchaseOrder { get; set; }

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 税号1
    /// </summary>
    public string? TaxNumber1 { get; set; } = string.Empty;

    /// <summary>
    /// 税号2
    /// </summary>
    public string? TaxNumber2 { get; set; } = string.Empty;

    /// <summary>
    /// 税号3（来源主数据最长 60）
    /// </summary>
    public string? TaxNumber3 { get; set; } = string.Empty;

    /// <summary>
    /// 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
    /// </summary>
    public int? CreditLevel { get; set; }

    /// <summary>
    /// 经销商等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）
    /// </summary>
    public int? VendorLevel { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 经销商状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? VendorStatus { get; set; }

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
// 创建Vendor DTO
// ========================================

/// <summary>
/// 创建Vendor DTO
/// </summary>
public class TaktVendorCreateDto
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
    /// 经销商（与公司、采购组织共同唯一）
    /// </summary>
    [Required(ErrorMessage = "经销商（与公司、采购组织共同唯一）不能为空")]
    public string VendorCode { get; set; } = string.Empty;

    /// <summary>
    /// 账户组
    /// </summary>
    [Required(ErrorMessage = "账户组不能为空")]
    public string AccountGroup { get; set; } = string.Empty;

    /// <summary>
    /// 称谓（字典 logistics_sales_customer_title；1=公司 2=先生 3=夫人 4=先生和夫人）
    /// </summary>
    public int VendorTitle { get; set; } = 0;

    /// <summary>
    /// 名称 1
    /// </summary>
    [Required(ErrorMessage = "名称 1不能为空")]
    public string VendorName1 { get; set; } = string.Empty;

    /// <summary>
    /// 名称 2
    /// </summary>
    public string? VendorName2 { get; set; } = string.Empty;

    /// <summary>
    /// 名称 3
    /// </summary>
    public string? VendorName3 { get; set; } = string.Empty;

    /// <summary>
    /// 名称 4
    /// </summary>
    public string? VendorName4 { get; set; } = string.Empty;

    /// <summary>
    /// 简称
    /// </summary>
    public string? VendorShortName { get; set; } = string.Empty;

    /// <summary>
    /// 国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? RegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? CityCode { get; set; } = string.Empty;

    /// <summary>
    /// 地址1
    /// </summary>
    public string? Addr1 { get; set; } = string.Empty;

    /// <summary>
    /// 地址2（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr2 { get; set; } = string.Empty;

    /// <summary>
    /// 邮政编码
    /// </summary>
    public string? PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// 电话号码1
    /// </summary>
    public string? Phone1 { get; set; } = string.Empty;

    /// <summary>
    /// 电话号码2
    /// </summary>
    public string? Phone2 { get; set; } = string.Empty;

    /// <summary>
    /// 传真号码
    /// </summary>
    public string? FaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 贸易伙伴
    /// </summary>
    public string? TradingPartner { get; set; } = string.Empty;

    /// <summary>
    /// 权限组
    /// </summary>
    public string? AuthorizationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 组代码
    /// </summary>
    public string? GroupKey { get; set; } = string.Empty;

    /// <summary>
    /// 经销商类型（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
    /// </summary>
    public int VendorType { get; set; } = 0;

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
    /// 信用信息号
    /// </summary>
    public string? CreditInfoNumber { get; set; } = string.Empty;

    /// <summary>
    /// 银行国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? BankCountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行代码（选项 TaktBanks/options；DictValue=BankCode）
    /// </summary>
    public string? BankCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行账户
    /// </summary>
    public string? BankAccount { get; set; } = string.Empty;

    /// <summary>
    /// 科目持有者
    /// </summary>
    public string? AccountHolder { get; set; } = string.Empty;

    /// <summary>
    /// 指令码
    /// </summary>
    public string? BankChargeBearerCode { get; set; } = string.Empty;

    /// <summary>
    /// 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）
    /// </summary>
    [Required(ErrorMessage = "统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）不能为空")]
    public string ReconciliationAccount { get; set; } = string.Empty;

    /// <summary>
    /// 总部
    /// </summary>
    public string? HeadOfficeAccountCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划组
    /// </summary>
    public string? PlanningGroup { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）
    /// </summary>
    [Required(ErrorMessage = "付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）不能为空")]
    public string PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 检查双重发票（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int DoubleInvoiceCheck { get; set; } = 0;

    /// <summary>
    /// 付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
    /// </summary>
    public int PaymentMethod { get; set; } = 0;

    /// <summary>
    /// 付款冻结（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int PaymentBlock { get; set; } = 0;

    /// <summary>
    /// 开户行（选项 TaktBanks/options；DictValue=BankCode）
    /// </summary>
    public string? HouseBank { get; set; } = string.Empty;

    /// <summary>
    /// 在客户和供应商之间清算（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int ClearingWithCustomer { get; set; } = 0;

    /// <summary>
    /// 会计职员
    /// </summary>
    public string? AccountingClerk { get; set; } = string.Empty;

    /// <summary>
    /// 我们带有经销商的帐目号
    /// </summary>
    public string? OurAccountAtVendor { get; set; } = string.Empty;

    /// <summary>
    /// 经销商联系人
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 经销商联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 经销商联系邮件（邮箱地址短字符串 40 不够）
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 代扣所得税国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? WithholdingTaxCountry { get; set; } = string.Empty;

    /// <summary>
    /// 预扣税类型（选项 TaktTaxCodes/options；DictValue=TaxCode；选中后回填预扣税代码）
    /// </summary>
    public string? WithholdingTaxType { get; set; } = string.Empty;

    /// <summary>
    /// 预扣税代码（由预扣税类型 WithholdingTaxType 回填；选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? WithholdingTaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "采购组织（选项 TaktPlants/options；DictValue=PlantCode）不能为空")]
    public string PurchasingOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 订单货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    [Required(ErrorMessage = "订单货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）不能为空")]
    public string OrderCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
    /// </summary>
    [Required(ErrorMessage = "国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）不能为空")]
    public string Incoterms1 { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条款2（地点说明）
    /// </summary>
    [Required(ErrorMessage = "国际贸易条款2（地点说明）不能为空")]
    public string Incoterms2 { get; set; } = string.Empty;

    /// <summary>
    /// 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
    /// </summary>
    public int PricingDateControl { get; set; } = 0;

    /// <summary>
    /// 基于收货的发票校验（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int GrBasedInvoiceInspection { get; set; } = 0;

    /// <summary>
    /// 自动评估GRR结算交货（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int EvaluatedReceiptSettlement { get; set; } = 0;

    /// <summary>
    /// 自动评估GR结算保留（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int ReturnErsFlag { get; set; } = 0;

    /// <summary>
    /// 自动建立采购订单（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int AutomaticPurchaseOrder { get; set; } = 0;

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    [Required(ErrorMessage = "采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）不能为空")]
    public string PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 税号1
    /// </summary>
    public string? TaxNumber1 { get; set; } = string.Empty;

    /// <summary>
    /// 税号2
    /// </summary>
    public string? TaxNumber2 { get; set; } = string.Empty;

    /// <summary>
    /// 税号3（来源主数据最长 60）
    /// </summary>
    public string? TaxNumber3 { get; set; } = string.Empty;

    /// <summary>
    /// 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
    /// </summary>
    public int CreditLevel { get; set; } = 0;

    /// <summary>
    /// 经销商等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）
    /// </summary>
    public int VendorLevel { get; set; } = 0;

    /// <summary>
    /// 经销商状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int VendorStatus { get; set; } = 0;

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
// 更新Vendor DTO
// ========================================

/// <summary>
/// 更新Vendor DTO
/// 继承 TaktVendorCreateDto，添加 VendorId 字段
/// </summary>
public class TaktVendorUpdateDto : TaktVendorCreateDto
{
    /// <summary>
    /// VendorID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long VendorId { get; set; }

}

// ========================================
// Vendor 状态 DTO
// ========================================

/// <summary>
/// Vendor 状态更新 DTO
/// </summary>
public class TaktVendorStatusDto
{
    /// <summary>
    /// VendorID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long VendorId { get; set; }

    /// <summary>
    /// 经销商状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "经销商状态（字典 sys_normal_disable；1=启用，0=禁用）不能为空")]
    public int VendorStatus { get; set; } = 0;
}

// ========================================
// Vendor 排序 DTO
// ========================================

/// <summary>
/// Vendor 排序更新 DTO
/// </summary>
public class TaktVendorSortDto
{
    /// <summary>
    /// VendorID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long VendorId { get; set; }

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
/// Vendor 导入模板行 DTO
/// </summary>
public class TaktVendorTemplateDto
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
    /// 经销商（与公司、采购组织共同唯一）
    /// </summary>
    public string? VendorCode { get; set; } = string.Empty;

    /// <summary>
    /// 账户组
    /// </summary>
    public string? AccountGroup { get; set; } = string.Empty;

    /// <summary>
    /// 称谓（字典 logistics_sales_customer_title；1=公司 2=先生 3=夫人 4=先生和夫人）
    /// </summary>
    public int? VendorTitle { get; set; }

    /// <summary>
    /// 名称 1
    /// </summary>
    public string? VendorName1 { get; set; } = string.Empty;

    /// <summary>
    /// 名称 2
    /// </summary>
    public string? VendorName2 { get; set; } = string.Empty;

    /// <summary>
    /// 名称 3
    /// </summary>
    public string? VendorName3 { get; set; } = string.Empty;

    /// <summary>
    /// 名称 4
    /// </summary>
    public string? VendorName4 { get; set; } = string.Empty;

    /// <summary>
    /// 简称
    /// </summary>
    public string? VendorShortName { get; set; } = string.Empty;

    /// <summary>
    /// 国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? RegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? CityCode { get; set; } = string.Empty;

    /// <summary>
    /// 地址1
    /// </summary>
    public string? Addr1 { get; set; } = string.Empty;

    /// <summary>
    /// 地址2（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr2 { get; set; } = string.Empty;

    /// <summary>
    /// 邮政编码
    /// </summary>
    public string? PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// 电话号码1
    /// </summary>
    public string? Phone1 { get; set; } = string.Empty;

    /// <summary>
    /// 电话号码2
    /// </summary>
    public string? Phone2 { get; set; } = string.Empty;

    /// <summary>
    /// 传真号码
    /// </summary>
    public string? FaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 贸易伙伴
    /// </summary>
    public string? TradingPartner { get; set; } = string.Empty;

    /// <summary>
    /// 权限组
    /// </summary>
    public string? AuthorizationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 组代码
    /// </summary>
    public string? GroupKey { get; set; } = string.Empty;

    /// <summary>
    /// 经销商类型（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
    /// </summary>
    public int? VendorType { get; set; }

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    public string? EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    public string? IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 信用信息号
    /// </summary>
    public string? CreditInfoNumber { get; set; } = string.Empty;

    /// <summary>
    /// 银行国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? BankCountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行代码（选项 TaktBanks/options；DictValue=BankCode）
    /// </summary>
    public string? BankCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行账户
    /// </summary>
    public string? BankAccount { get; set; } = string.Empty;

    /// <summary>
    /// 科目持有者
    /// </summary>
    public string? AccountHolder { get; set; } = string.Empty;

    /// <summary>
    /// 指令码
    /// </summary>
    public string? BankChargeBearerCode { get; set; } = string.Empty;

    /// <summary>
    /// 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）
    /// </summary>
    public string? ReconciliationAccount { get; set; } = string.Empty;

    /// <summary>
    /// 总部
    /// </summary>
    public string? HeadOfficeAccountCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划组
    /// </summary>
    public string? PlanningGroup { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）
    /// </summary>
    public string? PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 检查双重发票（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? DoubleInvoiceCheck { get; set; }

    /// <summary>
    /// 付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
    /// </summary>
    public int? PaymentMethod { get; set; }

    /// <summary>
    /// 付款冻结（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? PaymentBlock { get; set; }

    /// <summary>
    /// 开户行（选项 TaktBanks/options；DictValue=BankCode）
    /// </summary>
    public string? HouseBank { get; set; } = string.Empty;

    /// <summary>
    /// 在客户和供应商之间清算（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? ClearingWithCustomer { get; set; }

    /// <summary>
    /// 会计职员
    /// </summary>
    public string? AccountingClerk { get; set; } = string.Empty;

    /// <summary>
    /// 我们带有经销商的帐目号
    /// </summary>
    public string? OurAccountAtVendor { get; set; } = string.Empty;

    /// <summary>
    /// 经销商联系人
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 经销商联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 经销商联系邮件（邮箱地址短字符串 40 不够）
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 代扣所得税国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? WithholdingTaxCountry { get; set; } = string.Empty;

    /// <summary>
    /// 预扣税类型（选项 TaktTaxCodes/options；DictValue=TaxCode；选中后回填预扣税代码）
    /// </summary>
    public string? WithholdingTaxType { get; set; } = string.Empty;

    /// <summary>
    /// 预扣税代码（由预扣税类型 WithholdingTaxType 回填；选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? WithholdingTaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PurchasingOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 订单货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string? OrderCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
    /// </summary>
    public string? Incoterms1 { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条款2（地点说明）
    /// </summary>
    public string? Incoterms2 { get; set; } = string.Empty;

    /// <summary>
    /// 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
    /// </summary>
    public int? PricingDateControl { get; set; }

    /// <summary>
    /// 基于收货的发票校验（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? GrBasedInvoiceInspection { get; set; }

    /// <summary>
    /// 自动评估GRR结算交货（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? EvaluatedReceiptSettlement { get; set; }

    /// <summary>
    /// 自动评估GR结算保留（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? ReturnErsFlag { get; set; }

    /// <summary>
    /// 自动建立采购订单（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? AutomaticPurchaseOrder { get; set; }

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 税号1
    /// </summary>
    public string? TaxNumber1 { get; set; } = string.Empty;

    /// <summary>
    /// 税号2
    /// </summary>
    public string? TaxNumber2 { get; set; } = string.Empty;

    /// <summary>
    /// 税号3（来源主数据最长 60）
    /// </summary>
    public string? TaxNumber3 { get; set; } = string.Empty;

    /// <summary>
    /// 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
    /// </summary>
    public int? CreditLevel { get; set; }

    /// <summary>
    /// 经销商等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）
    /// </summary>
    public int? VendorLevel { get; set; }

    /// <summary>
    /// 经销商状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? VendorStatus { get; set; }

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
/// Vendor 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktVendorImportDto
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
    /// 经销商（与公司、采购组织共同唯一）
    /// </summary>
    public string? VendorCode { get; set; } = string.Empty;

    /// <summary>
    /// 账户组
    /// </summary>
    public string? AccountGroup { get; set; } = string.Empty;

    /// <summary>
    /// 称谓（字典 logistics_sales_customer_title；1=公司 2=先生 3=夫人 4=先生和夫人）
    /// </summary>
    public int? VendorTitle { get; set; }

    /// <summary>
    /// 名称 1
    /// </summary>
    public string? VendorName1 { get; set; } = string.Empty;

    /// <summary>
    /// 名称 2
    /// </summary>
    public string? VendorName2 { get; set; } = string.Empty;

    /// <summary>
    /// 名称 3
    /// </summary>
    public string? VendorName3 { get; set; } = string.Empty;

    /// <summary>
    /// 名称 4
    /// </summary>
    public string? VendorName4 { get; set; } = string.Empty;

    /// <summary>
    /// 简称
    /// </summary>
    public string? VendorShortName { get; set; } = string.Empty;

    /// <summary>
    /// 国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? RegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? CityCode { get; set; } = string.Empty;

    /// <summary>
    /// 地址1
    /// </summary>
    public string? Addr1 { get; set; } = string.Empty;

    /// <summary>
    /// 地址2（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr2 { get; set; } = string.Empty;

    /// <summary>
    /// 邮政编码
    /// </summary>
    public string? PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// 电话号码1
    /// </summary>
    public string? Phone1 { get; set; } = string.Empty;

    /// <summary>
    /// 电话号码2
    /// </summary>
    public string? Phone2 { get; set; } = string.Empty;

    /// <summary>
    /// 传真号码
    /// </summary>
    public string? FaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 贸易伙伴
    /// </summary>
    public string? TradingPartner { get; set; } = string.Empty;

    /// <summary>
    /// 权限组
    /// </summary>
    public string? AuthorizationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 组代码
    /// </summary>
    public string? GroupKey { get; set; } = string.Empty;

    /// <summary>
    /// 经销商类型（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
    /// </summary>
    public int? VendorType { get; set; }

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    public string? EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    public string? IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 信用信息号
    /// </summary>
    public string? CreditInfoNumber { get; set; } = string.Empty;

    /// <summary>
    /// 银行国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? BankCountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行代码（选项 TaktBanks/options；DictValue=BankCode）
    /// </summary>
    public string? BankCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行账户
    /// </summary>
    public string? BankAccount { get; set; } = string.Empty;

    /// <summary>
    /// 科目持有者
    /// </summary>
    public string? AccountHolder { get; set; } = string.Empty;

    /// <summary>
    /// 指令码
    /// </summary>
    public string? BankChargeBearerCode { get; set; } = string.Empty;

    /// <summary>
    /// 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）
    /// </summary>
    public string? ReconciliationAccount { get; set; } = string.Empty;

    /// <summary>
    /// 总部
    /// </summary>
    public string? HeadOfficeAccountCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划组
    /// </summary>
    public string? PlanningGroup { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）
    /// </summary>
    public string? PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 检查双重发票（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? DoubleInvoiceCheck { get; set; }

    /// <summary>
    /// 付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
    /// </summary>
    public int? PaymentMethod { get; set; }

    /// <summary>
    /// 付款冻结（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? PaymentBlock { get; set; }

    /// <summary>
    /// 开户行（选项 TaktBanks/options；DictValue=BankCode）
    /// </summary>
    public string? HouseBank { get; set; } = string.Empty;

    /// <summary>
    /// 在客户和供应商之间清算（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? ClearingWithCustomer { get; set; }

    /// <summary>
    /// 会计职员
    /// </summary>
    public string? AccountingClerk { get; set; } = string.Empty;

    /// <summary>
    /// 我们带有经销商的帐目号
    /// </summary>
    public string? OurAccountAtVendor { get; set; } = string.Empty;

    /// <summary>
    /// 经销商联系人
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 经销商联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 经销商联系邮件（邮箱地址短字符串 40 不够）
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 代扣所得税国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? WithholdingTaxCountry { get; set; } = string.Empty;

    /// <summary>
    /// 预扣税类型（选项 TaktTaxCodes/options；DictValue=TaxCode；选中后回填预扣税代码）
    /// </summary>
    public string? WithholdingTaxType { get; set; } = string.Empty;

    /// <summary>
    /// 预扣税代码（由预扣税类型 WithholdingTaxType 回填；选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? WithholdingTaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PurchasingOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 订单货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string? OrderCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
    /// </summary>
    public string? Incoterms1 { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条款2（地点说明）
    /// </summary>
    public string? Incoterms2 { get; set; } = string.Empty;

    /// <summary>
    /// 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
    /// </summary>
    public int? PricingDateControl { get; set; }

    /// <summary>
    /// 基于收货的发票校验（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? GrBasedInvoiceInspection { get; set; }

    /// <summary>
    /// 自动评估GRR结算交货（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? EvaluatedReceiptSettlement { get; set; }

    /// <summary>
    /// 自动评估GR结算保留（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? ReturnErsFlag { get; set; }

    /// <summary>
    /// 自动建立采购订单（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? AutomaticPurchaseOrder { get; set; }

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 税号1
    /// </summary>
    public string? TaxNumber1 { get; set; } = string.Empty;

    /// <summary>
    /// 税号2
    /// </summary>
    public string? TaxNumber2 { get; set; } = string.Empty;

    /// <summary>
    /// 税号3（来源主数据最长 60）
    /// </summary>
    public string? TaxNumber3 { get; set; } = string.Empty;

    /// <summary>
    /// 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
    /// </summary>
    public int? CreditLevel { get; set; }

    /// <summary>
    /// 经销商等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）
    /// </summary>
    public int? VendorLevel { get; set; }

    /// <summary>
    /// 经销商状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? VendorStatus { get; set; }

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
/// Vendor 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktVendorExportDto
{
    /// <summary>
    /// VendorID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long VendorId { get; set; }

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
    /// 经销商（与公司、采购组织共同唯一）
    /// </summary>
    public string VendorCode { get; set; } = string.Empty;

    /// <summary>
    /// 账户组
    /// </summary>
    public string AccountGroup { get; set; } = string.Empty;

    /// <summary>
    /// 称谓（字典 logistics_sales_customer_title；1=公司 2=先生 3=夫人 4=先生和夫人）
    /// </summary>
    public int VendorTitle { get; set; } = 0;

    /// <summary>
    /// 名称 1
    /// </summary>
    public string VendorName1 { get; set; } = string.Empty;

    /// <summary>
    /// 名称 2
    /// </summary>
    public string? VendorName2 { get; set; } = string.Empty;

    /// <summary>
    /// 名称 3
    /// </summary>
    public string? VendorName3 { get; set; } = string.Empty;

    /// <summary>
    /// 名称 4
    /// </summary>
    public string? VendorName4 { get; set; } = string.Empty;

    /// <summary>
    /// 简称
    /// </summary>
    public string? VendorShortName { get; set; } = string.Empty;

    /// <summary>
    /// 国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? RegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? CityCode { get; set; } = string.Empty;

    /// <summary>
    /// 地址1
    /// </summary>
    public string? Addr1 { get; set; } = string.Empty;

    /// <summary>
    /// 地址2（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr2 { get; set; } = string.Empty;

    /// <summary>
    /// 邮政编码
    /// </summary>
    public string? PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// 电话号码1
    /// </summary>
    public string? Phone1 { get; set; } = string.Empty;

    /// <summary>
    /// 电话号码2
    /// </summary>
    public string? Phone2 { get; set; } = string.Empty;

    /// <summary>
    /// 传真号码
    /// </summary>
    public string? FaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 贸易伙伴
    /// </summary>
    public string? TradingPartner { get; set; } = string.Empty;

    /// <summary>
    /// 权限组
    /// </summary>
    public string? AuthorizationGroup { get; set; } = string.Empty;

    /// <summary>
    /// 组代码
    /// </summary>
    public string? GroupKey { get; set; } = string.Empty;

    /// <summary>
    /// 经销商类型（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
    /// </summary>
    public int VendorType { get; set; } = 0;

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    public string EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    public string IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 信用信息号
    /// </summary>
    public string? CreditInfoNumber { get; set; } = string.Empty;

    /// <summary>
    /// 银行国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? BankCountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行代码（选项 TaktBanks/options；DictValue=BankCode）
    /// </summary>
    public string? BankCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行账户
    /// </summary>
    public string? BankAccount { get; set; } = string.Empty;

    /// <summary>
    /// 科目持有者
    /// </summary>
    public string? AccountHolder { get; set; } = string.Empty;

    /// <summary>
    /// 指令码
    /// </summary>
    public string? BankChargeBearerCode { get; set; } = string.Empty;

    /// <summary>
    /// 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）
    /// </summary>
    public string ReconciliationAccount { get; set; } = string.Empty;

    /// <summary>
    /// 总部
    /// </summary>
    public string? HeadOfficeAccountCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划组
    /// </summary>
    public string? PlanningGroup { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）
    /// </summary>
    public string PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 检查双重发票（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int DoubleInvoiceCheck { get; set; } = 0;

    /// <summary>
    /// 付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
    /// </summary>
    public int PaymentMethod { get; set; } = 0;

    /// <summary>
    /// 付款冻结（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int PaymentBlock { get; set; } = 0;

    /// <summary>
    /// 开户行（选项 TaktBanks/options；DictValue=BankCode）
    /// </summary>
    public string? HouseBank { get; set; } = string.Empty;

    /// <summary>
    /// 在客户和供应商之间清算（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int ClearingWithCustomer { get; set; } = 0;

    /// <summary>
    /// 会计职员
    /// </summary>
    public string? AccountingClerk { get; set; } = string.Empty;

    /// <summary>
    /// 我们带有经销商的帐目号
    /// </summary>
    public string? OurAccountAtVendor { get; set; } = string.Empty;

    /// <summary>
    /// 经销商联系人
    /// </summary>
    public string? ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// 经销商联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 经销商联系邮件（邮箱地址短字符串 40 不够）
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 代扣所得税国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? WithholdingTaxCountry { get; set; } = string.Empty;

    /// <summary>
    /// 预扣税类型（选项 TaktTaxCodes/options；DictValue=TaxCode；选中后回填预扣税代码）
    /// </summary>
    public string? WithholdingTaxType { get; set; } = string.Empty;

    /// <summary>
    /// 预扣税代码（由预扣税类型 WithholdingTaxType 回填；选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? WithholdingTaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PurchasingOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 订单货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string OrderCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
    /// </summary>
    public string Incoterms1 { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条款2（地点说明）
    /// </summary>
    public string Incoterms2 { get; set; } = string.Empty;

    /// <summary>
    /// 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
    /// </summary>
    public int PricingDateControl { get; set; } = 0;

    /// <summary>
    /// 基于收货的发票校验（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int GrBasedInvoiceInspection { get; set; } = 0;

    /// <summary>
    /// 自动评估GRR结算交货（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int EvaluatedReceiptSettlement { get; set; } = 0;

    /// <summary>
    /// 自动评估GR结算保留（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int ReturnErsFlag { get; set; } = 0;

    /// <summary>
    /// 自动建立采购订单（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int AutomaticPurchaseOrder { get; set; } = 0;

    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    public string PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 税号1
    /// </summary>
    public string? TaxNumber1 { get; set; } = string.Empty;

    /// <summary>
    /// 税号2
    /// </summary>
    public string? TaxNumber2 { get; set; } = string.Empty;

    /// <summary>
    /// 税号3（来源主数据最长 60）
    /// </summary>
    public string? TaxNumber3 { get; set; } = string.Empty;

    /// <summary>
    /// 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
    /// </summary>
    public int CreditLevel { get; set; } = 0;

    /// <summary>
    /// 经销商等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）
    /// </summary>
    public int VendorLevel { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 经销商状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int VendorStatus { get; set; } = 0;

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

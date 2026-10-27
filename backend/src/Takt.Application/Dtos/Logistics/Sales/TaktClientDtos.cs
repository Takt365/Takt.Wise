// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Sales
// 文件名称：TaktClientDtos.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：Client 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktClient 生成，请按需审阅）
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
// Client 响应 DTO
// ========================================

/// <summary>
/// Takt客户端信息实体 <para>业务唯一键：TenantCode+CompanyCode+SalesOrganization+ClientCode。</para>
/// 对应前端 TaktClientDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktClientDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ClientID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ClientId { get; set; }

    /// <summary>
    /// 客户端编码（与租户、公司、销售组织共同唯一；前端表单选择编码规则后自动通过 TaktNumbering 客户端编码规则生成并展示，非手输；单据类型菜单：顾客信息）
    /// </summary>
    public string ClientCode { get; set; } = string.Empty;

    /// <summary>
    /// 科目组
    /// </summary>
    public string AccountGroup { get; set; } = string.Empty;

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
    /// 称谓（字典 logistics_sales_client_title；1=公司 2=先生 3=夫人 4=先生和夫人）
    /// </summary>
    public int ClientTitle { get; set; } = 0;

    /// <summary>
    /// 名称1
    /// </summary>
    public string ClientName1 { get; set; } = string.Empty;

    /// <summary>
    /// 名称2
    /// </summary>
    public string? ClientName2 { get; set; } = string.Empty;

    /// <summary>
    /// 名称3
    /// </summary>
    public string? ClientName3 { get; set; } = string.Empty;

    /// <summary>
    /// 名称4
    /// </summary>
    public string? ClientName4 { get; set; } = string.Empty;

    /// <summary>
    /// 简称
    /// </summary>
    public string? ClientShortName { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? RegistrationCountry { get; set; } = string.Empty;

    /// <summary>
    /// 注册地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? RegistrationRegion { get; set; } = string.Empty;

    /// <summary>
    /// 注册城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? RegistrationCity { get; set; } = string.Empty;

    /// <summary>
    /// 地址1（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr1 { get; set; } = string.Empty;

    /// <summary>
    /// 地址2（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr2 { get; set; } = string.Empty;

    /// <summary>
    /// 地址3（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr3 { get; set; } = string.Empty;

    /// <summary>
    /// 地址4（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr4 { get; set; } = string.Empty;

    /// <summary>
    /// 地址5（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr5 { get; set; } = string.Empty;

    /// <summary>
    /// 邮政编码
    /// </summary>
    public string? PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// 电话1
    /// </summary>
    public string? Phone1 { get; set; } = string.Empty;

    /// <summary>
    /// 电话2
    /// </summary>
    public string? Phone2 { get; set; } = string.Empty;

    /// <summary>
    /// 传真
    /// </summary>
    public string? FaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string TradingPartner { get; set; } = string.Empty;

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
    /// 银行国家（字典 sys_country_code；DictValue=ISO alpha-2）
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
    /// 账户持有人
    /// </summary>
    public string? BankAccountHolder { get; set; } = string.Empty;

    /// <summary>
    /// 客户端分类（字典 logistics_sales_client_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
    /// </summary>
    public int ClientCategory { get; set; } = 0;

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    public string EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    public string IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）
    /// </summary>
    public string ReconciliationAccount { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）
    /// </summary>
    public string PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
    /// </summary>
    public int PaymentMethod { get; set; } = 0;

    /// <summary>
    /// 销售地区
    /// </summary>
    public string? SalesDistrict { get; set; } = string.Empty;

    /// <summary>
    /// 销售部门（选项 TaktDepts/options；DictValue=DeptCode）
    /// </summary>
    public string? SalesDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
    /// </summary>
    public string? SalesGroup { get; set; } = string.Empty;

    /// <summary>
    /// 货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）
    /// </summary>
    public string CustomerPricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）
    /// </summary>
    public string ShippingConditions { get; set; } = string.Empty;

    /// <summary>
    /// 交货工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string DeliveringPlant { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
    /// </summary>
    public string Incoterms1 { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条款2（地点说明）
    /// </summary>
    public string Incoterms2 { get; set; } = string.Empty;

    /// <summary>
    /// 帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）
    /// </summary>
    public string AccountAssignmentGroup { get; set; } = string.Empty;

    /// <summary>
    /// 税分类（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? TaxClassification { get; set; } = string.Empty;

    /// <summary>
    /// 客户端联系人
    /// </summary>
    public string? ClientContact { get; set; } = string.Empty;

    /// <summary>
    /// 客户端联系电话
    /// </summary>
    public string? ClientPhone { get; set; } = string.Empty;

    /// <summary>
    /// 客户端联系邮件（邮箱地址短字符串 40 不够）
    /// </summary>
    public string? ClientEmail { get; set; } = string.Empty;

    /// <summary>
    /// 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
    /// </summary>
    public int CreditLevel { get; set; } = 0;

    /// <summary>
    /// 客户端等级（字典 logistics_sales_client_level；0=普通 1=重要 2=VIP 3=战略）
    /// </summary>
    public int ClientLevel { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 客户端状态（字典 sys_normal_disable）
    /// </summary>
    public int ClientStatus { get; set; } = 0;

}

// ========================================
// Client 查询 DTO
// ========================================

/// <summary>
/// Client 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktClientQueryDto : TaktPagedQuery
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
    /// 客户端编码（与租户、公司、销售组织共同唯一；前端表单选择编码规则后自动通过 TaktNumbering 客户端编码规则生成并展示，非手输；单据类型菜单：顾客信息）
    /// </summary>
    public string? ClientCode { get; set; } = string.Empty;

    /// <summary>
    /// 科目组
    /// </summary>
    public string? AccountGroup { get; set; } = string.Empty;

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
    /// 称谓（字典 logistics_sales_client_title；1=公司 2=先生 3=夫人 4=先生和夫人）
    /// </summary>
    public int? ClientTitle { get; set; }

    /// <summary>
    /// 名称1
    /// </summary>
    public string? ClientName1 { get; set; } = string.Empty;

    /// <summary>
    /// 名称2
    /// </summary>
    public string? ClientName2 { get; set; } = string.Empty;

    /// <summary>
    /// 名称3
    /// </summary>
    public string? ClientName3 { get; set; } = string.Empty;

    /// <summary>
    /// 名称4
    /// </summary>
    public string? ClientName4 { get; set; } = string.Empty;

    /// <summary>
    /// 简称
    /// </summary>
    public string? ClientShortName { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? RegistrationCountry { get; set; } = string.Empty;

    /// <summary>
    /// 注册地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? RegistrationRegion { get; set; } = string.Empty;

    /// <summary>
    /// 注册城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? RegistrationCity { get; set; } = string.Empty;

    /// <summary>
    /// 地址1（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr1 { get; set; } = string.Empty;

    /// <summary>
    /// 地址2（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr2 { get; set; } = string.Empty;

    /// <summary>
    /// 地址3（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr3 { get; set; } = string.Empty;

    /// <summary>
    /// 地址4（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr4 { get; set; } = string.Empty;

    /// <summary>
    /// 地址5（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr5 { get; set; } = string.Empty;

    /// <summary>
    /// 邮政编码
    /// </summary>
    public string? PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// 电话1
    /// </summary>
    public string? Phone1 { get; set; } = string.Empty;

    /// <summary>
    /// 电话2
    /// </summary>
    public string? Phone2 { get; set; } = string.Empty;

    /// <summary>
    /// 传真
    /// </summary>
    public string? FaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? TradingPartner { get; set; } = string.Empty;

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
    /// 银行国家（字典 sys_country_code；DictValue=ISO alpha-2）
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
    /// 账户持有人
    /// </summary>
    public string? BankAccountHolder { get; set; } = string.Empty;

    /// <summary>
    /// 客户端分类（字典 logistics_sales_client_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
    /// </summary>
    public int? ClientCategory { get; set; }

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    public string? EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    public string? IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）
    /// </summary>
    public string? ReconciliationAccount { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）
    /// </summary>
    public string? PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
    /// </summary>
    public int? PaymentMethod { get; set; }

    /// <summary>
    /// 销售地区
    /// </summary>
    public string? SalesDistrict { get; set; } = string.Empty;

    /// <summary>
    /// 销售部门（选项 TaktDepts/options；DictValue=DeptCode）
    /// </summary>
    public string? SalesDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
    /// </summary>
    public string? SalesGroup { get; set; } = string.Empty;

    /// <summary>
    /// 货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）
    /// </summary>
    public string? CustomerPricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）
    /// </summary>
    public string? ShippingConditions { get; set; } = string.Empty;

    /// <summary>
    /// 交货工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? DeliveringPlant { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
    /// </summary>
    public string? Incoterms1 { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条款2（地点说明）
    /// </summary>
    public string? Incoterms2 { get; set; } = string.Empty;

    /// <summary>
    /// 帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）
    /// </summary>
    public string? AccountAssignmentGroup { get; set; } = string.Empty;

    /// <summary>
    /// 税分类（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? TaxClassification { get; set; } = string.Empty;

    /// <summary>
    /// 客户端联系人
    /// </summary>
    public string? ClientContact { get; set; } = string.Empty;

    /// <summary>
    /// 客户端联系电话
    /// </summary>
    public string? ClientPhone { get; set; } = string.Empty;

    /// <summary>
    /// 客户端联系邮件（邮箱地址短字符串 40 不够）
    /// </summary>
    public string? ClientEmail { get; set; } = string.Empty;

    /// <summary>
    /// 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
    /// </summary>
    public int? CreditLevel { get; set; }

    /// <summary>
    /// 客户端等级（字典 logistics_sales_client_level；0=普通 1=重要 2=VIP 3=战略）
    /// </summary>
    public int? ClientLevel { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 客户端状态（字典 sys_normal_disable）
    /// </summary>
    public int? ClientStatus { get; set; }

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
// 创建Client DTO
// ========================================

/// <summary>
/// 创建Client DTO
/// </summary>
public class TaktClientCreateDto
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
    /// 客户端编码（与租户、公司、销售组织共同唯一；前端表单选择编码规则后自动通过 TaktNumbering 客户端编码规则生成并展示，非手输；单据类型菜单：顾客信息）
    /// </summary>
    [Required(ErrorMessage = "客户端编码（与租户、公司、销售组织共同唯一；前端表单选择编码规则后自动通过 TaktNumbering 客户端编码规则生成并展示，非手输；单据类型菜单：顾客信息）不能为空")]
    public string ClientCode { get; set; } = string.Empty;

    /// <summary>
    /// 编码规则编码（前端表单从 TaktNumberings/options 选择；对应 TaktNumbering.RuleCode；不落库）
    /// </summary>
    public string? NumberingRuleCode { get; set; }

    /// <summary>
    /// 科目组
    /// </summary>
    [Required(ErrorMessage = "科目组不能为空")]
    public string AccountGroup { get; set; } = string.Empty;

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
    /// 称谓（字典 logistics_sales_client_title；1=公司 2=先生 3=夫人 4=先生和夫人）
    /// </summary>
    public int ClientTitle { get; set; } = 0;

    /// <summary>
    /// 名称1
    /// </summary>
    [Required(ErrorMessage = "名称1不能为空")]
    public string ClientName1 { get; set; } = string.Empty;

    /// <summary>
    /// 名称2
    /// </summary>
    public string? ClientName2 { get; set; } = string.Empty;

    /// <summary>
    /// 名称3
    /// </summary>
    public string? ClientName3 { get; set; } = string.Empty;

    /// <summary>
    /// 名称4
    /// </summary>
    public string? ClientName4 { get; set; } = string.Empty;

    /// <summary>
    /// 简称
    /// </summary>
    public string? ClientShortName { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? RegistrationCountry { get; set; } = string.Empty;

    /// <summary>
    /// 注册地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? RegistrationRegion { get; set; } = string.Empty;

    /// <summary>
    /// 注册城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? RegistrationCity { get; set; } = string.Empty;

    /// <summary>
    /// 地址1（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr1 { get; set; } = string.Empty;

    /// <summary>
    /// 地址2（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr2 { get; set; } = string.Empty;

    /// <summary>
    /// 地址3（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr3 { get; set; } = string.Empty;

    /// <summary>
    /// 地址4（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr4 { get; set; } = string.Empty;

    /// <summary>
    /// 地址5（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr5 { get; set; } = string.Empty;

    /// <summary>
    /// 邮政编码
    /// </summary>
    public string? PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// 电话1
    /// </summary>
    public string? Phone1 { get; set; } = string.Empty;

    /// <summary>
    /// 电话2
    /// </summary>
    public string? Phone2 { get; set; } = string.Empty;

    /// <summary>
    /// 传真
    /// </summary>
    public string? FaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    [Required(ErrorMessage = "供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）不能为空")]
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）不能为空")]
    public string TradingPartner { get; set; } = string.Empty;

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
    /// 银行国家（字典 sys_country_code；DictValue=ISO alpha-2）
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
    /// 账户持有人
    /// </summary>
    public string? BankAccountHolder { get; set; } = string.Empty;

    /// <summary>
    /// 客户端分类（字典 logistics_sales_client_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
    /// </summary>
    public int ClientCategory { get; set; } = 0;

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
    /// 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）
    /// </summary>
    [Required(ErrorMessage = "统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）不能为空")]
    public string ReconciliationAccount { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）
    /// </summary>
    [Required(ErrorMessage = "付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）不能为空")]
    public string PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
    /// </summary>
    public int PaymentMethod { get; set; } = 0;

    /// <summary>
    /// 销售地区
    /// </summary>
    public string? SalesDistrict { get; set; } = string.Empty;

    /// <summary>
    /// 销售部门（选项 TaktDepts/options；DictValue=DeptCode）
    /// </summary>
    public string? SalesDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
    /// </summary>
    public string? SalesGroup { get; set; } = string.Empty;

    /// <summary>
    /// 货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    [Required(ErrorMessage = "货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）不能为空")]
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）
    /// </summary>
    [Required(ErrorMessage = "客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）不能为空")]
    public string CustomerPricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）
    /// </summary>
    [Required(ErrorMessage = "装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）不能为空")]
    public string ShippingConditions { get; set; } = string.Empty;

    /// <summary>
    /// 交货工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "交货工厂（选项 TaktPlants/options；DictValue=PlantCode）不能为空")]
    public string DeliveringPlant { get; set; } = string.Empty;

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
    /// 帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）
    /// </summary>
    [Required(ErrorMessage = "帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）不能为空")]
    public string AccountAssignmentGroup { get; set; } = string.Empty;

    /// <summary>
    /// 税分类（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? TaxClassification { get; set; } = string.Empty;

    /// <summary>
    /// 客户端联系人
    /// </summary>
    public string? ClientContact { get; set; } = string.Empty;

    /// <summary>
    /// 客户端联系电话
    /// </summary>
    public string? ClientPhone { get; set; } = string.Empty;

    /// <summary>
    /// 客户端联系邮件（邮箱地址短字符串 40 不够）
    /// </summary>
    public string? ClientEmail { get; set; } = string.Empty;

    /// <summary>
    /// 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
    /// </summary>
    public int CreditLevel { get; set; } = 0;

    /// <summary>
    /// 客户端等级（字典 logistics_sales_client_level；0=普通 1=重要 2=VIP 3=战略）
    /// </summary>
    public int ClientLevel { get; set; } = 0;

    /// <summary>
    /// 客户端状态（字典 sys_normal_disable）
    /// </summary>
    public int ClientStatus { get; set; } = 0;

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
// 更新Client DTO
// ========================================

/// <summary>
/// 更新Client DTO
/// 继承 TaktClientCreateDto，添加 ClientId 字段
/// </summary>
public class TaktClientUpdateDto : TaktClientCreateDto
{
    /// <summary>
    /// ClientID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ClientId { get; set; }

}

// ========================================
// Client 状态 DTO
// ========================================

/// <summary>
/// Client 状态更新 DTO
/// </summary>
public class TaktClientStatusDto
{
    /// <summary>
    /// ClientID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ClientId { get; set; }

    /// <summary>
    /// 客户端状态（字典 sys_normal_disable）
    /// </summary>
    [Required(ErrorMessage = "客户端状态（字典 sys_normal_disable）不能为空")]
    public int ClientStatus { get; set; } = 0;
}

// ========================================
// Client 排序 DTO
// ========================================

/// <summary>
/// Client 排序更新 DTO
/// </summary>
public class TaktClientSortDto
{
    /// <summary>
    /// ClientID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ClientId { get; set; }

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
/// Client 导入模板行 DTO
/// </summary>
public class TaktClientTemplateDto
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
    /// 客户端编码（与租户、公司、销售组织共同唯一；前端表单选择编码规则后自动通过 TaktNumbering 客户端编码规则生成并展示，非手输；单据类型菜单：顾客信息）
    /// </summary>
    public string? ClientCode { get; set; } = string.Empty;

    /// <summary>
    /// 科目组
    /// </summary>
    public string? AccountGroup { get; set; } = string.Empty;

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
    /// 称谓（字典 logistics_sales_client_title；1=公司 2=先生 3=夫人 4=先生和夫人）
    /// </summary>
    public int? ClientTitle { get; set; }

    /// <summary>
    /// 名称1
    /// </summary>
    public string? ClientName1 { get; set; } = string.Empty;

    /// <summary>
    /// 名称2
    /// </summary>
    public string? ClientName2 { get; set; } = string.Empty;

    /// <summary>
    /// 名称3
    /// </summary>
    public string? ClientName3 { get; set; } = string.Empty;

    /// <summary>
    /// 名称4
    /// </summary>
    public string? ClientName4 { get; set; } = string.Empty;

    /// <summary>
    /// 简称
    /// </summary>
    public string? ClientShortName { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? RegistrationCountry { get; set; } = string.Empty;

    /// <summary>
    /// 注册地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? RegistrationRegion { get; set; } = string.Empty;

    /// <summary>
    /// 注册城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? RegistrationCity { get; set; } = string.Empty;

    /// <summary>
    /// 地址1（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr1 { get; set; } = string.Empty;

    /// <summary>
    /// 地址2（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr2 { get; set; } = string.Empty;

    /// <summary>
    /// 地址3（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr3 { get; set; } = string.Empty;

    /// <summary>
    /// 地址4（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr4 { get; set; } = string.Empty;

    /// <summary>
    /// 地址5（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr5 { get; set; } = string.Empty;

    /// <summary>
    /// 邮政编码
    /// </summary>
    public string? PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// 电话1
    /// </summary>
    public string? Phone1 { get; set; } = string.Empty;

    /// <summary>
    /// 电话2
    /// </summary>
    public string? Phone2 { get; set; } = string.Empty;

    /// <summary>
    /// 传真
    /// </summary>
    public string? FaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? TradingPartner { get; set; } = string.Empty;

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
    /// 银行国家（字典 sys_country_code；DictValue=ISO alpha-2）
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
    /// 账户持有人
    /// </summary>
    public string? BankAccountHolder { get; set; } = string.Empty;

    /// <summary>
    /// 客户端分类（字典 logistics_sales_client_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
    /// </summary>
    public int? ClientCategory { get; set; }

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    public string? EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    public string? IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）
    /// </summary>
    public string? ReconciliationAccount { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）
    /// </summary>
    public string? PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
    /// </summary>
    public int? PaymentMethod { get; set; }

    /// <summary>
    /// 销售地区
    /// </summary>
    public string? SalesDistrict { get; set; } = string.Empty;

    /// <summary>
    /// 销售部门（选项 TaktDepts/options；DictValue=DeptCode）
    /// </summary>
    public string? SalesDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
    /// </summary>
    public string? SalesGroup { get; set; } = string.Empty;

    /// <summary>
    /// 货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）
    /// </summary>
    public string? CustomerPricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）
    /// </summary>
    public string? ShippingConditions { get; set; } = string.Empty;

    /// <summary>
    /// 交货工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? DeliveringPlant { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
    /// </summary>
    public string? Incoterms1 { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条款2（地点说明）
    /// </summary>
    public string? Incoterms2 { get; set; } = string.Empty;

    /// <summary>
    /// 帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）
    /// </summary>
    public string? AccountAssignmentGroup { get; set; } = string.Empty;

    /// <summary>
    /// 税分类（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? TaxClassification { get; set; } = string.Empty;

    /// <summary>
    /// 客户端联系人
    /// </summary>
    public string? ClientContact { get; set; } = string.Empty;

    /// <summary>
    /// 客户端联系电话
    /// </summary>
    public string? ClientPhone { get; set; } = string.Empty;

    /// <summary>
    /// 客户端联系邮件（邮箱地址短字符串 40 不够）
    /// </summary>
    public string? ClientEmail { get; set; } = string.Empty;

    /// <summary>
    /// 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
    /// </summary>
    public int? CreditLevel { get; set; }

    /// <summary>
    /// 客户端等级（字典 logistics_sales_client_level；0=普通 1=重要 2=VIP 3=战略）
    /// </summary>
    public int? ClientLevel { get; set; }

    /// <summary>
    /// 客户端状态（字典 sys_normal_disable）
    /// </summary>
    public int? ClientStatus { get; set; }

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
/// Client 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktClientImportDto
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
    /// 客户端编码（与租户、公司、销售组织共同唯一；前端表单选择编码规则后自动通过 TaktNumbering 客户端编码规则生成并展示，非手输；单据类型菜单：顾客信息）
    /// </summary>
    public string? ClientCode { get; set; } = string.Empty;

    /// <summary>
    /// 科目组
    /// </summary>
    public string? AccountGroup { get; set; } = string.Empty;

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
    /// 称谓（字典 logistics_sales_client_title；1=公司 2=先生 3=夫人 4=先生和夫人）
    /// </summary>
    public int? ClientTitle { get; set; }

    /// <summary>
    /// 名称1
    /// </summary>
    public string? ClientName1 { get; set; } = string.Empty;

    /// <summary>
    /// 名称2
    /// </summary>
    public string? ClientName2 { get; set; } = string.Empty;

    /// <summary>
    /// 名称3
    /// </summary>
    public string? ClientName3 { get; set; } = string.Empty;

    /// <summary>
    /// 名称4
    /// </summary>
    public string? ClientName4 { get; set; } = string.Empty;

    /// <summary>
    /// 简称
    /// </summary>
    public string? ClientShortName { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? RegistrationCountry { get; set; } = string.Empty;

    /// <summary>
    /// 注册地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? RegistrationRegion { get; set; } = string.Empty;

    /// <summary>
    /// 注册城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? RegistrationCity { get; set; } = string.Empty;

    /// <summary>
    /// 地址1（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr1 { get; set; } = string.Empty;

    /// <summary>
    /// 地址2（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr2 { get; set; } = string.Empty;

    /// <summary>
    /// 地址3（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr3 { get; set; } = string.Empty;

    /// <summary>
    /// 地址4（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr4 { get; set; } = string.Empty;

    /// <summary>
    /// 地址5（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr5 { get; set; } = string.Empty;

    /// <summary>
    /// 邮政编码
    /// </summary>
    public string? PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// 电话1
    /// </summary>
    public string? Phone1 { get; set; } = string.Empty;

    /// <summary>
    /// 电话2
    /// </summary>
    public string? Phone2 { get; set; } = string.Empty;

    /// <summary>
    /// 传真
    /// </summary>
    public string? FaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? TradingPartner { get; set; } = string.Empty;

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
    /// 银行国家（字典 sys_country_code；DictValue=ISO alpha-2）
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
    /// 账户持有人
    /// </summary>
    public string? BankAccountHolder { get; set; } = string.Empty;

    /// <summary>
    /// 客户端分类（字典 logistics_sales_client_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
    /// </summary>
    public int? ClientCategory { get; set; }

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    public string? EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    public string? IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）
    /// </summary>
    public string? ReconciliationAccount { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）
    /// </summary>
    public string? PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
    /// </summary>
    public int? PaymentMethod { get; set; }

    /// <summary>
    /// 销售地区
    /// </summary>
    public string? SalesDistrict { get; set; } = string.Empty;

    /// <summary>
    /// 销售部门（选项 TaktDepts/options；DictValue=DeptCode）
    /// </summary>
    public string? SalesDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
    /// </summary>
    public string? SalesGroup { get; set; } = string.Empty;

    /// <summary>
    /// 货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）
    /// </summary>
    public string? CustomerPricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）
    /// </summary>
    public string? ShippingConditions { get; set; } = string.Empty;

    /// <summary>
    /// 交货工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? DeliveringPlant { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
    /// </summary>
    public string? Incoterms1 { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条款2（地点说明）
    /// </summary>
    public string? Incoterms2 { get; set; } = string.Empty;

    /// <summary>
    /// 帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）
    /// </summary>
    public string? AccountAssignmentGroup { get; set; } = string.Empty;

    /// <summary>
    /// 税分类（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? TaxClassification { get; set; } = string.Empty;

    /// <summary>
    /// 客户端联系人
    /// </summary>
    public string? ClientContact { get; set; } = string.Empty;

    /// <summary>
    /// 客户端联系电话
    /// </summary>
    public string? ClientPhone { get; set; } = string.Empty;

    /// <summary>
    /// 客户端联系邮件（邮箱地址短字符串 40 不够）
    /// </summary>
    public string? ClientEmail { get; set; } = string.Empty;

    /// <summary>
    /// 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
    /// </summary>
    public int? CreditLevel { get; set; }

    /// <summary>
    /// 客户端等级（字典 logistics_sales_client_level；0=普通 1=重要 2=VIP 3=战略）
    /// </summary>
    public int? ClientLevel { get; set; }

    /// <summary>
    /// 客户端状态（字典 sys_normal_disable）
    /// </summary>
    public int? ClientStatus { get; set; }

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
/// Client 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktClientExportDto
{
    /// <summary>
    /// ClientID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ClientId { get; set; }

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
    /// 客户端编码（与租户、公司、销售组织共同唯一；前端表单选择编码规则后自动通过 TaktNumbering 客户端编码规则生成并展示，非手输；单据类型菜单：顾客信息）
    /// </summary>
    public string ClientCode { get; set; } = string.Empty;

    /// <summary>
    /// 科目组
    /// </summary>
    public string AccountGroup { get; set; } = string.Empty;

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
    /// 称谓（字典 logistics_sales_client_title；1=公司 2=先生 3=夫人 4=先生和夫人）
    /// </summary>
    public int ClientTitle { get; set; } = 0;

    /// <summary>
    /// 名称1
    /// </summary>
    public string ClientName1 { get; set; } = string.Empty;

    /// <summary>
    /// 名称2
    /// </summary>
    public string? ClientName2 { get; set; } = string.Empty;

    /// <summary>
    /// 名称3
    /// </summary>
    public string? ClientName3 { get; set; } = string.Empty;

    /// <summary>
    /// 名称4
    /// </summary>
    public string? ClientName4 { get; set; } = string.Empty;

    /// <summary>
    /// 简称
    /// </summary>
    public string? ClientShortName { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? RegistrationCountry { get; set; } = string.Empty;

    /// <summary>
    /// 注册地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? RegistrationRegion { get; set; } = string.Empty;

    /// <summary>
    /// 注册城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? RegistrationCity { get; set; } = string.Empty;

    /// <summary>
    /// 地址1（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr1 { get; set; } = string.Empty;

    /// <summary>
    /// 地址2（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr2 { get; set; } = string.Empty;

    /// <summary>
    /// 地址3（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr3 { get; set; } = string.Empty;

    /// <summary>
    /// 地址4（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr4 { get; set; } = string.Empty;

    /// <summary>
    /// 地址5（含门牌，短字符串 40 不够）
    /// </summary>
    public string? Addr5 { get; set; } = string.Empty;

    /// <summary>
    /// 邮政编码
    /// </summary>
    public string? PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// 电话1
    /// </summary>
    public string? Phone1 { get; set; } = string.Empty;

    /// <summary>
    /// 电话2
    /// </summary>
    public string? Phone2 { get; set; } = string.Empty;

    /// <summary>
    /// 传真
    /// </summary>
    public string? FaxNumber { get; set; } = string.Empty;

    /// <summary>
    /// 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string TradingPartner { get; set; } = string.Empty;

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
    /// 银行国家（字典 sys_country_code；DictValue=ISO alpha-2）
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
    /// 账户持有人
    /// </summary>
    public string? BankAccountHolder { get; set; } = string.Empty;

    /// <summary>
    /// 客户端分类（字典 logistics_sales_client_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
    /// </summary>
    public int ClientCategory { get; set; } = 0;

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    public string EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    public string IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）
    /// </summary>
    public string ReconciliationAccount { get; set; } = string.Empty;

    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）
    /// </summary>
    public string PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// 付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
    /// </summary>
    public int PaymentMethod { get; set; } = 0;

    /// <summary>
    /// 销售地区
    /// </summary>
    public string? SalesDistrict { get; set; } = string.Empty;

    /// <summary>
    /// 销售部门（选项 TaktDepts/options；DictValue=DeptCode）
    /// </summary>
    public string? SalesDepartment { get; set; } = string.Empty;

    /// <summary>
    /// 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
    /// </summary>
    public string? SalesGroup { get; set; } = string.Empty;

    /// <summary>
    /// 货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）
    /// </summary>
    public string CustomerPricingProcedure { get; set; } = string.Empty;

    /// <summary>
    /// 装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）
    /// </summary>
    public string ShippingConditions { get; set; } = string.Empty;

    /// <summary>
    /// 交货工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string DeliveringPlant { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
    /// </summary>
    public string Incoterms1 { get; set; } = string.Empty;

    /// <summary>
    /// 国际贸易条款2（地点说明）
    /// </summary>
    public string Incoterms2 { get; set; } = string.Empty;

    /// <summary>
    /// 帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）
    /// </summary>
    public string AccountAssignmentGroup { get; set; } = string.Empty;

    /// <summary>
    /// 税分类（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    public string? TaxClassification { get; set; } = string.Empty;

    /// <summary>
    /// 客户端联系人
    /// </summary>
    public string? ClientContact { get; set; } = string.Empty;

    /// <summary>
    /// 客户端联系电话
    /// </summary>
    public string? ClientPhone { get; set; } = string.Empty;

    /// <summary>
    /// 客户端联系邮件（邮箱地址短字符串 40 不够）
    /// </summary>
    public string? ClientEmail { get; set; } = string.Empty;

    /// <summary>
    /// 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
    /// </summary>
    public int CreditLevel { get; set; } = 0;

    /// <summary>
    /// 客户端等级（字典 logistics_sales_client_level；0=普通 1=重要 2=VIP 3=战略）
    /// </summary>
    public int ClientLevel { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 客户端状态（字典 sys_normal_disable）
    /// </summary>
    public int ClientStatus { get; set; } = 0;

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

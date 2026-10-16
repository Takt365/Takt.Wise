// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Sales
// 文件名称：TaktCustomer.cs
// 创建时间：2026-05-12
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt客户信息实体，定义客户领域模型
// 
// 业务语义说明：
// Customer 适用于核心交付物为产品、货物或可明确交易的标的的场景
// （如：商品购买客户、设备采购客户、物料采购客户等）
// Client 适用于核心交付物为服务、项目交付、长期合作或专业服务的场景
// （如：咨询公司客户、SaaS订阅客户、外包服务客户等）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Sales;

/// <summary>
/// Takt客户信息实体
/// <para>业务唯一键：TenantCode+CompanyCode+SalesOrganization+CustomerCode。</para>
/// </summary>
[SugarTable("takt_logistics_sales_customer", "客户信息表")]
[SugarIndex("ix_customer_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_customer_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_customer_customer_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SalesOrganization), OrderByType.Asc, nameof(CustomerCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_sales_customer_customer_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CustomerStatus), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_customer_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktCustomer : TaktCompanyEntityBase
{
    /// <summary>
    /// 客户编码（与租户、公司、销售组织共同唯一；前端表单选择编码规则后自动通过 TaktNumbering 客户编码规则生成并展示，非手输；单据类型菜单：客户信息）
    /// </summary>
    [SugarColumn(ColumnName = "customer_code", ColumnDescription = "客户编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string CustomerCode { get; set; } = string.Empty;
    /// <summary>
    /// 科目组
    /// </summary>
    [SugarColumn(ColumnName = "account_group", ColumnDescription = "科目组", ColumnDataType = "varchar", Length = 4, IsNullable = false, DefaultValue = "")]
    public string AccountGroup { get; set; } = string.Empty;
    /// <summary>
    /// 销售组织（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    [SugarColumn(ColumnName = "sales_organization", ColumnDescription = "销售组织", ColumnDataType = "varchar", Length = 4, IsNullable = false, DefaultValue = "")]
    public string SalesOrganization { get; set; } = string.Empty;
    /// <summary>
    /// 分销渠道
    /// </summary>
    [SugarColumn(ColumnName = "distribution_channel", ColumnDescription = "分销渠道", ColumnDataType = "varchar", Length = 2, IsNullable = false, DefaultValue = "")]
    public string DistributionChannel { get; set; } = string.Empty;
    /// <summary>
    /// 产品组
    /// </summary>
    [SugarColumn(ColumnName = "product_group", ColumnDescription = "产品组", ColumnDataType = "varchar", Length = 2, IsNullable = false, DefaultValue = "")]
    public string ProductGroup { get; set; } = string.Empty;
    /// <summary>
    /// 称谓（字典 logistics_sales_customer_title；1=公司 2=先生 3=夫人 4=先生和夫人）
    /// </summary>
    [SugarColumn(ColumnName = "customer_title", ColumnDescription = "称谓", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int CustomerTitle { get; set; } = 1;
    /// <summary>
    /// 名称1
    /// </summary>
    [SugarColumn(ColumnName = "customer_name1", ColumnDescription = "名称1", ColumnDataType = "nvarchar", Length = 140, IsNullable = false)]
    public string CustomerName1 { get; set; } = string.Empty;
    /// <summary>
    /// 名称2
    /// </summary>
    [SugarColumn(ColumnName = "customer_name2", ColumnDescription = "名称2", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? CustomerName2 { get; set; }
    /// <summary>
    /// 名称3
    /// </summary>
    [SugarColumn(ColumnName = "customer_name3", ColumnDescription = "名称3", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? CustomerName3 { get; set; }
    /// <summary>
    /// 名称4
    /// </summary>
    [SugarColumn(ColumnName = "customer_name4", ColumnDescription = "名称4", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? CustomerName4 { get; set; }
    /// <summary>
    /// 简称
    /// </summary>
    [SugarColumn(ColumnName = "customer_short_name", ColumnDescription = "简称", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? CustomerShortName { get; set; }
    /// <summary>
    /// 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    [SugarColumn(ColumnName = "registration_country", ColumnDescription = "注册国家", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? RegistrationCountry { get; set; }
    /// <summary>
    /// 注册地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    [SugarColumn(ColumnName = "registration_region", ColumnDescription = "注册地区", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? RegistrationRegion { get; set; }
    /// <summary>
    /// 注册城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    [SugarColumn(ColumnName = "registration_city", ColumnDescription = "注册城市", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? RegistrationCity { get; set; }
    /// <summary>
    /// 地址1（含门牌，短字符串 40 不够）
    /// </summary>
    [SugarColumn(ColumnName = "addr1", ColumnDescription = "地址1", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? Addr1 { get; set; }
    /// <summary>
    /// 地址2（含门牌，短字符串 40 不够）
    /// </summary>
    [SugarColumn(ColumnName = "addr2", ColumnDescription = "地址2", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? Addr2 { get; set; }
    /// <summary>
    /// 地址3（含门牌，短字符串 40 不够）
    /// </summary>
    [SugarColumn(ColumnName = "addr3", ColumnDescription = "地址3", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? Addr3 { get; set; }
    /// <summary>
    /// 地址4（含门牌，短字符串 40 不够）
    /// </summary>
    [SugarColumn(ColumnName = "addr4", ColumnDescription = "地址4", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? Addr4 { get; set; }
    /// <summary>
    /// 地址5（含门牌，短字符串 40 不够）
    /// </summary>
    [SugarColumn(ColumnName = "addr5", ColumnDescription = "地址5", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? Addr5 { get; set; }
    /// <summary>
    /// 邮政编码
    /// </summary>
    [SugarColumn(ColumnName = "postal_code", ColumnDescription = "邮政编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? PostalCode { get; set; }
    /// <summary>
    /// 电话1
    /// </summary>
    [SugarColumn(ColumnName = "phone1", ColumnDescription = "电话1", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? Phone1 { get; set; }
    /// <summary>
    /// 电话2
    /// </summary>
    [SugarColumn(ColumnName = "phone2", ColumnDescription = "电话2", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? Phone2 { get; set; }
    /// <summary>
    /// 传真
    /// </summary>
    [SugarColumn(ColumnName = "fax_number", ColumnDescription = "传真", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? FaxNumber { get; set; }
    /// <summary>
    /// 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_code", ColumnDescription = "供应商", ColumnDataType = "nvarchar", Length = 10, IsNullable = false, DefaultValue = "")]
    public string SupplierCode { get; set; } = string.Empty;
    /// <summary>
    /// 贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "trading_partner", ColumnDescription = "贸易伙伴", ColumnDataType = "varchar", Length = 4, IsNullable = false, DefaultValue = "")]
    public string TradingPartner { get; set; } = string.Empty;
    /// <summary>
    /// 税号1
    /// </summary>
    [SugarColumn(ColumnName = "tax_number1", ColumnDescription = "税号1", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? TaxNumber1 { get; set; }
    /// <summary>
    /// 税号2
    /// </summary>
    [SugarColumn(ColumnName = "tax_number2", ColumnDescription = "税号2", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? TaxNumber2 { get; set; }
    /// <summary>
    /// 税号3（来源主数据最长 60）
    /// </summary>
    [SugarColumn(ColumnName = "tax_number3", ColumnDescription = "税号3", ColumnDataType = "nvarchar", Length = 60, IsNullable = true)]
    public string? TaxNumber3 { get; set; }
    /// <summary>
    /// 银行国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    [SugarColumn(ColumnName = "bank_country_code", ColumnDescription = "银行国家", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? BankCountryCode { get; set; }
    /// <summary>
    /// 银行代码（选项 TaktBanks/options；DictValue=BankCode）
    /// </summary>
    [SugarColumn(ColumnName = "bank_code", ColumnDescription = "银行代码", ColumnDataType = "nvarchar", Length = 15, IsNullable = true)]
    public string? BankCode { get; set; }
    /// <summary>
    /// 银行账户
    /// </summary>
    [SugarColumn(ColumnName = "bank_account", ColumnDescription = "银行账户", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? BankAccount { get; set; }
    /// <summary>
    /// 账户持有人
    /// </summary>
    [SugarColumn(ColumnName = "bank_account_holder", ColumnDescription = "账户持有人", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? BankAccountHolder { get; set; }
    /// <summary>
    /// 客户分类（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
    /// </summary>
    [SugarColumn(ColumnName = "customer_category", ColumnDescription = "客户分类", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CustomerCategory { get; set; } = 0;
    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    [SugarColumn(ColumnName = "enterprise_nature", ColumnDescription = "企业性质", ColumnDataType = "varchar", Length = 4, IsNullable = false, DefaultValue = "150")]
    public string EnterpriseNature { get; set; } = "150";
    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    [SugarColumn(ColumnName = "industry_attribute", ColumnDescription = "行业属性", ColumnDataType = "varchar", Length = 4, IsNullable = false, DefaultValue = "C")]
    public string IndustryAttribute { get; set; } = "C";
    /// <summary>
    /// 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）
    /// </summary>
    [SugarColumn(ColumnName = "reconciliation_account", ColumnDescription = "统驭科目", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = "")]
    public string ReconciliationAccount { get; set; } = string.Empty;
    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）
    /// </summary>
    [SugarColumn(ColumnName = "payment_terms", ColumnDescription = "付款条件", ColumnDataType = "nvarchar", Length = 40, IsNullable = false, DefaultValue = "PREPAYSHIP")]
    public string PaymentTerms { get; set; } = "PREPAYSHIP";
    /// <summary>
    /// 付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
    /// </summary>
    [SugarColumn(ColumnName = "payment_method", ColumnDescription = "付款方式", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PaymentMethod { get; set; } = 0;
    /// <summary>
    /// 销售地区
    /// </summary>
    [SugarColumn(ColumnName = "sales_district", ColumnDescription = "销售地区", ColumnDataType = "varchar", Length = 6, IsNullable = true)]
    public string? SalesDistrict { get; set; }
    /// <summary>
    /// 销售部门（选项 TaktDepts/options；DictValue=DeptCode）
    /// </summary>
    [SugarColumn(ColumnName = "sales_department", ColumnDescription = "销售部门", ColumnDataType = "varchar", Length = 6, IsNullable = true)]
    public string? SalesDepartment { get; set; }
    /// <summary>
    /// 销售组（选项 TaktSalesGroups/options；DictValue=SalesGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "sales_group", ColumnDescription = "销售组", ColumnDataType = "varchar", Length = 6, IsNullable = true)]
    public string? SalesGroup { get; set; }
    /// <summary>
    /// 货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    [SugarColumn(ColumnName = "currency_code", ColumnDescription = "货币", ColumnDataType = "nvarchar", Length = 3, IsNullable = true, DefaultValue = "CNY")]
    public string CurrencyCode { get; set; } = "CNY";
    /// <summary>
    /// 客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）
    /// </summary>
    [SugarColumn(ColumnName = "customer_pricing_procedure", ColumnDescription = "客户定价过程", ColumnDataType = "varchar", Length = 2, IsNullable = false, DefaultValue = "1")]
    public string CustomerPricingProcedure { get; set; } = "1";
    /// <summary>
    /// 装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）
    /// </summary>
    [SugarColumn(ColumnName = "shipping_conditions", ColumnDescription = "装运条件", ColumnDataType = "varchar", Length = 2, IsNullable = false, DefaultValue = "")]
    public string ShippingConditions { get; set; } = string.Empty;
    /// <summary>
    /// 交货工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "delivering_plant", ColumnDescription = "交货工厂", ColumnDataType = "varchar", Length = 4, IsNullable = false, DefaultValue = "")]
    public string DeliveringPlant { get; set; } = string.Empty;
    /// <summary>
    /// 国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
    /// </summary>
    [SugarColumn(ColumnName = "incoterms1", ColumnDescription = "国际贸易条款1", ColumnDataType = "varchar", Length = 3, IsNullable = false, DefaultValue = "FOB")]
    public string Incoterms1 { get; set; } = "FOB";
    /// <summary>
    /// 国际贸易条款2（地点说明）
    /// </summary>
    [SugarColumn(ColumnName = "incoterms2", ColumnDescription = "国际贸易条款2", ColumnDataType = "nvarchar", Length = 40, IsNullable = false, DefaultValue = "")]
    public string Incoterms2 { get; set; } = string.Empty;
    /// <summary>
    /// 帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）
    /// </summary>
    [SugarColumn(ColumnName = "account_assignment_group", ColumnDescription = "帐户分配组", ColumnDataType = "varchar", Length = 2, IsNullable = false, DefaultValue = "")]
    public string AccountAssignmentGroup { get; set; } = string.Empty;
    /// <summary>
    /// 税分类（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    [SugarColumn(ColumnName = "tax_classification", ColumnDescription = "税分类", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? TaxClassification { get; set; }
    /// <summary>
    /// 客户联系人
    /// </summary>
    [SugarColumn(ColumnName = "customer_contact", ColumnDescription = "客户联系人", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? CustomerContact { get; set; }
    /// <summary>
    /// 客户联系电话
    /// </summary>
    [SugarColumn(ColumnName = "customer_phone", ColumnDescription = "客户联系电话", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? CustomerPhone { get; set; }
    /// <summary>
    /// 客户联系邮件（邮箱地址短字符串 40 不够）
    /// </summary>
    [SugarColumn(ColumnName = "customer_email", ColumnDescription = "客户联系邮件", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? CustomerEmail { get; set; }
    /// <summary>
    /// 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
    /// </summary>
    [SugarColumn(ColumnName = "credit_level", ColumnDescription = "信用等级", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CreditLevel { get; set; } = 0;
    /// <summary>
    /// 客户等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）
    /// </summary>
    [SugarColumn(ColumnName = "customer_level", ColumnDescription = "客户等级", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CustomerLevel { get; set; } = 0;
    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 客户状态（字典 sys_normal_disable）
    /// </summary>
    [SugarColumn(ColumnName = "customer_status", ColumnDescription = "客户状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int CustomerStatus { get; set; } = 1;
}

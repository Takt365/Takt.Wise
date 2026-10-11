// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Sales
// 文件名称：TaktClient.cs
// 创建时间：2026-05-12
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt客户端信息实体，定义客户端领域模型
// 
// 业务语义说明：
// Client 适用于核心交付物为服务、项目交付、长期合作或专业服务的场景
// （如：咨询公司客户、SaaS订阅客户、外包服务客户等）
// Customer 适用于核心交付物为产品、货物或可明确交易的标的的场景
// （如：商品购买客户、设备采购客户、物料采购客户等）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Sales;

/// <summary>
/// Takt客户端信息实体
/// </summary>
[SugarTable("takt_logistics_sales_client", "客户端信息表")]
[SugarIndex("ix_client_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_client_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_client_client_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(ClientCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_sales_client_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_client_client_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ClientStatus), OrderByType.Asc, false)]
public class TaktClient : TaktCompanyEntityBase
{
    /// <summary>
    /// 客户端编码（唯一索引）
    /// </summary>
    [SugarColumn(ColumnName = "client_code", ColumnDescription = "客户端编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string ClientCode { get; set; } = string.Empty;
    /// <summary>
    /// 客户端名称1
    /// </summary>
    [SugarColumn(ColumnName = "client_name1", ColumnDescription = "客户端名称1", ColumnDataType = "nvarchar", Length = 140, IsNullable = false)]
    public string ClientName1 { get; set; } = string.Empty;
    /// <summary>
    /// 客户端名称2
    /// </summary>
    [SugarColumn(ColumnName = "client_name2", ColumnDescription = "客户端名称2", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? ClientName2 { get; set; }
    /// <summary>
    /// 客户端简称
    /// </summary>
    [SugarColumn(ColumnName = "client_short_name", ColumnDescription = "客户端简称", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? ClientShortName { get; set; }
    /// <summary>
    /// 客户端类型（字典 logistics_sales_client_category；0=终端客户 1=分销商 2=零售商 3=电商平台 4=其他）
    /// </summary>
    [SugarColumn(ColumnName = "client_type", ColumnDescription = "客户端类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ClientType { get; set; } = 0;
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
    /// 客户端标识（税务登记证号/统一社会信用代码）
    /// </summary>
    [SugarColumn(ColumnName = "client_tax_number", ColumnDescription = "客户端标识", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ClientTaxNumber { get; set; }
    /// <summary>
    /// 税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
    /// </summary>
    [SugarColumn(ColumnName = "tax_code", ColumnDescription = "税码", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? TaxCode { get; set; }
    /// <summary>
    /// 税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）
    /// </summary>
    [SugarColumn(ColumnName = "tax_rate", ColumnDescription = "税率", ColumnDataType = "int", IsNullable = false, DefaultValue = "13")]
    public int TaxRate { get; set; } = 13;
    /// <summary>
    /// 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    [SugarColumn(ColumnName = "registration_country", ColumnDescription = "注册国家", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? RegistrationCountry { get; set; }
    /// <summary>
    /// 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    [SugarColumn(ColumnName = "registration_province", ColumnDescription = "注册省", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? RegistrationProvince { get; set; }
    /// <summary>
    /// 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    [SugarColumn(ColumnName = "registration_city", ColumnDescription = "注册市", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? RegistrationCity { get; set; }
    /// <summary>
    /// 注册地址1
    /// </summary>
    [SugarColumn(ColumnName = "registration_address1", ColumnDescription = "注册地址1", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? RegistrationAddress1 { get; set; }
    /// <summary>
    /// 注册地址2
    /// </summary>
    [SugarColumn(ColumnName = "registration_address2", ColumnDescription = "注册地址2", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? RegistrationAddress2 { get; set; }
    /// <summary>
    /// 客户端电话
    /// </summary>
    [SugarColumn(ColumnName = "client_phone", ColumnDescription = "客户端电话", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ClientPhone { get; set; }
    /// <summary>
    /// 客户端传真
    /// </summary>
    [SugarColumn(ColumnName = "client_fax", ColumnDescription = "客户端传真", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ClientFax { get; set; }
    /// <summary>
    /// 客户端邮箱
    /// </summary>
    [SugarColumn(ColumnName = "client_email", ColumnDescription = "客户端邮箱", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? ClientEmail { get; set; }
    /// <summary>
    /// 客户端网站
    /// </summary>
    [SugarColumn(ColumnName = "client_website", ColumnDescription = "客户端网站", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? ClientWebsite { get; set; }
    /// <summary>
    /// 联系人
    /// </summary>
    [SugarColumn(ColumnName = "contact_person", ColumnDescription = "联系人", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ContactPerson { get; set; }
    /// <summary>
    /// 联系人电话
    /// </summary>
    [SugarColumn(ColumnName = "contact_phone", ColumnDescription = "联系人电话", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ContactPhone { get; set; }
    /// <summary>
    /// 联系人邮箱
    /// </summary>
    [SugarColumn(ColumnName = "contact_email", ColumnDescription = "联系人邮箱", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? ContactEmail { get; set; }
    /// <summary>
    /// 结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    [SugarColumn(ColumnName = "currency_code", ColumnDescription = "结算币种代码", ColumnDataType = "nvarchar", Length = 3, IsNullable = true, DefaultValue = "CNY")]
    public string CurrencyCode { get; set; } = "CNY";
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
    /// 客户组（字典 logistics_sales_customer_group；DictValue=Z1～Z4）
    /// </summary>
    [SugarColumn(ColumnName = "customer_group", ColumnDescription = "客户组", ColumnDataType = "varchar", Length = 2, IsNullable = false, DefaultValue = "")]
    public string CustomerGroup { get; set; } = string.Empty;
    /// <summary>
    /// 贸易伙伴（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "trading_partner", ColumnDescription = "贸易伙伴", ColumnDataType = "varchar", Length = 4, IsNullable = false, DefaultValue = "")]
    public string TradingPartner { get; set; } = string.Empty;
    /// <summary>
    /// 帐户分配组（字典 logistics_sales_account_assignment_group；DictValue=01/02/03/Y1～Y4/Z0～ZD）
    /// </summary>
    [SugarColumn(ColumnName = "account_assignment_group", ColumnDescription = "帐户分配组", ColumnDataType = "varchar", Length = 2, IsNullable = false, DefaultValue = "")]
    public string AccountAssignmentGroup { get; set; } = string.Empty;
    /// <summary>
    /// 供应商（选项 TaktSuppliers/options；DictValue=SupplierCode）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_code", ColumnDescription = "供应商", ColumnDataType = "nvarchar", Length = 10, IsNullable = false, DefaultValue = "")]
    public string SupplierCode { get; set; } = string.Empty;
    /// <summary>
    /// 尼尔森标识
    /// </summary>
    [SugarColumn(ColumnName = "nielsen_indicator", ColumnDescription = "尼尔森标识", ColumnDataType = "varchar", Length = 2, IsNullable = false, DefaultValue = "")]
    public string NielsenIndicator { get; set; } = string.Empty;
    /// <summary>
    /// 中心记帐冻结（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "central_posting_block", ColumnDescription = "中心记帐冻结", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CentralPostingBlock { get; set; } = 0;
    /// <summary>
    /// 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=D；DictValue=AccountTitleCode）
    /// </summary>
    [SugarColumn(ColumnName = "reconciliation_account", ColumnDescription = "统驭科目", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = "")]
    public string ReconciliationAccount { get; set; } = string.Empty;
    /// <summary>
    /// 总部（选项 TaktClients/options；DictValue=ClientCode）
    /// </summary>
    [SugarColumn(ColumnName = "headquarters", ColumnDescription = "总部", ColumnDataType = "nvarchar", Length = 20, IsNullable = false, DefaultValue = "")]
    public string Headquarters { get; set; } = string.Empty;
    /// <summary>
    /// 具有供应商的清算（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "clearing_with_vendor", ColumnDescription = "具有供应商的清算", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ClearingWithVendor { get; set; } = 0;
    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
    /// </summary>
    [SugarColumn(ColumnName = "payment_terms", ColumnDescription = "付款条件", ColumnDataType = "nvarchar", Length = 40, IsNullable = false, DefaultValue = "prepayship")]
    public string PaymentTerms { get; set; } = "prepayship";
    /// <summary>
    /// 付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    [SugarColumn(ColumnName = "payment_method", ColumnDescription = "付款方式", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int PaymentMethod { get; set; } = 1;
    /// <summary>
    /// 交货工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "delivering_plant", ColumnDescription = "交货工厂", ColumnDataType = "varchar", Length = 4, IsNullable = false, DefaultValue = "")]
    public string DeliveringPlant { get; set; } = string.Empty;
    /// <summary>
    /// 国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
    /// </summary>
    [SugarColumn(ColumnName = "incoterms1", ColumnDescription = "国际贸易条件1", ColumnDataType = "varchar", Length = 3, IsNullable = false, DefaultValue = "FOB")]
    public string Incoterms1 { get; set; } = "FOB";
    /// <summary>
    /// 国际贸易条件2（地点说明）
    /// </summary>
    [SugarColumn(ColumnName = "incoterms2", ColumnDescription = "国际贸易条件2", ColumnDataType = "nvarchar", Length = 40, IsNullable = false, DefaultValue = "")]
    public string Incoterms2 { get; set; } = string.Empty;
    /// <summary>
    /// 装运条件（字典 logistics_sales_shipping_conditions；DictValue=Z1～Z3）
    /// </summary>
    [SugarColumn(ColumnName = "shipping_conditions", ColumnDescription = "装运条件", ColumnDataType = "varchar", Length = 2, IsNullable = false, DefaultValue = "")]
    public string ShippingConditions { get; set; } = string.Empty;
    /// <summary>
    /// 客户定价过程（字典 logistics_sales_customer_pricing_procedure；DictValue=1/2/3；默认 1）
    /// </summary>
    [SugarColumn(ColumnName = "customer_pricing_procedure", ColumnDescription = "客户定价过程", ColumnDataType = "varchar", Length = 2, IsNullable = false, DefaultValue = "1")]
    public string CustomerPricingProcedure { get; set; } = "1";
    /// <summary>
    /// 销售渠道（字典 logistics_sales_channel；0=直销 1=经销 2=代销 3=电商 4=其他）
    /// </summary>
    [SugarColumn(ColumnName = "sales_channel", ColumnDescription = "销售渠道", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SalesChannel { get; set; } = 0;
    /// <summary>
    /// 平台名称（电商平台名称）
    /// </summary>
    [SugarColumn(ColumnName = "platform_name", ColumnDescription = "平台名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? PlatformName { get; set; }
    /// <summary>
    /// 店铺名称
    /// </summary>
    [SugarColumn(ColumnName = "store_name", ColumnDescription = "店铺名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? StoreName { get; set; }
    /// <summary>
    /// 客户端等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）
    /// </summary>
    [SugarColumn(ColumnName = "client_level", ColumnDescription = "客户端等级", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ClientLevel { get; set; } = 0;
    /// <summary>
    /// 评价分数（0-100分）
    /// </summary>
    [SugarColumn(ColumnName = "evaluation_score", ColumnDescription = "评价分数", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal EvaluationScore { get; set; } = 0;
    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 客户端状态（字典 sys_normal_disable）
    /// </summary>
    [SugarColumn(ColumnName = "client_status", ColumnDescription = "客户端状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ClientStatus { get; set; } = 1;
}

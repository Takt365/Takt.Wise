// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Procurement
// 文件名称：TaktSupplier.cs
// 创建时间：2026-05-12
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt供货商实体，定义供货商领域模型
// 
// 业务语义说明：
// Supplier（供应商）：更常用于"供货方"，强调向你提供物料、产品、零部件的对象
// 典型出现在采购（Procurement）、库存、生产、MM（物料管理）等场景
// 例如：制造商的原材料供应商、贸易公司的货品供应商
// 
// Vendor（卖方/供应商）：含义更广，常作为"交易对方"的统称，既可指供货方，
// 也可涵盖服务提供者、资产出租方等；在ERP/财务系统里，"Vendor Master"用
// 来管理所有对外付款对象（包含Supplier、服务公司、租赁公司等）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Procurement;

/// <summary>
/// Takt供货商实体
/// <para>业务唯一键：TenantCode+CompanyCode+PurchasingOrganization+SupplierCode。</para>
/// </summary>
[SugarTable("takt_logistics_procurement_supplier", "供货商信息表")]
[SugarIndex("ix_supplier_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_supplier_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_procurement_supplier_supplier_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PurchasingOrganization), OrderByType.Asc, nameof(SupplierCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_procurement_supplier_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktSupplier : TaktCompanyEntityBase
{
    /// <summary>
    /// 供应商（与租户、公司、采购组织共同唯一；前端表单选择编码规则后自动通过 TaktNumbering 供应商编码规则生成并展示，非手输；单据类型菜单：供应商）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_code", ColumnDescription = "供应商", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string SupplierCode { get; set; } = string.Empty;
    /// <summary>
    /// 账户组
    /// </summary>
    [SugarColumn(ColumnName = "account_group", ColumnDescription = "账户组", ColumnDataType = "varchar", Length = 4, IsNullable = false, DefaultValue = "")]
    public string AccountGroup { get; set; } = string.Empty;
    /// <summary>
    /// 称谓（字典 logistics_sales_customer_title；1=公司 2=先生 3=夫人 4=先生和夫人）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_title", ColumnDescription = "称谓", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int SupplierTitle { get; set; } = 1;
    /// <summary>
    /// 名称 1
    /// </summary>
    [SugarColumn(ColumnName = "supplier_name1", ColumnDescription = "名称 1", ColumnDataType = "nvarchar", Length = 140, IsNullable = false)]
    public string SupplierName1 { get; set; } = string.Empty;
    /// <summary>
    /// 名称 2
    /// </summary>
    [SugarColumn(ColumnName = "supplier_name2", ColumnDescription = "名称 2", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? SupplierName2 { get; set; }
    /// <summary>
    /// 名称 3
    /// </summary>
    [SugarColumn(ColumnName = "supplier_name3", ColumnDescription = "名称 3", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? SupplierName3 { get; set; }
    /// <summary>
    /// 名称 4
    /// </summary>
    [SugarColumn(ColumnName = "supplier_name4", ColumnDescription = "名称 4", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? SupplierName4 { get; set; }
    /// <summary>
    /// 简称
    /// </summary>
    [SugarColumn(ColumnName = "supplier_short_name", ColumnDescription = "简称", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? SupplierShortName { get; set; }
    /// <summary>
    /// 国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    [SugarColumn(ColumnName = "country_code", ColumnDescription = "国家", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? CountryCode { get; set; }
    /// <summary>
    /// 地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    [SugarColumn(ColumnName = "region_code", ColumnDescription = "地区", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? RegionCode { get; set; }
    /// <summary>
    /// 城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    [SugarColumn(ColumnName = "city_code", ColumnDescription = "城市", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? CityCode { get; set; }
    /// <summary>
    /// 地址1
    /// </summary>
    [SugarColumn(ColumnName = "addr1", ColumnDescription = "地址1", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? Addr1 { get; set; }
    /// <summary>
    /// 地址2（含门牌，短字符串 40 不够）
    /// </summary>
    [SugarColumn(ColumnName = "addr2", ColumnDescription = "地址2", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? Addr2 { get; set; }
    /// <summary>
    /// 邮政编码
    /// </summary>
    [SugarColumn(ColumnName = "postal_code", ColumnDescription = "邮政编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? PostalCode { get; set; }
    /// <summary>
    /// 电话号码1
    /// </summary>
    [SugarColumn(ColumnName = "phone1", ColumnDescription = "电话号码1", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? Phone1 { get; set; }
    /// <summary>
    /// 电话号码2
    /// </summary>
    [SugarColumn(ColumnName = "phone2", ColumnDescription = "电话号码2", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? Phone2 { get; set; }
    /// <summary>
    /// 传真号码
    /// </summary>
    [SugarColumn(ColumnName = "fax_number", ColumnDescription = "传真号码", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? FaxNumber { get; set; }
    /// <summary>
    /// 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    [SugarColumn(ColumnName = "customer_code", ColumnDescription = "客户", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? CustomerCode { get; set; }
    /// <summary>
    /// 贸易伙伴
    /// </summary>
    [SugarColumn(ColumnName = "trading_partner", ColumnDescription = "贸易伙伴", ColumnDataType = "varchar", Length = 6, IsNullable = true)]
    public string? TradingPartner { get; set; }
    /// <summary>
    /// 权限组
    /// </summary>
    [SugarColumn(ColumnName = "authorization_group", ColumnDescription = "权限组", ColumnDataType = "varchar", Length = 4, IsNullable = true)]
    public string? AuthorizationGroup { get; set; }
    /// <summary>
    /// 组代码
    /// </summary>
    [SugarColumn(ColumnName = "group_key", ColumnDescription = "组代码", ColumnDataType = "varchar", Length = 10, IsNullable = true)]
    public string? GroupKey { get; set; }
    /// <summary>
    /// 供应商类型（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_type", ColumnDescription = "供应商类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SupplierType { get; set; } = 0;
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
    /// 信用信息号
    /// </summary>
    [SugarColumn(ColumnName = "credit_info_number", ColumnDescription = "信用信息号", ColumnDataType = "nvarchar", Length = 11, IsNullable = true)]
    public string? CreditInfoNumber { get; set; }
    /// <summary>
    /// 银行国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    [SugarColumn(ColumnName = "bank_country_code", ColumnDescription = "银行国家代码", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
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
    /// 科目持有者
    /// </summary>
    [SugarColumn(ColumnName = "account_holder", ColumnDescription = "科目持有者", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? AccountHolder { get; set; }
    /// <summary>
    /// 指令码
    /// </summary>
    [SugarColumn(ColumnName = "bank_charge_bearer_code", ColumnDescription = "指令码", ColumnDataType = "varchar", Length = 2, IsNullable = true)]
    public string? BankChargeBearerCode { get; set; }
    /// <summary>
    /// 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）
    /// </summary>
    [SugarColumn(ColumnName = "reconciliation_account", ColumnDescription = "统驭科目", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = "")]
    public string ReconciliationAccount { get; set; } = string.Empty;
    /// <summary>
    /// 总部
    /// </summary>
    [SugarColumn(ColumnName = "head_office_account_code", ColumnDescription = "总部", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? HeadOfficeAccountCode { get; set; }
    /// <summary>
    /// 计划组
    /// </summary>
    [SugarColumn(ColumnName = "planning_group", ColumnDescription = "计划组", ColumnDataType = "varchar", Length = 10, IsNullable = true)]
    public string? PlanningGroup { get; set; }
    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）
    /// </summary>
    [SugarColumn(ColumnName = "payment_terms", ColumnDescription = "付款条件", ColumnDataType = "nvarchar", Length = 40, IsNullable = false, DefaultValue = "COD")]
    public string PaymentTerms { get; set; } = "COD";
    /// <summary>
    /// 检查双重发票（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "double_invoice_check", ColumnDescription = "检查双重发票", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int DoubleInvoiceCheck { get; set; } = 1;
    /// <summary>
    /// 付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
    /// </summary>
    [SugarColumn(ColumnName = "payment_method", ColumnDescription = "付款方式", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PaymentMethod { get; set; } = 0;
    /// <summary>
    /// 付款冻结（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "payment_block", ColumnDescription = "付款冻结", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PaymentBlock { get; set; } = 0;
    /// <summary>
    /// 开户行（选项 TaktBanks/options；DictValue=BankCode）
    /// </summary>
    [SugarColumn(ColumnName = "house_bank", ColumnDescription = "开户行", ColumnDataType = "nvarchar", Length = 15, IsNullable = true)]
    public string? HouseBank { get; set; }
    /// <summary>
    /// 在客户和供应商之间清算（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "clearing_with_customer", ColumnDescription = "客商对冲", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ClearingWithCustomer { get; set; } = 0;
    /// <summary>
    /// 会计职员
    /// </summary>
    [SugarColumn(ColumnName = "accounting_clerk", ColumnDescription = "会计职员", ColumnDataType = "varchar", Length = 2, IsNullable = true)]
    public string? AccountingClerk { get; set; }
    /// <summary>
    /// 我们带有供应商的帐目号
    /// </summary>
    [SugarColumn(ColumnName = "our_account_at_supplier", ColumnDescription = "社内供应商", ColumnDataType = "nvarchar", Length = 12, IsNullable = true)]
    public string? OurAccountAtSupplier { get; set; }
    /// <summary>
    /// 供应商联系人
    /// </summary>
    [SugarColumn(ColumnName = "contact_person", ColumnDescription = "供应商联系人", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? ContactPerson { get; set; }
    /// <summary>
    /// 供应商联系电话
    /// </summary>
    [SugarColumn(ColumnName = "contact_phone", ColumnDescription = "供应商联系电话", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? ContactPhone { get; set; }
    /// <summary>
    /// 供应商联系邮件（邮箱地址短字符串 40 不够）
    /// </summary>
    [SugarColumn(ColumnName = "contact_email", ColumnDescription = "供应商联系邮件", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? ContactEmail { get; set; }
    /// <summary>
    /// 代扣所得税国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    [SugarColumn(ColumnName = "withholding_tax_country", ColumnDescription = "代扣所得税国家", ColumnDataType = "nvarchar", Length = 2, IsNullable = true)]
    public string? WithholdingTaxCountry { get; set; }
    /// <summary>
    /// 预扣税类型（选项 TaktTaxCodes/options；DictValue=TaxCode；选中后回填预扣税代码）
    /// </summary>
    [SugarColumn(ColumnName = "withholding_tax_type", ColumnDescription = "预扣税类型", ColumnDataType = "varchar", Length = 2, IsNullable = true)]
    public string? WithholdingTaxType { get; set; }
    /// <summary>
    /// 预扣税代码（由预扣税类型 WithholdingTaxType 回填；选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    [SugarColumn(ColumnName = "withholding_tax_code", ColumnDescription = "预扣税代码", ColumnDataType = "varchar", Length = 2, IsNullable = true)]
    public string? WithholdingTaxCode { get; set; }
    /// <summary>
    /// 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "purchasing_organization", ColumnDescription = "采购组织", ColumnDataType = "varchar", Length = 4, IsNullable = false, DefaultValue = "")]
    public string PurchasingOrganization { get; set; } = string.Empty;
    /// <summary>
    /// 订单货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    [SugarColumn(ColumnName = "order_currency_code", ColumnDescription = "订单货币", ColumnDataType = "nvarchar", Length = 3, IsNullable = true, DefaultValue = "CNY")]
    public string OrderCurrencyCode { get; set; } = "CNY";
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
    /// 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
    /// </summary>
    [SugarColumn(ColumnName = "pricing_date_control", ColumnDescription = "定价日期控制", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int PricingDateControl { get; set; } = 1;
    /// <summary>
    /// 基于收货的发票校验（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "gr_based_invoice_inspection", ColumnDescription = "基于收货的发票校验", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int GrBasedInvoiceInspection { get; set; } = 0;
    /// <summary>
    /// 自动评估GRR结算交货（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "evaluated_receipt_settlement", ColumnDescription = "自动评估GRR结算交货", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int EvaluatedReceiptSettlement { get; set; } = 0;
    /// <summary>
    /// 自动评估GR结算保留（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "return_ers_flag", ColumnDescription = "自动评估GR结算保留", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ReturnErsFlag { get; set; } = 0;
    /// <summary>
    /// 自动建立采购订单（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "automatic_purchase_order", ColumnDescription = "自动建立采购订单", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int AutomaticPurchaseOrder { get; set; } = 0;
    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_group", ColumnDescription = "采购组", ColumnDataType = "varchar", Length = 6, IsNullable = false, DefaultValue = "")]
    public string PurchaseGroup { get; set; } = string.Empty;
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
    /// 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
    /// </summary>
    [SugarColumn(ColumnName = "credit_level", ColumnDescription = "信用等级", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CreditLevel { get; set; } = 0;
    /// <summary>
    /// 供应商等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_level", ColumnDescription = "供应商等级", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SupplierLevel { get; set; } = 0;
    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 供货商状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_status", ColumnDescription = "供货商状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int SupplierStatus { get; set; } = 1;
}

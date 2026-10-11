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
/// </summary>
[SugarTable("takt_logistics_procurement_supplier", "供货商信息表")]
[SugarIndex("ix_supplier_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_supplier_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_procurement_supplier_supplier_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SupplierCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_procurement_supplier_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktSupplier : TaktCompanyEntityBase
{
    /// <summary>
    /// 供货商编码（唯一索引）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_code", ColumnDescription = "供货商编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string SupplierCode { get; set; } = string.Empty;
    /// <summary>
    /// 供货商名称1
    /// </summary>
    [SugarColumn(ColumnName = "supplier_name1", ColumnDescription = "供货商名称1", ColumnDataType = "nvarchar", Length = 140, IsNullable = false)]
    public string SupplierName1 { get; set; } = string.Empty;
    /// <summary>
    /// 供货商名称2
    /// </summary>
    [SugarColumn(ColumnName = "supplier_name2", ColumnDescription = "供货商名称2", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? SupplierName2 { get; set; }
    /// <summary>
    /// 供货商简称
    /// </summary>
    [SugarColumn(ColumnName = "supplier_short_name", ColumnDescription = "供货商简称", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? SupplierShortName { get; set; }
    /// <summary>
    /// 供货商类型（字典 logistics_procurement_supplier_category；0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_type", ColumnDescription = "供货商类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
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
    /// 供货商标识（税务登记证号/统一社会信用代码）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_tax_number", ColumnDescription = "供货商标识", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? SupplierTaxNumber { get; set; }
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
    /// 供货商电话
    /// </summary>
    [SugarColumn(ColumnName = "supplier_phone", ColumnDescription = "供货商电话", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? SupplierPhone { get; set; }
    /// <summary>
    /// 供货商传真
    /// </summary>
    [SugarColumn(ColumnName = "supplier_fax", ColumnDescription = "供货商传真", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? SupplierFax { get; set; }
    /// <summary>
    /// 供货商邮箱
    /// </summary>
    [SugarColumn(ColumnName = "supplier_email", ColumnDescription = "供货商邮箱", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? SupplierEmail { get; set; }
    /// <summary>
    /// 供货商网站
    /// </summary>
    [SugarColumn(ColumnName = "supplier_website", ColumnDescription = "供货商网站", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? SupplierWebsite { get; set; }
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
    /// 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）
    /// </summary>
    [SugarColumn(ColumnName = "reconciliation_account", ColumnDescription = "统驭科目", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = "")]
    public string ReconciliationAccount { get; set; } = string.Empty;
    /// <summary>
    /// 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
    /// </summary>
    [SugarColumn(ColumnName = "customer_code", ColumnDescription = "客户", ColumnDataType = "nvarchar", Length = 10, IsNullable = false, DefaultValue = "")]
    public string CustomerCode { get; set; } = string.Empty;
    /// <summary>
    /// 具有客户的清算（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "clearing_with_customer", ColumnDescription = "具有客户的清算", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ClearingWithCustomer { get; set; } = 0;
    /// <summary>
    /// 付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
    /// </summary>
    [SugarColumn(ColumnName = "payment_method", ColumnDescription = "付款方式", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int PaymentMethod { get; set; } = 1;
    /// <summary>
    /// 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
    /// </summary>
    [SugarColumn(ColumnName = "payment_terms", ColumnDescription = "付款条件", ColumnDataType = "nvarchar", Length = 40, IsNullable = false, DefaultValue = "cod")]
    public string PaymentTerms { get; set; } = "cod";
    /// <summary>
    /// 银行代码（选项 TaktBanks/options；DictValue=BankCode）
    /// </summary>
    [SugarColumn(ColumnName = "bank_code", ColumnDescription = "银行代码", ColumnDataType = "nvarchar", Length = 15, IsNullable = false, DefaultValue = "")]
    public string BankCode { get; set; } = string.Empty;
    /// <summary>
    /// 银行帐号
    /// </summary>
    [SugarColumn(ColumnName = "bank_account", ColumnDescription = "银行帐号", ColumnDataType = "nvarchar", Length = 40, IsNullable = false, DefaultValue = "")]
    public string BankAccount { get; set; } = string.Empty;
    /// <summary>
    /// 帐户持有人
    /// </summary>
    [SugarColumn(ColumnName = "account_holder", ColumnDescription = "帐户持有人", ColumnDataType = "nvarchar", Length = 100, IsNullable = false, DefaultValue = "")]
    public string AccountHolder { get; set; } = string.Empty;
    /// <summary>
    /// 基于收货的发票验证（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "gr_based_invoice_inspection", ColumnDescription = "基于收货的发票验证", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int GrBasedInvoiceInspection { get; set; } = 0;
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
    /// 自动产生的采购订单（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "automatic_purchase_order", ColumnDescription = "自动产生的采购订单", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int AutomaticPurchaseOrder { get; set; } = 0;
    /// <summary>
    /// 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
    /// </summary>
    [SugarColumn(ColumnName = "pricing_date_control", ColumnDescription = "定价日期控制", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int PricingDateControl { get; set; } = 1;
    /// <summary>
    /// 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_group", ColumnDescription = "采购组", ColumnDataType = "nvarchar", Length = 3, IsNullable = false, DefaultValue = "")]
    public string PurchaseGroup { get; set; } = string.Empty;
    /// <summary>
    /// 计划交货时间（天）
    /// </summary>
    [SugarColumn(ColumnName = "planned_delivery_time_days", ColumnDescription = "计划交货时间", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PlannedDeliveryTimeDays { get; set; } = 0;
    /// <summary>
    /// 评估收据结算（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "evaluated_receipt_settlement", ColumnDescription = "评估收据结算", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int EvaluatedReceiptSettlement { get; set; } = 0;
    /// <summary>
    /// 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "purchasing_organization", ColumnDescription = "采购组织", ColumnDataType = "varchar", Length = 4, IsNullable = false, DefaultValue = "")]
    public string PurchasingOrganization { get; set; } = string.Empty;
    /// <summary>
    /// 供货商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_level", ColumnDescription = "供货商等级", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SupplierLevel { get; set; } = 0;
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
    /// 供货商状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_status", ColumnDescription = "供货商状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int SupplierStatus { get; set; } = 1;
}

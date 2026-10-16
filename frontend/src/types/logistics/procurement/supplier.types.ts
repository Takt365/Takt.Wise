// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：supplier.types.ts
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/procurement 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * Takt供货商实体 <para>业务唯一键：TenantCode+CompanyCode+PurchasingOrganization+SupplierCode。</para>
 * 对应前端 TaktSupplierDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Supplier
 * @description 对应后端 TaktSupplierDto
 */
export interface Supplier extends CompanyDtoBase {
  /**
   * SupplierID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  supplierId: string;

  /**
   * 供应商（与公司、采购组织共同唯一）
   */
  supplierCode: string;

  /**
   * 账户组
   */
  accountGroup: string;

  /**
   * 称谓（字典 logistics_sales_customer_title；1=公司 2=先生 3=夫人 4=先生和夫人）
   */
  supplierTitle: number;

  /**
   * 名称 1
   */
  supplierName1: string;

  /**
   * 名称 2
   */
  supplierName2?: string;

  /**
   * 名称 3
   */
  supplierName3?: string;

  /**
   * 名称 4
   */
  supplierName4?: string;

  /**
   * 简称
   */
  supplierShortName?: string;

  /**
   * 国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode?: string;

  /**
   * 地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  regionCode?: string;

  /**
   * 城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  cityCode?: string;

  /**
   * 地址1
   */
  addr1?: string;

  /**
   * 地址2（含门牌，短字符串 40 不够）
   */
  addr2?: string;

  /**
   * 邮政编码
   */
  postalCode?: string;

  /**
   * 电话号码1
   */
  phone1?: string;

  /**
   * 电话号码2
   */
  phone2?: string;

  /**
   * 传真号码
   */
  faxNumber?: string;

  /**
   * 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 贸易伙伴
   */
  tradingPartner?: string;

  /**
   * 权限组
   */
  authorizationGroup?: string;

  /**
   * 组代码
   */
  groupKey?: string;

  /**
   * 供应商类型（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
   */
  supplierType: number;

  /**
   * 企业性质（字典 sys_enterprise_nature）
   */
  enterpriseNature: string;

  /**
   * 行业属性（字典 sys_industry_attribute）
   */
  industryAttribute: string;

  /**
   * 信用信息号
   */
  creditInfoNumber?: string;

  /**
   * 银行国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  bankCountryCode?: string;

  /**
   * 银行代码（选项 TaktBanks/options；DictValue=BankCode）
   */
  bankCode?: string;

  /**
   * 银行账户
   */
  bankAccount?: string;

  /**
   * 科目持有者
   */
  accountHolder?: string;

  /**
   * 指令码
   */
  bankChargeBearerCode?: string;

  /**
   * 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）
   */
  reconciliationAccount: string;

  /**
   * 总部
   */
  headOfficeAccountCode?: string;

  /**
   * 计划组
   */
  planningGroup?: string;

  /**
   * 付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）
   */
  paymentTerms: string;

  /**
   * 检查双重发票（字典 sys_yes_no；0=否 1=是）
   */
  doubleInvoiceCheck: number;

  /**
   * 付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
   */
  paymentMethod: number;

  /**
   * 付款冻结（字典 sys_yes_no；0=否 1=是）
   */
  paymentBlock: number;

  /**
   * 开户行（选项 TaktBanks/options；DictValue=BankCode）
   */
  houseBank?: string;

  /**
   * 在客户和供应商之间清算（字典 sys_yes_no；0=否 1=是）
   */
  clearingWithCustomer: number;

  /**
   * 会计职员
   */
  accountingClerk?: string;

  /**
   * 我们带有供应商的帐目号
   */
  ourAccountAtSupplier?: string;

  /**
   * 供应商联系人
   */
  contactPerson?: string;

  /**
   * 供应商联系电话
   */
  contactPhone?: string;

  /**
   * 供应商联系邮件（邮箱地址短字符串 40 不够）
   */
  contactEmail?: string;

  /**
   * 代扣所得税国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  withholdingTaxCountry?: string;

  /**
   * 预扣税类型（选项 TaktTaxCodes/options；DictValue=TaxCode；选中后回填预扣税代码）
   */
  withholdingTaxType?: string;

  /**
   * 预扣税代码（由预扣税类型 WithholdingTaxType 回填；选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  withholdingTaxCode?: string;

  /**
   * 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
   */
  purchasingOrganization: string;

  /**
   * 订单货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  orderCurrencyCode: string;

  /**
   * 国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
   */
  incoterms1: string;

  /**
   * 国际贸易条款2（地点说明）
   */
  incoterms2: string;

  /**
   * 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
   */
  pricingDateControl: number;

  /**
   * 基于收货的发票校验（字典 sys_yes_no；0=否 1=是）
   */
  grBasedInvoiceInspection: number;

  /**
   * 自动评估GRR结算交货（字典 sys_yes_no；0=否 1=是）
   */
  evaluatedReceiptSettlement: number;

  /**
   * 自动评估GR结算保留（字典 sys_yes_no；0=否 1=是）
   */
  returnErsFlag: number;

  /**
   * 自动建立采购订单（字典 sys_yes_no；0=否 1=是）
   */
  automaticPurchaseOrder: number;

  /**
   * 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroup: string;

  /**
   * 税号1
   */
  taxNumber1?: string;

  /**
   * 税号2
   */
  taxNumber2?: string;

  /**
   * 税号3（来源主数据最长 60）
   */
  taxNumber3?: string;

  /**
   * 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
   */
  creditLevel: number;

  /**
   * 供应商等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）
   */
  supplierLevel: number;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

  /**
   * 供货商状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  supplierStatus: number;

}


/**
 * Supplier 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SupplierQuery
 * @description 对应后端 TaktSupplierQueryDto
 */
export interface SupplierQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 供应商（与公司、采购组织共同唯一）
   */
  supplierCode?: string;

  /**
   * 账户组
   */
  accountGroup?: string;

  /**
   * 称谓（字典 logistics_sales_customer_title；1=公司 2=先生 3=夫人 4=先生和夫人）
   */
  supplierTitle?: number;

  /**
   * 名称 1
   */
  supplierName1?: string;

  /**
   * 名称 2
   */
  supplierName2?: string;

  /**
   * 名称 3
   */
  supplierName3?: string;

  /**
   * 名称 4
   */
  supplierName4?: string;

  /**
   * 简称
   */
  supplierShortName?: string;

  /**
   * 国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode?: string;

  /**
   * 地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  regionCode?: string;

  /**
   * 城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  cityCode?: string;

  /**
   * 地址1
   */
  addr1?: string;

  /**
   * 地址2（含门牌，短字符串 40 不够）
   */
  addr2?: string;

  /**
   * 邮政编码
   */
  postalCode?: string;

  /**
   * 电话号码1
   */
  phone1?: string;

  /**
   * 电话号码2
   */
  phone2?: string;

  /**
   * 传真号码
   */
  faxNumber?: string;

  /**
   * 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 贸易伙伴
   */
  tradingPartner?: string;

  /**
   * 权限组
   */
  authorizationGroup?: string;

  /**
   * 组代码
   */
  groupKey?: string;

  /**
   * 供应商类型（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
   */
  supplierType?: number;

  /**
   * 企业性质（字典 sys_enterprise_nature）
   */
  enterpriseNature?: string;

  /**
   * 行业属性（字典 sys_industry_attribute）
   */
  industryAttribute?: string;

  /**
   * 信用信息号
   */
  creditInfoNumber?: string;

  /**
   * 银行国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  bankCountryCode?: string;

  /**
   * 银行代码（选项 TaktBanks/options；DictValue=BankCode）
   */
  bankCode?: string;

  /**
   * 银行账户
   */
  bankAccount?: string;

  /**
   * 科目持有者
   */
  accountHolder?: string;

  /**
   * 指令码
   */
  bankChargeBearerCode?: string;

  /**
   * 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）
   */
  reconciliationAccount?: string;

  /**
   * 总部
   */
  headOfficeAccountCode?: string;

  /**
   * 计划组
   */
  planningGroup?: string;

  /**
   * 付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）
   */
  paymentTerms?: string;

  /**
   * 检查双重发票（字典 sys_yes_no；0=否 1=是）
   */
  doubleInvoiceCheck?: number;

  /**
   * 付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
   */
  paymentMethod?: number;

  /**
   * 付款冻结（字典 sys_yes_no；0=否 1=是）
   */
  paymentBlock?: number;

  /**
   * 开户行（选项 TaktBanks/options；DictValue=BankCode）
   */
  houseBank?: string;

  /**
   * 在客户和供应商之间清算（字典 sys_yes_no；0=否 1=是）
   */
  clearingWithCustomer?: number;

  /**
   * 会计职员
   */
  accountingClerk?: string;

  /**
   * 我们带有供应商的帐目号
   */
  ourAccountAtSupplier?: string;

  /**
   * 供应商联系人
   */
  contactPerson?: string;

  /**
   * 供应商联系电话
   */
  contactPhone?: string;

  /**
   * 供应商联系邮件（邮箱地址短字符串 40 不够）
   */
  contactEmail?: string;

  /**
   * 代扣所得税国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  withholdingTaxCountry?: string;

  /**
   * 预扣税类型（选项 TaktTaxCodes/options；DictValue=TaxCode；选中后回填预扣税代码）
   */
  withholdingTaxType?: string;

  /**
   * 预扣税代码（由预扣税类型 WithholdingTaxType 回填；选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  withholdingTaxCode?: string;

  /**
   * 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
   */
  purchasingOrganization?: string;

  /**
   * 订单货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  orderCurrencyCode?: string;

  /**
   * 国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
   */
  incoterms1?: string;

  /**
   * 国际贸易条款2（地点说明）
   */
  incoterms2?: string;

  /**
   * 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
   */
  pricingDateControl?: number;

  /**
   * 基于收货的发票校验（字典 sys_yes_no；0=否 1=是）
   */
  grBasedInvoiceInspection?: number;

  /**
   * 自动评估GRR结算交货（字典 sys_yes_no；0=否 1=是）
   */
  evaluatedReceiptSettlement?: number;

  /**
   * 自动评估GR结算保留（字典 sys_yes_no；0=否 1=是）
   */
  returnErsFlag?: number;

  /**
   * 自动建立采购订单（字典 sys_yes_no；0=否 1=是）
   */
  automaticPurchaseOrder?: number;

  /**
   * 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

  /**
   * 税号1
   */
  taxNumber1?: string;

  /**
   * 税号2
   */
  taxNumber2?: string;

  /**
   * 税号3（来源主数据最长 60）
   */
  taxNumber3?: string;

  /**
   * 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
   */
  creditLevel?: number;

  /**
   * 供应商等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）
   */
  supplierLevel?: number;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder?: number;

  /**
   * 供货商状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  supplierStatus?: number;

  /**
   * 创建时间（范围查询-开始）
   */
  createdAtStart?: string;

  /**
   * 创建时间（范围查询-结束）
   */
  createdAtEnd?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建Supplier DTO
 * 对应前端 SupplierCreate
 * @description 对应后端 TaktSupplierCreateDto
 */
export interface SupplierCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode: string;

  /**
   * 供应商（与公司、采购组织共同唯一；前端表单选择编码规则后自动通过 TaktNumbering 供应商编码规则生成并展示，非手输；单据类型菜单：供应商）
   */
  supplierCode: string;

  /**
   * 编码规则编码（新增时选择供应商编码规则后取号；不落库）
   */
  numberingRuleCode?: string;

  /**
   * 账户组
   */
  accountGroup: string;

  /**
   * 称谓（字典 logistics_sales_customer_title；1=公司 2=先生 3=夫人 4=先生和夫人）
   */
  supplierTitle: number;

  /**
   * 名称 1
   */
  supplierName1: string;

  /**
   * 名称 2
   */
  supplierName2?: string;

  /**
   * 名称 3
   */
  supplierName3?: string;

  /**
   * 名称 4
   */
  supplierName4?: string;

  /**
   * 简称
   */
  supplierShortName?: string;

  /**
   * 国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode?: string;

  /**
   * 地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  regionCode?: string;

  /**
   * 城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  cityCode?: string;

  /**
   * 地址1
   */
  addr1?: string;

  /**
   * 地址2（含门牌，短字符串 40 不够）
   */
  addr2?: string;

  /**
   * 邮政编码
   */
  postalCode?: string;

  /**
   * 电话号码1
   */
  phone1?: string;

  /**
   * 电话号码2
   */
  phone2?: string;

  /**
   * 传真号码
   */
  faxNumber?: string;

  /**
   * 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 贸易伙伴
   */
  tradingPartner?: string;

  /**
   * 权限组
   */
  authorizationGroup?: string;

  /**
   * 组代码
   */
  groupKey?: string;

  /**
   * 供应商类型（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
   */
  supplierType: number;

  /**
   * 企业性质（字典 sys_enterprise_nature）
   */
  enterpriseNature: string;

  /**
   * 行业属性（字典 sys_industry_attribute）
   */
  industryAttribute: string;

  /**
   * 信用信息号
   */
  creditInfoNumber?: string;

  /**
   * 银行国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  bankCountryCode?: string;

  /**
   * 银行代码（选项 TaktBanks/options；DictValue=BankCode）
   */
  bankCode?: string;

  /**
   * 银行账户
   */
  bankAccount?: string;

  /**
   * 科目持有者
   */
  accountHolder?: string;

  /**
   * 指令码
   */
  bankChargeBearerCode?: string;

  /**
   * 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）
   */
  reconciliationAccount: string;

  /**
   * 总部
   */
  headOfficeAccountCode?: string;

  /**
   * 计划组
   */
  planningGroup?: string;

  /**
   * 付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）
   */
  paymentTerms: string;

  /**
   * 检查双重发票（字典 sys_yes_no；0=否 1=是）
   */
  doubleInvoiceCheck: number;

  /**
   * 付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
   */
  paymentMethod: number;

  /**
   * 付款冻结（字典 sys_yes_no；0=否 1=是）
   */
  paymentBlock: number;

  /**
   * 开户行（选项 TaktBanks/options；DictValue=BankCode）
   */
  houseBank?: string;

  /**
   * 在客户和供应商之间清算（字典 sys_yes_no；0=否 1=是）
   */
  clearingWithCustomer: number;

  /**
   * 会计职员
   */
  accountingClerk?: string;

  /**
   * 我们带有供应商的帐目号
   */
  ourAccountAtSupplier?: string;

  /**
   * 供应商联系人
   */
  contactPerson?: string;

  /**
   * 供应商联系电话
   */
  contactPhone?: string;

  /**
   * 供应商联系邮件（邮箱地址短字符串 40 不够）
   */
  contactEmail?: string;

  /**
   * 代扣所得税国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  withholdingTaxCountry?: string;

  /**
   * 预扣税类型（选项 TaktTaxCodes/options；DictValue=TaxCode；选中后回填预扣税代码）
   */
  withholdingTaxType?: string;

  /**
   * 预扣税代码（由预扣税类型 WithholdingTaxType 回填；选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  withholdingTaxCode?: string;

  /**
   * 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
   */
  purchasingOrganization: string;

  /**
   * 订单货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  orderCurrencyCode: string;

  /**
   * 国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
   */
  incoterms1: string;

  /**
   * 国际贸易条款2（地点说明）
   */
  incoterms2: string;

  /**
   * 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
   */
  pricingDateControl: number;

  /**
   * 基于收货的发票校验（字典 sys_yes_no；0=否 1=是）
   */
  grBasedInvoiceInspection: number;

  /**
   * 自动评估GRR结算交货（字典 sys_yes_no；0=否 1=是）
   */
  evaluatedReceiptSettlement: number;

  /**
   * 自动评估GR结算保留（字典 sys_yes_no；0=否 1=是）
   */
  returnErsFlag: number;

  /**
   * 自动建立采购订单（字典 sys_yes_no；0=否 1=是）
   */
  automaticPurchaseOrder: number;

  /**
   * 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroup: string;

  /**
   * 税号1
   */
  taxNumber1?: string;

  /**
   * 税号2
   */
  taxNumber2?: string;

  /**
   * 税号3（来源主数据最长 60）
   */
  taxNumber3?: string;

  /**
   * 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
   */
  creditLevel: number;

  /**
   * 供应商等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）
   */
  supplierLevel: number;

  /**
   * 供货商状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  supplierStatus: number;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新Supplier DTO
 * 继承 TaktSupplierCreateDto，添加 SupplierId 字段
 * 对应前端 SupplierUpdate
 * @description 对应后端 TaktSupplierUpdateDto
 */
export interface SupplierUpdate extends SupplierCreate {
  /**
   * SupplierID（标识要更新的实体）
   */
  supplierId: string;

}


/**
 * Supplier 状态更新 DTO
 * 对应前端 SupplierStatus
 * @description 对应后端 TaktSupplierStatusDto
 */
export interface SupplierStatus {
  /**
   * SupplierID
   */
  supplierId: string;

  /**
   * 供货商状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  supplierStatus: number;

}


/**
 * Supplier 排序更新 DTO
 * 对应前端 SupplierSort
 * @description 对应后端 TaktSupplierSortDto
 */
export interface SupplierSort {
  /**
   * SupplierID
   */
  supplierId: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

}


/**
 * Supplier 导入模板行 DTO
 * 对应前端 SupplierTemplate
 * @description 对应后端 TaktSupplierTemplateDto
 */
export interface SupplierTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode?: string;

  /**
   * 供应商（与公司、采购组织共同唯一）
   */
  supplierCode?: string;

  /**
   * 账户组
   */
  accountGroup?: string;

  /**
   * 称谓（字典 logistics_sales_customer_title；1=公司 2=先生 3=夫人 4=先生和夫人）
   */
  supplierTitle?: number;

  /**
   * 名称 1
   */
  supplierName1?: string;

  /**
   * 名称 2
   */
  supplierName2?: string;

  /**
   * 名称 3
   */
  supplierName3?: string;

  /**
   * 名称 4
   */
  supplierName4?: string;

  /**
   * 简称
   */
  supplierShortName?: string;

  /**
   * 国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode?: string;

  /**
   * 地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  regionCode?: string;

  /**
   * 城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  cityCode?: string;

  /**
   * 地址1
   */
  addr1?: string;

  /**
   * 地址2（含门牌，短字符串 40 不够）
   */
  addr2?: string;

  /**
   * 邮政编码
   */
  postalCode?: string;

  /**
   * 电话号码1
   */
  phone1?: string;

  /**
   * 电话号码2
   */
  phone2?: string;

  /**
   * 传真号码
   */
  faxNumber?: string;

  /**
   * 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 贸易伙伴
   */
  tradingPartner?: string;

  /**
   * 权限组
   */
  authorizationGroup?: string;

  /**
   * 组代码
   */
  groupKey?: string;

  /**
   * 供应商类型（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
   */
  supplierType?: number;

  /**
   * 企业性质（字典 sys_enterprise_nature）
   */
  enterpriseNature?: string;

  /**
   * 行业属性（字典 sys_industry_attribute）
   */
  industryAttribute?: string;

  /**
   * 信用信息号
   */
  creditInfoNumber?: string;

  /**
   * 银行国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  bankCountryCode?: string;

  /**
   * 银行代码（选项 TaktBanks/options；DictValue=BankCode）
   */
  bankCode?: string;

  /**
   * 银行账户
   */
  bankAccount?: string;

  /**
   * 科目持有者
   */
  accountHolder?: string;

  /**
   * 指令码
   */
  bankChargeBearerCode?: string;

  /**
   * 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）
   */
  reconciliationAccount?: string;

  /**
   * 总部
   */
  headOfficeAccountCode?: string;

  /**
   * 计划组
   */
  planningGroup?: string;

  /**
   * 付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）
   */
  paymentTerms?: string;

  /**
   * 检查双重发票（字典 sys_yes_no；0=否 1=是）
   */
  doubleInvoiceCheck?: number;

  /**
   * 付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
   */
  paymentMethod?: number;

  /**
   * 付款冻结（字典 sys_yes_no；0=否 1=是）
   */
  paymentBlock?: number;

  /**
   * 开户行（选项 TaktBanks/options；DictValue=BankCode）
   */
  houseBank?: string;

  /**
   * 在客户和供应商之间清算（字典 sys_yes_no；0=否 1=是）
   */
  clearingWithCustomer?: number;

  /**
   * 会计职员
   */
  accountingClerk?: string;

  /**
   * 我们带有供应商的帐目号
   */
  ourAccountAtSupplier?: string;

  /**
   * 供应商联系人
   */
  contactPerson?: string;

  /**
   * 供应商联系电话
   */
  contactPhone?: string;

  /**
   * 供应商联系邮件（邮箱地址短字符串 40 不够）
   */
  contactEmail?: string;

  /**
   * 代扣所得税国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  withholdingTaxCountry?: string;

  /**
   * 预扣税类型（选项 TaktTaxCodes/options；DictValue=TaxCode；选中后回填预扣税代码）
   */
  withholdingTaxType?: string;

  /**
   * 预扣税代码（由预扣税类型 WithholdingTaxType 回填；选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  withholdingTaxCode?: string;

  /**
   * 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
   */
  purchasingOrganization?: string;

  /**
   * 订单货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  orderCurrencyCode?: string;

  /**
   * 国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
   */
  incoterms1?: string;

  /**
   * 国际贸易条款2（地点说明）
   */
  incoterms2?: string;

  /**
   * 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
   */
  pricingDateControl?: number;

  /**
   * 基于收货的发票校验（字典 sys_yes_no；0=否 1=是）
   */
  grBasedInvoiceInspection?: number;

  /**
   * 自动评估GRR结算交货（字典 sys_yes_no；0=否 1=是）
   */
  evaluatedReceiptSettlement?: number;

  /**
   * 自动评估GR结算保留（字典 sys_yes_no；0=否 1=是）
   */
  returnErsFlag?: number;

  /**
   * 自动建立采购订单（字典 sys_yes_no；0=否 1=是）
   */
  automaticPurchaseOrder?: number;

  /**
   * 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

  /**
   * 税号1
   */
  taxNumber1?: string;

  /**
   * 税号2
   */
  taxNumber2?: string;

  /**
   * 税号3（来源主数据最长 60）
   */
  taxNumber3?: string;

  /**
   * 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
   */
  creditLevel?: number;

  /**
   * 供应商等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）
   */
  supplierLevel?: number;

  /**
   * 供货商状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  supplierStatus?: number;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * Supplier 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SupplierImport
 * @description 对应后端 TaktSupplierImportDto
 */
export interface SupplierImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode?: string;

  /**
   * 供应商（与公司、采购组织共同唯一）
   */
  supplierCode?: string;

  /**
   * 账户组
   */
  accountGroup?: string;

  /**
   * 称谓（字典 logistics_sales_customer_title；1=公司 2=先生 3=夫人 4=先生和夫人）
   */
  supplierTitle?: number;

  /**
   * 名称 1
   */
  supplierName1?: string;

  /**
   * 名称 2
   */
  supplierName2?: string;

  /**
   * 名称 3
   */
  supplierName3?: string;

  /**
   * 名称 4
   */
  supplierName4?: string;

  /**
   * 简称
   */
  supplierShortName?: string;

  /**
   * 国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode?: string;

  /**
   * 地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  regionCode?: string;

  /**
   * 城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  cityCode?: string;

  /**
   * 地址1
   */
  addr1?: string;

  /**
   * 地址2（含门牌，短字符串 40 不够）
   */
  addr2?: string;

  /**
   * 邮政编码
   */
  postalCode?: string;

  /**
   * 电话号码1
   */
  phone1?: string;

  /**
   * 电话号码2
   */
  phone2?: string;

  /**
   * 传真号码
   */
  faxNumber?: string;

  /**
   * 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 贸易伙伴
   */
  tradingPartner?: string;

  /**
   * 权限组
   */
  authorizationGroup?: string;

  /**
   * 组代码
   */
  groupKey?: string;

  /**
   * 供应商类型（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
   */
  supplierType?: number;

  /**
   * 企业性质（字典 sys_enterprise_nature）
   */
  enterpriseNature?: string;

  /**
   * 行业属性（字典 sys_industry_attribute）
   */
  industryAttribute?: string;

  /**
   * 信用信息号
   */
  creditInfoNumber?: string;

  /**
   * 银行国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  bankCountryCode?: string;

  /**
   * 银行代码（选项 TaktBanks/options；DictValue=BankCode）
   */
  bankCode?: string;

  /**
   * 银行账户
   */
  bankAccount?: string;

  /**
   * 科目持有者
   */
  accountHolder?: string;

  /**
   * 指令码
   */
  bankChargeBearerCode?: string;

  /**
   * 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）
   */
  reconciliationAccount?: string;

  /**
   * 总部
   */
  headOfficeAccountCode?: string;

  /**
   * 计划组
   */
  planningGroup?: string;

  /**
   * 付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）
   */
  paymentTerms?: string;

  /**
   * 检查双重发票（字典 sys_yes_no；0=否 1=是）
   */
  doubleInvoiceCheck?: number;

  /**
   * 付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
   */
  paymentMethod?: number;

  /**
   * 付款冻结（字典 sys_yes_no；0=否 1=是）
   */
  paymentBlock?: number;

  /**
   * 开户行（选项 TaktBanks/options；DictValue=BankCode）
   */
  houseBank?: string;

  /**
   * 在客户和供应商之间清算（字典 sys_yes_no；0=否 1=是）
   */
  clearingWithCustomer?: number;

  /**
   * 会计职员
   */
  accountingClerk?: string;

  /**
   * 我们带有供应商的帐目号
   */
  ourAccountAtSupplier?: string;

  /**
   * 供应商联系人
   */
  contactPerson?: string;

  /**
   * 供应商联系电话
   */
  contactPhone?: string;

  /**
   * 供应商联系邮件（邮箱地址短字符串 40 不够）
   */
  contactEmail?: string;

  /**
   * 代扣所得税国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  withholdingTaxCountry?: string;

  /**
   * 预扣税类型（选项 TaktTaxCodes/options；DictValue=TaxCode；选中后回填预扣税代码）
   */
  withholdingTaxType?: string;

  /**
   * 预扣税代码（由预扣税类型 WithholdingTaxType 回填；选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  withholdingTaxCode?: string;

  /**
   * 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
   */
  purchasingOrganization?: string;

  /**
   * 订单货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  orderCurrencyCode?: string;

  /**
   * 国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
   */
  incoterms1?: string;

  /**
   * 国际贸易条款2（地点说明）
   */
  incoterms2?: string;

  /**
   * 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
   */
  pricingDateControl?: number;

  /**
   * 基于收货的发票校验（字典 sys_yes_no；0=否 1=是）
   */
  grBasedInvoiceInspection?: number;

  /**
   * 自动评估GRR结算交货（字典 sys_yes_no；0=否 1=是）
   */
  evaluatedReceiptSettlement?: number;

  /**
   * 自动评估GR结算保留（字典 sys_yes_no；0=否 1=是）
   */
  returnErsFlag?: number;

  /**
   * 自动建立采购订单（字典 sys_yes_no；0=否 1=是）
   */
  automaticPurchaseOrder?: number;

  /**
   * 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

  /**
   * 税号1
   */
  taxNumber1?: string;

  /**
   * 税号2
   */
  taxNumber2?: string;

  /**
   * 税号3（来源主数据最长 60）
   */
  taxNumber3?: string;

  /**
   * 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
   */
  creditLevel?: number;

  /**
   * 供应商等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）
   */
  supplierLevel?: number;

  /**
   * 供货商状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  supplierStatus?: number;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * Supplier 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SupplierExport
 * @description 对应后端 TaktSupplierExportDto
 */
export interface SupplierExport {
  /**
   * SupplierID
   */
  supplierId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 供应商（与公司、采购组织共同唯一）
   */
  supplierCode: string;

  /**
   * 账户组
   */
  accountGroup: string;

  /**
   * 称谓（字典 logistics_sales_customer_title；1=公司 2=先生 3=夫人 4=先生和夫人）
   */
  supplierTitle: number;

  /**
   * 名称 1
   */
  supplierName1: string;

  /**
   * 名称 2
   */
  supplierName2?: string;

  /**
   * 名称 3
   */
  supplierName3?: string;

  /**
   * 名称 4
   */
  supplierName4?: string;

  /**
   * 简称
   */
  supplierShortName?: string;

  /**
   * 国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode?: string;

  /**
   * 地区（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  regionCode?: string;

  /**
   * 城市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  cityCode?: string;

  /**
   * 地址1
   */
  addr1?: string;

  /**
   * 地址2（含门牌，短字符串 40 不够）
   */
  addr2?: string;

  /**
   * 邮政编码
   */
  postalCode?: string;

  /**
   * 电话号码1
   */
  phone1?: string;

  /**
   * 电话号码2
   */
  phone2?: string;

  /**
   * 传真号码
   */
  faxNumber?: string;

  /**
   * 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 贸易伙伴
   */
  tradingPartner?: string;

  /**
   * 权限组
   */
  authorizationGroup?: string;

  /**
   * 组代码
   */
  groupKey?: string;

  /**
   * 供应商类型（字典 logistics_sales_customer_category；0=企业客户 1=个人客户 2=政府机构 3=其他）
   */
  supplierType: number;

  /**
   * 企业性质（字典 sys_enterprise_nature）
   */
  enterpriseNature: string;

  /**
   * 行业属性（字典 sys_industry_attribute）
   */
  industryAttribute: string;

  /**
   * 信用信息号
   */
  creditInfoNumber?: string;

  /**
   * 银行国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  bankCountryCode?: string;

  /**
   * 银行代码（选项 TaktBanks/options；DictValue=BankCode）
   */
  bankCode?: string;

  /**
   * 银行账户
   */
  bankAccount?: string;

  /**
   * 科目持有者
   */
  accountHolder?: string;

  /**
   * 指令码
   */
  bankChargeBearerCode?: string;

  /**
   * 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）
   */
  reconciliationAccount: string;

  /**
   * 总部
   */
  headOfficeAccountCode?: string;

  /**
   * 计划组
   */
  planningGroup?: string;

  /**
   * 付款条件（字典 accounting_financial_payment_terms_param；DictValue=PREPAYSHIP/COD/NET30 等）
   */
  paymentTerms: string;

  /**
   * 检查双重发票（字典 sys_yes_no；0=否 1=是）
   */
  doubleInvoiceCheck: number;

  /**
   * 付款方式（字典 accounting_financial_payment_method；0=现金 1=银行转账 2=支票 3=信用证 4=其他）
   */
  paymentMethod: number;

  /**
   * 付款冻结（字典 sys_yes_no；0=否 1=是）
   */
  paymentBlock: number;

  /**
   * 开户行（选项 TaktBanks/options；DictValue=BankCode）
   */
  houseBank?: string;

  /**
   * 在客户和供应商之间清算（字典 sys_yes_no；0=否 1=是）
   */
  clearingWithCustomer: number;

  /**
   * 会计职员
   */
  accountingClerk?: string;

  /**
   * 我们带有供应商的帐目号
   */
  ourAccountAtSupplier?: string;

  /**
   * 供应商联系人
   */
  contactPerson?: string;

  /**
   * 供应商联系电话
   */
  contactPhone?: string;

  /**
   * 供应商联系邮件（邮箱地址短字符串 40 不够）
   */
  contactEmail?: string;

  /**
   * 代扣所得税国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  withholdingTaxCountry?: string;

  /**
   * 预扣税类型（选项 TaktTaxCodes/options；DictValue=TaxCode；选中后回填预扣税代码）
   */
  withholdingTaxType?: string;

  /**
   * 预扣税代码（由预扣税类型 WithholdingTaxType 回填；选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  withholdingTaxCode?: string;

  /**
   * 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
   */
  purchasingOrganization: string;

  /**
   * 订单货币（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  orderCurrencyCode: string;

  /**
   * 国际贸易条款1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
   */
  incoterms1: string;

  /**
   * 国际贸易条款2（地点说明）
   */
  incoterms2: string;

  /**
   * 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
   */
  pricingDateControl: number;

  /**
   * 基于收货的发票校验（字典 sys_yes_no；0=否 1=是）
   */
  grBasedInvoiceInspection: number;

  /**
   * 自动评估GRR结算交货（字典 sys_yes_no；0=否 1=是）
   */
  evaluatedReceiptSettlement: number;

  /**
   * 自动评估GR结算保留（字典 sys_yes_no；0=否 1=是）
   */
  returnErsFlag: number;

  /**
   * 自动建立采购订单（字典 sys_yes_no；0=否 1=是）
   */
  automaticPurchaseOrder: number;

  /**
   * 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroup: string;

  /**
   * 税号1
   */
  taxNumber1?: string;

  /**
   * 税号2
   */
  taxNumber2?: string;

  /**
   * 税号3（来源主数据最长 60）
   */
  taxNumber3?: string;

  /**
   * 信用等级（字典 logistics_sales_credit_rating；0=无 1=A级 2=AA级 3=AAA级 4=B级 5=C级）
   */
  creditLevel: number;

  /**
   * 供应商等级（字典 logistics_sales_customer_level；0=普通 1=重要 2=VIP 3=战略）
   */
  supplierLevel: number;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

  /**
   * 供货商状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  supplierStatus: number;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}


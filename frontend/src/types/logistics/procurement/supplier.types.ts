// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：supplier.types.ts
// 创建时间：2026-08-23
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
 * Takt供货商实体
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
   * 供货商编码（唯一索引）
   */
  supplierCode: string;

  /**
   * 供货商名称1
   */
  supplierName1: string;

  /**
   * 供货商名称2
   */
  supplierName2?: string;

  /**
   * 供货商简称
   */
  supplierShortName?: string;

  /**
   * 供货商类型（字典 logistics_procurement_supplier_category；0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）
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
   * 供货商标识（税务登记证号/统一社会信用代码）
   */
  supplierTaxNumber?: string;

  /**
   * 税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
   */
  taxCode?: string;

  /**
   * 税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）
   */
  taxRate: number;

  /**
   * 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  registrationCountry?: string;

  /**
   * 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  registrationProvince?: string;

  /**
   * 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  registrationCity?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 供货商电话
   */
  supplierPhone?: string;

  /**
   * 供货商传真
   */
  supplierFax?: string;

  /**
   * 供货商邮箱
   */
  supplierEmail?: string;

  /**
   * 供货商网站
   */
  supplierWebsite?: string;

  /**
   * 联系人
   */
  contactPerson?: string;

  /**
   * 联系人电话
   */
  contactPhone?: string;

  /**
   * 联系人邮箱
   */
  contactEmail?: string;

  /**
   * 结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  currencyCode: string;

  /**
   * 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）
   */
  reconciliationAccount: string;

  /**
   * 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode: string;

  /**
   * 具有客户的清算（字典 sys_yes_no；0=否 1=是）
   */
  clearingWithCustomer: number;

  /**
   * 付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod: number;

  /**
   * 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
   */
  paymentTerms: string;

  /**
   * 银行代码（选项 TaktBanks/options；DictValue=BankCode）
   */
  bankCode: string;

  /**
   * 银行帐号
   */
  bankAccount: string;

  /**
   * 帐户持有人
   */
  accountHolder: string;

  /**
   * 基于收货的发票验证（字典 sys_yes_no；0=否 1=是）
   */
  grBasedInvoiceInspection: number;

  /**
   * 国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
   */
  incoterms1: string;

  /**
   * 国际贸易条件2（地点说明）
   */
  incoterms2: string;

  /**
   * 自动产生的采购订单（字典 sys_yes_no；0=否 1=是）
   */
  automaticPurchaseOrder: number;

  /**
   * 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
   */
  pricingDateControl: number;

  /**
   * 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroup: string;

  /**
   * 计划交货时间（天）
   */
  plannedDeliveryTimeDays: number;

  /**
   * 评估收据结算（字典 sys_yes_no；0=否 1=是）
   */
  evaluatedReceiptSettlement: number;

  /**
   * 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
   */
  purchasingOrganization: string;

  /**
   * 供货商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时）
   */
  supplierLevel: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore: number;

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
   * 供货商编码（唯一索引）
   */
  supplierCode?: string;

  /**
   * 供货商名称1
   */
  supplierName1?: string;

  /**
   * 供货商名称2
   */
  supplierName2?: string;

  /**
   * 供货商简称
   */
  supplierShortName?: string;

  /**
   * 供货商类型（字典 logistics_procurement_supplier_category；0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）
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
   * 供货商标识（税务登记证号/统一社会信用代码）
   */
  supplierTaxNumber?: string;

  /**
   * 税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
   */
  taxCode?: string;

  /**
   * 税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）
   */
  taxRate?: number;

  /**
   * 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  registrationCountry?: string;

  /**
   * 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  registrationProvince?: string;

  /**
   * 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  registrationCity?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 供货商电话
   */
  supplierPhone?: string;

  /**
   * 供货商传真
   */
  supplierFax?: string;

  /**
   * 供货商邮箱
   */
  supplierEmail?: string;

  /**
   * 供货商网站
   */
  supplierWebsite?: string;

  /**
   * 联系人
   */
  contactPerson?: string;

  /**
   * 联系人电话
   */
  contactPhone?: string;

  /**
   * 联系人邮箱
   */
  contactEmail?: string;

  /**
   * 结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  currencyCode?: string;

  /**
   * 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）
   */
  reconciliationAccount?: string;

  /**
   * 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 具有客户的清算（字典 sys_yes_no；0=否 1=是）
   */
  clearingWithCustomer?: number;

  /**
   * 付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod?: number;

  /**
   * 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
   */
  paymentTerms?: string;

  /**
   * 银行代码（选项 TaktBanks/options；DictValue=BankCode）
   */
  bankCode?: string;

  /**
   * 银行帐号
   */
  bankAccount?: string;

  /**
   * 帐户持有人
   */
  accountHolder?: string;

  /**
   * 基于收货的发票验证（字典 sys_yes_no；0=否 1=是）
   */
  grBasedInvoiceInspection?: number;

  /**
   * 国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
   */
  incoterms1?: string;

  /**
   * 国际贸易条件2（地点说明）
   */
  incoterms2?: string;

  /**
   * 自动产生的采购订单（字典 sys_yes_no；0=否 1=是）
   */
  automaticPurchaseOrder?: number;

  /**
   * 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
   */
  pricingDateControl?: number;

  /**
   * 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

  /**
   * 计划交货时间（天）
   */
  plannedDeliveryTimeDays?: number;

  /**
   * 评估收据结算（字典 sys_yes_no；0=否 1=是）
   */
  evaluatedReceiptSettlement?: number;

  /**
   * 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
   */
  purchasingOrganization?: string;

  /**
   * 供货商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时）
   */
  supplierLevel?: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore?: number;

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
   * 供货商编码（唯一索引）
   */
  supplierCode: string;

  /**
   * 供货商名称1
   */
  supplierName1: string;

  /**
   * 供货商名称2
   */
  supplierName2?: string;

  /**
   * 供货商简称
   */
  supplierShortName?: string;

  /**
   * 供货商类型（字典 logistics_procurement_supplier_category；0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）
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
   * 供货商标识（税务登记证号/统一社会信用代码）
   */
  supplierTaxNumber?: string;

  /**
   * 税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
   */
  taxCode?: string;

  /**
   * 税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）
   */
  taxRate: number;

  /**
   * 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  registrationCountry?: string;

  /**
   * 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  registrationProvince?: string;

  /**
   * 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  registrationCity?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 供货商电话
   */
  supplierPhone?: string;

  /**
   * 供货商传真
   */
  supplierFax?: string;

  /**
   * 供货商邮箱
   */
  supplierEmail?: string;

  /**
   * 供货商网站
   */
  supplierWebsite?: string;

  /**
   * 联系人
   */
  contactPerson?: string;

  /**
   * 联系人电话
   */
  contactPhone?: string;

  /**
   * 联系人邮箱
   */
  contactEmail?: string;

  /**
   * 结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  currencyCode: string;

  /**
   * 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）
   */
  reconciliationAccount: string;

  /**
   * 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode: string;

  /**
   * 具有客户的清算（字典 sys_yes_no；0=否 1=是）
   */
  clearingWithCustomer: number;

  /**
   * 付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod: number;

  /**
   * 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
   */
  paymentTerms: string;

  /**
   * 银行代码（选项 TaktBanks/options；DictValue=BankCode）
   */
  bankCode: string;

  /**
   * 银行帐号
   */
  bankAccount: string;

  /**
   * 帐户持有人
   */
  accountHolder: string;

  /**
   * 基于收货的发票验证（字典 sys_yes_no；0=否 1=是）
   */
  grBasedInvoiceInspection: number;

  /**
   * 国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
   */
  incoterms1: string;

  /**
   * 国际贸易条件2（地点说明）
   */
  incoterms2: string;

  /**
   * 自动产生的采购订单（字典 sys_yes_no；0=否 1=是）
   */
  automaticPurchaseOrder: number;

  /**
   * 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
   */
  pricingDateControl: number;

  /**
   * 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroup: string;

  /**
   * 计划交货时间（天）
   */
  plannedDeliveryTimeDays: number;

  /**
   * 评估收据结算（字典 sys_yes_no；0=否 1=是）
   */
  evaluatedReceiptSettlement: number;

  /**
   * 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
   */
  purchasingOrganization: string;

  /**
   * 供货商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时）
   */
  supplierLevel: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore: number;

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
   * 供货商编码（唯一索引）
   */
  supplierCode?: string;

  /**
   * 供货商名称1
   */
  supplierName1?: string;

  /**
   * 供货商名称2
   */
  supplierName2?: string;

  /**
   * 供货商简称
   */
  supplierShortName?: string;

  /**
   * 供货商类型（字典 logistics_procurement_supplier_category；0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）
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
   * 供货商标识（税务登记证号/统一社会信用代码）
   */
  supplierTaxNumber?: string;

  /**
   * 税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
   */
  taxCode?: string;

  /**
   * 税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）
   */
  taxRate?: number;

  /**
   * 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  registrationCountry?: string;

  /**
   * 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  registrationProvince?: string;

  /**
   * 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  registrationCity?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 供货商电话
   */
  supplierPhone?: string;

  /**
   * 供货商传真
   */
  supplierFax?: string;

  /**
   * 供货商邮箱
   */
  supplierEmail?: string;

  /**
   * 供货商网站
   */
  supplierWebsite?: string;

  /**
   * 联系人
   */
  contactPerson?: string;

  /**
   * 联系人电话
   */
  contactPhone?: string;

  /**
   * 联系人邮箱
   */
  contactEmail?: string;

  /**
   * 结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  currencyCode?: string;

  /**
   * 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）
   */
  reconciliationAccount?: string;

  /**
   * 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 具有客户的清算（字典 sys_yes_no；0=否 1=是）
   */
  clearingWithCustomer?: number;

  /**
   * 付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod?: number;

  /**
   * 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
   */
  paymentTerms?: string;

  /**
   * 银行代码（选项 TaktBanks/options；DictValue=BankCode）
   */
  bankCode?: string;

  /**
   * 银行帐号
   */
  bankAccount?: string;

  /**
   * 帐户持有人
   */
  accountHolder?: string;

  /**
   * 基于收货的发票验证（字典 sys_yes_no；0=否 1=是）
   */
  grBasedInvoiceInspection?: number;

  /**
   * 国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
   */
  incoterms1?: string;

  /**
   * 国际贸易条件2（地点说明）
   */
  incoterms2?: string;

  /**
   * 自动产生的采购订单（字典 sys_yes_no；0=否 1=是）
   */
  automaticPurchaseOrder?: number;

  /**
   * 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
   */
  pricingDateControl?: number;

  /**
   * 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

  /**
   * 计划交货时间（天）
   */
  plannedDeliveryTimeDays?: number;

  /**
   * 评估收据结算（字典 sys_yes_no；0=否 1=是）
   */
  evaluatedReceiptSettlement?: number;

  /**
   * 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
   */
  purchasingOrganization?: string;

  /**
   * 供货商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时）
   */
  supplierLevel?: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore?: number;

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
   * 供货商编码（唯一索引）
   */
  supplierCode?: string;

  /**
   * 供货商名称1
   */
  supplierName1?: string;

  /**
   * 供货商名称2
   */
  supplierName2?: string;

  /**
   * 供货商简称
   */
  supplierShortName?: string;

  /**
   * 供货商类型（字典 logistics_procurement_supplier_category；0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）
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
   * 供货商标识（税务登记证号/统一社会信用代码）
   */
  supplierTaxNumber?: string;

  /**
   * 税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
   */
  taxCode?: string;

  /**
   * 税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）
   */
  taxRate?: number;

  /**
   * 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  registrationCountry?: string;

  /**
   * 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  registrationProvince?: string;

  /**
   * 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  registrationCity?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 供货商电话
   */
  supplierPhone?: string;

  /**
   * 供货商传真
   */
  supplierFax?: string;

  /**
   * 供货商邮箱
   */
  supplierEmail?: string;

  /**
   * 供货商网站
   */
  supplierWebsite?: string;

  /**
   * 联系人
   */
  contactPerson?: string;

  /**
   * 联系人电话
   */
  contactPhone?: string;

  /**
   * 联系人邮箱
   */
  contactEmail?: string;

  /**
   * 结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  currencyCode?: string;

  /**
   * 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）
   */
  reconciliationAccount?: string;

  /**
   * 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 具有客户的清算（字典 sys_yes_no；0=否 1=是）
   */
  clearingWithCustomer?: number;

  /**
   * 付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod?: number;

  /**
   * 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
   */
  paymentTerms?: string;

  /**
   * 银行代码（选项 TaktBanks/options；DictValue=BankCode）
   */
  bankCode?: string;

  /**
   * 银行帐号
   */
  bankAccount?: string;

  /**
   * 帐户持有人
   */
  accountHolder?: string;

  /**
   * 基于收货的发票验证（字典 sys_yes_no；0=否 1=是）
   */
  grBasedInvoiceInspection?: number;

  /**
   * 国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
   */
  incoterms1?: string;

  /**
   * 国际贸易条件2（地点说明）
   */
  incoterms2?: string;

  /**
   * 自动产生的采购订单（字典 sys_yes_no；0=否 1=是）
   */
  automaticPurchaseOrder?: number;

  /**
   * 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
   */
  pricingDateControl?: number;

  /**
   * 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

  /**
   * 计划交货时间（天）
   */
  plannedDeliveryTimeDays?: number;

  /**
   * 评估收据结算（字典 sys_yes_no；0=否 1=是）
   */
  evaluatedReceiptSettlement?: number;

  /**
   * 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
   */
  purchasingOrganization?: string;

  /**
   * 供货商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时）
   */
  supplierLevel?: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore?: number;

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
   * 供货商编码（唯一索引）
   */
  supplierCode: string;

  /**
   * 供货商名称1
   */
  supplierName1: string;

  /**
   * 供货商名称2
   */
  supplierName2?: string;

  /**
   * 供货商简称
   */
  supplierShortName?: string;

  /**
   * 供货商类型（字典 logistics_procurement_supplier_category；0=生产商，1=代理商，2=经销商，3=贸易商，4=其他）
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
   * 供货商标识（税务登记证号/统一社会信用代码）
   */
  supplierTaxNumber?: string;

  /**
   * 税码（字典 accounting_financial_tax_code；按 CultureCode 匹配 TaktDictData.CultureCode；DictValue 随区域变化）
   */
  taxCode?: string;

  /**
   * 税率（百分比整数；由税码 TaxCode / 字典 accounting_financial_tax_code.ExtValue 回填，如 J2→13）
   */
  taxRate: number;

  /**
   * 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  registrationCountry?: string;

  /**
   * 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  registrationProvince?: string;

  /**
   * 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  registrationCity?: string;

  /**
   * 注册地址1
   */
  registrationAddress1?: string;

  /**
   * 注册地址2
   */
  registrationAddress2?: string;

  /**
   * 供货商电话
   */
  supplierPhone?: string;

  /**
   * 供货商传真
   */
  supplierFax?: string;

  /**
   * 供货商邮箱
   */
  supplierEmail?: string;

  /**
   * 供货商网站
   */
  supplierWebsite?: string;

  /**
   * 联系人
   */
  contactPerson?: string;

  /**
   * 联系人电话
   */
  contactPhone?: string;

  /**
   * 联系人邮箱
   */
  contactEmail?: string;

  /**
   * 结算币种代码（字典 accounting_financial_currency_code；DictValue=CNY/USD 等）
   */
  currencyCode: string;

  /**
   * 统驭科目（选项 TaktAccountTitles/options?reconciliationOnly=true&amp;auxiliaryType=K；DictValue=AccountTitleCode）
   */
  reconciliationAccount: string;

  /**
   * 客户（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode: string;

  /**
   * 具有客户的清算（字典 sys_yes_no；0=否 1=是）
   */
  clearingWithCustomer: number;

  /**
   * 付款方式（字典 accounting_financial_payment_method；0=现金，1=银行转账，2=支票，3=信用证，4=其他）
   */
  paymentMethod: number;

  /**
   * 付款条件（字典 accounting_financial_payment_terms_param；DictValue=prepayship/cod/net30 等）
   */
  paymentTerms: string;

  /**
   * 银行代码（选项 TaktBanks/options；DictValue=BankCode）
   */
  bankCode: string;

  /**
   * 银行帐号
   */
  bankAccount: string;

  /**
   * 帐户持有人
   */
  accountHolder: string;

  /**
   * 基于收货的发票验证（字典 sys_yes_no；0=否 1=是）
   */
  grBasedInvoiceInspection: number;

  /**
   * 国际贸易条件1（字典 logistics_sales_incoterms1；CFR/CIF/…/FOB 等；默认 FOB）
   */
  incoterms1: string;

  /**
   * 国际贸易条件2（地点说明）
   */
  incoterms2: string;

  /**
   * 自动产生的采购订单（字典 sys_yes_no；0=否 1=是）
   */
  automaticPurchaseOrder: number;

  /**
   * 定价日期控制（字典 logistics_procurement_pricing_date_control；1=订单日期，2=交货日期，3=当前日期，4=手动，5=收货日期；默认 1）
   */
  pricingDateControl: number;

  /**
   * 采购组（选项 TaktPurchaseGroups/options；DictValue=PurchaseGroupCode）
   */
  purchaseGroup: string;

  /**
   * 计划交货时间（天）
   */
  plannedDeliveryTimeDays: number;

  /**
   * 评估收据结算（字典 sys_yes_no；0=否 1=是）
   */
  evaluatedReceiptSettlement: number;

  /**
   * 采购组织（选项 TaktPlants/options；DictValue=PlantCode）
   */
  purchasingOrganization: string;

  /**
   * 供货商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时）
   */
  supplierLevel: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore: number;

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


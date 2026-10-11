// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：vendor.types.ts
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
 * Takt经销商实体
 * 对应前端 TaktVendorDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Vendor
 * @description 对应后端 TaktVendorDto
 */
export interface Vendor extends CompanyDtoBase {
  /**
   * VendorID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  vendorId: string;

  /**
   * 经销商编码（唯一索引）
   */
  vendorCode: string;

  /**
   * 经销商名称1
   */
  vendorName1: string;

  /**
   * 经销商名称2
   */
  vendorName2?: string;

  /**
   * 经销商简称
   */
  vendorShortName?: string;

  /**
   * 经销商类型（字典 logistics_sales_vendor_category；0=授权经销商，1=一般经销商，2=代理商，3=零售商，4=其他）
   */
  vendorType: number;

  /**
   * 企业性质（字典 sys_enterprise_nature）
   */
  enterpriseNature: string;

  /**
   * 行业属性（字典 sys_industry_attribute）
   */
  industryAttribute: string;

  /**
   * 经销商标识（税务登记证号/统一社会信用代码）
   */
  vendorTaxNumber?: string;

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
   * 经销商电话
   */
  vendorPhone?: string;

  /**
   * 经销商传真
   */
  vendorFax?: string;

  /**
   * 经销商邮箱
   */
  vendorEmail?: string;

  /**
   * 经销商网站
   */
  vendorWebsite?: string;

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
   * 信用等级（字典 logistics_sales_credit_rating；0=无，1=A级，2=AA级，3=AAA级，4=B级，5=C级）
   */
  creditLevel: number;

  /**
   * 信用额度（精确到分，存储为整数，单位为分）
   */
  creditAmount: number;

  /**
   * 授权品牌
   */
  authorizedBrand?: string;

  /**
   * 代理区域
   */
  agentRegion?: string;

  /**
   * 经销商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时；业务「核心」对应档位 1）
   */
  vendorLevel: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore: number;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

  /**
   * 经销商状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  vendorStatus: number;

}


/**
 * Vendor 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 VendorQuery
 * @description 对应后端 TaktVendorQueryDto
 */
export interface VendorQuery extends TaktPagedQuery {
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
   * 经销商编码（唯一索引）
   */
  vendorCode?: string;

  /**
   * 经销商名称1
   */
  vendorName1?: string;

  /**
   * 经销商名称2
   */
  vendorName2?: string;

  /**
   * 经销商简称
   */
  vendorShortName?: string;

  /**
   * 经销商类型（字典 logistics_sales_vendor_category；0=授权经销商，1=一般经销商，2=代理商，3=零售商，4=其他）
   */
  vendorType?: number;

  /**
   * 企业性质（字典 sys_enterprise_nature）
   */
  enterpriseNature?: string;

  /**
   * 行业属性（字典 sys_industry_attribute）
   */
  industryAttribute?: string;

  /**
   * 经销商标识（税务登记证号/统一社会信用代码）
   */
  vendorTaxNumber?: string;

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
   * 经销商电话
   */
  vendorPhone?: string;

  /**
   * 经销商传真
   */
  vendorFax?: string;

  /**
   * 经销商邮箱
   */
  vendorEmail?: string;

  /**
   * 经销商网站
   */
  vendorWebsite?: string;

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
   * 信用等级（字典 logistics_sales_credit_rating；0=无，1=A级，2=AA级，3=AAA级，4=B级，5=C级）
   */
  creditLevel?: number;

  /**
   * 信用额度（精确到分，存储为整数，单位为分）
   */
  creditAmount?: number;

  /**
   * 授权品牌
   */
  authorizedBrand?: string;

  /**
   * 代理区域
   */
  agentRegion?: string;

  /**
   * 经销商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时；业务「核心」对应档位 1）
   */
  vendorLevel?: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore?: number;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder?: number;

  /**
   * 经销商状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  vendorStatus?: number;

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
 * 创建Vendor DTO
 * 对应前端 VendorCreate
 * @description 对应后端 TaktVendorCreateDto
 */
export interface VendorCreate {
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
   * 经销商编码（唯一索引）
   */
  vendorCode: string;

  /**
   * 经销商名称1
   */
  vendorName1: string;

  /**
   * 经销商名称2
   */
  vendorName2?: string;

  /**
   * 经销商简称
   */
  vendorShortName?: string;

  /**
   * 经销商类型（字典 logistics_sales_vendor_category；0=授权经销商，1=一般经销商，2=代理商，3=零售商，4=其他）
   */
  vendorType: number;

  /**
   * 企业性质（字典 sys_enterprise_nature）
   */
  enterpriseNature: string;

  /**
   * 行业属性（字典 sys_industry_attribute）
   */
  industryAttribute: string;

  /**
   * 经销商标识（税务登记证号/统一社会信用代码）
   */
  vendorTaxNumber?: string;

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
   * 经销商电话
   */
  vendorPhone?: string;

  /**
   * 经销商传真
   */
  vendorFax?: string;

  /**
   * 经销商邮箱
   */
  vendorEmail?: string;

  /**
   * 经销商网站
   */
  vendorWebsite?: string;

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
   * 信用等级（字典 logistics_sales_credit_rating；0=无，1=A级，2=AA级，3=AAA级，4=B级，5=C级）
   */
  creditLevel: number;

  /**
   * 信用额度（精确到分，存储为整数，单位为分）
   */
  creditAmount: number;

  /**
   * 授权品牌
   */
  authorizedBrand?: string;

  /**
   * 代理区域
   */
  agentRegion?: string;

  /**
   * 经销商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时；业务「核心」对应档位 1）
   */
  vendorLevel: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore: number;

  /**
   * 经销商状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  vendorStatus: number;

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
 * 更新Vendor DTO
 * 继承 TaktVendorCreateDto，添加 VendorId 字段
 * 对应前端 VendorUpdate
 * @description 对应后端 TaktVendorUpdateDto
 */
export interface VendorUpdate extends VendorCreate {
  /**
   * VendorID（标识要更新的实体）
   */
  vendorId: string;

}


/**
 * Vendor 状态更新 DTO
 * 对应前端 VendorStatus
 * @description 对应后端 TaktVendorStatusDto
 */
export interface VendorStatus {
  /**
   * VendorID
   */
  vendorId: string;

  /**
   * 经销商状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  vendorStatus: number;

}


/**
 * Vendor 排序更新 DTO
 * 对应前端 VendorSort
 * @description 对应后端 TaktVendorSortDto
 */
export interface VendorSort {
  /**
   * VendorID
   */
  vendorId: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

}


/**
 * Vendor 导入模板行 DTO
 * 对应前端 VendorTemplate
 * @description 对应后端 TaktVendorTemplateDto
 */
export interface VendorTemplate {
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
   * 经销商编码（唯一索引）
   */
  vendorCode?: string;

  /**
   * 经销商名称1
   */
  vendorName1?: string;

  /**
   * 经销商名称2
   */
  vendorName2?: string;

  /**
   * 经销商简称
   */
  vendorShortName?: string;

  /**
   * 经销商类型（字典 logistics_sales_vendor_category；0=授权经销商，1=一般经销商，2=代理商，3=零售商，4=其他）
   */
  vendorType?: number;

  /**
   * 企业性质（字典 sys_enterprise_nature）
   */
  enterpriseNature?: string;

  /**
   * 行业属性（字典 sys_industry_attribute）
   */
  industryAttribute?: string;

  /**
   * 经销商标识（税务登记证号/统一社会信用代码）
   */
  vendorTaxNumber?: string;

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
   * 经销商电话
   */
  vendorPhone?: string;

  /**
   * 经销商传真
   */
  vendorFax?: string;

  /**
   * 经销商邮箱
   */
  vendorEmail?: string;

  /**
   * 经销商网站
   */
  vendorWebsite?: string;

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
   * 信用等级（字典 logistics_sales_credit_rating；0=无，1=A级，2=AA级，3=AAA级，4=B级，5=C级）
   */
  creditLevel?: number;

  /**
   * 信用额度（精确到分，存储为整数，单位为分）
   */
  creditAmount?: number;

  /**
   * 授权品牌
   */
  authorizedBrand?: string;

  /**
   * 代理区域
   */
  agentRegion?: string;

  /**
   * 经销商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时；业务「核心」对应档位 1）
   */
  vendorLevel?: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore?: number;

  /**
   * 经销商状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  vendorStatus?: number;

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
 * Vendor 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 VendorImport
 * @description 对应后端 TaktVendorImportDto
 */
export interface VendorImport {
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
   * 经销商编码（唯一索引）
   */
  vendorCode?: string;

  /**
   * 经销商名称1
   */
  vendorName1?: string;

  /**
   * 经销商名称2
   */
  vendorName2?: string;

  /**
   * 经销商简称
   */
  vendorShortName?: string;

  /**
   * 经销商类型（字典 logistics_sales_vendor_category；0=授权经销商，1=一般经销商，2=代理商，3=零售商，4=其他）
   */
  vendorType?: number;

  /**
   * 企业性质（字典 sys_enterprise_nature）
   */
  enterpriseNature?: string;

  /**
   * 行业属性（字典 sys_industry_attribute）
   */
  industryAttribute?: string;

  /**
   * 经销商标识（税务登记证号/统一社会信用代码）
   */
  vendorTaxNumber?: string;

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
   * 经销商电话
   */
  vendorPhone?: string;

  /**
   * 经销商传真
   */
  vendorFax?: string;

  /**
   * 经销商邮箱
   */
  vendorEmail?: string;

  /**
   * 经销商网站
   */
  vendorWebsite?: string;

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
   * 信用等级（字典 logistics_sales_credit_rating；0=无，1=A级，2=AA级，3=AAA级，4=B级，5=C级）
   */
  creditLevel?: number;

  /**
   * 信用额度（精确到分，存储为整数，单位为分）
   */
  creditAmount?: number;

  /**
   * 授权品牌
   */
  authorizedBrand?: string;

  /**
   * 代理区域
   */
  agentRegion?: string;

  /**
   * 经销商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时；业务「核心」对应档位 1）
   */
  vendorLevel?: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore?: number;

  /**
   * 经销商状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  vendorStatus?: number;

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
 * Vendor 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 VendorExport
 * @description 对应后端 TaktVendorExportDto
 */
export interface VendorExport {
  /**
   * VendorID
   */
  vendorId: string;

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
   * 经销商编码（唯一索引）
   */
  vendorCode: string;

  /**
   * 经销商名称1
   */
  vendorName1: string;

  /**
   * 经销商名称2
   */
  vendorName2?: string;

  /**
   * 经销商简称
   */
  vendorShortName?: string;

  /**
   * 经销商类型（字典 logistics_sales_vendor_category；0=授权经销商，1=一般经销商，2=代理商，3=零售商，4=其他）
   */
  vendorType: number;

  /**
   * 企业性质（字典 sys_enterprise_nature）
   */
  enterpriseNature: string;

  /**
   * 行业属性（字典 sys_industry_attribute）
   */
  industryAttribute: string;

  /**
   * 经销商标识（税务登记证号/统一社会信用代码）
   */
  vendorTaxNumber?: string;

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
   * 经销商电话
   */
  vendorPhone?: string;

  /**
   * 经销商传真
   */
  vendorFax?: string;

  /**
   * 经销商邮箱
   */
  vendorEmail?: string;

  /**
   * 经销商网站
   */
  vendorWebsite?: string;

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
   * 信用等级（字典 logistics_sales_credit_rating；0=无，1=A级，2=AA级，3=AAA级，4=B级，5=C级）
   */
  creditLevel: number;

  /**
   * 信用额度（精确到分，存储为整数，单位为分）
   */
  creditAmount: number;

  /**
   * 授权品牌
   */
  authorizedBrand?: string;

  /**
   * 代理区域
   */
  agentRegion?: string;

  /**
   * 经销商等级（字典 logistics_sales_grade；0=普通，1=优选，2=战略，3=临时；业务「核心」对应档位 1）
   */
  vendorLevel: number;

  /**
   * 评价分数（0-100分）
   */
  evaluationScore: number;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

  /**
   * 经销商状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  vendorStatus: number;

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


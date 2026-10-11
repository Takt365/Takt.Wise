// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：invoice.types.ts
// 创建时间：2026-09-04
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/sales 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * Takt销售发票主表实体（公司级）
 * 对应前端 TaktSalesInvoiceDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalesInvoice
 * @description 对应后端 TaktSalesInvoiceDto
 */
export interface SalesInvoice extends CompanyDtoBase {
  /**
   * SalesInvoiceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  salesInvoiceId: string;

  /**
   * 开票凭证
   */
  billingDocumentCode: string;

  /**
   * 开票类型
   */
  billingType?: string;

  /**
   * 出具发票类别
   */
  billingCategory?: string;

  /**
   * SD 凭证类别
   */
  documentCategory?: string;

  /**
   * 凭证货币（字典 accounting_financial_currency_code）
   */
  currencyCode: string;

  /**
   * 销售组织
   */
  salesOrganization?: string;

  /**
   * 分销渠道
   */
  distributionChannel?: string;

  /**
   * 定价过程
   */
  pricingProcedure?: string;

  /**
   * 单据条件号
   */
  conditionCode?: string;

  /**
   * 装运条件（字典 logistics_sales_shipping_conditions）
   */
  shippingConditions?: string;

  /**
   * 出具发票日期
   */
  billingDate: string;

  /**
   * 客户组
   */
  customerGroup?: string;

  /**
   * 国际贸易条件
   */
  incoterms1?: string;

  /**
   * 国际贸易条件(部分2)（最长 28，故 Length=28）
   */
  incoterms2?: string;

  /**
   * 过账状态
   */
  postingStatus?: string;

  /**
   * 会计汇率
   */
  accountingExchangeRate?: number;

  /**
   * 付款条件
   */
  paymentTerms?: string;

  /**
   * 客户分配帐户组别
   */
  accountAssignmentGroup?: string;

  /**
   * 目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode?: string;

  /**
   * 净价值
   */
  netAmount: number;

  /**
   * 付款方（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  payerCode?: string;

  /**
   * 售达方（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode: string;

  /**
   * 统计货币（字典 accounting_financial_currency_code）
   */
  statisticsCurrencyCode?: string;

  /**
   * 外贸数据编号
   */
  foreignTradeCode?: string;

  /**
   * 已取消的开票凭证
   */
  cancelledBillingDocument?: string;

  /**
   * 发票清单类型
   */
  invoiceListType?: string;

  /**
   * 产品组
   */
  division?: string;

  /**
   * 定价的层次类型
   */
  hierarchyTypePricing?: string;

  /**
   * 贸易伙伴
   */
  tradingPartner?: string;

  /**
   * 征税国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  taxDepartureCountry?: string;

  /**
   * 组织销售税编号
   */
  organizationSalesTaxNumber?: string;

  /**
   * 国家销售税编号
   */
  countrySalesTaxNumber?: string;

  /**
   * 参考（最长 16，故 Length=16）
   */
  referenceCode?: string;

  /**
   * 已被取消
   */
  cancelledFlag?: string;

  /**
   * 换算日期
   */
  exchangeRateDate?: string;

  /**
   * 付款参考（最长 30，故 Length=30）
   */
  paymentReference?: string;

  /**
   * 冲销原因
   */
  reversalReason?: string;

  /**
   * 过账人（当前登录用户对应的 EmployeeCode）
   */
  postedBy?: string;

  /**
   * 销售发票明细列表（主子表关系） （子表：TaktSalesInvoiceItem）
   */
  items?: SalesInvoiceItem[];

}


/**
 * SalesInvoice 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SalesInvoiceQuery
 * @description 对应后端 TaktSalesInvoiceQueryDto
 */
export interface SalesInvoiceQuery extends TaktPagedQuery {
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
   * 开票凭证
   */
  billingDocumentCode?: string;

  /**
   * 开票类型
   */
  billingType?: string;

  /**
   * 出具发票类别
   */
  billingCategory?: string;

  /**
   * SD 凭证类别
   */
  documentCategory?: string;

  /**
   * 凭证货币（字典 accounting_financial_currency_code）
   */
  currencyCode?: string;

  /**
   * 销售组织
   */
  salesOrganization?: string;

  /**
   * 分销渠道
   */
  distributionChannel?: string;

  /**
   * 定价过程
   */
  pricingProcedure?: string;

  /**
   * 单据条件号
   */
  conditionCode?: string;

  /**
   * 装运条件（字典 logistics_sales_shipping_conditions）
   */
  shippingConditions?: string;

  /**
   * 出具发票日期（范围查询-开始）
   */
  billingDateStart?: string;

  /**
   * 出具发票日期（范围查询-结束）
   */
  billingDateEnd?: string;

  /**
   * 客户组
   */
  customerGroup?: string;

  /**
   * 国际贸易条件
   */
  incoterms1?: string;

  /**
   * 国际贸易条件(部分2)（最长 28，故 Length=28）
   */
  incoterms2?: string;

  /**
   * 过账状态
   */
  postingStatus?: string;

  /**
   * 会计汇率
   */
  accountingExchangeRate?: number;

  /**
   * 付款条件
   */
  paymentTerms?: string;

  /**
   * 客户分配帐户组别
   */
  accountAssignmentGroup?: string;

  /**
   * 目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode?: string;

  /**
   * 净价值
   */
  netAmount?: number;

  /**
   * 付款方（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  payerCode?: string;

  /**
   * 售达方（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 统计货币（字典 accounting_financial_currency_code）
   */
  statisticsCurrencyCode?: string;

  /**
   * 外贸数据编号
   */
  foreignTradeCode?: string;

  /**
   * 已取消的开票凭证
   */
  cancelledBillingDocument?: string;

  /**
   * 发票清单类型
   */
  invoiceListType?: string;

  /**
   * 产品组
   */
  division?: string;

  /**
   * 定价的层次类型
   */
  hierarchyTypePricing?: string;

  /**
   * 贸易伙伴
   */
  tradingPartner?: string;

  /**
   * 征税国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  taxDepartureCountry?: string;

  /**
   * 组织销售税编号
   */
  organizationSalesTaxNumber?: string;

  /**
   * 国家销售税编号
   */
  countrySalesTaxNumber?: string;

  /**
   * 参考（最长 16，故 Length=16）
   */
  referenceCode?: string;

  /**
   * 已被取消
   */
  cancelledFlag?: string;

  /**
   * 换算日期（范围查询-开始）
   */
  exchangeRateDateStart?: string;

  /**
   * 换算日期（范围查询-结束）
   */
  exchangeRateDateEnd?: string;

  /**
   * 付款参考（最长 30，故 Length=30）
   */
  paymentReference?: string;

  /**
   * 冲销原因
   */
  reversalReason?: string;

  /**
   * 过账人（当前登录用户对应的 EmployeeCode）
   */
  postedBy?: string;

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
 * 创建SalesInvoice DTO
 * 对应前端 SalesInvoiceCreate
 * @description 对应后端 TaktSalesInvoiceCreateDto
 */
export interface SalesInvoiceCreate {
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
   * 开票凭证
   */
  billingDocumentCode: string;

  /**
   * 开票类型
   */
  billingType?: string;

  /**
   * 出具发票类别
   */
  billingCategory?: string;

  /**
   * SD 凭证类别
   */
  documentCategory?: string;

  /**
   * 凭证货币（字典 accounting_financial_currency_code）
   */
  currencyCode: string;

  /**
   * 销售组织
   */
  salesOrganization?: string;

  /**
   * 分销渠道
   */
  distributionChannel?: string;

  /**
   * 定价过程
   */
  pricingProcedure?: string;

  /**
   * 单据条件号
   */
  conditionCode?: string;

  /**
   * 装运条件（字典 logistics_sales_shipping_conditions）
   */
  shippingConditions?: string;

  /**
   * 出具发票日期
   */
  billingDate: string;

  /**
   * 客户组
   */
  customerGroup?: string;

  /**
   * 国际贸易条件
   */
  incoterms1?: string;

  /**
   * 国际贸易条件(部分2)（最长 28，故 Length=28）
   */
  incoterms2?: string;

  /**
   * 过账状态
   */
  postingStatus?: string;

  /**
   * 会计汇率
   */
  accountingExchangeRate?: number;

  /**
   * 付款条件
   */
  paymentTerms?: string;

  /**
   * 客户分配帐户组别
   */
  accountAssignmentGroup?: string;

  /**
   * 目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode?: string;

  /**
   * 净价值
   */
  netAmount: number;

  /**
   * 付款方（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  payerCode?: string;

  /**
   * 售达方（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode: string;

  /**
   * 统计货币（字典 accounting_financial_currency_code）
   */
  statisticsCurrencyCode?: string;

  /**
   * 外贸数据编号
   */
  foreignTradeCode?: string;

  /**
   * 已取消的开票凭证
   */
  cancelledBillingDocument?: string;

  /**
   * 发票清单类型
   */
  invoiceListType?: string;

  /**
   * 产品组
   */
  division?: string;

  /**
   * 定价的层次类型
   */
  hierarchyTypePricing?: string;

  /**
   * 贸易伙伴
   */
  tradingPartner?: string;

  /**
   * 征税国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  taxDepartureCountry?: string;

  /**
   * 组织销售税编号
   */
  organizationSalesTaxNumber?: string;

  /**
   * 国家销售税编号
   */
  countrySalesTaxNumber?: string;

  /**
   * 参考（最长 16，故 Length=16）
   */
  referenceCode?: string;

  /**
   * 已被取消
   */
  cancelledFlag?: string;

  /**
   * 换算日期
   */
  exchangeRateDate?: string;

  /**
   * 付款参考（最长 30，故 Length=30）
   */
  paymentReference?: string;

  /**
   * 冲销原因
   */
  reversalReason?: string;

  /**
   * 过账人（当前登录用户对应的 EmployeeCode）
   */
  postedBy?: string;

  /**
   * 销售发票明细列表（主子表关系）（子表，级联保存）
   */
  items?: SalesInvoiceItemCreate[];

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
 * 更新SalesInvoice DTO
 * 继承 TaktSalesInvoiceCreateDto，添加 SalesInvoiceId 字段
 * 对应前端 SalesInvoiceUpdate
 * @description 对应后端 TaktSalesInvoiceUpdateDto
 */
export interface SalesInvoiceUpdate extends SalesInvoiceCreate {
  /**
   * SalesInvoiceID（标识要更新的实体）
   */
  salesInvoiceId: string;

  /**
   * 销售发票明细列表（主子表关系）（子表，级联保存）
   */
  items?: any;

}


/**
 * SalesInvoice 状态更新 DTO
 * 对应前端 SalesInvoiceStatus
 * @description 对应后端 TaktSalesInvoiceStatusDto
 */
export interface SalesInvoiceStatus {
  /**
   * SalesInvoiceID
   */
  salesInvoiceId: string;

  /**
   * 过账状态
   */
  postingStatus: string;

}


/**
 * SalesInvoice 导入模板行 DTO
 * 对应前端 SalesInvoiceTemplate
 * @description 对应后端 TaktSalesInvoiceTemplateDto
 */
export interface SalesInvoiceTemplate {
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
   * 开票凭证
   */
  billingDocumentCode?: string;

  /**
   * 开票类型
   */
  billingType?: string;

  /**
   * 出具发票类别
   */
  billingCategory?: string;

  /**
   * SD 凭证类别
   */
  documentCategory?: string;

  /**
   * 凭证货币（字典 accounting_financial_currency_code）
   */
  currencyCode?: string;

  /**
   * 销售组织
   */
  salesOrganization?: string;

  /**
   * 分销渠道
   */
  distributionChannel?: string;

  /**
   * 定价过程
   */
  pricingProcedure?: string;

  /**
   * 单据条件号
   */
  conditionCode?: string;

  /**
   * 装运条件（字典 logistics_sales_shipping_conditions）
   */
  shippingConditions?: string;

  /**
   * 出具发票日期
   */
  billingDate?: string;

  /**
   * 客户组
   */
  customerGroup?: string;

  /**
   * 国际贸易条件
   */
  incoterms1?: string;

  /**
   * 国际贸易条件(部分2)（最长 28，故 Length=28）
   */
  incoterms2?: string;

  /**
   * 过账状态
   */
  postingStatus?: string;

  /**
   * 会计汇率
   */
  accountingExchangeRate?: number;

  /**
   * 付款条件
   */
  paymentTerms?: string;

  /**
   * 客户分配帐户组别
   */
  accountAssignmentGroup?: string;

  /**
   * 目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode?: string;

  /**
   * 净价值
   */
  netAmount?: number;

  /**
   * 付款方（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  payerCode?: string;

  /**
   * 售达方（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 统计货币（字典 accounting_financial_currency_code）
   */
  statisticsCurrencyCode?: string;

  /**
   * 外贸数据编号
   */
  foreignTradeCode?: string;

  /**
   * 已取消的开票凭证
   */
  cancelledBillingDocument?: string;

  /**
   * 发票清单类型
   */
  invoiceListType?: string;

  /**
   * 产品组
   */
  division?: string;

  /**
   * 定价的层次类型
   */
  hierarchyTypePricing?: string;

  /**
   * 贸易伙伴
   */
  tradingPartner?: string;

  /**
   * 征税国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  taxDepartureCountry?: string;

  /**
   * 组织销售税编号
   */
  organizationSalesTaxNumber?: string;

  /**
   * 国家销售税编号
   */
  countrySalesTaxNumber?: string;

  /**
   * 参考（最长 16，故 Length=16）
   */
  referenceCode?: string;

  /**
   * 已被取消
   */
  cancelledFlag?: string;

  /**
   * 换算日期
   */
  exchangeRateDate?: string;

  /**
   * 付款参考（最长 30，故 Length=30）
   */
  paymentReference?: string;

  /**
   * 冲销原因
   */
  reversalReason?: string;

  /**
   * 过账人（当前登录用户对应的 EmployeeCode）
   */
  postedBy?: string;

  /**
   * 销售发票明细列表（主子表关系）（子表，级联保存）
   */
  items?: SalesInvoiceItemCreate[];

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
 * SalesInvoice 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SalesInvoiceImport
 * @description 对应后端 TaktSalesInvoiceImportDto
 */
export interface SalesInvoiceImport {
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
   * 开票凭证
   */
  billingDocumentCode?: string;

  /**
   * 开票类型
   */
  billingType?: string;

  /**
   * 出具发票类别
   */
  billingCategory?: string;

  /**
   * SD 凭证类别
   */
  documentCategory?: string;

  /**
   * 凭证货币（字典 accounting_financial_currency_code）
   */
  currencyCode?: string;

  /**
   * 销售组织
   */
  salesOrganization?: string;

  /**
   * 分销渠道
   */
  distributionChannel?: string;

  /**
   * 定价过程
   */
  pricingProcedure?: string;

  /**
   * 单据条件号
   */
  conditionCode?: string;

  /**
   * 装运条件（字典 logistics_sales_shipping_conditions）
   */
  shippingConditions?: string;

  /**
   * 出具发票日期
   */
  billingDate?: string;

  /**
   * 客户组
   */
  customerGroup?: string;

  /**
   * 国际贸易条件
   */
  incoterms1?: string;

  /**
   * 国际贸易条件(部分2)（最长 28，故 Length=28）
   */
  incoterms2?: string;

  /**
   * 过账状态
   */
  postingStatus?: string;

  /**
   * 会计汇率
   */
  accountingExchangeRate?: number;

  /**
   * 付款条件
   */
  paymentTerms?: string;

  /**
   * 客户分配帐户组别
   */
  accountAssignmentGroup?: string;

  /**
   * 目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode?: string;

  /**
   * 净价值
   */
  netAmount?: number;

  /**
   * 付款方（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  payerCode?: string;

  /**
   * 售达方（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode?: string;

  /**
   * 统计货币（字典 accounting_financial_currency_code）
   */
  statisticsCurrencyCode?: string;

  /**
   * 外贸数据编号
   */
  foreignTradeCode?: string;

  /**
   * 已取消的开票凭证
   */
  cancelledBillingDocument?: string;

  /**
   * 发票清单类型
   */
  invoiceListType?: string;

  /**
   * 产品组
   */
  division?: string;

  /**
   * 定价的层次类型
   */
  hierarchyTypePricing?: string;

  /**
   * 贸易伙伴
   */
  tradingPartner?: string;

  /**
   * 征税国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  taxDepartureCountry?: string;

  /**
   * 组织销售税编号
   */
  organizationSalesTaxNumber?: string;

  /**
   * 国家销售税编号
   */
  countrySalesTaxNumber?: string;

  /**
   * 参考（最长 16，故 Length=16）
   */
  referenceCode?: string;

  /**
   * 已被取消
   */
  cancelledFlag?: string;

  /**
   * 换算日期
   */
  exchangeRateDate?: string;

  /**
   * 付款参考（最长 30，故 Length=30）
   */
  paymentReference?: string;

  /**
   * 冲销原因
   */
  reversalReason?: string;

  /**
   * 过账人（当前登录用户对应的 EmployeeCode）
   */
  postedBy?: string;

  /**
   * 销售发票明细列表（主子表关系）（子表，级联保存）
   */
  items?: SalesInvoiceItemCreate[];

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
 * SalesInvoice 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalesInvoiceExport
 * @description 对应后端 TaktSalesInvoiceExportDto
 */
export interface SalesInvoiceExport {
  /**
   * SalesInvoiceID
   */
  salesInvoiceId: string;

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
   * 开票凭证
   */
  billingDocumentCode: string;

  /**
   * 开票类型
   */
  billingType?: string;

  /**
   * 出具发票类别
   */
  billingCategory?: string;

  /**
   * SD 凭证类别
   */
  documentCategory?: string;

  /**
   * 凭证货币（字典 accounting_financial_currency_code）
   */
  currencyCode: string;

  /**
   * 销售组织
   */
  salesOrganization?: string;

  /**
   * 分销渠道
   */
  distributionChannel?: string;

  /**
   * 定价过程
   */
  pricingProcedure?: string;

  /**
   * 单据条件号
   */
  conditionCode?: string;

  /**
   * 装运条件（字典 logistics_sales_shipping_conditions）
   */
  shippingConditions?: string;

  /**
   * 出具发票日期
   */
  billingDate: string;

  /**
   * 客户组
   */
  customerGroup?: string;

  /**
   * 国际贸易条件
   */
  incoterms1?: string;

  /**
   * 国际贸易条件(部分2)（最长 28，故 Length=28）
   */
  incoterms2?: string;

  /**
   * 过账状态
   */
  postingStatus?: string;

  /**
   * 会计汇率
   */
  accountingExchangeRate?: number;

  /**
   * 付款条件
   */
  paymentTerms?: string;

  /**
   * 客户分配帐户组别
   */
  accountAssignmentGroup?: string;

  /**
   * 目的地国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode?: string;

  /**
   * 净价值
   */
  netAmount: number;

  /**
   * 付款方（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  payerCode?: string;

  /**
   * 售达方（选项 TaktCustomers/options；DictValue=CustomerCode）
   */
  customerCode: string;

  /**
   * 统计货币（字典 accounting_financial_currency_code）
   */
  statisticsCurrencyCode?: string;

  /**
   * 外贸数据编号
   */
  foreignTradeCode?: string;

  /**
   * 已取消的开票凭证
   */
  cancelledBillingDocument?: string;

  /**
   * 发票清单类型
   */
  invoiceListType?: string;

  /**
   * 产品组
   */
  division?: string;

  /**
   * 定价的层次类型
   */
  hierarchyTypePricing?: string;

  /**
   * 贸易伙伴
   */
  tradingPartner?: string;

  /**
   * 征税国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  taxDepartureCountry?: string;

  /**
   * 组织销售税编号
   */
  organizationSalesTaxNumber?: string;

  /**
   * 国家销售税编号
   */
  countrySalesTaxNumber?: string;

  /**
   * 参考（最长 16，故 Length=16）
   */
  referenceCode?: string;

  /**
   * 已被取消
   */
  cancelledFlag?: string;

  /**
   * 换算日期
   */
  exchangeRateDate?: string;

  /**
   * 付款参考（最长 30，故 Length=30）
   */
  paymentReference?: string;

  /**
   * 冲销原因
   */
  reversalReason?: string;

  /**
   * 过账人（当前登录用户对应的 EmployeeCode）
   */
  postedBy?: string;

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


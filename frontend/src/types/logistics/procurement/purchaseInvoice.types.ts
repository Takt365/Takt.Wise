// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchaseInvoice.types.ts
// 创建时间：2026-09-04
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
 * Takt采购发票主表实体（公司级；字段按 RBKP 业务清单）
 * 对应前端 TaktPurchaseInvoiceDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchaseInvoice
 * @description 对应后端 TaktPurchaseInvoiceDto
 */
export interface PurchaseInvoice extends CompanyDtoBase {
  /**
   * PurchaseInvoiceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  purchaseInvoiceId: string;

  /**
   * 发票凭证编号
   */
  purchaseInvoiceCode: string;

  /**
   * 会计年度
   */
  fiscalYear: string;

  /**
   * 凭证类型（字典 logistics_purchase_invoice_document_type）
   */
  documentType?: string;

  /**
   * 凭证日期
   */
  documentDate: string;

  /**
   * 过帐日期
   */
  postingDate: string;

  /**
   * 交易类型（字典 logistics_purchase_invoice_transaction_event_type）
   */
  transactionEventType?: string;

  /**
   * 参照
   */
  referenceCode?: string;

  /**
   * 出票方（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 货币（字典 accounting_financial_currency_code）
   */
  currencyCode: string;

  /**
   * 汇率
   */
  exchangeRate?: number;

  /**
   * 总发票金额
   */
  grossAmount: number;

  /**
   * 增值税金额
   */
  vatAmount?: number;

  /**
   * 税务代码
   */
  taxJurisdictionCode?: string;

  /**
   * 天数 1（现金折扣天数）
   */
  cashDiscountDays1?: number;

  /**
   * 发票
   */
  invoiceFlag?: string;

  /**
   * 凭证抬头文本
   */
  headerText?: string;

  /**
   * 冲销者（冲销凭证编号）
   */
  reversalDocumentCode?: string;

  /**
   * 年（冲销会计年度）
   */
  reversalFiscalYear?: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 供货国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  supplyingCountry?: string;

  /**
   * 税率（税务汇率）
   */
  taxExchangeRate?: number;

  /**
   * 付款基准日期
   */
  baselineDate?: string;

  /**
   * 换算日期
   */
  exchangeRateDate?: string;

  /**
   * 事务代码
   */
  transactionCode?: string;

  /**
   * 过账人（当前登录用户对应的 EmployeeCode）
   */
  postedBy?: string;

  /**
   * 采购发票明细列表（主子表关系） （子表：TaktPurchaseInvoiceItem）
   */
  items?: PurchaseInvoiceItem[];

}


/**
 * PurchaseInvoice 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PurchaseInvoiceQuery
 * @description 对应后端 TaktPurchaseInvoiceQueryDto
 */
export interface PurchaseInvoiceQuery extends TaktPagedQuery {
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
   * 发票凭证编号
   */
  purchaseInvoiceCode?: string;

  /**
   * 会计年度
   */
  fiscalYear?: string;

  /**
   * 凭证类型（字典 logistics_purchase_invoice_document_type）
   */
  documentType?: string;

  /**
   * 凭证日期（范围查询-开始）
   */
  documentDateStart?: string;

  /**
   * 凭证日期（范围查询-结束）
   */
  documentDateEnd?: string;

  /**
   * 过帐日期（范围查询-开始）
   */
  postingDateStart?: string;

  /**
   * 过帐日期（范围查询-结束）
   */
  postingDateEnd?: string;

  /**
   * 交易类型（字典 logistics_purchase_invoice_transaction_event_type）
   */
  transactionEventType?: string;

  /**
   * 参照
   */
  referenceCode?: string;

  /**
   * 出票方（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 货币（字典 accounting_financial_currency_code）
   */
  currencyCode?: string;

  /**
   * 汇率
   */
  exchangeRate?: number;

  /**
   * 总发票金额
   */
  grossAmount?: number;

  /**
   * 增值税金额
   */
  vatAmount?: number;

  /**
   * 税务代码
   */
  taxJurisdictionCode?: string;

  /**
   * 天数 1（现金折扣天数）
   */
  cashDiscountDays1?: number;

  /**
   * 发票
   */
  invoiceFlag?: string;

  /**
   * 凭证抬头文本
   */
  headerText?: string;

  /**
   * 冲销者（冲销凭证编号）
   */
  reversalDocumentCode?: string;

  /**
   * 年（冲销会计年度）
   */
  reversalFiscalYear?: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 供货国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  supplyingCountry?: string;

  /**
   * 税率（税务汇率）
   */
  taxExchangeRate?: number;

  /**
   * 付款基准日期（范围查询-开始）
   */
  baselineDateStart?: string;

  /**
   * 付款基准日期（范围查询-结束）
   */
  baselineDateEnd?: string;

  /**
   * 换算日期（范围查询-开始）
   */
  exchangeRateDateStart?: string;

  /**
   * 换算日期（范围查询-结束）
   */
  exchangeRateDateEnd?: string;

  /**
   * 事务代码
   */
  transactionCode?: string;

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
 * 创建PurchaseInvoice DTO
 * 对应前端 PurchaseInvoiceCreate
 * @description 对应后端 TaktPurchaseInvoiceCreateDto
 */
export interface PurchaseInvoiceCreate {
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
   * 发票凭证编号（前端表单选择编码规则后自动通过 TaktNumbering 发票凭证编号规则生成并展示，非手输；单据类型菜单：采购发票）
   */
  purchaseInvoiceCode: string;

  /**
   * 编码规则编码（新增时选择采购发票编码规则后取号；不落库）
   */
  numberingRuleCode?: string;

  /**
   * 会计年度
   */
  fiscalYear: string;

  /**
   * 凭证类型（字典 logistics_purchase_invoice_document_type）
   */
  documentType?: string;

  /**
   * 凭证日期
   */
  documentDate: string;

  /**
   * 过帐日期
   */
  postingDate: string;

  /**
   * 交易类型（字典 logistics_purchase_invoice_transaction_event_type）
   */
  transactionEventType?: string;

  /**
   * 参照
   */
  referenceCode?: string;

  /**
   * 出票方（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 货币（字典 accounting_financial_currency_code）
   */
  currencyCode: string;

  /**
   * 汇率
   */
  exchangeRate?: number;

  /**
   * 总发票金额
   */
  grossAmount: number;

  /**
   * 增值税金额
   */
  vatAmount?: number;

  /**
   * 税务代码
   */
  taxJurisdictionCode?: string;

  /**
   * 天数 1（现金折扣天数）
   */
  cashDiscountDays1?: number;

  /**
   * 发票
   */
  invoiceFlag?: string;

  /**
   * 凭证抬头文本
   */
  headerText?: string;

  /**
   * 冲销者（冲销凭证编号）
   */
  reversalDocumentCode?: string;

  /**
   * 年（冲销会计年度）
   */
  reversalFiscalYear?: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 供货国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  supplyingCountry?: string;

  /**
   * 税率（税务汇率）
   */
  taxExchangeRate?: number;

  /**
   * 付款基准日期
   */
  baselineDate?: string;

  /**
   * 换算日期
   */
  exchangeRateDate?: string;

  /**
   * 事务代码
   */
  transactionCode?: string;

  /**
   * 过账人（当前登录用户对应的 EmployeeCode）
   */
  postedBy?: string;

  /**
   * 采购发票明细列表（主子表关系）（子表，级联保存）
   */
  items?: PurchaseInvoiceItemCreate[];

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
 * 更新PurchaseInvoice DTO
 * 继承 TaktPurchaseInvoiceCreateDto，添加 PurchaseInvoiceId 字段
 * 对应前端 PurchaseInvoiceUpdate
 * @description 对应后端 TaktPurchaseInvoiceUpdateDto
 */
export interface PurchaseInvoiceUpdate extends PurchaseInvoiceCreate {
  /**
   * PurchaseInvoiceID（标识要更新的实体）
   */
  purchaseInvoiceId: string;

  /**
   * 采购发票明细列表（主子表关系）（子表，级联保存）
   */
  items?: any;

}


/**
 * PurchaseInvoice 导入模板行 DTO
 * 对应前端 PurchaseInvoiceTemplate
 * @description 对应后端 TaktPurchaseInvoiceTemplateDto
 */
export interface PurchaseInvoiceTemplate {
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
   * 发票凭证编号
   */
  purchaseInvoiceCode?: string;

  /**
   * 会计年度
   */
  fiscalYear?: string;

  /**
   * 凭证类型（字典 logistics_purchase_invoice_document_type）
   */
  documentType?: string;

  /**
   * 凭证日期
   */
  documentDate?: string;

  /**
   * 过帐日期
   */
  postingDate?: string;

  /**
   * 交易类型（字典 logistics_purchase_invoice_transaction_event_type）
   */
  transactionEventType?: string;

  /**
   * 参照
   */
  referenceCode?: string;

  /**
   * 出票方（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 货币（字典 accounting_financial_currency_code）
   */
  currencyCode?: string;

  /**
   * 汇率
   */
  exchangeRate?: number;

  /**
   * 总发票金额
   */
  grossAmount?: number;

  /**
   * 增值税金额
   */
  vatAmount?: number;

  /**
   * 税务代码
   */
  taxJurisdictionCode?: string;

  /**
   * 天数 1（现金折扣天数）
   */
  cashDiscountDays1?: number;

  /**
   * 发票
   */
  invoiceFlag?: string;

  /**
   * 凭证抬头文本
   */
  headerText?: string;

  /**
   * 冲销者（冲销凭证编号）
   */
  reversalDocumentCode?: string;

  /**
   * 年（冲销会计年度）
   */
  reversalFiscalYear?: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 供货国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  supplyingCountry?: string;

  /**
   * 税率（税务汇率）
   */
  taxExchangeRate?: number;

  /**
   * 付款基准日期
   */
  baselineDate?: string;

  /**
   * 换算日期
   */
  exchangeRateDate?: string;

  /**
   * 事务代码
   */
  transactionCode?: string;

  /**
   * 过账人（当前登录用户对应的 EmployeeCode）
   */
  postedBy?: string;

  /**
   * 采购发票明细列表（主子表关系）（子表，级联保存）
   */
  items?: PurchaseInvoiceItemCreate[];

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
 * PurchaseInvoice 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PurchaseInvoiceImport
 * @description 对应后端 TaktPurchaseInvoiceImportDto
 */
export interface PurchaseInvoiceImport {
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
   * 发票凭证编号
   */
  purchaseInvoiceCode?: string;

  /**
   * 会计年度
   */
  fiscalYear?: string;

  /**
   * 凭证类型（字典 logistics_purchase_invoice_document_type）
   */
  documentType?: string;

  /**
   * 凭证日期
   */
  documentDate?: string;

  /**
   * 过帐日期
   */
  postingDate?: string;

  /**
   * 交易类型（字典 logistics_purchase_invoice_transaction_event_type）
   */
  transactionEventType?: string;

  /**
   * 参照
   */
  referenceCode?: string;

  /**
   * 出票方（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 货币（字典 accounting_financial_currency_code）
   */
  currencyCode?: string;

  /**
   * 汇率
   */
  exchangeRate?: number;

  /**
   * 总发票金额
   */
  grossAmount?: number;

  /**
   * 增值税金额
   */
  vatAmount?: number;

  /**
   * 税务代码
   */
  taxJurisdictionCode?: string;

  /**
   * 天数 1（现金折扣天数）
   */
  cashDiscountDays1?: number;

  /**
   * 发票
   */
  invoiceFlag?: string;

  /**
   * 凭证抬头文本
   */
  headerText?: string;

  /**
   * 冲销者（冲销凭证编号）
   */
  reversalDocumentCode?: string;

  /**
   * 年（冲销会计年度）
   */
  reversalFiscalYear?: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 供货国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  supplyingCountry?: string;

  /**
   * 税率（税务汇率）
   */
  taxExchangeRate?: number;

  /**
   * 付款基准日期
   */
  baselineDate?: string;

  /**
   * 换算日期
   */
  exchangeRateDate?: string;

  /**
   * 事务代码
   */
  transactionCode?: string;

  /**
   * 过账人（当前登录用户对应的 EmployeeCode）
   */
  postedBy?: string;

  /**
   * 采购发票明细列表（主子表关系）（子表，级联保存）
   */
  items?: PurchaseInvoiceItemCreate[];

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
 * PurchaseInvoice 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchaseInvoiceExport
 * @description 对应后端 TaktPurchaseInvoiceExportDto
 */
export interface PurchaseInvoiceExport {
  /**
   * PurchaseInvoiceID
   */
  purchaseInvoiceId: string;

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
   * 发票凭证编号
   */
  purchaseInvoiceCode: string;

  /**
   * 会计年度
   */
  fiscalYear: string;

  /**
   * 凭证类型（字典 logistics_purchase_invoice_document_type）
   */
  documentType?: string;

  /**
   * 凭证日期
   */
  documentDate: string;

  /**
   * 过帐日期
   */
  postingDate: string;

  /**
   * 交易类型（字典 logistics_purchase_invoice_transaction_event_type）
   */
  transactionEventType?: string;

  /**
   * 参照
   */
  referenceCode?: string;

  /**
   * 出票方（选项 TaktSuppliers/options；DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 货币（字典 accounting_financial_currency_code）
   */
  currencyCode: string;

  /**
   * 汇率
   */
  exchangeRate?: number;

  /**
   * 总发票金额
   */
  grossAmount: number;

  /**
   * 增值税金额
   */
  vatAmount?: number;

  /**
   * 税务代码
   */
  taxJurisdictionCode?: string;

  /**
   * 天数 1（现金折扣天数）
   */
  cashDiscountDays1?: number;

  /**
   * 发票
   */
  invoiceFlag?: string;

  /**
   * 凭证抬头文本
   */
  headerText?: string;

  /**
   * 冲销者（冲销凭证编号）
   */
  reversalDocumentCode?: string;

  /**
   * 年（冲销会计年度）
   */
  reversalFiscalYear?: string;

  /**
   * 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
   */
  taxCode?: string;

  /**
   * 供货国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  supplyingCountry?: string;

  /**
   * 税率（税务汇率）
   */
  taxExchangeRate?: number;

  /**
   * 付款基准日期
   */
  baselineDate?: string;

  /**
   * 换算日期
   */
  exchangeRateDate?: string;

  /**
   * 事务代码
   */
  transactionCode?: string;

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


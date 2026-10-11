// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/procurement/purchase-invoice/composables
// 文件名称：usePurchaseInvoiceI18n.ts
// 功能描述：Takt采购发票主表实体字段清单 + usePurchaseInvoiceI18n（字段名映射一次，文案由 entity.purchaseinvoice.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PurchaseInvoiceQuery } from '@/types/logistics/procurement/purchaseInvoice.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktPurchaseInvoiceI18nSeedData 一致的实体 slug */
export const PURCHASEINVOICE_ENTITY_SLUG = 'purchaseinvoice'

/** entity.purchaseinvoice._self 静态属性（导入组件 entity-i18n-key 等） */
export const PURCHASEINVOICE_SELF_I18N_KEY = buildEntitySelfI18nKey(PURCHASEINVOICE_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PURCHASEINVOICE_LIST_FIELDS = [
  'purchaseInvoiceCode',
  'fiscalYear',
  'documentType',
  'documentDate',
  'postingDate',
  'transactionEventType',
  'referenceCode',
  'supplierCode',
  'currencyCode',
  'exchangeRate',
  'grossAmount',
  'vatAmount',
  'taxJurisdictionCode',
  'cashDiscountDays1',
  'invoiceFlag',
  'headerText',
  'reversalDocumentCode',
  'reversalFiscalYear',
  'taxCode',
  'supplyingCountry',
  'taxExchangeRate',
  'baselineDate',
  'exchangeRateDate',
  'transactionCode',
  'postedBy',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PURCHASEINVOICE_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  purchaseInvoiceCode: 'required',
  fiscalYear: 'required',
  documentType: 'optional',
  documentDate: 'select',
  postingDate: 'select',
  transactionEventType: 'optional',
  referenceCode: 'optional',
  supplierCode: 'select',
  currencyCode: 'select',
  exchangeRate: 'optional',
  grossAmount: 'select',
  vatAmount: 'optional',
  taxJurisdictionCode: 'optional',
  cashDiscountDays1: 'optional',
  invoiceFlag: 'optional',
  headerText: 'optional',
  reversalDocumentCode: 'optional',
  reversalFiscalYear: 'optional',
  taxCode: 'optional',
  supplyingCountry: 'optional',
  taxExchangeRate: 'optional',
  baselineDate: 'optional',
  exchangeRateDate: 'optional',
  transactionCode: 'optional',
  postedBy: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type PurchaseInvoiceField = keyof typeof PURCHASEINVOICE_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PURCHASEINVOICE_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'purchaseInvoiceCode',
  'fiscalYear',
  'documentType',
  'documentDateStart',
  'documentDateEnd',
  'postingDateStart',
  'postingDateEnd',
  'transactionEventType',
  'referenceCode',
  'supplierCode',
  'currencyCode',
  'taxJurisdictionCode',
  'invoiceFlag',
  'headerText',
  'reversalDocumentCode',
  'reversalFiscalYear',
  'taxCode',
  'supplyingCountry',
  'baselineDateStart',
  'baselineDateEnd',
  'exchangeRateDateStart',
  'exchangeRateDateEnd',
  'transactionCode',
  'postedBy',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof PurchaseInvoiceQuery)[]

export type PurchaseInvoiceQueryField =
  | (typeof PURCHASEINVOICE_QUERY_STRING_FIELDS)[number]
  | 'exchangeRate' | 'grossAmount' | 'vatAmount' | 'cashDiscountDays1' | 'taxExchangeRate'

/** 高级查询抽屉全部字段（含数值） */
export const PURCHASEINVOICE_QUERY_FIELDS: readonly PurchaseInvoiceQueryField[] = [
  ...PURCHASEINVOICE_QUERY_STRING_FIELDS,
  'exchangeRate',
  'grossAmount',
  'vatAmount',
  'cashDiscountDays1',
  'taxExchangeRate',
]

/**
 * Takt采购发票主表实体字段 i18n：index / purchase-invoice-form 统一入口
 */
export function usePurchaseInvoiceI18n() {
  const ef = useEntityFieldI18n(PURCHASEINVOICE_ENTITY_SLUG)

  function ph(field: PurchaseInvoiceField): string {
    return ef.placeholder(field, PURCHASEINVOICE_PLACEHOLDER[field])
  }

  function queryPh(field: PurchaseInvoiceQueryField, kind: EntityFieldPlaceholderKind): string {
    return ef.queryPlaceholder(field, kind)
  }

  return {
    t: ef.t,
    label: ef.label,
    queryLabel: ef.queryLabel,
    queryPh,
    self: ef.self,
    ph,
  }
}

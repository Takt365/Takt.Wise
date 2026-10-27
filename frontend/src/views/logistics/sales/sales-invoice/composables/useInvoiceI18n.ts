// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/sales/sales-invoice/composables
// 文件名称：useInvoiceI18n.ts
// 功能描述：Takt销售发票主表实体字段清单 + useSalesInvoiceI18n（字段名映射一次，文案由 entity.salesinvoice.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SalesInvoiceQuery } from '@/types/logistics/sales/invoice.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSalesInvoiceI18nSeedData 一致的实体 slug */
export const SALESINVOICE_ENTITY_SLUG = 'salesinvoice'

/** entity.salesinvoice._self 静态属性（导入组件 entity-i18n-key 等） */
export const SALESINVOICE_SELF_I18N_KEY = buildEntitySelfI18nKey(SALESINVOICE_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SALESINVOICE_LIST_FIELDS = [
  'billingDocumentCode',
  'billingType',
  'billingCategory',
  'documentCategory',
  'currencyCode',
  'salesOrganization',
  'distributionChannel',
  'pricingProcedure',
  'conditionCode',
  'shippingConditions',
  'billingDate',
  'customerGroup',
  'incoterms1',
  'incoterms2',
  'postingStatus',
  'accountingExchangeRate',
  'paymentTerms',
  'accountAssignmentGroup',
  'countryCode',
  'netAmount',
  'payerCode',
  'customerCode',
  'statisticsCurrencyCode',
  'foreignTradeCode',
  'cancelledBillingDocument',
  'invoiceListType',
  'division',
  'hierarchyTypePricing',
  'tradingPartner',
  'taxDepartureCountry',
  'organizationSalesTaxNumber',
  'countrySalesTaxNumber',
  'referenceCode',
  'cancelledFlag',
  'exchangeRateDate',
  'paymentReference',
  'reversalReason',
  'postedBy',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SALESINVOICE_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  billingDocumentCode: 'required',
  billingType: 'optional',
  billingCategory: 'optional',
  documentCategory: 'optional',
  currencyCode: 'select',
  salesOrganization: 'optional',
  distributionChannel: 'optional',
  pricingProcedure: 'optional',
  conditionCode: 'optional',
  shippingConditions: 'optional',
  billingDate: 'select',
  customerGroup: 'optional',
  incoterms1: 'optional',
  incoterms2: 'optional',
  postingStatus: 'optional',
  accountingExchangeRate: 'optional',
  paymentTerms: 'optional',
  accountAssignmentGroup: 'optional',
  countryCode: 'optional',
  netAmount: 'select',
  payerCode: 'optional',
  customerCode: 'select',
  statisticsCurrencyCode: 'optional',
  foreignTradeCode: 'optional',
  cancelledBillingDocument: 'optional',
  invoiceListType: 'optional',
  division: 'optional',
  hierarchyTypePricing: 'optional',
  tradingPartner: 'optional',
  taxDepartureCountry: 'optional',
  organizationSalesTaxNumber: 'optional',
  countrySalesTaxNumber: 'optional',
  referenceCode: 'optional',
  cancelledFlag: 'optional',
  exchangeRateDate: 'optional',
  paymentReference: 'optional',
  reversalReason: 'optional',
  postedBy: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SalesInvoiceField = keyof typeof SALESINVOICE_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SALESINVOICE_QUERY_STRING_FIELDS = [
  'cultureCode',
  'billingDocumentCode',
  'billingType',
  'billingCategory',
  'documentCategory',
  'currencyCode',
  'salesOrganization',
  'distributionChannel',
  'pricingProcedure',
  'conditionCode',
  'shippingConditions',
  'billingDateStart',
  'billingDateEnd',
  'customerGroup',
  'incoterms1',
  'incoterms2',
  'postingStatus',
  'paymentTerms',
  'accountAssignmentGroup',
  'countryCode',
  'payerCode',
  'customerCode',
  'statisticsCurrencyCode',
  'foreignTradeCode',
  'cancelledBillingDocument',
  'invoiceListType',
  'division',
  'hierarchyTypePricing',
  'tradingPartner',
  'taxDepartureCountry',
  'organizationSalesTaxNumber',
  'countrySalesTaxNumber',
  'referenceCode',
  'cancelledFlag',
  'exchangeRateDateStart',
  'exchangeRateDateEnd',
  'paymentReference',
  'reversalReason',
  'postedBy',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SalesInvoiceQuery)[]

export type SalesInvoiceQueryField =
  | (typeof SALESINVOICE_QUERY_STRING_FIELDS)[number]
  | 'accountingExchangeRate' | 'netAmount'

/** 高级查询抽屉全部字段（含数值） */
export const SALESINVOICE_QUERY_FIELDS: readonly SalesInvoiceQueryField[] = [
  ...SALESINVOICE_QUERY_STRING_FIELDS,
  'accountingExchangeRate',
  'netAmount',
]

/**
 * Takt销售发票主表实体字段 i18n：index / invoice-form 统一入口
 */
export function useSalesInvoiceI18n() {
  const ef = useEntityFieldI18n(SALESINVOICE_ENTITY_SLUG)

  function ph(field: SalesInvoiceField): string {
    return ef.placeholder(field, SALESINVOICE_PLACEHOLDER[field])
  }

  function queryPh(field: SalesInvoiceQueryField, kind: EntityFieldPlaceholderKind): string {
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

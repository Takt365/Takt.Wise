// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/sales/sales-invoice/composables
// 文件名称：useInvoiceItemI18n.ts
// 功能描述：SalesInvoiceItem字段清单 + useSalesInvoiceItemI18n（字段名映射一次，文案由 entity.salesinvoiceitem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SalesInvoiceItemQuery } from '@/types/logistics/sales/invoiceItem.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSalesInvoiceItemI18nSeedData 一致的实体 slug */
export const SALESINVOICEITEM_ENTITY_SLUG = 'salesinvoiceitem'

/** entity.salesinvoiceitem._self 静态属性（导入组件 entity-i18n-key 等） */
export const SALESINVOICEITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(SALESINVOICEITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SALESINVOICEITEM_LIST_FIELDS = [
  'salesInvoiceId',
  'billingDocumentCode',
  'lineNumber',
  'billingQuantity',
  'salesUnit',
  'baseUnit',
  'scaleQuantity',
  'billingQuantitySku',
  'netWeight',
  'grossWeight',
  'weightUnit',
  'businessAreaCode',
  'pricingDate',
  'serviceRenderedDate',
  'pricingExchangeRate',
  'netAmount',
  'referenceDocumentCode',
  'referenceDocumentItem',
  'referenceDocumentCategory',
  'salesDocumentCode',
  'salesDocumentItem',
  'salesDocumentReferenceFlag',
  'materialCode',
  'materialDescription',
  'pricingReferenceMaterialCode',
  'batchCode',
  'materialGroup',
  'salesItemCategory',
  'productHierarchy',
  'shippingPoint',
  'division',
  'partnerItem',
  'departureCountry',
  'plantRegion',
  'pricingFlag',
  'warehouseCode',
  'costAmount',
  'subtotal1',
  'subtotal2',
  'subtotal3',
  'subtotal4',
  'subtotal5',
  'subtotal6',
  'statisticsExchangeRate',
  'profitCenterCode',
  'creditPrice',
  'customerGroupSalesOrder',
  'destinationCountryOrder',
  'regionOrder',
  'salesOrganizationOrder',
  'distributionChannelOrder',
  'documentCategory',
  'taxAmount',
  'grossAmount',
  'exchangeRateDate',
  'isObsolete',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const SALESINVOICEITEM_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'salesInvoiceId',
  'billingDocumentCode',
  'lineNumber',
  'billingQuantity',
  'salesUnit',
  'baseUnit',
  'scaleQuantity',
  'billingQuantitySku',
  'netWeight',
  'grossWeight',
  'weightUnit',
  'businessAreaCode',
  'pricingDate',
  'serviceRenderedDate',
  'pricingExchangeRate',
  'netAmount',
  'referenceDocumentCode',
  'referenceDocumentItem',
  'referenceDocumentCategory',
  'salesDocumentCode',
  'salesDocumentItem',
  'salesDocumentReferenceFlag',
  'materialCode',
  'materialDescription',
  'pricingReferenceMaterialCode',
  'batchCode',
  'materialGroup',
  'salesItemCategory',
  'productHierarchy',
  'shippingPoint',
  'division',
  'partnerItem',
  'departureCountry',
  'plantRegion',
  'pricingFlag',
  'warehouseCode',
  'costAmount',
  'subtotal1',
  'subtotal2',
  'subtotal3',
  'subtotal4',
  'subtotal5',
  'subtotal6',
  'statisticsExchangeRate',
  'profitCenterCode',
  'creditPrice',
  'customerGroupSalesOrder',
  'destinationCountryOrder',
  'regionOrder',
  'salesOrganizationOrder',
  'distributionChannelOrder',
  'documentCategory',
  'taxAmount',
  'grossAmount',
  'exchangeRateDate',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const SALESINVOICEITEM_SUMMARY_SUM_FIELDS = [
  'billingQuantity',
  'scaleQuantity',
  'billingQuantitySku',
  'netWeight',
  'grossWeight',
  'pricingExchangeRate',
  'netAmount',
  'referenceDocumentItem',
  'salesDocumentItem',
  'partnerItem',
  'costAmount',
  'subtotal1',
  'subtotal2',
  'subtotal3',
  'subtotal4',
  'subtotal5',
  'subtotal6',
  'statisticsExchangeRate',
  'creditPrice',
  'taxAmount',
  'grossAmount',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SALESINVOICEITEM_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  billingDocumentCode: 'optional',
  lineNumber: 'select',
  billingQuantity: 'optional',
  salesUnit: 'optional',
  baseUnit: 'optional',
  scaleQuantity: 'optional',
  billingQuantitySku: 'optional',
  netWeight: 'optional',
  grossWeight: 'optional',
  weightUnit: 'optional',
  businessAreaCode: 'optional',
  pricingDate: 'optional',
  serviceRenderedDate: 'optional',
  pricingExchangeRate: 'optional',
  netAmount: 'optional',
  referenceDocumentCode: 'optional',
  referenceDocumentItem: 'optional',
  referenceDocumentCategory: 'optional',
  salesDocumentCode: 'optional',
  salesDocumentItem: 'optional',
  salesDocumentReferenceFlag: 'optional',
  materialCode: 'select',
  materialDescription: 'optional',
  pricingReferenceMaterialCode: 'optional',
  batchCode: 'optional',
  materialGroup: 'optional',
  salesItemCategory: 'optional',
  productHierarchy: 'optional',
  shippingPoint: 'optional',
  division: 'optional',
  partnerItem: 'optional',
  departureCountry: 'optional',
  plantRegion: 'optional',
  pricingFlag: 'optional',
  warehouseCode: 'optional',
  costAmount: 'optional',
  subtotal1: 'optional',
  subtotal2: 'optional',
  subtotal3: 'optional',
  subtotal4: 'optional',
  subtotal5: 'optional',
  subtotal6: 'optional',
  statisticsExchangeRate: 'optional',
  profitCenterCode: 'optional',
  creditPrice: 'optional',
  customerGroupSalesOrder: 'optional',
  destinationCountryOrder: 'optional',
  regionOrder: 'optional',
  salesOrganizationOrder: 'optional',
  distributionChannelOrder: 'optional',
  documentCategory: 'optional',
  taxAmount: 'optional',
  grossAmount: 'optional',
  exchangeRateDate: 'optional',
  isObsolete: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SalesInvoiceItemField = keyof typeof SALESINVOICEITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SALESINVOICEITEM_QUERY_STRING_FIELDS = [
  'cultureCode',
  'billingDocumentCode',
  'salesUnit',
  'baseUnit',
  'weightUnit',
  'businessAreaCode',
  'pricingDateStart',
  'pricingDateEnd',
  'serviceRenderedDateStart',
  'serviceRenderedDateEnd',
  'referenceDocumentCode',
  'referenceDocumentCategory',
  'salesDocumentCode',
  'salesDocumentReferenceFlag',
  'materialCode',
  'materialDescription',
  'pricingReferenceMaterialCode',
  'batchCode',
  'materialGroup',
  'salesItemCategory',
  'productHierarchy',
  'shippingPoint',
  'division',
  'departureCountry',
  'plantRegion',
  'pricingFlag',
  'warehouseCode',
  'profitCenterCode',
  'customerGroupSalesOrder',
  'destinationCountryOrder',
  'regionOrder',
  'salesOrganizationOrder',
  'distributionChannelOrder',
  'documentCategory',
  'exchangeRateDateStart',
  'exchangeRateDateEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SalesInvoiceItemQuery)[]

export type SalesInvoiceItemQueryField =
  | (typeof SALESINVOICEITEM_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'billingQuantity' | 'scaleQuantity' | 'billingQuantitySku' | 'netWeight' | 'grossWeight' | 'pricingExchangeRate' | 'netAmount' | 'referenceDocumentItem' | 'salesDocumentItem' | 'partnerItem' | 'costAmount' | 'subtotal1' | 'subtotal2' | 'subtotal3' | 'subtotal4' | 'subtotal5' | 'subtotal6' | 'statisticsExchangeRate' | 'creditPrice' | 'taxAmount' | 'grossAmount' | 'isObsolete'

/** 高级查询抽屉全部字段（含数值） */
export const SALESINVOICEITEM_QUERY_FIELDS: readonly SalesInvoiceItemQueryField[] = [
  ...SALESINVOICEITEM_QUERY_STRING_FIELDS,
  'lineNumber',
  'billingQuantity',
  'scaleQuantity',
  'billingQuantitySku',
  'netWeight',
  'grossWeight',
  'pricingExchangeRate',
  'netAmount',
  'referenceDocumentItem',
  'salesDocumentItem',
  'partnerItem',
  'costAmount',
  'subtotal1',
  'subtotal2',
  'subtotal3',
  'subtotal4',
  'subtotal5',
  'subtotal6',
  'statisticsExchangeRate',
  'creditPrice',
  'taxAmount',
  'grossAmount',
  'isObsolete',
]

/**
 * SalesInvoiceItem字段 i18n：index / invoice-item-form 统一入口
 */
export function useSalesInvoiceItemI18n() {
  const ef = useEntityFieldI18n(SALESINVOICEITEM_ENTITY_SLUG)

  function ph(field: SalesInvoiceItemField): string {
    return ef.placeholder(field, SALESINVOICEITEM_PLACEHOLDER[field])
  }

  function queryPh(field: SalesInvoiceItemQueryField, kind: EntityFieldPlaceholderKind): string {
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

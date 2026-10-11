// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/sales/quotation/composables
// 文件名称：useQuotationItemI18n.ts
// 功能描述：SalesQuotationItem字段清单 + useSalesQuotationItemI18n（字段名映射一次，文案由 entity.salesquotationitem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SalesQuotationItemQuery } from '@/types/logistics/sales/quotationItem.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSalesQuotationItemI18nSeedData 一致的实体 slug */
export const SALESQUOTATIONITEM_ENTITY_SLUG = 'salesquotationitem'

/** entity.salesquotationitem._self 静态属性（导入组件 entity-i18n-key 等） */
export const SALESQUOTATIONITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(SALESQUOTATIONITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SALESQUOTATIONITEM_LIST_FIELDS = [
  'salesQuotationId',
  'salesQuotationName',
  'salesQuotationCode',
  'lineNumber',
  'materialCode',
  'materialDescription',
  'materialSpecification',
  'salesUnit',
  'quotationQuantity',
  'salesPerUnit',
  'quotationUnitPrice',
  'discountRate',
  'discountAmount',
  'taxCode',
  'taxIncludedAmount',
  'untaxedAmount',
  'taxAmount',
  'quotationAmount',
  'pricingDate',
  'grossWeight',
  'netWeight',
  'weightUnit',
  'volume',
  'volumeUnit',
  'profitCenterCode',
  'isObsolete',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const SALESQUOTATIONITEM_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'salesQuotationId',
  'salesQuotationName',
  'salesQuotationCode',
  'lineNumber',
  'materialCode',
  'materialDescription',
  'materialSpecification',
  'salesUnit',
  'quotationQuantity',
  'salesPerUnit',
  'quotationUnitPrice',
  'discountRate',
  'discountAmount',
  'taxCode',
  'taxIncludedAmount',
  'untaxedAmount',
  'taxAmount',
  'quotationAmount',
  'pricingDate',
  'grossWeight',
  'netWeight',
  'weightUnit',
  'volume',
  'volumeUnit',
  'profitCenterCode',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const SALESQUOTATIONITEM_SUMMARY_SUM_FIELDS = [
  'quotationQuantity',
  'salesPerUnit',
  'quotationUnitPrice',
  'discountRate',
  'discountAmount',
  'taxIncludedAmount',
  'untaxedAmount',
  'taxAmount',
  'quotationAmount',
  'grossWeight',
  'netWeight',
  'volume',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SALESQUOTATIONITEM_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  lineNumber: 'select',
  materialCode: 'select',
  materialDescription: 'optional',
  materialSpecification: 'optional',
  salesUnit: 'select',
  quotationQuantity: 'select',
  salesPerUnit: 'select',
  quotationUnitPrice: 'select',
  discountRate: 'select',
  discountAmount: 'select',
  taxCode: 'optional',
  taxIncludedAmount: 'select',
  untaxedAmount: 'select',
  taxAmount: 'select',
  quotationAmount: 'select',
  pricingDate: 'optional',
  grossWeight: 'optional',
  netWeight: 'optional',
  weightUnit: 'optional',
  volume: 'optional',
  volumeUnit: 'optional',
  profitCenterCode: 'optional',
  isObsolete: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SalesQuotationItemField = keyof typeof SALESQUOTATIONITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SALESQUOTATIONITEM_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'salesQuotationCode',
  'materialCode',
  'materialDescription',
  'materialSpecification',
  'salesUnit',
  'taxCode',
  'pricingDateStart',
  'pricingDateEnd',
  'weightUnit',
  'volumeUnit',
  'profitCenterCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SalesQuotationItemQuery)[]

export type SalesQuotationItemQueryField =
  | (typeof SALESQUOTATIONITEM_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'quotationQuantity' | 'salesPerUnit' | 'quotationUnitPrice' | 'discountRate' | 'discountAmount' | 'taxIncludedAmount' | 'untaxedAmount' | 'taxAmount' | 'quotationAmount' | 'grossWeight' | 'netWeight' | 'volume' | 'isObsolete'

/** 高级查询抽屉全部字段（含数值） */
export const SALESQUOTATIONITEM_QUERY_FIELDS: readonly SalesQuotationItemQueryField[] = [
  ...SALESQUOTATIONITEM_QUERY_STRING_FIELDS,
  'lineNumber',
  'quotationQuantity',
  'salesPerUnit',
  'quotationUnitPrice',
  'discountRate',
  'discountAmount',
  'taxIncludedAmount',
  'untaxedAmount',
  'taxAmount',
  'quotationAmount',
  'grossWeight',
  'netWeight',
  'volume',
  'isObsolete',
]

/**
 * SalesQuotationItem字段 i18n：index / quotation-item-form 统一入口
 */
export function useSalesQuotationItemI18n() {
  const ef = useEntityFieldI18n(SALESQUOTATIONITEM_ENTITY_SLUG)

  function ph(field: SalesQuotationItemField): string {
    return ef.placeholder(field, SALESQUOTATIONITEM_PLACEHOLDER[field])
  }

  function queryPh(field: SalesQuotationItemQueryField, kind: EntityFieldPlaceholderKind): string {
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

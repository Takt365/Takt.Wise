// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/procurement/purchase-inquiry/composables
// 文件名称：usePurchaseInquiryItemI18n.ts
// 功能描述：PurchaseInquiryItem字段清单 + usePurchaseInquiryItemI18n（字段名映射一次，文案由 entity.purchaseinquiryitem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PurchaseInquiryItemQuery } from '@/types/logistics/procurement/purchaseInquiryItem.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktPurchaseInquiryItemI18nSeedData 一致的实体 slug */
export const PURCHASEINQUIRYITEM_ENTITY_SLUG = 'purchaseinquiryitem'

/** entity.purchaseinquiryitem._self 静态属性（导入组件 entity-i18n-key 等） */
export const PURCHASEINQUIRYITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(PURCHASEINQUIRYITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PURCHASEINQUIRYITEM_LIST_FIELDS = [
  'purchaseInquiryId',
  'purchaseInquiryCode',
  'lineNumber',
  'allocationCategory',
  'materialCode',
  'materialDescription',
  'materialSpecification',
  'inquiryUnit',
  'inquiryQuantity',
  'purchasePerUnit',
  'quotedUnitPrice',
  'taxCode',
  'taxIncludedAmount',
  'untaxedAmount',
  'taxAmount',
  'inquiryAmount',
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
export const PURCHASEINQUIRYITEM_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'purchaseInquiryId',
  'purchaseInquiryCode',
  'lineNumber',
  'allocationCategory',
  'materialCode',
  'materialDescription',
  'materialSpecification',
  'inquiryUnit',
  'inquiryQuantity',
  'purchasePerUnit',
  'quotedUnitPrice',
  'taxCode',
  'taxIncludedAmount',
  'untaxedAmount',
  'taxAmount',
  'inquiryAmount',
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
export const PURCHASEINQUIRYITEM_SUMMARY_SUM_FIELDS = [
  'inquiryQuantity',
  'purchasePerUnit',
  'quotedUnitPrice',
  'taxIncludedAmount',
  'untaxedAmount',
  'taxAmount',
  'inquiryAmount',
  'grossWeight',
  'netWeight',
  'volume',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PURCHASEINQUIRYITEM_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  lineNumber: 'select',
  allocationCategory: 'select',
  materialCode: 'optional',
  materialDescription: 'optional',
  materialSpecification: 'optional',
  inquiryUnit: 'select',
  inquiryQuantity: 'select',
  purchasePerUnit: 'select',
  quotedUnitPrice: 'optional',
  taxCode: 'optional',
  taxIncludedAmount: 'select',
  untaxedAmount: 'select',
  taxAmount: 'select',
  inquiryAmount: 'select',
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
export type PurchaseInquiryItemField = keyof typeof PURCHASEINQUIRYITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PURCHASEINQUIRYITEM_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'purchaseInquiryCode',
  'allocationCategory',
  'materialCode',
  'materialDescription',
  'materialSpecification',
  'inquiryUnit',
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
] as const satisfies readonly (keyof PurchaseInquiryItemQuery)[]

export type PurchaseInquiryItemQueryField =
  | (typeof PURCHASEINQUIRYITEM_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'inquiryQuantity' | 'purchasePerUnit' | 'quotedUnitPrice' | 'taxIncludedAmount' | 'untaxedAmount' | 'taxAmount' | 'inquiryAmount' | 'grossWeight' | 'netWeight' | 'volume' | 'isObsolete'

/** 高级查询抽屉全部字段（含数值） */
export const PURCHASEINQUIRYITEM_QUERY_FIELDS: readonly PurchaseInquiryItemQueryField[] = [
  ...PURCHASEINQUIRYITEM_QUERY_STRING_FIELDS,
  'lineNumber',
  'inquiryQuantity',
  'purchasePerUnit',
  'quotedUnitPrice',
  'taxIncludedAmount',
  'untaxedAmount',
  'taxAmount',
  'inquiryAmount',
  'grossWeight',
  'netWeight',
  'volume',
  'isObsolete',
]

/**
 * PurchaseInquiryItem字段 i18n：index / purchase-inquiry-item-form 统一入口
 */
export function usePurchaseInquiryItemI18n() {
  const ef = useEntityFieldI18n(PURCHASEINQUIRYITEM_ENTITY_SLUG)

  function ph(field: PurchaseInquiryItemField): string {
    return ef.placeholder(field, PURCHASEINQUIRYITEM_PLACEHOLDER[field])
  }

  function queryPh(field: PurchaseInquiryItemQueryField, kind: EntityFieldPlaceholderKind): string {
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

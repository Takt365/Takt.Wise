// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/procurement/purchase-info/composables
// 文件名称：usePurchaseInfoItemI18n.ts
// 功能描述：PurchaseInfoItem字段清单 + usePurchaseInfoItemI18n（字段名映射一次，文案由 entity.purchaseinfoitem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PurchaseInfoItemQuery } from '@/types/logistics/procurement/purchaseInfoItem.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktPurchaseInfoItemI18nSeedData 一致的实体 slug */
export const PURCHASEINFORECORDITEM_ENTITY_SLUG = 'purchaseinfoitem'

/** entity.purchaseinfoitem._self 静态属性（导入组件 entity-i18n-key 等） */
export const PURCHASEINFORECORDITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(PURCHASEINFORECORDITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PURCHASEINFORECORDITEM_LIST_FIELDS = [
  'purchaseInfoId',
  'purchaseInfoCode',
  'purchaseInfoSeq',
  'priceType',
  'scaleType',
  'scaleBasis',
  'scaleQuantity',
  'scaleUnit',
  'scaleValue',
  'scaleCurrencyCode',
  'calculationType',
  'price',
  'untaxedPrice',
  'taxIncludedPrice',
  'taxAmount',
  'conditionCurrencyCode',
  'priceUnit',
  'unitOfMeasure',
  'minOrderQuantity',
  'roundingValue',
  'plannedDeliveryTimeDays',
  'isObsolete',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const PURCHASEINFORECORDITEM_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'purchaseInfoId',
  'purchaseInfoCode',
  'purchaseInfoSeq',
  'priceType',
  'scaleType',
  'scaleBasis',
  'scaleQuantity',
  'scaleUnit',
  'scaleValue',
  'scaleCurrencyCode',
  'calculationType',
  'price',
  'untaxedPrice',
  'taxIncludedPrice',
  'taxAmount',
  'conditionCurrencyCode',
  'priceUnit',
  'unitOfMeasure',
  'minOrderQuantity',
  'roundingValue',
  'plannedDeliveryTimeDays',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const PURCHASEINFORECORDITEM_SUMMARY_SUM_FIELDS = [
  'purchaseInfoSeq',
  'scaleQuantity',
  'scaleValue',
  'price',
  'untaxedPrice',
  'taxIncludedPrice',
  'taxAmount',
  'priceUnit',
  'minOrderQuantity',
  'roundingValue',
  'plannedDeliveryTimeDays',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PURCHASEINFORECORDITEM_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  purchaseInfoSeq: 'select',
  priceType: 'optional',
  scaleType: 'optional',
  scaleBasis: 'optional',
  scaleQuantity: 'select',
  scaleUnit: 'optional',
  scaleValue: 'select',
  scaleCurrencyCode: 'optional',
  calculationType: 'select',
  price: 'select',
  untaxedPrice: 'optional',
  taxIncludedPrice: 'optional',
  taxAmount: 'optional',
  conditionCurrencyCode: 'select',
  priceUnit: 'select',
  unitOfMeasure: 'select',
  minOrderQuantity: 'select',
  roundingValue: 'select',
  plannedDeliveryTimeDays: 'select',
  isObsolete: 'select',
  scaleQuantities: 'optional',
  scaleValues: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type PurchaseInfoItemField = keyof typeof PURCHASEINFORECORDITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PURCHASEINFORECORDITEM_QUERY_STRING_FIELDS = [
  'cultureCode',
  'purchaseInfoCode',
  'priceType',
  'scaleType',
  'scaleBasis',
  'scaleUnit',
  'scaleCurrencyCode',
  'calculationType',
  'conditionCurrencyCode',
  'unitOfMeasure',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof PurchaseInfoItemQuery)[]

export type PurchaseInfoItemQueryField =
  | (typeof PURCHASEINFORECORDITEM_QUERY_STRING_FIELDS)[number]
  | 'purchaseInfoSeq' | 'scaleQuantity' | 'scaleValue' | 'price' | 'untaxedPrice' | 'taxIncludedPrice' | 'taxAmount' | 'priceUnit' | 'minOrderQuantity' | 'roundingValue' | 'plannedDeliveryTimeDays' | 'isObsolete'

/** 高级查询抽屉全部字段（含数值） */
export const PURCHASEINFORECORDITEM_QUERY_FIELDS: readonly PurchaseInfoItemQueryField[] = [
  ...PURCHASEINFORECORDITEM_QUERY_STRING_FIELDS,
  'purchaseInfoSeq',
  'scaleQuantity',
  'scaleValue',
  'price',
  'untaxedPrice',
  'taxIncludedPrice',
  'taxAmount',
  'priceUnit',
  'minOrderQuantity',
  'roundingValue',
  'plannedDeliveryTimeDays',
  'isObsolete',
]

/**
 * PurchaseInfoItem字段 i18n：index / PurchaseInfoItemForm 统一入口
 */
export function usePurchaseInfoItemI18n() {
  const ef = useEntityFieldI18n(PURCHASEINFORECORDITEM_ENTITY_SLUG)

  function ph(field: PurchaseInfoItemField): string {
    return ef.placeholder(field, PURCHASEINFORECORDITEM_PLACEHOLDER[field])
  }

  function queryPh(field: PurchaseInfoItemQueryField, kind: EntityFieldPlaceholderKind): string {
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

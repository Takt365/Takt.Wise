// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/procurement/purchase-info-item/composables
// 文件名称：usePurchaseInfoItemI18n.ts
// 功能描述：Takt采购信息行实体字段清单 + usePurchaseInfoItemI18n（字段名映射一次，文案由 entity.purchaseinfoitem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PurchaseInfoItemQuery } from '@/types/logistics/procurement/purchaseInfoItem.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktPurchaseInfoItemI18nSeedData 一致的实体 slug */
export const PURCHASEPRICEITEM_ENTITY_SLUG = 'purchasepriceitem'

/** entity.purchaseinfoitem._self 静态属性（导入组件 entity-i18n-key 等） */
export const PURCHASEPRICEITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(PURCHASEPRICEITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PURCHASEPRICEITEM_LIST_FIELDS = [
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
  'remark',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PURCHASEPRICEITEM_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type PurchaseInfoItemField = keyof typeof PURCHASEPRICEITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PURCHASEPRICEITEM_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof PurchaseInfoItemQuery)[]

export type PurchaseInfoItemQueryField = (typeof PURCHASEPRICEITEM_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const PURCHASEPRICEITEM_QUERY_FIELDS: readonly PurchaseInfoItemQueryField[] = [...PURCHASEPRICEITEM_QUERY_STRING_FIELDS]

/**
 * Takt采购信息行实体字段 i18n：index / purchase-price-item-form 统一入口
 */
export function usePurchaseInfoItemI18n() {
  const ef = useEntityFieldI18n(PURCHASEPRICEITEM_ENTITY_SLUG)

  function ph(field: PurchaseInfoItemField): string {
    return ef.placeholder(field, PURCHASEPRICEITEM_PLACEHOLDER[field])
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

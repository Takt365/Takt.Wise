// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/procurement/purchase-forecast/composables
// 文件名称：usePurchaseForecastI18n.ts
// 功能描述：Takt采购预测实体字段清单 + usePurchaseForecastI18n（字段名映射一次，文案由 entity.purchaseforecast.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PurchaseForecastQuery } from '@/types/logistics/procurement/purchaseForecast.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktPurchaseForecastI18nSeedData 一致的实体 slug */
export const PURCHASEFORECAST_ENTITY_SLUG = 'purchaseforecast'

/** entity.purchaseforecast._self 静态属性（导入组件 entity-i18n-key 等） */
export const PURCHASEFORECAST_SELF_I18N_KEY = buildEntitySelfI18nKey(PURCHASEFORECAST_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PURCHASEFORECAST_LIST_FIELDS = [
  'purchaseForecastCode',
  'planDate',
  'sendDate',
  'sendVersionNo',
  'salesProduct',
  'productCategoryCode',
  'profitCenterCode',
  'modelCode',
  'materialCode',
  'materialDescription',
  'supplierCode',
  'supplierName1',
  'plannerGroupCode',
  'planBy',
  'totalQuantity',
  'totalAmount',
  'convertedQuantity',
  'convertedAmount',
  'planStatus',
  'convertedStatus',
  'planDescription',
  'remark',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PURCHASEFORECAST_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type PurchaseForecastField = keyof typeof PURCHASEFORECAST_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PURCHASEFORECAST_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof PurchaseForecastQuery)[]

export type PurchaseForecastQueryField = (typeof PURCHASEFORECAST_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const PURCHASEFORECAST_QUERY_FIELDS: readonly PurchaseForecastQueryField[] = [...PURCHASEFORECAST_QUERY_STRING_FIELDS]

/**
 * Takt采购预测实体字段 i18n：index / purchase-forecast-form 统一入口
 */
export function usePurchaseForecastI18n() {
  const ef = useEntityFieldI18n(PURCHASEFORECAST_ENTITY_SLUG)

  function ph(field: PurchaseForecastField): string {
    return ef.placeholder(field, PURCHASEFORECAST_PLACEHOLDER[field])
  }

  function queryPh(field: PurchaseForecastQueryField, kind: EntityFieldPlaceholderKind): string {
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

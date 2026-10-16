// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/sales/condition-item/composables
// 文件名称：useConditionScaleQuantityI18n.ts
// 功能描述：SalesConditionScaleQuantity字段清单 + useSalesConditionScaleQuantityI18n（字段名映射一次，文案由 entity.salesconditionscalequantity.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SalesConditionScaleQuantityQuery } from '@/types/logistics/sales/conditionScaleQuantity.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSalesConditionScaleQuantityI18nSeedData 一致的实体 slug */
export const SALESPRICESCALEQUANTITY_ENTITY_SLUG = 'salespricescalequantity'

/** entity.salesconditionscalequantity._self 静态属性（导入组件 entity-i18n-key 等） */
export const SALESPRICESCALEQUANTITY_SELF_I18N_KEY = buildEntitySelfI18nKey(SALESPRICESCALEQUANTITY_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SALESPRICESCALEQUANTITY_LIST_FIELDS = [
  'salesConditionCode',
  'salesConditionSeq',
  'salesScaleSeq',
  'scaleQuantity',
  'price',
  'untaxedPrice',
  'taxIncludedPrice',
  'taxAmount',
  'isObsolete',
  'remark',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const SALESPRICESCALEQUANTITY_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'salesConditionCode',
  'salesConditionSeq',
  'salesScaleSeq',
  'scaleQuantity',
  'price',
  'untaxedPrice',
  'taxIncludedPrice',
  'taxAmount',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const SALESPRICESCALEQUANTITY_SUMMARY_SUM_FIELDS = [
  'salesConditionSeq',
  'salesScaleSeq',
  'scaleQuantity',
  'price',
  'untaxedPrice',
  'taxIncludedPrice',
  'taxAmount',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SALESPRICESCALEQUANTITY_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SalesConditionScaleQuantityField = keyof typeof SALESPRICESCALEQUANTITY_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SALESPRICESCALEQUANTITY_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof SalesConditionScaleQuantityQuery)[]

export type SalesConditionScaleQuantityQueryField = (typeof SALESPRICESCALEQUANTITY_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const SALESPRICESCALEQUANTITY_QUERY_FIELDS: readonly SalesConditionScaleQuantityQueryField[] = [...SALESPRICESCALEQUANTITY_QUERY_STRING_FIELDS]

/**
 * SalesConditionScaleQuantity字段 i18n：index / price-scale-quantity-form 统一入口
 */
export function useSalesConditionScaleQuantityI18n() {
  const ef = useEntityFieldI18n(SALESPRICESCALEQUANTITY_ENTITY_SLUG)

  function ph(field: SalesConditionScaleQuantityField): string {
    return ef.placeholder(field, SALESPRICESCALEQUANTITY_PLACEHOLDER[field])
  }

  function queryPh(field: SalesConditionScaleQuantityQueryField, kind: EntityFieldPlaceholderKind): string {
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

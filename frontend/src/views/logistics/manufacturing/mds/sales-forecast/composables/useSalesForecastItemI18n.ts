// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/mds/sales-forecast/composables
// 文件名称：useSalesForecastItemI18n.ts
// 功能描述：SalesForecastItem字段清单 + useSalesForecastItemI18n（字段名映射一次，文案由 entity.salesforecastitem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SalesForecastItemQuery } from '@/types/logistics/manufacturing/mds/salesForecastItem.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSalesForecastItemI18nSeedData 一致的实体 slug */
export const SALESFORECASTITEM_ENTITY_SLUG = 'salesforecastitem'

/** entity.salesforecastitem._self 静态属性（导入组件 entity-i18n-key 等） */
export const SALESFORECASTITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(SALESFORECASTITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SALESFORECASTITEM_LIST_FIELDS = [
  'salesForecastCode',
  'lineNumber',
  'fiscalYear',
  'planMonth',
  'planQuantity001',
  'planQuantity002',
  'planQuantityDelta',
  'convertedQuantity',
  'estimatedUnitPrice',
  'estimatedAmount',
  'isObsolete',
  'remark',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const SALESFORECASTITEM_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'salesForecastCode',
  'lineNumber',
  'fiscalYear',
  'planMonth',
  'planQuantity001',
  'planQuantity002',
  'planQuantityDelta',
  'convertedQuantity',
  'estimatedUnitPrice',
  'estimatedAmount',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const SALESFORECASTITEM_SUMMARY_SUM_FIELDS = [
  'planMonth',
  'planQuantity001',
  'planQuantity002',
  'planQuantityDelta',
  'convertedQuantity',
  'estimatedUnitPrice',
  'estimatedAmount',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SALESFORECASTITEM_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SalesForecastItemField = keyof typeof SALESFORECASTITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SALESFORECASTITEM_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof SalesForecastItemQuery)[]

export type SalesForecastItemQueryField = (typeof SALESFORECASTITEM_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const SALESFORECASTITEM_QUERY_FIELDS: readonly SalesForecastItemQueryField[] = [...SALESFORECASTITEM_QUERY_STRING_FIELDS]

/**
 * SalesForecastItem字段 i18n：index / sales-forecast-item-form 统一入口
 */
export function useSalesForecastItemI18n() {
  const ef = useEntityFieldI18n(SALESFORECASTITEM_ENTITY_SLUG)

  function ph(field: SalesForecastItemField): string {
    return ef.placeholder(field, SALESFORECASTITEM_PLACEHOLDER[field])
  }

  function queryPh(field: SalesForecastItemQueryField, kind: EntityFieldPlaceholderKind): string {
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

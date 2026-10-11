// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/accounting/financial/exchange-rate/composables
// 文件名称：useExchangeRateI18n.ts
// 功能描述：汇率实体字段清单 + useExchangeRateI18n（字段名映射一次，文案由 entity.exchangerate.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { ExchangeRateQuery } from '@/types/accounting/financial/exchangeRate.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktExchangeRateI18nSeedData 一致的实体 slug */
export const EXCHANGERATE_ENTITY_SLUG = 'exchangerate'

/** entity.exchangerate._self 静态属性（导入组件 entity-i18n-key 等） */
export const EXCHANGERATE_SELF_I18N_KEY = buildEntitySelfI18nKey(EXCHANGERATE_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const EXCHANGERATE_LIST_FIELDS = [
  'fromCurrencyCode',
  'toCurrencyCode',
  'exchangeRateType',
  'exchangeRate',
  'ratioFrom',
  'ratioTo',
  'validFrom',
  'validTo',
  'exchangeRateStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const EXCHANGERATE_PLACEHOLDER = {
  tenantCode: 'optional',
  fromCurrencyCode: 'select',
  toCurrencyCode: 'select',
  exchangeRateType: 'select',
  exchangeRate: 'select',
  ratioFrom: 'select',
  ratioTo: 'select',
  validFrom: 'select',
  validTo: 'select',
  exchangeRateStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type ExchangeRateField = keyof typeof EXCHANGERATE_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const EXCHANGERATE_QUERY_STRING_FIELDS = [
  'fromCurrencyCode',
  'toCurrencyCode',
  'exchangeRateType',
  'validFromStart',
  'validFromEnd',
  'validToStart',
  'validToEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof ExchangeRateQuery)[]

export type ExchangeRateQueryField =
  | (typeof EXCHANGERATE_QUERY_STRING_FIELDS)[number]
  | 'exchangeRate' | 'ratioFrom' | 'ratioTo' | 'exchangeRateStatus'

/** 高级查询抽屉全部字段（含数值） */
export const EXCHANGERATE_QUERY_FIELDS: readonly ExchangeRateQueryField[] = [
  ...EXCHANGERATE_QUERY_STRING_FIELDS,
  'exchangeRate',
  'ratioFrom',
  'ratioTo',
  'exchangeRateStatus',
]

/**
 * 汇率实体字段 i18n：index / exchange-rate-form 统一入口
 */
export function useExchangeRateI18n() {
  const ef = useEntityFieldI18n(EXCHANGERATE_ENTITY_SLUG)

  function ph(field: ExchangeRateField): string {
    return ef.placeholder(field, EXCHANGERATE_PLACEHOLDER[field])
  }

  function queryPh(field: ExchangeRateQueryField, kind: EntityFieldPlaceholderKind): string {
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

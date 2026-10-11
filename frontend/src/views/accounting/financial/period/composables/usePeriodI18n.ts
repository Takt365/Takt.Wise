// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/accounting/financial/period/composables
// 文件名称：usePeriodI18n.ts
// 功能描述：财务期间字段清单 + useFinancialPeriodI18n（字段名映射一次，文案由 entity.financialperiod.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { FinancialPeriodQuery } from '@/types/accounting/financial/period.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktFinancialPeriodI18nSeedData 一致的实体 slug */
export const FINANCIALPERIOD_ENTITY_SLUG = 'financialperiod'

/** entity.financialperiod._self 静态属性（导入组件 entity-i18n-key 等） */
export const FINANCIALPERIOD_SELF_I18N_KEY = buildEntitySelfI18nKey(FINANCIALPERIOD_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const FINANCIALPERIOD_LIST_FIELDS = [
  'countryCode',
  'financialYearCode',
  'periodCode',
  'calendarYear',
  'calendarMonth',
  'financialQuarterCode',
  'isBuiltIn',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const FINANCIALPERIOD_PLACEHOLDER = {
  tenantCode: 'optional',
  countryCode: 'select',
  financialYearCode: 'required',
  periodCode: 'required',
  calendarYear: 'select',
  calendarMonth: 'select',
  financialQuarterCode: 'required',
  isBuiltIn: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type FinancialPeriodField = keyof typeof FINANCIALPERIOD_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const FINANCIALPERIOD_QUERY_STRING_FIELDS = [
  'countryCode',
  'financialYearCode',
  'periodCode',
  'financialQuarterCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof FinancialPeriodQuery)[]

export type FinancialPeriodQueryField =
  | (typeof FINANCIALPERIOD_QUERY_STRING_FIELDS)[number]
  | 'calendarYear' | 'calendarMonth' | 'isBuiltIn'

/** 高级查询抽屉全部字段（含数值） */
export const FINANCIALPERIOD_QUERY_FIELDS: readonly FinancialPeriodQueryField[] = [
  ...FINANCIALPERIOD_QUERY_STRING_FIELDS,
  'calendarYear',
  'calendarMonth',
  'isBuiltIn',
]

/**
 * 财务期间字段 i18n：index / period-form 统一入口
 */
export function useFinancialPeriodI18n() {
  const ef = useEntityFieldI18n(FINANCIALPERIOD_ENTITY_SLUG)

  function ph(field: FinancialPeriodField): string {
    return ef.placeholder(field, FINANCIALPERIOD_PLACEHOLDER[field])
  }

  function queryPh(field: FinancialPeriodQueryField, kind: EntityFieldPlaceholderKind): string {
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

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/accounting/financial/budget-actual/composables
// 文件名称：useBudgetActualI18n.ts
// 功能描述：预算实绩实体字段清单 + useBudgetActualI18n（字段名映射一次，文案由 entity.budgetactual.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { BudgetActualQuery } from '@/types/accounting/financial/budgetActual.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktBudgetActualI18nSeedData 一致的实体 slug */
export const BUDGETACTUAL_ENTITY_SLUG = 'budgetactual'

/** entity.budgetactual._self 静态属性（导入组件 entity-i18n-key 等） */
export const BUDGETACTUAL_SELF_I18N_KEY = buildEntitySelfI18nKey(BUDGETACTUAL_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const BUDGETACTUAL_LIST_FIELDS = [
  'plantCode',
  'periodCode',
  'costCenterCode',
  'costCenterName',
  'budgetItemCode',
  'budgetItemName',
  'accountTitleCode',
  'budgetType',
  'measureType',
  'budgetAmount',
  'actualAmount',
  'varianceAmount',
  'variancePercent',
  'priorPeriodActual',
  'ytdBudgetAmount',
  'ytdActualAmount',
  'ytdVarianceAmount',
  'currencyCode',
  'budgetActualStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const BUDGETACTUAL_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  plantCode: 'select',
  periodCode: 'required',
  costCenterCode: 'select',
  costCenterName: 'optional',
  budgetItemCode: 'required',
  budgetItemName: 'required',
  accountTitleCode: 'optional',
  budgetType: 'select',
  measureType: 'select',
  budgetAmount: 'select',
  actualAmount: 'select',
  varianceAmount: 'select',
  variancePercent: 'select',
  priorPeriodActual: 'select',
  ytdBudgetAmount: 'select',
  ytdActualAmount: 'select',
  ytdVarianceAmount: 'select',
  currencyCode: 'select',
  budgetActualStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type BudgetActualField = keyof typeof BUDGETACTUAL_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const BUDGETACTUAL_QUERY_STRING_FIELDS = [
  'periodCode',
  'costCenterCode',
  'costCenterName',
  'budgetItemCode',
  'budgetItemName',
  'accountTitleCode',
  'currencyCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof BudgetActualQuery)[]

export type BudgetActualQueryField =
  | (typeof BUDGETACTUAL_QUERY_STRING_FIELDS)[number]
  | 'budgetType' | 'measureType' | 'budgetAmount' | 'actualAmount' | 'varianceAmount' | 'variancePercent' | 'priorPeriodActual' | 'ytdBudgetAmount' | 'ytdActualAmount' | 'ytdVarianceAmount' | 'budgetActualStatus'

/** 高级查询抽屉全部字段（含数值） */
export const BUDGETACTUAL_QUERY_FIELDS: readonly BudgetActualQueryField[] = [
  ...BUDGETACTUAL_QUERY_STRING_FIELDS,
  'budgetType',
  'measureType',
  'budgetAmount',
  'actualAmount',
  'varianceAmount',
  'variancePercent',
  'priorPeriodActual',
  'ytdBudgetAmount',
  'ytdActualAmount',
  'ytdVarianceAmount',
  'budgetActualStatus',
]

/**
 * 预算实绩实体字段 i18n：index / budget-actual-form 统一入口
 */
export function useBudgetActualI18n() {
  const ef = useEntityFieldI18n(BUDGETACTUAL_ENTITY_SLUG)

  function ph(field: BudgetActualField): string {
    return ef.placeholder(field, BUDGETACTUAL_PLACEHOLDER[field])
  }

  function queryPh(field: BudgetActualQueryField, kind: EntityFieldPlaceholderKind): string {
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

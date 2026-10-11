// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/accounting/financial/profit-loss/composables
// 文件名称：useProfitLossI18n.ts
// 功能描述：利润表字段清单 + useProfitLossI18n（字段名映射一次，文案由 entity.profitloss.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { ProfitLossQuery } from '@/types/accounting/financial/profitLoss.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktProfitLossI18nSeedData 一致的实体 slug */
export const PROFITLOSS_ENTITY_SLUG = 'profitloss'

/** entity.profitloss._self 静态属性（导入组件 entity-i18n-key 等） */
export const PROFITLOSS_SELF_I18N_KEY = buildEntitySelfI18nKey(PROFITLOSS_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PROFITLOSS_LIST_FIELDS = [
  'plantCode',
  'periodCode',
  'statementLineCode',
  'statementLineName',
  'accountTitleCode',
  'accountTitleName',
  'lineCategory',
  'isTotalLine',
  'periodAmount',
  'priorPeriodAmount',
  'yearToDateAmount',
  'isExpense',
  'currencyCode',
  'profitLossStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PROFITLOSS_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  plantCode: 'select',
  periodCode: 'required',
  statementLineCode: 'required',
  statementLineName: 'required',
  accountTitleCode: 'optional',
  accountTitleName: 'optional',
  lineCategory: 'select',
  isTotalLine: 'select',
  periodAmount: 'select',
  priorPeriodAmount: 'select',
  yearToDateAmount: 'select',
  isExpense: 'select',
  currencyCode: 'select',
  profitLossStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type ProfitLossField = keyof typeof PROFITLOSS_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PROFITLOSS_QUERY_STRING_FIELDS = [
  'plantCode',
  'periodCode',
  'statementLineCode',
  'statementLineName',
  'accountTitleCode',
  'accountTitleName',
  'currencyCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof ProfitLossQuery)[]

export type ProfitLossQueryField =
  | (typeof PROFITLOSS_QUERY_STRING_FIELDS)[number]
  | 'lineCategory' | 'isTotalLine' | 'periodAmount' | 'priorPeriodAmount' | 'yearToDateAmount' | 'isExpense' | 'profitLossStatus'

/** 高级查询抽屉全部字段（含数值） */
export const PROFITLOSS_QUERY_FIELDS: readonly ProfitLossQueryField[] = [
  ...PROFITLOSS_QUERY_STRING_FIELDS,
  'lineCategory',
  'isTotalLine',
  'periodAmount',
  'priorPeriodAmount',
  'yearToDateAmount',
  'isExpense',
  'profitLossStatus',
]

/**
 * 利润表字段 i18n：index / profit-loss-form 统一入口
 */
export function useProfitLossI18n() {
  const ef = useEntityFieldI18n(PROFITLOSS_ENTITY_SLUG)

  function ph(field: ProfitLossField): string {
    return ef.placeholder(field, PROFITLOSS_PLACEHOLDER[field])
  }

  function queryPh(field: ProfitLossQueryField, kind: EntityFieldPlaceholderKind): string {
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

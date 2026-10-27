// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/accounting/financial/balance-sheet/composables
// 文件名称：useBalanceSheetI18n.ts
// 功能描述：资产负债表行实体字段清单 + useBalanceSheetI18n（字段名映射一次，文案由 entity.balancesheet.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { BalanceSheetQuery } from '@/types/accounting/financial/balanceSheet.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktBalanceSheetI18nSeedData 一致的实体 slug */
export const BALANCESHEET_ENTITY_SLUG = 'balancesheet'

/** entity.balancesheet._self 静态属性（导入组件 entity-i18n-key 等） */
export const BALANCESHEET_SELF_I18N_KEY = buildEntitySelfI18nKey(BALANCESHEET_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const BALANCESHEET_LIST_FIELDS = [
  'plantCode',
  'periodCode',
  'statementLineCode',
  'statementLineName',
  'accountTitleCode',
  'accountTitleName',
  'lineCategory',
  'balanceDirection',
  'isTotalLine',
  'openingBalance',
  'debitAmount',
  'creditAmount',
  'closingBalance',
  'presentationAmount',
  'priorPeriodAmount',
  'currencyCode',
  'balanceSheetStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const BALANCESHEET_PLACEHOLDER = {
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
  balanceDirection: 'select',
  isTotalLine: 'select',
  openingBalance: 'select',
  debitAmount: 'select',
  creditAmount: 'select',
  closingBalance: 'select',
  presentationAmount: 'select',
  priorPeriodAmount: 'select',
  currencyCode: 'select',
  balanceSheetStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type BalanceSheetField = keyof typeof BALANCESHEET_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const BALANCESHEET_QUERY_STRING_FIELDS = [
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
] as const satisfies readonly (keyof BalanceSheetQuery)[]

export type BalanceSheetQueryField =
  | (typeof BALANCESHEET_QUERY_STRING_FIELDS)[number]
  | 'lineCategory' | 'balanceDirection' | 'isTotalLine' | 'openingBalance' | 'debitAmount' | 'creditAmount' | 'closingBalance' | 'presentationAmount' | 'priorPeriodAmount' | 'balanceSheetStatus'

/** 高级查询抽屉全部字段（含数值） */
export const BALANCESHEET_QUERY_FIELDS: readonly BalanceSheetQueryField[] = [
  ...BALANCESHEET_QUERY_STRING_FIELDS,
  'lineCategory',
  'balanceDirection',
  'isTotalLine',
  'openingBalance',
  'debitAmount',
  'creditAmount',
  'closingBalance',
  'presentationAmount',
  'priorPeriodAmount',
  'balanceSheetStatus',
]

/**
 * 资产负债表行实体字段 i18n：index / balance-sheet-form 统一入口
 */
export function useBalanceSheetI18n() {
  const ef = useEntityFieldI18n(BALANCESHEET_ENTITY_SLUG)

  function ph(field: BalanceSheetField): string {
    return ef.placeholder(field, BALANCESHEET_PLACEHOLDER[field])
  }

  function queryPh(field: BalanceSheetQueryField, kind: EntityFieldPlaceholderKind): string {
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

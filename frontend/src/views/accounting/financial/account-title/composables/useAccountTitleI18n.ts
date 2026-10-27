// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/accounting/financial/account-title/composables
// 文件名称：useAccountTitleI18n.ts
// 功能描述：会计科目实体字段清单 + useAccountTitleI18n（字段名映射一次，文案由 entity.accounttitle.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { AccountTitleQuery } from '@/types/accounting/financial/accountTitle.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktAccountTitleI18nSeedData 一致的实体 slug */
export const ACCOUNTTITLE_ENTITY_SLUG = 'accounttitle'

/** entity.accounttitle._self 静态属性（导入组件 entity-i18n-key 等） */
export const ACCOUNTTITLE_SELF_I18N_KEY = buildEntitySelfI18nKey(ACCOUNTTITLE_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const ACCOUNTTITLE_LIST_FIELDS = [
  'accountTitleCode',
  'accountTitleName',
  'parentId',
  'accountTitleType',
  'balanceDirection',
  'accountTitleLevel',
  'isLeaf',
  'isAuxiliary',
  'auxiliaryType',
  'isQuantity',
  'isCurrency',
  'isCash',
  'isBank',
  'validFrom',
  'validTo',
  'accountTitleStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const ACCOUNTTITLE_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'select',
  accountTitleCode: 'required',
  accountTitleName: 'required',
  parentId: 'required',
  accountTitleType: 'select',
  balanceDirection: 'select',
  accountTitleLevel: 'select',
  isAuxiliary: 'select',
  auxiliaryType: 'select',
  isQuantity: 'select',
  isCurrency: 'select',
  isCash: 'select',
  isBank: 'select',
  validFrom: 'select',
  validTo: 'select',
  accountTitleStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type AccountTitleField = keyof typeof ACCOUNTTITLE_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const ACCOUNTTITLE_QUERY_STRING_FIELDS = [
  'cultureCode',
  'accountTitleCode',
  'accountTitleName',
  'parentId',
  'accountTitleType',
  'auxiliaryType',
  'validFromStart',
  'validFromEnd',
  'validToStart',
  'validToEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof AccountTitleQuery)[]

export type AccountTitleQueryField =
  | (typeof ACCOUNTTITLE_QUERY_STRING_FIELDS)[number]
  | 'balanceDirection' | 'accountTitleLevel' | 'isLeaf' | 'isAuxiliary' | 'isQuantity' | 'isCurrency' | 'isCash' | 'isBank' | 'accountTitleStatus'

/** 高级查询抽屉全部字段（含数值） */
export const ACCOUNTTITLE_QUERY_FIELDS: readonly AccountTitleQueryField[] = [
  ...ACCOUNTTITLE_QUERY_STRING_FIELDS,
  'balanceDirection',
  'accountTitleLevel',
  'isLeaf',
  'isAuxiliary',
  'isQuantity',
  'isCurrency',
  'isCash',
  'isBank',
  'accountTitleStatus',
]

/**
 * 会计科目实体字段 i18n：index / account-title-form 统一入口
 */
export function useAccountTitleI18n() {
  const ef = useEntityFieldI18n(ACCOUNTTITLE_ENTITY_SLUG)

  function ph(field: AccountTitleField): string {
    return ef.placeholder(field, ACCOUNTTITLE_PLACEHOLDER[field])
  }

  function queryPh(field: AccountTitleQueryField, kind: EntityFieldPlaceholderKind): string {
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

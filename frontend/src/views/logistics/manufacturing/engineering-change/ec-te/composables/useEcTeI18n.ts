// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/engineering-change/ec-te/composables
// 文件名称：useEcTeI18n.ts
// 功能描述：设变技术课字段清单 + useEcTeI18n（字段名映射一次，文案由 entity.ecte.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { EcTeQuery } from '@/types/logistics/manufacturing/engineering-change/ecTe.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktEcTeI18nSeedData 一致的实体 slug */
export const ECENG_ENTITY_SLUG = 'ecte'

/** entity.ecte._self 静态属性（导入组件 entity-i18n-key 等） */
export const ECENG_SELF_I18N_KEY = buildEntitySelfI18nKey(ECENG_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const ECENG_LIST_FIELDS = [
  'ecCode',
  'ecIssueDate',
  'changeStatus',
  'ecTitle',
  'ecContent',
  'ecLeader',
  'ecLossAmount',
  'ecImplScope',
  'ecControlScope',
  'ecEntryDate',
  'ecStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const ECENG_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'required',
  ecCode: 'required',
  ecIssueDate: 'select',
  changeStatus: 'select',
  ecTitle: 'required',
  ecContent: 'optional',
  ecLeader: 'select',
  ecLossAmount: 'select',
  ecImplScope: 'select',
  ecControlScope: 'select',
  ecEntryDate: 'select',
  ecStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type EcTeField = keyof typeof ECENG_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const ECENG_QUERY_STRING_FIELDS = [
  'ecCode',
  'ecIssueDateStart',
  'ecIssueDateEnd',
  'ecTitle',
  'ecContent',
  'ecLeader',
  'ecEntryDateStart',
  'ecEntryDateEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof EcTeQuery)[]

export type EcTeQueryField =
  | (typeof ECENG_QUERY_STRING_FIELDS)[number]
  | 'changeStatus' | 'ecLossAmount' | 'ecImplScope' | 'ecControlScope' | 'ecStatus'

/** 高级查询抽屉全部字段（含数值；顺序与查询抽屉表单项一致） */
export const ECENG_QUERY_FIELDS: readonly EcTeQueryField[] = [
  'ecCode',
  'ecIssueDateStart',
  'ecIssueDateEnd',
  'changeStatus',
  'ecTitle',
  'ecContent',
  'ecLeader',
  'ecLossAmount',
  'ecImplScope',
  'ecControlScope',
  'ecEntryDateStart',
  'ecEntryDateEnd',
  'ecStatus',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
]

/**
 * 设变技术课字段 i18n：index / ec-te-form 统一入口
 */
export function useEcTeI18n() {
  const ef = useEntityFieldI18n(ECENG_ENTITY_SLUG)

  function ph(field: EcTeField): string {
    return ef.placeholder(field, ECENG_PLACEHOLDER[field])
  }

  function queryPh(field: EcTeQueryField, kind: EntityFieldPlaceholderKind): string {
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

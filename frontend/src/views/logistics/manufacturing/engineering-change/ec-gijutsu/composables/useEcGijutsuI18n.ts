// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/engineering-change/ec-gijutsu/composables
// 文件名称：useEcGijutsuI18n.ts
// 功能描述：设变技术课字段清单 + useEcGijutsuI18n（字段名映射一次，文案由 entity.ecgijutsu.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { EcGijutsuQuery } from '@/types/logistics/manufacturing/engineering-change/ecGijutsu.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktEcGijutsuI18nSeedData 一致的实体 slug */
export const ECGIJUTSU_ENTITY_SLUG = 'ecgijutsu'

/** entity.ecgijutsu._self 静态属性（导入组件 entity-i18n-key 等） */
export const ECGIJUTSU_SELF_I18N_KEY = buildEntitySelfI18nKey(ECGIJUTSU_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const ECGIJUTSU_LIST_FIELDS = [
  'ecCode',
  'ecIssueDate',
  'changeStatus',
  'ecTitle',
  'ecContent',
  'ecLeader',
  'ecLossAmount',
  'ecScope',
  'ecDeptScope',
  'ecEntryDate',
  'ecStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const ECGIJUTSU_PLACEHOLDER = {
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
  ecScope: 'select',
  ecDeptScope: 'select',
  ecEntryDate: 'select',
  ecStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type EcGijutsuField = keyof typeof ECGIJUTSU_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const ECGIJUTSU_QUERY_STRING_FIELDS = [
  'plantCode',
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
] as const satisfies readonly (keyof EcGijutsuQuery)[]

export type EcGijutsuQueryField =
  | (typeof ECGIJUTSU_QUERY_STRING_FIELDS)[number]
  | 'changeStatus' | 'ecLossAmount' | 'ecScope' | 'ecDeptScope' | 'ecStatus'

/** 高级查询抽屉全部字段（含数值；顺序与查询抽屉表单项一致） */
export const ECGIJUTSU_QUERY_FIELDS: readonly EcGijutsuQueryField[] = [
  'plantCode',
  'ecCode',
  'ecIssueDateStart',
  'ecIssueDateEnd',
  'changeStatus',
  'ecTitle',
  'ecContent',
  'ecLeader',
  'ecLossAmount',
  'ecScope',
  'ecDeptScope',
  'ecEntryDateStart',
  'ecEntryDateEnd',
  'ecStatus',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
]

/**
 * 设变技术课字段 i18n：index / ec-gijutsu-form 统一入口
 */
export function useEcGijutsuI18n() {
  const ef = useEntityFieldI18n(ECGIJUTSU_ENTITY_SLUG)

  function ph(field: EcGijutsuField): string {
    return ef.placeholder(field, ECGIJUTSU_PLACEHOLDER[field])
  }

  function queryPh(field: EcGijutsuQueryField, kind: EntityFieldPlaceholderKind): string {
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

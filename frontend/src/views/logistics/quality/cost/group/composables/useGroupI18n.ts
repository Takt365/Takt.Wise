// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/quality/cost/group/composables
// 文件名称：useGroupI18n.ts
// 功能描述：成本组主数据实体字段清单 + useCostGroupI18n（字段名映射一次，文案由 entity.costgroup.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CostGroupQuery } from '@/types/logistics/quality/cost/group.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktCostGroupI18nSeedData 一致的实体 slug */
export const COSTGROUP_ENTITY_SLUG = 'costgroup'

/** entity.costgroup._self 静态属性（导入组件 entity-i18n-key 等） */
export const COSTGROUP_SELF_I18N_KEY = buildEntitySelfI18nKey(COSTGROUP_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const COSTGROUP_LIST_FIELDS = [
  'costGroupCode',
  'costGroupName',
  'costGroupDescription',
  'costGroupCategory',
  'contactPhone',
  'contactEmail',
  'isBuiltIn',
  'groupStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const COSTGROUP_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  costGroupCode: 'required',
  costGroupName: 'required',
  costGroupDescription: 'optional',
  costGroupCategory: 'select',
  contactPhone: 'optional',
  contactEmail: 'optional',
  isBuiltIn: 'select',
  groupStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type CostGroupField = keyof typeof COSTGROUP_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const COSTGROUP_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'costGroupCode',
  'costGroupName',
  'costGroupDescription',
  'contactPhone',
  'contactEmail',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof CostGroupQuery)[]

export type CostGroupQueryField =
  | (typeof COSTGROUP_QUERY_STRING_FIELDS)[number]
  | 'costGroupCategory' | 'isBuiltIn' | 'groupStatus'

/** 高级查询抽屉全部字段（含数值） */
export const COSTGROUP_QUERY_FIELDS: readonly CostGroupQueryField[] = [
  ...COSTGROUP_QUERY_STRING_FIELDS,
  'costGroupCategory',
  'isBuiltIn',
  'groupStatus',
]

/**
 * 成本组主数据实体字段 i18n：index / CostGroupForm 统一入口
 */
export function useCostGroupI18n() {
  const ef = useEntityFieldI18n(COSTGROUP_ENTITY_SLUG)

  function ph(field: CostGroupField): string {
    return ef.placeholder(field, COSTGROUP_PLACEHOLDER[field])
  }

  function queryPh(field: CostGroupQueryField, kind: EntityFieldPlaceholderKind): string {
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

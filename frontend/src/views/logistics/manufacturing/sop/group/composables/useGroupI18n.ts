// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/sop/group/composables
// 文件名称：useGroupI18n.ts
// 功能描述：SOP组主数据实体字段清单 + useSopGroupI18n（字段名映射一次，文案由 entity.sopgroup.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SopGroupQuery } from '@/types/logistics/manufacturing/sop/group.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSopGroupI18nSeedData 一致的实体 slug */
export const SOPGROUP_ENTITY_SLUG = 'sopgroup'

/** entity.sopgroup._self 静态属性（导入组件 entity-i18n-key 等） */
export const SOPGROUP_SELF_I18N_KEY = buildEntitySelfI18nKey(SOPGROUP_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SOPGROUP_LIST_FIELDS = [
  'sopGroupCode',
  'sopGroupName',
  'sopGroupDescription',
  'sopGroupCategory',
  'contactPhone',
  'contactEmail',
  'isBuiltIn',
  'groupStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SOPGROUP_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  sopGroupCode: 'required',
  sopGroupName: 'required',
  sopGroupDescription: 'optional',
  sopGroupCategory: 'select',
  contactPhone: 'optional',
  contactEmail: 'optional',
  isBuiltIn: 'select',
  groupStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SopGroupField = keyof typeof SOPGROUP_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SOPGROUP_QUERY_STRING_FIELDS = [
  'cultureCode',
  'sopGroupCode',
  'sopGroupName',
  'sopGroupDescription',
  'contactPhone',
  'contactEmail',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SopGroupQuery)[]

export type SopGroupQueryField =
  | (typeof SOPGROUP_QUERY_STRING_FIELDS)[number]
  | 'sopGroupCategory' | 'isBuiltIn' | 'groupStatus'

/** 高级查询抽屉全部字段（含数值） */
export const SOPGROUP_QUERY_FIELDS: readonly SopGroupQueryField[] = [
  ...SOPGROUP_QUERY_STRING_FIELDS,
  'sopGroupCategory',
  'isBuiltIn',
  'groupStatus',
]

/**
 * SOP组主数据实体字段 i18n：index / SopGroupForm 统一入口
 */
export function useSopGroupI18n() {
  const ef = useEntityFieldI18n(SOPGROUP_ENTITY_SLUG)

  function ph(field: SopGroupField): string {
    return ef.placeholder(field, SOPGROUP_PLACEHOLDER[field])
  }

  function queryPh(field: SopGroupQueryField, kind: EntityFieldPlaceholderKind): string {
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

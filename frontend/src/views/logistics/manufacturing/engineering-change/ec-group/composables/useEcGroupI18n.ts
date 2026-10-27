// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/engineering-change/ec-group/composables
// 文件名称：useEcGroupI18n.ts
// 功能描述：设变组主数据实体字段清单 + useEcGroupI18n（字段名映射一次，文案由 entity.ecgroup.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { EcGroupQuery } from '@/types/logistics/manufacturing/engineering-change/ecGroup.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktEcGroupI18nSeedData 一致的实体 slug */
export const ECGROUP_ENTITY_SLUG = 'ecgroup'

/** entity.ecgroup._self 静态属性（导入组件 entity-i18n-key 等） */
export const ECGROUP_SELF_I18N_KEY = buildEntitySelfI18nKey(ECGROUP_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const ECGROUP_LIST_FIELDS = [
  'plantCode',
  'ecGroupCode',
  'ecGroupName',
  'ecGroupDescription',
  'ecGroupCategory',
  'contactPhone',
  'contactEmail',
  'isBuiltIn',
  'groupStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const ECGROUP_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'select',
  ecGroupCode: 'required',
  ecGroupName: 'required',
  ecGroupDescription: 'optional',
  ecGroupCategory: 'select',
  contactPhone: 'optional',
  contactEmail: 'optional',
  isBuiltIn: 'select',
  groupStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type EcGroupField = keyof typeof ECGROUP_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const ECGROUP_QUERY_STRING_FIELDS = [
  'ecGroupCode',
  'ecGroupName',
  'ecGroupDescription',
  'contactPhone',
  'contactEmail',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof EcGroupQuery)[]

export type EcGroupQueryField =
  | (typeof ECGROUP_QUERY_STRING_FIELDS)[number]
  | 'ecGroupCategory' | 'isBuiltIn' | 'groupStatus'

/** 高级查询抽屉全部字段（含数值） */
export const ECGROUP_QUERY_FIELDS: readonly EcGroupQueryField[] = [
  ...ECGROUP_QUERY_STRING_FIELDS,
  'ecGroupCategory',
  'isBuiltIn',
  'groupStatus',
]

/**
 * 设变组主数据实体字段 i18n：index / ec-group-form 统一入口
 */
export function useEcGroupI18n() {
  const ef = useEntityFieldI18n(ECGROUP_ENTITY_SLUG)

  function ph(field: EcGroupField): string {
    return ef.placeholder(field, ECGROUP_PLACEHOLDER[field])
  }

  function queryPh(field: EcGroupQueryField, kind: EntityFieldPlaceholderKind): string {
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

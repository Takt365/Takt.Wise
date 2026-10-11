// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/bom/group/composables
// 文件名称：useGroupI18n.ts
// 功能描述：BOM组主数据实体字段清单 + useBomGroupI18n（字段名映射一次，文案由 entity.bomgroup.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { BomGroupQuery } from '@/types/logistics/manufacturing/bom/group.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktBomGroupI18nSeedData 一致的实体 slug */
export const BOMGROUP_ENTITY_SLUG = 'bomgroup'

/** entity.bomgroup._self 静态属性（导入组件 entity-i18n-key 等） */
export const BOMGROUP_SELF_I18N_KEY = buildEntitySelfI18nKey(BOMGROUP_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const BOMGROUP_LIST_FIELDS = [
  'bomGroupCode',
  'bomGroupName',
  'bomGroupDescription',
  'bomGroupCategory',
  'contactPhone',
  'contactEmail',
  'isBuiltIn',
  'groupStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const BOMGROUP_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  bomGroupCode: 'required',
  bomGroupName: 'required',
  bomGroupDescription: 'optional',
  bomGroupCategory: 'select',
  contactPhone: 'optional',
  contactEmail: 'optional',
  isBuiltIn: 'select',
  groupStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type BomGroupField = keyof typeof BOMGROUP_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const BOMGROUP_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'bomGroupCode',
  'bomGroupName',
  'bomGroupDescription',
  'contactPhone',
  'contactEmail',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof BomGroupQuery)[]

export type BomGroupQueryField =
  | (typeof BOMGROUP_QUERY_STRING_FIELDS)[number]
  | 'bomGroupCategory' | 'isBuiltIn' | 'groupStatus'

/** 高级查询抽屉全部字段（含数值） */
export const BOMGROUP_QUERY_FIELDS: readonly BomGroupQueryField[] = [
  ...BOMGROUP_QUERY_STRING_FIELDS,
  'bomGroupCategory',
  'isBuiltIn',
  'groupStatus',
]

/**
 * BOM组主数据实体字段 i18n：index / BomGroupForm 统一入口
 */
export function useBomGroupI18n() {
  const ef = useEntityFieldI18n(BOMGROUP_ENTITY_SLUG)

  function ph(field: BomGroupField): string {
    return ef.placeholder(field, BOMGROUP_PLACEHOLDER[field])
  }

  function queryPh(field: BomGroupQueryField, kind: EntityFieldPlaceholderKind): string {
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

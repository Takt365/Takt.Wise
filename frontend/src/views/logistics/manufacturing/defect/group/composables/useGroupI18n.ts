// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/defect/group/composables
// 文件名称：useGroupI18n.ts
// 功能描述：不良组主数据实体字段清单 + useDefectGroupI18n（字段名映射一次，文案由 entity.defectgroup.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { DefectGroupQuery } from '@/types/logistics/manufacturing/defect/group.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktDefectGroupI18nSeedData 一致的实体 slug */
export const DEFECTGROUP_ENTITY_SLUG = 'defectgroup'

/** entity.defectgroup._self 静态属性（导入组件 entity-i18n-key 等） */
export const DEFECTGROUP_SELF_I18N_KEY = buildEntitySelfI18nKey(DEFECTGROUP_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const DEFECTGROUP_LIST_FIELDS = [
  'defectGroupCode',
  'defectGroupName',
  'defectGroupDescription',
  'defectGroupCategory',
  'contactPhone',
  'contactEmail',
  'isBuiltIn',
  'groupStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const DEFECTGROUP_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  defectGroupCategory: 'select',
  defectGroupCode: 'required',
  defectGroupName: 'required',
  defectGroupDescription: 'optional',
  contactPhone: 'optional',
  contactEmail: 'optional',
  isBuiltIn: 'select',
  groupStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type DefectGroupField = keyof typeof DEFECTGROUP_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const DEFECTGROUP_QUERY_STRING_FIELDS = [
  'cultureCode',
  'defectGroupCode',
  'defectGroupName',
  'defectGroupDescription',
  'contactPhone',
  'contactEmail',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof DefectGroupQuery)[]

export type DefectGroupQueryField =
  | (typeof DEFECTGROUP_QUERY_STRING_FIELDS)[number]
  | 'defectGroupCategory' | 'isBuiltIn' | 'groupStatus'

/** 高级查询抽屉全部字段（含数值） */
export const DEFECTGROUP_QUERY_FIELDS: readonly DefectGroupQueryField[] = [
  ...DEFECTGROUP_QUERY_STRING_FIELDS,
  'defectGroupCategory',
  'isBuiltIn',
  'groupStatus',
]

/**
 * 不良组主数据实体字段 i18n：index / group-form 统一入口
 */
export function useDefectGroupI18n() {
  const ef = useEntityFieldI18n(DEFECTGROUP_ENTITY_SLUG)

  function ph(field: DefectGroupField): string {
    return ef.placeholder(field, DEFECTGROUP_PLACEHOLDER[field])
  }

  function queryPh(field: DefectGroupQueryField, kind: EntityFieldPlaceholderKind): string {
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

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/foundation/admin-division/composables
// 文件名称：useAdminDivisionI18n.ts
// 功能描述：行政区划实体字段清单 + useAdminDivisionI18n（字段名映射一次，文案由 entity.admindivision.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { AdminDivisionQuery } from '@/types/foundation/adminDivision.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktAdminDivisionI18nSeedData 一致的实体 slug */
export const ADMINDIVISION_ENTITY_SLUG = 'admindivision'

/** entity.admindivision._self 静态属性（导入组件 entity-i18n-key 等） */
export const ADMINDIVISION_SELF_I18N_KEY = buildEntitySelfI18nKey(ADMINDIVISION_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const ADMINDIVISION_LIST_FIELDS = [
  'countryCode',
  'divisionCode',
  'divisionName',
  'parentId',
  'level',
  'divisionPath',
  'isLeaf',
  'postalCode',
  'currencyCode',
  'phoneCode',
  'isBuiltIn',
  'divisionStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const ADMINDIVISION_PLACEHOLDER = {
  tenantCode: 'optional',
  countryCode: 'select',
  divisionCode: 'required',
  divisionName: 'required',
  parentId: 'required',
  divisionPath: 'required',
  postalCode: 'optional',
  currencyCode: 'select',
  phoneCode: 'required',
  isBuiltIn: 'select',
  divisionStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type AdminDivisionField = keyof typeof ADMINDIVISION_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const ADMINDIVISION_QUERY_STRING_FIELDS = [
  'countryCode',
  'divisionCode',
  'divisionName',
  'parentId',
  'divisionPath',
  'postalCode',
  'currencyCode',
  'phoneCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof AdminDivisionQuery)[]

export type AdminDivisionQueryField =
  | (typeof ADMINDIVISION_QUERY_STRING_FIELDS)[number]
  | 'level' | 'isLeaf' | 'isBuiltIn' | 'divisionStatus'

/** 高级查询抽屉全部字段（含数值） */
export const ADMINDIVISION_QUERY_FIELDS: readonly AdminDivisionQueryField[] = [
  ...ADMINDIVISION_QUERY_STRING_FIELDS,
  'level',
  'isLeaf',
  'isBuiltIn',
  'divisionStatus',
]

/**
 * 行政区划实体字段 i18n：index / admin-division-form 统一入口
 */
export function useAdminDivisionI18n() {
  const ef = useEntityFieldI18n(ADMINDIVISION_ENTITY_SLUG)

  function ph(field: AdminDivisionField): string {
    return ef.placeholder(field, ADMINDIVISION_PLACEHOLDER[field])
  }

  function queryPh(field: AdminDivisionQueryField, kind: EntityFieldPlaceholderKind): string {
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

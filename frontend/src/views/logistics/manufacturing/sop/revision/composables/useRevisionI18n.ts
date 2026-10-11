// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/sop/revision/composables
// 文件名称：useRevisionI18n.ts
// 功能描述：SOP 版本实体字段清单 + useSopRevisionI18n（字段名映射一次，文案由 entity.soprevision.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SopRevisionQuery } from '@/types/logistics/manufacturing/sop/revision.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSopRevisionI18nSeedData 一致的实体 slug */
export const SOPREVISION_ENTITY_SLUG = 'soprevision'

/** entity.soprevision._self 静态属性（导入组件 entity-i18n-key 等） */
export const SOPREVISION_SELF_I18N_KEY = buildEntitySelfI18nKey(SOPREVISION_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SOPREVISION_LIST_FIELDS = [
  'sopId',
  'revision',
  'fileUrl',
  'changeDesc',
  'ecnId',
  'isLocked',
  'forceLeaderAck',
  'revisionStatus',
  'effectiveRule',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SOPREVISION_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  sopId: 'select',
  revision: 'required',
  fileUrl: 'optional',
  changeDesc: 'optional',
  ecnId: 'optional',
  isLocked: 'select',
  forceLeaderAck: 'select',
  revisionStatus: 'select',
  effectiveRule: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SopRevisionField = keyof typeof SOPREVISION_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SOPREVISION_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'sopId',
  'revision',
  'fileUrl',
  'changeDesc',
  'ecnId',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SopRevisionQuery)[]

export type SopRevisionQueryField =
  | (typeof SOPREVISION_QUERY_STRING_FIELDS)[number]
  | 'isLocked' | 'forceLeaderAck' | 'revisionStatus' | 'effectiveRule'

/** 高级查询抽屉全部字段（含数值） */
export const SOPREVISION_QUERY_FIELDS: readonly SopRevisionQueryField[] = [
  ...SOPREVISION_QUERY_STRING_FIELDS,
  'isLocked',
  'forceLeaderAck',
  'revisionStatus',
  'effectiveRule',
]

/**
 * SOP 版本实体字段 i18n：index / revision-form 统一入口
 */
export function useSopRevisionI18n() {
  const ef = useEntityFieldI18n(SOPREVISION_ENTITY_SLUG)

  function ph(field: SopRevisionField): string {
    return ef.placeholder(field, SOPREVISION_PLACEHOLDER[field])
  }

  function queryPh(field: SopRevisionQueryField, kind: EntityFieldPlaceholderKind): string {
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

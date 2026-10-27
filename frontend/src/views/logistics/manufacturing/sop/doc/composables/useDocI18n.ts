// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/sop/doc/composables
// 文件名称：useDocI18n.ts
// 功能描述：SOP 文档头实体字段清单 + useSopDocI18n（字段名映射一次，文案由 entity.sopdoc.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SopDocQuery } from '@/types/logistics/manufacturing/sop/doc.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSopDocI18nSeedData 一致的实体 slug */
export const SOPDOC_ENTITY_SLUG = 'sopdoc'

/** entity.sopdoc._self 静态属性（导入组件 entity-i18n-key 等） */
export const SOPDOC_SELF_I18N_KEY = buildEntitySelfI18nKey(SOPDOC_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SOPDOC_LIST_FIELDS = [
  'sopCode',
  'sopName',
  'materialCode',
  'routingItemId',
  'workstationId',
  'currentRevisionId',
  'sopStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SOPDOC_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  sopCode: 'required',
  sopName: 'required',
  materialCode: 'select',
  routingItemId: 'select',
  workstationId: 'optional',
  currentRevisionId: 'optional',
  sopStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SopDocField = keyof typeof SOPDOC_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SOPDOC_QUERY_STRING_FIELDS = [
  'cultureCode',
  'sopCode',
  'sopName',
  'materialCode',
  'routingItemId',
  'workstationId',
  'currentRevisionId',
  'initiatorId',
  'initiatedAtStart',
  'initiatedAtEnd',
  'approvedBy',
  'approvedAtStart',
  'approvedAtEnd',
  'flowInstanceId',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SopDocQuery)[]

export type SopDocQueryField =
  | (typeof SOPDOC_QUERY_STRING_FIELDS)[number]
  | 'sopStatus' | 'approvalStatus'

/** 高级查询抽屉全部字段（含数值） */
export const SOPDOC_QUERY_FIELDS: readonly SopDocQueryField[] = [
  ...SOPDOC_QUERY_STRING_FIELDS,
  'sopStatus',
  'approvalStatus',
]

/**
 * SOP 文档头实体字段 i18n：index / doc-form 统一入口
 */
export function useSopDocI18n() {
  const ef = useEntityFieldI18n(SOPDOC_ENTITY_SLUG)

  function ph(field: SopDocField): string {
    return ef.placeholder(field, SOPDOC_PLACEHOLDER[field])
  }

  function queryPh(field: SopDocQueryField, kind: EntityFieldPlaceholderKind): string {
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

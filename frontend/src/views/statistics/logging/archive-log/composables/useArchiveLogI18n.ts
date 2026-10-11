// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/statistics/logging/archive-log/composables
// 文件名称：useArchiveLogI18n.ts
// 功能描述：归档日志字段清单 + useArchiveLogI18n（字段名映射一次，文案由 entity.archivelog.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { ArchiveLogQuery } from '@/types/statistics/logging/archiveLog.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktArchiveLogI18nSeedData 一致的实体 slug */
export const ARCHIVELOG_ENTITY_SLUG = 'archivelog'

/** entity.archivelog._self 静态属性（导入组件 entity-i18n-key 等） */
export const ARCHIVELOG_SELF_I18N_KEY = buildEntitySelfI18nKey(ARCHIVELOG_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const ARCHIVELOG_LIST_FIELDS = [
  'archiveKind',
  'sourceId',
  'sourceName',
  'targetName',
  'archiveYear',
  'sourceCount',
  'archivedCount',
  'deletedCount',
  'runStatus',
  'errorMessage',
  'startedAt',
  'finishedAt',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const ARCHIVELOG_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  archiveKind: 'required',
  sourceId: 'required',
  sourceName: 'required',
  targetName: 'required',
  archiveYear: 'optional',
  sourceCount: 'select',
  archivedCount: 'select',
  deletedCount: 'select',
  runStatus: 'select',
  errorMessage: 'optional',
  startedAt: 'select',
  finishedAt: 'optional',
  extField: 'optional',
  remark: 'optional',
  plantCode: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type ArchiveLogField = keyof typeof ARCHIVELOG_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const ARCHIVELOG_QUERY_STRING_FIELDS = [
  'archiveKind',
  'sourceId',
  'sourceName',
  'targetName',
  'errorMessage',
  'startedAtStart',
  'startedAtEnd',
  'finishedAtStart',
  'finishedAtEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof ArchiveLogQuery)[]

export type ArchiveLogQueryField =
  | (typeof ARCHIVELOG_QUERY_STRING_FIELDS)[number]
  | 'archiveYear' | 'sourceCount' | 'archivedCount' | 'deletedCount' | 'runStatus'

/** 高级查询抽屉全部字段（含数值） */
export const ARCHIVELOG_QUERY_FIELDS: readonly ArchiveLogQueryField[] = [
  ...ARCHIVELOG_QUERY_STRING_FIELDS,
  'archiveYear',
  'sourceCount',
  'archivedCount',
  'deletedCount',
  'runStatus',
]

/**
 * 归档日志字段 i18n：index / archive-log-form 统一入口
 */
export function useArchiveLogI18n() {
  const ef = useEntityFieldI18n(ARCHIVELOG_ENTITY_SLUG)

  function ph(field: ArchiveLogField): string {
    return ef.placeholder(field, ARCHIVELOG_PLACEHOLDER[field])
  }

  function queryPh(field: ArchiveLogQueryField, kind: EntityFieldPlaceholderKind): string {
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

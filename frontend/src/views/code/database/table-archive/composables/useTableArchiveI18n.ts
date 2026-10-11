// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/code/database/table-archive/composables
// 文件名称：useTableArchiveI18n.ts
// 功能描述：数据表归档字段清单 + useTableArchiveI18n（entity.tablearchive.* 动态解析）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TableArchiveQuery } from '@/types/code/database/tableArchive.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktTableArchiveI18nSeedData 一致的实体 slug */
export const TABLE_ARCHIVE_ENTITY_SLUG = 'tablearchive'

/** entity.tablearchive._self */
export const TABLE_ARCHIVE_SELF_I18N_KEY = buildEntitySelfI18nKey(TABLE_ARCHIVE_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const TABLE_ARCHIVE_LIST_FIELDS = [
  'targetTenantCode',
  'targetDatabaseName',
  'tableName',
  'archiveKeyColumn',
  'archiveKeyKind',
  'retainHotYears',
  'archiveName',
  'sortOrder',
  'archiveStatus',
] as const

/** 表单占位类型 */
export const TABLE_ARCHIVE_PLACEHOLDER = {
  targetTenantCode: 'required',
  targetDatabaseName: 'required',
  tableName: 'select',
  archiveKeyColumn: 'select',
  archiveKeyKind: 'select',
  retainHotYears: 'optional',
  archiveName: 'optional',
  sortOrder: 'optional',
  archiveStatus: 'select',
  extField: 'optional',
  remark: 'optional',
  plantCode: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

export type TableArchiveField = keyof typeof TABLE_ARCHIVE_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const TABLE_ARCHIVE_QUERY_STRING_FIELDS = [
  'targetTenantCode',
  'targetDatabaseName',
  'tableName',
  'archiveKeyColumn',
  'archiveName',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof TableArchiveQuery)[]

export type TableArchiveQueryField =
  | (typeof TABLE_ARCHIVE_QUERY_STRING_FIELDS)[number]
  | 'archiveKeyKind'
  | 'retainHotYears'
  | 'sortOrder'
  | 'archiveStatus'

/** 高级查询抽屉全部字段 */
export const TABLE_ARCHIVE_QUERY_FIELDS: readonly TableArchiveQueryField[] = [
  ...TABLE_ARCHIVE_QUERY_STRING_FIELDS,
  'archiveKeyKind',
  'retainHotYears',
  'sortOrder',
  'archiveStatus',
]

/**
 * 数据表归档字段 i18n：index / table-archive-form 统一入口
 */
export function useTableArchiveI18n() {
  const ef = useEntityFieldI18n(TABLE_ARCHIVE_ENTITY_SLUG)

  function ph(field: TableArchiveField): string {
    return ef.placeholder(field, TABLE_ARCHIVE_PLACEHOLDER[field])
  }

  function queryPh(field: TableArchiveQueryField, kind: EntityFieldPlaceholderKind): string {
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

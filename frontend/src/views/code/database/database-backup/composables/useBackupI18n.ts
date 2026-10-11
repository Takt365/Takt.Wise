// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/code/database/databaseBackup.types/composables
// 文件名称：useBackupI18n.ts
// 功能描述：数据库备份记录字段清单 + useDatabaseBackupI18n（字段名映射一次，文案由 entity.databasebackup.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { DatabaseBackupQuery } from '@/types/code/database/backup.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktDatabaseBackupI18nSeedData 一致的实体 slug */
export const DATABASEBACKUP_ENTITY_SLUG = 'databasebackup'

/** entity.databasebackup._self 静态属性（导入组件 entity-i18n-key 等） */
export const DATABASEBACKUP_SELF_I18N_KEY = buildEntitySelfI18nKey(DATABASEBACKUP_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const DATABASEBACKUP_LIST_FIELDS = [
  'backupCode',
  'targetTenantCode',
  'targetDatabaseName',
  'backupType',
  'backupPathType',
  'executeMode',
  'backupPath',
  'backupFileName',
  'scheduledAt',
  'lastRunAt',
  'quartzTaskId',
  'backupStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const DATABASEBACKUP_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  backupCode: 'required',
  targetTenantCode: 'required',
  targetDatabaseName: 'required',
  backupType: 'select',
  backupPathType: 'select',
  executeMode: 'select',
  backupPath: 'required',
  backupHost: 'optional',
  backupPort: 'optional',
  backupUserName: 'optional',
  backupPassword: 'optional',
  backupFileName: 'optional',
  scheduledAt: 'optional',
  lastRunAt: 'optional',
  quartzTaskId: 'optional',
  backupStatus: 'select',
  extField: 'optional',
  remark: 'optional',
  plantCode: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type DatabaseBackupField = keyof typeof DATABASEBACKUP_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const DATABASEBACKUP_QUERY_STRING_FIELDS = [
  'backupCode',
  'targetTenantCode',
  'targetDatabaseName',
  'backupPath',
  'backupFileName',
  'lastRunAtStart',
  'lastRunAtEnd',
  'scheduledAtStart',
  'scheduledAtEnd',
  'quartzTaskId',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof DatabaseBackupQuery)[]

export type DatabaseBackupQueryField =
  | (typeof DATABASEBACKUP_QUERY_STRING_FIELDS)[number]
  | 'backupType' | 'backupPathType' | 'executeMode' | 'backupStatus'

/** 高级查询抽屉全部字段（含数值） */
export const DATABASEBACKUP_QUERY_FIELDS: readonly DatabaseBackupQueryField[] = [
  ...DATABASEBACKUP_QUERY_STRING_FIELDS,
  'backupType',
  'backupPathType',
  'executeMode',
  'backupStatus',
]

/**
 * 数据库备份记录字段 i18n：index / backup-form 统一入口
 */
export function useDatabaseBackupI18n() {
  const ef = useEntityFieldI18n(DATABASEBACKUP_ENTITY_SLUG)

  function ph(field: DatabaseBackupField): string {
    return ef.placeholder(field, DATABASEBACKUP_PLACEHOLDER[field])
  }

  function queryPh(field: DatabaseBackupQueryField, kind: EntityFieldPlaceholderKind): string {
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

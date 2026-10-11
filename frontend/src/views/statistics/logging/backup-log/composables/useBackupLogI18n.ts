// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/statistics/logging/backup-log/composables
// 文件名称：useBackupLogI18n.ts
// 功能描述：备份日志字段清单 + useBackupLogI18n（字段名映射一次，文案由 entity.backuplog.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { BackupLogQuery } from '@/types/statistics/logging/backupLog.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktBackupLogI18nSeedData 一致的实体 slug */
export const BACKUPLOG_ENTITY_SLUG = 'backuplog'

/** entity.backuplog._self 静态属性（导入组件 entity-i18n-key 等） */
export const BACKUPLOG_SELF_I18N_KEY = buildEntitySelfI18nKey(BACKUPLOG_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const BACKUPLOG_LIST_FIELDS = [
  'backupKind',
  'sourceId',
  'sourceName',
  'sourceCode',
  'targetName',
  'targetScope',
  'syncMode',
  'executeMode',
  'pathType',
  'resultPath',
  'fileSizeBytes',
  'runStatus',
  'errorMessage',
  'startedAt',
  'finishedAt',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const BACKUPLOG_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  backupKind: 'required',
  sourceId: 'required',
  sourceCode: 'required',
  targetName: 'required',
  targetScope: 'optional',
  syncMode: 'select',
  executeMode: 'select',
  pathType: 'select',
  resultPath: 'optional',
  fileSizeBytes: 'required',
  runStatus: 'select',
  errorMessage: 'optional',
  startedAt: 'select',
  finishedAt: 'optional',
  extField: 'optional',
  remark: 'optional',
  plantCode: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type BackupLogField = keyof typeof BACKUPLOG_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const BACKUPLOG_QUERY_STRING_FIELDS = [
  'backupKind',
  'sourceId',
  'sourceCode',
  'targetName',
  'targetScope',
  'resultPath',
  'fileSizeBytes',
  'errorMessage',
  'startedAtStart',
  'startedAtEnd',
  'finishedAtStart',
  'finishedAtEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof BackupLogQuery)[]

export type BackupLogQueryField =
  | (typeof BACKUPLOG_QUERY_STRING_FIELDS)[number]
  | 'syncMode' | 'executeMode' | 'pathType' | 'runStatus'

/** 高级查询抽屉全部字段（含数值） */
export const BACKUPLOG_QUERY_FIELDS: readonly BackupLogQueryField[] = [
  ...BACKUPLOG_QUERY_STRING_FIELDS,
  'syncMode',
  'executeMode',
  'pathType',
  'runStatus',
]

/**
 * 备份日志字段 i18n：index / backup-log-form 统一入口
 */
export function useBackupLogI18n() {
  const ef = useEntityFieldI18n(BACKUPLOG_ENTITY_SLUG)

  function ph(field: BackupLogField): string {
    return ef.placeholder(field, BACKUPLOG_PLACEHOLDER[field])
  }

  function queryPh(field: BackupLogQueryField, kind: EntityFieldPlaceholderKind): string {
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

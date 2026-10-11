// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/statistics/logging/quartz-log/composables
// 文件名称：useQuartzLogI18n.ts
// 功能描述：Quartz 任务执行日志实体字段清单 + useQuartzLogI18n（字段名映射一次，文案由 entity.quartzlog.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { QuartzLogQuery } from '@/types/statistics/logging/quartzLog.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktQuartzLogI18nSeedData 一致的实体 slug */
export const QUARTZLOG_ENTITY_SLUG = 'quartzlog'

/** entity.quartzlog._self 静态属性（导入组件 entity-i18n-key 等） */
export const QUARTZLOG_SELF_I18N_KEY = buildEntitySelfI18nKey(QUARTZLOG_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const QUARTZLOG_LIST_FIELDS = [
  'quartzTaskId',
  'taskName',
  'jobGroup',
  'taskType',
  'executeTime',
  'executeDuration',
  'executeParams',
  'executeMessage',
  'errorInfo',
  'executeIp',
  'executeHost',
  'executeStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const QUARTZLOG_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'select',
  quartzTaskId: 'required',
  taskName: 'required',
  jobGroup: 'select',
  taskType: 'select',
  executeTime: 'select',
  executeDuration: 'required',
  executeParams: 'required',
  executeMessage: 'required',
  errorInfo: 'required',
  executeIp: 'required',
  executeHost: 'required',
  executeStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type QuartzLogField = keyof typeof QUARTZLOG_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const QUARTZLOG_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'quartzTaskId',
  'taskName',
  'jobGroup',
  'taskType',
  'executeTimeStart',
  'executeTimeEnd',
  'executeDuration',
  'executeParams',
  'executeMessage',
  'errorInfo',
  'executeIp',
  'executeHost',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof QuartzLogQuery)[]

export type QuartzLogQueryField =
  | (typeof QUARTZLOG_QUERY_STRING_FIELDS)[number]
  | 'executeStatus'

/** 高级查询抽屉全部字段（含数值） */
export const QUARTZLOG_QUERY_FIELDS: readonly QuartzLogQueryField[] = [
  ...QUARTZLOG_QUERY_STRING_FIELDS,
  'executeStatus',
]

/**
 * Quartz 任务执行日志实体字段 i18n：index / quartz-log-form 统一入口
 */
export function useQuartzLogI18n() {
  const ef = useEntityFieldI18n(QUARTZLOG_ENTITY_SLUG)

  function ph(field: QuartzLogField): string {
    return ef.placeholder(field, QUARTZLOG_PLACEHOLDER[field])
  }

  function queryPh(field: QuartzLogQueryField, kind: EntityFieldPlaceholderKind): string {
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

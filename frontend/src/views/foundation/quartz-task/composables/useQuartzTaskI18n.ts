// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/foundation/quartz-task/composables
// 文件名称：useQuartzTaskI18n.ts
// 功能描述：Quartz 定时任务实体字段清单 + useQuartzTaskI18n（字段名映射一次，文案由 entity.quartztask.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { QuartzTaskQuery } from '@/types/foundation/quartzTask.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktQuartzTaskI18nSeedData 一致的实体 slug */
export const QUARTZTASK_ENTITY_SLUG = 'quartztask'

/** entity.quartztask._self 静态属性（导入组件 entity-i18n-key 等） */
export const QUARTZTASK_SELF_I18N_KEY = buildEntitySelfI18nKey(QUARTZTASK_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const QUARTZTASK_LIST_FIELDS = [
  'taskCode',
  'taskName',
  'jobName',
  'jobGroup',
  'taskType',
  'assemblyName',
  'className',
  'apiUrl',
  'requestMethod',
  'sqlScript',
  'triggerType',
  'cronExpression',
  'intervalSeconds',
  'executeParams',
  'concurrent',
  'misfirePolicy',
  'firstRunAt',
  'executeCount',
  'lastRunAt',
  'nextRunAt',
  'taskDescription',
  'taskStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const QUARTZTASK_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'select',
  taskCode: 'required',
  taskName: 'required',
  jobName: 'required',
  jobGroup: 'select',
  taskType: 'select',
  assemblyName: 'required',
  className: 'required',
  apiUrl: 'optional',
  requestMethod: 'optional',
  sqlScript: 'optional',
  triggerType: 'select',
  cronExpression: 'required',
  intervalSeconds: 'select',
  executeParams: 'optional',
  concurrent: 'select',
  misfirePolicy: 'select',
  firstRunAt: 'optional',
  executeCount: 'select',
  lastRunAt: 'optional',
  nextRunAt: 'optional',
  taskDescription: 'optional',
  taskStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type QuartzTaskField = keyof typeof QUARTZTASK_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const QUARTZTASK_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'taskCode',
  'taskName',
  'jobName',
  'jobGroup',
  'taskType',
  'assemblyName',
  'className',
  'apiUrl',
  'requestMethod',
  'sqlScript',
  'cronExpression',
  'executeParams',
  'firstRunAtStart',
  'firstRunAtEnd',
  'lastRunAtStart',
  'lastRunAtEnd',
  'nextRunAtStart',
  'nextRunAtEnd',
  'taskDescription',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof QuartzTaskQuery)[]

export type QuartzTaskQueryField =
  | (typeof QUARTZTASK_QUERY_STRING_FIELDS)[number]
  | 'triggerType' | 'intervalSeconds' | 'concurrent' | 'misfirePolicy' | 'executeCount' | 'taskStatus'

/** 高级查询抽屉全部字段（含数值） */
export const QUARTZTASK_QUERY_FIELDS: readonly QuartzTaskQueryField[] = [
  ...QUARTZTASK_QUERY_STRING_FIELDS,
  'triggerType',
  'intervalSeconds',
  'concurrent',
  'misfirePolicy',
  'executeCount',
  'taskStatus',
]

/**
 * Quartz 定时任务实体字段 i18n：index / quartz-task-form 统一入口
 */
export function useQuartzTaskI18n() {
  const ef = useEntityFieldI18n(QUARTZTASK_ENTITY_SLUG)

  function ph(field: QuartzTaskField): string {
    return ef.placeholder(field, QUARTZTASK_PLACEHOLDER[field])
  }

  function queryPh(field: QuartzTaskQueryField, kind: EntityFieldPlaceholderKind): string {
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

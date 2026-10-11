// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/workflow/flow-instance/composables
// 文件名称：useFlowTaskI18n.ts
// 功能描述：FlowTask字段清单 + useFlowTaskI18n（字段名映射一次，文案由 entity.flowtask.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { FlowTaskQuery } from '@/types/workflow/flowTask.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktFlowTaskI18nSeedData 一致的实体 slug */
export const FLOWTASK_ENTITY_SLUG = 'flowtask'

/** entity.flowtask._self 静态属性（导入组件 entity-i18n-key 等） */
export const FLOWTASK_SELF_I18N_KEY = buildEntitySelfI18nKey(FLOWTASK_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const FLOWTASK_LIST_FIELDS = [
  'taskDefinitionKey',
  'taskName',
  'assigneeUserId',
  'assigneeUserName',
  'ownerUserId',
  'taskStatus',
  'signType',
  'priority',
  'isAddSign',
  'addSignId',
  'ExtField',
  'remark',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const FLOWTASK_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'taskDefinitionKey',
  'taskName',
  'assigneeUserId',
  'assigneeUserName',
  'ownerUserId',
  'taskStatus',
  'signType',
  'priority',
  'isAddSign',
  'addSignId',
  'ExtField',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const FLOWTASK_SUMMARY_SUM_FIELDS = [
  'taskStatus',
  'signType',
  'priority',
  'isAddSign',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const FLOWTASK_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type FlowTaskField = keyof typeof FLOWTASK_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const FLOWTASK_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof FlowTaskQuery)[]

export type FlowTaskQueryField = (typeof FLOWTASK_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const FLOWTASK_QUERY_FIELDS: readonly FlowTaskQueryField[] = [...FLOWTASK_QUERY_STRING_FIELDS]

/**
 * FlowTask字段 i18n：index / flow-task-form 统一入口
 */
export function useFlowTaskI18n() {
  const ef = useEntityFieldI18n(FLOWTASK_ENTITY_SLUG)

  function ph(field: FlowTaskField): string {
    return ef.placeholder(field, FLOWTASK_PLACEHOLDER[field])
  }

  function queryPh(field: FlowTaskQueryField, kind: EntityFieldPlaceholderKind): string {
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

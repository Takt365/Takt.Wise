// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/sop/exec/composables
// 文件名称：useExecStepI18n.ts
// 功能描述：SopExecStep字段清单 + useSopExecStepI18n（字段名映射一次，文案由 entity.sopexecstep.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SopExecStepQuery } from '@/types/logistics/manufacturing/sop/execStep.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSopExecStepI18nSeedData 一致的实体 slug */
export const SOPEXECSTEP_ENTITY_SLUG = 'sopexecstep'

/** entity.sopexecstep._self 静态属性（导入组件 entity-i18n-key 等） */
export const SOPEXECSTEP_SELF_I18N_KEY = buildEntitySelfI18nKey(SOPEXECSTEP_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SOPEXECSTEP_LIST_FIELDS = [
  'execId',
  'stepId',
  'stepNo',
  'startedAt',
  'endedAt',
  'stepResult',
  'confirmedBy',
  'confirmedAt',
  'blockNextStep',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const SOPEXECSTEP_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'execId',
  'stepId',
  'stepNo',
  'startedAt',
  'endedAt',
  'stepResult',
  'confirmedBy',
  'confirmedAt',
  'blockNextStep',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const SOPEXECSTEP_SUMMARY_SUM_FIELDS = [
  'stepNo',
  'stepResult',
  'blockNextStep',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SOPEXECSTEP_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  stepId: 'select',
  stepNo: 'select',
  startedAt: 'select',
  endedAt: 'optional',
  stepResult: 'optional',
  confirmedBy: 'optional',
  confirmedAt: 'optional',
  blockNextStep: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SopExecStepField = keyof typeof SOPEXECSTEP_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SOPEXECSTEP_QUERY_STRING_FIELDS = [
  'cultureCode',
  'stepId',
  'startedAtStart',
  'startedAtEnd',
  'endedAtStart',
  'endedAtEnd',
  'confirmedBy',
  'confirmedAtStart',
  'confirmedAtEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SopExecStepQuery)[]

export type SopExecStepQueryField =
  | (typeof SOPEXECSTEP_QUERY_STRING_FIELDS)[number]
  | 'stepNo' | 'stepResult' | 'blockNextStep'

/** 高级查询抽屉全部字段（含数值） */
export const SOPEXECSTEP_QUERY_FIELDS: readonly SopExecStepQueryField[] = [
  ...SOPEXECSTEP_QUERY_STRING_FIELDS,
  'stepNo',
  'stepResult',
  'blockNextStep',
]

/**
 * SopExecStep字段 i18n：index / exec-step-form 统一入口
 */
export function useSopExecStepI18n() {
  const ef = useEntityFieldI18n(SOPEXECSTEP_ENTITY_SLUG)

  function ph(field: SopExecStepField): string {
    return ef.placeholder(field, SOPEXECSTEP_PLACEHOLDER[field])
  }

  function queryPh(field: SopExecStepQueryField, kind: EntityFieldPlaceholderKind): string {
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

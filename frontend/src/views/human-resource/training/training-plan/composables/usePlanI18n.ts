// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/human-resource/training/training-plan/composables
// 文件名称：usePlanI18n.ts
// 功能描述：培训计划字段清单 + useTrainingPlanI18n（字段名映射一次，文案由 entity.trainingplan.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TrainingPlanQuery } from '@/types/human-resource/training/plan.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktTrainingPlanI18nSeedData 一致的实体 slug */
export const TRAININGPLAN_ENTITY_SLUG = 'trainingplan'

/** entity.trainingplan._self 静态属性（导入组件 entity-i18n-key 等） */
export const TRAININGPLAN_SELF_I18N_KEY = buildEntitySelfI18nKey(TRAININGPLAN_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const TRAININGPLAN_LIST_FIELDS = [
  'planCode',
  'planName',
  'planYear',
  'planType',
  'applicableDepartment',
  'startDate',
  'endDate',
  'trainingObjectives',
  'plannedHeadcount',
  'trainingBudget',
  'trainingPlanDescription',
  'trainingPlanStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const TRAININGPLAN_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  planCode: 'required',
  planName: 'required',
  planYear: 'select',
  planType: 'select',
  applicableDepartment: 'required',
  startDate: 'select',
  endDate: 'select',
  trainingObjectives: 'required',
  plannedHeadcount: 'select',
  trainingBudget: 'select',
  trainingPlanDescription: 'optional',
  trainingPlanStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type TrainingPlanField = keyof typeof TRAININGPLAN_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const TRAININGPLAN_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'planCode',
  'planName',
  'planType',
  'applicableDepartment',
  'startDateStart',
  'startDateEnd',
  'endDateStart',
  'endDateEnd',
  'trainingObjectives',
  'trainingPlanDescription',
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
] as const satisfies readonly (keyof TrainingPlanQuery)[]

export type TrainingPlanQueryField =
  | (typeof TRAININGPLAN_QUERY_STRING_FIELDS)[number]
  | 'planYear' | 'plannedHeadcount' | 'trainingBudget' | 'trainingPlanStatus' | 'approvalStatus'

/** 高级查询抽屉全部字段（含数值） */
export const TRAININGPLAN_QUERY_FIELDS: readonly TrainingPlanQueryField[] = [
  ...TRAININGPLAN_QUERY_STRING_FIELDS,
  'planYear',
  'plannedHeadcount',
  'trainingBudget',
  'trainingPlanStatus',
  'approvalStatus',
]

/**
 * 培训计划字段 i18n：index / plan-form 统一入口
 */
export function useTrainingPlanI18n() {
  const ef = useEntityFieldI18n(TRAININGPLAN_ENTITY_SLUG)

  function ph(field: TrainingPlanField): string {
    return ef.placeholder(field, TRAININGPLAN_PLACEHOLDER[field])
  }

  function queryPh(field: TrainingPlanQueryField, kind: EntityFieldPlaceholderKind): string {
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

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/aps/aps-schedule/composables
// 文件名称：useScheduleI18n.ts
// 功能描述：APS排程主表字段清单 + useApsScheduleI18n（字段名映射一次，文案由 entity.apsschedule.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { ApsScheduleQuery } from '@/types/logistics/manufacturing/aps/schedule.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktApsScheduleI18nSeedData 一致的实体 slug */
export const APSSCHEDULE_ENTITY_SLUG = 'apsschedule'

/** entity.apsschedule._self 静态属性（导入组件 entity-i18n-key 等） */
export const APSSCHEDULE_SELF_I18N_KEY = buildEntitySelfI18nKey(APSSCHEDULE_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const APSSCHEDULE_LIST_FIELDS = [
  'materialRequirementsPlanningCode',
  'plantCode',
  'scheduleCode',
  'scheduleName',
  'scheduleType',
  'planDate',
  'planStartTime',
  'planEndTime',
  'planCycle',
  'workshopCode',
  'workshopName',
  'productionLineCode',
  'productionLineName',
  'scheduleStrategy',
  'scheduleAlgorithm',
  'optimizationObjective',
  'scheduleStatus',
  'plannerGroupCode',
  'plannerGroupName',
  'publishTime',
  'publishUserId',
  'publishUserName',
  'scheduleDescription',
  'remark',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const APSSCHEDULE_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type ApsScheduleField = keyof typeof APSSCHEDULE_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const APSSCHEDULE_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof ApsScheduleQuery)[]

export type ApsScheduleQueryField = (typeof APSSCHEDULE_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const APSSCHEDULE_QUERY_FIELDS: readonly ApsScheduleQueryField[] = [...APSSCHEDULE_QUERY_STRING_FIELDS]

/**
 * APS排程主表字段 i18n：index / schedule-form 统一入口
 */
export function useApsScheduleI18n() {
  const ef = useEntityFieldI18n(APSSCHEDULE_ENTITY_SLUG)

  function ph(field: ApsScheduleField): string {
    return ef.placeholder(field, APSSCHEDULE_PLACEHOLDER[field])
  }

  function queryPh(field: ApsScheduleQueryField, kind: EntityFieldPlaceholderKind): string {
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

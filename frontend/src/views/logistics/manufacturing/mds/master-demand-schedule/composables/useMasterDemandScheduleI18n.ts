// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/mds/master-demand-schedule/composables
// 文件名称：useMasterDemandScheduleI18n.ts
// 功能描述：主需求计划 MDS 头表字段清单 + useMasterDemandScheduleI18n（字段名映射一次，文案由 entity.masterdemandschedule.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { MasterDemandScheduleQuery } from '@/types/logistics/manufacturing/mds/masterDemandSchedule.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktMasterDemandScheduleI18nSeedData 一致的实体 slug */
export const MASTERDEMANDSCHEDULE_ENTITY_SLUG = 'masterdemandschedule'

/** entity.masterdemandschedule._self 静态属性（导入组件 entity-i18n-key 等） */
export const MASTERDEMANDSCHEDULE_SELF_I18N_KEY = buildEntitySelfI18nKey(MASTERDEMANDSCHEDULE_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const MASTERDEMANDSCHEDULE_LIST_FIELDS = [
  'mdsCode',
  'planPeriodStart',
  'planPeriodEnd',
  'bucketType',
  'scheduleStatus',
  'remark',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const MASTERDEMANDSCHEDULE_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type MasterDemandScheduleField = keyof typeof MASTERDEMANDSCHEDULE_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const MASTERDEMANDSCHEDULE_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof MasterDemandScheduleQuery)[]

export type MasterDemandScheduleQueryField = (typeof MASTERDEMANDSCHEDULE_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const MASTERDEMANDSCHEDULE_QUERY_FIELDS: readonly MasterDemandScheduleQueryField[] = [...MASTERDEMANDSCHEDULE_QUERY_STRING_FIELDS]

/**
 * 主需求计划 MDS 头表字段 i18n：index / master-demand-schedule-form 统一入口
 */
export function useMasterDemandScheduleI18n() {
  const ef = useEntityFieldI18n(MASTERDEMANDSCHEDULE_ENTITY_SLUG)

  function ph(field: MasterDemandScheduleField): string {
    return ef.placeholder(field, MASTERDEMANDSCHEDULE_PLACEHOLDER[field])
  }

  function queryPh(field: MasterDemandScheduleQueryField, kind: EntityFieldPlaceholderKind): string {
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

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/mds/master-demand-schedule/composables
// 文件名称：useMasterDemandScheduleLineI18n.ts
// 功能描述：MasterDemandScheduleLine字段清单 + useMasterDemandScheduleLineI18n（字段名映射一次，文案由 entity.masterdemandscheduleline.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { MasterDemandScheduleLineQuery } from '@/types/logistics/manufacturing/mds/masterDemandScheduleLine.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktMasterDemandScheduleLineI18nSeedData 一致的实体 slug */
export const MASTERDEMANDSCHEDULELINE_ENTITY_SLUG = 'masterdemandscheduleline'

/** entity.masterdemandscheduleline._self 静态属性（导入组件 entity-i18n-key 等） */
export const MASTERDEMANDSCHEDULELINE_SELF_I18N_KEY = buildEntitySelfI18nKey(MASTERDEMANDSCHEDULELINE_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const MASTERDEMANDSCHEDULELINE_LIST_FIELDS = [
  'mdsCode',
  'demandSourceType',
  'salesOrderId',
  'salesOrderLineNumber',
  'salesForecastId',
  'salesForecastLineNumber',
  'materialCode',
  'bucketStart',
  'bucketEnd',
  'demandQuantity',
  'unitOfMeasure',
  'remark',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const MASTERDEMANDSCHEDULELINE_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'mdsCode',
  'demandSourceType',
  'salesOrderId',
  'salesOrderLineNumber',
  'salesForecastId',
  'salesForecastLineNumber',
  'materialCode',
  'bucketStart',
  'bucketEnd',
  'demandQuantity',
  'unitOfMeasure',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const MASTERDEMANDSCHEDULELINE_SUMMARY_SUM_FIELDS = [
  'demandSourceType',
  'salesOrderLineNumber',
  'salesForecastLineNumber',
  'demandQuantity',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const MASTERDEMANDSCHEDULELINE_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type MasterDemandScheduleLineField = keyof typeof MASTERDEMANDSCHEDULELINE_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const MASTERDEMANDSCHEDULELINE_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof MasterDemandScheduleLineQuery)[]

export type MasterDemandScheduleLineQueryField = (typeof MASTERDEMANDSCHEDULELINE_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const MASTERDEMANDSCHEDULELINE_QUERY_FIELDS: readonly MasterDemandScheduleLineQueryField[] = [...MASTERDEMANDSCHEDULELINE_QUERY_STRING_FIELDS]

/**
 * MasterDemandScheduleLine字段 i18n：index / master-demand-schedule-line-form 统一入口
 */
export function useMasterDemandScheduleLineI18n() {
  const ef = useEntityFieldI18n(MASTERDEMANDSCHEDULELINE_ENTITY_SLUG)

  function ph(field: MasterDemandScheduleLineField): string {
    return ef.placeholder(field, MASTERDEMANDSCHEDULELINE_PLACEHOLDER[field])
  }

  function queryPh(field: MasterDemandScheduleLineQueryField, kind: EntityFieldPlaceholderKind): string {
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

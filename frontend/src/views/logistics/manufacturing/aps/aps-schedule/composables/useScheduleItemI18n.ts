// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/aps/aps-schedule/composables
// 文件名称：useScheduleItemI18n.ts
// 功能描述：ApsScheduleItem字段清单 + useApsScheduleItemI18n（字段名映射一次，文案由 entity.apsscheduleitem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { ApsScheduleItemQuery } from '@/types/logistics/manufacturing/aps/scheduleItem.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktApsScheduleItemI18nSeedData 一致的实体 slug */
export const APSSCHEDULEITEM_ENTITY_SLUG = 'apsscheduleitem'

/** entity.apsscheduleitem._self 静态属性（导入组件 entity-i18n-key 等） */
export const APSSCHEDULEITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(APSSCHEDULEITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const APSSCHEDULEITEM_LIST_FIELDS = [
  'apsScheduleCode',
  'apsOrderId',
  'apsOperationId',
  'routingItemId',
  'lineNumber',
  'workOrderCode',
  'productCode',
  'productName',
  'workCenterCode',
  'workCenterDescription',
  'processCode',
  'processName',
  'processSequence',
  'processStandardST',
  'processStandardSTUnit',
  'extraMinutes',
  'planQuantity',
  'planStartTime',
  'planEndTime',
  'actualStartTime',
  'actualEndTime',
  'processStatus',
  'priority',
  'isObsolete',
  'remark',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const APSSCHEDULEITEM_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'apsScheduleCode',
  'apsOrderId',
  'apsOperationId',
  'routingItemId',
  'lineNumber',
  'workOrderCode',
  'productCode',
  'productName',
  'workCenterCode',
  'workCenterDescription',
  'processCode',
  'processName',
  'processSequence',
  'processStandardST',
  'processStandardSTUnit',
  'extraMinutes',
  'planQuantity',
  'planStartTime',
  'planEndTime',
  'actualStartTime',
  'actualEndTime',
  'processStatus',
  'priority',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const APSSCHEDULEITEM_SUMMARY_SUM_FIELDS = [
  'processSequence',
  'processStandardST',
  'processStandardSTUnit',
  'extraMinutes',
  'planQuantity',
  'processStatus',
  'priority',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const APSSCHEDULEITEM_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type ApsScheduleItemField = keyof typeof APSSCHEDULEITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const APSSCHEDULEITEM_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof ApsScheduleItemQuery)[]

export type ApsScheduleItemQueryField = (typeof APSSCHEDULEITEM_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const APSSCHEDULEITEM_QUERY_FIELDS: readonly ApsScheduleItemQueryField[] = [...APSSCHEDULEITEM_QUERY_STRING_FIELDS]

/**
 * ApsScheduleItem字段 i18n：index / schedule-item-form 统一入口
 */
export function useApsScheduleItemI18n() {
  const ef = useEntityFieldI18n(APSSCHEDULEITEM_ENTITY_SLUG)

  function ph(field: ApsScheduleItemField): string {
    return ef.placeholder(field, APSSCHEDULEITEM_PLACEHOLDER[field])
  }

  function queryPh(field: ApsScheduleItemQueryField, kind: EntityFieldPlaceholderKind): string {
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

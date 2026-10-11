// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/customer-service/order/composables
// 文件名称：useOperationI18n.ts
// 功能描述：ApsOperation字段清单 + useApsOperationI18n（字段名映射一次，文案由 entity.apsoperation.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { ApsOperationQuery } from '@/types/logistics/manufacturing/aps/operation.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktApsOperationI18nSeedData 一致的实体 slug */
export const APSOPERATION_ENTITY_SLUG = 'apsoperation'

/** entity.apsoperation._self 静态属性（导入组件 entity-i18n-key 等） */
export const APSOPERATION_SELF_I18N_KEY = buildEntitySelfI18nKey(APSOPERATION_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const APSOPERATION_LIST_FIELDS = [
  'apsOrderCode',
  'lineNumber',
  'routingItemId',
  'processCode',
  'processName',
  'workCenterCode',
  'workCenterResourceId',
  'plannedStartTime',
  'plannedEndTime',
  'plannedDurationMinutes',
  'changeoverMinutes',
  'operationStatus',
  'isObsolete',
  'remark',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const APSOPERATION_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'apsOrderCode',
  'lineNumber',
  'routingItemId',
  'processCode',
  'processName',
  'workCenterCode',
  'workCenterResourceId',
  'plannedStartTime',
  'plannedEndTime',
  'plannedDurationMinutes',
  'changeoverMinutes',
  'operationStatus',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const APSOPERATION_SUMMARY_SUM_FIELDS = [
  'plannedDurationMinutes',
  'changeoverMinutes',
  'operationStatus',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const APSOPERATION_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type ApsOperationField = keyof typeof APSOPERATION_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const APSOPERATION_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof ApsOperationQuery)[]

export type ApsOperationQueryField = (typeof APSOPERATION_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const APSOPERATION_QUERY_FIELDS: readonly ApsOperationQueryField[] = [...APSOPERATION_QUERY_STRING_FIELDS]

/**
 * ApsOperation字段 i18n：index / operation-form 统一入口
 */
export function useApsOperationI18n() {
  const ef = useEntityFieldI18n(APSOPERATION_ENTITY_SLUG)

  function ph(field: ApsOperationField): string {
    return ef.placeholder(field, APSOPERATION_PLACEHOLDER[field])
  }

  function queryPh(field: ApsOperationQueryField, kind: EntityFieldPlaceholderKind): string {
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

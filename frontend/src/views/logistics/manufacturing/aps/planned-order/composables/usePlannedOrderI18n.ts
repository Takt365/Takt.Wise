// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/aps/planned-order/composables
// 文件名称：usePlannedOrderI18n.ts
// 功能描述：计划订单字段清单 + usePlannedOrderI18n（字段名映射一次，文案由 entity.plannedorder.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PlannedOrderQuery } from '@/types/logistics/manufacturing/aps/plannedOrder.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktPlannedOrderI18nSeedData 一致的实体 slug */
export const PLANNEDORDER_ENTITY_SLUG = 'plannedorder'

/** entity.plannedorder._self 静态属性（导入组件 entity-i18n-key 等） */
export const PLANNEDORDER_SELF_I18N_KEY = buildEntitySelfI18nKey(PLANNEDORDER_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PLANNEDORDER_LIST_FIELDS = [
  'plantCode',
  'plannedOrderCode',
  'materialRequirementsPlanningId',
  'materialRequirementsPlanningCode',
  'materialRequirementsPlanningItemId',
  'materialCode',
  'plannedQuantity',
  'unitOfMeasure',
  'plannedStartTime',
  'plannedEndTime',
  'routingCode',
  'orderStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PLANNEDORDER_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  plantCode: 'select',
  plannedOrderCode: 'required',
  materialRequirementsPlanningId: 'optional',
  materialRequirementsPlanningCode: 'optional',
  materialRequirementsPlanningItemId: 'optional',
  materialCode: 'select',
  plannedQuantity: 'select',
  unitOfMeasure: 'select',
  plannedStartTime: 'optional',
  plannedEndTime: 'optional',
  routingCode: 'optional',
  orderStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type PlannedOrderField = keyof typeof PLANNEDORDER_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PLANNEDORDER_QUERY_STRING_FIELDS = [
  'plannedOrderCode',
  'materialRequirementsPlanningId',
  'materialRequirementsPlanningCode',
  'materialRequirementsPlanningItemId',
  'materialCode',
  'unitOfMeasure',
  'plannedStartTimeStart',
  'plannedStartTimeEnd',
  'plannedEndTimeStart',
  'plannedEndTimeEnd',
  'routingCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof PlannedOrderQuery)[]

export type PlannedOrderQueryField =
  | (typeof PLANNEDORDER_QUERY_STRING_FIELDS)[number]
  | 'plannedQuantity' | 'orderStatus'

/** 高级查询抽屉全部字段（含数值） */
export const PLANNEDORDER_QUERY_FIELDS: readonly PlannedOrderQueryField[] = [
  ...PLANNEDORDER_QUERY_STRING_FIELDS,
  'plannedQuantity',
  'orderStatus',
]

/**
 * 计划订单字段 i18n：index / planned-order-form 统一入口
 */
export function usePlannedOrderI18n() {
  const ef = useEntityFieldI18n(PLANNEDORDER_ENTITY_SLUG)

  function ph(field: PlannedOrderField): string {
    return ef.placeholder(field, PLANNEDORDER_PLACEHOLDER[field])
  }

  function queryPh(field: PlannedOrderQueryField, kind: EntityFieldPlaceholderKind): string {
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

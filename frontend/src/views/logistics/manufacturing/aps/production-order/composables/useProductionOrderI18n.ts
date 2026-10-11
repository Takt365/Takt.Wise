// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/aps/production-order/composables
// 文件名称：useProductionOrderI18n.ts
// 功能描述：生产工单实体字段清单 + useProductionOrderI18n（字段名映射一次，文案由 entity.productionorder.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { ProductionOrderQuery } from '@/types/logistics/manufacturing/aps/productionOrder.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktProductionOrderI18nSeedData 一致的实体 slug */
export const PRODUCTIONORDER_ENTITY_SLUG = 'productionorder'

/** entity.productionorder._self 静态属性（导入组件 entity-i18n-key 等） */
export const PRODUCTIONORDER_SELF_I18N_KEY = buildEntitySelfI18nKey(PRODUCTIONORDER_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PRODUCTIONORDER_LIST_FIELDS = [
  'plantCode',
  'prodOrderType',
  'prodOrderCode',
  'materialCode',
  'prodOrderQty',
  'producedQty',
  'unitOfMeasure',
  'actualStartDate',
  'actualEndDate',
  'priority',
  'workCenter',
  'prodBatch',
  'serialCode',
  'routingCode',
  'plannedOrderId',
  'apsOrderId',
  'plannedStartTime',
  'plannedEndTime',
  'orderStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PRODUCTIONORDER_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  plantCode: 'select',
  prodOrderType: 'select',
  prodOrderCode: 'required',
  materialCode: 'select',
  prodOrderQty: 'select',
  producedQty: 'select',
  unitOfMeasure: 'select',
  actualStartDate: 'optional',
  actualEndDate: 'optional',
  priority: 'select',
  workCenter: 'optional',
  prodBatch: 'optional',
  serialCode: 'optional',
  routingCode: 'optional',
  plannedOrderId: 'optional',
  apsOrderId: 'optional',
  plannedStartTime: 'optional',
  plannedEndTime: 'optional',
  orderStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type ProductionOrderField = keyof typeof PRODUCTIONORDER_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PRODUCTIONORDER_QUERY_STRING_FIELDS = [
  'plantCode',
  'prodOrderType',
  'prodOrderCode',
  'materialCode',
  'unitOfMeasure',
  'actualStartDateStart',
  'actualStartDateEnd',
  'actualEndDateStart',
  'actualEndDateEnd',
  'workCenter',
  'prodBatch',
  'serialCode',
  'routingCode',
  'plannedOrderId',
  'apsOrderId',
  'plannedStartTimeStart',
  'plannedStartTimeEnd',
  'plannedEndTimeStart',
  'plannedEndTimeEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof ProductionOrderQuery)[]

export type ProductionOrderQueryField =
  | (typeof PRODUCTIONORDER_QUERY_STRING_FIELDS)[number]
  | 'prodOrderQty' | 'producedQty' | 'priority' | 'orderStatus'

/** 高级查询抽屉全部字段（含数值） */
export const PRODUCTIONORDER_QUERY_FIELDS: readonly ProductionOrderQueryField[] = [
  ...PRODUCTIONORDER_QUERY_STRING_FIELDS,
  'prodOrderQty',
  'producedQty',
  'priority',
  'orderStatus',
]

/**
 * 生产工单实体字段 i18n：index / production-order-form 统一入口
 */
export function useProductionOrderI18n() {
  const ef = useEntityFieldI18n(PRODUCTIONORDER_ENTITY_SLUG)

  function ph(field: ProductionOrderField): string {
    return ef.placeholder(field, PRODUCTIONORDER_PLACEHOLDER[field])
  }

  function queryPh(field: ProductionOrderQueryField, kind: EntityFieldPlaceholderKind): string {
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

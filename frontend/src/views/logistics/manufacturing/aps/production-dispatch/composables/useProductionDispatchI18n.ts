// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/aps/production-dispatch/composables
// 文件名称：useProductionDispatchI18n.ts
// 功能描述：生产派工单字段清单 + useProductionDispatchI18n（字段名映射一次，文案由 entity.productiondispatch.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { ProductionDispatchQuery } from '@/types/logistics/manufacturing/aps/productionDispatch.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktProductionDispatchI18nSeedData 一致的实体 slug */
export const PRODUCTIONDISPATCH_ENTITY_SLUG = 'productiondispatch'

/** entity.productiondispatch._self 静态属性（导入组件 entity-i18n-key 等） */
export const PRODUCTIONDISPATCH_SELF_I18N_KEY = buildEntitySelfI18nKey(PRODUCTIONDISPATCH_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PRODUCTIONDISPATCH_LIST_FIELDS = [
  'plantCode',
  'dispatchCode',
  'productionOrderId',
  'prodOrderCode',
  'apsOperationId',
  'workCenterCode',
  'processCode',
  'dispatchQuantity',
  'plannedStartTime',
  'plannedEndTime',
  'dispatchStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PRODUCTIONDISPATCH_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  plantCode: 'select',
  dispatchCode: 'required',
  productionOrderId: 'select',
  prodOrderCode: 'required',
  apsOperationId: 'optional',
  workCenterCode: 'optional',
  processCode: 'optional',
  dispatchQuantity: 'select',
  plannedStartTime: 'optional',
  plannedEndTime: 'optional',
  dispatchStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type ProductionDispatchField = keyof typeof PRODUCTIONDISPATCH_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PRODUCTIONDISPATCH_QUERY_STRING_FIELDS = [
  'plantCode',
  'dispatchCode',
  'productionOrderId',
  'prodOrderCode',
  'apsOperationId',
  'workCenterCode',
  'processCode',
  'plannedStartTimeStart',
  'plannedStartTimeEnd',
  'plannedEndTimeStart',
  'plannedEndTimeEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof ProductionDispatchQuery)[]

export type ProductionDispatchQueryField =
  | (typeof PRODUCTIONDISPATCH_QUERY_STRING_FIELDS)[number]
  | 'dispatchQuantity' | 'dispatchStatus'

/** 高级查询抽屉全部字段（含数值） */
export const PRODUCTIONDISPATCH_QUERY_FIELDS: readonly ProductionDispatchQueryField[] = [
  ...PRODUCTIONDISPATCH_QUERY_STRING_FIELDS,
  'dispatchQuantity',
  'dispatchStatus',
]

/**
 * 生产派工单字段 i18n：index / production-dispatch-form 统一入口
 */
export function useProductionDispatchI18n() {
  const ef = useEntityFieldI18n(PRODUCTIONDISPATCH_ENTITY_SLUG)

  function ph(field: ProductionDispatchField): string {
    return ef.placeholder(field, PRODUCTIONDISPATCH_PLACEHOLDER[field])
  }

  function queryPh(field: ProductionDispatchQueryField, kind: EntityFieldPlaceholderKind): string {
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

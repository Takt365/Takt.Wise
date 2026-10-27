// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/mrp/purchase-plan/composables
// 文件名称：usePurchasePlanI18n.ts
// 功能描述：Takt采购计划实体字段清单 + usePurchasePlanI18n（字段名映射一次，文案由 entity.purchaseplan.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PurchasePlanQuery } from '@/types/logistics/manufacturing/mrp/purchasePlan.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktPurchasePlanI18nSeedData 一致的实体 slug */
export const PURCHASEPLAN_ENTITY_SLUG = 'purchaseplan'

/** entity.purchaseplan._self 静态属性（导入组件 entity-i18n-key 等） */
export const PURCHASEPLAN_SELF_I18N_KEY = buildEntitySelfI18nKey(PURCHASEPLAN_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PURCHASEPLAN_LIST_FIELDS = [
  'purchasePlanCode',
  'materialRequirementsPlanningId',
  'materialRequirementsPlanningCode',
  'productionPlanId',
  'productionPlanCode',
  'planDate',
  'planPeriodStart',
  'planPeriodEnd',
  'purchaseGroupCode',
  'plannerGroupCode',
  'plannerGroupName',
  'totalQuantity',
  'totalAmount',
  'convertedQuantity',
  'convertedAmount',
  'planStatus',
  'convertedStatus',
  'planDescription',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PURCHASEPLAN_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  purchasePlanCode: 'required',
  materialRequirementsPlanningId: 'optional',
  materialRequirementsPlanningCode: 'optional',
  productionPlanId: 'optional',
  productionPlanCode: 'optional',
  planDate: 'select',
  planPeriodStart: 'select',
  planPeriodEnd: 'select',
  purchaseGroupCode: 'optional',
  plannerGroupCode: 'optional',
  plannerGroupName: 'optional',
  totalQuantity: 'select',
  totalAmount: 'select',
  convertedQuantity: 'select',
  convertedAmount: 'select',
  planStatus: 'select',
  convertedStatus: 'select',
  planDescription: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type PurchasePlanField = keyof typeof PURCHASEPLAN_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PURCHASEPLAN_QUERY_STRING_FIELDS = [
  'cultureCode',
  'purchasePlanCode',
  'materialRequirementsPlanningId',
  'materialRequirementsPlanningCode',
  'productionPlanId',
  'productionPlanCode',
  'planDateStart',
  'planDateEnd',
  'planPeriodStartStart',
  'planPeriodStartEnd',
  'planPeriodEndStart',
  'planPeriodEndEnd',
  'purchaseGroupCode',
  'plannerGroupCode',
  'plannerGroupName',
  'planDescription',
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
] as const satisfies readonly (keyof PurchasePlanQuery)[]

export type PurchasePlanQueryField =
  | (typeof PURCHASEPLAN_QUERY_STRING_FIELDS)[number]
  | 'totalQuantity' | 'totalAmount' | 'convertedQuantity' | 'convertedAmount' | 'planStatus' | 'convertedStatus' | 'approvalStatus'

/** 高级查询抽屉全部字段（含数值） */
export const PURCHASEPLAN_QUERY_FIELDS: readonly PurchasePlanQueryField[] = [
  ...PURCHASEPLAN_QUERY_STRING_FIELDS,
  'totalQuantity',
  'totalAmount',
  'convertedQuantity',
  'convertedAmount',
  'planStatus',
  'convertedStatus',
  'approvalStatus',
]

/**
 * Takt采购计划实体字段 i18n：index / purchase-plan-form 统一入口
 */
export function usePurchasePlanI18n() {
  const ef = useEntityFieldI18n(PURCHASEPLAN_ENTITY_SLUG)

  function ph(field: PurchasePlanField): string {
    return ef.placeholder(field, PURCHASEPLAN_PLACEHOLDER[field])
  }

  function queryPh(field: PurchasePlanQueryField, kind: EntityFieldPlaceholderKind): string {
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

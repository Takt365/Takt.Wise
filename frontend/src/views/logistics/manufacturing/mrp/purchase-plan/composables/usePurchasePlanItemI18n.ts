// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/mrp/purchase-plan/composables
// 文件名称：usePurchasePlanItemI18n.ts
// 功能描述：PurchasePlanItem字段清单 + usePurchasePlanItemI18n（字段名映射一次，文案由 entity.purchaseplanitem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PurchasePlanItemQuery } from '@/types/logistics/manufacturing/mrp/purchasePlanItem.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktPurchasePlanItemI18nSeedData 一致的实体 slug */
export const PURCHASEPLANITEM_ENTITY_SLUG = 'purchaseplanitem'

/** entity.purchaseplanitem._self 静态属性（导入组件 entity-i18n-key 等） */
export const PURCHASEPLANITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(PURCHASEPLANITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PURCHASEPLANITEM_LIST_FIELDS = [
  'purchasePlanId',
  'purchasePlanCode',
  'lineNumber',
  'productionPlanId',
  'productionPlanCode',
  'productionPlanLineNumber',
  'materialRequirementsPlanningItemId',
  'materialCode',
  'materialDescription',
  'materialSpecification',
  'planUnit',
  'planQuantity',
  'plannedArrivalDate',
  'convertedQuantity',
  'estimatedUnitPrice',
  'estimatedAmount',
  'taxIncludedPrice',
  'untaxedPrice',
  'taxAmount',
  'referenceSupplierCode',
  'referenceSupplierName1',
  'isObsolete',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const PURCHASEPLANITEM_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'purchasePlanId',
  'purchasePlanCode',
  'lineNumber',
  'productionPlanId',
  'productionPlanCode',
  'productionPlanLineNumber',
  'materialRequirementsPlanningItemId',
  'materialCode',
  'materialDescription',
  'materialSpecification',
  'planUnit',
  'planQuantity',
  'plannedArrivalDate',
  'convertedQuantity',
  'estimatedUnitPrice',
  'estimatedAmount',
  'taxIncludedPrice',
  'untaxedPrice',
  'taxAmount',
  'referenceSupplierCode',
  'referenceSupplierName1',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const PURCHASEPLANITEM_SUMMARY_SUM_FIELDS = [
  'productionPlanLineNumber',
  'planQuantity',
  'convertedQuantity',
  'estimatedUnitPrice',
  'estimatedAmount',
  'taxIncludedPrice',
  'untaxedPrice',
  'taxAmount',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PURCHASEPLANITEM_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  lineNumber: 'select',
  productionPlanId: 'optional',
  productionPlanCode: 'optional',
  productionPlanLineNumber: 'optional',
  materialRequirementsPlanningItemId: 'optional',
  materialCode: 'select',
  materialDescription: 'optional',
  materialSpecification: 'optional',
  planUnit: 'select',
  planQuantity: 'select',
  plannedArrivalDate: 'optional',
  convertedQuantity: 'select',
  estimatedUnitPrice: 'select',
  estimatedAmount: 'select',
  taxIncludedPrice: 'select',
  untaxedPrice: 'select',
  taxAmount: 'select',
  referenceSupplierCode: 'optional',
  referenceSupplierName1: 'optional',
  isObsolete: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type PurchasePlanItemField = keyof typeof PURCHASEPLANITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PURCHASEPLANITEM_QUERY_STRING_FIELDS = [
  'cultureCode',
  'purchasePlanCode',
  'productionPlanId',
  'productionPlanCode',
  'materialRequirementsPlanningItemId',
  'materialCode',
  'materialDescription',
  'materialSpecification',
  'planUnit',
  'plannedArrivalDateStart',
  'plannedArrivalDateEnd',
  'referenceSupplierCode',
  'referenceSupplierName1',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof PurchasePlanItemQuery)[]

export type PurchasePlanItemQueryField =
  | (typeof PURCHASEPLANITEM_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'productionPlanLineNumber' | 'planQuantity' | 'convertedQuantity' | 'estimatedUnitPrice' | 'estimatedAmount' | 'taxIncludedPrice' | 'untaxedPrice' | 'taxAmount' | 'isObsolete'

/** 高级查询抽屉全部字段（含数值） */
export const PURCHASEPLANITEM_QUERY_FIELDS: readonly PurchasePlanItemQueryField[] = [
  ...PURCHASEPLANITEM_QUERY_STRING_FIELDS,
  'lineNumber',
  'productionPlanLineNumber',
  'planQuantity',
  'convertedQuantity',
  'estimatedUnitPrice',
  'estimatedAmount',
  'taxIncludedPrice',
  'untaxedPrice',
  'taxAmount',
  'isObsolete',
]

/**
 * PurchasePlanItem字段 i18n：index / purchase-plan-item-form 统一入口
 */
export function usePurchasePlanItemI18n() {
  const ef = useEntityFieldI18n(PURCHASEPLANITEM_ENTITY_SLUG)

  function ph(field: PurchasePlanItemField): string {
    return ef.placeholder(field, PURCHASEPLANITEM_PLACEHOLDER[field])
  }

  function queryPh(field: PurchasePlanItemQueryField, kind: EntityFieldPlaceholderKind): string {
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

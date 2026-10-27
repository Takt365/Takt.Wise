// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/procurement/purchase-request/composables
// 文件名称：usePurchaseRequestItemI18n.ts
// 功能描述：PurchaseRequestItem字段清单 + usePurchaseRequestItemI18n（字段名映射一次，文案由 entity.purchaserequestitem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PurchaseRequestItemQuery } from '@/types/logistics/procurement/purchaseRequestItem.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktPurchaseRequestItemI18nSeedData 一致的实体 slug */
export const PURCHASEREQUESTITEM_ENTITY_SLUG = 'purchaserequestitem'

/** entity.purchaserequestitem._self 静态属性（导入组件 entity-i18n-key 等） */
export const PURCHASEREQUESTITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(PURCHASEREQUESTITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PURCHASEREQUESTITEM_LIST_FIELDS = [
  'purchaseRequestId',
  'purchaseRequestCode',
  'purchasePlanItemId',
  'lineNumber',
  'allocationCategory',
  'materialCode',
  'materialDescription',
  'materialSpecification',
  'requestUnit',
  'requestQuantity',
  'convertedQuantity',
  'purchasePerUnit',
  'purchaseRequestUnitPrice',
  'taxCode',
  'taxIncludedAmount',
  'untaxedAmount',
  'taxAmount',
  'requestAmount',
  'pricingDate',
  'grossWeight',
  'netWeight',
  'weightUnit',
  'volume',
  'volumeUnit',
  'profitCenterCode',
  'isObsolete',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const PURCHASEREQUESTITEM_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'purchaseRequestId',
  'purchaseRequestCode',
  'purchasePlanItemId',
  'lineNumber',
  'allocationCategory',
  'materialCode',
  'materialDescription',
  'materialSpecification',
  'requestUnit',
  'requestQuantity',
  'convertedQuantity',
  'purchasePerUnit',
  'purchaseRequestUnitPrice',
  'taxCode',
  'taxIncludedAmount',
  'untaxedAmount',
  'taxAmount',
  'requestAmount',
  'pricingDate',
  'grossWeight',
  'netWeight',
  'weightUnit',
  'volume',
  'volumeUnit',
  'profitCenterCode',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const PURCHASEREQUESTITEM_SUMMARY_SUM_FIELDS = [
  'requestQuantity',
  'convertedQuantity',
  'purchasePerUnit',
  'purchaseRequestUnitPrice',
  'taxIncludedAmount',
  'untaxedAmount',
  'taxAmount',
  'requestAmount',
  'grossWeight',
  'netWeight',
  'volume',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PURCHASEREQUESTITEM_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  purchasePlanItemId: 'optional',
  lineNumber: 'select',
  allocationCategory: 'select',
  materialCode: 'optional',
  materialDescription: 'optional',
  materialSpecification: 'optional',
  requestUnit: 'select',
  requestQuantity: 'select',
  convertedQuantity: 'select',
  purchasePerUnit: 'select',
  purchaseRequestUnitPrice: 'select',
  taxCode: 'optional',
  taxIncludedAmount: 'select',
  untaxedAmount: 'select',
  taxAmount: 'select',
  requestAmount: 'select',
  pricingDate: 'optional',
  grossWeight: 'optional',
  netWeight: 'optional',
  weightUnit: 'optional',
  volume: 'optional',
  volumeUnit: 'optional',
  profitCenterCode: 'optional',
  isObsolete: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type PurchaseRequestItemField = keyof typeof PURCHASEREQUESTITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PURCHASEREQUESTITEM_QUERY_STRING_FIELDS = [
  'cultureCode',
  'purchaseRequestCode',
  'purchasePlanItemId',
  'allocationCategory',
  'materialCode',
  'materialDescription',
  'materialSpecification',
  'requestUnit',
  'taxCode',
  'pricingDateStart',
  'pricingDateEnd',
  'weightUnit',
  'volumeUnit',
  'profitCenterCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof PurchaseRequestItemQuery)[]

export type PurchaseRequestItemQueryField =
  | (typeof PURCHASEREQUESTITEM_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'requestQuantity' | 'convertedQuantity' | 'purchasePerUnit' | 'purchaseRequestUnitPrice' | 'taxIncludedAmount' | 'untaxedAmount' | 'taxAmount' | 'requestAmount' | 'grossWeight' | 'netWeight' | 'volume' | 'isObsolete'

/** 高级查询抽屉全部字段（含数值） */
export const PURCHASEREQUESTITEM_QUERY_FIELDS: readonly PurchaseRequestItemQueryField[] = [
  ...PURCHASEREQUESTITEM_QUERY_STRING_FIELDS,
  'lineNumber',
  'requestQuantity',
  'convertedQuantity',
  'purchasePerUnit',
  'purchaseRequestUnitPrice',
  'taxIncludedAmount',
  'untaxedAmount',
  'taxAmount',
  'requestAmount',
  'grossWeight',
  'netWeight',
  'volume',
  'isObsolete',
]

/**
 * PurchaseRequestItem字段 i18n：index / purchase-request-item-form 统一入口
 */
export function usePurchaseRequestItemI18n() {
  const ef = useEntityFieldI18n(PURCHASEREQUESTITEM_ENTITY_SLUG)

  function ph(field: PurchaseRequestItemField): string {
    return ef.placeholder(field, PURCHASEREQUESTITEM_PLACEHOLDER[field])
  }

  function queryPh(field: PurchaseRequestItemQueryField, kind: EntityFieldPlaceholderKind): string {
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

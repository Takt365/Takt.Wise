// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/procurement/purchase-order/composables
// 文件名称：usePurchaseOrderI18n.ts
// 功能描述：Takt采购订单实体字段清单 + usePurchaseOrderI18n（字段名映射一次，文案由 entity.purchaseorder.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PurchaseOrderQuery } from '@/types/logistics/procurement/purchaseOrder.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktPurchaseOrderI18nSeedData 一致的实体 slug */
export const PURCHASEORDER_ENTITY_SLUG = 'purchaseorder'

/** entity.purchaseorder._self 静态属性（导入组件 entity-i18n-key 等） */
export const PURCHASEORDER_SELF_I18N_KEY = buildEntitySelfI18nKey(PURCHASEORDER_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PURCHASEORDER_LIST_FIELDS = [
  'purchaseOrderCode',
  'purchaseRequestId',
  'purchaseRequestCode',
  'supplierCode',
  'supplierName1',
  'orderDate',
  'requiredArrivalDate',
  'actualArrivalDate',
  'purchaseGroup',
  'purchaseOrderType',
  'paymentTerms',
  'pricingProcedure',
  'pricingConditionCode',
  'totalQuantity',
  'totalAmount',
  'discountAmount',
  'currencyCode',
  'exchangeRate',
  'taxCode',
  'taxRate',
  'taxAmount',
  'actualAmount',
  'receivedQuantity',
  'receivedAmount',
  'paidAmount',
  'paymentMethod',
  'deliveryMethod',
  'deliveryAddress',
  'orderStatus',
  'deliveryStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PURCHASEORDER_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  purchaseOrderCode: 'required',
  purchaseRequestId: 'optional',
  purchaseRequestCode: 'optional',
  supplierCode: 'select',
  supplierName1: 'optional',
  orderDate: 'select',
  requiredArrivalDate: 'optional',
  actualArrivalDate: 'optional',
  purchaseGroup: 'optional',
  purchaseOrderType: 'optional',
  paymentTerms: 'optional',
  pricingProcedure: 'optional',
  pricingConditionCode: 'optional',
  totalQuantity: 'select',
  totalAmount: 'select',
  discountAmount: 'select',
  currencyCode: 'select',
  exchangeRate: 'select',
  taxCode: 'optional',
  taxRate: 'select',
  taxAmount: 'select',
  actualAmount: 'select',
  receivedQuantity: 'select',
  receivedAmount: 'select',
  paidAmount: 'select',
  paymentMethod: 'select',
  deliveryMethod: 'select',
  deliveryAddress: 'optional',
  orderStatus: 'select',
  deliveryStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type PurchaseOrderField = keyof typeof PURCHASEORDER_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PURCHASEORDER_QUERY_STRING_FIELDS = [
  'cultureCode',
  'purchaseOrderCode',
  'purchaseRequestId',
  'purchaseRequestCode',
  'supplierCode',
  'supplierName1',
  'orderDateStart',
  'orderDateEnd',
  'requiredArrivalDateStart',
  'requiredArrivalDateEnd',
  'actualArrivalDateStart',
  'actualArrivalDateEnd',
  'purchaseGroup',
  'purchaseOrderType',
  'paymentTerms',
  'pricingProcedure',
  'pricingConditionCode',
  'currencyCode',
  'taxCode',
  'deliveryAddress',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof PurchaseOrderQuery)[]

export type PurchaseOrderQueryField =
  | (typeof PURCHASEORDER_QUERY_STRING_FIELDS)[number]
  | 'totalQuantity' | 'totalAmount' | 'discountAmount' | 'exchangeRate' | 'taxRate' | 'taxAmount' | 'actualAmount' | 'receivedQuantity' | 'receivedAmount' | 'paidAmount' | 'paymentMethod' | 'deliveryMethod' | 'orderStatus' | 'deliveryStatus'

/** 高级查询抽屉全部字段（含数值） */
export const PURCHASEORDER_QUERY_FIELDS: readonly PurchaseOrderQueryField[] = [
  ...PURCHASEORDER_QUERY_STRING_FIELDS,
  'totalQuantity',
  'totalAmount',
  'discountAmount',
  'exchangeRate',
  'taxRate',
  'taxAmount',
  'actualAmount',
  'receivedQuantity',
  'receivedAmount',
  'paidAmount',
  'paymentMethod',
  'deliveryMethod',
  'orderStatus',
  'deliveryStatus',
]

/**
 * Takt采购订单实体字段 i18n：index / purchase-order-form 统一入口
 */
export function usePurchaseOrderI18n() {
  const ef = useEntityFieldI18n(PURCHASEORDER_ENTITY_SLUG)

  function ph(field: PurchaseOrderField): string {
    return ef.placeholder(field, PURCHASEORDER_PLACEHOLDER[field])
  }

  function queryPh(field: PurchaseOrderQueryField, kind: EntityFieldPlaceholderKind): string {
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

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/sales/order/composables
// 文件名称：useOrderI18n.ts
// 功能描述：Takt销售订单实体字段清单 + useSalesOrderI18n（字段名映射一次，文案由 entity.salesorder.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SalesOrderQuery } from '@/types/logistics/sales/order.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSalesOrderI18nSeedData 一致的实体 slug */
export const SALESORDER_ENTITY_SLUG = 'salesorder'

/** entity.salesorder._self 静态属性（导入组件 entity-i18n-key 等） */
export const SALESORDER_SELF_I18N_KEY = buildEntitySelfI18nKey(SALESORDER_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SALESORDER_LIST_FIELDS = [
  'salesOrderCode',
  'customerCode',
  'customerName1',
  'orderDate',
  'requiredDeliveryDate',
  'actualDeliveryDate',
  'salesGroup',
  'salesOrderType',
  'orderReason',
  'salesOrganization',
  'pricingProcedure',
  'pricingConditionCode',
  'invoiceType',
  'purchaseOrderCode',
  'purchaseOrderDate',
  'totalQuantity',
  'totalAmount',
  'discountAmount',
  'currencyCode',
  'exchangeRate',
  'taxCode',
  'taxRate',
  'taxAmount',
  'actualAmount',
  'shippedQuantity',
  'shippedAmount',
  'receivedAmount',
  'deliveryMethod',
  'paymentMethod',
  'deliveryAddress',
  'orderStatus',
  'deliveryStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SALESORDER_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  salesOrderCode: 'required',
  customerCode: 'select',
  customerName1: 'optional',
  orderDate: 'select',
  requiredDeliveryDate: 'optional',
  actualDeliveryDate: 'optional',
  salesGroup: 'optional',
  salesOrderType: 'optional',
  orderReason: 'optional',
  salesOrganization: 'optional',
  pricingProcedure: 'optional',
  pricingConditionCode: 'optional',
  invoiceType: 'optional',
  purchaseOrderCode: 'optional',
  purchaseOrderDate: 'optional',
  totalQuantity: 'select',
  totalAmount: 'select',
  discountAmount: 'select',
  currencyCode: 'select',
  exchangeRate: 'select',
  taxCode: 'optional',
  taxRate: 'select',
  taxAmount: 'select',
  actualAmount: 'select',
  shippedQuantity: 'select',
  shippedAmount: 'select',
  receivedAmount: 'select',
  deliveryMethod: 'select',
  paymentMethod: 'select',
  deliveryAddress: 'optional',
  orderStatus: 'select',
  deliveryStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SalesOrderField = keyof typeof SALESORDER_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SALESORDER_QUERY_STRING_FIELDS = [
  'cultureCode',
  'salesOrderCode',
  'customerCode',
  'customerName1',
  'orderDateStart',
  'orderDateEnd',
  'requiredDeliveryDateStart',
  'requiredDeliveryDateEnd',
  'actualDeliveryDateStart',
  'actualDeliveryDateEnd',
  'salesGroup',
  'salesOrderType',
  'orderReason',
  'salesOrganization',
  'pricingProcedure',
  'pricingConditionCode',
  'invoiceType',
  'purchaseOrderCode',
  'purchaseOrderDateStart',
  'purchaseOrderDateEnd',
  'currencyCode',
  'taxCode',
  'deliveryAddress',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SalesOrderQuery)[]

export type SalesOrderQueryField =
  | (typeof SALESORDER_QUERY_STRING_FIELDS)[number]
  | 'totalQuantity' | 'totalAmount' | 'discountAmount' | 'exchangeRate' | 'taxRate' | 'taxAmount' | 'actualAmount' | 'shippedQuantity' | 'shippedAmount' | 'receivedAmount' | 'deliveryMethod' | 'paymentMethod' | 'orderStatus' | 'deliveryStatus'

/** 高级查询抽屉全部字段（含数值） */
export const SALESORDER_QUERY_FIELDS: readonly SalesOrderQueryField[] = [
  ...SALESORDER_QUERY_STRING_FIELDS,
  'totalQuantity',
  'totalAmount',
  'discountAmount',
  'exchangeRate',
  'taxRate',
  'taxAmount',
  'actualAmount',
  'shippedQuantity',
  'shippedAmount',
  'receivedAmount',
  'deliveryMethod',
  'paymentMethod',
  'orderStatus',
  'deliveryStatus',
]

/**
 * Takt销售订单实体字段 i18n：index / order-form 统一入口
 */
export function useSalesOrderI18n() {
  const ef = useEntityFieldI18n(SALESORDER_ENTITY_SLUG)

  function ph(field: SalesOrderField): string {
    return ef.placeholder(field, SALESORDER_PLACEHOLDER[field])
  }

  function queryPh(field: SalesOrderQueryField, kind: EntityFieldPlaceholderKind): string {
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

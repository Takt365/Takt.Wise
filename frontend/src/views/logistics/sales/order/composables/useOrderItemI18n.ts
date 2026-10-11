// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/sales/order/composables
// 文件名称：useOrderItemI18n.ts
// 功能描述：SalesOrderItem字段清单 + useSalesOrderItemI18n（字段名映射一次，文案由 entity.salesorderitem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SalesOrderItemQuery } from '@/types/logistics/sales/orderItem.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSalesOrderItemI18nSeedData 一致的实体 slug */
export const SALESORDERITEM_ENTITY_SLUG = 'salesorderitem'

/** entity.salesorderitem._self 静态属性（导入组件 entity-i18n-key 等） */
export const SALESORDERITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(SALESORDERITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SALESORDERITEM_LIST_FIELDS = [
  'salesOrderId',
  'salesOrderName',
  'salesOrderCode',
  'lineNumber',
  'materialCode',
  'materialDescription',
  'materialSpecification',
  'salesUnit',
  'orderQuantity',
  'targetQuantity',
  'shippedQuantity',
  'salesPerUnit',
  'salesUnitPrice',
  'discountRate',
  'discountAmount',
  'taxIncludedAmount',
  'untaxedAmount',
  'taxAmount',
  'salesAmount',
  'grossWeight',
  'netWeight',
  'weightUnit',
  'volume',
  'volumeUnit',
  'profitCenterCode',
  'deliveryStatus',
  'isObsolete',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const SALESORDERITEM_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'salesOrderId',
  'salesOrderName',
  'salesOrderCode',
  'lineNumber',
  'materialCode',
  'materialDescription',
  'materialSpecification',
  'salesUnit',
  'orderQuantity',
  'targetQuantity',
  'shippedQuantity',
  'salesPerUnit',
  'salesUnitPrice',
  'discountRate',
  'discountAmount',
  'taxIncludedAmount',
  'untaxedAmount',
  'taxAmount',
  'salesAmount',
  'grossWeight',
  'netWeight',
  'weightUnit',
  'volume',
  'volumeUnit',
  'profitCenterCode',
  'deliveryStatus',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const SALESORDERITEM_SUMMARY_SUM_FIELDS = [
  'orderQuantity',
  'targetQuantity',
  'shippedQuantity',
  'salesPerUnit',
  'salesUnitPrice',
  'discountRate',
  'discountAmount',
  'taxIncludedAmount',
  'untaxedAmount',
  'taxAmount',
  'salesAmount',
  'grossWeight',
  'netWeight',
  'volume',
  'deliveryStatus',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SALESORDERITEM_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  lineNumber: 'select',
  materialCode: 'select',
  materialDescription: 'optional',
  materialSpecification: 'optional',
  salesUnit: 'select',
  orderQuantity: 'select',
  targetQuantity: 'select',
  shippedQuantity: 'select',
  salesPerUnit: 'select',
  salesUnitPrice: 'select',
  discountRate: 'select',
  discountAmount: 'select',
  taxIncludedAmount: 'select',
  untaxedAmount: 'select',
  taxAmount: 'select',
  salesAmount: 'select',
  grossWeight: 'optional',
  netWeight: 'optional',
  weightUnit: 'optional',
  volume: 'optional',
  volumeUnit: 'optional',
  profitCenterCode: 'optional',
  deliveryStatus: 'select',
  isObsolete: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SalesOrderItemField = keyof typeof SALESORDERITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SALESORDERITEM_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'salesOrderCode',
  'materialCode',
  'materialDescription',
  'materialSpecification',
  'salesUnit',
  'weightUnit',
  'volumeUnit',
  'profitCenterCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SalesOrderItemQuery)[]

export type SalesOrderItemQueryField =
  | (typeof SALESORDERITEM_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'orderQuantity' | 'targetQuantity' | 'shippedQuantity' | 'salesPerUnit' | 'salesUnitPrice' | 'discountRate' | 'discountAmount' | 'taxIncludedAmount' | 'untaxedAmount' | 'taxAmount' | 'salesAmount' | 'grossWeight' | 'netWeight' | 'volume' | 'deliveryStatus' | 'isObsolete'

/** 高级查询抽屉全部字段（含数值） */
export const SALESORDERITEM_QUERY_FIELDS: readonly SalesOrderItemQueryField[] = [
  ...SALESORDERITEM_QUERY_STRING_FIELDS,
  'lineNumber',
  'orderQuantity',
  'targetQuantity',
  'shippedQuantity',
  'salesPerUnit',
  'salesUnitPrice',
  'discountRate',
  'discountAmount',
  'taxIncludedAmount',
  'untaxedAmount',
  'taxAmount',
  'salesAmount',
  'grossWeight',
  'netWeight',
  'volume',
  'deliveryStatus',
  'isObsolete',
]

/**
 * SalesOrderItem字段 i18n：index / order-item-form 统一入口
 */
export function useSalesOrderItemI18n() {
  const ef = useEntityFieldI18n(SALESORDERITEM_ENTITY_SLUG)

  function ph(field: SalesOrderItemField): string {
    return ef.placeholder(field, SALESORDERITEM_PLACEHOLDER[field])
  }

  function queryPh(field: SalesOrderItemQueryField, kind: EntityFieldPlaceholderKind): string {
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

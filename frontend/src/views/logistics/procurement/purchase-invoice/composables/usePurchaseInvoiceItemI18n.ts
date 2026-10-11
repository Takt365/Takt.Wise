// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/procurement/purchase-invoice/composables
// 文件名称：usePurchaseInvoiceItemI18n.ts
// 功能描述：PurchaseInvoiceItem字段清单 + usePurchaseInvoiceItemI18n（字段名映射一次，文案由 entity.purchaseinvoiceitem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PurchaseInvoiceItemQuery } from '@/types/logistics/procurement/purchaseInvoiceItem.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktPurchaseInvoiceItemI18nSeedData 一致的实体 slug */
export const PURCHASEINVOICEITEM_ENTITY_SLUG = 'purchaseinvoiceitem'

/** entity.purchaseinvoiceitem._self 静态属性（导入组件 entity-i18n-key 等） */
export const PURCHASEINVOICEITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(PURCHASEINVOICEITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PURCHASEINVOICEITEM_LIST_FIELDS = [
  'purchaseInvoiceId',
  'purchaseInvoiceCode',
  'lineNumber',
  'purchaseOrderCode',
  'purchaseOrderItem',
  'accountAssignmentSeq',
  'materialCode',
  'valuationArea',
  'amount',
  'debitCreditIndicator',
  'taxCode',
  'quantity',
  'orderUnit',
  'poPriceQuantity',
  'poPriceUnit',
  'valuatedStockQuantity',
  'previousPeriodStock',
  'baseUnit',
  'valuationClass',
  'updatePoHistoryFlag',
  'subsequentDebitCredit',
  'blockReasonPrice',
  'blockReasonQuantity',
  'blockReasonQuality',
  'blockReasonEnhanced',
  'valueString',
  'referenceCode',
  'conditionType',
  'totalValuatedStockValue',
  'previousPeriodValue',
  'referenceDocumentCode',
  'referenceDocumentYear',
  'referenceDocumentItem',
  'stockManagedMaterialCode',
  'itemText',
  'materialDocumentItem',
  'isObsolete',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const PURCHASEINVOICEITEM_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'purchaseInvoiceId',
  'purchaseInvoiceCode',
  'lineNumber',
  'purchaseOrderCode',
  'purchaseOrderItem',
  'accountAssignmentSeq',
  'materialCode',
  'valuationArea',
  'amount',
  'debitCreditIndicator',
  'taxCode',
  'quantity',
  'orderUnit',
  'poPriceQuantity',
  'poPriceUnit',
  'valuatedStockQuantity',
  'previousPeriodStock',
  'baseUnit',
  'valuationClass',
  'updatePoHistoryFlag',
  'subsequentDebitCredit',
  'blockReasonPrice',
  'blockReasonQuantity',
  'blockReasonQuality',
  'blockReasonEnhanced',
  'valueString',
  'referenceCode',
  'conditionType',
  'totalValuatedStockValue',
  'previousPeriodValue',
  'referenceDocumentCode',
  'referenceDocumentYear',
  'referenceDocumentItem',
  'stockManagedMaterialCode',
  'itemText',
  'materialDocumentItem',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const PURCHASEINVOICEITEM_SUMMARY_SUM_FIELDS = [
  'purchaseOrderItem',
  'amount',
  'quantity',
  'poPriceQuantity',
  'valuatedStockQuantity',
  'previousPeriodStock',
  'totalValuatedStockValue',
  'previousPeriodValue',
  'referenceDocumentItem',
  'materialDocumentItem',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PURCHASEINVOICEITEM_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  lineNumber: 'select',
  purchaseOrderCode: 'optional',
  purchaseOrderItem: 'optional',
  accountAssignmentSeq: 'optional',
  materialCode: 'optional',
  valuationArea: 'optional',
  amount: 'optional',
  debitCreditIndicator: 'optional',
  taxCode: 'optional',
  quantity: 'optional',
  orderUnit: 'optional',
  poPriceQuantity: 'optional',
  poPriceUnit: 'optional',
  valuatedStockQuantity: 'optional',
  previousPeriodStock: 'optional',
  baseUnit: 'optional',
  valuationClass: 'optional',
  updatePoHistoryFlag: 'optional',
  subsequentDebitCredit: 'optional',
  blockReasonPrice: 'optional',
  blockReasonQuantity: 'optional',
  blockReasonQuality: 'optional',
  blockReasonEnhanced: 'optional',
  valueString: 'optional',
  referenceCode: 'optional',
  conditionType: 'optional',
  totalValuatedStockValue: 'optional',
  previousPeriodValue: 'optional',
  referenceDocumentCode: 'optional',
  referenceDocumentYear: 'optional',
  referenceDocumentItem: 'optional',
  stockManagedMaterialCode: 'optional',
  itemText: 'optional',
  materialDocumentItem: 'optional',
  isObsolete: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type PurchaseInvoiceItemField = keyof typeof PURCHASEINVOICEITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PURCHASEINVOICEITEM_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'purchaseInvoiceCode',
  'purchaseOrderCode',
  'accountAssignmentSeq',
  'materialCode',
  'valuationArea',
  'debitCreditIndicator',
  'taxCode',
  'orderUnit',
  'poPriceUnit',
  'baseUnit',
  'valuationClass',
  'updatePoHistoryFlag',
  'subsequentDebitCredit',
  'blockReasonPrice',
  'blockReasonQuantity',
  'blockReasonQuality',
  'blockReasonEnhanced',
  'valueString',
  'referenceCode',
  'conditionType',
  'referenceDocumentCode',
  'referenceDocumentYear',
  'stockManagedMaterialCode',
  'itemText',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof PurchaseInvoiceItemQuery)[]

export type PurchaseInvoiceItemQueryField =
  | (typeof PURCHASEINVOICEITEM_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'purchaseOrderItem' | 'amount' | 'quantity' | 'poPriceQuantity' | 'valuatedStockQuantity' | 'previousPeriodStock' | 'totalValuatedStockValue' | 'previousPeriodValue' | 'referenceDocumentItem' | 'materialDocumentItem' | 'isObsolete'

/** 高级查询抽屉全部字段（含数值） */
export const PURCHASEINVOICEITEM_QUERY_FIELDS: readonly PurchaseInvoiceItemQueryField[] = [
  ...PURCHASEINVOICEITEM_QUERY_STRING_FIELDS,
  'lineNumber',
  'purchaseOrderItem',
  'amount',
  'quantity',
  'poPriceQuantity',
  'valuatedStockQuantity',
  'previousPeriodStock',
  'totalValuatedStockValue',
  'previousPeriodValue',
  'referenceDocumentItem',
  'materialDocumentItem',
  'isObsolete',
]

/**
 * PurchaseInvoiceItem字段 i18n：index / purchase-invoice-item-form 统一入口
 */
export function usePurchaseInvoiceItemI18n() {
  const ef = useEntityFieldI18n(PURCHASEINVOICEITEM_ENTITY_SLUG)

  function ph(field: PurchaseInvoiceItemField): string {
    return ef.placeholder(field, PURCHASEINVOICEITEM_PLACEHOLDER[field])
  }

  function queryPh(field: PurchaseInvoiceItemQueryField, kind: EntityFieldPlaceholderKind): string {
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

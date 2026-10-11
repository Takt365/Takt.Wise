// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/procurement/purchase-inquiry/composables
// 文件名称：usePurchaseInquiryI18n.ts
// 功能描述：采购询价实体字段清单 + usePurchaseInquiryI18n（字段名映射一次，文案由 entity.purchaseinquiry.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PurchaseInquiryQuery } from '@/types/logistics/procurement/purchaseInquiry.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktPurchaseInquiryI18nSeedData 一致的实体 slug */
export const PURCHASEINQUIRY_ENTITY_SLUG = 'purchaseinquiry'

/** entity.purchaseinquiry._self 静态属性（导入组件 entity-i18n-key 等） */
export const PURCHASEINQUIRY_SELF_I18N_KEY = buildEntitySelfI18nKey(PURCHASEINQUIRY_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PURCHASEINQUIRY_LIST_FIELDS = [
  'purchaseInquiryCode',
  'inquiryDate',
  'quoteDeadlineDate',
  'inquiryEmployeeId',
  'inquiryEmployeeName',
  'supplierCode',
  'supplierName1',
  'purchaseInquiryType',
  'pricingProcedure',
  'pricingConditionCode',
  'currencyCode',
  'taxCode',
  'taxRate',
  'taxAmount',
  'paymentMode',
  'chainScheme',
  'totalQuantity',
  'totalAmount',
  'convertedQuantity',
  'convertedAmount',
  'inquiryReason',
  'inquiryStatus',
  'convertedStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PURCHASEINQUIRY_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  purchaseInquiryCode: 'required',
  inquiryDate: 'select',
  quoteDeadlineDate: 'optional',
  inquiryEmployeeId: 'optional',
  inquiryEmployeeName: 'optional',
  supplierCode: 'select',
  supplierName1: 'optional',
  purchaseInquiryType: 'optional',
  pricingProcedure: 'optional',
  pricingConditionCode: 'optional',
  currencyCode: 'select',
  taxCode: 'optional',
  taxRate: 'select',
  taxAmount: 'select',
  paymentMode: 'select',
  chainScheme: 'select',
  totalQuantity: 'select',
  totalAmount: 'select',
  convertedQuantity: 'select',
  convertedAmount: 'select',
  inquiryReason: 'optional',
  inquiryStatus: 'select',
  convertedStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type PurchaseInquiryField = keyof typeof PURCHASEINQUIRY_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PURCHASEINQUIRY_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'purchaseInquiryCode',
  'inquiryDateStart',
  'inquiryDateEnd',
  'quoteDeadlineDateStart',
  'quoteDeadlineDateEnd',
  'inquiryEmployeeId',
  'inquiryEmployeeName',
  'supplierCode',
  'supplierName1',
  'purchaseInquiryType',
  'pricingProcedure',
  'pricingConditionCode',
  'currencyCode',
  'taxCode',
  'paymentMode',
  'inquiryReason',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof PurchaseInquiryQuery)[]

export type PurchaseInquiryQueryField =
  | (typeof PURCHASEINQUIRY_QUERY_STRING_FIELDS)[number]
  | 'taxRate' | 'taxAmount' | 'chainScheme' | 'totalQuantity' | 'totalAmount' | 'convertedQuantity' | 'convertedAmount' | 'inquiryStatus' | 'convertedStatus'

/** 高级查询抽屉全部字段（含数值） */
export const PURCHASEINQUIRY_QUERY_FIELDS: readonly PurchaseInquiryQueryField[] = [
  ...PURCHASEINQUIRY_QUERY_STRING_FIELDS,
  'taxRate',
  'taxAmount',
  'chainScheme',
  'totalQuantity',
  'totalAmount',
  'convertedQuantity',
  'convertedAmount',
  'inquiryStatus',
  'convertedStatus',
]

/**
 * 采购询价实体字段 i18n：index / purchase-inquiry-form 统一入口
 */
export function usePurchaseInquiryI18n() {
  const ef = useEntityFieldI18n(PURCHASEINQUIRY_ENTITY_SLUG)

  function ph(field: PurchaseInquiryField): string {
    return ef.placeholder(field, PURCHASEINQUIRY_PLACEHOLDER[field])
  }

  function queryPh(field: PurchaseInquiryQueryField, kind: EntityFieldPlaceholderKind): string {
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

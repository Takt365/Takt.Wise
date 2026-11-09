// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/procurement/purchase-condition/composables
// 文件名称：usePurchaseConditionI18n.ts
// 功能描述：Takt采购条件实体字段清单 + usePurchaseConditionI18n（字段名映射一次，文案由 entity.purchasecondition.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PurchaseConditionQuery } from '@/types/logistics/procurement/purchaseCondition.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktPurchaseConditionI18nSeedData 一致的实体 slug */
export const PURCHASECONDITION_ENTITY_SLUG = 'purchasecondition'

/** entity.purchasecondition._self 静态属性（导入组件 entity-i18n-key 等） */
export const PURCHASECONDITION_SELF_I18N_KEY = buildEntitySelfI18nKey(PURCHASECONDITION_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PURCHASECONDITION_LIST_FIELDS = [
  'purchaseConditionCode',
  'priceType',
  'supplierCode',
  'materialCode',
  'materialDescription',
  'purchaseGroup',
  'taxCode',
  'grBasedInvoiceInspection',
  'pricingDateControl',
  'validFrom',
  'validTo',
  'purchaseInquiryId',
  'purchaseInquiryCode',
  'variableKey',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PURCHASECONDITION_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  numberingRuleCode: 'select',
  purchaseConditionCode: 'optional',
  priceType: 'select',
  supplierCode: 'select',
  materialCode: 'select',
  materialDescription: 'optional',
  purchaseGroup: 'optional',
  taxCode: 'optional',
  grBasedInvoiceInspection: 'select',
  pricingDateControl: 'select',
  validFrom: 'select',
  validTo: 'select',
  purchaseInquiryId: 'optional',
  purchaseInquiryCode: 'optional',
  variableKey: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type PurchaseConditionField = keyof typeof PURCHASECONDITION_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PURCHASECONDITION_QUERY_STRING_FIELDS = [
  'cultureCode',
  'purchaseConditionCode',
  'priceType',
  'supplierCode',
  'materialCode',
  'materialDescription',
  'purchaseGroup',
  'taxCode',
  'validFromStart',
  'validFromEnd',
  'validToStart',
  'validToEnd',
  'purchaseInquiryId',
  'purchaseInquiryCode',
  'variableKey',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof PurchaseConditionQuery)[]

export type PurchaseConditionQueryField =
  | (typeof PURCHASECONDITION_QUERY_STRING_FIELDS)[number]
  | 'grBasedInvoiceInspection' | 'pricingDateControl'

/** 高级查询抽屉全部字段（含数值） */
export const PURCHASECONDITION_QUERY_FIELDS: readonly PurchaseConditionQueryField[] = [
  ...PURCHASECONDITION_QUERY_STRING_FIELDS,
  'grBasedInvoiceInspection',
  'pricingDateControl',
]

/**
 * Takt采购条件实体字段 i18n：index / PurchaseConditionForm 统一入口
 */
export function usePurchaseConditionI18n() {
  const ef = useEntityFieldI18n(PURCHASECONDITION_ENTITY_SLUG)

  function ph(field: PurchaseConditionField): string {
    return ef.placeholder(field, PURCHASECONDITION_PLACEHOLDER[field])
  }

  function queryPh(field: PurchaseConditionQueryField, kind: EntityFieldPlaceholderKind): string {
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

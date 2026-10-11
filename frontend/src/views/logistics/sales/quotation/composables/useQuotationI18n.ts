// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/sales/quotation/composables
// 文件名称：useQuotationI18n.ts
// 功能描述：Takt销售报价实体字段清单 + useSalesQuotationI18n（字段名映射一次，文案由 entity.salesquotation.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SalesQuotationQuery } from '@/types/logistics/sales/quotation.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSalesQuotationI18nSeedData 一致的实体 slug */
export const SALESQUOTATION_ENTITY_SLUG = 'salesquotation'

/** entity.salesquotation._self 静态属性（导入组件 entity-i18n-key 等） */
export const SALESQUOTATION_SELF_I18N_KEY = buildEntitySelfI18nKey(SALESQUOTATION_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SALESQUOTATION_LIST_FIELDS = [
  'salesQuotationCode',
  'customerCode',
  'customerName1',
  'quotationDate',
  'validUntilDate',
  'salesGroup',
  'salesQuotationType',
  'pricingProcedure',
  'pricingConditionCode',
  'totalQuantity',
  'totalAmount',
  'discountAmount',
  'currencyCode',
  'taxCode',
  'taxRate',
  'taxAmount',
  'actualAmount',
  'salesOrderCode',
  'quotationStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SALESQUOTATION_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  salesQuotationCode: 'required',
  customerCode: 'select',
  customerName1: 'optional',
  quotationDate: 'select',
  validUntilDate: 'optional',
  salesGroup: 'optional',
  salesQuotationType: 'optional',
  pricingProcedure: 'optional',
  pricingConditionCode: 'optional',
  totalQuantity: 'select',
  totalAmount: 'select',
  discountAmount: 'select',
  currencyCode: 'select',
  taxCode: 'optional',
  taxRate: 'select',
  taxAmount: 'select',
  actualAmount: 'select',
  salesOrderCode: 'optional',
  quotationStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SalesQuotationField = keyof typeof SALESQUOTATION_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SALESQUOTATION_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'salesQuotationCode',
  'customerCode',
  'customerName1',
  'quotationDateStart',
  'quotationDateEnd',
  'validUntilDateStart',
  'validUntilDateEnd',
  'salesGroup',
  'salesQuotationType',
  'pricingProcedure',
  'pricingConditionCode',
  'currencyCode',
  'taxCode',
  'salesOrderCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SalesQuotationQuery)[]

export type SalesQuotationQueryField =
  | (typeof SALESQUOTATION_QUERY_STRING_FIELDS)[number]
  | 'totalQuantity' | 'totalAmount' | 'discountAmount' | 'taxRate' | 'taxAmount' | 'actualAmount' | 'quotationStatus'

/** 高级查询抽屉全部字段（含数值） */
export const SALESQUOTATION_QUERY_FIELDS: readonly SalesQuotationQueryField[] = [
  ...SALESQUOTATION_QUERY_STRING_FIELDS,
  'totalQuantity',
  'totalAmount',
  'discountAmount',
  'taxRate',
  'taxAmount',
  'actualAmount',
  'quotationStatus',
]

/**
 * Takt销售报价实体字段 i18n：index / quotation-form 统一入口
 */
export function useSalesQuotationI18n() {
  const ef = useEntityFieldI18n(SALESQUOTATION_ENTITY_SLUG)

  function ph(field: SalesQuotationField): string {
    return ef.placeholder(field, SALESQUOTATION_PLACEHOLDER[field])
  }

  function queryPh(field: SalesQuotationQueryField, kind: EntityFieldPlaceholderKind): string {
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

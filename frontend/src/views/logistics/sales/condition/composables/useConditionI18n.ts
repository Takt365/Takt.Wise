// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/sales/condition/composables
// 文件名称：useConditionI18n.ts
// 功能描述：Takt销售条件实体字段清单 + useSalesConditionI18n（字段名映射一次，文案由 entity.salescondition.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SalesConditionQuery } from '@/types/logistics/sales/condition.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSalesConditionI18nSeedData 一致的实体 slug */
export const SALESCONDITIONRECORD_ENTITY_SLUG = 'salescondition'

/** entity.salescondition._self 静态属性（导入组件 entity-i18n-key 等） */
export const SALESCONDITIONRECORD_SELF_I18N_KEY = buildEntitySelfI18nKey(SALESCONDITIONRECORD_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SALESCONDITIONRECORD_LIST_FIELDS = [
  'salesConditionCode',
  'priceType',
  'customerCode',
  'materialCode',
  'materialDescription',
  'salesGroup',
  'taxCode',
  'grBasedInvoiceInspection',
  'pricingDateControl',
  'validFrom',
  'validTo',
  'salesQuotationId',
  'salesQuotationCode',
  'variableKey',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SALESCONDITIONRECORD_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  salesConditionCode: 'required',
  priceType: 'select',
  customerCode: 'select',
  materialCode: 'select',
  materialDescription: 'optional',
  salesGroup: 'optional',
  taxCode: 'optional',
  grBasedInvoiceInspection: 'select',
  pricingDateControl: 'select',
  validFrom: 'select',
  validTo: 'select',
  salesQuotationId: 'optional',
  salesQuotationCode: 'optional',
  variableKey: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SalesConditionField = keyof typeof SALESCONDITIONRECORD_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SALESCONDITIONRECORD_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'salesConditionCode',
  'priceType',
  'customerCode',
  'materialCode',
  'materialDescription',
  'salesGroup',
  'taxCode',
  'validFromStart',
  'validFromEnd',
  'validToStart',
  'validToEnd',
  'salesQuotationId',
  'salesQuotationCode',
  'variableKey',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SalesConditionQuery)[]

export type SalesConditionQueryField =
  | (typeof SALESCONDITIONRECORD_QUERY_STRING_FIELDS)[number]
  | 'grBasedInvoiceInspection' | 'pricingDateControl'

/** 高级查询抽屉全部字段（含数值） */
export const SALESCONDITIONRECORD_QUERY_FIELDS: readonly SalesConditionQueryField[] = [
  ...SALESCONDITIONRECORD_QUERY_STRING_FIELDS,
  'grBasedInvoiceInspection',
  'pricingDateControl',
]

/**
 * Takt销售条件实体字段 i18n：index / SalesConditionForm 统一入口
 */
export function useSalesConditionI18n() {
  const ef = useEntityFieldI18n(SALESCONDITIONRECORD_ENTITY_SLUG)

  function ph(field: SalesConditionField): string {
    return ef.placeholder(field, SALESCONDITIONRECORD_PLACEHOLDER[field])
  }

  function queryPh(field: SalesConditionQueryField, kind: EntityFieldPlaceholderKind): string {
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

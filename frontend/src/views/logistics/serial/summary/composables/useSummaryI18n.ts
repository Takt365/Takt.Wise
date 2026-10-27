// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/serial/summary/composables
// 文件名称：useSummaryI18n.ts
// 功能描述：序列号汇总字段清单 + useSerialSummaryI18n（字段名映射一次，文案由 entity.serialsummary.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SerialSummaryQuery } from '@/types/logistics/serial/summary.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSerialSummaryI18nSeedData 一致的实体 slug */
export const SERIALSUMMARY_ENTITY_SLUG = 'serialsummary'

/** entity.serialsummary._self 静态属性（导入组件 entity-i18n-key 等） */
export const SERIALSUMMARY_SELF_I18N_KEY = buildEntitySelfI18nKey(SERIALSUMMARY_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SERIALSUMMARY_LIST_FIELDS = [
  'plantCode',
  'inboundCode',
  'inboundDate',
  'materialCode',
  'inboundSerialCode',
  'inboundQuantity',
  'productInboundSerialCode',
  'outboundCode',
  'shippingInvoiceCode',
  'loadingDate',
  'destination',
  'destinationPort',
  'outboundDate',
  'outboundSerialCode',
  'outboundQuantity',
  'productOutboundSerialCode',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SERIALSUMMARY_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  plantCode: 'select',
  inboundCode: 'required',
  inboundDate: 'select',
  materialCode: 'select',
  inboundSerialCode: 'required',
  inboundQuantity: 'select',
  productInboundSerialCode: 'required',
  outboundCode: 'required',
  shippingInvoiceCode: 'required',
  loadingDate: 'optional',
  destination: 'select',
  destinationPort: 'select',
  outboundDate: 'optional',
  outboundSerialCode: 'required',
  outboundQuantity: 'select',
  productOutboundSerialCode: 'required',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SerialSummaryField = keyof typeof SERIALSUMMARY_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SERIALSUMMARY_QUERY_STRING_FIELDS = [
  'inboundCode',
  'inboundDateStart',
  'inboundDateEnd',
  'materialCode',
  'inboundSerialCode',
  'productInboundSerialCode',
  'outboundCode',
  'shippingInvoiceCode',
  'loadingDateStart',
  'loadingDateEnd',
  'destination',
  'destinationPort',
  'outboundDateStart',
  'outboundDateEnd',
  'outboundSerialCode',
  'productOutboundSerialCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SerialSummaryQuery)[]

export type SerialSummaryQueryField =
  | (typeof SERIALSUMMARY_QUERY_STRING_FIELDS)[number]
  | 'inboundQuantity' | 'outboundQuantity'

/** 高级查询抽屉全部字段（含数值） */
export const SERIALSUMMARY_QUERY_FIELDS: readonly SerialSummaryQueryField[] = [
  ...SERIALSUMMARY_QUERY_STRING_FIELDS,
  'inboundQuantity',
  'outboundQuantity',
]

/**
 * 序列号汇总字段 i18n：index / summary-form 统一入口
 */
export function useSerialSummaryI18n() {
  const ef = useEntityFieldI18n(SERIALSUMMARY_ENTITY_SLUG)

  function ph(field: SerialSummaryField): string {
    return ef.placeholder(field, SERIALSUMMARY_PLACEHOLDER[field])
  }

  function queryPh(field: SerialSummaryQueryField, kind: EntityFieldPlaceholderKind): string {
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

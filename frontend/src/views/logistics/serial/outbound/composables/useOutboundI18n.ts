// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/serial/outbound/composables
// 文件名称：useOutboundI18n.ts
// 功能描述：序列号出库主表实体字段清单 + useSerialOutboundI18n（字段名映射一次，文案由 entity.serialoutbound.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SerialOutboundQuery } from '@/types/logistics/serial/outbound.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSerialOutboundI18nSeedData 一致的实体 slug */
export const SERIALOUTBOUND_ENTITY_SLUG = 'serialoutbound'

/** entity.serialoutbound._self 静态属性（导入组件 entity-i18n-key 等） */
export const SERIALOUTBOUND_SELF_I18N_KEY = buildEntitySelfI18nKey(SERIALOUTBOUND_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SERIALOUTBOUND_LIST_FIELDS = [
  'plantCode',
  'outboundCode',
  'shippingInvoiceCode',
  'outboundDate',
  'destination',
  'destinationPort',
  'outboundType',
  'warehouseCode',
  'locationCode',
  'totalQuantity',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SERIALOUTBOUND_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  outboundCode: 'required',
  shippingInvoiceCode: 'required',
  outboundDate: 'select',
  destination: 'select',
  destinationPort: 'select',
  outboundType: 'select',
  warehouseCode: 'select',
  locationCode: 'select',
  totalQuantity: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SerialOutboundField = keyof typeof SERIALOUTBOUND_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SERIALOUTBOUND_QUERY_STRING_FIELDS = [
  'outboundCode',
  'shippingInvoiceCode',
  'outboundDateStart',
  'outboundDateEnd',
  'destination',
  'destinationPort',
  'warehouseCode',
  'locationCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SerialOutboundQuery)[]

export type SerialOutboundQueryField =
  | (typeof SERIALOUTBOUND_QUERY_STRING_FIELDS)[number]
  | 'outboundType' | 'totalQuantity'

/** 高级查询抽屉全部字段（含数值） */
export const SERIALOUTBOUND_QUERY_FIELDS: readonly SerialOutboundQueryField[] = [
  ...SERIALOUTBOUND_QUERY_STRING_FIELDS,
  'outboundType',
  'totalQuantity',
]

/**
 * 序列号出库主表实体字段 i18n：index / outbound-form 统一入口
 */
export function useSerialOutboundI18n() {
  const ef = useEntityFieldI18n(SERIALOUTBOUND_ENTITY_SLUG)

  function ph(field: SerialOutboundField): string {
    return ef.placeholder(field, SERIALOUTBOUND_PLACEHOLDER[field])
  }

  function queryPh(field: SerialOutboundQueryField, kind: EntityFieldPlaceholderKind): string {
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

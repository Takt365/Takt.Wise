// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/serial/inbound/composables
// 文件名称：useInboundI18n.ts
// 功能描述：序列号入库主表实体字段清单 + useSerialInboundI18n（字段名映射一次，文案由 entity.serialinbound.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SerialInboundQuery } from '@/types/logistics/serial/inbound.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSerialInboundI18nSeedData 一致的实体 slug */
export const SERIALINBOUND_ENTITY_SLUG = 'serialinbound'

/** entity.serialinbound._self 静态属性（导入组件 entity-i18n-key 等） */
export const SERIALINBOUND_SELF_I18N_KEY = buildEntitySelfI18nKey(SERIALINBOUND_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SERIALINBOUND_LIST_FIELDS = [
  'plantCode',
  'inboundCode',
  'inboundDate',
  'inboundType',
  'warehouseCode',
  'locationCode',
  'totalQuantity',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SERIALINBOUND_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  inboundCode: 'required',
  inboundDate: 'select',
  inboundType: 'select',
  warehouseCode: 'select',
  locationCode: 'select',
  totalQuantity: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SerialInboundField = keyof typeof SERIALINBOUND_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SERIALINBOUND_QUERY_STRING_FIELDS = [
  'plantCode',
  'inboundCode',
  'inboundDateStart',
  'inboundDateEnd',
  'warehouseCode',
  'locationCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SerialInboundQuery)[]

export type SerialInboundQueryField =
  | (typeof SERIALINBOUND_QUERY_STRING_FIELDS)[number]
  | 'inboundType' | 'totalQuantity'

/** 高级查询抽屉全部字段（含数值） */
export const SERIALINBOUND_QUERY_FIELDS: readonly SerialInboundQueryField[] = [
  ...SERIALINBOUND_QUERY_STRING_FIELDS,
  'inboundType',
  'totalQuantity',
]

/**
 * 序列号入库主表实体字段 i18n：index / inbound-form 统一入口
 */
export function useSerialInboundI18n() {
  const ef = useEntityFieldI18n(SERIALINBOUND_ENTITY_SLUG)

  function ph(field: SerialInboundField): string {
    return ef.placeholder(field, SERIALINBOUND_PLACEHOLDER[field])
  }

  function queryPh(field: SerialInboundQueryField, kind: EntityFieldPlaceholderKind): string {
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

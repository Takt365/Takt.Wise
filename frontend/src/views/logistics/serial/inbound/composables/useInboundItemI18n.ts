// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/serial/inbound/composables
// 文件名称：useInboundItemI18n.ts
// 功能描述：SerialInboundItem字段清单 + useSerialInboundItemI18n（字段名映射一次，文案由 entity.serialinbounditem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SerialInboundItemQuery } from '@/types/logistics/serial/inboundItem.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSerialInboundItemI18nSeedData 一致的实体 slug */
export const SERIALINBOUNDITEM_ENTITY_SLUG = 'serialinbounditem'

/** entity.serialinbounditem._self 静态属性（导入组件 entity-i18n-key 等） */
export const SERIALINBOUNDITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(SERIALINBOUNDITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SERIALINBOUNDITEM_LIST_FIELDS = [
  'inboundId',
  'inboundCode',
  'lineNumber',
  'inboundSerialCode',
  'isObsolete',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const SERIALINBOUNDITEM_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'inboundId',
  'inboundCode',
  'lineNumber',
  'inboundSerialCode',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const SERIALINBOUNDITEM_SUMMARY_SUM_FIELDS = [
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SERIALINBOUNDITEM_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  inboundCode: 'optional',
  lineNumber: 'select',
  inboundSerialCode: 'required',
  isObsolete: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SerialInboundItemField = keyof typeof SERIALINBOUNDITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SERIALINBOUNDITEM_QUERY_STRING_FIELDS = [
  'inboundCode',
  'inboundSerialCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SerialInboundItemQuery)[]

export type SerialInboundItemQueryField =
  | (typeof SERIALINBOUNDITEM_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'isObsolete'

/** 高级查询抽屉全部字段（含数值） */
export const SERIALINBOUNDITEM_QUERY_FIELDS: readonly SerialInboundItemQueryField[] = [
  ...SERIALINBOUNDITEM_QUERY_STRING_FIELDS,
  'lineNumber',
  'isObsolete',
]

/**
 * SerialInboundItem字段 i18n：index / inbound-item-form 统一入口
 */
export function useSerialInboundItemI18n() {
  const ef = useEntityFieldI18n(SERIALINBOUNDITEM_ENTITY_SLUG)

  function ph(field: SerialInboundItemField): string {
    return ef.placeholder(field, SERIALINBOUNDITEM_PLACEHOLDER[field])
  }

  function queryPh(field: SerialInboundItemQueryField, kind: EntityFieldPlaceholderKind): string {
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

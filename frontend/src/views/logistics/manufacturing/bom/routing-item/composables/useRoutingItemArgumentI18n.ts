// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/bom/routing-item/composables
// 文件名称：useRoutingItemArgumentI18n.ts
// 功能描述：RoutingItemArgument字段清单 + useRoutingItemArgumentI18n（字段名映射一次，文案由 entity.routingitemargument.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { RoutingItemArgumentQuery } from '@/types/logistics/manufacturing/bom/routingItemArgument.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktRoutingItemArgumentI18nSeedData 一致的实体 slug */
export const ROUTINGITEMARGUMENT_ENTITY_SLUG = 'routingitemargument'

/** entity.routingitemargument._self 静态属性（导入组件 entity-i18n-key 等） */
export const ROUTINGITEMARGUMENT_SELF_I18N_KEY = buildEntitySelfI18nKey(ROUTINGITEMARGUMENT_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const ROUTINGITEMARGUMENT_LIST_FIELDS = [
  'routingItemId',
  'paramCode',
  'paramName',
  'paramUnit',
  'standardValue',
  'lowerLimit',
  'upperLimit',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const ROUTINGITEMARGUMENT_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'routingItemId',
  'paramCode',
  'paramName',
  'paramUnit',
  'standardValue',
  'lowerLimit',
  'upperLimit',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const ROUTINGITEMARGUMENT_SUMMARY_SUM_FIELDS = [
  'standardValue',
  'lowerLimit',
  'upperLimit',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const ROUTINGITEMARGUMENT_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  paramCode: 'required',
  paramName: 'required',
  paramUnit: 'optional',
  standardValue: 'optional',
  lowerLimit: 'optional',
  upperLimit: 'optional',
  plantCode: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type RoutingItemArgumentField = keyof typeof ROUTINGITEMARGUMENT_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const ROUTINGITEMARGUMENT_QUERY_STRING_FIELDS = [
  'cultureCode',
  'paramCode',
  'paramName',
  'paramUnit',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof RoutingItemArgumentQuery)[]

export type RoutingItemArgumentQueryField =
  | (typeof ROUTINGITEMARGUMENT_QUERY_STRING_FIELDS)[number]
  | 'standardValue' | 'lowerLimit' | 'upperLimit'

/** 高级查询抽屉全部字段（含数值） */
export const ROUTINGITEMARGUMENT_QUERY_FIELDS: readonly RoutingItemArgumentQueryField[] = [
  ...ROUTINGITEMARGUMENT_QUERY_STRING_FIELDS,
  'standardValue',
  'lowerLimit',
  'upperLimit',
]

/**
 * RoutingItemArgument字段 i18n：index / routing-item-argument-form 统一入口
 */
export function useRoutingItemArgumentI18n() {
  const ef = useEntityFieldI18n(ROUTINGITEMARGUMENT_ENTITY_SLUG)

  function ph(field: RoutingItemArgumentField): string {
    return ef.placeholder(field, ROUTINGITEMARGUMENT_PLACEHOLDER[field])
  }

  function queryPh(field: RoutingItemArgumentQueryField, kind: EntityFieldPlaceholderKind): string {
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

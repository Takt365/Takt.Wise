// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/bom/routing/composables
// 文件名称：useRoutingItemI18n.ts
// 功能描述：RoutingItem字段清单 + useRoutingItemI18n（字段名映射一次，文案由 entity.routingitem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { RoutingItemQuery } from '@/types/logistics/manufacturing/bom/routingItem.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktRoutingItemI18nSeedData 一致的实体 slug */
export const ROUTINGITEM_ENTITY_SLUG = 'routingitem'

/** entity.routingitem._self 静态属性（导入组件 entity-i18n-key 等） */
export const ROUTINGITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(ROUTINGITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const ROUTINGITEM_LIST_FIELDS = [
  'routingId',
  'routingCode',
  'lineNumber',
  'baseUnit',
  'baseQuantity',
  'standardMinutes',
  'timeUnit',
  'standardShorts',
  'pointsUnit',
  'pointsToMinutesRate',
  'convertedMinutes',
  'setupMinutes',
  'teardownMinutes',
  'isInspection',
  'processDescription',
  'processSegmentType',
  'extJson',
  'isObsolete',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const ROUTINGITEM_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'routingId',
  'routingCode',
  'lineNumber',
  'baseUnit',
  'baseQuantity',
  'standardMinutes',
  'timeUnit',
  'standardShorts',
  'pointsUnit',
  'pointsToMinutesRate',
  'convertedMinutes',
  'setupMinutes',
  'teardownMinutes',
  'isInspection',
  'processDescription',
  'processSegmentType',
  'extJson',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const ROUTINGITEM_SUMMARY_SUM_FIELDS = [
  'baseQuantity',
  'standardMinutes',
  'standardShorts',
  'convertedMinutes',
  'setupMinutes',
  'teardownMinutes',
  'isInspection',
  'processSegmentType',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const ROUTINGITEM_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  lineNumber: 'select',
  baseUnit: 'select',
  baseQuantity: 'select',
  standardMinutes: 'select',
  timeUnit: 'select',
  standardShorts: 'select',
  pointsUnit: 'select',
  pointsToMinutesRate: 'select',
  convertedMinutes: 'select',
  setupMinutes: 'select',
  teardownMinutes: 'select',
  isInspection: 'select',
  processDescription: 'optional',
  processSegmentType: 'select',
  extJson: 'optional',
  isObsolete: 'select',
  arguments: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type RoutingItemField = keyof typeof ROUTINGITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const ROUTINGITEM_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'routingCode',
  'baseUnit',
  'timeUnit',
  'pointsUnit',
  'pointsToMinutesRate',
  'processDescription',
  'extJson',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof RoutingItemQuery)[]

export type RoutingItemQueryField =
  | (typeof ROUTINGITEM_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'baseQuantity' | 'standardMinutes' | 'standardShorts' | 'convertedMinutes' | 'setupMinutes' | 'teardownMinutes' | 'isInspection' | 'processSegmentType' | 'isObsolete'

/** 高级查询抽屉全部字段（含数值） */
export const ROUTINGITEM_QUERY_FIELDS: readonly RoutingItemQueryField[] = [
  ...ROUTINGITEM_QUERY_STRING_FIELDS,
  'lineNumber',
  'baseQuantity',
  'standardMinutes',
  'standardShorts',
  'convertedMinutes',
  'setupMinutes',
  'teardownMinutes',
  'isInspection',
  'processSegmentType',
  'isObsolete',
]

/**
 * RoutingItem字段 i18n：index / routing-item-form 统一入口
 */
export function useRoutingItemI18n() {
  const ef = useEntityFieldI18n(ROUTINGITEM_ENTITY_SLUG)

  function ph(field: RoutingItemField): string {
    return ef.placeholder(field, ROUTINGITEM_PLACEHOLDER[field])
  }

  function queryPh(field: RoutingItemQueryField, kind: EntityFieldPlaceholderKind): string {
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

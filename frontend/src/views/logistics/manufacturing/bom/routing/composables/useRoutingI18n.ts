// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/bom/routing/composables
// 文件名称：useRoutingI18n.ts
// 功能描述：工艺路线主表实体字段清单 + useRoutingI18n（字段名映射一次，文案由 entity.routing.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { RoutingQuery } from '@/types/logistics/manufacturing/bom/routing.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktRoutingI18nSeedData 一致的实体 slug */
export const ROUTING_ENTITY_SLUG = 'routing'

/** entity.routing._self 静态属性（导入组件 entity-i18n-key 等） */
export const ROUTING_SELF_I18N_KEY = buildEntitySelfI18nKey(ROUTING_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const ROUTING_LIST_FIELDS = [
  'workCenter',
  'routingCode',
  'routingName',
  'routingGroupCode',
  'routingGroupName',
  'purpose',
  'materialCode',
  'version',
  'routingStatus',
  'effectiveDate',
  'expiryDate',
  'routingDescription',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const ROUTING_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  workCenter: 'select',
  routingCode: 'required',
  routingName: 'required',
  routingGroupCode: 'select',
  routingGroupName: 'optional',
  purpose: 'select',
  materialCode: 'select',
  version: 'required',
  routingStatus: 'select',
  effectiveDate: 'optional',
  expiryDate: 'optional',
  routingDescription: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type RoutingField = keyof typeof ROUTING_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const ROUTING_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'workCenter',
  'routingCode',
  'routingName',
  'routingGroupCode',
  'routingGroupName',
  'materialCode',
  'version',
  'effectiveDateStart',
  'effectiveDateEnd',
  'expiryDateStart',
  'expiryDateEnd',
  'routingDescription',
  'initiatorId',
  'initiatedAtStart',
  'initiatedAtEnd',
  'approvedBy',
  'approvedAtStart',
  'approvedAtEnd',
  'flowInstanceId',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof RoutingQuery)[]

export type RoutingQueryField =
  | (typeof ROUTING_QUERY_STRING_FIELDS)[number]
  | 'purpose' | 'routingStatus' | 'approvalStatus'

/** 高级查询抽屉全部字段（含数值） */
export const ROUTING_QUERY_FIELDS: readonly RoutingQueryField[] = [
  ...ROUTING_QUERY_STRING_FIELDS,
  'purpose',
  'routingStatus',
  'approvalStatus',
]

/**
 * 工艺路线主表实体字段 i18n：index / routing-form 统一入口
 */
export function useRoutingI18n() {
  const ef = useEntityFieldI18n(ROUTING_ENTITY_SLUG)

  function ph(field: RoutingField): string {
    return ef.placeholder(field, ROUTING_PLACEHOLDER[field])
  }

  function queryPh(field: RoutingQueryField, kind: EntityFieldPlaceholderKind): string {
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

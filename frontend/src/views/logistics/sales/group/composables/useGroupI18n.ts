// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/sales/group/composables
// 文件名称：useGroupI18n.ts
// 功能描述：销售组主数据实体字段清单 + useSalesGroupI18n（字段名映射一次，文案由 entity.salesgroup.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SalesGroupQuery } from '@/types/logistics/sales/group.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSalesGroupI18nSeedData 一致的实体 slug */
export const SALESGROUP_ENTITY_SLUG = 'salesgroup'

/** entity.salesgroup._self 静态属性（导入组件 entity-i18n-key 等） */
export const SALESGROUP_SELF_I18N_KEY = buildEntitySelfI18nKey(SALESGROUP_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SALESGROUP_LIST_FIELDS = [
  'salesGroupCode',
  'salesGroupName',
  'salesGroupDescription',
  'salesGroupCategory',
  'contactPhone',
  'contactEmail',
  'isBuiltIn',
  'groupStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SALESGROUP_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  salesGroupCode: 'required',
  salesGroupName: 'required',
  salesGroupDescription: 'optional',
  salesGroupCategory: 'select',
  contactPhone: 'optional',
  contactEmail: 'optional',
  isBuiltIn: 'select',
  groupStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SalesGroupField = keyof typeof SALESGROUP_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SALESGROUP_QUERY_STRING_FIELDS = [
  'cultureCode',
  'salesGroupCode',
  'salesGroupName',
  'salesGroupDescription',
  'contactPhone',
  'contactEmail',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SalesGroupQuery)[]

export type SalesGroupQueryField =
  | (typeof SALESGROUP_QUERY_STRING_FIELDS)[number]
  | 'salesGroupCategory' | 'isBuiltIn' | 'groupStatus'

/** 高级查询抽屉全部字段（含数值） */
export const SALESGROUP_QUERY_FIELDS: readonly SalesGroupQueryField[] = [
  ...SALESGROUP_QUERY_STRING_FIELDS,
  'salesGroupCategory',
  'isBuiltIn',
  'groupStatus',
]

/**
 * 销售组主数据实体字段 i18n：index / group-form 统一入口
 */
export function useSalesGroupI18n() {
  const ef = useEntityFieldI18n(SALESGROUP_ENTITY_SLUG)

  function ph(field: SalesGroupField): string {
    return ef.placeholder(field, SALESGROUP_PLACEHOLDER[field])
  }

  function queryPh(field: SalesGroupQueryField, kind: EntityFieldPlaceholderKind): string {
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

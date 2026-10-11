// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/customer-service/group/composables
// 文件名称：useGroupI18n.ts
// 功能描述：客户服务组主数据实体字段清单 + useCustomerServiceGroupI18n（字段名映射一次，文案由 entity.customerservicegroup.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CustomerServiceGroupQuery } from '@/types/logistics/customer-service/group.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktCustomerServiceGroupI18nSeedData 一致的实体 slug */
export const CUSTOMERSERVICEGROUP_ENTITY_SLUG = 'customerservicegroup'

/** entity.customerservicegroup._self 静态属性（导入组件 entity-i18n-key 等） */
export const CUSTOMERSERVICEGROUP_SELF_I18N_KEY = buildEntitySelfI18nKey(CUSTOMERSERVICEGROUP_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const CUSTOMERSERVICEGROUP_LIST_FIELDS = [
  'customerServiceGroupCode',
  'customerServiceGroupName',
  'customerServiceGroupDescription',
  'customerServiceGroupCategory',
  'contactPhone',
  'contactEmail',
  'isBuiltIn',
  'groupStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const CUSTOMERSERVICEGROUP_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  customerServiceGroupCode: 'required',
  customerServiceGroupName: 'required',
  customerServiceGroupDescription: 'optional',
  customerServiceGroupCategory: 'select',
  contactPhone: 'optional',
  contactEmail: 'optional',
  isBuiltIn: 'select',
  groupStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type CustomerServiceGroupField = keyof typeof CUSTOMERSERVICEGROUP_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const CUSTOMERSERVICEGROUP_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'customerServiceGroupCode',
  'customerServiceGroupName',
  'customerServiceGroupDescription',
  'contactPhone',
  'contactEmail',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof CustomerServiceGroupQuery)[]

export type CustomerServiceGroupQueryField =
  | (typeof CUSTOMERSERVICEGROUP_QUERY_STRING_FIELDS)[number]
  | 'customerServiceGroupCategory' | 'isBuiltIn' | 'groupStatus'

/** 高级查询抽屉全部字段（含数值） */
export const CUSTOMERSERVICEGROUP_QUERY_FIELDS: readonly CustomerServiceGroupQueryField[] = [
  ...CUSTOMERSERVICEGROUP_QUERY_STRING_FIELDS,
  'customerServiceGroupCategory',
  'isBuiltIn',
  'groupStatus',
]

/**
 * 客户服务组主数据实体字段 i18n：index / CustomerServiceGroupForm 统一入口
 */
export function useCustomerServiceGroupI18n() {
  const ef = useEntityFieldI18n(CUSTOMERSERVICEGROUP_ENTITY_SLUG)

  function ph(field: CustomerServiceGroupField): string {
    return ef.placeholder(field, CUSTOMERSERVICEGROUP_PLACEHOLDER[field])
  }

  function queryPh(field: CustomerServiceGroupQueryField, kind: EntityFieldPlaceholderKind): string {
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

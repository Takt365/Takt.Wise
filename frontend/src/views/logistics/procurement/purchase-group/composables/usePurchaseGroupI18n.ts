// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/procurement/purchase-group/composables
// 文件名称：usePurchaseGroupI18n.ts
// 功能描述：Takt采购组主数据实体字段清单 + usePurchaseGroupI18n（字段名映射一次，文案由 entity.purchasegroup.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PurchaseGroupQuery } from '@/types/logistics/procurement/purchaseGroup.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktPurchaseGroupI18nSeedData 一致的实体 slug */
export const PURCHASEGROUP_ENTITY_SLUG = 'purchasegroup'

/** entity.purchasegroup._self 静态属性（导入组件 entity-i18n-key 等） */
export const PURCHASEGROUP_SELF_I18N_KEY = buildEntitySelfI18nKey(PURCHASEGROUP_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PURCHASEGROUP_LIST_FIELDS = [
  'plantCode',
  'purchaseGroupCode',
  'purchaseGroupName',
  'purchaseGroupDescription',
  'purchaseGroupCategory',
  'contactPhone',
  'contactEmail',
  'isBuiltIn',
  'groupStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PURCHASEGROUP_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  cultureCode: 'optional',
  plantCode: 'select',
  purchaseGroupCode: 'required',
  purchaseGroupName: 'required',
  purchaseGroupDescription: 'optional',
  purchaseGroupCategory: 'select',
  contactPhone: 'optional',
  contactEmail: 'optional',
  isBuiltIn: 'select',
  groupStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type PurchaseGroupField = keyof typeof PURCHASEGROUP_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PURCHASEGROUP_QUERY_STRING_FIELDS = [
  'purchaseGroupCode',
  'purchaseGroupName',
  'purchaseGroupDescription',
  'contactPhone',
  'contactEmail',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof PurchaseGroupQuery)[]

export type PurchaseGroupQueryField =
  | (typeof PURCHASEGROUP_QUERY_STRING_FIELDS)[number]
  | 'purchaseGroupCategory' | 'isBuiltIn' | 'groupStatus'

/** 高级查询抽屉全部字段（含数值） */
export const PURCHASEGROUP_QUERY_FIELDS: readonly PurchaseGroupQueryField[] = [
  ...PURCHASEGROUP_QUERY_STRING_FIELDS,
  'purchaseGroupCategory',
  'isBuiltIn',
  'groupStatus',
]

/**
 * Takt采购组主数据实体字段 i18n：index / purchase-group-form 统一入口
 */
export function usePurchaseGroupI18n() {
  const ef = useEntityFieldI18n(PURCHASEGROUP_ENTITY_SLUG)

  function ph(field: PurchaseGroupField): string {
    return ef.placeholder(field, PURCHASEGROUP_PLACEHOLDER[field])
  }

  function queryPh(field: PurchaseGroupQueryField, kind: EntityFieldPlaceholderKind): string {
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

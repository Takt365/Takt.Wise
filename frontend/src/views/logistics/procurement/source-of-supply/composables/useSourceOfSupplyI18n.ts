// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/procurement/source-of-supply/composables
// 文件名称：useSourceOfSupplyI18n.ts
// 功能描述：Takt货源清单实体字段清单 + useSourceOfSupplyI18n（字段名映射一次，文案由 entity.sourceofsupply.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SourceOfSupplyQuery } from '@/types/logistics/procurement/sourceOfSupply.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSourceOfSupplyI18nSeedData 一致的实体 slug */
export const SOURCEOFSUPPLY_ENTITY_SLUG = 'sourceofsupply'

/** entity.sourceofsupply._self 静态属性（导入组件 entity-i18n-key 等） */
export const SOURCEOFSUPPLY_SELF_I18N_KEY = buildEntitySelfI18nKey(SOURCEOFSUPPLY_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SOURCEOFSUPPLY_LIST_FIELDS = [
  'plantCode',
  'sourceOfSupplyCode',
  'materialCode',
  'supplierCode',
  'purchaseGroup',
  'isFixed',
  'isBlocked',
  'purchaseUnit',
  'minOrderQuantity',
  'roundingValue',
  'plannedDeliveryTimeDays',
  'agreementNumber',
  'agreementLineNumber',
  'validFrom',
  'validTo',
  'sourceStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SOURCEOFSUPPLY_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  plantCode: 'select',
  sourceOfSupplyCode: 'required',
  materialCode: 'select',
  supplierCode: 'select',
  purchaseGroup: 'optional',
  isFixed: 'select',
  isBlocked: 'select',
  purchaseUnit: 'select',
  minOrderQuantity: 'select',
  roundingValue: 'select',
  plannedDeliveryTimeDays: 'select',
  agreementNumber: 'optional',
  agreementLineNumber: 'optional',
  validFrom: 'select',
  validTo: 'select',
  sourceStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SourceOfSupplyField = keyof typeof SOURCEOFSUPPLY_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SOURCEOFSUPPLY_QUERY_STRING_FIELDS = [
  'plantCode',
  'sourceOfSupplyCode',
  'materialCode',
  'supplierCode',
  'purchaseGroup',
  'purchaseUnit',
  'agreementNumber',
  'validFromStart',
  'validFromEnd',
  'validToStart',
  'validToEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SourceOfSupplyQuery)[]

export type SourceOfSupplyQueryField =
  | (typeof SOURCEOFSUPPLY_QUERY_STRING_FIELDS)[number]
  | 'isFixed' | 'isBlocked' | 'minOrderQuantity' | 'roundingValue' | 'plannedDeliveryTimeDays' | 'agreementLineNumber' | 'sourceStatus'

/** 高级查询抽屉全部字段（含数值） */
export const SOURCEOFSUPPLY_QUERY_FIELDS: readonly SourceOfSupplyQueryField[] = [
  ...SOURCEOFSUPPLY_QUERY_STRING_FIELDS,
  'isFixed',
  'isBlocked',
  'minOrderQuantity',
  'roundingValue',
  'plannedDeliveryTimeDays',
  'agreementLineNumber',
  'sourceStatus',
]

/**
 * Takt货源清单实体字段 i18n：index / source-of-supply-form 统一入口
 */
export function useSourceOfSupplyI18n() {
  const ef = useEntityFieldI18n(SOURCEOFSUPPLY_ENTITY_SLUG)

  function ph(field: SourceOfSupplyField): string {
    return ef.placeholder(field, SOURCEOFSUPPLY_PLACEHOLDER[field])
  }

  function queryPh(field: SourceOfSupplyQueryField, kind: EntityFieldPlaceholderKind): string {
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

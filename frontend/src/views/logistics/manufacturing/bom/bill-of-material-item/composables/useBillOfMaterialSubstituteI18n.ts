// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/bom/bill-of-material-item/composables
// 文件名称：useBillOfMaterialSubstituteI18n.ts
// 功能描述：BillOfMaterialSubstitute字段清单 + useBillOfMaterialSubstituteI18n（字段名映射一次，文案由 entity.billofmaterialsubstitute.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { BillOfMaterialSubstituteQuery } from '@/types/logistics/manufacturing/bom/billOfMaterialSubstitute.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktBillOfMaterialSubstituteI18nSeedData 一致的实体 slug */
export const BILLOFMATERIALSUBSTITUTE_ENTITY_SLUG = 'billofmaterialsubstitute'

/** entity.billofmaterialsubstitute._self 静态属性（导入组件 entity-i18n-key 等） */
export const BILLOFMATERIALSUBSTITUTE_SELF_I18N_KEY = buildEntitySelfI18nKey(BILLOFMATERIALSUBSTITUTE_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const BILLOFMATERIALSUBSTITUTE_LIST_FIELDS = [
  'billOfMaterialItemId',
  'billOfMaterialId',
  'bomCode',
  'primaryMaterialCode',
  'lineNumber',
  'substituteMaterialId',
  'substituteMaterialCode',
  'substituteGroup',
  'substitutePriority',
  'usageQuantity',
  'materialUnit',
  'usageRatio',
  'isEnabled',
  'effectiveDate',
  'expiryDate',
  'isObsolete',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const BILLOFMATERIALSUBSTITUTE_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'billOfMaterialItemId',
  'billOfMaterialId',
  'bomCode',
  'primaryMaterialCode',
  'lineNumber',
  'substituteMaterialId',
  'substituteMaterialCode',
  'substituteGroup',
  'substitutePriority',
  'usageQuantity',
  'materialUnit',
  'usageRatio',
  'isEnabled',
  'effectiveDate',
  'expiryDate',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const BILLOFMATERIALSUBSTITUTE_SUMMARY_SUM_FIELDS = [
  'substitutePriority',
  'usageQuantity',
  'usageRatio',
  'isEnabled',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const BILLOFMATERIALSUBSTITUTE_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  billOfMaterialId: 'optional',
  bomCode: 'optional',
  primaryMaterialCode: 'optional',
  lineNumber: 'select',
  substituteMaterialId: 'select',
  substituteMaterialCode: 'optional',
  substituteGroup: 'optional',
  substitutePriority: 'select',
  usageQuantity: 'select',
  materialUnit: 'select',
  usageRatio: 'select',
  isEnabled: 'select',
  effectiveDate: 'optional',
  expiryDate: 'optional',
  isObsolete: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type BillOfMaterialSubstituteField = keyof typeof BILLOFMATERIALSUBSTITUTE_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const BILLOFMATERIALSUBSTITUTE_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'billOfMaterialId',
  'bomCode',
  'primaryMaterialCode',
  'substituteMaterialId',
  'substituteMaterialCode',
  'substituteGroup',
  'materialUnit',
  'effectiveDateStart',
  'effectiveDateEnd',
  'expiryDateStart',
  'expiryDateEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof BillOfMaterialSubstituteQuery)[]

export type BillOfMaterialSubstituteQueryField =
  | (typeof BILLOFMATERIALSUBSTITUTE_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'substitutePriority' | 'usageQuantity' | 'usageRatio' | 'isEnabled' | 'isObsolete'

/** 高级查询抽屉全部字段（含数值） */
export const BILLOFMATERIALSUBSTITUTE_QUERY_FIELDS: readonly BillOfMaterialSubstituteQueryField[] = [
  ...BILLOFMATERIALSUBSTITUTE_QUERY_STRING_FIELDS,
  'lineNumber',
  'substitutePriority',
  'usageQuantity',
  'usageRatio',
  'isEnabled',
  'isObsolete',
]

/**
 * BillOfMaterialSubstitute字段 i18n：index / bill-of-material-substitute-form 统一入口
 */
export function useBillOfMaterialSubstituteI18n() {
  const ef = useEntityFieldI18n(BILLOFMATERIALSUBSTITUTE_ENTITY_SLUG)

  function ph(field: BillOfMaterialSubstituteField): string {
    return ef.placeholder(field, BILLOFMATERIALSUBSTITUTE_PLACEHOLDER[field])
  }

  function queryPh(field: BillOfMaterialSubstituteQueryField, kind: EntityFieldPlaceholderKind): string {
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

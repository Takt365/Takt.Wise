// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/accounting/financial/tax-condition/composables
// 文件名称：useTaxConditionI18n.ts
// 功能描述：税务条件实体字段清单 + useTaxConditionI18n（字段名映射一次，文案由 entity.taxcondition.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaxConditionQuery } from '@/types/accounting/financial/taxCondition.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktTaxConditionI18nSeedData 一致的实体 slug */
export const TAXCONDITION_ENTITY_SLUG = 'taxcondition'

/** entity.taxcondition._self 静态属性（导入组件 entity-i18n-key 等） */
export const TAXCONDITION_SELF_I18N_KEY = buildEntitySelfI18nKey(TAXCONDITION_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const TAXCONDITION_LIST_FIELDS = [
  'applicationCode',
  'conditionTypeCode',
  'countryCode',
  'taxCode',
  'conditionRecordCode',
  'taxConditionStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const TAXCONDITION_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  applicationCode: 'select',
  conditionTypeCode: 'select',
  countryCode: 'select',
  taxCode: 'select',
  conditionRecordCode: 'required',
  taxConditionStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type TaxConditionField = keyof typeof TAXCONDITION_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const TAXCONDITION_QUERY_STRING_FIELDS = [
  'cultureCode',
  'applicationCode',
  'conditionTypeCode',
  'countryCode',
  'taxCode',
  'conditionRecordCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof TaxConditionQuery)[]

export type TaxConditionQueryField =
  | (typeof TAXCONDITION_QUERY_STRING_FIELDS)[number]
  | 'taxConditionStatus'

/** 高级查询抽屉全部字段（含数值） */
export const TAXCONDITION_QUERY_FIELDS: readonly TaxConditionQueryField[] = [
  ...TAXCONDITION_QUERY_STRING_FIELDS,
  'taxConditionStatus',
]

/**
 * 税务条件实体字段 i18n：index / TaxConditionForm 统一入口
 */
export function useTaxConditionI18n() {
  const ef = useEntityFieldI18n(TAXCONDITION_ENTITY_SLUG)

  function ph(field: TaxConditionField): string {
    return ef.placeholder(field, TAXCONDITION_PLACEHOLDER[field])
  }

  function queryPh(field: TaxConditionQueryField, kind: EntityFieldPlaceholderKind): string {
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

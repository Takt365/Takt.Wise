// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/mps/standard-operation-rate/composables
// 文件名称：useStandardOperationRateI18n.ts
// 功能描述：标准生产稼动率实体 OperationRate 为标准对标目标值字段清单 + useStandardOperationRateI18n（字段名映射一次，文案由 entity.standardoperationrate.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { StandardOperationRateQuery } from '@/types/logistics/manufacturing/mps/standardOperationRate.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktStandardOperationRateI18nSeedData 一致的实体 slug */
export const STANDARDOPERATIONRATE_ENTITY_SLUG = 'standardoperationrate'

/** entity.standardoperationrate._self 静态属性（导入组件 entity-i18n-key 等） */
export const STANDARDOPERATIONRATE_SELF_I18N_KEY = buildEntitySelfI18nKey(STANDARDOPERATIONRATE_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const STANDARDOPERATIONRATE_LIST_FIELDS = [
  'plantCode',
  'financialYear',
  'operationType',
  'operationRate',
  'effectiveDate',
  'expiryDate',
  'rateStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const STANDARDOPERATIONRATE_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  plantCode: 'select',
  financialYear: 'required',
  operationType: 'select',
  operationRate: 'select',
  effectiveDate: 'select',
  expiryDate: 'optional',
  rateStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type StandardOperationRateField = keyof typeof STANDARDOPERATIONRATE_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const STANDARDOPERATIONRATE_QUERY_STRING_FIELDS = [
  'plantCode',
  'financialYear',
  'effectiveDateStart',
  'effectiveDateEnd',
  'expiryDateStart',
  'expiryDateEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof StandardOperationRateQuery)[]

export type StandardOperationRateQueryField =
  | (typeof STANDARDOPERATIONRATE_QUERY_STRING_FIELDS)[number]
  | 'operationType' | 'operationRate' | 'rateStatus'

/** 高级查询抽屉全部字段（含数值） */
export const STANDARDOPERATIONRATE_QUERY_FIELDS: readonly StandardOperationRateQueryField[] = [
  ...STANDARDOPERATIONRATE_QUERY_STRING_FIELDS,
  'operationType',
  'operationRate',
  'rateStatus',
]

/**
 * 标准生产稼动率实体 OperationRate 为标准对标目标值字段 i18n：index / standard-operation-rate-form 统一入口
 */
export function useStandardOperationRateI18n() {
  const ef = useEntityFieldI18n(STANDARDOPERATIONRATE_ENTITY_SLUG)

  function ph(field: StandardOperationRateField): string {
    return ef.placeholder(field, STANDARDOPERATIONRATE_PLACEHOLDER[field])
  }

  function queryPh(field: StandardOperationRateQueryField, kind: EntityFieldPlaceholderKind): string {
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

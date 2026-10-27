// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/defect/assy-defect/composables
// 文件名称：useAssyDefectDetailI18n.ts
// 功能描述：AssyDefectDetail字段清单 + useAssyDefectDetailI18n（字段名映射一次，文案由 entity.assydefectdetail.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { AssyDefectDetailQuery } from '@/types/logistics/manufacturing/defect/assyDefectDetail.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktAssyDefectDetailI18nSeedData 一致的实体 slug */
export const ASSYDEFECTDETAIL_ENTITY_SLUG = 'assydefectdetail'

/** entity.assydefectdetail._self 静态属性（导入组件 entity-i18n-key 等） */
export const ASSYDEFECTDETAIL_SELF_I18N_KEY = buildEntitySelfI18nKey(ASSYDEFECTDETAIL_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const ASSYDEFECTDETAIL_LIST_FIELDS = [
  'assyDefectId',
  'prodOrderCode',
  'prodActualQty',
  'goodQuantity',
  'lineNumber',
  'defectCategory',
  'defectQty',
  'cumulativeDefectQty',
  'randomCardCode',
  'occurrenceEngineering',
  'testStep',
  'defectSymptom',
  'defectLocation',
  'defectReason',
  'repairOperator',
  'isObsolete',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const ASSYDEFECTDETAIL_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'assyDefectId',
  'prodOrderCode',
  'prodActualQty',
  'goodQuantity',
  'lineNumber',
  'defectCategory',
  'defectQty',
  'cumulativeDefectQty',
  'randomCardCode',
  'occurrenceEngineering',
  'testStep',
  'defectSymptom',
  'defectLocation',
  'defectReason',
  'repairOperator',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const ASSYDEFECTDETAIL_SUMMARY_SUM_FIELDS = [
  'prodActualQty',
  'goodQuantity',
  'defectQty',
  'cumulativeDefectQty',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const ASSYDEFECTDETAIL_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  prodOrderCode: 'optional',
  prodActualQty: 'optional',
  goodQuantity: 'optional',
  lineNumber: 'select',
  defectCategory: 'optional',
  defectQty: 'select',
  cumulativeDefectQty: 'select',
  randomCardCode: 'optional',
  occurrenceEngineering: 'optional',
  testStep: 'optional',
  defectSymptom: 'optional',
  defectLocation: 'optional',
  defectReason: 'optional',
  repairOperator: 'optional',
  isObsolete: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type AssyDefectDetailField = keyof typeof ASSYDEFECTDETAIL_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const ASSYDEFECTDETAIL_QUERY_STRING_FIELDS = [
  'cultureCode',
  'prodOrderCode',
  'defectCategory',
  'randomCardCode',
  'occurrenceEngineering',
  'testStep',
  'defectSymptom',
  'defectLocation',
  'defectReason',
  'repairOperator',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof AssyDefectDetailQuery)[]

export type AssyDefectDetailQueryField =
  | (typeof ASSYDEFECTDETAIL_QUERY_STRING_FIELDS)[number]
  | 'prodActualQty' | 'goodQuantity' | 'lineNumber' | 'defectQty' | 'cumulativeDefectQty' | 'isObsolete'

/** 高级查询抽屉全部字段（含数值） */
export const ASSYDEFECTDETAIL_QUERY_FIELDS: readonly AssyDefectDetailQueryField[] = [
  ...ASSYDEFECTDETAIL_QUERY_STRING_FIELDS,
  'prodActualQty',
  'goodQuantity',
  'lineNumber',
  'defectQty',
  'cumulativeDefectQty',
  'isObsolete',
]

/**
 * AssyDefectDetail字段 i18n：index / assy-defect-detail-form 统一入口
 */
export function useAssyDefectDetailI18n() {
  const ef = useEntityFieldI18n(ASSYDEFECTDETAIL_ENTITY_SLUG)

  function ph(field: AssyDefectDetailField): string {
    return ef.placeholder(field, ASSYDEFECTDETAIL_PLACEHOLDER[field])
  }

  function queryPh(field: AssyDefectDetailQueryField, kind: EntityFieldPlaceholderKind): string {
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

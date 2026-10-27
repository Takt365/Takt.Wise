// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/quality/operation/inspection-standard/composables
// 文件名称：useInspectionStandardItemI18n.ts
// 功能描述：InspectionStandardItem字段清单 + useInspectionStandardItemI18n（字段名映射一次，文案由 entity.inspectionstandarditem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { InspectionStandardItemQuery } from '@/types/logistics/quality/operation/inspectionStandardItem.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktInspectionStandardItemI18nSeedData 一致的实体 slug */
export const INSPECTIONSTANDARDITEM_ENTITY_SLUG = 'inspectionstandarditem'

/** entity.inspectionstandarditem._self 静态属性（导入组件 entity-i18n-key 等） */
export const INSPECTIONSTANDARDITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(INSPECTIONSTANDARDITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const INSPECTIONSTANDARDITEM_LIST_FIELDS = [
  'inspectionStandardId',
  'lineNumber',
  'itemCode',
  'itemName',
  'itemType',
  'defectLevel',
  'inspectionMode',
  'standardValue',
  'upperLimit',
  'lowerLimit',
  'inspectionTool',
  'inspectionMethodDescription',
  'acceptanceCriteria',
  'rejectionCriteria',
  'isQualifiedBasis',
  'isObsolete',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const INSPECTIONSTANDARDITEM_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'inspectionStandardId',
  'lineNumber',
  'itemCode',
  'itemName',
  'itemType',
  'defectLevel',
  'inspectionMode',
  'standardValue',
  'upperLimit',
  'lowerLimit',
  'inspectionTool',
  'inspectionMethodDescription',
  'acceptanceCriteria',
  'rejectionCriteria',
  'isQualifiedBasis',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const INSPECTIONSTANDARDITEM_SUMMARY_SUM_FIELDS = [
  'itemType',
  'inspectionMode',
  'isQualifiedBasis',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const INSPECTIONSTANDARDITEM_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  lineNumber: 'select',
  itemCode: 'required',
  itemName: 'required',
  itemType: 'select',
  defectLevel: 'select',
  inspectionMode: 'select',
  standardValue: 'required',
  upperLimit: 'required',
  lowerLimit: 'required',
  inspectionTool: 'required',
  inspectionMethodDescription: 'optional',
  acceptanceCriteria: 'required',
  rejectionCriteria: 'required',
  isQualifiedBasis: 'select',
  isObsolete: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type InspectionStandardItemField = keyof typeof INSPECTIONSTANDARDITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const INSPECTIONSTANDARDITEM_QUERY_STRING_FIELDS = [
  'itemCode',
  'itemName',
  'defectLevel',
  'standardValue',
  'upperLimit',
  'lowerLimit',
  'inspectionTool',
  'inspectionMethodDescription',
  'acceptanceCriteria',
  'rejectionCriteria',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof InspectionStandardItemQuery)[]

export type InspectionStandardItemQueryField =
  | (typeof INSPECTIONSTANDARDITEM_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'itemType' | 'inspectionMode' | 'isQualifiedBasis' | 'isObsolete'

/** 高级查询抽屉全部字段（含数值） */
export const INSPECTIONSTANDARDITEM_QUERY_FIELDS: readonly InspectionStandardItemQueryField[] = [
  ...INSPECTIONSTANDARDITEM_QUERY_STRING_FIELDS,
  'lineNumber',
  'itemType',
  'inspectionMode',
  'isQualifiedBasis',
  'isObsolete',
]

/**
 * InspectionStandardItem字段 i18n：index / inspection-standard-item-form 统一入口
 */
export function useInspectionStandardItemI18n() {
  const ef = useEntityFieldI18n(INSPECTIONSTANDARDITEM_ENTITY_SLUG)

  function ph(field: InspectionStandardItemField): string {
    return ef.placeholder(field, INSPECTIONSTANDARDITEM_PLACEHOLDER[field])
  }

  function queryPh(field: InspectionStandardItemQueryField, kind: EntityFieldPlaceholderKind): string {
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

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/engineering-change/source-ec/composables
// 文件名称：useSourceEcDetailI18n.ts
// 功能描述：SourceEcDetail字段清单 + useSourceEcDetailI18n（字段名映射一次，文案由 entity.sourceecdetail.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SourceEcDetailQuery } from '@/types/logistics/manufacturing/engineering-change/sourceEcDetail.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSourceEcDetailI18nSeedData 一致的实体 slug */
export const SOURCEECDETAIL_ENTITY_SLUG = 'sourceecdetail'

/** entity.sourceecdetail._self 静态属性（导入组件 entity-i18n-key 等） */
export const SOURCEECDETAIL_SELF_I18N_KEY = buildEntitySelfI18nKey(SOURCEECDETAIL_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SOURCEECDETAIL_LIST_FIELDS = [
  'sourceEcId',
  'sourceEcCode',
  'lineNumber',
  'sourceRootMaterialCode',
  'sourceParentMaterialCode',
  'sourceOldMaterialCode',
  'sourceOldMaterialDescription',
  'sourceOldUsageQuantity',
  'sourceOldItemPosition',
  'sourceNewMaterialCode',
  'sourceNewMaterialDescription',
  'sourceNewUsageQuantity',
  'sourceNewItemPosition',
  'sourceBomCode',
  'sourceCompatibility',
  'source2ndVendor',
  'sourceInstruction',
  'sourceOldPartDisposition',
  'sourceBomEffectiveDate',
  'isObsolete',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id） */
export const SOURCEECDETAIL_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'sourceEcId',
  'sourceEcCode',
  'lineNumber',
  'sourceRootMaterialCode',
  'sourceParentMaterialCode',
  'sourceOldMaterialCode',
  'sourceOldMaterialDescription',
  'sourceOldUsageQuantity',
  'sourceOldItemPosition',
  'sourceNewMaterialCode',
  'sourceNewMaterialDescription',
  'sourceNewUsageQuantity',
  'sourceNewItemPosition',
  'sourceBomCode',
  'sourceCompatibility',
  'source2ndVendor',
  'sourceInstruction',
  'sourceOldPartDisposition',
  'sourceBomEffectiveDate',
  'isObsolete',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const SOURCEECDETAIL_SUMMARY_SUM_FIELDS = [
  'sourceOldUsageQuantity',
  'sourceNewUsageQuantity',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SOURCEECDETAIL_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  lineNumber: 'select',
  sourceRootMaterialCode: 'required',
  sourceParentMaterialCode: 'required',
  sourceOldMaterialCode: 'optional',
  sourceOldMaterialDescription: 'optional',
  sourceOldUsageQuantity: 'optional',
  sourceOldItemPosition: 'optional',
  sourceNewMaterialCode: 'optional',
  sourceNewMaterialDescription: 'optional',
  sourceNewUsageQuantity: 'optional',
  sourceNewItemPosition: 'optional',
  sourceBomCode: 'optional',
  sourceCompatibility: 'optional',
  source2ndVendor: 'optional',
  sourceInstruction: 'optional',
  sourceOldPartDisposition: 'optional',
  sourceBomEffectiveDate: 'optional',
  isObsolete: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SourceEcDetailField = keyof typeof SOURCEECDETAIL_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SOURCEECDETAIL_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'sourceEcCode',
  'sourceRootMaterialCode',
  'sourceParentMaterialCode',
  'sourceOldMaterialCode',
  'sourceOldMaterialDescription',
  'sourceOldItemPosition',
  'sourceNewMaterialCode',
  'sourceNewMaterialDescription',
  'sourceNewItemPosition',
  'sourceBomCode',
  'sourceCompatibility',
  'source2ndVendor',
  'sourceInstruction',
  'sourceOldPartDisposition',
  'sourceBomEffectiveDateStart',
  'sourceBomEffectiveDateEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SourceEcDetailQuery)[]

export type SourceEcDetailQueryField =
  | (typeof SOURCEECDETAIL_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'sourceOldUsageQuantity' | 'sourceNewUsageQuantity' | 'isObsolete'

/** 高级查询抽屉全部字段（含数值） */
export const SOURCEECDETAIL_QUERY_FIELDS: readonly SourceEcDetailQueryField[] = [
  ...SOURCEECDETAIL_QUERY_STRING_FIELDS,
  'lineNumber',
  'sourceOldUsageQuantity',
  'sourceNewUsageQuantity',
  'isObsolete',
]

/**
 * SourceEcDetail字段 i18n：index / source-ec-detail-form 统一入口
 */
export function useSourceEcDetailI18n() {
  const ef = useEntityFieldI18n(SOURCEECDETAIL_ENTITY_SLUG)

  function ph(field: SourceEcDetailField): string {
    return ef.placeholder(field, SOURCEECDETAIL_PLACEHOLDER[field])
  }

  function queryPh(field: SourceEcDetailQueryField, kind: EntityFieldPlaceholderKind): string {
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

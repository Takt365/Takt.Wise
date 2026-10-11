// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/code/generator/gen-table/composables
// 文件名称：useGenTableColumnI18n.ts
// 功能描述：GenTableColumn字段清单 + useGenTableColumnI18n（字段名映射一次，文案由 entity.gentablecolumn.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { GenTableColumnQuery } from '@/types/code/generator/genTableColumn.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktGenTableColumnI18nSeedData 一致的实体 slug */
export const GENTABLECOLUMN_ENTITY_SLUG = 'gentablecolumn'

/** entity.gentablecolumn._self 静态属性（导入组件 entity-i18n-key 等） */
export const GENTABLECOLUMN_SELF_I18N_KEY = buildEntitySelfI18nKey(GENTABLECOLUMN_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const GENTABLECOLUMN_LIST_FIELDS = [
  'genTableId',
  'lineNumber',
  'databaseColumnName',
  'columnComment',
  'databaseDataType',
  'csharpDataType',
  'csharpColumnName',
  'length',
  'decimalDigits',
  'isPk',
  'isIncrement',
  'isRequired',
  'isCreate',
  'isUpdate',
  'isUnique',
  'isList',
  'isExport',
  'isSort',
  'isQuery',
  'queryType',
  'htmlType',
  'dictType',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const GENTABLECOLUMN_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'genTableId',
  'lineNumber',
  'databaseColumnName',
  'columnComment',
  'databaseDataType',
  'csharpDataType',
  'csharpColumnName',
  'length',
  'decimalDigits',
  'isPk',
  'isIncrement',
  'isRequired',
  'isCreate',
  'isUpdate',
  'isUnique',
  'isList',
  'isExport',
  'isSort',
  'isQuery',
  'queryType',
  'htmlType',
  'dictType',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const GENTABLECOLUMN_SUMMARY_SUM_FIELDS = [
  'length',
  'decimalDigits',
  'isPk',
  'isIncrement',
  'isRequired',
  'isCreate',
  'isUpdate',
  'isUnique',
  'isList',
  'isExport',
  'isSort',
  'isQuery',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const GENTABLECOLUMN_PLACEHOLDER = {
  tenantCode: 'optional',
  lineNumber: 'select',
  databaseColumnName: 'required',
  columnComment: 'optional',
  databaseDataType: 'select',
  csharpDataType: 'select',
  csharpColumnName: 'required',
  length: 'select',
  decimalDigits: 'select',
  isPk: 'select',
  isIncrement: 'select',
  isRequired: 'select',
  isCreate: 'select',
  isUpdate: 'select',
  isUnique: 'select',
  isList: 'select',
  isExport: 'select',
  isSort: 'select',
  isQuery: 'select',
  queryType: 'select',
  htmlType: 'select',
  dictType: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type GenTableColumnField = keyof typeof GENTABLECOLUMN_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const GENTABLECOLUMN_QUERY_STRING_FIELDS = [
  'databaseColumnName',
  'columnComment',
  'databaseDataType',
  'csharpDataType',
  'csharpColumnName',
  'queryType',
  'htmlType',
  'dictType',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof GenTableColumnQuery)[]

export type GenTableColumnQueryField =
  | (typeof GENTABLECOLUMN_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'length' | 'decimalDigits' | 'isPk' | 'isIncrement' | 'isRequired' | 'isCreate' | 'isUpdate' | 'isUnique' | 'isList' | 'isExport' | 'isSort' | 'isQuery'

/** 高级查询抽屉全部字段（含数值） */
export const GENTABLECOLUMN_QUERY_FIELDS: readonly GenTableColumnQueryField[] = [
  ...GENTABLECOLUMN_QUERY_STRING_FIELDS,
  'lineNumber',
  'length',
  'decimalDigits',
  'isPk',
  'isIncrement',
  'isRequired',
  'isCreate',
  'isUpdate',
  'isUnique',
  'isList',
  'isExport',
  'isSort',
  'isQuery',
]

/**
 * GenTableColumn字段 i18n：index / gen-table-column-form 统一入口
 */
export function useGenTableColumnI18n() {
  const ef = useEntityFieldI18n(GENTABLECOLUMN_ENTITY_SLUG)

  function ph(field: GenTableColumnField): string {
    return ef.placeholder(field, GENTABLECOLUMN_PLACEHOLDER[field])
  }

  function queryPh(field: GenTableColumnQueryField, kind: EntityFieldPlaceholderKind): string {
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

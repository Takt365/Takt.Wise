// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/foundation/dict-type/composables
// 文件名称：useDictDataI18n.ts
// 功能描述：DictData字段清单 + useDictDataI18n（字段名映射一次，文案由 entity.dictdata.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { DictDataQuery } from '@/types/foundation/dictData.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktDictDataI18nSeedData 一致的实体 slug */
export const DICTDATA_ENTITY_SLUG = 'dictdata'

/** entity.dictdata._self 静态属性（导入组件 entity-i18n-key 等） */
export const DICTDATA_SELF_I18N_KEY = buildEntitySelfI18nKey(DICTDATA_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const DICTDATA_LIST_FIELDS = [
  'cultureCode',
  'dictLabel',
  'dictValue',
  'isDefault',
  'remark',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const DICTDATA_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'cultureCode',
  'isDefault',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const DICTDATA_SUMMARY_SUM_FIELDS = [
  'isDefault',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const DICTDATA_PLACEHOLDER = {
  tenantCode: 'optional',
  cultureCode: 'select',
  dictLabel: 'required',
  dictValue: 'required',
  i18nKey: 'required',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type DictDataField = keyof typeof DICTDATA_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const DICTDATA_QUERY_STRING_FIELDS = [
  'cultureCode',
  'dictLabel',
  'dictValue',
] as const satisfies readonly (keyof DictDataQuery)[]

export type DictDataQueryField = (typeof DICTDATA_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const DICTDATA_QUERY_FIELDS: readonly DictDataQueryField[] = [...DICTDATA_QUERY_STRING_FIELDS]

/**
 * DictData字段 i18n：index / dict-data-form 统一入口
 */
export function useDictDataI18n() {
  const ef = useEntityFieldI18n(DICTDATA_ENTITY_SLUG)

  function ph(field: DictDataField): string {
    return ef.placeholder(field, DICTDATA_PLACEHOLDER[field])
  }

  function queryPh(field: DictDataQueryField, kind: EntityFieldPlaceholderKind): string {
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

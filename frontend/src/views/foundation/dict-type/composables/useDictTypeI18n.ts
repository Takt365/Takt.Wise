// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/foundation/dict-type/composables
// 文件名称：useDictTypeI18n.ts
// 功能描述：字典类型实体 用于定义系统中使用的各种字典分类字段清单 + useDictTypeI18n（字段名映射一次，文案由 entity.dicttype.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { DictTypeQuery } from '@/types/foundation/dictType.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktDictTypeI18nSeedData 一致的实体 slug */
export const DICTTYPE_ENTITY_SLUG = 'dicttype'

/** entity.dicttype._self 静态属性（导入组件 entity-i18n-key 等） */
export const DICTTYPE_SELF_I18N_KEY = buildEntitySelfI18nKey(DICTTYPE_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const DICTTYPE_LIST_FIELDS = [
  'dictTypeCode',
  'dictTypeName',
  'dataSource',
  'dictScript',
  'isBuiltIn',
  'dictStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const DICTTYPE_PLACEHOLDER = {
  tenantCode: 'optional',
  dictTypeCode: 'required',
  dictTypeName: 'required',
  dataSource: 'select',
  dictScript: 'optional',
  isBuiltIn: 'select',
  dictStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type DictTypeField = keyof typeof DICTTYPE_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const DICTTYPE_QUERY_STRING_FIELDS = [
  'dictTypeCode',
  'dictTypeName',
  'dictScript',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof DictTypeQuery)[]

export type DictTypeQueryField =
  | (typeof DICTTYPE_QUERY_STRING_FIELDS)[number]
  | 'dataSource' | 'isBuiltIn' | 'dictStatus'

/** 高级查询抽屉全部字段（含数值） */
export const DICTTYPE_QUERY_FIELDS: readonly DictTypeQueryField[] = [
  ...DICTTYPE_QUERY_STRING_FIELDS,
  'dataSource',
  'isBuiltIn',
  'dictStatus',
]

/**
 * 字典类型实体 用于定义系统中使用的各种字典分类字段 i18n：index / dict-type-form 统一入口
 */
export function useDictTypeI18n() {
  const ef = useEntityFieldI18n(DICTTYPE_ENTITY_SLUG)

  function ph(field: DictTypeField): string {
    return ef.placeholder(field, DICTTYPE_PLACEHOLDER[field])
  }

  function queryPh(field: DictTypeQueryField, kind: EntityFieldPlaceholderKind): string {
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

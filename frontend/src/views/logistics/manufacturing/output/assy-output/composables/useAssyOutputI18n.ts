// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/output/assy-output/composables
// 文件名称：useAssyOutputI18n.ts
// 功能描述：组立日报字段清单 + useAssyOutputI18n（字段名映射一次，文案由 entity.assyoutput.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { AssyOutputQuery } from '@/types/logistics/manufacturing/output/assyOutput.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktAssyOutputI18nSeedData 一致的实体 slug */
export const ASSYOUTPUT_ENTITY_SLUG = 'assyoutput'

/** entity.assyoutput._self 静态属性（导入组件 entity-i18n-key 等） */
export const ASSYOUTPUT_SELF_I18N_KEY = buildEntitySelfI18nKey(ASSYOUTPUT_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const ASSYOUTPUT_LIST_FIELDS = [
  'prodCategory',
  'prodDate',
  'teamCode',
  'directLabor',
  'indirectLabor',
  'shiftNo',
  'prodOrderType',
  'prodOrderCode',
  'modelCode',
  'materialCode',
  'batchCode',
  'prodOrderQty',
  'serialCode',
  'stdMinutes',
  'stdCapacity',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const ASSYOUTPUT_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  prodCategory: 'select',
  prodDate: 'select',
  teamCode: 'select',
  directLabor: 'select',
  indirectLabor: 'select',
  shiftNo: 'select',
  prodOrderType: 'optional',
  prodOrderCode: 'select',
  modelCode: 'optional',
  materialCode: 'optional',
  batchCode: 'optional',
  prodOrderQty: 'optional',
  serialCode: 'optional',
  stdMinutes: 'optional',
  stdCapacity: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type AssyOutputField = keyof typeof ASSYOUTPUT_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const ASSYOUTPUT_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'prodCategory',
  'prodDateStart',
  'prodDateEnd',
  'teamCode',
  'prodOrderType',
  'prodOrderCode',
  'modelCode',
  'materialCode',
  'batchCode',
  'serialCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof AssyOutputQuery)[]

export type AssyOutputQueryField =
  | (typeof ASSYOUTPUT_QUERY_STRING_FIELDS)[number]
  | 'directLabor' | 'indirectLabor' | 'shiftNo' | 'prodOrderQty' | 'stdMinutes' | 'stdCapacity'

/** 高级查询抽屉全部字段（含数值） */
export const ASSYOUTPUT_QUERY_FIELDS: readonly AssyOutputQueryField[] = [
  ...ASSYOUTPUT_QUERY_STRING_FIELDS,
  'directLabor',
  'indirectLabor',
  'shiftNo',
  'prodOrderQty',
  'stdMinutes',
  'stdCapacity',
]

/**
 * 组立日报字段 i18n：index / assy-output-form 统一入口
 */
export function useAssyOutputI18n() {
  const ef = useEntityFieldI18n(ASSYOUTPUT_ENTITY_SLUG)

  function ph(field: AssyOutputField): string {
    return ef.placeholder(field, ASSYOUTPUT_PLACEHOLDER[field])
  }

  function queryPh(field: AssyOutputQueryField, kind: EntityFieldPlaceholderKind): string {
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

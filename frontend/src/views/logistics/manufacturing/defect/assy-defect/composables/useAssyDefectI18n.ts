// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/defect/assy-defect/composables
// 文件名称：useAssyDefectI18n.ts
// 功能描述：组立不良日报实体 不良率字段清单 + useAssyDefectI18n（字段名映射一次，文案由 entity.assydefect.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { AssyDefectQuery } from '@/types/logistics/manufacturing/defect/assyDefect.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktAssyDefectI18nSeedData 一致的实体 slug */
export const ASSYDEFECT_ENTITY_SLUG = 'assydefect'

/** entity.assydefect._self 静态属性（导入组件 entity-i18n-key 等） */
export const ASSYDEFECT_SELF_I18N_KEY = buildEntitySelfI18nKey(ASSYDEFECT_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const ASSYDEFECT_LIST_FIELDS = [
  'prodCategory',
  'prodDate',
  'teamCode',
  'shiftNo',
  'prodOrderType',
  'prodOrderCode',
  'prodOrderQty',
  'modelCode',
  'batchCode',
  'materialCode',
  'prodActualQty',
  'goodQuantity',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const ASSYDEFECT_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  prodCategory: 'select',
  prodDate: 'select',
  teamCode: 'select',
  shiftNo: 'select',
  prodOrderType: 'optional',
  prodOrderCode: 'required',
  prodOrderQty: 'select',
  modelCode: 'required',
  batchCode: 'optional',
  materialCode: 'required',
  prodActualQty: 'select',
  goodQuantity: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type AssyDefectField = keyof typeof ASSYDEFECT_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const ASSYDEFECT_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'prodCategory',
  'prodDateStart',
  'prodDateEnd',
  'teamCode',
  'prodOrderType',
  'prodOrderCode',
  'modelCode',
  'batchCode',
  'materialCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof AssyDefectQuery)[]

export type AssyDefectQueryField =
  | (typeof ASSYDEFECT_QUERY_STRING_FIELDS)[number]
  | 'shiftNo' | 'prodOrderQty' | 'prodActualQty' | 'goodQuantity'

/** 高级查询抽屉全部字段（含数值） */
export const ASSYDEFECT_QUERY_FIELDS: readonly AssyDefectQueryField[] = [
  ...ASSYDEFECT_QUERY_STRING_FIELDS,
  'shiftNo',
  'prodOrderQty',
  'prodActualQty',
  'goodQuantity',
]

/**
 * 组立不良日报实体 不良率字段 i18n：index / assy-defect-form 统一入口
 */
export function useAssyDefectI18n() {
  const ef = useEntityFieldI18n(ASSYDEFECT_ENTITY_SLUG)

  function ph(field: AssyDefectField): string {
    return ef.placeholder(field, ASSYDEFECT_PLACEHOLDER[field])
  }

  function queryPh(field: AssyDefectQueryField, kind: EntityFieldPlaceholderKind): string {
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

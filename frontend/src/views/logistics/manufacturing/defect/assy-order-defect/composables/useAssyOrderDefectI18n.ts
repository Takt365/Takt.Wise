// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/defect/assy-order-defect/composables
// 文件名称：useAssyOrderDefectI18n.ts
// 功能描述：组立工单不良统计实体字段清单 + useAssyOrderDefectI18n（字段名映射一次，文案由 entity.assyorderdefect.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { AssyOrderDefectQuery } from '@/types/logistics/manufacturing/defect/assyOrderDefect.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktAssyOrderDefectI18nSeedData 一致的实体 slug */
export const ASSYORDERDEFECT_ENTITY_SLUG = 'assyorderdefect'

/** entity.assyorderdefect._self 静态属性（导入组件 entity-i18n-key 等） */
export const ASSYORDERDEFECT_SELF_I18N_KEY = buildEntitySelfI18nKey(ASSYORDERDEFECT_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const ASSYORDERDEFECT_LIST_FIELDS = [
  'plantCode',
  'prodCategory',
  'prodOrderCode',
  'prodDateGroup',
  'modelCode',
  'materialCode',
  'batchCode',
  'prodOrderQty',
  'prodActualQty',
  'goodQuantity',
  'defectQty',
  'defectRatePercent',
  'yieldRatePercent',
  'lastProdDate',
  'reportCount',
  'orderStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const ASSYORDERDEFECT_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  plantCode: 'required',
  prodCategory: 'select',
  prodOrderCode: 'select',
  prodDateGroup: 'optional',
  modelCode: 'required',
  materialCode: 'required',
  batchCode: 'optional',
  prodOrderQty: 'select',
  prodActualQty: 'select',
  goodQuantity: 'select',
  defectQty: 'select',
  defectRatePercent: 'select',
  yieldRatePercent: 'select',
  lastProdDate: 'optional',
  reportCount: 'select',
  orderStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type AssyOrderDefectField = keyof typeof ASSYORDERDEFECT_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const ASSYORDERDEFECT_QUERY_STRING_FIELDS = [
  'prodCategory',
  'prodOrderCode',
  'prodDateGroup',
  'modelCode',
  'materialCode',
  'batchCode',
  'lastProdDateStart',
  'lastProdDateEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof AssyOrderDefectQuery)[]

export type AssyOrderDefectQueryField =
  | (typeof ASSYORDERDEFECT_QUERY_STRING_FIELDS)[number]
  | 'prodOrderQty' | 'prodActualQty' | 'goodQuantity' | 'defectQty' | 'defectRatePercent' | 'yieldRatePercent' | 'reportCount' | 'orderStatus'

/** 高级查询抽屉全部字段（含数值） */
export const ASSYORDERDEFECT_QUERY_FIELDS: readonly AssyOrderDefectQueryField[] = [
  ...ASSYORDERDEFECT_QUERY_STRING_FIELDS,
  'prodOrderQty',
  'prodActualQty',
  'goodQuantity',
  'defectQty',
  'defectRatePercent',
  'yieldRatePercent',
  'reportCount',
  'orderStatus',
]

/**
 * 组立工单不良统计实体字段 i18n：index / assy-order-defect-form 统一入口
 */
export function useAssyOrderDefectI18n() {
  const ef = useEntityFieldI18n(ASSYORDERDEFECT_ENTITY_SLUG)

  function ph(field: AssyOrderDefectField): string {
    return ef.placeholder(field, ASSYORDERDEFECT_PLACEHOLDER[field])
  }

  function queryPh(field: AssyOrderDefectQueryField, kind: EntityFieldPlaceholderKind): string {
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

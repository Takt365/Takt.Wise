// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/defect/assy-batch-defect/composables
// 文件名称：useAssyBatchDefectI18n.ts
// 功能描述：组立批量不良统计实体字段清单 + useAssyBatchDefectI18n（字段名映射一次，文案由 entity.assybatchdefect.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { AssyBatchDefectQuery } from '@/types/logistics/manufacturing/defect/assyBatchDefect.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktAssyBatchDefectI18nSeedData 一致的实体 slug */
export const ASSYBATCHDEFECT_ENTITY_SLUG = 'assybatchdefect'

/** entity.assybatchdefect._self 静态属性（导入组件 entity-i18n-key 等） */
export const ASSYBATCHDEFECT_SELF_I18N_KEY = buildEntitySelfI18nKey(ASSYBATCHDEFECT_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const ASSYBATCHDEFECT_LIST_FIELDS = [
  'plantCode',
  'prodCategory',
  'batchCode',
  'prodDateGroup',
  'prodOrderGroup',
  'modelCode',
  'materialGroup',
  'batchOrderQty',
  'prodOrderQtyGroup',
  'prodActualQty',
  'goodQuantity',
  'defectQty',
  'defectRatePercent',
  'yieldRatePercent',
  'lastProdDate',
  'reportCount',
  'batchStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const ASSYBATCHDEFECT_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  plantCode: 'required',
  prodCategory: 'select',
  batchCode: 'required',
  prodDateGroup: 'optional',
  prodOrderGroup: 'optional',
  modelCode: 'required',
  materialGroup: 'optional',
  batchOrderQty: 'select',
  prodOrderQtyGroup: 'optional',
  prodActualQty: 'select',
  goodQuantity: 'select',
  defectQty: 'select',
  defectRatePercent: 'select',
  yieldRatePercent: 'select',
  lastProdDate: 'optional',
  reportCount: 'select',
  batchStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type AssyBatchDefectField = keyof typeof ASSYBATCHDEFECT_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const ASSYBATCHDEFECT_QUERY_STRING_FIELDS = [
  'plantCode',
  'prodCategory',
  'batchCode',
  'prodDateGroup',
  'prodOrderGroup',
  'modelCode',
  'materialGroup',
  'prodOrderQtyGroup',
  'lastProdDateStart',
  'lastProdDateEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof AssyBatchDefectQuery)[]

export type AssyBatchDefectQueryField =
  | (typeof ASSYBATCHDEFECT_QUERY_STRING_FIELDS)[number]
  | 'batchOrderQty' | 'prodActualQty' | 'goodQuantity' | 'defectQty' | 'defectRatePercent' | 'yieldRatePercent' | 'reportCount' | 'batchStatus'

/** 高级查询抽屉全部字段（含数值） */
export const ASSYBATCHDEFECT_QUERY_FIELDS: readonly AssyBatchDefectQueryField[] = [
  ...ASSYBATCHDEFECT_QUERY_STRING_FIELDS,
  'batchOrderQty',
  'prodActualQty',
  'goodQuantity',
  'defectQty',
  'defectRatePercent',
  'yieldRatePercent',
  'reportCount',
  'batchStatus',
]

/**
 * 组立批量不良统计实体字段 i18n：index / assy-batch-defect-form 统一入口
 */
export function useAssyBatchDefectI18n() {
  const ef = useEntityFieldI18n(ASSYBATCHDEFECT_ENTITY_SLUG)

  function ph(field: AssyBatchDefectField): string {
    return ef.placeholder(field, ASSYBATCHDEFECT_PLACEHOLDER[field])
  }

  function queryPh(field: AssyBatchDefectQueryField, kind: EntityFieldPlaceholderKind): string {
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

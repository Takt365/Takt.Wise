// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/quality/operation/fqc-order-item/composables
// 文件名称：useFqcDefectHandlingI18n.ts
// 功能描述：FqcDefectHandling字段清单 + useFqcDefectHandlingI18n（字段名映射一次，文案由 entity.fqcdefecthandling.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { FqcDefectHandlingQuery } from '@/types/logistics/quality/operation/fqcDefectHandling.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktFqcDefectHandlingI18nSeedData 一致的实体 slug */
export const FQCDEFECTHANDLING_ENTITY_SLUG = 'fqcdefecthandling'

/** entity.fqcdefecthandling._self 静态属性（导入组件 entity-i18n-key 等） */
export const FQCDEFECTHANDLING_SELF_I18N_KEY = buildEntitySelfI18nKey(FQCDEFECTHANDLING_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const FQCDEFECTHANDLING_LIST_FIELDS = [
  'fqcDefectHandlingCode',
  'fqcOrderItemId',
  'fqcOrderCode',
  'lineNumber',
  'defectType',
  'defectCode',
  'defectDescription',
  'defectQuantity',
  'handlingMethod',
  'handlingDescription',
  'responsibleDept',
  'responsibleGroupCode',
  'handlerGroupCode',
  'handlingAt',
  'verifyDeptId',
  'qualityOwnerGroupCode',
  'verifyAt',
  'verifyStatus',
  'correctiveAction',
  'defectImages',
  'attachments',
  'handlingStatus',
  'isObsolete',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const FQCDEFECTHANDLING_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'fqcDefectHandlingCode',
  'fqcOrderItemId',
  'fqcOrderCode',
  'lineNumber',
  'defectType',
  'defectCode',
  'defectDescription',
  'defectQuantity',
  'handlingMethod',
  'handlingDescription',
  'responsibleDept',
  'responsibleGroupCode',
  'handlerGroupCode',
  'handlingAt',
  'verifyDeptId',
  'qualityOwnerGroupCode',
  'verifyAt',
  'verifyStatus',
  'correctiveAction',
  'defectImages',
  'attachments',
  'handlingStatus',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const FQCDEFECTHANDLING_SUMMARY_SUM_FIELDS = [
  'defectType',
  'defectQuantity',
  'handlingMethod',
  'handlingStatus',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const FQCDEFECTHANDLING_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  fqcDefectHandlingCode: 'required',
  fqcOrderCode: 'required',
  lineNumber: 'select',
  defectType: 'select',
  defectCode: 'required',
  defectDescription: 'optional',
  defectQuantity: 'select',
  handlingMethod: 'select',
  handlingDescription: 'optional',
  responsibleDept: 'optional',
  responsibleGroupCode: 'optional',
  handlerGroupCode: 'optional',
  verifyDeptId: 'select',
  qualityOwnerGroupCode: 'select',
  handlingAt: 'optional',
  verifyAt: 'optional',
  verifyStatus: 'select',
  correctiveAction: 'optional',
  defectImages: 'optional',
  attachments: 'optional',
  handlingStatus: 'select',
  isObsolete: 'select',
  plantCode: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type FqcDefectHandlingField = keyof typeof FQCDEFECTHANDLING_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const FQCDEFECTHANDLING_QUERY_STRING_FIELDS = [
  'fqcDefectHandlingCode',
  'fqcOrderCode',
  'defectCode',
  'defectDescription',
  'handlingDescription',
  'responsibleDept',
  'responsibleGroupCode',
  'handlerGroupCode',
  'handlingAtStart',
  'handlingAtEnd',
  'verifyDeptId',
  'qualityOwnerGroupCode',
  'verifyAtStart',
  'verifyAtEnd',
  'correctiveAction',
  'defectImages',
  'attachments',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof FqcDefectHandlingQuery)[]

export type FqcDefectHandlingQueryField =
  | (typeof FQCDEFECTHANDLING_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'defectType' | 'defectQuantity' | 'handlingMethod' | 'handlingStatus' | 'verifyStatus' | 'isObsolete'

/** 高级查询抽屉全部字段（含数值） */
export const FQCDEFECTHANDLING_QUERY_FIELDS: readonly FqcDefectHandlingQueryField[] = [
  ...FQCDEFECTHANDLING_QUERY_STRING_FIELDS,
  'lineNumber',
  'defectType',
  'defectQuantity',
  'handlingMethod',
  'verifyStatus',
  'handlingStatus',
  'isObsolete',
]

/**
 * FqcDefectHandling字段 i18n：index / fqc-defect-handling-form 统一入口
 */
export function useFqcDefectHandlingI18n() {
  const ef = useEntityFieldI18n(FQCDEFECTHANDLING_ENTITY_SLUG)

  function ph(field: FqcDefectHandlingField): string {
    return ef.placeholder(field, FQCDEFECTHANDLING_PLACEHOLDER[field])
  }

  function queryPh(field: FqcDefectHandlingQueryField, kind: EntityFieldPlaceholderKind): string {
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

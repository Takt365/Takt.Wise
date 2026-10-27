// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/defect/pcba-inspection/composables
// 文件名称：usePcbaInspectionDetailI18n.ts
// 功能描述：PcbaInspectionDetail字段清单 + usePcbaInspectionDetailI18n（字段名映射一次，文案由 entity.pcbainspectiondetail.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PcbaInspectionDetailQuery } from '@/types/logistics/manufacturing/defect/pcbaInspectionDetail.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktPcbaInspectionDetailI18nSeedData 一致的实体 slug */
export const PCBAINSPECTIONDETAIL_ENTITY_SLUG = 'pcbainspectiondetail'

/** entity.pcbainspectiondetail._self 静态属性（导入组件 entity-i18n-key 等） */
export const PCBAINSPECTIONDETAIL_SELF_I18N_KEY = buildEntitySelfI18nKey(PCBAINSPECTIONDETAIL_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PCBAINSPECTIONDETAIL_LIST_FIELDS = [
  'pcbaInspectionId',
  'prodOrderCode',
  'lineNumber',
  'pcbaBoardType',
  'visualInspectionLine',
  'aoiLine',
  'bSideAssemblyDate',
  'tSideAssemblyDate',
  'shiftNo',
  'inspectorName',
  'dailyCompletedQty',
  'inspectionQty',
  'inspectionStatus',
  'TeamCode',
  'inspectionWorkHours',
  'aoiWorkHours',
  'defectQty',
  'handPlacement',
  'serialNumber',
  'content',
  'defectLocation',
  'isObsolete',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const PCBAINSPECTIONDETAIL_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'pcbaInspectionId',
  'prodOrderCode',
  'lineNumber',
  'pcbaBoardType',
  'visualInspectionLine',
  'aoiLine',
  'bSideAssemblyDate',
  'tSideAssemblyDate',
  'shiftNo',
  'inspectorName',
  'dailyCompletedQty',
  'inspectionQty',
  'inspectionStatus',
  'TeamCode',
  'inspectionWorkHours',
  'aoiWorkHours',
  'defectQty',
  'handPlacement',
  'serialNumber',
  'content',
  'defectLocation',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const PCBAINSPECTIONDETAIL_SUMMARY_SUM_FIELDS = [
  'shiftNo',
  'dailyCompletedQty',
  'inspectionQty',
  'inspectionStatus',
  'inspectionWorkHours',
  'aoiWorkHours',
  'defectQty',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PCBAINSPECTIONDETAIL_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  prodOrderCode: 'optional',
  lineNumber: 'select',
  pcbaBoardType: 'optional',
  visualInspectionLine: 'optional',
  aoiLine: 'optional',
  bSideAssemblyDate: 'optional',
  tSideAssemblyDate: 'optional',
  shiftNo: 'select',
  inspectorName: 'optional',
  dailyCompletedQty: 'select',
  inspectionQty: 'select',
  inspectionStatus: 'select',
  TeamCode: 'optional',
  inspectionWorkHours: 'select',
  aoiWorkHours: 'select',
  defectQty: 'select',
  handPlacement: 'optional',
  serialNumber: 'optional',
  content: 'optional',
  defectLocation: 'optional',
  isObsolete: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type PcbaInspectionDetailField = keyof typeof PCBAINSPECTIONDETAIL_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PCBAINSPECTIONDETAIL_QUERY_STRING_FIELDS = [
  'prodOrderCode',
  'pcbaBoardType',
  'visualInspectionLine',
  'aoiLine',
  'bSideAssemblyDateStart',
  'bSideAssemblyDateEnd',
  'tSideAssemblyDateStart',
  'tSideAssemblyDateEnd',
  'inspectorName',
  'TeamCode',
  'handPlacement',
  'serialNumber',
  'content',
  'defectLocation',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof PcbaInspectionDetailQuery)[]

export type PcbaInspectionDetailQueryField =
  | (typeof PCBAINSPECTIONDETAIL_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'shiftNo' | 'dailyCompletedQty' | 'inspectionQty' | 'inspectionStatus' | 'inspectionWorkHours' | 'aoiWorkHours' | 'defectQty' | 'isObsolete'

/** 高级查询抽屉全部字段（含数值） */
export const PCBAINSPECTIONDETAIL_QUERY_FIELDS: readonly PcbaInspectionDetailQueryField[] = [
  ...PCBAINSPECTIONDETAIL_QUERY_STRING_FIELDS,
  'lineNumber',
  'shiftNo',
  'dailyCompletedQty',
  'inspectionQty',
  'inspectionStatus',
  'inspectionWorkHours',
  'aoiWorkHours',
  'defectQty',
  'isObsolete',
]

/**
 * PcbaInspectionDetail字段 i18n：index / pcba-inspection-detail-form 统一入口
 */
export function usePcbaInspectionDetailI18n() {
  const ef = useEntityFieldI18n(PCBAINSPECTIONDETAIL_ENTITY_SLUG)

  function ph(field: PcbaInspectionDetailField): string {
    return ef.placeholder(field, PCBAINSPECTIONDETAIL_PLACEHOLDER[field])
  }

  function queryPh(field: PcbaInspectionDetailQueryField, kind: EntityFieldPlaceholderKind): string {
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

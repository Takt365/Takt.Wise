// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/output/pcba-output/composables
// 文件名称：usePcbaOutputDetailI18n.ts
// 功能描述：PcbaOutputDetail字段清单 + usePcbaOutputDetailI18n（字段名映射一次，文案由 entity.pcbaoutputdetail.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PcbaOutputDetailQuery } from '@/types/logistics/manufacturing/output/pcbaOutputDetail.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktPcbaOutputDetailI18nSeedData 一致的实体 slug */
export const PCBAOUTPUTDETAIL_ENTITY_SLUG = 'pcbaoutputdetail'

/** entity.pcbaoutputdetail._self 静态属性（导入组件 entity-i18n-key 等） */
export const PCBAOUTPUTDETAIL_SELF_I18N_KEY = buildEntitySelfI18nKey(PCBAOUTPUTDETAIL_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PCBAOUTPUTDETAIL_LIST_FIELDS = [
  'pcbaOutputId',
  'prodOrderCode',
  'lineNumber',
  'timePeriod',
  'teamCode',
  'prodEquipCode',
  'directLabor',
  'indirectLabor',
  'shiftNo',
  'stdMinutes',
  'stdLaborCapacity',
  'stdShorts',
  'stdEquipmentCapacity',
  'pcbBoardType',
  'panelSide',
  'batchQty',
  'dailyCompletedQty',
  'totalCompletedQty',
  'completedStatus',
  'serialCode',
  'defectCount',
  'downtimeMinutes',
  'downtimeReason',
  'downtimeDescription',
  'inputMinutes',
  'actualMinutes',
  'repairMinutes',
  'switchCount',
  'switchTime',
  'stopTime',
  'totalMinutes',
  'unachievedReason',
  'unachievedDescription',
  'confirmMinutes',
  'mixedProd',
  'achievementRate',
  'isObsolete',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const PCBAOUTPUTDETAIL_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'pcbaOutputId',
  'prodOrderCode',
  'lineNumber',
  'timePeriod',
  'teamCode',
  'prodEquipCode',
  'directLabor',
  'indirectLabor',
  'shiftNo',
  'stdMinutes',
  'stdLaborCapacity',
  'stdShorts',
  'stdEquipmentCapacity',
  'pcbBoardType',
  'panelSide',
  'batchQty',
  'dailyCompletedQty',
  'totalCompletedQty',
  'completedStatus',
  'serialCode',
  'defectCount',
  'downtimeMinutes',
  'downtimeReason',
  'downtimeDescription',
  'inputMinutes',
  'actualMinutes',
  'repairMinutes',
  'switchCount',
  'switchTime',
  'stopTime',
  'totalMinutes',
  'unachievedReason',
  'unachievedDescription',
  'confirmMinutes',
  'mixedProd',
  'achievementRate',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列：仅当日完成数、不良台数、报工工时 */
export const PCBAOUTPUTDETAIL_SUMMARY_SUM_FIELDS = [
  'dailyCompletedQty',
  'defectCount',
  'confirmMinutes',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PCBAOUTPUTDETAIL_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  prodOrderCode: 'optional',
  lineNumber: 'select',
  timePeriod: 'required',
  teamCode: 'select',
  prodEquipCode: 'select',
  directLabor: 'select',
  indirectLabor: 'select',
  shiftNo: 'select',
  stdMinutes: 'optional',
  stdLaborCapacity: 'optional',
  stdShorts: 'select',
  stdEquipmentCapacity: 'optional',
  pcbBoardType: 'required',
  panelSide: 'select',
  batchQty: 'select',
  dailyCompletedQty: 'select',
  totalCompletedQty: 'optional',
  completedStatus: 'optional',
  serialCode: 'required',
  defectCount: 'select',
  downtimeMinutes: 'select',
  downtimeReason: 'optional',
  downtimeDescription: 'optional',
  inputMinutes: 'optional',
  actualMinutes: 'optional',
  repairMinutes: 'select',
  switchCount: 'select',
  switchTime: 'select',
  stopTime: 'select',
  totalMinutes: 'select',
  unachievedReason: 'optional',
  unachievedDescription: 'optional',
  confirmMinutes: 'select',
  mixedProd: 'select',
  achievementRate: 'optional',
  isObsolete: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type PcbaOutputDetailField = keyof typeof PCBAOUTPUTDETAIL_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PCBAOUTPUTDETAIL_QUERY_STRING_FIELDS = [
  'cultureCode',
  'prodOrderCode',
  'timePeriod',
  'teamCode',
  'prodEquipCode',
  'pcbBoardType',
  'panelSide',
  'serialCode',
  'downtimeReason',
  'downtimeDescription',
  'unachievedReason',
  'unachievedDescription',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof PcbaOutputDetailQuery)[]

export type PcbaOutputDetailQueryField =
  | (typeof PCBAOUTPUTDETAIL_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'directLabor' | 'indirectLabor' | 'shiftNo' | 'stdMinutes' | 'stdLaborCapacity' | 'stdShorts' | 'stdEquipmentCapacity' | 'batchQty' | 'dailyCompletedQty' | 'totalCompletedQty' | 'completedStatus' | 'defectCount' | 'downtimeMinutes' | 'inputMinutes' | 'actualMinutes' | 'repairMinutes' | 'switchCount' | 'switchTime' | 'stopTime' | 'totalMinutes' | 'confirmMinutes' | 'mixedProd' | 'achievementRate' | 'isObsolete'

/** 高级查询抽屉全部字段（含数值） */
export const PCBAOUTPUTDETAIL_QUERY_FIELDS: readonly PcbaOutputDetailQueryField[] = [
  ...PCBAOUTPUTDETAIL_QUERY_STRING_FIELDS,
  'lineNumber',
  'directLabor',
  'indirectLabor',
  'shiftNo',
  'stdMinutes',
  'stdLaborCapacity',
  'stdShorts',
  'stdEquipmentCapacity',
  'batchQty',
  'dailyCompletedQty',
  'totalCompletedQty',
  'completedStatus',
  'defectCount',
  'downtimeMinutes',
  'inputMinutes',
  'actualMinutes',
  'repairMinutes',
  'switchCount',
  'switchTime',
  'stopTime',
  'totalMinutes',
  'confirmMinutes',
  'mixedProd',
  'achievementRate',
  'isObsolete',
]

/**
 * PcbaOutputDetail字段 i18n：index / pcba-output-detail-form 统一入口
 */
export function usePcbaOutputDetailI18n() {
  const ef = useEntityFieldI18n(PCBAOUTPUTDETAIL_ENTITY_SLUG)

  function ph(field: PcbaOutputDetailField): string {
    return ef.placeholder(field, PCBAOUTPUTDETAIL_PLACEHOLDER[field])
  }

  function queryPh(field: PcbaOutputDetailQueryField, kind: EntityFieldPlaceholderKind): string {
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

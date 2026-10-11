// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/mps/equipment-operation-rate/composables
// 文件名称：useEquipmentOperationRateI18n.ts
// 功能描述：机器稼动率实体字段清单 + useEquipmentOperationRateI18n（字段名映射一次，文案由 entity.equipmentoperationrate.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { EquipmentOperationRateQuery } from '@/types/logistics/manufacturing/mps/equipmentOperationRate.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktEquipmentOperationRateI18nSeedData 一致的实体 slug */
export const EQUIPMENTOPERATIONRATE_ENTITY_SLUG = 'equipmentoperationrate'

/** entity.equipmentoperationrate._self 静态属性（导入组件 entity-i18n-key 等） */
export const EQUIPMENTOPERATIONRATE_SELF_I18N_KEY = buildEntitySelfI18nKey(EQUIPMENTOPERATIONRATE_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const EQUIPMENTOPERATIONRATE_LIST_FIELDS = [
  'plantCode',
  'timeCategory',
  'startDate',
  'endDate',
  'weekNumber',
  'monthNumber',
  'EquipCode',
  'equipmentName',
  'equipmentType',
  'TeamCode',
  'shiftNo',
  'plannedRuntime',
  'actualRuntime',
  'downtime',
  'equipmentOperationRate',
  'plannedOutput',
  'actualOutput',
  'qualifiedQuantity',
  'defectiveQuantity',
  'yieldRate',
  'downtimeReasonType',
  'downtimeReason',
  'equipmentOperator',
  'equipmentMaintainer',
  'teamLeader',
  'rateStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const EQUIPMENTOPERATIONRATE_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  plantCode: 'select',
  timeCategory: 'select',
  startDate: 'select',
  endDate: 'select',
  weekNumber: 'optional',
  monthNumber: 'optional',
  EquipCode: 'select',
  equipmentName: 'required',
  equipmentType: 'select',
  TeamCode: 'optional',
  shiftNo: 'select',
  plannedRuntime: 'select',
  actualRuntime: 'select',
  downtime: 'select',
  equipmentOperationRate: 'select',
  plannedOutput: 'select',
  actualOutput: 'select',
  qualifiedQuantity: 'select',
  defectiveQuantity: 'select',
  yieldRate: 'select',
  downtimeReasonType: 'optional',
  downtimeReason: 'optional',
  equipmentOperator: 'optional',
  equipmentMaintainer: 'optional',
  teamLeader: 'optional',
  rateStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type EquipmentOperationRateField = keyof typeof EQUIPMENTOPERATIONRATE_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const EQUIPMENTOPERATIONRATE_QUERY_STRING_FIELDS = [
  'plantCode',
  'startDateStart',
  'startDateEnd',
  'endDateStart',
  'endDateEnd',
  'EquipCode',
  'equipmentName',
  'TeamCode',
  'downtimeReason',
  'equipmentOperator',
  'equipmentMaintainer',
  'teamLeader',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof EquipmentOperationRateQuery)[]

export type EquipmentOperationRateQueryField =
  | (typeof EQUIPMENTOPERATIONRATE_QUERY_STRING_FIELDS)[number]
  | 'timeCategory' | 'weekNumber' | 'monthNumber' | 'equipmentType' | 'shiftNo' | 'plannedRuntime' | 'actualRuntime' | 'downtime' | 'equipmentOperationRate' | 'plannedOutput' | 'actualOutput' | 'qualifiedQuantity' | 'defectiveQuantity' | 'yieldRate' | 'downtimeReasonType' | 'rateStatus'

/** 高级查询抽屉全部字段（含数值） */
export const EQUIPMENTOPERATIONRATE_QUERY_FIELDS: readonly EquipmentOperationRateQueryField[] = [
  ...EQUIPMENTOPERATIONRATE_QUERY_STRING_FIELDS,
  'timeCategory',
  'weekNumber',
  'monthNumber',
  'equipmentType',
  'shiftNo',
  'plannedRuntime',
  'actualRuntime',
  'downtime',
  'equipmentOperationRate',
  'plannedOutput',
  'actualOutput',
  'qualifiedQuantity',
  'defectiveQuantity',
  'yieldRate',
  'downtimeReasonType',
  'rateStatus',
]

/**
 * 机器稼动率实体字段 i18n：index / equipment-operation-rate-form 统一入口
 */
export function useEquipmentOperationRateI18n() {
  const ef = useEntityFieldI18n(EQUIPMENTOPERATIONRATE_ENTITY_SLUG)

  function ph(field: EquipmentOperationRateField): string {
    return ef.placeholder(field, EQUIPMENTOPERATIONRATE_PLACEHOLDER[field])
  }

  function queryPh(field: EquipmentOperationRateQueryField, kind: EntityFieldPlaceholderKind): string {
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

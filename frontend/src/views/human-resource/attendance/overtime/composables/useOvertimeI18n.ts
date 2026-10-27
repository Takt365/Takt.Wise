// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/human-resource/attendance/overtime/composables
// 文件名称：useOvertimeI18n.ts
// 功能描述：加班申请字段清单 + useOvertimeI18n（字段名映射一次，文案由 entity.overtime.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { OvertimeQuery } from '@/types/human-resource/attendance/overtime.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktOvertimeI18nSeedData 一致的实体 slug */
export const OVERTIME_ENTITY_SLUG = 'overtime'

/** entity.overtime._self 静态属性（导入组件 entity-i18n-key 等） */
export const OVERTIME_SELF_I18N_KEY = buildEntitySelfI18nKey(OVERTIME_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const OVERTIME_LIST_FIELDS = [
  'handlingBy',
  'handlingAt',
  'handlingComment',
  'overtimeStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const OVERTIME_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  deptName: 'optional',
  overtimeDate: 'select',
  plannedStartTime: 'select',
  plannedEndTime: 'select',
  totalEmployees: 'select',
  totalPlannedHours: 'select',
  totalActualHours: 'select',
  overtimeType: 'select',
  reason: 'optional',
  handlingBy: 'select',
  handlingAt: 'optional',
  handlingComment: 'optional',
  overtimeStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type OvertimeField = keyof typeof OVERTIME_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const OVERTIME_QUERY_STRING_FIELDS = [
  'deptId',
  'deptName',
  'overtimeDateStart',
  'overtimeDateEnd',
  'plannedStartTimeStart',
  'plannedStartTimeEnd',
  'plannedEndTimeStart',
  'plannedEndTimeEnd',
  'reason',
  'handlingBy',
  'handlingAtStart',
  'handlingAtEnd',
  'handlingComment',
  'initiatorId',
  'initiatedAtStart',
  'initiatedAtEnd',
  'approvedBy',
  'approvedAtStart',
  'approvedAtEnd',
  'flowInstanceId',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof OvertimeQuery)[]

export type OvertimeQueryField =
  | (typeof OVERTIME_QUERY_STRING_FIELDS)[number]
  | 'totalEmployees' | 'totalPlannedHours' | 'totalActualHours' | 'overtimeType' | 'overtimeStatus' | 'approvalStatus'

/** 高级查询抽屉全部字段（含数值） */
export const OVERTIME_QUERY_FIELDS: readonly OvertimeQueryField[] = [
  ...OVERTIME_QUERY_STRING_FIELDS,
  'totalEmployees',
  'totalPlannedHours',
  'totalActualHours',
  'overtimeType',
  'overtimeStatus',
  'approvalStatus',
]

/**
 * 加班申请字段 i18n：index / overtime-form 统一入口
 */
export function useOvertimeI18n() {
  const ef = useEntityFieldI18n(OVERTIME_ENTITY_SLUG)

  function ph(field: OvertimeField): string {
    return ef.placeholder(field, OVERTIME_PLACEHOLDER[field])
  }

  function queryPh(field: OvertimeQueryField, kind: EntityFieldPlaceholderKind): string {
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

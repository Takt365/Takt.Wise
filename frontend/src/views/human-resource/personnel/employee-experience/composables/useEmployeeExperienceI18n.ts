// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/human-resource/personnel/employee-experience/composables
// 文件名称：useEmployeeExperienceI18n.ts
// 功能描述：EmployeeExperience字段清单 + useEmployeeExperienceI18n（字段名映射一次，文案由 entity.employeeexperience.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { EmployeeExperienceQuery } from '@/types/human-resource/personnel/employeeExperience.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktEmployeeExperienceI18nSeedData 一致的实体 slug */
export const EMPLOYEEEXPERIENCE_ENTITY_SLUG = 'employeeexperience'

/** entity.employeeexperience._self 静态属性（导入组件 entity-i18n-key 等） */
export const EMPLOYEEEXPERIENCE_SELF_I18N_KEY = buildEntitySelfI18nKey(EMPLOYEEEXPERIENCE_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const EMPLOYEEEXPERIENCE_LIST_FIELDS = [
  'employeeId',
  'employeeCode',
  'employeeName',
  'companyName',
  'positionName',
  'jobContent',
  'startDate',
  'endDate',
  'witnessName',
  'witnessPhone',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const EMPLOYEEEXPERIENCE_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'employeeId',
  'employeeCode',
  'employeeName',
  'companyName',
  'positionName',
  'jobContent',
  'startDate',
  'endDate',
  'witnessName',
  'witnessPhone',
  'action',
] as const

/** 明细右栏 panel 合计列（无可合计数值字段） */
export const EMPLOYEEEXPERIENCE_SUMMARY_SUM_FIELDS = [] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const EMPLOYEEEXPERIENCE_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  employeeName: 'optional',
  companyName: 'required',
  positionName: 'optional',
  jobContent: 'optional',
  startDate: 'optional',
  endDate: 'optional',
  witnessName: 'optional',
  witnessPhone: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type EmployeeExperienceField = keyof typeof EMPLOYEEEXPERIENCE_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const EMPLOYEEEXPERIENCE_QUERY_STRING_FIELDS = [
  'cultureCode',
  'employeeCode',
  'employeeName',
  'companyName',
  'positionName',
  'jobContent',
  'startDateStart',
  'startDateEnd',
  'endDateStart',
  'endDateEnd',
  'witnessName',
  'witnessPhone',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof EmployeeExperienceQuery)[]

export type EmployeeExperienceQueryField = (typeof EMPLOYEEEXPERIENCE_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const EMPLOYEEEXPERIENCE_QUERY_FIELDS: readonly EmployeeExperienceQueryField[] = [...EMPLOYEEEXPERIENCE_QUERY_STRING_FIELDS]

/**
 * EmployeeExperience字段 i18n：index / employee-experience-form 统一入口
 */
export function useEmployeeExperienceI18n() {
  const ef = useEntityFieldI18n(EMPLOYEEEXPERIENCE_ENTITY_SLUG)

  function ph(field: EmployeeExperienceField): string {
    return ef.placeholder(field, EMPLOYEEEXPERIENCE_PLACEHOLDER[field])
  }

  function queryPh(field: EmployeeExperienceQueryField, kind: EntityFieldPlaceholderKind): string {
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

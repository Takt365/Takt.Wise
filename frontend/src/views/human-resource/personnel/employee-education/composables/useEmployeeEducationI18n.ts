// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/human-resource/personnel/employee-education/composables
// 文件名称：useEmployeeEducationI18n.ts
// 功能描述：EmployeeEducation字段清单 + useEmployeeEducationI18n（字段名映射一次，文案由 entity.employeeeducation.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { EmployeeEducationQuery } from '@/types/human-resource/personnel/employeeEducation.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktEmployeeEducationI18nSeedData 一致的实体 slug */
export const EMPLOYEEEDUCATION_ENTITY_SLUG = 'employeeeducation'

/** entity.employeeeducation._self 静态属性（导入组件 entity-i18n-key 等） */
export const EMPLOYEEEDUCATION_SELF_I18N_KEY = buildEntitySelfI18nKey(EMPLOYEEEDUCATION_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const EMPLOYEEEDUCATION_LIST_FIELDS = [
  'employeeId',
  'employeeCode',
  'employeeName',
  'schoolName',
  'educationLevel',
  'degreeLevel',
  'majorName',
  'certificateCode',
  'startDate',
  'endDate',
  'isHighest',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const EMPLOYEEEDUCATION_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'employeeId',
  'employeeCode',
  'employeeName',
  'schoolName',
  'educationLevel',
  'degreeLevel',
  'majorName',
  'certificateCode',
  'startDate',
  'endDate',
  'isHighest',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const EMPLOYEEEDUCATION_SUMMARY_SUM_FIELDS = [
  'educationLevel',
  'degreeLevel',
  'isHighest',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const EMPLOYEEEDUCATION_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  employeeName: 'optional',
  schoolName: 'required',
  educationLevel: 'optional',
  degreeLevel: 'optional',
  majorName: 'optional',
  certificateCode: 'optional',
  startDate: 'optional',
  endDate: 'optional',
  isHighest: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type EmployeeEducationField = keyof typeof EMPLOYEEEDUCATION_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const EMPLOYEEEDUCATION_QUERY_STRING_FIELDS = [
  'cultureCode',
  'employeeCode',
  'employeeName',
  'schoolName',
  'majorName',
  'certificateCode',
  'startDateStart',
  'startDateEnd',
  'endDateStart',
  'endDateEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof EmployeeEducationQuery)[]

export type EmployeeEducationQueryField =
  | (typeof EMPLOYEEEDUCATION_QUERY_STRING_FIELDS)[number]
  | 'educationLevel' | 'degreeLevel' | 'isHighest'

/** 高级查询抽屉全部字段（含数值） */
export const EMPLOYEEEDUCATION_QUERY_FIELDS: readonly EmployeeEducationQueryField[] = [
  ...EMPLOYEEEDUCATION_QUERY_STRING_FIELDS,
  'educationLevel',
  'degreeLevel',
  'isHighest',
]

/**
 * EmployeeEducation字段 i18n：index / employee-education-form 统一入口
 */
export function useEmployeeEducationI18n() {
  const ef = useEntityFieldI18n(EMPLOYEEEDUCATION_ENTITY_SLUG)

  function ph(field: EmployeeEducationField): string {
    return ef.placeholder(field, EMPLOYEEEDUCATION_PLACEHOLDER[field])
  }

  function queryPh(field: EmployeeEducationQueryField, kind: EntityFieldPlaceholderKind): string {
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

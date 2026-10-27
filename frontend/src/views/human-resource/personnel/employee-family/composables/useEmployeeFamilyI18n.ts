// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/human-resource/personnel/employee-family/composables
// 文件名称：useEmployeeFamilyI18n.ts
// 功能描述：EmployeeFamily字段清单 + useEmployeeFamilyI18n（字段名映射一次，文案由 entity.employeefamily.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { EmployeeFamilyQuery } from '@/types/human-resource/personnel/employeeFamily.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktEmployeeFamilyI18nSeedData 一致的实体 slug */
export const EMPLOYEEFAMILY_ENTITY_SLUG = 'employeefamily'

/** entity.employeefamily._self 静态属性（导入组件 entity-i18n-key 等） */
export const EMPLOYEEFAMILY_SELF_I18N_KEY = buildEntitySelfI18nKey(EMPLOYEEFAMILY_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const EMPLOYEEFAMILY_LIST_FIELDS = [
  'employeeId',
  'employeeCode',
  'employeeName',
  'memberName',
  'relationType',
  'phoneNumber',
  'workUnit',
  'jobTitle',
  'birthDate',
  'isEmergencyContact',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const EMPLOYEEFAMILY_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'employeeId',
  'employeeCode',
  'employeeName',
  'memberName',
  'relationType',
  'phoneNumber',
  'workUnit',
  'jobTitle',
  'birthDate',
  'isEmergencyContact',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const EMPLOYEEFAMILY_SUMMARY_SUM_FIELDS = [
  'relationType',
  'isEmergencyContact',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const EMPLOYEEFAMILY_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  employeeName: 'optional',
  memberName: 'required',
  relationType: 'select',
  phoneNumber: 'optional',
  workUnit: 'optional',
  jobTitle: 'optional',
  birthDate: 'optional',
  isEmergencyContact: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type EmployeeFamilyField = keyof typeof EMPLOYEEFAMILY_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const EMPLOYEEFAMILY_QUERY_STRING_FIELDS = [
  'cultureCode',
  'employeeCode',
  'employeeName',
  'memberName',
  'phoneNumber',
  'workUnit',
  'jobTitle',
  'birthDateStart',
  'birthDateEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof EmployeeFamilyQuery)[]

export type EmployeeFamilyQueryField =
  | (typeof EMPLOYEEFAMILY_QUERY_STRING_FIELDS)[number]
  | 'relationType' | 'isEmergencyContact'

/** 高级查询抽屉全部字段（含数值） */
export const EMPLOYEEFAMILY_QUERY_FIELDS: readonly EmployeeFamilyQueryField[] = [
  ...EMPLOYEEFAMILY_QUERY_STRING_FIELDS,
  'relationType',
  'isEmergencyContact',
]

/**
 * EmployeeFamily字段 i18n：index / employee-family-form 统一入口
 */
export function useEmployeeFamilyI18n() {
  const ef = useEntityFieldI18n(EMPLOYEEFAMILY_ENTITY_SLUG)

  function ph(field: EmployeeFamilyField): string {
    return ef.placeholder(field, EMPLOYEEFAMILY_PLACEHOLDER[field])
  }

  function queryPh(field: EmployeeFamilyQueryField, kind: EntityFieldPlaceholderKind): string {
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

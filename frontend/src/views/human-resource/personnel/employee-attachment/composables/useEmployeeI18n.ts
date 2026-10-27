// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/human-resource/personnel/employee-attachment/composables
// 文件名称：useEmployeeI18n.ts
// 功能描述：员工实体字段清单 + useEmployeeI18n（字段名映射一次，文案由 entity.employee.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { EmployeeQuery } from '@/types/human-resource/personnel/employee.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktEmployeeI18nSeedData 一致的实体 slug */
export const EMPLOYEE_ENTITY_SLUG = 'employee'

/** entity.employee._self 静态属性（导入组件 entity-i18n-key 等） */
export const EMPLOYEE_SELF_I18N_KEY = buildEntitySelfI18nKey(EMPLOYEE_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const EMPLOYEE_LIST_FIELDS = [
  'employeeCode',
  'employeeName',
  'gender',
  'birthDate',
  'idCardCode',
  'mobile',
  'email',
  'nativePlace',
  'ethnicity',
  'politicalAffiliation',
  'maritalStatus',
  'employeeStatus',
  'isBuiltIn',
  'avatar',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const EMPLOYEE_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  employeeCode: 'required',
  employeeName: 'required',
  gender: 'select',
  birthDate: 'select',
  idCardCode: 'required',
  mobile: 'required',
  email: 'optional',
  nativePlace: 'select',
  ethnicity: 'select',
  politicalAffiliation: 'select',
  maritalStatus: 'select',
  employeeStatus: 'select',
  isBuiltIn: 'select',
  avatar: 'optional',
  employeeDeptIds: 'optional',
  employeePostIds: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type EmployeeField = keyof typeof EMPLOYEE_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const EMPLOYEE_QUERY_STRING_FIELDS = [
  'cultureCode',
  'employeeCode',
  'employeeName',
  'birthDateStart',
  'birthDateEnd',
  'idCardCode',
  'mobile',
  'email',
  'nativePlace',
  'avatar',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof EmployeeQuery)[]

export type EmployeeQueryField =
  | (typeof EMPLOYEE_QUERY_STRING_FIELDS)[number]
  | 'gender' | 'ethnicity' | 'politicalAffiliation' | 'maritalStatus' | 'employeeStatus' | 'isBuiltIn'

/** 高级查询抽屉全部字段（含数值） */
export const EMPLOYEE_QUERY_FIELDS: readonly EmployeeQueryField[] = [
  ...EMPLOYEE_QUERY_STRING_FIELDS,
  'gender',
  'ethnicity',
  'politicalAffiliation',
  'maritalStatus',
  'employeeStatus',
  'isBuiltIn',
]

/**
 * 员工实体字段 i18n：index / employee-form 统一入口
 */
export function useEmployeeI18n() {
  const ef = useEntityFieldI18n(EMPLOYEE_ENTITY_SLUG)

  function ph(field: EmployeeField): string {
    return ef.placeholder(field, EMPLOYEE_PLACEHOLDER[field])
  }

  function queryPh(field: EmployeeQueryField, kind: EntityFieldPlaceholderKind): string {
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

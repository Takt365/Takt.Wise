// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/human-resource/personnel/employee-joined/composables
// 文件名称：useEmployeeJoinedI18n.ts
// 功能描述：EmployeeJoined字段清单 + useEmployeeJoinedI18n（字段名映射一次，文案由 entity.employeejoined.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { EmployeeJoinedQuery } from '@/types/human-resource/personnel/employeeJoined.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktEmployeeJoinedI18nSeedData 一致的实体 slug */
export const EMPLOYEEJOINED_ENTITY_SLUG = 'employeejoined'

/** entity.employeejoined._self 静态属性（导入组件 entity-i18n-key 等） */
export const EMPLOYEEJOINED_SELF_I18N_KEY = buildEntitySelfI18nKey(EMPLOYEEJOINED_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const EMPLOYEEJOINED_LIST_FIELDS = [
  'employeeId',
  'employeeCode',
  'employeeName',
  'onboardingId',
  'onboardingName',
  'joinedDate',
  'probationEndDate',
  'regularDate',
  'deptId',
  'deptName',
  'postId',
  'postName',
  'jobTitle',
  'workNature',
  'employmentType',
  'directManagerId',
  'directManagerName',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const EMPLOYEEJOINED_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'employeeId',
  'employeeCode',
  'employeeName',
  'onboardingId',
  'onboardingName',
  'joinedDate',
  'probationEndDate',
  'regularDate',
  'deptId',
  'deptName',
  'postId',
  'postName',
  'jobTitle',
  'workNature',
  'employmentType',
  'directManagerId',
  'directManagerName',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const EMPLOYEEJOINED_SUMMARY_SUM_FIELDS = [
  'workNature',
  'employmentType',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const EMPLOYEEJOINED_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  employeeName: 'optional',
  onboardingId: 'optional',
  joinedDate: 'select',
  probationEndDate: 'optional',
  regularDate: 'optional',
  deptId: 'select',
  deptName: 'required',
  postId: 'optional',
  postName: 'optional',
  jobTitle: 'optional',
  workNature: 'select',
  employmentType: 'select',
  directManagerId: 'optional',
  directManagerName: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type EmployeeJoinedField = keyof typeof EMPLOYEEJOINED_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const EMPLOYEEJOINED_QUERY_STRING_FIELDS = [
  'cultureCode',
  'employeeCode',
  'employeeName',
  'onboardingId',
  'joinedDateStart',
  'joinedDateEnd',
  'probationEndDateStart',
  'probationEndDateEnd',
  'regularDateStart',
  'regularDateEnd',
  'deptId',
  'deptName',
  'postId',
  'postName',
  'jobTitle',
  'directManagerId',
  'directManagerName',
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
] as const satisfies readonly (keyof EmployeeJoinedQuery)[]

export type EmployeeJoinedQueryField =
  | (typeof EMPLOYEEJOINED_QUERY_STRING_FIELDS)[number]
  | 'workNature' | 'employmentType' | 'approvalStatus'

/** 高级查询抽屉全部字段（含数值） */
export const EMPLOYEEJOINED_QUERY_FIELDS: readonly EmployeeJoinedQueryField[] = [
  ...EMPLOYEEJOINED_QUERY_STRING_FIELDS,
  'workNature',
  'employmentType',
  'approvalStatus',
]

/**
 * EmployeeJoined字段 i18n：index / employee-joined-form 统一入口
 */
export function useEmployeeJoinedI18n() {
  const ef = useEntityFieldI18n(EMPLOYEEJOINED_ENTITY_SLUG)

  function ph(field: EmployeeJoinedField): string {
    return ef.placeholder(field, EMPLOYEEJOINED_PLACEHOLDER[field])
  }

  function queryPh(field: EmployeeJoinedQueryField, kind: EntityFieldPlaceholderKind): string {
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

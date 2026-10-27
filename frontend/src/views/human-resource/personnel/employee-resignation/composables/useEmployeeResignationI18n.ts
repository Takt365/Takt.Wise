// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/human-resource/personnel/employee-resignation/composables
// 文件名称：useEmployeeResignationI18n.ts
// 功能描述：EmployeeResignation字段清单 + useEmployeeResignationI18n（字段名映射一次，文案由 entity.employeeresignation.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { EmployeeResignationQuery } from '@/types/human-resource/personnel/employeeResignation.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktEmployeeResignationI18nSeedData 一致的实体 slug */
export const EMPLOYEERESIGNATION_ENTITY_SLUG = 'employeeresignation'

/** entity.employeeresignation._self 静态属性（导入组件 entity-i18n-key 等） */
export const EMPLOYEERESIGNATION_SELF_I18N_KEY = buildEntitySelfI18nKey(EMPLOYEERESIGNATION_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const EMPLOYEERESIGNATION_LIST_FIELDS = [
  'employeeId',
  'employeeCode',
  'employeeName',
  'resignationType',
  'applyDate',
  'lastWorkDate',
  'terminationDate',
  'reason',
  'handoverNotes',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const EMPLOYEERESIGNATION_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'employeeId',
  'employeeCode',
  'employeeName',
  'resignationType',
  'applyDate',
  'lastWorkDate',
  'terminationDate',
  'reason',
  'handoverNotes',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const EMPLOYEERESIGNATION_SUMMARY_SUM_FIELDS = [
  'resignationType',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const EMPLOYEERESIGNATION_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  employeeName: 'optional',
  resignationType: 'select',
  applyDate: 'optional',
  lastWorkDate: 'optional',
  terminationDate: 'optional',
  reason: 'optional',
  handoverNotes: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type EmployeeResignationField = keyof typeof EMPLOYEERESIGNATION_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const EMPLOYEERESIGNATION_QUERY_STRING_FIELDS = [
  'cultureCode',
  'employeeCode',
  'employeeName',
  'applyDateStart',
  'applyDateEnd',
  'lastWorkDateStart',
  'lastWorkDateEnd',
  'terminationDateStart',
  'terminationDateEnd',
  'reason',
  'handoverNotes',
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
] as const satisfies readonly (keyof EmployeeResignationQuery)[]

export type EmployeeResignationQueryField =
  | (typeof EMPLOYEERESIGNATION_QUERY_STRING_FIELDS)[number]
  | 'resignationType' | 'approvalStatus'

/** 高级查询抽屉全部字段（含数值） */
export const EMPLOYEERESIGNATION_QUERY_FIELDS: readonly EmployeeResignationQueryField[] = [
  ...EMPLOYEERESIGNATION_QUERY_STRING_FIELDS,
  'resignationType',
  'approvalStatus',
]

/**
 * EmployeeResignation字段 i18n：index / employee-resignation-form 统一入口
 */
export function useEmployeeResignationI18n() {
  const ef = useEntityFieldI18n(EMPLOYEERESIGNATION_ENTITY_SLUG)

  function ph(field: EmployeeResignationField): string {
    return ef.placeholder(field, EMPLOYEERESIGNATION_PLACEHOLDER[field])
  }

  function queryPh(field: EmployeeResignationQueryField, kind: EntityFieldPlaceholderKind): string {
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

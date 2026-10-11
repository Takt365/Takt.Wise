// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/human-resource/personnel/employee-delegation/composables
// 文件名称：useEmployeeDelegationI18n.ts
// 功能描述：EmployeeDelegation字段清单 + useEmployeeDelegationI18n（字段名映射一次，文案由 entity.employeedelegation.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { EmployeeDelegationQuery } from '@/types/human-resource/personnel/employeeDelegation.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktEmployeeDelegationI18nSeedData 一致的实体 slug */
export const EMPLOYEEDELEGATION_ENTITY_SLUG = 'employeedelegation'

/** entity.employeedelegation._self 静态属性（导入组件 entity-i18n-key 等） */
export const EMPLOYEEDELEGATION_SELF_I18N_KEY = buildEntitySelfI18nKey(EMPLOYEEDELEGATION_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const EMPLOYEEDELEGATION_LIST_FIELDS = [
  'proxyEmployeeId',
  'proxyEmployeeCode',
  'proxyEmployeeName',
  'originalEmployeeId',
  'originalEmployeeCode',
  'originalEmployeeName',
  'delegationType',
  'scopeType',
  'scopeId',
  'reason',
  'startDate',
  'endDate',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const EMPLOYEEDELEGATION_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'proxyEmployeeId',
  'proxyEmployeeCode',
  'proxyEmployeeName',
  'originalEmployeeId',
  'originalEmployeeCode',
  'originalEmployeeName',
  'delegationType',
  'scopeType',
  'scopeId',
  'reason',
  'startDate',
  'endDate',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const EMPLOYEEDELEGATION_SUMMARY_SUM_FIELDS = [
  'delegationType',
  'scopeType',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const EMPLOYEEDELEGATION_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  proxyEmployeeId: 'select',
  proxyEmployeeCode: 'optional',
  proxyEmployeeName: 'optional',
  originalEmployeeCode: 'optional',
  originalEmployeeName: 'optional',
  delegationType: 'select',
  scopeType: 'select',
  scopeId: 'optional',
  reason: 'required',
  startDate: 'select',
  endDate: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type EmployeeDelegationField = keyof typeof EMPLOYEEDELEGATION_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const EMPLOYEEDELEGATION_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'proxyEmployeeId',
  'proxyEmployeeCode',
  'proxyEmployeeName',
  'originalEmployeeCode',
  'originalEmployeeName',
  'scopeId',
  'reason',
  'startDateStart',
  'startDateEnd',
  'endDateStart',
  'endDateEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof EmployeeDelegationQuery)[]

export type EmployeeDelegationQueryField =
  | (typeof EMPLOYEEDELEGATION_QUERY_STRING_FIELDS)[number]
  | 'delegationType' | 'scopeType'

/** 高级查询抽屉全部字段（含数值） */
export const EMPLOYEEDELEGATION_QUERY_FIELDS: readonly EmployeeDelegationQueryField[] = [
  ...EMPLOYEEDELEGATION_QUERY_STRING_FIELDS,
  'delegationType',
  'scopeType',
]

/**
 * EmployeeDelegation字段 i18n：index / employee-delegation-form 统一入口
 */
export function useEmployeeDelegationI18n() {
  const ef = useEntityFieldI18n(EMPLOYEEDELEGATION_ENTITY_SLUG)

  function ph(field: EmployeeDelegationField): string {
    return ef.placeholder(field, EMPLOYEEDELEGATION_PLACEHOLDER[field])
  }

  function queryPh(field: EmployeeDelegationQueryField, kind: EntityFieldPlaceholderKind): string {
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

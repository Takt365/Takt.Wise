// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/human-resource/personnel/employee-contract/composables
// 文件名称：useEmployeeContractI18n.ts
// 功能描述：EmployeeContract字段清单 + useEmployeeContractI18n（字段名映射一次，文案由 entity.employeecontract.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { EmployeeContractQuery } from '@/types/human-resource/personnel/employeeContract.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktEmployeeContractI18nSeedData 一致的实体 slug */
export const EMPLOYEECONTRACT_ENTITY_SLUG = 'employeecontract'

/** entity.employeecontract._self 静态属性（导入组件 entity-i18n-key 等） */
export const EMPLOYEECONTRACT_SELF_I18N_KEY = buildEntitySelfI18nKey(EMPLOYEECONTRACT_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const EMPLOYEECONTRACT_LIST_FIELDS = [
  'employeeId',
  'employeeCode',
  'employeeName',
  'contractCode',
  'contractType',
  'startDate',
  'endDate',
  'probationEndDate',
  'signDate',
  'signCompany',
  'contractStatus',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const EMPLOYEECONTRACT_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'employeeId',
  'employeeCode',
  'employeeName',
  'contractCode',
  'contractType',
  'startDate',
  'endDate',
  'probationEndDate',
  'signDate',
  'signCompany',
  'contractStatus',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const EMPLOYEECONTRACT_SUMMARY_SUM_FIELDS = [
  'contractType',
  'contractStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const EMPLOYEECONTRACT_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  employeeName: 'optional',
  contractCode: 'required',
  contractType: 'select',
  startDate: 'select',
  endDate: 'optional',
  probationEndDate: 'optional',
  signDate: 'optional',
  signCompany: 'optional',
  contractStatus: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type EmployeeContractField = keyof typeof EMPLOYEECONTRACT_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const EMPLOYEECONTRACT_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'employeeCode',
  'employeeName',
  'contractCode',
  'startDateStart',
  'startDateEnd',
  'endDateStart',
  'endDateEnd',
  'probationEndDateStart',
  'probationEndDateEnd',
  'signDateStart',
  'signDateEnd',
  'signCompany',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof EmployeeContractQuery)[]

export type EmployeeContractQueryField =
  | (typeof EMPLOYEECONTRACT_QUERY_STRING_FIELDS)[number]
  | 'contractType' | 'contractStatus'

/** 高级查询抽屉全部字段（含数值） */
export const EMPLOYEECONTRACT_QUERY_FIELDS: readonly EmployeeContractQueryField[] = [
  ...EMPLOYEECONTRACT_QUERY_STRING_FIELDS,
  'contractType',
  'contractStatus',
]

/**
 * EmployeeContract字段 i18n：index / employee-contract-form 统一入口
 */
export function useEmployeeContractI18n() {
  const ef = useEntityFieldI18n(EMPLOYEECONTRACT_ENTITY_SLUG)

  function ph(field: EmployeeContractField): string {
    return ef.placeholder(field, EMPLOYEECONTRACT_PLACEHOLDER[field])
  }

  function queryPh(field: EmployeeContractQueryField, kind: EntityFieldPlaceholderKind): string {
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

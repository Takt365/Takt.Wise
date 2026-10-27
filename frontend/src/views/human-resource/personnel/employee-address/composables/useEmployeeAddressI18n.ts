// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/human-resource/personnel/employee-address/composables
// 文件名称：useEmployeeAddressI18n.ts
// 功能描述：EmployeeAddress字段清单 + useEmployeeAddressI18n（字段名映射一次，文案由 entity.employeeaddress.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { EmployeeAddressQuery } from '@/types/human-resource/personnel/employeeAddress.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktEmployeeAddressI18nSeedData 一致的实体 slug */
export const EMPLOYEEADDRESS_ENTITY_SLUG = 'employeeaddress'

/** entity.employeeaddress._self 静态属性（导入组件 entity-i18n-key 等） */
export const EMPLOYEEADDRESS_SELF_I18N_KEY = buildEntitySelfI18nKey(EMPLOYEEADDRESS_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const EMPLOYEEADDRESS_LIST_FIELDS = [
  'employeeId',
  'employeeCode',
  'employeeName',
  'addressType',
  'country',
  'province',
  'city',
  'district',
  'address1',
  'address2',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const EMPLOYEEADDRESS_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'employeeId',
  'employeeCode',
  'employeeName',
  'addressType',
  'country',
  'province',
  'city',
  'district',
  'address1',
  'address2',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const EMPLOYEEADDRESS_SUMMARY_SUM_FIELDS = [
  'addressType',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const EMPLOYEEADDRESS_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  employeeName: 'optional',
  addressType: 'select',
  country: 'select',
  province: 'select',
  city: 'select',
  district: 'select',
  address1: 'optional',
  address2: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type EmployeeAddressField = keyof typeof EMPLOYEEADDRESS_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const EMPLOYEEADDRESS_QUERY_STRING_FIELDS = [
  'cultureCode',
  'employeeCode',
  'employeeName',
  'country',
  'province',
  'city',
  'district',
  'address1',
  'address2',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof EmployeeAddressQuery)[]

export type EmployeeAddressQueryField =
  | (typeof EMPLOYEEADDRESS_QUERY_STRING_FIELDS)[number]
  | 'addressType'

/** 高级查询抽屉全部字段（含数值） */
export const EMPLOYEEADDRESS_QUERY_FIELDS: readonly EmployeeAddressQueryField[] = [
  ...EMPLOYEEADDRESS_QUERY_STRING_FIELDS,
  'addressType',
]

/**
 * EmployeeAddress字段 i18n：index / employee-address-form 统一入口
 */
export function useEmployeeAddressI18n() {
  const ef = useEntityFieldI18n(EMPLOYEEADDRESS_ENTITY_SLUG)

  function ph(field: EmployeeAddressField): string {
    return ef.placeholder(field, EMPLOYEEADDRESS_PLACEHOLDER[field])
  }

  function queryPh(field: EmployeeAddressQueryField, kind: EntityFieldPlaceholderKind): string {
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

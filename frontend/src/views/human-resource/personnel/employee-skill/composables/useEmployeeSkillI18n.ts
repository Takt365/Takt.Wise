// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/human-resource/personnel/employee-skill/composables
// 文件名称：useEmployeeSkillI18n.ts
// 功能描述：EmployeeSkill字段清单 + useEmployeeSkillI18n（字段名映射一次，文案由 entity.employeeskill.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { EmployeeSkillQuery } from '@/types/human-resource/personnel/employeeSkill.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktEmployeeSkillI18nSeedData 一致的实体 slug */
export const EMPLOYEESKILL_ENTITY_SLUG = 'employeeskill'

/** entity.employeeskill._self 静态属性（导入组件 entity-i18n-key 等） */
export const EMPLOYEESKILL_SELF_I18N_KEY = buildEntitySelfI18nKey(EMPLOYEESKILL_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const EMPLOYEESKILL_LIST_FIELDS = [
  'employeeId',
  'employeeCode',
  'employeeName',
  'skillName',
  'skillLevel',
  'certificateName',
  'certificateCode',
  'obtainedDate',
  'expiryDate',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const EMPLOYEESKILL_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'employeeId',
  'employeeCode',
  'employeeName',
  'skillName',
  'skillLevel',
  'certificateName',
  'certificateCode',
  'obtainedDate',
  'expiryDate',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const EMPLOYEESKILL_SUMMARY_SUM_FIELDS = [
  'skillLevel',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const EMPLOYEESKILL_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  employeeName: 'optional',
  skillName: 'required',
  skillLevel: 'select',
  certificateName: 'optional',
  certificateCode: 'optional',
  obtainedDate: 'optional',
  expiryDate: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type EmployeeSkillField = keyof typeof EMPLOYEESKILL_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const EMPLOYEESKILL_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'employeeCode',
  'employeeName',
  'skillName',
  'certificateName',
  'certificateCode',
  'obtainedDateStart',
  'obtainedDateEnd',
  'expiryDateStart',
  'expiryDateEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof EmployeeSkillQuery)[]

export type EmployeeSkillQueryField =
  | (typeof EMPLOYEESKILL_QUERY_STRING_FIELDS)[number]
  | 'skillLevel'

/** 高级查询抽屉全部字段（含数值） */
export const EMPLOYEESKILL_QUERY_FIELDS: readonly EmployeeSkillQueryField[] = [
  ...EMPLOYEESKILL_QUERY_STRING_FIELDS,
  'skillLevel',
]

/**
 * EmployeeSkill字段 i18n：index / employee-skill-form 统一入口
 */
export function useEmployeeSkillI18n() {
  const ef = useEntityFieldI18n(EMPLOYEESKILL_ENTITY_SLUG)

  function ph(field: EmployeeSkillField): string {
    return ef.placeholder(field, EMPLOYEESKILL_PLACEHOLDER[field])
  }

  function queryPh(field: EmployeeSkillQueryField, kind: EntityFieldPlaceholderKind): string {
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

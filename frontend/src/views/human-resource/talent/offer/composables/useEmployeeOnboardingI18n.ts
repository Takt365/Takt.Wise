// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/human-resource/talent/offer/composables
// 文件名称：useEmployeeOnboardingI18n.ts
// 功能描述：EmployeeOnboarding字段清单 + useEmployeeOnboardingI18n（字段名映射一次，文案由 entity.employeeonboarding.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { EmployeeOnboardingQuery } from '@/types/human-resource/talent/employee-onboarding'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktEmployeeOnboardingI18nSeedData 一致的实体 slug */
export const EMPLOYEEONBOARDING_ENTITY_SLUG = 'employeeonboarding'

/** entity.employeeonboarding._self 静态属性（导入组件 entity-i18n-key 等） */
export const EMPLOYEEONBOARDING_SELF_I18N_KEY = buildEntitySelfI18nKey(EMPLOYEEONBOARDING_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const EMPLOYEEONBOARDING_LIST_FIELDS = [

] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const EMPLOYEEONBOARDING_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'action',
] as const

/** 明细右栏 panel 合计列（无可合计数值字段） */
export const EMPLOYEEONBOARDING_SUMMARY_SUM_FIELDS = [] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const EMPLOYEEONBOARDING_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type EmployeeOnboardingField = keyof typeof EMPLOYEEONBOARDING_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const EMPLOYEEONBOARDING_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof EmployeeOnboardingQuery)[]

export type EmployeeOnboardingQueryField = (typeof EMPLOYEEONBOARDING_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const EMPLOYEEONBOARDING_QUERY_FIELDS: readonly EmployeeOnboardingQueryField[] = [...EMPLOYEEONBOARDING_QUERY_STRING_FIELDS]

/**
 * EmployeeOnboarding字段 i18n：index / employee-onboarding-form 统一入口
 */
export function useEmployeeOnboardingI18n() {
  const ef = useEntityFieldI18n(EMPLOYEEONBOARDING_ENTITY_SLUG)

  function ph(field: EmployeeOnboardingField): string {
    return ef.placeholder(field, EMPLOYEEONBOARDING_PLACEHOLDER[field])
  }

  function queryPh(field: EmployeeOnboardingQueryField, kind: EntityFieldPlaceholderKind): string {
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

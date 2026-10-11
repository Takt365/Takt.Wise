// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/quality/complaint/customer-satisfaction-survey/composables
// 文件名称：useCustomerSatisfactionSurveyI18n.ts
// 功能描述：客户满意度调查表主表实体字段清单 + useCustomerSatisfactionSurveyI18n（字段名映射一次，文案由 entity.customersatisfactionsurvey.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CustomerSatisfactionSurveyQuery } from '@/types/logistics/quality/complaint/customerSatisfactionSurvey.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktCustomerSatisfactionSurveyI18nSeedData 一致的实体 slug */
export const CUSTOMERSATISFACTIONSURVEY_ENTITY_SLUG = 'customersatisfactionsurvey'

/** entity.customersatisfactionsurvey._self 静态属性（导入组件 entity-i18n-key 等） */
export const CUSTOMERSATISFACTIONSURVEY_SELF_I18N_KEY = buildEntitySelfI18nKey(CUSTOMERSATISFACTIONSURVEY_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const CUSTOMERSATISFACTIONSURVEY_LIST_FIELDS = [
  'followUpStatus',
  'remark',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const CUSTOMERSATISFACTIONSURVEY_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type CustomerSatisfactionSurveyField = keyof typeof CUSTOMERSATISFACTIONSURVEY_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const CUSTOMERSATISFACTIONSURVEY_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof CustomerSatisfactionSurveyQuery)[]

export type CustomerSatisfactionSurveyQueryField = (typeof CUSTOMERSATISFACTIONSURVEY_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const CUSTOMERSATISFACTIONSURVEY_QUERY_FIELDS: readonly CustomerSatisfactionSurveyQueryField[] = [...CUSTOMERSATISFACTIONSURVEY_QUERY_STRING_FIELDS]

/**
 * 客户满意度调查表主表实体字段 i18n：index / customer-satisfaction-survey-form 统一入口
 */
export function useCustomerSatisfactionSurveyI18n() {
  const ef = useEntityFieldI18n(CUSTOMERSATISFACTIONSURVEY_ENTITY_SLUG)

  function ph(field: CustomerSatisfactionSurveyField): string {
    return ef.placeholder(field, CUSTOMERSATISFACTIONSURVEY_PLACEHOLDER[field])
  }

  function queryPh(field: CustomerSatisfactionSurveyQueryField, kind: EntityFieldPlaceholderKind): string {
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

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/quality/complaint/customer-satisfaction-survey/composables
// 文件名称：useCustomerSatisfactionSurveyItemI18n.ts
// 功能描述：CustomerSatisfactionSurveyItem字段清单 + useCustomerSatisfactionSurveyItemI18n（字段名映射一次，文案由 entity.customersatisfactionsurveyitem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CustomerSatisfactionSurveyItemQuery } from '@/types/logistics/quality/complaint/customerSatisfactionSurveyItem.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktCustomerSatisfactionSurveyItemI18nSeedData 一致的实体 slug */
export const CUSTOMERSATISFACTIONSURVEYITEM_ENTITY_SLUG = 'customersatisfactionsurveyitem'

/** entity.customersatisfactionsurveyitem._self 静态属性（导入组件 entity-i18n-key 等） */
export const CUSTOMERSATISFACTIONSURVEYITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(CUSTOMERSATISFACTIONSURVEYITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const CUSTOMERSATISFACTIONSURVEYITEM_LIST_FIELDS = [
  'customerSatisfactionSurveyCode',
  'lineNumber',
  'categoryType',
  'itemName',
  'itemDescription',
  'weight',
  'score',
  'satisfactionLevel',
  'customerFeedback',
  'improvementSuggestion',
  'followUpAction',
  'followUpStatus',
  'isObsolete',
  'remark',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const CUSTOMERSATISFACTIONSURVEYITEM_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'customerSatisfactionSurveyCode',
  'lineNumber',
  'categoryType',
  'itemName',
  'itemDescription',
  'weight',
  'score',
  'satisfactionLevel',
  'customerFeedback',
  'improvementSuggestion',
  'followUpAction',
  'followUpStatus',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const CUSTOMERSATISFACTIONSURVEYITEM_SUMMARY_SUM_FIELDS = [
  'categoryType',
  'weight',
  'score',
  'satisfactionLevel',
  'followUpStatus',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const CUSTOMERSATISFACTIONSURVEYITEM_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type CustomerSatisfactionSurveyItemField = keyof typeof CUSTOMERSATISFACTIONSURVEYITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const CUSTOMERSATISFACTIONSURVEYITEM_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof CustomerSatisfactionSurveyItemQuery)[]

export type CustomerSatisfactionSurveyItemQueryField = (typeof CUSTOMERSATISFACTIONSURVEYITEM_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const CUSTOMERSATISFACTIONSURVEYITEM_QUERY_FIELDS: readonly CustomerSatisfactionSurveyItemQueryField[] = [...CUSTOMERSATISFACTIONSURVEYITEM_QUERY_STRING_FIELDS]

/**
 * CustomerSatisfactionSurveyItem字段 i18n：index / customer-satisfaction-survey-item-form 统一入口
 */
export function useCustomerSatisfactionSurveyItemI18n() {
  const ef = useEntityFieldI18n(CUSTOMERSATISFACTIONSURVEYITEM_ENTITY_SLUG)

  function ph(field: CustomerSatisfactionSurveyItemField): string {
    return ef.placeholder(field, CUSTOMERSATISFACTIONSURVEYITEM_PLACEHOLDER[field])
  }

  function queryPh(field: CustomerSatisfactionSurveyItemQueryField, kind: EntityFieldPlaceholderKind): string {
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

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/quality/cost/incident/composables
// 文件名称：useIncidentItemI18n.ts
// 功能描述：QualityIncidentItem字段清单 + useQualityIncidentItemI18n（字段名映射一次，文案由 entity.qualityincidentitem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { QualityIncidentItemQuery } from '@/types/logistics/quality/cost/incidentItem.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktQualityIncidentItemI18nSeedData 一致的实体 slug */
export const QUALITYINCIDENTITEM_ENTITY_SLUG = 'qualityincidentitem'

/** entity.qualityincidentitem._self 静态属性（导入组件 entity-i18n-key 等） */
export const QUALITYINCIDENTITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(QUALITYINCIDENTITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const QUALITYINCIDENTITEM_LIST_FIELDS = [
  'qualityIncidentId',
  'qualityIncidentCode',
  'lineNumber',
  'materialCode',
  'materialDescription',
  'scrapCost',
  'scrapSize',
  'partPrice',
  'scrapReasonCost',
  'freightCharges',
  'otherExpenses',
  'reasonWorkTimeMinutes',
  'tax',
  'reasonOtherExpenses',
  'scrapNote',
  'isObsolete',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const QUALITYINCIDENTITEM_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'qualityIncidentId',
  'qualityIncidentCode',
  'lineNumber',
  'materialCode',
  'materialDescription',
  'scrapCost',
  'scrapSize',
  'partPrice',
  'scrapReasonCost',
  'freightCharges',
  'otherExpenses',
  'reasonWorkTimeMinutes',
  'tax',
  'reasonOtherExpenses',
  'scrapNote',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const QUALITYINCIDENTITEM_SUMMARY_SUM_FIELDS = [
  'scrapCost',
  'scrapSize',
  'partPrice',
  'scrapReasonCost',
  'freightCharges',
  'otherExpenses',
  'reasonWorkTimeMinutes',
  'tax',
  'reasonOtherExpenses',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const QUALITYINCIDENTITEM_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  lineNumber: 'select',
  materialCode: 'select',
  materialDescription: 'optional',
  scrapCost: 'select',
  scrapSize: 'select',
  partPrice: 'select',
  scrapReasonCost: 'select',
  freightCharges: 'select',
  otherExpenses: 'select',
  reasonWorkTimeMinutes: 'select',
  tax: 'select',
  reasonOtherExpenses: 'select',
  scrapNote: 'optional',
  isObsolete: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type QualityIncidentItemField = keyof typeof QUALITYINCIDENTITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const QUALITYINCIDENTITEM_QUERY_STRING_FIELDS = [
  'qualityIncidentCode',
  'materialCode',
  'materialDescription',
  'scrapNote',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof QualityIncidentItemQuery)[]

export type QualityIncidentItemQueryField =
  | (typeof QUALITYINCIDENTITEM_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'scrapCost' | 'scrapSize' | 'partPrice' | 'scrapReasonCost' | 'freightCharges' | 'otherExpenses' | 'reasonWorkTimeMinutes' | 'tax' | 'reasonOtherExpenses' | 'isObsolete'

/** 高级查询抽屉全部字段（含数值） */
export const QUALITYINCIDENTITEM_QUERY_FIELDS: readonly QualityIncidentItemQueryField[] = [
  ...QUALITYINCIDENTITEM_QUERY_STRING_FIELDS,
  'lineNumber',
  'scrapCost',
  'scrapSize',
  'partPrice',
  'scrapReasonCost',
  'freightCharges',
  'otherExpenses',
  'reasonWorkTimeMinutes',
  'tax',
  'reasonOtherExpenses',
  'isObsolete',
]

/**
 * QualityIncidentItem字段 i18n：index / incident-item-form 统一入口
 */
export function useQualityIncidentItemI18n() {
  const ef = useEntityFieldI18n(QUALITYINCIDENTITEM_ENTITY_SLUG)

  function ph(field: QualityIncidentItemField): string {
    return ef.placeholder(field, QUALITYINCIDENTITEM_PLACEHOLDER[field])
  }

  function queryPh(field: QualityIncidentItemQueryField, kind: EntityFieldPlaceholderKind): string {
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

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/quality/cost/issue/composables
// 文件名称：useIssueMeetingI18n.ts
// 功能描述：QualityIssueMeeting字段清单 + useQualityIssueMeetingI18n（字段名映射一次，文案由 entity.qualityissuemeeting.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { QualityIssueMeetingQuery } from '@/types/logistics/quality/cost/issueMeeting.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktQualityIssueMeetingI18nSeedData 一致的实体 slug */
export const QUALITYISSUEMEETING_ENTITY_SLUG = 'qualityissuemeeting'

/** entity.qualityissuemeeting._self 静态属性（导入组件 entity-i18n-key 等） */
export const QUALITYISSUEMEETING_SELF_I18N_KEY = buildEntitySelfI18nKey(QUALITYISSUEMEETING_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const QUALITYISSUEMEETING_LIST_FIELDS = [
  'qualityIssueCode',
  'lineNumber',
  'directManpowerCostPerMinute',
  'indirectManpowerCostPerMinute',
  'meetingInvestigationContent',
  'meetingInvestigationCost',
  'meetingTimeMinutes',
  'directParticipantCount',
  'indirectParticipantCount',
  'investigationWorkTimeMinutes',
  'travelCost',
  'otherExpenses',
  'otherWorkTimeMinutes',
  'otherApparatusCost',
  'responseGroupCode',
  'responseGroupName',
  'isObsolete',
  'remark',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const QUALITYISSUEMEETING_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'qualityIssueCode',
  'lineNumber',
  'directManpowerCostPerMinute',
  'indirectManpowerCostPerMinute',
  'meetingInvestigationContent',
  'meetingInvestigationCost',
  'meetingTimeMinutes',
  'directParticipantCount',
  'indirectParticipantCount',
  'investigationWorkTimeMinutes',
  'travelCost',
  'otherExpenses',
  'otherWorkTimeMinutes',
  'otherApparatusCost',
  'responseGroupCode',
  'responseGroupName',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const QUALITYISSUEMEETING_SUMMARY_SUM_FIELDS = [
  'directManpowerCostPerMinute',
  'indirectManpowerCostPerMinute',
  'meetingInvestigationCost',
  'meetingTimeMinutes',
  'directParticipantCount',
  'indirectParticipantCount',
  'investigationWorkTimeMinutes',
  'travelCost',
  'otherExpenses',
  'otherWorkTimeMinutes',
  'otherApparatusCost',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const QUALITYISSUEMEETING_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type QualityIssueMeetingField = keyof typeof QUALITYISSUEMEETING_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const QUALITYISSUEMEETING_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof QualityIssueMeetingQuery)[]

export type QualityIssueMeetingQueryField = (typeof QUALITYISSUEMEETING_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const QUALITYISSUEMEETING_QUERY_FIELDS: readonly QualityIssueMeetingQueryField[] = [...QUALITYISSUEMEETING_QUERY_STRING_FIELDS]

/**
 * QualityIssueMeeting字段 i18n：index / issue-meeting-form 统一入口
 */
export function useQualityIssueMeetingI18n() {
  const ef = useEntityFieldI18n(QUALITYISSUEMEETING_ENTITY_SLUG)

  function ph(field: QualityIssueMeetingField): string {
    return ef.placeholder(field, QUALITYISSUEMEETING_PLACEHOLDER[field])
  }

  function queryPh(field: QualityIssueMeetingQueryField, kind: EntityFieldPlaceholderKind): string {
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

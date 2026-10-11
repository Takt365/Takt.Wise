// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/routine/meeting-center/meeting/composables
// 文件名称：useMeetingMinutesI18n.ts
// 功能描述：MeetingMinutes 字段清单 + useMeetingMinutesI18n（文案由 entity.meetingminutes.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { MeetingMinutesQuery } from '@/types/routine/meeting-center/meetingMinutes.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktMeetingMinutesI18nSeedData 一致的实体 slug */
export const MEETINGMINUTES_ENTITY_SLUG = 'meetingminutes'

/** entity.meetingminutes._self */
export const MEETINGMINUTES_SELF_I18N_KEY = buildEntitySelfI18nKey(MEETINGMINUTES_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const MEETINGMINUTES_LIST_FIELDS = [
  'meetingTitle',
  'lineNumber',
  'meetingSummary',
  'recorderName',
  'fileName',
] as const

/** 明细右栏 panel 默认展示列 */
export const MEETINGMINUTES_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'meetingTitle',
  'lineNumber',
  'meetingSummary',
  'recorderName',
  'action',
] as const

/** 汇总求和字段（无） */
export const MEETINGMINUTES_SUMMARY_SUM_FIELDS = [] as const

/** 查询字符串字段 */
export const MEETINGMINUTES_QUERY_STRING_FIELDS = [
  'meetingTitle',
  'meetingMinutes',
  'meetingSummary',
  'recorderName',
  'fileName',
  'accessUrl',
] as const

/** 查询字段全集（抽屉） */
export const MEETINGMINUTES_QUERY_FIELDS = [
  ...MEETINGMINUTES_QUERY_STRING_FIELDS,
] as const

/**
 * 会后纪要字段 i18n
 * @returns 标签/占位符工具
 */
export function useMeetingMinutesI18n() {
  return useEntityFieldI18n(MEETINGMINUTES_ENTITY_SLUG)
}

export type { MeetingMinutesQuery, EntityFieldPlaceholderKind }

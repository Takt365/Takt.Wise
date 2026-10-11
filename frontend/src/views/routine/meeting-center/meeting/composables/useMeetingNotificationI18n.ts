// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/routine/meeting-center/meeting/composables
// 文件名称：useMeetingNotificationI18n.ts
// 功能描述：MeetingNotification 字段清单 + useMeetingNotificationI18n（文案由 entity.meetingnotification.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { MeetingNotificationQuery } from '@/types/routine/meeting-center/meetingNotification.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktMeetingNotificationI18nSeedData 一致的实体 slug */
export const MEETINGNOTIFICATION_ENTITY_SLUG = 'meetingnotification'

/** entity.meetingnotification._self */
export const MEETINGNOTIFICATION_SELF_I18N_KEY = buildEntitySelfI18nKey(MEETINGNOTIFICATION_ENTITY_SLUG)

/** 明细右栏 panel 默认展示列 */
export const MEETINGNOTIFICATION_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'userName',
  'recipientEmail',
  'notificationType',
  'deliveryStatus',
  'sentAt',
  'confirmedAt',
  'action',
] as const

/** 查询字符串字段 */
export const MEETINGNOTIFICATION_QUERY_STRING_FIELDS = [
  'userName',
  'recipientEmail',
  'notificationSubject',
] as const

/** 投递状态：已发送 */
export const MEETING_NOTIFICATION_DELIVERY_SENT = 1

/** 投递状态：已确认 */
export const MEETING_NOTIFICATION_DELIVERY_CONFIRMED = 2

/**
 * 会议通知字段 i18n
 * @returns 标签/占位符工具
 */
export function useMeetingNotificationI18n() {
  return useEntityFieldI18n(MEETINGNOTIFICATION_ENTITY_SLUG)
}

export type { MeetingNotificationQuery, EntityFieldPlaceholderKind }

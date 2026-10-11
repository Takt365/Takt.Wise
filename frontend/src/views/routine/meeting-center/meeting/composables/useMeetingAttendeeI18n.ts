// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/routine/meeting-center/meeting/composables
// 文件名称：useMeetingAttendeeI18n.ts
// 功能描述：MeetingAttendee字段清单 + useMeetingAttendeeI18n（字段名映射一次，文案由 entity.meetingattendee.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { MeetingAttendeeQuery } from '@/types/routine/meeting-center/meetingAttendee.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktMeetingAttendeeI18nSeedData 一致的实体 slug */
export const MEETINGATTENDEE_ENTITY_SLUG = 'meetingattendee'

/** entity.meetingattendee._self 静态属性（导入组件 entity-i18n-key 等） */
export const MEETINGATTENDEE_SELF_I18N_KEY = buildEntitySelfI18nKey(MEETINGATTENDEE_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const MEETINGATTENDEE_LIST_FIELDS = [
  'userId',
  'userName',
  'attendeeRole',
  'attendanceStatus',
  'checkInTime',
  'checkOutTime',
  'checkInMethod',
  'remark',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const MEETINGATTENDEE_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'userId',
  'userName',
  'attendeeRole',
  'attendanceStatus',
  'checkInTime',
  'checkOutTime',
  'checkInMethod',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const MEETINGATTENDEE_SUMMARY_SUM_FIELDS = [
  'attendeeRole',
  'attendanceStatus',
  'checkInMethod',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const MEETINGATTENDEE_PLACEHOLDER = {
  lineNumber: 'select',
  userId: 'select',
  userName: 'optional',
  attendeeRole: 'select',
  attendanceStatus: 'select',
  checkInTime: 'optional',
  checkOutTime: 'optional',
  checkInMethod: 'select',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type MeetingAttendeeField = keyof typeof MEETINGATTENDEE_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const MEETINGATTENDEE_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof MeetingAttendeeQuery)[]

export type MeetingAttendeeQueryField = (typeof MEETINGATTENDEE_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const MEETINGATTENDEE_QUERY_FIELDS: readonly MeetingAttendeeQueryField[] = [...MEETINGATTENDEE_QUERY_STRING_FIELDS]

/**
 * MeetingAttendee字段 i18n：index / meeting-attendee-form 统一入口
 */
export function useMeetingAttendeeI18n() {
  const ef = useEntityFieldI18n(MEETINGATTENDEE_ENTITY_SLUG)

  function ph(field: MeetingAttendeeField): string {
    return ef.placeholder(field, MEETINGATTENDEE_PLACEHOLDER[field])
  }

  function queryPh(field: MeetingAttendeeQueryField, kind: EntityFieldPlaceholderKind): string {
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

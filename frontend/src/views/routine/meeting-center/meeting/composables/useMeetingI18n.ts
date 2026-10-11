// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/routine/meeting-center/meeting/composables
// 文件名称：useMeetingI18n.ts
// 功能描述：会议中心主实体 支持内部/外部/视频/混合会议排期、议程及参与人管理字段清单 + useMeetingI18n（字段名映射一次，文案由 entity.meeting.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { MeetingQuery } from '@/types/routine/meeting-center/meeting.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktMeetingI18nSeedData 一致的实体 slug */
export const MEETING_ENTITY_SLUG = 'meeting'

/** entity.meeting._self 静态属性（导入组件 entity-i18n-key 等） */
export const MEETING_SELF_I18N_KEY = buildEntitySelfI18nKey(MEETING_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const MEETING_LIST_FIELDS = [
  'meetingTitle',
  'meetingType',
  'meetingStatus',
  'startTime',
  'endTime',
  'location',
  'meetingLink',
  'meetingAgenda',
  'meetingTags',
  'organizerId',
  'organizerName',
  'deptId',
  'deptName',
  'maxAttendees',
  'reminderMinutes',
  'meetingRoomId',
  'meetingRoomName',
  'remark',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const MEETING_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  meetingCode: 'required',
  meetingTitle: 'required',
  meetingType: 'select',
  startTime: 'required',
  endTime: 'required',
  location: 'optional',
  meetingLink: 'optional',
  meetingAgenda: 'optional',
  meetingTags: 'optional',
  organizerId: 'select',
  organizerName: 'optional',
  deptId: 'optional',
  deptName: 'optional',
  maxAttendees: 'select',
  reminderMinutes: 'select',
  meetingRoomId: 'select',
  meetingRoomName: 'optional',
  meetingStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type MeetingField = keyof typeof MEETING_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const MEETING_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof MeetingQuery)[]

export type MeetingQueryField = (typeof MEETING_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const MEETING_QUERY_FIELDS: readonly MeetingQueryField[] = [...MEETING_QUERY_STRING_FIELDS]

/**
 * 会议中心主实体 支持内部/外部/视频/混合会议排期、议程及出席人管理字段 i18n：index / meeting-form 统一入口
 */
export function useMeetingI18n() {
  const ef = useEntityFieldI18n(MEETING_ENTITY_SLUG)

  function ph(field: MeetingField): string {
    return ef.placeholder(field, MEETING_PLACEHOLDER[field])
  }

  function queryPh(field: MeetingQueryField, kind: EntityFieldPlaceholderKind): string {
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

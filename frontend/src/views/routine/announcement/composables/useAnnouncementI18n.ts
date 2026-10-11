// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/routine/announcement/composables
// 文件名称：useAnnouncementI18n.ts
// 功能描述：公告通知实体 用于发布系统公告、通知等信息字段清单 + useAnnouncementI18n（字段名映射一次，文案由 entity.announcement.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { AnnouncementQuery } from '@/types/routine/announcement.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktAnnouncementI18nSeedData 一致的实体 slug */
export const ANNOUNCEMENT_ENTITY_SLUG = 'announcement'

/** entity.announcement._self 静态属性（导入组件 entity-i18n-key 等） */
export const ANNOUNCEMENT_SELF_I18N_KEY = buildEntitySelfI18nKey(ANNOUNCEMENT_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const ANNOUNCEMENT_LIST_FIELDS = [
  'announcementCode',
  'announcementTitle',
  'announcementType',
  'content',
  'summary',
  'tags',
  'fileName',
  'accessUrl',
  'publishTime',
  'isScheduled',
  'isTop',
  'topPriority',
  'expireTime',
  'viewCount',
  'targetScope',
  'targetDepartments',
  'targetUsers',
  'announcementStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const ANNOUNCEMENT_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  numberingRuleCode: 'select',
  announcementCode: 'optional',
  numberingRuleCode: 'optional',
  announcementTitle: 'required',
  announcementType: 'select',
  content: 'optional',
  summary: 'optional',
  tags: 'optional',
  fileName: 'optional',
  accessUrl: 'optional',
  publishTime: 'optional',
  isScheduled: 'select',
  isTop: 'select',
  topPriority: 'select',
  expireTime: 'optional',
  viewCount: 'select',
  targetScope: 'select',
  targetDepartments: 'optional',
  targetUsers: 'optional',
  announcementStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type AnnouncementField = keyof typeof ANNOUNCEMENT_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const ANNOUNCEMENT_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'announcementCode',
  'announcementTitle',
  'content',
  'summary',
  'tags',
  'fileName',
  'accessUrl',
  'publishTimeStart',
  'publishTimeEnd',
  'expireTimeStart',
  'expireTimeEnd',
  'targetDepartments',
  'targetUsers',
  'initiatorId',
  'initiatedAtStart',
  'initiatedAtEnd',
  'approvedBy',
  'approvedAtStart',
  'approvedAtEnd',
  'flowInstanceId',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof AnnouncementQuery)[]

export type AnnouncementQueryField =
  | (typeof ANNOUNCEMENT_QUERY_STRING_FIELDS)[number]
  | 'announcementType' | 'isScheduled' | 'isTop' | 'topPriority' | 'viewCount' | 'targetScope' | 'announcementStatus' | 'approvalStatus'

/** 高级查询抽屉全部字段（含数值） */
export const ANNOUNCEMENT_QUERY_FIELDS: readonly AnnouncementQueryField[] = [
  ...ANNOUNCEMENT_QUERY_STRING_FIELDS,
  'announcementType',
  'isScheduled',
  'isTop',
  'topPriority',
  'viewCount',
  'targetScope',
  'announcementStatus',
  'approvalStatus',
]

/**
 * 公告通知实体 用于发布系统公告、通知等信息字段 i18n：index / AnnouncementForm 统一入口
 */
export function useAnnouncementI18n() {
  const ef = useEntityFieldI18n(ANNOUNCEMENT_ENTITY_SLUG)

  function ph(field: AnnouncementField): string {
    return ef.placeholder(field, ANNOUNCEMENT_PLACEHOLDER[field])
  }

  function queryPh(field: AnnouncementQueryField, kind: EntityFieldPlaceholderKind): string {
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

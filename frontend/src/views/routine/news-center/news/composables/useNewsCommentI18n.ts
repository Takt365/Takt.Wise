// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/routine/news-center/news-comment/composables
// 文件名称：useNewsCommentI18n.ts
// 功能描述：NewsComment字段清单 + useNewsCommentI18n（字段名映射一次，文案由 entity.newscomment.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { NewsCommentQuery } from '@/types/routine/news-center/newsComment.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktNewsCommentI18nSeedData 一致的实体 slug */
export const NEWSCOMMENT_ENTITY_SLUG = 'newscomment'

/** entity.newscomment._self 静态属性（导入组件 entity-i18n-key 等） */
export const NEWSCOMMENT_SELF_I18N_KEY = buildEntitySelfI18nKey(NEWSCOMMENT_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const NEWSCOMMENT_LIST_FIELDS = [
  'newsId',
  'lineNumber',
  'parentId',
  'userId',
  'userName',
  'userAvatar',
  'replyToUserId',
  'replyToUserName',
  'commentContent',
  'commentTime',
  'newsCommentLikeCount',
  'replyCount',
  'commentLevel',
  'commentStatus',
  'isObsolete',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const NEWSCOMMENT_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'newsId',
  'lineNumber',
  'parentId',
  'userId',
  'userName',
  'userAvatar',
  'replyToUserId',
  'replyToUserName',
  'commentContent',
  'commentTime',
  'newsCommentLikeCount',
  'replyCount',
  'commentLevel',
  'commentStatus',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const NEWSCOMMENT_SUMMARY_SUM_FIELDS = [
  'newsCommentLikeCount',
  'replyCount',
  'commentLevel',
  'commentStatus',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const NEWSCOMMENT_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  lineNumber: 'select',
  parentId: 'select',
  userId: 'select',
  userName: 'optional',
  userAvatar: 'optional',
  replyToUserId: 'optional',
  replyToUserName: 'optional',
  commentContent: 'optional',
  commentTime: 'select',
  newsCommentLikeCount: 'select',
  replyCount: 'select',
  commentLevel: 'select',
  commentStatus: 'select',
  isObsolete: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type NewsCommentField = keyof typeof NEWSCOMMENT_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const NEWSCOMMENT_QUERY_STRING_FIELDS = [
  'cultureCode',
  'parentId',
  'userId',
  'userName',
  'userAvatar',
  'replyToUserId',
  'replyToUserName',
  'commentContent',
  'commentTimeStart',
  'commentTimeEnd',
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
] as const satisfies readonly (keyof NewsCommentQuery)[]

export type NewsCommentQueryField =
  | (typeof NEWSCOMMENT_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'newsCommentLikeCount' | 'replyCount' | 'commentLevel' | 'commentStatus' | 'isObsolete' | 'approvalStatus'

/** 高级查询抽屉全部字段（含数值） */
export const NEWSCOMMENT_QUERY_FIELDS: readonly NewsCommentQueryField[] = [
  ...NEWSCOMMENT_QUERY_STRING_FIELDS,
  'lineNumber',
  'newsCommentLikeCount',
  'replyCount',
  'commentLevel',
  'commentStatus',
  'isObsolete',
  'approvalStatus',
]

/**
 * NewsComment字段 i18n：index / news-comment-form 统一入口
 */
export function useNewsCommentI18n() {
  const ef = useEntityFieldI18n(NEWSCOMMENT_ENTITY_SLUG)

  function ph(field: NewsCommentField): string {
    return ef.placeholder(field, NEWSCOMMENT_PLACEHOLDER[field])
  }

  function queryPh(field: NewsCommentQueryField, kind: EntityFieldPlaceholderKind): string {
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

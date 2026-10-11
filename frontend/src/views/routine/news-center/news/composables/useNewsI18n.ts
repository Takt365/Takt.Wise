// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/routine/news-center/news/composables
// 文件名称：useNewsI18n.ts
// 功能描述：新闻中心主实体 支持分类、置顶、推荐、社交统计字段清单 + useNewsI18n（字段名映射一次，文案由 entity.news.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { NewsQuery } from '@/types/routine/news-center/news.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktNewsI18nSeedData 一致的实体 slug */
export const NEWS_ENTITY_SLUG = 'news'

/** entity.news._self 静态属性（导入组件 entity-i18n-key 等） */
export const NEWS_SELF_I18N_KEY = buildEntitySelfI18nKey(NEWS_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const NEWS_LIST_FIELDS = [
  'newsCode',
  'newsCategory',
  'newsTitle',
  'newsSummary',
  'newsTags',
  'newsContent',
  'newsCoverImage',
  'newsIsTop',
  'newsIsRecommended',
  'newsEffectiveTime',
  'newsExpireTime',
  'newsReadCount',
  'newsLikeCount',
  'newsCommentCount',
  'newsFavoriteCount',
  'newsShareCount',
  'newsAttachmentCount',
  'deptId',
  'deptName',
  'publisherId',
  'publisherName',
  'newsPublishTime',
  'targetScope',
  'targetDepartments',
  'targetUsers',
  'newsStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const NEWS_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  newsCode: 'required',
  newsCategory: 'select',
  newsTitle: 'required',
  newsSummary: 'optional',
  newsTags: 'optional',
  newsContent: 'optional',
  newsCoverImage: 'optional',
  newsIsTop: 'select',
  newsIsRecommended: 'select',
  newsEffectiveTime: 'optional',
  newsExpireTime: 'optional',
  newsReadCount: 'select',
  newsLikeCount: 'select',
  newsCommentCount: 'select',
  newsFavoriteCount: 'select',
  newsShareCount: 'select',
  newsAttachmentCount: 'select',
  deptId: 'optional',
  deptName: 'optional',
  publisherId: 'select',
  publisherName: 'optional',
  newsPublishTime: 'optional',
  targetScope: 'select',
  targetDepartments: 'optional',
  targetUsers: 'optional',
  newsStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type NewsField = keyof typeof NEWS_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const NEWS_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'newsCode',
  'newsTitle',
  'newsSummary',
  'newsTags',
  'newsContent',
  'newsCoverImage',
  'newsEffectiveTimeStart',
  'newsEffectiveTimeEnd',
  'newsExpireTimeStart',
  'newsExpireTimeEnd',
  'deptId',
  'deptName',
  'publisherId',
  'publisherName',
  'newsPublishTimeStart',
  'newsPublishTimeEnd',
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
] as const satisfies readonly (keyof NewsQuery)[]

export type NewsQueryField =
  | (typeof NEWS_QUERY_STRING_FIELDS)[number]
  | 'newsCategory' | 'newsIsTop' | 'newsIsRecommended' | 'newsReadCount' | 'newsLikeCount' | 'newsCommentCount' | 'newsFavoriteCount' | 'newsShareCount' | 'newsAttachmentCount' | 'targetScope' | 'newsStatus' | 'approvalStatus'

/** 高级查询抽屉全部字段（含数值） */
export const NEWS_QUERY_FIELDS: readonly NewsQueryField[] = [
  ...NEWS_QUERY_STRING_FIELDS,
  'newsCategory',
  'newsIsTop',
  'newsIsRecommended',
  'newsReadCount',
  'newsLikeCount',
  'newsCommentCount',
  'newsFavoriteCount',
  'newsShareCount',
  'newsAttachmentCount',
  'targetScope',
  'newsStatus',
  'approvalStatus',
]

/**
 * 新闻中心主实体 支持分类、置顶、推荐、社交统计字段 i18n：index / news-form 统一入口
 */
export function useNewsI18n() {
  const ef = useEntityFieldI18n(NEWS_ENTITY_SLUG)

  function ph(field: NewsField): string {
    return ef.placeholder(field, NEWS_PLACEHOLDER[field])
  }

  function queryPh(field: NewsQueryField, kind: EntityFieldPlaceholderKind): string {
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

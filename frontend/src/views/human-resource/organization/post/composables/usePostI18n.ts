// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/human-resource/organization/post/composables
// 文件名称：usePostI18n.ts
// 功能描述：岗位实体 代表组织架构中的岗位/职位字段清单 + usePostI18n（字段名映射一次，文案由 entity.post.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PostQuery } from '@/types/human-resource/organization/post.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktPostI18nSeedData 一致的实体 slug */
export const POST_ENTITY_SLUG = 'post'

/** entity.post._self 静态属性（导入组件 entity-i18n-key 等） */
export const POST_SELF_I18N_KEY = buildEntitySelfI18nKey(POST_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const POST_LIST_FIELDS = [
  'postCode',
  'postName',
  'deptId',
  'deptName',
  'postCategory',
  'postLevel',
  'headcount',
  'currentCount',
  'responsibilities',
  'requirements',
  'educationRequired',
  'experienceYears',
  'salaryMin',
  'salaryMax',
  'isBuiltIn',
  'postDescription',
  'postStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const POST_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'select',
  postCode: 'required',
  postName: 'required',
  deptId: 'select',
  deptName: 'optional',
  postCategory: 'select',
  postLevel: 'select',
  headcount: 'select',
  currentCount: 'select',
  responsibilities: 'required',
  requirements: 'required',
  educationRequired: 'select',
  experienceYears: 'select',
  salaryMin: 'optional',
  salaryMax: 'optional',
  isBuiltIn: 'select',
  postDescription: 'optional',
  postStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type PostField = keyof typeof POST_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const POST_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'postCode',
  'postName',
  'deptId',
  'deptName',
  'postCategory',
  'postLevel',
  'responsibilities',
  'requirements',
  'postDescription',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof PostQuery)[]

export type PostQueryField =
  | (typeof POST_QUERY_STRING_FIELDS)[number]
  | 'headcount' | 'currentCount' | 'educationRequired' | 'experienceYears' | 'salaryMin' | 'salaryMax' | 'isBuiltIn' | 'postStatus'

/** 高级查询抽屉全部字段（含数值） */
export const POST_QUERY_FIELDS: readonly PostQueryField[] = [
  ...POST_QUERY_STRING_FIELDS,
  'headcount',
  'currentCount',
  'educationRequired',
  'experienceYears',
  'salaryMin',
  'salaryMax',
  'isBuiltIn',
  'postStatus',
]

/**
 * 岗位实体 代表组织架构中的岗位/职位字段 i18n：index / post-form 统一入口
 */
export function usePostI18n() {
  const ef = useEntityFieldI18n(POST_ENTITY_SLUG)

  function ph(field: PostField): string {
    return ef.placeholder(field, POST_PLACEHOLDER[field])
  }

  function queryPh(field: PostQueryField, kind: EntityFieldPlaceholderKind): string {
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

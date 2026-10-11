// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/human-resource/talent/job-posting/composables
// 文件名称：useJobPostingI18n.ts
// 功能描述：职位发布字段清单 + useTalentJobPostingI18n（字段名映射一次，文案由 entity.talentjobposting.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TalentJobPostingQuery } from '@/types/human-resource/talent/jobPosting.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktTalentJobPostingI18nSeedData 一致的实体 slug */
export const TALENTJOBPOSTING_ENTITY_SLUG = 'talentjobposting'

/** entity.talentjobposting._self 静态属性（导入组件 entity-i18n-key 等） */
export const TALENTJOBPOSTING_SELF_I18N_KEY = buildEntitySelfI18nKey(TALENTJOBPOSTING_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const TALENTJOBPOSTING_LIST_FIELDS = [
  'postingCode',
  'talentJobPostingTitle',
  'postingStatus',
  'publishDate',
  'openDate',
  'closeDate',
  'publishChannel',
  'reason',
  'remark',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const TALENTJOBPOSTING_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type TalentJobPostingField = keyof typeof TALENTJOBPOSTING_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const TALENTJOBPOSTING_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof TalentJobPostingQuery)[]

export type TalentJobPostingQueryField = (typeof TALENTJOBPOSTING_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const TALENTJOBPOSTING_QUERY_FIELDS: readonly TalentJobPostingQueryField[] = [...TALENTJOBPOSTING_QUERY_STRING_FIELDS]

/**
 * 职位发布字段 i18n：index / job-posting-form 统一入口
 */
export function useTalentJobPostingI18n() {
  const ef = useEntityFieldI18n(TALENTJOBPOSTING_ENTITY_SLUG)

  function ph(field: TalentJobPostingField): string {
    return ef.placeholder(field, TALENTJOBPOSTING_PLACEHOLDER[field])
  }

  function queryPh(field: TalentJobPostingQueryField, kind: EntityFieldPlaceholderKind): string {
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

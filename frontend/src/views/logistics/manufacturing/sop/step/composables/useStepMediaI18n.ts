// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/sop/step/composables
// 文件名称：useStepMediaI18n.ts
// 功能描述：SopStepMedia字段清单 + useSopStepMediaI18n（字段名映射一次，文案由 entity.sopstepmedia.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SopStepMediaQuery } from '@/types/logistics/manufacturing/sop/stepMedia.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSopStepMediaI18nSeedData 一致的实体 slug */
export const SOPSTEPMEDIA_ENTITY_SLUG = 'sopstepmedia'

/** entity.sopstepmedia._self 静态属性（导入组件 entity-i18n-key 等） */
export const SOPSTEPMEDIA_SELF_I18N_KEY = buildEntitySelfI18nKey(SOPSTEPMEDIA_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SOPSTEPMEDIA_LIST_FIELDS = [
  'stepId',
  'mediaType',
  'fileUrl',
  'fileExt',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const SOPSTEPMEDIA_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'stepId',
  'mediaType',
  'fileUrl',
  'fileExt',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const SOPSTEPMEDIA_SUMMARY_SUM_FIELDS = [
  'mediaType',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SOPSTEPMEDIA_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  mediaType: 'select',
  fileUrl: 'required',
  fileExt: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SopStepMediaField = keyof typeof SOPSTEPMEDIA_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SOPSTEPMEDIA_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'fileUrl',
  'fileExt',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SopStepMediaQuery)[]

export type SopStepMediaQueryField =
  | (typeof SOPSTEPMEDIA_QUERY_STRING_FIELDS)[number]
  | 'mediaType'

/** 高级查询抽屉全部字段（含数值） */
export const SOPSTEPMEDIA_QUERY_FIELDS: readonly SopStepMediaQueryField[] = [
  ...SOPSTEPMEDIA_QUERY_STRING_FIELDS,
  'mediaType',
]

/**
 * SopStepMedia字段 i18n：index / step-media-form 统一入口
 */
export function useSopStepMediaI18n() {
  const ef = useEntityFieldI18n(SOPSTEPMEDIA_ENTITY_SLUG)

  function ph(field: SopStepMediaField): string {
    return ef.placeholder(field, SOPSTEPMEDIA_PLACEHOLDER[field])
  }

  function queryPh(field: SopStepMediaQueryField, kind: EntityFieldPlaceholderKind): string {
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

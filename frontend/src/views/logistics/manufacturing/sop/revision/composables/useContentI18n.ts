// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/sop/revision/composables
// 文件名称：useContentI18n.ts
// 功能描述：SopContent字段清单 + useSopContentI18n（字段名映射一次，文案由 entity.sopcontent.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SopContentQuery } from '@/types/logistics/manufacturing/sop/content.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSopContentI18nSeedData 一致的实体 slug */
export const SOPCONTENT_ENTITY_SLUG = 'sopcontent'

/** entity.sopcontent._self 静态属性（导入组件 entity-i18n-key 等） */
export const SOPCONTENT_SELF_I18N_KEY = buildEntitySelfI18nKey(SOPCONTENT_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SOPCONTENT_LIST_FIELDS = [
  'revisionId',
  'sopId',
  'contentTitle',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const SOPCONTENT_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'revisionId',
  'sopId',
  'contentTitle',
  'action',
] as const

/** 明细右栏 panel 合计列（无可合计数值字段） */
export const SOPCONTENT_SUMMARY_SUM_FIELDS = [] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SOPCONTENT_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  sopId: 'select',
  contentTitle: 'optional',
  steps: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SopContentField = keyof typeof SOPCONTENT_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SOPCONTENT_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'sopId',
  'contentTitle',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SopContentQuery)[]

export type SopContentQueryField = (typeof SOPCONTENT_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const SOPCONTENT_QUERY_FIELDS: readonly SopContentQueryField[] = [...SOPCONTENT_QUERY_STRING_FIELDS]

/**
 * SopContent字段 i18n：index / content-form 统一入口
 */
export function useSopContentI18n() {
  const ef = useEntityFieldI18n(SOPCONTENT_ENTITY_SLUG)

  function ph(field: SopContentField): string {
    return ef.placeholder(field, SOPCONTENT_PLACEHOLDER[field])
  }

  function queryPh(field: SopContentQueryField, kind: EntityFieldPlaceholderKind): string {
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

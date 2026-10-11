// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/routine/visitor-center/visitor/composables
// 文件名称：useVisitorCompanionI18n.ts
// 功能描述：VisitorCompanion字段清单 + useVisitorCompanionI18n（字段名映射一次，文案由 entity.visitorcompanion.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { VisitorCompanionQuery } from '@/types/routine/visitor-center/visitorCompanion.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktVisitorCompanionI18nSeedData 一致的实体 slug */
export const VISITORCOMPANION_ENTITY_SLUG = 'visitorcompanion'

/** entity.visitorcompanion._self 静态属性（导入组件 entity-i18n-key 等） */
export const VISITORCOMPANION_SELF_I18N_KEY = buildEntitySelfI18nKey(VISITORCOMPANION_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const VISITORCOMPANION_LIST_FIELDS = [
  'department',
  'jobTitle',
  'companionName',
  'remark',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const VISITORCOMPANION_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'department',
  'jobTitle',
  'companionName',
  'action',
] as const

/** 明细右栏 panel 合计列（无可合计数值字段） */
export const VISITORCOMPANION_SUMMARY_SUM_FIELDS = [] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const VISITORCOMPANION_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type VisitorCompanionField = keyof typeof VISITORCOMPANION_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const VISITORCOMPANION_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof VisitorCompanionQuery)[]

export type VisitorCompanionQueryField = (typeof VISITORCOMPANION_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const VISITORCOMPANION_QUERY_FIELDS: readonly VisitorCompanionQueryField[] = [...VISITORCOMPANION_QUERY_STRING_FIELDS]

/**
 * VisitorCompanion字段 i18n：index / visitor-companion-form 统一入口
 */
export function useVisitorCompanionI18n() {
  const ef = useEntityFieldI18n(VISITORCOMPANION_ENTITY_SLUG)

  function ph(field: VisitorCompanionField): string {
    return ef.placeholder(field, VISITORCOMPANION_PLACEHOLDER[field])
  }

  function queryPh(field: VisitorCompanionQueryField, kind: EntityFieldPlaceholderKind): string {
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

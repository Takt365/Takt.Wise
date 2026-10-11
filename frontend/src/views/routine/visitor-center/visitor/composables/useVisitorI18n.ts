// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/routine/visitor-center/visitor/composables
// 文件名称：useVisitorI18n.ts
// 功能描述：来访接待主实体字段清单 + useVisitorI18n（字段名映射一次，文案由 entity.visitor.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { VisitorQuery } from '@/types/routine/visitor-center/visitor.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktVisitorI18nSeedData 一致的实体 slug */
export const VISITOR_ENTITY_SLUG = 'visitor'

/** entity.visitor._self 静态属性（导入组件 entity-i18n-key 等） */
export const VISITOR_SELF_I18N_KEY = buildEntitySelfI18nKey(VISITOR_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const VISITOR_LIST_FIELDS = [
  'visitStartTime',
  'visitEndTime',
  'remark',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const VISITOR_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type VisitorField = keyof typeof VISITOR_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const VISITOR_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof VisitorQuery)[]

export type VisitorQueryField = (typeof VISITOR_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const VISITOR_QUERY_FIELDS: readonly VisitorQueryField[] = [...VISITOR_QUERY_STRING_FIELDS]

/**
 * 来访接待主实体字段 i18n：index / visitor-form 统一入口
 */
export function useVisitorI18n() {
  const ef = useEntityFieldI18n(VISITOR_ENTITY_SLUG)

  function ph(field: VisitorField): string {
    return ef.placeholder(field, VISITOR_PLACEHOLDER[field])
  }

  function queryPh(field: VisitorQueryField, kind: EntityFieldPlaceholderKind): string {
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

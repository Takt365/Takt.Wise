// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/foundation/culture/composables
// 文件名称：useTranslationI18n.ts
// 功能描述：Translation字段清单 + useTranslationI18n（字段名映射一次，文案由 entity.translation.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TranslationQuery } from '@/types/foundation/translation.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktTranslationI18nSeedData 一致的实体 slug */
export const TRANSLATION_ENTITY_SLUG = 'translation'

/** entity.translation._self 静态属性（导入组件 entity-i18n-key 等） */
export const TRANSLATION_SELF_I18N_KEY = buildEntitySelfI18nKey(TRANSLATION_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const TRANSLATION_LIST_FIELDS = [
  'cultureId',
  'i18nKey',
  'translationText',
  'resourceGroup',
  'resourceType',
  'contextNote',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const TRANSLATION_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'cultureId',
  'i18nKey',
  'translationText',
  'resourceGroup',
  'resourceType',
  'contextNote',
  'action',
] as const

/** 明细右栏 panel 合计列（无可合计数值字段） */
export const TRANSLATION_SUMMARY_SUM_FIELDS = [] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const TRANSLATION_PLACEHOLDER = {
  tenantCode: 'optional',
  i18nKey: 'required',
  translationText: 'required',
  resourceGroup: 'select',
  resourceType: 'select',
  contextNote: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type TranslationField = keyof typeof TRANSLATION_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const TRANSLATION_QUERY_STRING_FIELDS = [
  'i18nKey',
  'translationText',
  'resourceGroup',
  'resourceType',
  'contextNote',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof TranslationQuery)[]

export type TranslationQueryField = (typeof TRANSLATION_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const TRANSLATION_QUERY_FIELDS: readonly TranslationQueryField[] = [...TRANSLATION_QUERY_STRING_FIELDS]

/**
 * Translation字段 i18n：index / translation-form 统一入口
 */
export function useTranslationI18n() {
  const ef = useEntityFieldI18n(TRANSLATION_ENTITY_SLUG)

  function ph(field: TranslationField): string {
    return ef.placeholder(field, TRANSLATION_PLACEHOLDER[field])
  }

  function queryPh(field: TranslationQueryField, kind: EntityFieldPlaceholderKind): string {
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

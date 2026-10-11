// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/composables/useEntityFieldI18n
// 文件名称：useEntityFieldI18n.ts
// 功能描述：实体字段文案 composable；翻译键由 takt-entity-i18n 按 slug+字段名推导，视图只传 camelCase 字段名
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { useI18n } from 'vue-i18n'
import {
  buildEntitySelfI18nKey,
  resolveEntityFieldI18nKey,
  resolveQueryRangeFieldLabel,
} from '@/utils/taktEntityI18n'

export type EntityFieldPlaceholderKind = 'required' | 'select' | 'optional'

const PLACEHOLDER_I18N_KEY: Readonly<Record<EntityFieldPlaceholderKind, string>> = {
  required: 'common.page.form.placeholder.required',
  select: 'common.page.form.placeholder.select',
  optional: 'common.page.form.placeholder.optional',
}

/**
 * 任意实体的字段 i18n（列表/查询/表单共用；不重复维护 entity.* 键表）
 * @param entitySlug 实体 slug（全小写，与后端种子一致，如 plant、company）
 */
export function useEntityFieldI18n(entitySlug: string) {
  const { t } = useI18n()

  /**
   * 业务字段标签
   * @param field DTO 属性 camelCase
   */
  function label(field: string): string {
    return t(resolveEntityFieldI18nKey(entitySlug, field))
  }

  /**
   * 高级查询字段标签（含日期/时间区间 Start/End）
   * @param field 查询 DTO 属性 camelCase
   */
  function queryLabel(field: string): string {
    return resolveQueryRangeFieldLabel(entitySlug, field, t)
  }

  /** 实体自称 entity.{slug}._self */
  function self(): string {
    return t(buildEntitySelfI18nKey(entitySlug))
  }

  /**
   * 表单/查询占位符
   * @param field 字段名
   * @param kind 占位类型
   */
  function placeholder(field: string, kind: EntityFieldPlaceholderKind): string {
    return t(PLACEHOLDER_I18N_KEY[kind], { field: label(field) })
  }

  /**
   * 高级查询占位符（日期/时间区间字段标签用 queryLabel）
   * @param field 查询 DTO 属性 camelCase
   * @param kind 占位类型
   */
  function queryPlaceholder(field: string, kind: EntityFieldPlaceholderKind): string {
    return t(PLACEHOLDER_I18N_KEY[kind], { field: queryLabel(field) })
  }

  return {
    t,
    label,
    queryLabel,
    self,
    placeholder,
    queryPlaceholder,
    required: (field: string) => placeholder(field, 'required'),
    select: (field: string) => placeholder(field, 'select'),
    optional: (field: string) => placeholder(field, 'optional'),
  }
}

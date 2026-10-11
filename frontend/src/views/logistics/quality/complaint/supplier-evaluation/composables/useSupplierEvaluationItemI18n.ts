// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/quality/complaint/supplier-evaluation/composables
// 文件名称：useSupplierEvaluationItemI18n.ts
// 功能描述：SupplierEvaluationItem字段清单 + useSupplierEvaluationItemI18n（字段名映射一次，文案由 entity.supplierevaluationitem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SupplierEvaluationItemQuery } from '@/types/logistics/quality/complaint/supplierEvaluationItem.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSupplierEvaluationItemI18nSeedData 一致的实体 slug */
export const SUPPLIEREVALUATIONITEM_ENTITY_SLUG = 'supplierevaluationitem'

/** entity.supplierevaluationitem._self 静态属性（导入组件 entity-i18n-key 等） */
export const SUPPLIEREVALUATIONITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(SUPPLIEREVALUATIONITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SUPPLIEREVALUATIONITEM_LIST_FIELDS = [
  'supplierEvaluationCode',
  'lineNumber',
  'categoryType',
  'itemName',
  'itemDescription',
  'weight',
  'scoringStandard',
  'score',
  'ratingLevel',
  'evaluationComment',
  'existingIssues',
  'improvementRequirement',
  'rectificationRequired',
  'rectificationDeadline',
  'rectificationStatus',
  'isObsolete',
  'remark',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const SUPPLIEREVALUATIONITEM_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'supplierEvaluationCode',
  'lineNumber',
  'categoryType',
  'itemName',
  'itemDescription',
  'weight',
  'scoringStandard',
  'score',
  'ratingLevel',
  'evaluationComment',
  'existingIssues',
  'improvementRequirement',
  'rectificationRequired',
  'rectificationDeadline',
  'rectificationStatus',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const SUPPLIEREVALUATIONITEM_SUMMARY_SUM_FIELDS = [
  'categoryType',
  'weight',
  'score',
  'ratingLevel',
  'rectificationRequired',
  'rectificationStatus',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SUPPLIEREVALUATIONITEM_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SupplierEvaluationItemField = keyof typeof SUPPLIEREVALUATIONITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SUPPLIEREVALUATIONITEM_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof SupplierEvaluationItemQuery)[]

export type SupplierEvaluationItemQueryField = (typeof SUPPLIEREVALUATIONITEM_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const SUPPLIEREVALUATIONITEM_QUERY_FIELDS: readonly SupplierEvaluationItemQueryField[] = [...SUPPLIEREVALUATIONITEM_QUERY_STRING_FIELDS]

/**
 * SupplierEvaluationItem字段 i18n：index / supplier-evaluation-item-form 统一入口
 */
export function useSupplierEvaluationItemI18n() {
  const ef = useEntityFieldI18n(SUPPLIEREVALUATIONITEM_ENTITY_SLUG)

  function ph(field: SupplierEvaluationItemField): string {
    return ef.placeholder(field, SUPPLIEREVALUATIONITEM_PLACEHOLDER[field])
  }

  function queryPh(field: SupplierEvaluationItemQueryField, kind: EntityFieldPlaceholderKind): string {
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

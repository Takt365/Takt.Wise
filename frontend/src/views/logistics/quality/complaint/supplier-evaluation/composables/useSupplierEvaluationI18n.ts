// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/quality/complaint/supplier-evaluation/composables
// 文件名称：useSupplierEvaluationI18n.ts
// 功能描述：供应商评价考核主表实体字段清单 + useSupplierEvaluationI18n（字段名映射一次，文案由 entity.supplierevaluation.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SupplierEvaluationQuery } from '@/types/logistics/quality/complaint/supplierEvaluation.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSupplierEvaluationI18nSeedData 一致的实体 slug */
export const SUPPLIEREVALUATION_ENTITY_SLUG = 'supplierevaluation'

/** entity.supplierevaluation._self 静态属性（导入组件 entity-i18n-key 等） */
export const SUPPLIEREVALUATION_SELF_I18N_KEY = buildEntitySelfI18nKey(SUPPLIEREVALUATION_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SUPPLIEREVALUATION_LIST_FIELDS = [
  'rectificationStatus',
  'remark',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SUPPLIEREVALUATION_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SupplierEvaluationField = keyof typeof SUPPLIEREVALUATION_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SUPPLIEREVALUATION_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof SupplierEvaluationQuery)[]

export type SupplierEvaluationQueryField = (typeof SUPPLIEREVALUATION_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const SUPPLIEREVALUATION_QUERY_FIELDS: readonly SupplierEvaluationQueryField[] = [...SUPPLIEREVALUATION_QUERY_STRING_FIELDS]

/**
 * 供应商评价考核主表实体字段 i18n：index / supplier-evaluation-form 统一入口
 */
export function useSupplierEvaluationI18n() {
  const ef = useEntityFieldI18n(SUPPLIEREVALUATION_ENTITY_SLUG)

  function ph(field: SupplierEvaluationField): string {
    return ef.placeholder(field, SUPPLIEREVALUATION_PLACEHOLDER[field])
  }

  function queryPh(field: SupplierEvaluationQueryField, kind: EntityFieldPlaceholderKind): string {
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

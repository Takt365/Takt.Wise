// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/quality/operation/inspection-standard/composables
// 文件名称：useInspectionStandardI18n.ts
// 功能描述：检验标准实体字段清单 + useInspectionStandardI18n（字段名映射一次，文案由 entity.inspectionstandard.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { InspectionStandardQuery } from '@/types/logistics/quality/operation/inspectionStandard.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktInspectionStandardI18nSeedData 一致的实体 slug */
export const INSPECTIONSTANDARD_ENTITY_SLUG = 'inspectionstandard'

/** entity.inspectionstandard._self 静态属性（导入组件 entity-i18n-key 等） */
export const INSPECTIONSTANDARD_SELF_I18N_KEY = buildEntitySelfI18nKey(INSPECTIONSTANDARD_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const INSPECTIONSTANDARD_LIST_FIELDS = [
  'standardCode',
  'standardName',
  'inspectionType',
  'materialCategoryCode',
  'materialCategoryName',
  'samplingSchemeCode',
  'samplingSchemeName',
  'standardDescription',
  'standardStatus',
  'remark',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const INSPECTIONSTANDARD_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type InspectionStandardField = keyof typeof INSPECTIONSTANDARD_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const INSPECTIONSTANDARD_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof InspectionStandardQuery)[]

export type InspectionStandardQueryField = (typeof INSPECTIONSTANDARD_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const INSPECTIONSTANDARD_QUERY_FIELDS: readonly InspectionStandardQueryField[] = [...INSPECTIONSTANDARD_QUERY_STRING_FIELDS]

/**
 * 检验标准实体字段 i18n：index / inspection-standard-form 统一入口
 */
export function useInspectionStandardI18n() {
  const ef = useEntityFieldI18n(INSPECTIONSTANDARD_ENTITY_SLUG)

  function ph(field: InspectionStandardField): string {
    return ef.placeholder(field, INSPECTIONSTANDARD_PLACEHOLDER[field])
  }

  function queryPh(field: InspectionStandardQueryField, kind: EntityFieldPlaceholderKind): string {
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

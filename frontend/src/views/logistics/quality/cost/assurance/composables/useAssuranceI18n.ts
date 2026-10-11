// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/quality/cost/assurance/composables
// 文件名称：useAssuranceI18n.ts
// 功能描述：品质业务主表字段清单 + useQualityAssuranceI18n（字段名映射一次，文案由 entity.qualityassurance.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { QualityAssuranceQuery } from '@/types/logistics/quality/cost/assurance.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktQualityAssuranceI18nSeedData 一致的实体 slug */
export const QUALITYASSURANCE_ENTITY_SLUG = 'qualityassurance'

/** entity.qualityassurance._self 静态属性（导入组件 entity-i18n-key 等） */
export const QUALITYASSURANCE_SELF_I18N_KEY = buildEntitySelfI18nKey(QUALITYASSURANCE_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const QUALITYASSURANCE_LIST_FIELDS = [
  'qualityAssuranceCode',
  'assuranceMonth',
  'customerName1',
  'debitNoteCode',
  'recordGroupCode',
  'recordGroupName',
  'totalQualityCost',
  'currencyCode',
  'remark',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const QUALITYASSURANCE_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type QualityAssuranceField = keyof typeof QUALITYASSURANCE_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const QUALITYASSURANCE_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof QualityAssuranceQuery)[]

export type QualityAssuranceQueryField = (typeof QUALITYASSURANCE_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const QUALITYASSURANCE_QUERY_FIELDS: readonly QualityAssuranceQueryField[] = [...QUALITYASSURANCE_QUERY_STRING_FIELDS]

/**
 * 品质业务主表字段 i18n：index / assurance-form 统一入口
 */
export function useQualityAssuranceI18n() {
  const ef = useEntityFieldI18n(QUALITYASSURANCE_ENTITY_SLUG)

  function ph(field: QualityAssuranceField): string {
    return ef.placeholder(field, QUALITYASSURANCE_PLACEHOLDER[field])
  }

  function queryPh(field: QualityAssuranceQueryField, kind: EntityFieldPlaceholderKind): string {
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

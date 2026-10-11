// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/quality/cost/incident/composables
// 文件名称：useIncidentI18n.ts
// 功能描述：品质事故主表字段清单 + useQualityIncidentI18n（字段名映射一次，文案由 entity.qualityincident.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { QualityIncidentQuery } from '@/types/logistics/quality/cost/incident.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktQualityIncidentI18nSeedData 一致的实体 slug */
export const QUALITYINCIDENT_ENTITY_SLUG = 'qualityincident'

/** entity.qualityincident._self 静态属性（导入组件 entity-i18n-key 等） */
export const QUALITYINCIDENT_SELF_I18N_KEY = buildEntitySelfI18nKey(QUALITYINCIDENT_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const QUALITYINCIDENT_LIST_FIELDS = [
  'qualityIncidentCode',
  'incidentDate',
  'indirectManpowerCostPerMinute',
  'model',
  'incidentReason',
  'totalScrapQuantity',
  'totalScrapCost',
  'currencyCode',
  'remark',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const QUALITYINCIDENT_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type QualityIncidentField = keyof typeof QUALITYINCIDENT_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const QUALITYINCIDENT_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof QualityIncidentQuery)[]

export type QualityIncidentQueryField = (typeof QUALITYINCIDENT_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const QUALITYINCIDENT_QUERY_FIELDS: readonly QualityIncidentQueryField[] = [...QUALITYINCIDENT_QUERY_STRING_FIELDS]

/**
 * 品质事故主表字段 i18n：index / incident-form 统一入口
 */
export function useQualityIncidentI18n() {
  const ef = useEntityFieldI18n(QUALITYINCIDENT_ENTITY_SLUG)

  function ph(field: QualityIncidentField): string {
    return ef.placeholder(field, QUALITYINCIDENT_PLACEHOLDER[field])
  }

  function queryPh(field: QualityIncidentQueryField, kind: EntityFieldPlaceholderKind): string {
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

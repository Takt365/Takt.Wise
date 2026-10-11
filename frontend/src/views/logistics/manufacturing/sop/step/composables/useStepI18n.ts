// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/sop/step/composables
// 文件名称：useStepI18n.ts
// 功能描述：SOP 工步实体字段清单 + useSopStepI18n（字段名映射一次，文案由 entity.sopstep.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { SopStepQuery } from '@/types/logistics/manufacturing/sop/step.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktSopStepI18nSeedData 一致的实体 slug */
export const SOPSTEP_ENTITY_SLUG = 'sopstep'

/** entity.sopstep._self 静态属性（导入组件 entity-i18n-key 等） */
export const SOPSTEP_SELF_I18N_KEY = buildEntitySelfI18nKey(SOPSTEP_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const SOPSTEP_LIST_FIELDS = [
  'contentId',
  'stepNo',
  'stepTitle',
  'stepDescription',
  'safetyAlert',
  'safetyPopupRequired',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const SOPSTEP_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  contentId: 'select',
  stepNo: 'select',
  stepTitle: 'required',
  stepDescription: 'optional',
  safetyAlert: 'optional',
  safetyPopupRequired: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type SopStepField = keyof typeof SOPSTEP_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const SOPSTEP_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'contentId',
  'stepTitle',
  'stepDescription',
  'safetyAlert',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof SopStepQuery)[]

export type SopStepQueryField =
  | (typeof SOPSTEP_QUERY_STRING_FIELDS)[number]
  | 'stepNo' | 'safetyPopupRequired'

/** 高级查询抽屉全部字段（含数值） */
export const SOPSTEP_QUERY_FIELDS: readonly SopStepQueryField[] = [
  ...SOPSTEP_QUERY_STRING_FIELDS,
  'stepNo',
  'safetyPopupRequired',
]

/**
 * SOP 工步实体字段 i18n：index / step-form 统一入口
 */
export function useSopStepI18n() {
  const ef = useEntityFieldI18n(SOPSTEP_ENTITY_SLUG)

  function ph(field: SopStepField): string {
    return ef.placeholder(field, SOPSTEP_PLACEHOLDER[field])
  }

  function queryPh(field: SopStepQueryField, kind: EntityFieldPlaceholderKind): string {
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

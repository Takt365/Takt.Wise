// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/output/assy-output/composables
// 文件名称：useAssyOutputDetailI18n.ts
// 功能描述：AssyOutputDetail字段清单 + useAssyOutputDetailI18n（字段名映射一次，文案由 entity.assyoutputdetail.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { AssyOutputDetailQuery } from '@/types/logistics/manufacturing/output/assyOutputDetail.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktAssyOutputDetailI18nSeedData 一致的实体 slug */
export const ASSYOUTPUTDETAIL_ENTITY_SLUG = 'assyoutputdetail'

/** entity.assyoutputdetail._self 静态属性（导入组件 entity-i18n-key 等） */
export const ASSYOUTPUTDETAIL_SELF_I18N_KEY = buildEntitySelfI18nKey(ASSYOUTPUTDETAIL_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const ASSYOUTPUTDETAIL_LIST_FIELDS = [
  'assyOutputId',
  'prodOrderCode',
  'lineNumber',
  'timePeriod',
  'stdCapacity',
  'prodActualQty',
  'downtimeMinutes',
  'downtimeReason',
  'downtimeDescription',
  'unachievedReason',
  'unachievedDescription',
  'inputMinutes',
  'actualMinutes',
  'indirectMinutes',
  'confirmMinutes',
  'mixedProd',
  'achievementRate',
  'isObsolete',
] as const

/** 主表弹窗内嵌子表可编辑列（不含主键/主表冗余只读派生列） */
export const ASSYOUTPUTDETAIL_EMBEDDED_TABLE_FIELDS = [
  'lineNumber',
  'timePeriod',
  'stdCapacity',
  'prodActualQty',
  'downtimeMinutes',
  'downtimeReason',
  'downtimeDescription',
  'unachievedReason',
  'unachievedDescription',
  'confirmMinutes',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const ASSYOUTPUTDETAIL_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'assyOutputId',
  'prodOrderCode',
  'lineNumber',
  'timePeriod',
  'stdCapacity',
  'prodActualQty',
  'downtimeMinutes',
  'downtimeReason',
  'downtimeDescription',
  'unachievedReason',
  'unachievedDescription',
  'inputMinutes',
  'actualMinutes',
  'indirectMinutes',
  'confirmMinutes',
  'mixedProd',
  'achievementRate',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const ASSYOUTPUTDETAIL_SUMMARY_SUM_FIELDS = [
  'stdCapacity',
  'prodActualQty',
  'downtimeMinutes',
  'inputMinutes',
  'actualMinutes',
  'indirectMinutes',
  'confirmMinutes',
  'mixedProd',
  'achievementRate',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const ASSYOUTPUTDETAIL_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  prodOrderCode: 'optional',
  lineNumber: 'select',
  timePeriod: 'optional',
  stdCapacity: 'optional',
  prodActualQty: 'select',
  downtimeMinutes: 'select',
  downtimeReason: 'optional',
  downtimeDescription: 'optional',
  unachievedReason: 'optional',
  unachievedDescription: 'optional',
  inputMinutes: 'optional',
  actualMinutes: 'optional',
  indirectMinutes: 'optional',
  confirmMinutes: 'select',
  mixedProd: 'select',
  achievementRate: 'optional',
  isObsolete: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type AssyOutputDetailField = keyof typeof ASSYOUTPUTDETAIL_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const ASSYOUTPUTDETAIL_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'prodOrderCode',
  'timePeriod',
  'downtimeReason',
  'downtimeDescription',
  'unachievedReason',
  'unachievedDescription',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof AssyOutputDetailQuery)[]

export type AssyOutputDetailQueryField =
  | (typeof ASSYOUTPUTDETAIL_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'stdCapacity' | 'prodActualQty' | 'downtimeMinutes' | 'inputMinutes' | 'actualMinutes' | 'indirectMinutes' | 'confirmMinutes' | 'mixedProd' | 'achievementRate' | 'isObsolete'

/** 高级查询抽屉全部字段（含数值） */
export const ASSYOUTPUTDETAIL_QUERY_FIELDS: readonly AssyOutputDetailQueryField[] = [
  ...ASSYOUTPUTDETAIL_QUERY_STRING_FIELDS,
  'lineNumber',
  'stdCapacity',
  'prodActualQty',
  'downtimeMinutes',
  'inputMinutes',
  'actualMinutes',
  'indirectMinutes',
  'confirmMinutes',
  'mixedProd',
  'achievementRate',
  'isObsolete',
]

/**
 * AssyOutputDetail字段 i18n：index / assy-output-detail-form 统一入口
 */
export function useAssyOutputDetailI18n() {
  const ef = useEntityFieldI18n(ASSYOUTPUTDETAIL_ENTITY_SLUG)

  function ph(field: AssyOutputDetailField): string {
    return ef.placeholder(field, ASSYOUTPUTDETAIL_PLACEHOLDER[field])
  }

  function queryPh(field: AssyOutputDetailQueryField, kind: EntityFieldPlaceholderKind): string {
    return ef.queryPlaceholder(field, kind)
  }

  /** 子表标准产能列提示 */
  function stdCapacityHint(): string {
    return ef.t('logistics.manufacturing.output.assy-output.page.detailstdcapacityhint')
  }

  /** 子表报工工时列提示 */
  function confirmMinutesHint(): string {
    return ef.t('logistics.manufacturing.output.assy-output.page.confirmminuteshint')
  }

  return {
    t: ef.t,
    label: ef.label,
    queryLabel: ef.queryLabel,
    queryPh,
    self: ef.self,
    ph,
    stdCapacityHint,
    confirmMinutesHint,
  }
}

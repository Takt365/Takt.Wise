// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/output/pcba-output/composables
// 文件名称：usePcbaOutputI18n.ts
// 功能描述：PCBA日报实体 达成率字段清单 + usePcbaOutputI18n（字段名映射一次，文案由 entity.pcbaoutput.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PcbaOutputQuery } from '@/types/logistics/manufacturing/output/pcbaOutput.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktPcbaOutputI18nSeedData 一致的实体 slug */
export const PCBAOUTPUT_ENTITY_SLUG = 'pcbaoutput'

/** entity.pcbaoutput._self 静态属性（导入组件 entity-i18n-key 等） */
export const PCBAOUTPUT_SELF_I18N_KEY = buildEntitySelfI18nKey(PCBAOUTPUT_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PCBAOUTPUT_LIST_FIELDS = [
  'prodCategory',
  'prodDate',
  'prodOrderType',
  'prodOrderCode',
  'modelCode',
  'materialCode',
  'batchCode',
  'prodOrderQty',
  'serialCode',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PCBAOUTPUT_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  prodCategory: 'select',
  prodDate: 'select',
  prodOrderType: 'optional',
  prodOrderCode: 'select',
  modelCode: 'optional',
  materialCode: 'optional',
  batchCode: 'optional',
  prodOrderQty: 'optional',
  serialCode: 'optional',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type PcbaOutputField = keyof typeof PCBAOUTPUT_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PCBAOUTPUT_QUERY_STRING_FIELDS = [
  'cultureCode',
  'prodCategory',
  'prodDateStart',
  'prodDateEnd',
  'prodOrderType',
  'prodOrderCode',
  'modelCode',
  'materialCode',
  'batchCode',
  'serialCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof PcbaOutputQuery)[]

export type PcbaOutputQueryField =
  | (typeof PCBAOUTPUT_QUERY_STRING_FIELDS)[number]
  | 'prodOrderQty'

/** 高级查询抽屉全部字段（含数值） */
export const PCBAOUTPUT_QUERY_FIELDS: readonly PcbaOutputQueryField[] = [
  ...PCBAOUTPUT_QUERY_STRING_FIELDS,
  'prodOrderQty',
]

/**
 * PCBA日报实体 达成率字段 i18n：index / pcba-output-form 统一入口
 */
export function usePcbaOutputI18n() {
  const ef = useEntityFieldI18n(PCBAOUTPUT_ENTITY_SLUG)

  function ph(field: PcbaOutputField): string {
    return ef.placeholder(field, PCBAOUTPUT_PLACEHOLDER[field])
  }

  function queryPh(field: PcbaOutputQueryField, kind: EntityFieldPlaceholderKind): string {
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

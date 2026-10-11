// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/labor-hour/pcba-mi-labor-hour/composables
// 文件名称：usePcbaMiLaborHourI18n.ts
// 功能描述：PCBA手插工数统计实体字段清单 + usePcbaMiLaborHourI18n（字段名映射一次，文案由 entity.pcbamilaborhour.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PcbaMiLaborHourQuery } from '@/types/logistics/manufacturing/labor-hour/pcbaMiLaborHour.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktPcbaMiLaborHourI18nSeedData 一致的实体 slug */
export const PCBAMILABORHOUR_ENTITY_SLUG = 'pcbamilaborhour'

/** entity.pcbamilaborhour._self 静态属性（导入组件 entity-i18n-key 等） */
export const PCBAMILABORHOUR_SELF_I18N_KEY = buildEntitySelfI18nKey(PCBAMILABORHOUR_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PCBAMILABORHOUR_LIST_FIELDS = [
  'prodDate',
  'TeamCode',
  'shiftNo',
  'stdCapacity',
  'prodActualQty',
  'inputMinutes',
  'downtimeMinutes',
  'confirmMinutes',
  'actualMinutes',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PCBAMILABORHOUR_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  prodDate: 'select',
  TeamCode: 'select',
  shiftNo: 'select',
  stdCapacity: 'select',
  prodActualQty: 'select',
  inputMinutes: 'select',
  downtimeMinutes: 'select',
  confirmMinutes: 'select',
  actualMinutes: 'select',
  extField: 'optional',
  remark: 'optional',
  plantCode: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type PcbaMiLaborHourField = keyof typeof PCBAMILABORHOUR_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PCBAMILABORHOUR_QUERY_STRING_FIELDS = [
  'prodDateStart',
  'prodDateEnd',
  'TeamCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof PcbaMiLaborHourQuery)[]

export type PcbaMiLaborHourQueryField =
  | (typeof PCBAMILABORHOUR_QUERY_STRING_FIELDS)[number]
  | 'shiftNo' | 'stdCapacity' | 'prodActualQty' | 'inputMinutes' | 'downtimeMinutes' | 'confirmMinutes' | 'actualMinutes'

/** 高级查询抽屉全部字段（含数值） */
export const PCBAMILABORHOUR_QUERY_FIELDS: readonly PcbaMiLaborHourQueryField[] = [
  ...PCBAMILABORHOUR_QUERY_STRING_FIELDS,
  'shiftNo',
  'stdCapacity',
  'prodActualQty',
  'inputMinutes',
  'downtimeMinutes',
  'confirmMinutes',
  'actualMinutes',
]

/**
 * PCBA手插工数统计实体字段 i18n：index / pcba-mi-labor-hour-form 统一入口
 */
export function usePcbaMiLaborHourI18n() {
  const ef = useEntityFieldI18n(PCBAMILABORHOUR_ENTITY_SLUG)

  function ph(field: PcbaMiLaborHourField): string {
    return ef.placeholder(field, PCBAMILABORHOUR_PLACEHOLDER[field])
  }

  function queryPh(field: PcbaMiLaborHourQueryField, kind: EntityFieldPlaceholderKind): string {
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

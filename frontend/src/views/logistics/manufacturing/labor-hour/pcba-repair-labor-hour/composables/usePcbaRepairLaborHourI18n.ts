// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/labor-hour/pcba-repair-labor-hour/composables
// 文件名称：usePcbaRepairLaborHourI18n.ts
// 功能描述：PCBA改修工数统计实体字段清单 + usePcbaRepairLaborHourI18n（字段名映射一次，文案由 entity.pcbarepairlaborhour.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PcbaRepairLaborHourQuery } from '@/types/logistics/manufacturing/labor-hour/pcbaRepairLaborHour.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktPcbaRepairLaborHourI18nSeedData 一致的实体 slug */
export const PCBAREPAIRLABORHOUR_ENTITY_SLUG = 'pcbarepairlaborhour'

/** entity.pcbarepairlaborhour._self 静态属性（导入组件 entity-i18n-key 等） */
export const PCBAREPAIRLABORHOUR_SELF_I18N_KEY = buildEntitySelfI18nKey(PCBAREPAIRLABORHOUR_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PCBAREPAIRLABORHOUR_LIST_FIELDS = [
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
export const PCBAREPAIRLABORHOUR_PLACEHOLDER = {
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
export type PcbaRepairLaborHourField = keyof typeof PCBAREPAIRLABORHOUR_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PCBAREPAIRLABORHOUR_QUERY_STRING_FIELDS = [
  'prodDateStart',
  'prodDateEnd',
  'TeamCode',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof PcbaRepairLaborHourQuery)[]

export type PcbaRepairLaborHourQueryField =
  | (typeof PCBAREPAIRLABORHOUR_QUERY_STRING_FIELDS)[number]
  | 'shiftNo' | 'stdCapacity' | 'prodActualQty' | 'inputMinutes' | 'downtimeMinutes' | 'confirmMinutes' | 'actualMinutes'

/** 高级查询抽屉全部字段（含数值） */
export const PCBAREPAIRLABORHOUR_QUERY_FIELDS: readonly PcbaRepairLaborHourQueryField[] = [
  ...PCBAREPAIRLABORHOUR_QUERY_STRING_FIELDS,
  'shiftNo',
  'stdCapacity',
  'prodActualQty',
  'inputMinutes',
  'downtimeMinutes',
  'confirmMinutes',
  'actualMinutes',
]

/**
 * PCBA改修工数统计实体字段 i18n：index / pcba-repair-labor-hour-form 统一入口
 */
export function usePcbaRepairLaborHourI18n() {
  const ef = useEntityFieldI18n(PCBAREPAIRLABORHOUR_ENTITY_SLUG)

  function ph(field: PcbaRepairLaborHourField): string {
    return ef.placeholder(field, PCBAREPAIRLABORHOUR_PLACEHOLDER[field])
  }

  function queryPh(field: PcbaRepairLaborHourQueryField, kind: EntityFieldPlaceholderKind): string {
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

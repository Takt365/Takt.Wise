// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/defect/pcba-inspection/composables
// 文件名称：usePcbaInspectionI18n.ts
// 功能描述：PCBA检查日报实体 不良率字段清单 + usePcbaInspectionI18n（字段名映射一次，文案由 entity.pcbainspection.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { PcbaInspectionQuery } from '@/types/logistics/manufacturing/defect/pcbaInspection.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktPcbaInspectionI18nSeedData 一致的实体 slug */
export const PCBAINSPECTION_ENTITY_SLUG = 'pcbainspection'

/** entity.pcbainspection._self 静态属性（导入组件 entity-i18n-key 等） */
export const PCBAINSPECTION_SELF_I18N_KEY = buildEntitySelfI18nKey(PCBAINSPECTION_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const PCBAINSPECTION_LIST_FIELDS = [
  'prodCategory',
  'prodOrderType',
  'prodOrderCode',
  'prodOrderQty',
  'modelCode',
  'batchCode',
  'materialCode',
  'remark',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const PCBAINSPECTION_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type PcbaInspectionField = keyof typeof PCBAINSPECTION_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const PCBAINSPECTION_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof PcbaInspectionQuery)[]

export type PcbaInspectionQueryField = (typeof PCBAINSPECTION_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const PCBAINSPECTION_QUERY_FIELDS: readonly PcbaInspectionQueryField[] = [...PCBAINSPECTION_QUERY_STRING_FIELDS]

/**
 * PCBA检查日报实体 不良率字段 i18n：index / pcba-inspection-form 统一入口
 */
export function usePcbaInspectionI18n() {
  const ef = useEntityFieldI18n(PCBAINSPECTION_ENTITY_SLUG)

  function ph(field: PcbaInspectionField): string {
    return ef.placeholder(field, PCBAINSPECTION_PLACEHOLDER[field])
  }

  function queryPh(field: PcbaInspectionQueryField, kind: EntityFieldPlaceholderKind): string {
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

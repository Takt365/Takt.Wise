// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/quality/operation/fqc-order/composables
// 文件名称：useFqcOrderItemI18n.ts
// 功能描述：FqcOrderItem字段清单 + useFqcOrderItemI18n（字段名映射一次，文案由 entity.fqcorderitem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { FqcOrderItemQuery } from '@/types/logistics/quality/operation/fqcOrderItem.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktFqcOrderItemI18nSeedData 一致的实体 slug */
export const FQCORDERITEM_ENTITY_SLUG = 'fqcorderitem'

/** entity.fqcorderitem._self 静态属性（导入组件 entity-i18n-key 等） */
export const FQCORDERITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(FQCORDERITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const FQCORDERITEM_LIST_FIELDS = [
  'fqcOrderCode',
  'lineNumber',
  'materialCode',
  'materialDescription',
  'batchCode',
  'warehouseQuantity',
  'standardCode',
  'samplingSchemeCode',
  'inspectionMethod',
  'sampleQuantity',
  'qualifiedQuantity',
  'unqualifiedQuantity',
  'inspectionReturnQuantity',
  'sampleSerialCode',
  'inspectionDescription',
  'inspectorBy',
  'inspectionDate',
  'judgeStatus',
  'isObsolete',
  'remark',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const FQCORDERITEM_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'fqcOrderCode',
  'lineNumber',
  'materialCode',
  'materialDescription',
  'batchCode',
  'warehouseQuantity',
  'standardCode',
  'samplingSchemeCode',
  'inspectionMethod',
  'sampleQuantity',
  'qualifiedQuantity',
  'unqualifiedQuantity',
  'inspectionReturnQuantity',
  'sampleSerialCode',
  'inspectionDescription',
  'inspectorBy',
  'inspectionDate',
  'judgeStatus',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const FQCORDERITEM_SUMMARY_SUM_FIELDS = [
  'warehouseQuantity',
  'inspectionMethod',
  'sampleQuantity',
  'qualifiedQuantity',
  'unqualifiedQuantity',
  'inspectionReturnQuantity',
  'judgeStatus',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const FQCORDERITEM_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type FqcOrderItemField = keyof typeof FQCORDERITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const FQCORDERITEM_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof FqcOrderItemQuery)[]

export type FqcOrderItemQueryField = (typeof FQCORDERITEM_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const FQCORDERITEM_QUERY_FIELDS: readonly FqcOrderItemQueryField[] = [...FQCORDERITEM_QUERY_STRING_FIELDS]

/**
 * FqcOrderItem字段 i18n：index / fqc-order-item-form 统一入口
 */
export function useFqcOrderItemI18n() {
  const ef = useEntityFieldI18n(FQCORDERITEM_ENTITY_SLUG)

  function ph(field: FqcOrderItemField): string {
    return ef.placeholder(field, FQCORDERITEM_PLACEHOLDER[field])
  }

  function queryPh(field: FqcOrderItemQueryField, kind: EntityFieldPlaceholderKind): string {
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

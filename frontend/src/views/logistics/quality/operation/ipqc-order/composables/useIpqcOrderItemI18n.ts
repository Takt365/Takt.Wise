// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/quality/operation/ipqc-order/composables
// 文件名称：useIpqcOrderItemI18n.ts
// 功能描述：IpqcOrderItem字段清单 + useIpqcOrderItemI18n（字段名映射一次，文案由 entity.ipqcorderitem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { IpqcOrderItemQuery } from '@/types/logistics/quality/operation/ipqcOrderItem.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktIpqcOrderItemI18nSeedData 一致的实体 slug */
export const IPQCORDERITEM_ENTITY_SLUG = 'ipqcorderitem'

/** entity.ipqcorderitem._self 静态属性（导入组件 entity-i18n-key 等） */
export const IPQCORDERITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(IPQCORDERITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const IPQCORDERITEM_LIST_FIELDS = [
  'ipqcOrderId',
  'ipqcOrderCode',
  'lineNumber',
  'materialCode',
  'materialDescription',
  'batchCode',
  'productionQuantity',
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
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const IPQCORDERITEM_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'ipqcOrderId',
  'ipqcOrderCode',
  'lineNumber',
  'materialCode',
  'materialDescription',
  'batchCode',
  'productionQuantity',
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
export const IPQCORDERITEM_SUMMARY_SUM_FIELDS = [
  'productionQuantity',
  'inspectionMethod',
  'sampleQuantity',
  'qualifiedQuantity',
  'unqualifiedQuantity',
  'inspectionReturnQuantity',
  'judgeStatus',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const IPQCORDERITEM_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  lineNumber: 'select',
  materialCode: 'select',
  materialDescription: 'optional',
  batchCode: 'optional',
  productionQuantity: 'select',
  standardCode: 'select',
  samplingSchemeCode: 'select',
  inspectionMethod: 'select',
  sampleQuantity: 'select',
  qualifiedQuantity: 'select',
  unqualifiedQuantity: 'select',
  inspectionReturnQuantity: 'select',
  sampleSerialCode: 'optional',
  inspectionDescription: 'optional',
  inspectorBy: 'required',
  inspectionDate: 'select',
  judgeStatus: 'select',
  isObsolete: 'select',
  defectHandlings: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type IpqcOrderItemField = keyof typeof IPQCORDERITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const IPQCORDERITEM_QUERY_STRING_FIELDS = [
  'plantCode',
  'ipqcOrderCode',
  'materialCode',
  'materialDescription',
  'batchCode',
  'standardCode',
  'samplingSchemeCode',
  'sampleSerialCode',
  'inspectionDescription',
  'inspectorBy',
  'inspectionDateStart',
  'inspectionDateEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof IpqcOrderItemQuery)[]

export type IpqcOrderItemQueryField =
  | (typeof IPQCORDERITEM_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'productionQuantity' | 'inspectionMethod' | 'sampleQuantity' | 'qualifiedQuantity' | 'unqualifiedQuantity' | 'inspectionReturnQuantity' | 'judgeStatus' | 'isObsolete'

/** 高级查询抽屉全部字段（含数值） */
export const IPQCORDERITEM_QUERY_FIELDS: readonly IpqcOrderItemQueryField[] = [
  ...IPQCORDERITEM_QUERY_STRING_FIELDS,
  'lineNumber',
  'productionQuantity',
  'inspectionMethod',
  'sampleQuantity',
  'qualifiedQuantity',
  'unqualifiedQuantity',
  'inspectionReturnQuantity',
  'judgeStatus',
  'isObsolete',
]

/**
 * IpqcOrderItem字段 i18n：index / ipqc-order-item-form 统一入口
 */
export function useIpqcOrderItemI18n() {
  const ef = useEntityFieldI18n(IPQCORDERITEM_ENTITY_SLUG)

  function ph(field: IpqcOrderItemField): string {
    return ef.placeholder(field, IPQCORDERITEM_PLACEHOLDER[field])
  }

  function queryPh(field: IpqcOrderItemQueryField, kind: EntityFieldPlaceholderKind): string {
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

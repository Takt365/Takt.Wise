// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/bom/bill-of-material/composables
// 文件名称：useBillOfMaterialItemI18n.ts
// 功能描述：BillOfMaterialItem字段清单 + useBillOfMaterialItemI18n（字段名映射一次，文案由 entity.billofmaterialitem.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { BillOfMaterialItemQuery } from '@/types/logistics/manufacturing/bom/billOfMaterialItem.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktBillOfMaterialItemI18nSeedData 一致的实体 slug */
export const BILLOFMATERIALITEM_ENTITY_SLUG = 'billofmaterialitem'

/** entity.billofmaterialitem._self 静态属性（导入组件 entity-i18n-key 等） */
export const BILLOFMATERIALITEM_SELF_I18N_KEY = buildEntitySelfI18nKey(BILLOFMATERIALITEM_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const BILLOFMATERIALITEM_LIST_FIELDS = [
  'billOfMaterialId',
  'bomCode',
  'lineNumber',
  'materialCode',
  'materialDescription',
  'usageQuantity',
  'materialUnit',
  'scrapRate',
  'actualUsageQuantity',
  'operationSeq',
  'workCenter',
  'position',
  'substituteGroup',
  'substitutePriority',
  'isOptional',
  'isPhantom',
  'isObsolete',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const BILLOFMATERIALITEM_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'billOfMaterialId',
  'bomCode',
  'lineNumber',
  'materialCode',
  'materialDescription',
  'usageQuantity',
  'materialUnit',
  'scrapRate',
  'actualUsageQuantity',
  'operationSeq',
  'workCenter',
  'position',
  'substituteGroup',
  'substitutePriority',
  'isOptional',
  'isPhantom',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const BILLOFMATERIALITEM_SUMMARY_SUM_FIELDS = [
  'usageQuantity',
  'scrapRate',
  'actualUsageQuantity',
  'operationSeq',
  'substitutePriority',
  'isOptional',
  'isPhantom',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const BILLOFMATERIALITEM_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  plantCode: 'optional',
  bomCode: 'optional',
  lineNumber: 'select',
  materialCode: 'select',
  materialDescription: 'optional',
  usageQuantity: 'select',
  materialUnit: 'select',
  scrapRate: 'select',
  actualUsageQuantity: 'select',
  operationSeq: 'select',
  workCenter: 'optional',
  position: 'optional',
  substituteGroup: 'optional',
  substitutePriority: 'select',
  isOptional: 'select',
  isPhantom: 'select',
  isObsolete: 'select',
  substitutes: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type BillOfMaterialItemField = keyof typeof BILLOFMATERIALITEM_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const BILLOFMATERIALITEM_QUERY_STRING_FIELDS = [
  'cultureCode',
  'plantCode',
  'bomCode',
  'materialCode',
  'materialDescription',
  'materialUnit',
  'workCenter',
  'position',
  'substituteGroup',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof BillOfMaterialItemQuery)[]

export type BillOfMaterialItemQueryField =
  | (typeof BILLOFMATERIALITEM_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'usageQuantity' | 'scrapRate' | 'actualUsageQuantity' | 'operationSeq' | 'substitutePriority' | 'isOptional' | 'isPhantom' | 'isObsolete'

/** 高级查询抽屉全部字段（含数值） */
export const BILLOFMATERIALITEM_QUERY_FIELDS: readonly BillOfMaterialItemQueryField[] = [
  ...BILLOFMATERIALITEM_QUERY_STRING_FIELDS,
  'lineNumber',
  'usageQuantity',
  'scrapRate',
  'actualUsageQuantity',
  'operationSeq',
  'substitutePriority',
  'isOptional',
  'isPhantom',
  'isObsolete',
]

/**
 * BillOfMaterialItem字段 i18n：index / bill-of-material-item-form 统一入口
 */
export function useBillOfMaterialItemI18n() {
  const ef = useEntityFieldI18n(BILLOFMATERIALITEM_ENTITY_SLUG)

  function ph(field: BillOfMaterialItemField): string {
    return ef.placeholder(field, BILLOFMATERIALITEM_PLACEHOLDER[field])
  }

  function queryPh(field: BillOfMaterialItemQueryField, kind: EntityFieldPlaceholderKind): string {
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

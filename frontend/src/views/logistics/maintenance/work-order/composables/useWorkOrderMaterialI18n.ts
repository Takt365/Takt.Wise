// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/maintenance/work-order/composables
// 文件名称：useWorkOrderMaterialI18n.ts
// 功能描述：MaintenanceWorkOrderMaterial字段清单 + useMaintenanceWorkOrderMaterialI18n（字段名映射一次，文案由 entity.maintenanceworkordermaterial.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { MaintenanceWorkOrderMaterialQuery } from '@/types/logistics/maintenance/workOrderMaterial.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktMaintenanceWorkOrderMaterialI18nSeedData 一致的实体 slug */
export const MAINTENANCEWORKORDERMATERIAL_ENTITY_SLUG = 'maintenanceworkordermaterial'

/** entity.maintenanceworkordermaterial._self 静态属性（导入组件 entity-i18n-key 等） */
export const MAINTENANCEWORKORDERMATERIAL_SELF_I18N_KEY = buildEntitySelfI18nKey(MAINTENANCEWORKORDERMATERIAL_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const MAINTENANCEWORKORDERMATERIAL_LIST_FIELDS = [
  'maintenanceWorkOrderId',
  'workOrderCode',
  'lineNumber',
  'materialId',
  'materialCode',
  'materialDescription',
  'requiredQuantity',
  'issuedQuantity',
  'materialUnit',
  'unitPrice',
  'amount',
  'warehouseCode',
  'storageLocation',
  'issueStatus',
  'issueTime',
  'isObsolete',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const MAINTENANCEWORKORDERMATERIAL_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'maintenanceWorkOrderId',
  'workOrderCode',
  'lineNumber',
  'materialId',
  'materialCode',
  'materialDescription',
  'requiredQuantity',
  'issuedQuantity',
  'materialUnit',
  'unitPrice',
  'amount',
  'warehouseCode',
  'storageLocation',
  'issueStatus',
  'issueTime',
  'isObsolete',
  'action',
] as const

/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const MAINTENANCEWORKORDERMATERIAL_SUMMARY_SUM_FIELDS = [
  'requiredQuantity',
  'issuedQuantity',
  'unitPrice',
  'amount',
  'issueStatus',
  'isObsolete',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const MAINTENANCEWORKORDERMATERIAL_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  cultureCode: 'optional',
  workOrderCode: 'optional',
  lineNumber: 'select',
  materialId: 'required',
  materialCode: 'required',
  materialDescription: 'optional',
  requiredQuantity: 'select',
  issuedQuantity: 'select',
  materialUnit: 'required',
  unitPrice: 'select',
  amount: 'select',
  warehouseCode: 'optional',
  storageLocation: 'optional',
  issueStatus: 'select',
  issueTime: 'optional',
  isObsolete: 'select',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type MaintenanceWorkOrderMaterialField = keyof typeof MAINTENANCEWORKORDERMATERIAL_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const MAINTENANCEWORKORDERMATERIAL_QUERY_STRING_FIELDS = [
  'workOrderCode',
  'materialId',
  'materialCode',
  'materialDescription',
  'materialUnit',
  'warehouseCode',
  'storageLocation',
  'issueTimeStart',
  'issueTimeEnd',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof MaintenanceWorkOrderMaterialQuery)[]

export type MaintenanceWorkOrderMaterialQueryField =
  | (typeof MAINTENANCEWORKORDERMATERIAL_QUERY_STRING_FIELDS)[number]
  | 'lineNumber' | 'requiredQuantity' | 'issuedQuantity' | 'unitPrice' | 'amount' | 'issueStatus' | 'isObsolete'

/** 高级查询抽屉全部字段（含数值） */
export const MAINTENANCEWORKORDERMATERIAL_QUERY_FIELDS: readonly MaintenanceWorkOrderMaterialQueryField[] = [
  ...MAINTENANCEWORKORDERMATERIAL_QUERY_STRING_FIELDS,
  'lineNumber',
  'requiredQuantity',
  'issuedQuantity',
  'unitPrice',
  'amount',
  'issueStatus',
  'isObsolete',
]

/**
 * MaintenanceWorkOrderMaterial字段 i18n：index / work-order-material-form 统一入口
 */
export function useMaintenanceWorkOrderMaterialI18n() {
  const ef = useEntityFieldI18n(MAINTENANCEWORKORDERMATERIAL_ENTITY_SLUG)

  function ph(field: MaintenanceWorkOrderMaterialField): string {
    return ef.placeholder(field, MAINTENANCEWORKORDERMATERIAL_PLACEHOLDER[field])
  }

  function queryPh(field: MaintenanceWorkOrderMaterialQueryField, kind: EntityFieldPlaceholderKind): string {
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

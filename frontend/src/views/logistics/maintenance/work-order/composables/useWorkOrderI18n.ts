// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/maintenance/work-order/composables
// 文件名称：useWorkOrderI18n.ts
// 功能描述：维护工单实体字段清单 + useMaintenanceWorkOrderI18n（字段名映射一次，文案由 entity.maintenanceworkorder.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { MaintenanceWorkOrderQuery } from '@/types/logistics/maintenance/workOrder.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 TaktMaintenanceWorkOrderI18nSeedData 一致的实体 slug */
export const MAINTENANCEWORKORDER_ENTITY_SLUG = 'maintenanceworkorder'

/** entity.maintenanceworkorder._self 静态属性（导入组件 entity-i18n-key 等） */
export const MAINTENANCEWORKORDER_SELF_I18N_KEY = buildEntitySelfI18nKey(MAINTENANCEWORKORDER_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const MAINTENANCEWORKORDER_LIST_FIELDS = [
  'workOrderCode',
  'maintenanceNotificationId',
  'notificationCode',
  'equipmentId',
  'EquipCode',
  'equipmentName',
  'maintenanceCategory',
  'maintenanceType',
  'workOrderStatus',
  'priority',
  'workCenter',
  'assignedTechnician',
  'maintenanceCompany',
  'plannedStartTime',
  'plannedEndTime',
  'actualStartTime',
  'actualEndTime',
  'faultDescription',
  'maintenanceContent',
  'solution',
  'costCenterId',
  'costCenterCode',
  'costElementId',
  'costElementCode',
  'totalMaterialCost',
  'totalLaborCost',
  'totalOtherCost',
  'totalCost',
  'settlementStatus',
  'settlementTime',
  'completedAt',
  'acceptedBy',
  'acceptedAt',
  'maintenanceResult',
  'nextMaintenanceDate',
  'maintenanceCycleDays',
  'maintenanceImages',
  'maintenanceDocuments',
  'acceptedSummary',
  'isHistoryArchived',
  'remark',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const MAINTENANCEWORKORDER_PLACEHOLDER = {

} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type MaintenanceWorkOrderField = keyof typeof MAINTENANCEWORKORDER_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const MAINTENANCEWORKORDER_QUERY_STRING_FIELDS = [

] as const satisfies readonly (keyof MaintenanceWorkOrderQuery)[]

export type MaintenanceWorkOrderQueryField = (typeof MAINTENANCEWORKORDER_QUERY_STRING_FIELDS)[number]

/** 高级查询抽屉全部字段（含数值） */
export const MAINTENANCEWORKORDER_QUERY_FIELDS: readonly MaintenanceWorkOrderQueryField[] = [...MAINTENANCEWORKORDER_QUERY_STRING_FIELDS]

/**
 * 维护工单实体字段 i18n：index / work-order-form 统一入口
 */
export function useMaintenanceWorkOrderI18n() {
  const ef = useEntityFieldI18n(MAINTENANCEWORKORDER_ENTITY_SLUG)

  function ph(field: MaintenanceWorkOrderField): string {
    return ef.placeholder(field, MAINTENANCEWORKORDER_PLACEHOLDER[field])
  }

  function queryPh(field: MaintenanceWorkOrderQueryField, kind: EntityFieldPlaceholderKind): string {
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

// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/maintenance/work-order/composables
// 文件名称：useWorkOrderMasterContext.ts
// 功能描述：维护工单实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { MaintenanceWorkOrder } from '@/types/logistics/maintenance/workOrder.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type MaintenanceWorkOrderRowRecord = MaintenanceWorkOrder | Record<string, unknown>

/** 主表选中行上下文 */
export interface MaintenanceWorkOrderMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<MaintenanceWorkOrderRowRecord | null>
}

const maintenanceWorkOrderMasterContextKey: InjectionKey<MaintenanceWorkOrderMasterContext> = Symbol('work-orderMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {MaintenanceWorkOrderMasterContext} 主表上下文
 */
export function provideMaintenanceWorkOrderMasterContext(): MaintenanceWorkOrderMasterContext {
  const selecmedMasterRow = ref<MaintenanceWorkOrderRowRecord | null>(null)
  const ctx: MaintenanceWorkOrderMasterContext = { selecmedMasterRow }
  provide(maintenanceWorkOrderMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {MaintenanceWorkOrderMasterContext} 主表上下文
 */
export function useMaintenanceWorkOrderMasterContext(): MaintenanceWorkOrderMasterContext {
  const ctx = inject(maintenanceWorkOrderMasterContextKey)
  if (!ctx) {
    throw new Error('useMaintenanceWorkOrderMasterContext must be used within work-order index')
  }
  return ctx
}

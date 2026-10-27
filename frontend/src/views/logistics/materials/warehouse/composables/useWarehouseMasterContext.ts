// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/materials/warehouse/composables
// 文件名称：useWarehouseMasterContext.ts
// 功能描述：Takt仓库主数据实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { Warehouse } from '@/types/logistics/materials/warehouse.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type WarehouseRowRecord = Warehouse | Record<string, unknown>

/** 主表选中行上下文 */
export interface WarehouseMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<WarehouseRowRecord | null>
}

const warehouseMasterContextKey: InjectionKey<WarehouseMasterContext> = Symbol('warehouseMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {WarehouseMasterContext} 主表上下文
 */
export function provideWarehouseMasterContext(): WarehouseMasterContext {
  const selecmedMasterRow = ref<WarehouseRowRecord | null>(null)
  const ctx: WarehouseMasterContext = { selecmedMasterRow }
  provide(warehouseMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {WarehouseMasterContext} 主表上下文
 */
export function useWarehouseMasterContext(): WarehouseMasterContext {
  const ctx = inject(warehouseMasterContextKey)
  if (!ctx) {
    throw new Error('useWarehouseMasterContext must be used within warehouse index')
  }
  return ctx
}

// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/sales/order/composables
// 文件名称：useOrderMasterContext.ts
// 功能描述：Takt销售订单实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { SalesOrder } from '@/types/logistics/sales/order.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type SalesOrderRowRecord = SalesOrder | Record<string, unknown>

/** 主表选中行上下文 */
export interface SalesOrderMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<SalesOrderRowRecord | null>
}

const salesOrderMasterContextKey: InjectionKey<SalesOrderMasterContext> = Symbol('orderMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {SalesOrderMasterContext} 主表上下文
 */
export function provideSalesOrderMasterContext(): SalesOrderMasterContext {
  const selecmedMasterRow = ref<SalesOrderRowRecord | null>(null)
  const ctx: SalesOrderMasterContext = { selecmedMasterRow }
  provide(salesOrderMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {SalesOrderMasterContext} 主表上下文
 */
export function useSalesOrderMasterContext(): SalesOrderMasterContext {
  const ctx = inject(salesOrderMasterContextKey)
  if (!ctx) {
    throw new Error('useSalesOrderMasterContext must be used within order index')
  }
  return ctx
}

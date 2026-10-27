// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/procurement/purchase-order/composables
// 文件名称：usePurchaseOrderMasterContext.ts
// 功能描述：Takt采购订单实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { PurchaseOrder } from '@/types/logistics/procurement/purchaseOrder.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type PurchaseOrderRowRecord = PurchaseOrder | Record<string, unknown>

/** 主表选中行上下文 */
export interface PurchaseOrderMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<PurchaseOrderRowRecord | null>
}

const purchaseOrderMasterContextKey: InjectionKey<PurchaseOrderMasterContext> = Symbol('purchase-orderMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {PurchaseOrderMasterContext} 主表上下文
 */
export function providePurchaseOrderMasterContext(): PurchaseOrderMasterContext {
  const selecmedMasterRow = ref<PurchaseOrderRowRecord | null>(null)
  const ctx: PurchaseOrderMasterContext = { selecmedMasterRow }
  provide(purchaseOrderMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {PurchaseOrderMasterContext} 主表上下文
 */
export function usePurchaseOrderMasterContext(): PurchaseOrderMasterContext {
  const ctx = inject(purchaseOrderMasterContextKey)
  if (!ctx) {
    throw new Error('usePurchaseOrderMasterContext must be used within purchase-order index')
  }
  return ctx
}

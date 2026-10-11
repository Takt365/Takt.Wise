// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/procurement/purchase-invoice/composables
// 文件名称：usePurchaseInvoiceMasterContext.ts
// 功能描述：Takt采购发票主表实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { PurchaseInvoice } from '@/types/logistics/procurement/purchaseInvoice.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type PurchaseInvoiceRowRecord = PurchaseInvoice | Record<string, unknown>

/** 主表选中行上下文 */
export interface PurchaseInvoiceMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<PurchaseInvoiceRowRecord | null>
}

const purchaseInvoiceMasterContextKey: InjectionKey<PurchaseInvoiceMasterContext> = Symbol('purchase-invoiceMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {PurchaseInvoiceMasterContext} 主表上下文
 */
export function providePurchaseInvoiceMasterContext(): PurchaseInvoiceMasterContext {
  const selectedMasterRow = ref<PurchaseInvoiceRowRecord | null>(null)
  const ctx: PurchaseInvoiceMasterContext = { selectedMasterRow }
  provide(purchaseInvoiceMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {PurchaseInvoiceMasterContext} 主表上下文
 */
export function usePurchaseInvoiceMasterContext(): PurchaseInvoiceMasterContext {
  const ctx = inject(purchaseInvoiceMasterContextKey)
  if (!ctx) {
    throw new Error('usePurchaseInvoiceMasterContext must be used within purchase-invoice index')
  }
  return ctx
}

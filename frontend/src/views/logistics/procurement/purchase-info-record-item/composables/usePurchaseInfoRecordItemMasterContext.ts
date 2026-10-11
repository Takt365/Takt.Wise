// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/procurement/purchase-info-record-item/composables
// 文件名称：usePurchaseInfoRecordItemMasterContext.ts
// 功能描述：Takt采购信息记录行实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { PurchaseInfoRecordItem } from '@/types/logistics/procurement/purchaseInfoRecordItem.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type PurchaseInfoRecordItemRowRecord = PurchaseInfoRecordItem | Record<string, unknown>

/** 主表选中行上下文 */
export interface PurchaseInfoRecordItemMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<PurchaseInfoRecordItemRowRecord | null>
}

const purchaseInfoRecordItemMasterContextKey: InjectionKey<PurchaseInfoRecordItemMasterContext> = Symbol('purchase-price-itemMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {PurchaseInfoRecordItemMasterContext} 主表上下文
 */
export function providePurchaseInfoRecordItemMasterContext(): PurchaseInfoRecordItemMasterContext {
  const selectedMasterRow = ref<PurchaseInfoRecordItemRowRecord | null>(null)
  const ctx: PurchaseInfoRecordItemMasterContext = { selectedMasterRow }
  provide(purchaseInfoRecordItemMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {PurchaseInfoRecordItemMasterContext} 主表上下文
 */
export function usePurchaseInfoRecordItemMasterContext(): PurchaseInfoRecordItemMasterContext {
  const ctx = inject(purchaseInfoRecordItemMasterContextKey)
  if (!ctx) {
    throw new Error('usePurchaseInfoRecordItemMasterContext must be used within purchase-price-item index')
  }
  return ctx
}

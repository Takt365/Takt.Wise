// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/procurement/purchase-info-item/composables
// 文件名称：usePurchaseInfoItemMasterContext.ts
// 功能描述：Takt采购信息行实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { PurchaseInfoItem } from '@/types/logistics/procurement/purchaseInfoItem.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type PurchaseInfoItemRowRecord = PurchaseInfoItem | Record<string, unknown>

/** 主表选中行上下文 */
export interface PurchaseInfoItemMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<PurchaseInfoItemRowRecord | null>
}

const purchaseInfoItemMasterContextKey: InjectionKey<PurchaseInfoItemMasterContext> = Symbol('purchase-price-itemMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {PurchaseInfoItemMasterContext} 主表上下文
 */
export function providePurchaseInfoItemMasterContext(): PurchaseInfoItemMasterContext {
  const selecmedMasterRow = ref<PurchaseInfoItemRowRecord | null>(null)
  const ctx: PurchaseInfoItemMasterContext = { selecmedMasterRow }
  provide(purchaseInfoItemMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {PurchaseInfoItemMasterContext} 主表上下文
 */
export function usePurchaseInfoItemMasterContext(): PurchaseInfoItemMasterContext {
  const ctx = inject(purchaseInfoItemMasterContextKey)
  if (!ctx) {
    throw new Error('usePurchaseInfoItemMasterContext must be used within purchase-price-item index')
  }
  return ctx
}

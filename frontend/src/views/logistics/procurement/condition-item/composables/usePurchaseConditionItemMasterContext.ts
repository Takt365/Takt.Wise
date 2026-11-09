// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/procurement/condition-item/composables
// 文件名称：usePurchaseConditionItemMasterContext.ts
// 功能描述：Takt采购条件行实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { PurchaseConditionItem } from '@/types/logistics/procurement/conditionItem.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type PurchaseConditionItemRowRecord = PurchaseConditionItem | Record<string, unknown>

/** 主表选中行上下文 */
export interface PurchaseConditionItemMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<PurchaseConditionItemRowRecord | null>
}

const purchaseConditionItemMasterContextKey: InjectionKey<PurchaseConditionItemMasterContext> = Symbol('purchase-price-itemMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {PurchaseConditionItemMasterContext} 主表上下文
 */
export function providePurchaseConditionItemMasterContext(): PurchaseConditionItemMasterContext {
  const selecmedMasterRow = ref<PurchaseConditionItemRowRecord | null>(null)
  const ctx: PurchaseConditionItemMasterContext = { selecmedMasterRow }
  provide(purchaseConditionItemMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {PurchaseConditionItemMasterContext} 主表上下文
 */
export function usePurchaseConditionItemMasterContext(): PurchaseConditionItemMasterContext {
  const ctx = inject(purchaseConditionItemMasterContextKey)
  if (!ctx) {
    throw new Error('usePurchaseConditionItemMasterContext must be used within purchase-price-item index')
  }
  return ctx
}

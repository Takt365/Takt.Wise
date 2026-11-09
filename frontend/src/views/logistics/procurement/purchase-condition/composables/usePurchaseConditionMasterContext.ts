// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/procurement/purchase-condition/composables
// 文件名称：usePurchaseConditionMasterContext.ts
// 功能描述：Takt采购条件实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { PurchaseCondition } from '@/types/logistics/procurement/purchaseCondition.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type PurchaseConditionRowRecord = PurchaseCondition | Record<string, unknown>

/** 主表选中行上下文 */
export interface PurchaseConditionMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<PurchaseConditionRowRecord | null>
}

const purchaseConditionMasterContextKey: InjectionKey<PurchaseConditionMasterContext> = Symbol('purchase-conditionMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {PurchaseConditionMasterContext} 主表上下文
 */
export function providePurchaseConditionMasterContext(): PurchaseConditionMasterContext {
  const selecmedMasterRow = ref<PurchaseConditionRowRecord | null>(null)
  const ctx: PurchaseConditionMasterContext = { selecmedMasterRow }
  provide(purchaseConditionMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {PurchaseConditionMasterContext} 主表上下文
 */
export function usePurchaseConditionMasterContext(): PurchaseConditionMasterContext {
  const ctx = inject(purchaseConditionMasterContextKey)
  if (!ctx) {
    throw new Error('usePurchaseConditionMasterContext must be used within purchase-condition index')
  }
  return ctx
}

// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/procurement/purchase-info/composables
// 文件名称：usePurchaseInfoMasterContext.ts
// 功能描述：Takt采购信息实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { PurchaseInfo } from '@/types/logistics/procurement/purchaseInfo.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type PurchaseInfoRowRecord = PurchaseInfo | Record<string, unknown>

/** 主表选中行上下文 */
export interface PurchaseInfoMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<PurchaseInfoRowRecord | null>
}

const purchaseInfoMasterContextKey: InjectionKey<PurchaseInfoMasterContext> = Symbol('purchase-infoMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {PurchaseInfoMasterContext} 主表上下文
 */
export function providePurchaseInfoMasterContext(): PurchaseInfoMasterContext {
  const selectedMasterRow = ref<PurchaseInfoRowRecord | null>(null)
  const ctx: PurchaseInfoMasterContext = { selectedMasterRow }
  provide(purchaseInfoMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {PurchaseInfoMasterContext} 主表上下文
 */
export function usePurchaseInfoMasterContext(): PurchaseInfoMasterContext {
  const ctx = inject(purchaseInfoMasterContextKey)
  if (!ctx) {
    throw new Error('usePurchaseInfoMasterContext must be used within purchase-info index')
  }
  return ctx
}

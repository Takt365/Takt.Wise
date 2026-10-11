// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/procurement/purchase-request/composables
// 文件名称：usePurchaseRequestMasterContext.ts
// 功能描述：Takt采购申请实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { PurchaseRequest } from '@/types/logistics/procurement/purchaseRequest.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type PurchaseRequestRowRecord = PurchaseRequest | Record<string, unknown>

/** 主表选中行上下文 */
export interface PurchaseRequestMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<PurchaseRequestRowRecord | null>
}

const purchaseRequestMasterContextKey: InjectionKey<PurchaseRequestMasterContext> = Symbol('purchase-requestMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {PurchaseRequestMasterContext} 主表上下文
 */
export function providePurchaseRequestMasterContext(): PurchaseRequestMasterContext {
  const selectedMasterRow = ref<PurchaseRequestRowRecord | null>(null)
  const ctx: PurchaseRequestMasterContext = { selectedMasterRow }
  provide(purchaseRequestMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {PurchaseRequestMasterContext} 主表上下文
 */
export function usePurchaseRequestMasterContext(): PurchaseRequestMasterContext {
  const ctx = inject(purchaseRequestMasterContextKey)
  if (!ctx) {
    throw new Error('usePurchaseRequestMasterContext must be used within purchase-request index')
  }
  return ctx
}

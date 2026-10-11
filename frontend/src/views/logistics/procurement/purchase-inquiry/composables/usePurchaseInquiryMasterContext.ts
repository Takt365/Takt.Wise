// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/procurement/purchase-inquiry/composables
// 文件名称：usePurchaseInquiryMasterContext.ts
// 功能描述：采购询价实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { PurchaseInquiry } from '@/types/logistics/procurement/purchaseInquiry.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type PurchaseInquiryRowRecord = PurchaseInquiry | Record<string, unknown>

/** 主表选中行上下文 */
export interface PurchaseInquiryMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<PurchaseInquiryRowRecord | null>
}

const purchaseInquiryMasterContextKey: InjectionKey<PurchaseInquiryMasterContext> = Symbol('purchase-inquiryMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {PurchaseInquiryMasterContext} 主表上下文
 */
export function providePurchaseInquiryMasterContext(): PurchaseInquiryMasterContext {
  const selectedMasterRow = ref<PurchaseInquiryRowRecord | null>(null)
  const ctx: PurchaseInquiryMasterContext = { selectedMasterRow }
  provide(purchaseInquiryMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {PurchaseInquiryMasterContext} 主表上下文
 */
export function usePurchaseInquiryMasterContext(): PurchaseInquiryMasterContext {
  const ctx = inject(purchaseInquiryMasterContextKey)
  if (!ctx) {
    throw new Error('usePurchaseInquiryMasterContext must be used within purchase-inquiry index')
  }
  return ctx
}

// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/procurement/purchase-info-record/composables
// 文件名称：usePurchaseInfoRecordMasterContext.ts
// 功能描述：Takt采购信息记录实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { PurchaseInfoRecord } from '@/types/logistics/procurement/purchaseInfoRecord.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type PurchaseInfoRecordRowRecord = PurchaseInfoRecord | Record<string, unknown>

/** 主表选中行上下文 */
export interface PurchaseInfoRecordMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<PurchaseInfoRecordRowRecord | null>
}

const purchaseInfoRecordMasterContextKey: InjectionKey<PurchaseInfoRecordMasterContext> = Symbol('purchase-info-recordMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {PurchaseInfoRecordMasterContext} 主表上下文
 */
export function providePurchaseInfoRecordMasterContext(): PurchaseInfoRecordMasterContext {
  const selectedMasterRow = ref<PurchaseInfoRecordRowRecord | null>(null)
  const ctx: PurchaseInfoRecordMasterContext = { selectedMasterRow }
  provide(purchaseInfoRecordMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {PurchaseInfoRecordMasterContext} 主表上下文
 */
export function usePurchaseInfoRecordMasterContext(): PurchaseInfoRecordMasterContext {
  const ctx = inject(purchaseInfoRecordMasterContextKey)
  if (!ctx) {
    throw new Error('usePurchaseInfoRecordMasterContext must be used within purchase-info-record index')
  }
  return ctx
}

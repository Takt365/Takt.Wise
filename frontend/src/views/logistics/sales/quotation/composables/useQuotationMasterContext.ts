// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/sales/quotation/composables
// 文件名称：useQuotationMasterContext.ts
// 功能描述：Takt销售报价实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { SalesQuotation } from '@/types/logistics/sales/quotation.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type SalesQuotationRowRecord = SalesQuotation | Record<string, unknown>

/** 主表选中行上下文 */
export interface SalesQuotationMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<SalesQuotationRowRecord | null>
}

const salesQuotationMasterContextKey: InjectionKey<SalesQuotationMasterContext> = Symbol('quotationMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {SalesQuotationMasterContext} 主表上下文
 */
export function provideSalesQuotationMasterContext(): SalesQuotationMasterContext {
  const selectedMasterRow = ref<SalesQuotationRowRecord | null>(null)
  const ctx: SalesQuotationMasterContext = { selectedMasterRow }
  provide(salesQuotationMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {SalesQuotationMasterContext} 主表上下文
 */
export function useSalesQuotationMasterContext(): SalesQuotationMasterContext {
  const ctx = inject(salesQuotationMasterContextKey)
  if (!ctx) {
    throw new Error('useSalesQuotationMasterContext must be used within quotation index')
  }
  return ctx
}

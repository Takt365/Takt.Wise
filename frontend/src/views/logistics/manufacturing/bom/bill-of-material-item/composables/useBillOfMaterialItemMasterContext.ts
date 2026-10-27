// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/bom/bill-of-material-item/composables
// 文件名称：useBillOfMaterialItemMasterContext.ts
// 功能描述：Takt物料清单明细实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { BillOfMaterialItem } from '@/types/logistics/manufacturing/bom/billOfMaterialItem.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type BillOfMaterialItemRowRecord = BillOfMaterialItem | Record<string, unknown>

/** 主表选中行上下文 */
export interface BillOfMaterialItemMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<BillOfMaterialItemRowRecord | null>
}

const billOfMaterialItemMasterContextKey: InjectionKey<BillOfMaterialItemMasterContext> = Symbol('bill-of-material-itemMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {BillOfMaterialItemMasterContext} 主表上下文
 */
export function provideBillOfMaterialItemMasterContext(): BillOfMaterialItemMasterContext {
  const selecmedMasterRow = ref<BillOfMaterialItemRowRecord | null>(null)
  const ctx: BillOfMaterialItemMasterContext = { selecmedMasterRow }
  provide(billOfMaterialItemMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {BillOfMaterialItemMasterContext} 主表上下文
 */
export function useBillOfMaterialItemMasterContext(): BillOfMaterialItemMasterContext {
  const ctx = inject(billOfMaterialItemMasterContextKey)
  if (!ctx) {
    throw new Error('useBillOfMaterialItemMasterContext must be used within bill-of-material-item index')
  }
  return ctx
}

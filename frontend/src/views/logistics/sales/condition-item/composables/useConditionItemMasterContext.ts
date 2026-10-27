// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/sales/condition-item/composables
// 文件名称：useConditionItemMasterContext.ts
// 功能描述：Takt销售条件行实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { SalesConditionItem } from '@/types/logistics/sales/conditionItem.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type SalesConditionItemRowRecord = SalesConditionItem | Record<string, unknown>

/** 主表选中行上下文 */
export interface SalesConditionItemMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<SalesConditionItemRowRecord | null>
}

const salesConditionItemMasterContextKey: InjectionKey<SalesConditionItemMasterContext> = Symbol('price-itemMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {SalesConditionItemMasterContext} 主表上下文
 */
export function provideSalesConditionItemMasterContext(): SalesConditionItemMasterContext {
  const selecmedMasterRow = ref<SalesConditionItemRowRecord | null>(null)
  const ctx: SalesConditionItemMasterContext = { selecmedMasterRow }
  provide(salesConditionItemMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {SalesConditionItemMasterContext} 主表上下文
 */
export function useSalesConditionItemMasterContext(): SalesConditionItemMasterContext {
  const ctx = inject(salesConditionItemMasterContextKey)
  if (!ctx) {
    throw new Error('useSalesConditionItemMasterContext must be used within price-item index')
  }
  return ctx
}

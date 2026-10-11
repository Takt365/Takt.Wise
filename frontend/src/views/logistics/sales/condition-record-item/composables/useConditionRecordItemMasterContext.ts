// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/sales/condition-record-item/composables
// 文件名称：useConditionRecordItemMasterContext.ts
// 功能描述：Takt销售条件记录行实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { SalesConditionRecordItem } from '@/types/logistics/sales/conditionRecordItem.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type SalesConditionRecordItemRowRecord = SalesConditionRecordItem | Record<string, unknown>

/** 主表选中行上下文 */
export interface SalesConditionRecordItemMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<SalesConditionRecordItemRowRecord | null>
}

const salesConditionRecordItemMasterContextKey: InjectionKey<SalesConditionRecordItemMasterContext> = Symbol('price-itemMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {SalesConditionRecordItemMasterContext} 主表上下文
 */
export function provideSalesConditionRecordItemMasterContext(): SalesConditionRecordItemMasterContext {
  const selectedMasterRow = ref<SalesConditionRecordItemRowRecord | null>(null)
  const ctx: SalesConditionRecordItemMasterContext = { selectedMasterRow }
  provide(salesConditionRecordItemMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {SalesConditionRecordItemMasterContext} 主表上下文
 */
export function useSalesConditionRecordItemMasterContext(): SalesConditionRecordItemMasterContext {
  const ctx = inject(salesConditionRecordItemMasterContextKey)
  if (!ctx) {
    throw new Error('useSalesConditionRecordItemMasterContext must be used within price-item index')
  }
  return ctx
}

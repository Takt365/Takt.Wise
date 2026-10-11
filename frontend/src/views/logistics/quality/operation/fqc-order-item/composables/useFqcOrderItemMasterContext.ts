// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/quality/operation/fqc-order-item/composables
// 文件名称：useFqcOrderItemMasterContext.ts
// 功能描述：FQC出货检验单明细实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { FqcOrderItem } from '@/types/logistics/quality/operation/fqcOrderItem.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type FqcOrderItemRowRecord = FqcOrderItem | Record<string, unknown>

/** 主表选中行上下文 */
export interface FqcOrderItemMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<FqcOrderItemRowRecord | null>
}

const fqcOrderItemMasterContextKey: InjectionKey<FqcOrderItemMasterContext> = Symbol('fqc-order-itemMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {FqcOrderItemMasterContext} 主表上下文
 */
export function provideFqcOrderItemMasterContext(): FqcOrderItemMasterContext {
  const selectedMasterRow = ref<FqcOrderItemRowRecord | null>(null)
  const ctx: FqcOrderItemMasterContext = { selectedMasterRow }
  provide(fqcOrderItemMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {FqcOrderItemMasterContext} 主表上下文
 */
export function useFqcOrderItemMasterContext(): FqcOrderItemMasterContext {
  const ctx = inject(fqcOrderItemMasterContextKey)
  if (!ctx) {
    throw new Error('useFqcOrderItemMasterContext must be used within fqc-order-item index')
  }
  return ctx
}

// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/quality/operation/iqc-order-item/composables
// 文件名称：useIqcOrderItemMasterContext.ts
// 功能描述：IQC进货检验单明细实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { IqcOrderItem } from '@/types/logistics/quality/operation/iqcOrderItem.types'

/** 主表选中行上下文 */
export interface IqcOrderItemMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<IqcOrderItem | null>
}

const iqcOrderItemMasterContextKey: InjectionKey<IqcOrderItemMasterContext> = Symbol('iqc-order-itemMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {IqcOrderItemMasterContext} 主表上下文
 */
export function provideIqcOrderItemMasterContext(): IqcOrderItemMasterContext {
  const selecmedMasterRow = ref<IqcOrderItem | null>(null)
  const ctx: IqcOrderItemMasterContext = { selecmedMasterRow }
  provide(iqcOrderItemMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {IqcOrderItemMasterContext} 主表上下文
 */
export function useIqcOrderItemMasterContext(): IqcOrderItemMasterContext {
  const ctx = inject(iqcOrderItemMasterContextKey)
  if (!ctx) {
    throw new Error('useIqcOrderItemMasterContext must be used within iqc-order-item index')
  }
  return ctx
}

// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/aps/aps-order/composables
// 文件名称：useApsOrderMasterContext.ts
// 功能描述：APS 排程订单主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { ApsOrder } from '@/types/logistics/manufacturing/aps/apsOrder.types'

/** 主表选中行上下文 */
export interface ApsOrderMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<ApsOrder | null>
}

const apsOrderMasterContextKey: InjectionKey<ApsOrderMasterContext> = Symbol('aps-orderMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {ApsOrderMasterContext} 主表上下文
 */
export function provideApsOrderMasterContext(): ApsOrderMasterContext {
  const selecmedMasterRow = ref<ApsOrder | null>(null)
  const ctx: ApsOrderMasterContext = { selecmedMasterRow }
  provide(apsOrderMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {ApsOrderMasterContext} 主表上下文
 */
export function useApsOrderMasterContext(): ApsOrderMasterContext {
  const ctx = inject(apsOrderMasterContextKey)
  if (!ctx) {
    throw new Error('useApsOrderMasterContext must be used within aps-order index')
  }
  return ctx
}

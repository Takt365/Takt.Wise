// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/procurement/purchase-forecast/composables
// 文件名称：usePurchaseForecastMasterContext.ts
// 功能描述：Takt采购预测实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { PurchaseForecast } from '@/types/logistics/procurement/purchaseForecast.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type PurchaseForecastRowRecord = PurchaseForecast | Record<string, unknown>

/** 主表选中行上下文 */
export interface PurchaseForecastMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<PurchaseForecastRowRecord | null>
}

const purchaseForecastMasterContextKey: InjectionKey<PurchaseForecastMasterContext> = Symbol('purchase-forecastMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {PurchaseForecastMasterContext} 主表上下文
 */
export function providePurchaseForecastMasterContext(): PurchaseForecastMasterContext {
  const selecmedMasterRow = ref<PurchaseForecastRowRecord | null>(null)
  const ctx: PurchaseForecastMasterContext = { selecmedMasterRow }
  provide(purchaseForecastMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {PurchaseForecastMasterContext} 主表上下文
 */
export function usePurchaseForecastMasterContext(): PurchaseForecastMasterContext {
  const ctx = inject(purchaseForecastMasterContextKey)
  if (!ctx) {
    throw new Error('usePurchaseForecastMasterContext must be used within purchase-forecast index')
  }
  return ctx
}

// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/mds/sales-forecast/composables
// 文件名称：useSalesForecastMasterContext.ts
// 功能描述：Takt销售预测实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { SalesForecast } from '@/types/logistics/manufacturing/mds/salesForecast.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type SalesForecastRowRecord = SalesForecast | Record<string, unknown>

/** 主表选中行上下文 */
export interface SalesForecastMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<SalesForecastRowRecord | null>
}

const salesForecastMasterContextKey: InjectionKey<SalesForecastMasterContext> = Symbol('sales-forecastMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {SalesForecastMasterContext} 主表上下文
 */
export function provideSalesForecastMasterContext(): SalesForecastMasterContext {
  const selectedMasterRow = ref<SalesForecastRowRecord | null>(null)
  const ctx: SalesForecastMasterContext = { selectedMasterRow }
  provide(salesForecastMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {SalesForecastMasterContext} 主表上下文
 */
export function useSalesForecastMasterContext(): SalesForecastMasterContext {
  const ctx = inject(salesForecastMasterContextKey)
  if (!ctx) {
    throw new Error('useSalesForecastMasterContext must be used within sales-forecast index')
  }
  return ctx
}

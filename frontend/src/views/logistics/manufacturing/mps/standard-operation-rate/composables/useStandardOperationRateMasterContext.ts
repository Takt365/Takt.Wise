// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/output/standard-operation-rate/composables
// 文件名称：useStandardOperationRateMasterContext.ts
// 功能描述：标准生产稼动率实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { StandardOperationRate } from '@/types/logistics/manufacturing/mps/standardOperationRate.types'

/** 主表选中行上下文 */
export interface StandardOperationRateMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<StandardOperationRate | null>
}

const standardOperationRateMasterContextKey: InjectionKey<StandardOperationRateMasterContext> = Symbol('standard-operation-rateMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {StandardOperationRateMasterContext} 主表上下文
 */
export function provideStandardOperationRateMasterContext(): StandardOperationRateMasterContext {
  const selecmedMasterRow = ref<StandardOperationRate | null>(null)
  const ctx: StandardOperationRateMasterContext = { selecmedMasterRow }
  provide(standardOperationRateMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {StandardOperationRateMasterContext} 主表上下文
 */
export function useStandardOperationRateMasterContext(): StandardOperationRateMasterContext {
  const ctx = inject(standardOperationRateMasterContextKey)
  if (!ctx) {
    throw new Error('useStandardOperationRateMasterContext must be used within standard-operation-rate index')
  }
  return ctx
}

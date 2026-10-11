// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/bom/standard-operation-time/composables
// 文件名称：useStandardOperationTimeMasterContext.ts
// 功能描述：标准工序时间实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { StandardOperationTime } from '@/types/logistics/manufacturing/bom/standardOperationTime.types'

/** 主表选中行上下文 */
export interface StandardOperationTimeMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<StandardOperationTime | null>
}

const standardOperationTimeMasterContextKey: InjectionKey<StandardOperationTimeMasterContext> = Symbol('standard-operation-timeMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {StandardOperationTimeMasterContext} 主表上下文
 */
export function provideStandardOperationTimeMasterContext(): StandardOperationTimeMasterContext {
  const selectedMasterRow = ref<StandardOperationTime | null>(null)
  const ctx: StandardOperationTimeMasterContext = { selectedMasterRow }
  provide(standardOperationTimeMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {StandardOperationTimeMasterContext} 主表上下文
 */
export function useStandardOperationTimeMasterContext(): StandardOperationTimeMasterContext {
  const ctx = inject(standardOperationTimeMasterContextKey)
  if (!ctx) {
    throw new Error('useStandardOperationTimeMasterContext must be used within standard-operation-time index')
  }
  return ctx
}

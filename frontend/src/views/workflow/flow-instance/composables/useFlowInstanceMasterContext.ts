// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/workflow/flow-instance/composables
// 文件名称：useFlowInstanceMasterContext.ts
// 功能描述：流程实例实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { FlowInstance } from '@/types/workflow/flowInstance.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type FlowInstanceRowRecord = FlowInstance | Record<string, unknown>

/** 主表选中行上下文 */
export interface FlowInstanceMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<FlowInstanceRowRecord | null>
}

const flowInstanceMasterContextKey: InjectionKey<FlowInstanceMasterContext> = Symbol('flow-instanceMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {FlowInstanceMasterContext} 主表上下文
 */
export function provideFlowInstanceMasterContext(): FlowInstanceMasterContext {
  const selectedMasterRow = ref<FlowInstanceRowRecord | null>(null)
  const ctx: FlowInstanceMasterContext = { selectedMasterRow }
  provide(flowInstanceMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {FlowInstanceMasterContext} 主表上下文
 */
export function useFlowInstanceMasterContext(): FlowInstanceMasterContext {
  const ctx = inject(flowInstanceMasterContextKey)
  if (!ctx) {
    throw new Error('useFlowInstanceMasterContext must be used within flow-instance index')
  }
  return ctx
}

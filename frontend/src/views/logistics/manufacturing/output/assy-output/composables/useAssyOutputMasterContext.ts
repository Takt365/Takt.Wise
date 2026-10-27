// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/output/assy-output/composables
// 文件名称：useAssyOutputMasterContext.ts
// 功能描述：组立日报主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { AssyOutput } from '@/types/logistics/manufacturing/output/assyOutput.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type AssyOutputRowRecord = AssyOutput | Record<string, unknown>

/** 主表选中行上下文 */
export interface AssyOutputMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<AssyOutputRowRecord | null>
}

const assyOutputMasterContextKey: InjectionKey<AssyOutputMasterContext> = Symbol('assy-outputMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {AssyOutputMasterContext} 主表上下文
 */
export function provideAssyOutputMasterContext(): AssyOutputMasterContext {
  const selecmedMasterRow = ref<AssyOutputRowRecord | null>(null)
  const ctx: AssyOutputMasterContext = { selecmedMasterRow }
  provide(assyOutputMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {AssyOutputMasterContext} 主表上下文
 */
export function useAssyOutputMasterContext(): AssyOutputMasterContext {
  const ctx = inject(assyOutputMasterContextKey)
  if (!ctx) {
    throw new Error('useAssyOutputMasterContext must be used within assy-output index')
  }
  return ctx
}

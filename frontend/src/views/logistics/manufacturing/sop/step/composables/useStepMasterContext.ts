// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/sop/step/composables
// 文件名称：useStepMasterContext.ts
// 功能描述：SOP 工步实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { SopStep } from '@/types/logistics/manufacturing/sop/step.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type SopStepRowRecord = SopStep | Record<string, unknown>

/** 主表选中行上下文 */
export interface SopStepMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<SopStepRowRecord | null>
}

const sopStepMasterContextKey: InjectionKey<SopStepMasterContext> = Symbol('stepMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {SopStepMasterContext} 主表上下文
 */
export function provideSopStepMasterContext(): SopStepMasterContext {
  const selectedMasterRow = ref<SopStepRowRecord | null>(null)
  const ctx: SopStepMasterContext = { selectedMasterRow }
  provide(sopStepMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {SopStepMasterContext} 主表上下文
 */
export function useSopStepMasterContext(): SopStepMasterContext {
  const ctx = inject(sopStepMasterContextKey)
  if (!ctx) {
    throw new Error('useSopStepMasterContext must be used within step index')
  }
  return ctx
}

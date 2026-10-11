// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/sop/sop-exec-step/composables
// 文件名称：useExecMasterContext.ts
// 功能描述：SOP 工位执行追溯实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { SopExec } from '@/types/logistics/manufacturing/sop/exec.types'

/** 主表选中行上下文 */
export interface SopExecMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<SopExec | null>
}

const sopExecMasterContextKey: InjectionKey<SopExecMasterContext> = Symbol('execMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {SopExecMasterContext} 主表上下文
 */
export function provideSopExecMasterContext(): SopExecMasterContext {
  const selectedMasterRow = ref<SopExec | null>(null)
  const ctx: SopExecMasterContext = { selectedMasterRow }
  provide(sopExecMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {SopExecMasterContext} 主表上下文
 */
export function useSopExecMasterContext(): SopExecMasterContext {
  const ctx = inject(sopExecMasterContextKey)
  if (!ctx) {
    throw new Error('useSopExecMasterContext must be used within exec index')
  }
  return ctx
}

// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/sop/doc/composables
// 文件名称：useDocMasterContext.ts
// 功能描述：SOP 文档头实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { SopDoc } from '@/types/logistics/manufacturing/sop/doc.types'

/** 主表选中行上下文 */
export interface SopDocMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<SopDoc | null>
}

const sopDocMasterContextKey: InjectionKey<SopDocMasterContext> = Symbol('docMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {SopDocMasterContext} 主表上下文
 */
export function provideSopDocMasterContext(): SopDocMasterContext {
  const selectedMasterRow = ref<SopDoc | null>(null)
  const ctx: SopDocMasterContext = { selectedMasterRow }
  provide(sopDocMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {SopDocMasterContext} 主表上下文
 */
export function useSopDocMasterContext(): SopDocMasterContext {
  const ctx = inject(sopDocMasterContextKey)
  if (!ctx) {
    throw new Error('useSopDocMasterContext must be used within doc index')
  }
  return ctx
}

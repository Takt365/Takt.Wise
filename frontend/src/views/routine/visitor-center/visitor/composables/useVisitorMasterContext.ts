// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/routine/visitor-center/visitor/composables
// 文件名称：useVisitorMasterContext.ts
// 功能描述：来访接待主实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { Visitor } from '@/types/routine/visitor-center/visitor.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type VisitorRowRecord = Visitor | Record<string, unknown>

/** 主表选中行上下文 */
export interface VisitorMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<VisitorRowRecord | null>
}

const visitorMasterContextKey: InjectionKey<VisitorMasterContext> = Symbol('visitorMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {VisitorMasterContext} 主表上下文
 */
export function provideVisitorMasterContext(): VisitorMasterContext {
  const selectedMasterRow = ref<VisitorRowRecord | null>(null)
  const ctx: VisitorMasterContext = { selectedMasterRow }
  provide(visitorMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {VisitorMasterContext} 主表上下文
 */
export function useVisitorMasterContext(): VisitorMasterContext {
  const ctx = inject(visitorMasterContextKey)
  if (!ctx) {
    throw new Error('useVisitorMasterContext must be used within visitor index')
  }
  return ctx
}

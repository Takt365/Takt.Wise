// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/engineering-change/source-ec/composables
// 文件名称：useSourceEcMasterContext.ts
// 功能描述：设变来源明细列表主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { SourceEc } from '@/types/logistics/manufacturing/engineering-change/sourceEc.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type SourceEcRowRecord = SourceEc | Record<string, unknown>

/** 主表选中行上下文 */
export interface SourceEcMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<SourceEcRowRecord | null>
}

const sourceEcMasterContextKey: InjectionKey<SourceEcMasterContext> = Symbol('source-ecMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {SourceEcMasterContext} 主表上下文
 */
export function provideSourceEcMasterContext(): SourceEcMasterContext {
  const selecmedMasterRow = ref<SourceEcRowRecord | null>(null)
  const ctx: SourceEcMasterContext = { selecmedMasterRow }
  provide(sourceEcMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {SourceEcMasterContext} 主表上下文
 */
export function useSourceEcMasterContext(): SourceEcMasterContext {
  const ctx = inject(sourceEcMasterContextKey)
  if (!ctx) {
    throw new Error('useSourceEcMasterContext must be used within source-ec index')
  }
  return ctx
}

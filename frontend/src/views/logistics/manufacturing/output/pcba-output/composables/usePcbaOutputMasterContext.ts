// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/output/pcba-output/composables
// 文件名称：usePcbaOutputMasterContext.ts
// 功能描述：PCBA日报实体 达成率主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { PcbaOutput } from '@/types/logistics/manufacturing/output/pcbaOutput.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type PcbaOutputRowRecord = PcbaOutput | Record<string, unknown>

/** 主表选中行上下文 */
export interface PcbaOutputMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<PcbaOutputRowRecord | null>
}

const pcbaOutputMasterContextKey: InjectionKey<PcbaOutputMasterContext> = Symbol('pcba-outputMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {PcbaOutputMasterContext} 主表上下文
 */
export function providePcbaOutputMasterContext(): PcbaOutputMasterContext {
  const selecmedMasterRow = ref<PcbaOutputRowRecord | null>(null)
  const ctx: PcbaOutputMasterContext = { selecmedMasterRow }
  provide(pcbaOutputMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {PcbaOutputMasterContext} 主表上下文
 */
export function usePcbaOutputMasterContext(): PcbaOutputMasterContext {
  const ctx = inject(pcbaOutputMasterContextKey)
  if (!ctx) {
    throw new Error('usePcbaOutputMasterContext must be used within pcba-output index')
  }
  return ctx
}

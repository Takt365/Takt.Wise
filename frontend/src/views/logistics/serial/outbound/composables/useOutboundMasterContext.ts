// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/serial/outbound/composables
// 文件名称：useOutboundMasterContext.ts
// 功能描述：序列号出库主表实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { SerialOutbound } from '@/types/logistics/serial/outbound.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type SerialOutboundRowRecord = SerialOutbound | Record<string, unknown>

/** 主表选中行上下文 */
export interface SerialOutboundMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<SerialOutboundRowRecord | null>
}

const serialOutboundMasterContextKey: InjectionKey<SerialOutboundMasterContext> = Symbol('outboundMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {SerialOutboundMasterContext} 主表上下文
 */
export function provideSerialOutboundMasterContext(): SerialOutboundMasterContext {
  const selecmedMasterRow = ref<SerialOutboundRowRecord | null>(null)
  const ctx: SerialOutboundMasterContext = { selecmedMasterRow }
  provide(serialOutboundMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {SerialOutboundMasterContext} 主表上下文
 */
export function useSerialOutboundMasterContext(): SerialOutboundMasterContext {
  const ctx = inject(serialOutboundMasterContextKey)
  if (!ctx) {
    throw new Error('useSerialOutboundMasterContext must be used within outbound index')
  }
  return ctx
}

// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/serial/inbound/composables
// 文件名称：useInboundMasterContext.ts
// 功能描述：序列号入库主表实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { SerialInbound } from '@/types/logistics/serial/inbound.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type SerialInboundRowRecord = SerialInbound | Record<string, unknown>

/** 主表选中行上下文 */
export interface SerialInboundMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<SerialInboundRowRecord | null>
}

const serialInboundMasterContextKey: InjectionKey<SerialInboundMasterContext> = Symbol('inboundMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {SerialInboundMasterContext} 主表上下文
 */
export function provideSerialInboundMasterContext(): SerialInboundMasterContext {
  const selecmedMasterRow = ref<SerialInboundRowRecord | null>(null)
  const ctx: SerialInboundMasterContext = { selecmedMasterRow }
  provide(serialInboundMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {SerialInboundMasterContext} 主表上下文
 */
export function useSerialInboundMasterContext(): SerialInboundMasterContext {
  const ctx = inject(serialInboundMasterContextKey)
  if (!ctx) {
    throw new Error('useSerialInboundMasterContext must be used within inbound index')
  }
  return ctx
}

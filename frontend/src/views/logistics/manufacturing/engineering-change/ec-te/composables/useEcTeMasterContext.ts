// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/engineering-change/ec-te/composables
// 文件名称：useEcTeMasterContext.ts
// 功能描述：设变技术课主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { EcTe } from '@/types/logistics/manufacturing/engineering-change/ecTe.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type EcTeRowRecord = EcTe | Record<string, unknown>

/** 主表选中行上下文 */
export interface EcTeMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<EcTeRowRecord | null>
}

const ecTeMasterContextKey: InjectionKey<EcTeMasterContext> = Symbol('ec-teMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {EcTeMasterContext} 主表上下文
 */
export function provideEcTeMasterContext(): EcTeMasterContext {
  const selecmedMasterRow = ref<EcTeRowRecord | null>(null)
  const ctx: EcTeMasterContext = { selecmedMasterRow }
  provide(ecTeMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {EcTeMasterContext} 主表上下文
 */
export function useEcTeMasterContext(): EcTeMasterContext {
  const ctx = inject(ecTeMasterContextKey)
  if (!ctx) {
    throw new Error('useEcTeMasterContext must be used within ec-te index')
  }
  return ctx
}

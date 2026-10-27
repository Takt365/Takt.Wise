// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/engineering-change/ec-te/composables
// 文件名称：useEcMasterContext.ts
// 功能描述：设变主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { EcTe } from '@/types/logistics/manufacturing/engineering-change/ecTe.types'

/** 主表选中行上下文 */
export interface EcMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<EcTe | null>
}

const ecMasterContextKey: InjectionKey<EcMasterContext> = Symbol('ecMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {EcMasterContext} 主表上下文
 */
export function provideEcMasterContext(): EcMasterContext {
  const selecmedMasterRow = ref<EcTe | null>(null)
  const ctx: EcMasterContext = { selecmedMasterRow }
  provide(ecMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {EcMasterContext} 主表上下文
 */
export function useEcMasterContext(): EcMasterContext {
  const ctx = inject(ecMasterContextKey)
  if (!ctx) {
    throw new Error('useEcMasterContext must be used within ec-te index')
  }
  return ctx
}

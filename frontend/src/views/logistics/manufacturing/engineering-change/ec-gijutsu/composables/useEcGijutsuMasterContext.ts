// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/engineering-change/ec-gijutsu/composables
// 文件名称：useEcGijutsuMasterContext.ts
// 功能描述：设变技术课主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { EcGijutsu } from '@/types/logistics/manufacturing/engineering-change/ecGijutsu.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type EcGijutsuRowRecord = EcGijutsu | Record<string, unknown>

/** 主表选中行上下文 */
export interface EcGijutsuMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<EcGijutsuRowRecord | null>
}

const ecGijutsuMasterContextKey: InjectionKey<EcGijutsuMasterContext> = Symbol('ec-gijutsuMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {EcGijutsuMasterContext} 主表上下文
 */
export function provideEcGijutsuMasterContext(): EcGijutsuMasterContext {
  const selectedMasterRow = ref<EcGijutsuRowRecord | null>(null)
  const ctx: EcGijutsuMasterContext = { selectedMasterRow }
  provide(ecGijutsuMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {EcGijutsuMasterContext} 主表上下文
 */
export function useEcGijutsuMasterContext(): EcGijutsuMasterContext {
  const ctx = inject(ecGijutsuMasterContextKey)
  if (!ctx) {
    throw new Error('useEcGijutsuMasterContext must be used within ec-gijutsu index')
  }
  return ctx
}

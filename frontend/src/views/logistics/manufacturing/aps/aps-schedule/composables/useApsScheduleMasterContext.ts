// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/aps/aps-schedule/composables
// 文件名称：useApsScheduleMasterContext.ts
// 功能描述：APS排程主表主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { ApsSchedule } from '@/types/logistics/manufacturing/aps/apsSchedule.types'

/** 主表选中行上下文 */
export interface ApsScheduleMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<ApsSchedule | null>
}

const apsScheduleMasterContextKey: InjectionKey<ApsScheduleMasterContext> = Symbol('aps-scheduleMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {ApsScheduleMasterContext} 主表上下文
 */
export function provideApsScheduleMasterContext(): ApsScheduleMasterContext {
  const selecmedMasterRow = ref<ApsSchedule | null>(null)
  const ctx: ApsScheduleMasterContext = { selecmedMasterRow }
  provide(apsScheduleMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {ApsScheduleMasterContext} 主表上下文
 */
export function useApsScheduleMasterContext(): ApsScheduleMasterContext {
  const ctx = inject(apsScheduleMasterContextKey)
  if (!ctx) {
    throw new Error('useApsScheduleMasterContext must be used within aps-schedule index')
  }
  return ctx
}

// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/human-resource/attendance/overtime/composables
// 文件名称：useOvertimeMasterContext.ts
// 功能描述：加班申请主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { Overtime } from '@/types/human-resource/attendance/overtime.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type OvertimeRowRecord = Overtime | Record<string, unknown>

/** 主表选中行上下文 */
export interface OvertimeMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<OvertimeRowRecord | null>
}

const overtimeMasterContextKey: InjectionKey<OvertimeMasterContext> = Symbol('overtimeMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {OvertimeMasterContext} 主表上下文
 */
export function provideOvertimeMasterContext(): OvertimeMasterContext {
  const selectedMasterRow = ref<OvertimeRowRecord | null>(null)
  const ctx: OvertimeMasterContext = { selectedMasterRow }
  provide(overtimeMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {OvertimeMasterContext} 主表上下文
 */
export function useOvertimeMasterContext(): OvertimeMasterContext {
  const ctx = inject(overtimeMasterContextKey)
  if (!ctx) {
    throw new Error('useOvertimeMasterContext must be used within overtime index')
  }
  return ctx
}

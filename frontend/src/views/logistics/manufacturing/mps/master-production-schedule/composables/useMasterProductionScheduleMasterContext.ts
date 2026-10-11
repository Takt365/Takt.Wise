// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/mps/master-production-schedule/composables
// 文件名称：useMasterProductionScheduleMasterContext.ts
// 功能描述：主生产计划 MPS 头表主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { MasterProductionSchedule } from '@/types/logistics/manufacturing/mps/masterProductionSchedule.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type MasterProductionScheduleRowRecord = MasterProductionSchedule | Record<string, unknown>

/** 主表选中行上下文 */
export interface MasterProductionScheduleMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<MasterProductionScheduleRowRecord | null>
}

const masterProductionScheduleMasterContextKey: InjectionKey<MasterProductionScheduleMasterContext> = Symbol('master-production-scheduleMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {MasterProductionScheduleMasterContext} 主表上下文
 */
export function provideMasterProductionScheduleMasterContext(): MasterProductionScheduleMasterContext {
  const selectedMasterRow = ref<MasterProductionScheduleRowRecord | null>(null)
  const ctx: MasterProductionScheduleMasterContext = { selectedMasterRow }
  provide(masterProductionScheduleMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {MasterProductionScheduleMasterContext} 主表上下文
 */
export function useMasterProductionScheduleMasterContext(): MasterProductionScheduleMasterContext {
  const ctx = inject(masterProductionScheduleMasterContextKey)
  if (!ctx) {
    throw new Error('useMasterProductionScheduleMasterContext must be used within master-production-schedule index')
  }
  return ctx
}

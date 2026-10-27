// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/mds/master-demand-schedule/composables
// 文件名称：useMasterDemandScheduleMasterContext.ts
// 功能描述：主需求计划 MDS 头表主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { MasterDemandSchedule } from '@/types/logistics/manufacturing/mds/masterDemandSchedule.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type MasterDemandScheduleRowRecord = MasterDemandSchedule | Record<string, unknown>

/** 主表选中行上下文 */
export interface MasterDemandScheduleMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<MasterDemandScheduleRowRecord | null>
}

const masterDemandScheduleMasterContextKey: InjectionKey<MasterDemandScheduleMasterContext> = Symbol('master-demand-scheduleMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {MasterDemandScheduleMasterContext} 主表上下文
 */
export function provideMasterDemandScheduleMasterContext(): MasterDemandScheduleMasterContext {
  const selecmedMasterRow = ref<MasterDemandScheduleRowRecord | null>(null)
  const ctx: MasterDemandScheduleMasterContext = { selecmedMasterRow }
  provide(masterDemandScheduleMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {MasterDemandScheduleMasterContext} 主表上下文
 */
export function useMasterDemandScheduleMasterContext(): MasterDemandScheduleMasterContext {
  const ctx = inject(masterDemandScheduleMasterContextKey)
  if (!ctx) {
    throw new Error('useMasterDemandScheduleMasterContext must be used within master-demand-schedule index')
  }
  return ctx
}

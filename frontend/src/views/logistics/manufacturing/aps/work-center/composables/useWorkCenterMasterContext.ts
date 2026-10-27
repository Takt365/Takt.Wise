// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/aps/work-center/composables
// 文件名称：useWorkCenterMasterContext.ts
// 功能描述：工作中心主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { WorkCenter } from '@/types/logistics/manufacturing/aps/workCenter.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type WorkCenterRowRecord = WorkCenter | Record<string, unknown>

/** 主表选中行上下文 */
export interface WorkCenterMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<WorkCenterRowRecord | null>
}

const workCenterMasterContextKey: InjectionKey<WorkCenterMasterContext> = Symbol('work-centerMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {WorkCenterMasterContext} 主表上下文
 */
export function provideWorkCenterMasterContext(): WorkCenterMasterContext {
  const selecmedMasterRow = ref<WorkCenterRowRecord | null>(null)
  const ctx: WorkCenterMasterContext = { selecmedMasterRow }
  provide(workCenterMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {WorkCenterMasterContext} 主表上下文
 */
export function useWorkCenterMasterContext(): WorkCenterMasterContext {
  const ctx = inject(workCenterMasterContextKey)
  if (!ctx) {
    throw new Error('useWorkCenterMasterContext must be used within work-center index')
  }
  return ctx
}

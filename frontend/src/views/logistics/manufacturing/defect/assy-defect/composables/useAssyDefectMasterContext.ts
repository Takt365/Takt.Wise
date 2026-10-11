// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/defect/assy-defect/composables
// 文件名称：useAssyDefectMasterContext.ts
// 功能描述：组立不良日报实体 不良率主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { AssyDefect } from '@/types/logistics/manufacturing/defect/assyDefect.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type AssyDefectRowRecord = AssyDefect | Record<string, unknown>

/** 主表选中行上下文 */
export interface AssyDefectMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<AssyDefectRowRecord | null>
}

const assyDefectMasterContextKey: InjectionKey<AssyDefectMasterContext> = Symbol('assy-defectMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {AssyDefectMasterContext} 主表上下文
 */
export function provideAssyDefectMasterContext(): AssyDefectMasterContext {
  const selectedMasterRow = ref<AssyDefectRowRecord | null>(null)
  const ctx: AssyDefectMasterContext = { selectedMasterRow }
  provide(assyDefectMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {AssyDefectMasterContext} 主表上下文
 */
export function useAssyDefectMasterContext(): AssyDefectMasterContext {
  const ctx = inject(assyDefectMasterContextKey)
  if (!ctx) {
    throw new Error('useAssyDefectMasterContext must be used within assy-defect index')
  }
  return ctx
}

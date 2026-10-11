// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/quality/operation/inspection-standard/composables
// 文件名称：useInspectionStandardMasterContext.ts
// 功能描述：检验标准实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { InspectionStandard } from '@/types/logistics/quality/operation/inspectionStandard.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type InspectionStandardRowRecord = InspectionStandard | Record<string, unknown>

/** 主表选中行上下文 */
export interface InspectionStandardMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<InspectionStandardRowRecord | null>
}

const inspectionStandardMasterContextKey: InjectionKey<InspectionStandardMasterContext> = Symbol('inspection-standardMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {InspectionStandardMasterContext} 主表上下文
 */
export function provideInspectionStandardMasterContext(): InspectionStandardMasterContext {
  const selectedMasterRow = ref<InspectionStandardRowRecord | null>(null)
  const ctx: InspectionStandardMasterContext = { selectedMasterRow }
  provide(inspectionStandardMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {InspectionStandardMasterContext} 主表上下文
 */
export function useInspectionStandardMasterContext(): InspectionStandardMasterContext {
  const ctx = inject(inspectionStandardMasterContextKey)
  if (!ctx) {
    throw new Error('useInspectionStandardMasterContext must be used within inspection-standard index')
  }
  return ctx
}

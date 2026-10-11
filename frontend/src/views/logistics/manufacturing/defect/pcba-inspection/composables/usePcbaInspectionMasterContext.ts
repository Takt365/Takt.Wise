// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/defect/pcba-inspection/composables
// 文件名称：usePcbaInspectionMasterContext.ts
// 功能描述：PCBA检查日报实体 不良率主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { PcbaInspection } from '@/types/logistics/manufacturing/defect/pcbaInspection.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type PcbaInspectionRowRecord = PcbaInspection | Record<string, unknown>

/** 主表选中行上下文 */
export interface PcbaInspectionMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<PcbaInspectionRowRecord | null>
}

const pcbaInspectionMasterContextKey: InjectionKey<PcbaInspectionMasterContext> = Symbol('pcba-inspectionMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {PcbaInspectionMasterContext} 主表上下文
 */
export function providePcbaInspectionMasterContext(): PcbaInspectionMasterContext {
  const selectedMasterRow = ref<PcbaInspectionRowRecord | null>(null)
  const ctx: PcbaInspectionMasterContext = { selectedMasterRow }
  provide(pcbaInspectionMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {PcbaInspectionMasterContext} 主表上下文
 */
export function usePcbaInspectionMasterContext(): PcbaInspectionMasterContext {
  const ctx = inject(pcbaInspectionMasterContextKey)
  if (!ctx) {
    throw new Error('usePcbaInspectionMasterContext must be used within pcba-inspection index')
  }
  return ctx
}

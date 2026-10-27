// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/defect/pcba-repair/composables
// 文件名称：usePcbaRepairMasterContext.ts
// 功能描述：PCBA改修日报实体 不良率主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { PcbaRepair } from '@/types/logistics/manufacturing/defect/pcbaRepair.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type PcbaRepairRowRecord = PcbaRepair | Record<string, unknown>

/** 主表选中行上下文 */
export interface PcbaRepairMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<PcbaRepairRowRecord | null>
}

const pcbaRepairMasterContextKey: InjectionKey<PcbaRepairMasterContext> = Symbol('pcba-repairMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {PcbaRepairMasterContext} 主表上下文
 */
export function providePcbaRepairMasterContext(): PcbaRepairMasterContext {
  const selecmedMasterRow = ref<PcbaRepairRowRecord | null>(null)
  const ctx: PcbaRepairMasterContext = { selecmedMasterRow }
  provide(pcbaRepairMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {PcbaRepairMasterContext} 主表上下文
 */
export function usePcbaRepairMasterContext(): PcbaRepairMasterContext {
  const ctx = inject(pcbaRepairMasterContextKey)
  if (!ctx) {
    throw new Error('usePcbaRepairMasterContext must be used within pcba-repair index')
  }
  return ctx
}

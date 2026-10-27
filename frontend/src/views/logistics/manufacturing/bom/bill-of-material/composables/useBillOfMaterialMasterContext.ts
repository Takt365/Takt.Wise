// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/bom/bill-of-material/composables
// 文件名称：useBillOfMaterialMasterContext.ts
// 功能描述：Takt物料清单实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { BillOfMaterial } from '@/types/logistics/manufacturing/bom/billOfMaterial.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type BillOfMaterialRowRecord = BillOfMaterial | Record<string, unknown>

/** 主表选中行上下文 */
export interface BillOfMaterialMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<BillOfMaterialRowRecord | null>
}

const billOfMaterialMasterContextKey: InjectionKey<BillOfMaterialMasterContext> = Symbol('bill-of-materialMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {BillOfMaterialMasterContext} 主表上下文
 */
export function provideBillOfMaterialMasterContext(): BillOfMaterialMasterContext {
  const selecmedMasterRow = ref<BillOfMaterialRowRecord | null>(null)
  const ctx: BillOfMaterialMasterContext = { selecmedMasterRow }
  provide(billOfMaterialMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {BillOfMaterialMasterContext} 主表上下文
 */
export function useBillOfMaterialMasterContext(): BillOfMaterialMasterContext {
  const ctx = inject(billOfMaterialMasterContextKey)
  if (!ctx) {
    throw new Error('useBillOfMaterialMasterContext must be used within bill-of-material index')
  }
  return ctx
}

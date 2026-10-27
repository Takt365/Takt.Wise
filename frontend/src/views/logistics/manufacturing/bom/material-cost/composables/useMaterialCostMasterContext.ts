// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/bom/material-cost/composables
// 文件名称：useMaterialCostMasterContext.ts
// 功能描述：左机种→中产品→右明细选中上下文（实体未拆分）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { BomMaterialCost, BomMaterialCostModelGroup } from '@/types/logistics/manufacturing/bom/materialCost.types'

/** 产品子表行（仍为 TaktBomMaterialCost 产品维度行） */
export type BomMaterialCostRowRecord = BomMaterialCost | Record<string, unknown>

/** 机种聚合主表行 */
export type BomMaterialCostModelGroupRecord = BomMaterialCostModelGroup | Record<string, unknown>

/** 三层关联选中上下文 */
export interface BomMaterialCostMasterContext {
  /** 机种维度主表选中（工厂+机种+核算期间） */
  selecmedModelGroup: Ref<BomMaterialCostModelGroupRecord | null>
  /** 产品子表选中（产品月成本行） */
  selecmedProductRow: Ref<BomMaterialCostRowRecord | null>
}

const bomMaterialCostMasterContextKey: InjectionKey<BomMaterialCostMasterContext> = Symbol('material-costMasterContext')

/**
 * 在汇总页 provide 三层选中上下文
 * @returns {BomMaterialCostMasterContext} 上下文
 */
export function provideBomMaterialCostMasterContext(): BomMaterialCostMasterContext {
  const selecmedModelGroup = ref<BomMaterialCostModelGroupRecord | null>(null)
  const selecmedProductRow = ref<BomMaterialCostRowRecord | null>(null)
  const ctx: BomMaterialCostMasterContext = { selecmedModelGroup, selecmedProductRow }
  provide(bomMaterialCostMasterContextKey, ctx)
  return ctx
}

/**
 * 在子表面板 inject 选中上下文
 * @returns {BomMaterialCostMasterContext} 上下文
 */
export function useBomMaterialCostMasterContext(): BomMaterialCostMasterContext {
  const ctx = inject(bomMaterialCostMasterContextKey)
  if (!ctx) {
    throw new Error('useBomMaterialCostMasterContext must be used within material-cost index')
  }
  return ctx
}

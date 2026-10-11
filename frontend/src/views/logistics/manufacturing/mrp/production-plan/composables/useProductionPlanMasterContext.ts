// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/mrp/production-plan/composables
// 文件名称：useProductionPlanMasterContext.ts
// 功能描述：Takt生产计划实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { ProductionPlan } from '@/types/logistics/manufacturing/mrp/productionPlan.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type ProductionPlanRowRecord = ProductionPlan | Record<string, unknown>

/** 主表选中行上下文 */
export interface ProductionPlanMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<ProductionPlanRowRecord | null>
}

const productionPlanMasterContextKey: InjectionKey<ProductionPlanMasterContext> = Symbol('production-planMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {ProductionPlanMasterContext} 主表上下文
 */
export function provideProductionPlanMasterContext(): ProductionPlanMasterContext {
  const selectedMasterRow = ref<ProductionPlanRowRecord | null>(null)
  const ctx: ProductionPlanMasterContext = { selectedMasterRow }
  provide(productionPlanMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {ProductionPlanMasterContext} 主表上下文
 */
export function useProductionPlanMasterContext(): ProductionPlanMasterContext {
  const ctx = inject(productionPlanMasterContextKey)
  if (!ctx) {
    throw new Error('useProductionPlanMasterContext must be used within production-plan index')
  }
  return ctx
}

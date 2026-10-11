// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/mps/production-team/composables
// 文件名称：useProductionTeamMasterContext.ts
// 功能描述：生产班组实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { ProductionTeam } from '@/types/logistics/manufacturing/mps/productionTeam.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type ProductionTeamRowRecord = ProductionTeam | Record<string, unknown>

/** 主表选中行上下文 */
export interface ProductionTeamMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<ProductionTeamRowRecord | null>
}

const productionTeamMasterContextKey: InjectionKey<ProductionTeamMasterContext> = Symbol('production-teamMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {ProductionTeamMasterContext} 主表上下文
 */
export function provideProductionTeamMasterContext(): ProductionTeamMasterContext {
  const selectedMasterRow = ref<ProductionTeamRowRecord | null>(null)
  const ctx: ProductionTeamMasterContext = { selectedMasterRow }
  provide(productionTeamMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {ProductionTeamMasterContext} 主表上下文
 */
export function useProductionTeamMasterContext(): ProductionTeamMasterContext {
  const ctx = inject(productionTeamMasterContextKey)
  if (!ctx) {
    throw new Error('useProductionTeamMasterContext must be used within production-team index')
  }
  return ctx
}

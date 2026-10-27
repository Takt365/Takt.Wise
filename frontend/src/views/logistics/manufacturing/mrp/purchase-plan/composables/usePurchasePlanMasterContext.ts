// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/mrp/purchase-plan/composables
// 文件名称：usePurchasePlanMasterContext.ts
// 功能描述：Takt采购计划实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { PurchasePlan } from '@/types/logistics/manufacturing/mrp/purchasePlan.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type PurchasePlanRowRecord = PurchasePlan | Record<string, unknown>

/** 主表选中行上下文 */
export interface PurchasePlanMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<PurchasePlanRowRecord | null>
}

const purchasePlanMasterContextKey: InjectionKey<PurchasePlanMasterContext> = Symbol('purchase-planMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {PurchasePlanMasterContext} 主表上下文
 */
export function providePurchasePlanMasterContext(): PurchasePlanMasterContext {
  const selecmedMasterRow = ref<PurchasePlanRowRecord | null>(null)
  const ctx: PurchasePlanMasterContext = { selecmedMasterRow }
  provide(purchasePlanMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {PurchasePlanMasterContext} 主表上下文
 */
export function usePurchasePlanMasterContext(): PurchasePlanMasterContext {
  const ctx = inject(purchasePlanMasterContextKey)
  if (!ctx) {
    throw new Error('usePurchasePlanMasterContext must be used within purchase-plan index')
  }
  return ctx
}

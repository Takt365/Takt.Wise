// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/sales/condition/composables
// 文件名称：useSalesConditionMasterContext.ts
// 功能描述：Takt销售条件实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { SalesCondition } from '@/types/logistics/sales/condition.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type SalesConditionRowRecord = SalesCondition | Record<string, unknown>

/** 主表选中行上下文 */
export interface SalesConditionMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<SalesConditionRowRecord | null>
}

const salesConditionMasterContextKey: InjectionKey<SalesConditionMasterContext> = Symbol('conditionMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {SalesConditionMasterContext} 主表上下文
 */
export function provideSalesConditionMasterContext(): SalesConditionMasterContext {
  const selecmedMasterRow = ref<SalesConditionRowRecord | null>(null)
  const ctx: SalesConditionMasterContext = { selecmedMasterRow }
  provide(salesConditionMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {SalesConditionMasterContext} 主表上下文
 */
export function useSalesConditionMasterContext(): SalesConditionMasterContext {
  const ctx = inject(salesConditionMasterContextKey)
  if (!ctx) {
    throw new Error('useSalesConditionMasterContext must be used within condition index')
  }
  return ctx
}

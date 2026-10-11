// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/sales/condition-record/composables
// 文件名称：useSalesConditionRecordMasterContext.ts
// 功能描述：Takt销售条件记录实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { SalesConditionRecord } from '@/types/logistics/sales/conditionRecord.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type SalesConditionRecordRowRecord = SalesConditionRecord | Record<string, unknown>

/** 主表选中行上下文 */
export interface SalesConditionRecordMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<SalesConditionRecordRowRecord | null>
}

const salesConditionRecordMasterContextKey: InjectionKey<SalesConditionRecordMasterContext> = Symbol('condition-recordMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {SalesConditionRecordMasterContext} 主表上下文
 */
export function provideSalesConditionRecordMasterContext(): SalesConditionRecordMasterContext {
  const selectedMasterRow = ref<SalesConditionRecordRowRecord | null>(null)
  const ctx: SalesConditionRecordMasterContext = { selectedMasterRow }
  provide(salesConditionRecordMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {SalesConditionRecordMasterContext} 主表上下文
 */
export function useSalesConditionRecordMasterContext(): SalesConditionRecordMasterContext {
  const ctx = inject(salesConditionRecordMasterContextKey)
  if (!ctx) {
    throw new Error('useSalesConditionRecordMasterContext must be used within condition-record index')
  }
  return ctx
}

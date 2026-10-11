// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/accounting/financial/expense/composables
// 文件名称：useExpenseMasterContext.ts
// 功能描述：费用单实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { Expense } from '@/types/accounting/financial/expense.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type ExpenseRowRecord = Expense | Record<string, unknown>

/** 主表选中行上下文 */
export interface ExpenseMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<ExpenseRowRecord | null>
}

const expenseMasterContextKey: InjectionKey<ExpenseMasterContext> = Symbol('expenseMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {ExpenseMasterContext} 主表上下文
 */
export function provideExpenseMasterContext(): ExpenseMasterContext {
  const selectedMasterRow = ref<ExpenseRowRecord | null>(null)
  const ctx: ExpenseMasterContext = { selectedMasterRow }
  provide(expenseMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {ExpenseMasterContext} 主表上下文
 */
export function useExpenseMasterContext(): ExpenseMasterContext {
  const ctx = inject(expenseMasterContextKey)
  if (!ctx) {
    throw new Error('useExpenseMasterContext must be used within expense index')
  }
  return ctx
}

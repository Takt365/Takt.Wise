// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/quality/complaint/supplier-evaluation/composables
// 文件名称：useSupplierEvaluationMasterContext.ts
// 功能描述：供应商评价考核主表实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { SupplierEvaluation } from '@/types/logistics/quality/complaint/supplierEvaluation.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type SupplierEvaluationRowRecord = SupplierEvaluation | Record<string, unknown>

/** 主表选中行上下文 */
export interface SupplierEvaluationMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<SupplierEvaluationRowRecord | null>
}

const supplierEvaluationMasterContextKey: InjectionKey<SupplierEvaluationMasterContext> = Symbol('supplier-evaluationMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {SupplierEvaluationMasterContext} 主表上下文
 */
export function provideSupplierEvaluationMasterContext(): SupplierEvaluationMasterContext {
  const selectedMasterRow = ref<SupplierEvaluationRowRecord | null>(null)
  const ctx: SupplierEvaluationMasterContext = { selectedMasterRow }
  provide(supplierEvaluationMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {SupplierEvaluationMasterContext} 主表上下文
 */
export function useSupplierEvaluationMasterContext(): SupplierEvaluationMasterContext {
  const ctx = inject(supplierEvaluationMasterContextKey)
  if (!ctx) {
    throw new Error('useSupplierEvaluationMasterContext must be used within supplier-evaluation index')
  }
  return ctx
}

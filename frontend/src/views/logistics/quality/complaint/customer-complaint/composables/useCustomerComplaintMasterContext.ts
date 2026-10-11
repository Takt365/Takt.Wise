// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/quality/complaint/customer-complaint/composables
// 文件名称：useCustomerComplaintMasterContext.ts
// 功能描述：客诉主表实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { CustomerComplaint } from '@/types/logistics/quality/complaint/customerComplaint.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type CustomerComplaintRowRecord = CustomerComplaint | Record<string, unknown>

/** 主表选中行上下文 */
export interface CustomerComplaintMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<CustomerComplaintRowRecord | null>
}

const customerComplaintMasterContextKey: InjectionKey<CustomerComplaintMasterContext> = Symbol('customer-complaintMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {CustomerComplaintMasterContext} 主表上下文
 */
export function provideCustomerComplaintMasterContext(): CustomerComplaintMasterContext {
  const selectedMasterRow = ref<CustomerComplaintRowRecord | null>(null)
  const ctx: CustomerComplaintMasterContext = { selectedMasterRow }
  provide(customerComplaintMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {CustomerComplaintMasterContext} 主表上下文
 */
export function useCustomerComplaintMasterContext(): CustomerComplaintMasterContext {
  const ctx = inject(customerComplaintMasterContextKey)
  if (!ctx) {
    throw new Error('useCustomerComplaintMasterContext must be used within customer-complaint index')
  }
  return ctx
}

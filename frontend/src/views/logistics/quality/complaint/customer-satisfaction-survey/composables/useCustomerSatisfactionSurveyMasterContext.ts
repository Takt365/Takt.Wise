// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/quality/complaint/customer-satisfaction-survey/composables
// 文件名称：useCustomerSatisfactionSurveyMasterContext.ts
// 功能描述：客户满意度调查表主表实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { CustomerSatisfactionSurvey } from '@/types/logistics/quality/complaint/customerSatisfactionSurvey.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type CustomerSatisfactionSurveyRowRecord = CustomerSatisfactionSurvey | Record<string, unknown>

/** 主表选中行上下文 */
export interface CustomerSatisfactionSurveyMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<CustomerSatisfactionSurveyRowRecord | null>
}

const customerSatisfactionSurveyMasterContextKey: InjectionKey<CustomerSatisfactionSurveyMasterContext> = Symbol('customer-satisfaction-surveyMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {CustomerSatisfactionSurveyMasterContext} 主表上下文
 */
export function provideCustomerSatisfactionSurveyMasterContext(): CustomerSatisfactionSurveyMasterContext {
  const selectedMasterRow = ref<CustomerSatisfactionSurveyRowRecord | null>(null)
  const ctx: CustomerSatisfactionSurveyMasterContext = { selectedMasterRow }
  provide(customerSatisfactionSurveyMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {CustomerSatisfactionSurveyMasterContext} 主表上下文
 */
export function useCustomerSatisfactionSurveyMasterContext(): CustomerSatisfactionSurveyMasterContext {
  const ctx = inject(customerSatisfactionSurveyMasterContextKey)
  if (!ctx) {
    throw new Error('useCustomerSatisfactionSurveyMasterContext must be used within customer-satisfaction-survey index')
  }
  return ctx
}

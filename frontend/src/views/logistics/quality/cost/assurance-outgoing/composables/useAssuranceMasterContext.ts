// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/quality/cost/assurance-outgoing/composables
// 文件名称：useAssuranceMasterContext.ts
// 功能描述：品质业务主表主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { QualityAssurance } from '@/types/logistics/quality/cost/assurance.types'

/** 主表选中行上下文 */
export interface QualityAssuranceMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<QualityAssurance | null>
}

const qualityAssuranceMasterContextKey: InjectionKey<QualityAssuranceMasterContext> = Symbol('assuranceMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {QualityAssuranceMasterContext} 主表上下文
 */
export function provideQualityAssuranceMasterContext(): QualityAssuranceMasterContext {
  const selectedMasterRow = ref<QualityAssurance | null>(null)
  const ctx: QualityAssuranceMasterContext = { selectedMasterRow }
  provide(qualityAssuranceMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {QualityAssuranceMasterContext} 主表上下文
 */
export function useQualityAssuranceMasterContext(): QualityAssuranceMasterContext {
  const ctx = inject(qualityAssuranceMasterContextKey)
  if (!ctx) {
    throw new Error('useQualityAssuranceMasterContext must be used within assurance index')
  }
  return ctx
}

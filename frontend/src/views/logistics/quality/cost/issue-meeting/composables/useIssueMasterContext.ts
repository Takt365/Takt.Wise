// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/quality/cost/issue-meeting/composables
// 文件名称：useIssueMasterContext.ts
// 功能描述：品质问题应对主表主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { QualityIssue } from '@/types/logistics/quality/cost/issue.types'

/** 主表选中行上下文 */
export interface QualityIssueMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<QualityIssue | null>
}

const qualityIssueMasterContextKey: InjectionKey<QualityIssueMasterContext> = Symbol('issueMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {QualityIssueMasterContext} 主表上下文
 */
export function provideQualityIssueMasterContext(): QualityIssueMasterContext {
  const selectedMasterRow = ref<QualityIssue | null>(null)
  const ctx: QualityIssueMasterContext = { selectedMasterRow }
  provide(qualityIssueMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {QualityIssueMasterContext} 主表上下文
 */
export function useQualityIssueMasterContext(): QualityIssueMasterContext {
  const ctx = inject(qualityIssueMasterContextKey)
  if (!ctx) {
    throw new Error('useQualityIssueMasterContext must be used within issue index')
  }
  return ctx
}

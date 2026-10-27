// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/quality/cost/incident-item/composables
// 文件名称：useIncidentMasterContext.ts
// 功能描述：品质事故主表主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { QualityIncident } from '@/types/logistics/quality/cost/incident.types'

/** 主表选中行上下文 */
export interface QualityIncidentMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<QualityIncident | null>
}

const qualityIncidentMasterContextKey: InjectionKey<QualityIncidentMasterContext> = Symbol('incidentMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {QualityIncidentMasterContext} 主表上下文
 */
export function provideQualityIncidentMasterContext(): QualityIncidentMasterContext {
  const selecmedMasterRow = ref<QualityIncident | null>(null)
  const ctx: QualityIncidentMasterContext = { selecmedMasterRow }
  provide(qualityIncidentMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {QualityIncidentMasterContext} 主表上下文
 */
export function useQualityIncidentMasterContext(): QualityIncidentMasterContext {
  const ctx = inject(qualityIncidentMasterContextKey)
  if (!ctx) {
    throw new Error('useQualityIncidentMasterContext must be used within incident index')
  }
  return ctx
}

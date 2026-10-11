// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/human-resource/talent/offer/composables
// 文件名称：useOfferMasterContext.ts
// 功能描述：录用信息主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { TalentOffer } from '@/types/human-resource/talent/offer.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type TalentOfferRowRecord = TalentOffer | Record<string, unknown>

/** 主表选中行上下文 */
export interface TalentOfferMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<TalentOfferRowRecord | null>
}

const talentOfferMasterContextKey: InjectionKey<TalentOfferMasterContext> = Symbol('offerMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {TalentOfferMasterContext} 主表上下文
 */
export function provideTalentOfferMasterContext(): TalentOfferMasterContext {
  const selectedMasterRow = ref<TalentOfferRowRecord | null>(null)
  const ctx: TalentOfferMasterContext = { selectedMasterRow }
  provide(talentOfferMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {TalentOfferMasterContext} 主表上下文
 */
export function useTalentOfferMasterContext(): TalentOfferMasterContext {
  const ctx = inject(talentOfferMasterContextKey)
  if (!ctx) {
    throw new Error('useTalentOfferMasterContext must be used within offer index')
  }
  return ctx
}

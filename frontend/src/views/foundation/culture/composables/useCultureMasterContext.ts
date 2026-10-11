// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/foundation/culture/composables
// 文件名称：useCultureMasterContext.ts
// 功能描述：区域文化实体 定义系统支持的多语言区域文化主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { Culture } from '@/types/foundation/culture.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type CultureRowRecord = Culture | Record<string, unknown>

/** 主表选中行上下文 */
export interface CultureMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<CultureRowRecord | null>
}

const cultureMasterContextKey: InjectionKey<CultureMasterContext> = Symbol('cultureMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {CultureMasterContext} 主表上下文
 */
export function provideCultureMasterContext(): CultureMasterContext {
  const selectedMasterRow = ref<CultureRowRecord | null>(null)
  const ctx: CultureMasterContext = { selectedMasterRow }
  provide(cultureMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {CultureMasterContext} 主表上下文
 */
export function useCultureMasterContext(): CultureMasterContext {
  const ctx = inject(cultureMasterContextKey)
  if (!ctx) {
    throw new Error('useCultureMasterContext must be used within culture index')
  }
  return ctx
}

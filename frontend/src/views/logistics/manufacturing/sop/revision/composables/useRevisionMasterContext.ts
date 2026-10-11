// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/sop/revision/composables
// 文件名称：useRevisionMasterContext.ts
// 功能描述：SOP 版本实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { SopRevision } from '@/types/logistics/manufacturing/sop/revision.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type SopRevisionRowRecord = SopRevision | Record<string, unknown>

/** 主表选中行上下文 */
export interface SopRevisionMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<SopRevisionRowRecord | null>
}

const sopRevisionMasterContextKey: InjectionKey<SopRevisionMasterContext> = Symbol('revisionMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {SopRevisionMasterContext} 主表上下文
 */
export function provideSopRevisionMasterContext(): SopRevisionMasterContext {
  const selectedMasterRow = ref<SopRevisionRowRecord | null>(null)
  const ctx: SopRevisionMasterContext = { selectedMasterRow }
  provide(sopRevisionMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {SopRevisionMasterContext} 主表上下文
 */
export function useSopRevisionMasterContext(): SopRevisionMasterContext {
  const ctx = inject(sopRevisionMasterContextKey)
  if (!ctx) {
    throw new Error('useSopRevisionMasterContext must be used within revision index')
  }
  return ctx
}

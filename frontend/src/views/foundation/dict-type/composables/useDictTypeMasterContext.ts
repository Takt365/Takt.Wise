// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/foundation/dict-type/composables
// 文件名称：useDictTypeMasterContext.ts
// 功能描述：字典类型实体 用于定义系统中使用的各种字典分类主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { DictType } from '@/types/foundation/dictType.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type DictTypeRowRecord = DictType | Record<string, unknown>

/** 主表选中行上下文 */
export interface DictTypeMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<DictTypeRowRecord | null>
}

const dictTypeMasterContextKey: InjectionKey<DictTypeMasterContext> = Symbol('dict-typeMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {DictTypeMasterContext} 主表上下文
 */
export function provideDictTypeMasterContext(): DictTypeMasterContext {
  const selectedMasterRow = ref<DictTypeRowRecord | null>(null)
  const ctx: DictTypeMasterContext = { selectedMasterRow }
  provide(dictTypeMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {DictTypeMasterContext} 主表上下文
 */
export function useDictTypeMasterContext(): DictTypeMasterContext {
  const ctx = inject(dictTypeMasterContextKey)
  if (!ctx) {
    throw new Error('useDictTypeMasterContext must be used within dict-type index')
  }
  return ctx
}

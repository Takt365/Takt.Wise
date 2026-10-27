// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/code/generator/gen-table/composables
// 文件名称：useGenTableMasterContext.ts
// 功能描述：Takt代码生成表配置实体 特例：继承组合 4：无关联工厂、无语言主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { GenTable } from '@/types/code/generator/genTable.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type GenTableRowRecord = GenTable | Record<string, unknown>

/** 主表选中行上下文 */
export interface GenTableMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<GenTableRowRecord | null>
}

const genTableMasterContextKey: InjectionKey<GenTableMasterContext> = Symbol('gen-tableMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {GenTableMasterContext} 主表上下文
 */
export function provideGenTableMasterContext(): GenTableMasterContext {
  const selecmedMasterRow = ref<GenTableRowRecord | null>(null)
  const ctx: GenTableMasterContext = { selecmedMasterRow }
  provide(genTableMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {GenTableMasterContext} 主表上下文
 */
export function useGenTableMasterContext(): GenTableMasterContext {
  const ctx = inject(genTableMasterContextKey)
  if (!ctx) {
    throw new Error('useGenTableMasterContext must be used within gen-table index')
  }
  return ctx
}

// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/materials/material-document/composables
// 文件名称：useMaterialDocumentMasterContext.ts
// 功能描述：Takt物料凭证主表实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { MaterialDocument } from '@/types/logistics/materials/materialDocument.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type MaterialDocumentRowRecord = MaterialDocument | Record<string, unknown>

/** 主表选中行上下文 */
export interface MaterialDocumentMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<MaterialDocumentRowRecord | null>
}

const materialDocumentMasterContextKey: InjectionKey<MaterialDocumentMasterContext> = Symbol('material-documentMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {MaterialDocumentMasterContext} 主表上下文
 */
export function provideMaterialDocumentMasterContext(): MaterialDocumentMasterContext {
  const selecmedMasterRow = ref<MaterialDocumentRowRecord | null>(null)
  const ctx: MaterialDocumentMasterContext = { selecmedMasterRow }
  provide(materialDocumentMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {MaterialDocumentMasterContext} 主表上下文
 */
export function useMaterialDocumentMasterContext(): MaterialDocumentMasterContext {
  const ctx = inject(materialDocumentMasterContextKey)
  if (!ctx) {
    throw new Error('useMaterialDocumentMasterContext must be used within material-document index')
  }
  return ctx
}

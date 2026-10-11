// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/routine/document-center/document/composables
// 文件名称：useDocumentMasterContext.ts
// 功能描述：文管中心主实体 支持制度、流程、模板等文档的分类、版本与权限控制主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { Document } from '@/types/routine/document-center/document.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type DocumentRowRecord = Document | Record<string, unknown>

/** 主表选中行上下文 */
export interface DocumentMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<DocumentRowRecord | null>
}

const documentMasterContextKey: InjectionKey<DocumentMasterContext> = Symbol('documentMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {DocumentMasterContext} 主表上下文
 */
export function provideDocumentMasterContext(): DocumentMasterContext {
  const selectedMasterRow = ref<DocumentRowRecord | null>(null)
  const ctx: DocumentMasterContext = { selectedMasterRow }
  provide(documentMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {DocumentMasterContext} 主表上下文
 */
export function useDocumentMasterContext(): DocumentMasterContext {
  const ctx = inject(documentMasterContextKey)
  if (!ctx) {
    throw new Error('useDocumentMasterContext must be used within document index')
  }
  return ctx
}

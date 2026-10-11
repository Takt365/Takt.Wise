// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/routine/news-center/news-comment/composables
// 文件名称：useNewsMasterContext.ts
// 功能描述：新闻中心主实体 支持分类、置顶、推荐、社交统计主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { News } from '@/types/routine/news-center/news.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type NewsRowRecord = News | Record<string, unknown>

/** 主表选中行上下文 */
export interface NewsMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<NewsRowRecord | null>
}

const newsMasterContextKey: InjectionKey<NewsMasterContext> = Symbol('newsMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {NewsMasterContext} 主表上下文
 */
export function provideNewsMasterContext(): NewsMasterContext {
  const selectedMasterRow = ref<NewsRowRecord | null>(null)
  const ctx: NewsMasterContext = { selectedMasterRow }
  provide(newsMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {NewsMasterContext} 主表上下文
 */
export function useNewsMasterContext(): NewsMasterContext {
  const ctx = inject(newsMasterContextKey)
  if (!ctx) {
    throw new Error('useNewsMasterContext must be used within news index')
  }
  return ctx
}

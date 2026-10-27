// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/bom/routing-item/composables
// 文件名称：useRoutingItemMasterContext.ts
// 功能描述：工艺路线明细表实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { RoutingItem } from '@/types/logistics/manufacturing/bom/routingItem.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type RoutingItemRowRecord = RoutingItem | Record<string, unknown>

/** 主表选中行上下文 */
export interface RoutingItemMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<RoutingItemRowRecord | null>
}

const routingItemMasterContextKey: InjectionKey<RoutingItemMasterContext> = Symbol('routing-itemMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {RoutingItemMasterContext} 主表上下文
 */
export function provideRoutingItemMasterContext(): RoutingItemMasterContext {
  const selecmedMasterRow = ref<RoutingItemRowRecord | null>(null)
  const ctx: RoutingItemMasterContext = { selecmedMasterRow }
  provide(routingItemMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {RoutingItemMasterContext} 主表上下文
 */
export function useRoutingItemMasterContext(): RoutingItemMasterContext {
  const ctx = inject(routingItemMasterContextKey)
  if (!ctx) {
    throw new Error('useRoutingItemMasterContext must be used within routing-item index')
  }
  return ctx
}

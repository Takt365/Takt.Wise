// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/bom/routing/composables
// 文件名称：useRoutingMasterContext.ts
// 功能描述：工艺路线主表实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { Routing } from '@/types/logistics/manufacturing/bom/routing.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type RoutingRowRecord = Routing | Record<string, unknown>

/** 主表选中行上下文 */
export interface RoutingMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<RoutingRowRecord | null>
}

const routingMasterContextKey: InjectionKey<RoutingMasterContext> = Symbol('routingMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {RoutingMasterContext} 主表上下文
 */
export function provideRoutingMasterContext(): RoutingMasterContext {
  const selecmedMasterRow = ref<RoutingRowRecord | null>(null)
  const ctx: RoutingMasterContext = { selecmedMasterRow }
  provide(routingMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {RoutingMasterContext} 主表上下文
 */
export function useRoutingMasterContext(): RoutingMasterContext {
  const ctx = inject(routingMasterContextKey)
  if (!ctx) {
    throw new Error('useRoutingMasterContext must be used within routing index')
  }
  return ctx
}

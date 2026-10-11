// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/quality/operation/ipqc-order-item/composables
// 文件名称：useIpqcOrderItemMasterContext.ts
// 功能描述：IPQC制程检验单明细实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { IpqcOrderItem } from '@/types/logistics/quality/operation/ipqcOrderItem.types'

/** 主表选中行上下文 */
export interface IpqcOrderItemMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<IpqcOrderItem | null>
}

const ipqcOrderItemMasterContextKey: InjectionKey<IpqcOrderItemMasterContext> = Symbol('ipqc-order-itemMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {IpqcOrderItemMasterContext} 主表上下文
 */
export function provideIpqcOrderItemMasterContext(): IpqcOrderItemMasterContext {
  const selectedMasterRow = ref<IpqcOrderItem | null>(null)
  const ctx: IpqcOrderItemMasterContext = { selectedMasterRow }
  provide(ipqcOrderItemMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {IpqcOrderItemMasterContext} 主表上下文
 */
export function useIpqcOrderItemMasterContext(): IpqcOrderItemMasterContext {
  const ctx = inject(ipqcOrderItemMasterContextKey)
  if (!ctx) {
    throw new Error('useIpqcOrderItemMasterContext must be used within ipqc-order-item index')
  }
  return ctx
}

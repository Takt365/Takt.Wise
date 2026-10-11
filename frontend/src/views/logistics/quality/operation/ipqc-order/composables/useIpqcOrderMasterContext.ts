// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/quality/operation/ipqc-order/composables
// 文件名称：useIpqcOrderMasterContext.ts
// 功能描述：IPQC制程检验单实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { IpqcOrder } from '@/types/logistics/quality/operation/ipqcOrder.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type IpqcOrderRowRecord = IpqcOrder | Record<string, unknown>

/** 主表选中行上下文 */
export interface IpqcOrderMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<IpqcOrderRowRecord | null>
}

const ipqcOrderMasterContextKey: InjectionKey<IpqcOrderMasterContext> = Symbol('ipqc-orderMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {IpqcOrderMasterContext} 主表上下文
 */
export function provideIpqcOrderMasterContext(): IpqcOrderMasterContext {
  const selectedMasterRow = ref<IpqcOrderRowRecord | null>(null)
  const ctx: IpqcOrderMasterContext = { selectedMasterRow }
  provide(ipqcOrderMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {IpqcOrderMasterContext} 主表上下文
 */
export function useIpqcOrderMasterContext(): IpqcOrderMasterContext {
  const ctx = inject(ipqcOrderMasterContextKey)
  if (!ctx) {
    throw new Error('useIpqcOrderMasterContext must be used within ipqc-order index')
  }
  return ctx
}

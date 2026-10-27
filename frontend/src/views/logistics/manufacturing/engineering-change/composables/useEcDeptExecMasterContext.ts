// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/engineering-change/composables
// 文件名称：useEcDeptExecMasterContext.ts
// 功能描述：执行部门页左栏执行主表选中行上下文（供右侧设变明细面板读取）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { InjectionKey, Ref } from 'vue'
/** 执行部门页主表选中行（各部门执行 DTO） */
export type EcDeptExecMasterRow = Record<string, unknown>

/** 执行部门页主表选中行上下文 */
export interface EcDeptExecMasterContext {
  /** 当前选中的部门执行行 */
  selecmedMasterRow: Ref<EcDeptExecMasterRow | null>
}

const ecDeptExecMasterContextKey: InjectionKey<EcDeptExecMasterContext> = Symbol('ecDeptExecMasterContext')

/**
 * 在执行部门列表页 provide 选中主表行
 * @returns {EcDeptExecMasterContext} 主表上下文
 */
export function provideEcDeptExecMasterContext(): EcDeptExecMasterContext {
  const selecmedMasterRow = ref<EcDeptExecMasterRow | null>(null)
  const ctx: EcDeptExecMasterContext = { selecmedMasterRow }
  provide(ecDeptExecMasterContextKey, ctx)
  return ctx
}

/**
 * 在右侧设变明细面板 inject 主表选中行
 * @returns {EcDeptExecMasterContext} 主表上下文
 */
export function useEcDeptExecMasterContext(): EcDeptExecMasterContext {
  const ctx = inject(ecDeptExecMasterContextKey)
  if (!ctx) {
    throw new Error('useEcDeptExecMasterContext must be used within ec-dept-exec-lr-page')
  }
  return ctx
}

// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/output/personnel-operation-rate/composables
// 文件名称：usePersonnelOperationRateMasterContext.ts
// 功能描述：人员稼动率实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { PersonnelOperationRate } from '@/types/logistics/manufacturing/output/personnel-operation-rate'

/** 主表选中行上下文 */
export interface PersonnelOperationRateMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<PersonnelOperationRate | null>
}

const personnelOperationRateMasterContextKey: InjectionKey<PersonnelOperationRateMasterContext> = Symbol('personnel-operation-rateMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {PersonnelOperationRateMasterContext} 主表上下文
 */
export function providePersonnelOperationRateMasterContext(): PersonnelOperationRateMasterContext {
  const selecmedMasterRow = ref<PersonnelOperationRate | null>(null)
  const ctx: PersonnelOperationRateMasterContext = { selecmedMasterRow }
  provide(personnelOperationRateMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {PersonnelOperationRateMasterContext} 主表上下文
 */
export function usePersonnelOperationRateMasterContext(): PersonnelOperationRateMasterContext {
  const ctx = inject(personnelOperationRateMasterContextKey)
  if (!ctx) {
    throw new Error('usePersonnelOperationRateMasterContext must be used within personnel-operation-rate index')
  }
  return ctx
}

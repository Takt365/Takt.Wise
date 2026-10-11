// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/output/equipment-operation-rate/composables
// 文件名称：useEquipmentOperationRateMasterContext.ts
// 功能描述：机器稼动率实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { EquipmentOperationRate } from '@/types/logistics/manufacturing/output/equipment-operation-rate'

/** 主表选中行上下文 */
export interface EquipmentOperationRateMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<EquipmentOperationRate | null>
}

const equipmentOperationRateMasterContextKey: InjectionKey<EquipmentOperationRateMasterContext> = Symbol('equipment-operation-rateMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {EquipmentOperationRateMasterContext} 主表上下文
 */
export function provideEquipmentOperationRateMasterContext(): EquipmentOperationRateMasterContext {
  const selectedMasterRow = ref<EquipmentOperationRate | null>(null)
  const ctx: EquipmentOperationRateMasterContext = { selectedMasterRow }
  provide(equipmentOperationRateMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {EquipmentOperationRateMasterContext} 主表上下文
 */
export function useEquipmentOperationRateMasterContext(): EquipmentOperationRateMasterContext {
  const ctx = inject(equipmentOperationRateMasterContextKey)
  if (!ctx) {
    throw new Error('useEquipmentOperationRateMasterContext must be used within equipment-operation-rate index')
  }
  return ctx
}

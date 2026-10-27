// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/maintenance/equipment/composables
// 文件名称：useEquipmentMasterContext.ts
// 功能描述：Takt工厂设备实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { Equipment } from '@/types/logistics/maintenance/equipment.types'

/** 主表选中行上下文 */
export interface EquipmentMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<Equipment | null>
}

const equipmentMasterContextKey: InjectionKey<EquipmentMasterContext> = Symbol('equipmentMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {EquipmentMasterContext} 主表上下文
 */
export function provideEquipmentMasterContext(): EquipmentMasterContext {
  const selecmedMasterRow = ref<Equipment | null>(null)
  const ctx: EquipmentMasterContext = { selecmedMasterRow }
  provide(equipmentMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {EquipmentMasterContext} 主表上下文
 */
export function useEquipmentMasterContext(): EquipmentMasterContext {
  const ctx = inject(equipmentMasterContextKey)
  if (!ctx) {
    throw new Error('useEquipmentMasterContext must be used within equipment index')
  }
  return ctx
}

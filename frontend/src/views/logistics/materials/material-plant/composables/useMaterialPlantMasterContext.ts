// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/materials/material-plant/composables
// 文件名称：useMaterialPlantMasterContext.ts
// 功能描述：Takt工厂物料实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { MaterialPlant } from '@/types/logistics/materials/materialPlant.types'

/** 主表选中行上下文 */
export interface MaterialPlantMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selectedMasterRow: Ref<MaterialPlant | null>
}

const materialPlantMasterContextKey: InjectionKey<MaterialPlantMasterContext> = Symbol('material-plantMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {MaterialPlantMasterContext} 主表上下文
 */
export function provideMaterialPlantMasterContext(): MaterialPlantMasterContext {
  const selectedMasterRow = ref<MaterialPlant | null>(null)
  const ctx: MaterialPlantMasterContext = { selectedMasterRow }
  provide(materialPlantMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {MaterialPlantMasterContext} 主表上下文
 */
export function useMaterialPlantMasterContext(): MaterialPlantMasterContext {
  const ctx = inject(materialPlantMasterContextKey)
  if (!ctx) {
    throw new Error('useMaterialPlantMasterContext must be used within material-plant index')
  }
  return ctx
}

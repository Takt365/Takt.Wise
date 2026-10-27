// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/mrp/material-requirements-planning/composables
// 文件名称：useMaterialRequirementsPlanningMasterContext.ts
// 功能描述：物料需求计划 MRP 头表主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { MaterialRequirementsPlanning } from '@/types/logistics/manufacturing/mrp/materialRequirementsPlanning.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type MaterialRequirementsPlanningRowRecord = MaterialRequirementsPlanning | Record<string, unknown>

/** 主表选中行上下文 */
export interface MaterialRequirementsPlanningMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<MaterialRequirementsPlanningRowRecord | null>
}

const materialRequirementsPlanningMasterContextKey: InjectionKey<MaterialRequirementsPlanningMasterContext> = Symbol('material-requirements-planningMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {MaterialRequirementsPlanningMasterContext} 主表上下文
 */
export function provideMaterialRequirementsPlanningMasterContext(): MaterialRequirementsPlanningMasterContext {
  const selecmedMasterRow = ref<MaterialRequirementsPlanningRowRecord | null>(null)
  const ctx: MaterialRequirementsPlanningMasterContext = { selecmedMasterRow }
  provide(materialRequirementsPlanningMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {MaterialRequirementsPlanningMasterContext} 主表上下文
 */
export function useMaterialRequirementsPlanningMasterContext(): MaterialRequirementsPlanningMasterContext {
  const ctx = inject(materialRequirementsPlanningMasterContextKey)
  if (!ctx) {
    throw new Error('useMaterialRequirementsPlanningMasterContext must be used within material-requirements-planning index')
  }
  return ctx
}

// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/logistics/manufacturing/bom/material-cost-trend/composables
// 文件名称：useMaterialCostAnalysisMasterContext.ts
// 功能描述：成本推移页查询条件上下文（工厂/物料类型/机种/产品/物料/期间）
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
/** 成本推移查询上下文 */
export interface BomMaterialCostAnalysisMasterContext {
  /** 查询工厂 */
  queryPlantCode: Ref<string | undefined>
  /** 查询物料类型（可空=全类型） */
  queryMaterialType: Ref<string | undefined>
  /** 查询机种（单选；产品成本推移/成本分析） */
  queryModelCode: Ref<string | undefined>
  /** 查询机种多选（机种成本推移；空=全部） */
  queryModelCodes: Ref<string[]>
  /** 查询产品（可选单选；产品成本推移/成本分析） */
  queryProductCode: Ref<string | undefined>
  /** 查询产品多选（空=已选机种下全部产品） */
  queryProductCodes: Ref<string[]>
  /** 查询物料/组件单选（兼容） */
  queryComponentCode: Ref<string | undefined>
  /** 查询物料/组件多选（机种成本推移；空=全部） */
  queryComponentCodes: Ref<string[]>
  /** 查询年月区间 [yyyy-MM, yyyy-MM] */
  periodRange: Ref<[string, string] | null>
  /** 兼容旧子组件：选中行（单表模式下可为空） */
  selectedMasterRow: Ref<Record<string, unknown> | null>
}

const key: InjectionKey<BomMaterialCostAnalysisMasterContext> = Symbol('material-cost-trend-query')

/**
 * 在推移页 provide 查询上下文
 * @returns {BomMaterialCostAnalysisMasterContext} 上下文
 */
export function provideBomMaterialCostAnalysisMasterContext(): BomMaterialCostAnalysisMasterContext {
  const queryPlantCode = ref<string | undefined>(undefined)
  const queryMaterialType = ref<string | undefined>(undefined)
  const queryModelCode = ref<string | undefined>(undefined)
  const queryModelCodes = ref<string[]>([])
  const queryProductCode = ref<string | undefined>(undefined)
  const queryProductCodes = ref<string[]>([])
  const queryComponentCode = ref<string | undefined>(undefined)
  const queryComponentCodes = ref<string[]>([])
  const periodRange = ref<[string, string] | null>(null)
  const selectedMasterRow = ref<Record<string, unknown> | null>(null)
  const ctx: BomMaterialCostAnalysisMasterContext = {
    queryPlantCode,
    queryMaterialType,
    queryModelCode,
    queryModelCodes,
    queryProductCode,
    queryProductCodes,
    queryComponentCode,
    queryComponentCodes,
    periodRange,
    selectedMasterRow,
  }
  provide(key, ctx)
  return ctx
}

/**
 * inject 查询上下文
 * @returns {BomMaterialCostAnalysisMasterContext} 上下文
 */
export function useBomMaterialCostAnalysisMasterContext(): BomMaterialCostAnalysisMasterContext {
  const ctx = inject(key)
  if (!ctx) {
    throw new Error('useBomMaterialCostAnalysisMasterContext must be used within trend index')
  }
  return ctx
}

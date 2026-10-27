// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/bom/model-cost-trend/composables
// 文件名称：useModelCostTrendQueryContext.ts
// 创建时间：2026-10-20
// 创建人：Takt365(Cursor AI)
// 功能描述：机种成本推移查询条件上下文与期间起止换算
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { InjectionKey, Ref } from 'vue'

/** 机种成本推移查询上下文 */
export interface ModelCostTrendQueryContext {
  /** 查询工厂 */
  queryPlantCode: Ref<string | undefined>
  /** 查询物料类型 */
  queryMaterialType: Ref<string | undefined>
  /** 查询机种（空=全部） */
  queryModelCode: Ref<string | undefined>
  /** 查询年月区间 [yyyy-MM, yyyy-MM] */
  periodRange: Ref<[string, string] | null>
}

const key: InjectionKey<ModelCostTrendQueryContext> = Symbol('model-cost-trend-query')

/**
 * 在机种成本推移页提供查询上下文
 * @returns {ModelCostTrendQueryContext} 上下文
 */
export function provideModelCostTrendQueryContext(): ModelCostTrendQueryContext {
  const queryPlantCode = ref<string | undefined>(undefined)
  const queryMaterialType = ref<string | undefined>(undefined)
  const queryModelCode = ref<string | undefined>(undefined)
  const periodRange = ref<[string, string] | null>(null)
  const ctx: ModelCostTrendQueryContext = {
    queryPlantCode,
    queryMaterialType,
    queryModelCode,
    periodRange,
  }
  provide(key, ctx)
  return ctx
}

/**
 * 读取机种成本推移查询上下文
 * @returns {ModelCostTrendQueryContext} 上下文
 */
export function useModelCostTrendQueryContext(): ModelCostTrendQueryContext {
  const ctx = inject(key)
  if (!ctx) {
    throw new Error('useModelCostTrendQueryContext must be used within model cost trend index')
  }
  return ctx
}

/**
 * 年月区间转为期间起止（yyyy-MM → 当月首日 yyyy-MM-dd）
 * @param range 年月区间
 * @returns periodDateStart / periodDateEnd
 */
export function periodRangeToMovingPricePeriodQuery(
  range: [string, string] | null | undefined,
): { periodDateStart?: string; periodDateEnd?: string } {
  if (!range?.[0]) {
    return {}
  }
  const periodDateStart = `${range[0]}-01`
  if (!range[1]) {
    return { periodDateStart }
  }
  return {
    periodDateStart,
    periodDateEnd: `${range[1]}-01`,
  }
}

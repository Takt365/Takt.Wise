// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/composables
// 文件名称：useAnalysisQueryDefaults.ts
// 创建时间：2026-09-21
// 创建人：Takt365(Cursor AI)
// 功能描述：分析页默认条件：工厂=当前登录公司 RelatedPlant；期间=截止上月共 3 个月（对齐成本差异推移）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { Ref } from 'vue'
import { resolveCurrentCompanyRelatedPlantCode } from '@/composables/useCompanyRelatedPlant'
import { buildDefaultCostingPeriodRange } from '@/views/logistics/manufacturing/bom/material-cost/utils/bomMaterialCostPeriod'

/**
 * 分析页工厂/期间默认值（挂载、重置、公司切换时调用；与成本差异推移一致）
 * @param plantCode 工厂编码 ref
 * @param periodRange 年月区间 ref
 * @returns applyDefaultPeriodRange / applyDefaultPlantFromCompany / applyAnalysisQueryDefaults
 */
export function useAnalysisQueryDefaults(
  plantCode: Ref<string | undefined>,
  periodRange: Ref<[string, string] | null>,
) {
  /** 默认期间：截止上月共 3 个月 */
  function applyDefaultPeriodRange(): void {
    periodRange.value = buildDefaultCostingPeriodRange(3)
  }

  /**
   * 默认工厂：当前登录公司关联工厂编码
   * @returns {Promise<void>}
   */
  async function applyDefaultPlantFromCompany(): Promise<void> {
    const plant = await resolveCurrentCompanyRelatedPlantCode()
    plantCode.value = plant || undefined
  }

  /**
   * 同时写入默认期间与默认工厂
   * @returns {Promise<void>}
   */
  async function applyAnalysisQueryDefaults(): Promise<void> {
    applyDefaultPeriodRange()
    await applyDefaultPlantFromCompany()
  }

  return {
    applyDefaultPeriodRange,
    applyDefaultPlantFromCompany,
    applyAnalysisQueryDefaults,
  }
}

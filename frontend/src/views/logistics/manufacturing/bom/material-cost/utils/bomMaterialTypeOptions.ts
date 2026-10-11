// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/views/logistics/manufacturing/bom/material-cost/utils
// 文件名称：bomMaterialTypeOptions.ts
// 创建时间：2026-08-07
// 创建人：Takt365(Cursor AI)
// 功能描述：分析/推移查询栏物料类型：先拉本表全量 options，再默认选中 FERT
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { getBomCostOptionMaterialTypeOptions } from '@/api/logistics/manufacturing/bom/costOption.api'
import type { TaktSelectOption } from '@/types/common.types'
import { resolveBomCostOptionPeriod } from './bomCostOptionParams'

/** 分析栏默认优先选中的物料类型 */
export const BOM_ANALYSIS_PREFERRED_MATERIAL_TYPE = 'FERT'

/**
 * 从选项中取 DictValue 文本
 * @param {TaktSelectOption} option 选项
 * @returns {string} 类型码
 */
export function bomMaterialTypeOptionValue(option: TaktSelectOption): string {
  return String(option.dictValue ?? '').trim()
}

/**
 * 选项加载完成后默认选中：优先 FERT，否则第一项
 * @param {readonly TaktSelectOption[]} options 本表物料类型选项
 * @returns {string | undefined} 默认选中值
 */
export function pickDefaultBomMaterialType(
  options: readonly TaktSelectOption[],
): string | undefined {
  if (!options?.length) {
    return undefined
  }
  const fert = options.find(
    (o) => bomMaterialTypeOptionValue(o).toUpperCase() === BOM_ANALYSIS_PREFERRED_MATERIAL_TYPE,
  )
  if (fert) {
    return bomMaterialTypeOptionValue(fert)
  }
  return bomMaterialTypeOptionValue(options[0]) || undefined
}

/**
 * 按工厂+期间拉取本表物料类型全量选项，并计算默认选中值（先列表、再默认 FERT）
 * @param {string | undefined | null} plantCode 工厂
 * @param { [string, string] | null | undefined } [periodRange] 年月区间
 * @param {string | null | undefined} [costingMonth] 单月（优先于区间）
 * @returns {Promise<{ options: TaktSelectOption[]; defaultType: string | undefined }>} 选项与默认值
 */
export async function loadBomMaterialTypeOptionsWithDefault(
  plantCode: string | undefined | null,
  periodRange?: [string, string] | null,
  costingMonth?: string | null,
): Promise<{ options: TaktSelectOption[]; defaultType: string | undefined }> {
  const plant = plantCode?.trim()
  const period = resolveBomCostOptionPeriod(periodRange, costingMonth)
  if (!plant || !period) {
    return { options: [], defaultType: undefined }
  }
  const options = (await getBomCostOptionMaterialTypeOptions({
    plantCode: plant,
    periodStart: period.periodStart,
    periodEnd: period.periodEnd,
  })) ?? []
  return {
    options,
    defaultType: pickDefaultBomMaterialType(options),
  }
}

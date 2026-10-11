// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/views/logistics/manufacturing/bom/material-cost/utils
// 文件名称：bomCostOptionParams.ts
// 创建时间：2026-08-21
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 成本查询栏选项期间/级联参数（对齐 TaktBomCostOptionDto）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { BomCostOptionQuery } from '@/types/logistics/manufacturing/bom/costOption.types'

/**
 * 解析选项期间（区间或单月）
 * @param { [string, string] | null | undefined } range 年月区间
 * @param {string | null | undefined} [singleMonth] 单月 yyyy-MM（优先）
 * @returns {{ periodStart: string; periodEnd: string } | undefined} 期间；缺月返回 undefined
 */
export function resolveBomCostOptionPeriod(
  range?: [string, string] | null,
  singleMonth?: string | null,
): { periodStart: string; periodEnd: string } | undefined {
  const month = singleMonth?.trim()
  if (month) {
    return { periodStart: month, periodEnd: month }
  }
  const start = range?.[0]?.trim()
  const end = range?.[1]?.trim() || start
  if (!start || !end) {
    return undefined
  }
  return start <= end
    ? { periodStart: start, periodEnd: end }
    : { periodStart: end, periodEnd: start }
}

/**
 * 是否已具备选项期间（区间或单月）
 * @param { [string, string] | null | undefined } [range] 年月区间
 * @param {string | null | undefined} [singleMonth] 单月
 * @returns {boolean} 可解析期间则为 true
 */
export function hasBomCostOptionPeriod(
  range?: [string, string] | null,
  singleMonth?: string | null,
): boolean {
  return !!resolveBomCostOptionPeriod(range, singleMonth)
}

/**
 * 组装 BOM 成本选项查询参数（无工厂或无期间则 undefined）
 * 机种/产品可空：空则不写入，下游选项不过滤该级
 * @param {object} input 工厂 / 期间 / 可选类型、机种、产品
 * @returns {BomCostOptionQuery | undefined} 查询参数
 */
export function buildBomCostOptionParams(input: {
  plantCode?: string | null
  periodRange?: [string, string] | null
  costingMonth?: string | null
  materialType?: string | null
  modelCode?: string | null
  modelCodes?: string[] | null
  productCode?: string | null
}): BomCostOptionQuery | undefined {
  const plant = input.plantCode?.trim()
  const period = resolveBomCostOptionPeriod(input.periodRange, input.costingMonth)
  if (!plant || !period) {
    return undefined
  }
  const query: BomCostOptionQuery = {
    plantCode: plant,
    periodStart: period.periodStart,
    periodEnd: period.periodEnd,
  }
  const type = input.materialType?.trim()
  if (type) {
    query.materialType = type
  }
  const model = input.modelCode?.trim()
  if (model) {
    query.modelCode = model
  }
  const models = (input.modelCodes ?? [])
    .map((c) => c.trim())
    .filter(Boolean)
  if (models.length > 0) {
    query.modelCodes = models.join(',')
  }
  const product = input.productCode?.trim()
  if (product) {
    query.productCode = product
  }
  return query
}

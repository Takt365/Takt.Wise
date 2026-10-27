// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/views/logistics/manufacturing/bom/material-cost/utils
// 文件名称：bomMaterialCostItemLineCost.ts
// 创建时间：2026-07-14
// 创建人：Takt365(Cursor AI)
// 功能描述：右表明细行成本纯计算（对齐后端 TaktBomMaterialCostItemLineCostHelper：生产相关=X、PCB SECT 标识为空、采购类型=F + qty×(price/unit)，5 位小数）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 成本/单价金额小数位数（与后端 CostDecimalDigits 一致） */
export const COST_DECIMAL_DIGITS = 5

/** 参与合计的明细行最小字段 */
export type BomMaterialCostItemLineCostFields = {
  productionRelated?: string | null
  purchaseType?: string | null
  /** PCB SECT 标识（空才参与；非空一律不参与） */
  pcbSectIndicator?: string | null
  componentQuantity?: number | string | null
  movingAveragePrice?: number | string | null
  movingPriceUnit?: number | string | null
}

/**
 * AwayFromZero 四舍五入（对齐 .NET MidpointRounding.AwayFromZero）
 * @param value 原值
 * @param digits 小数位
 * @returns {number} 舍入后
 */
function roundAwayFromZero(value: number, digits: number): number {
  const factor = 10 ** digits
  const scaled = value * factor
  const rounded = scaled >= 0 ? Math.floor(scaled + 0.5) : Math.ceil(scaled - 0.5)
  return rounded / factor
}

/**
 * 是否参与材料成本合计（productionRelated=X 且 pcbSectIndicator 为空 且 purchaseType=F）
 * @param row 明细行
 * @returns {boolean} 是否计入
 */
export function countsTowardBomMaterialCostItem(row: BomMaterialCostItemLineCostFields | null | undefined): boolean {
  if (!row) {
    return false
  }
  const productionRelated = String(row.productionRelated ?? '').trim()
  const purchaseType = String(row.purchaseType ?? '').trim()
  const pcbSectIndicator = String(row.pcbSectIndicator ?? '').trim()
  return (
    productionRelated.toUpperCase() === 'X'
    && purchaseType.toUpperCase() === 'F'
    && pcbSectIndicator.length === 0
  )
}

/**
 * 取移动价格单位（≤0 时按 1）
 * @param row 明细行
 * @returns {number} 价格单位
 */
export function resolveMovingPriceUnit(row: BomMaterialCostItemLineCostFields): number {
  const unit = Number(row.movingPriceUnit)
  if (!Number.isFinite(unit) || unit <= 0) {
    return 1
  }
  return unit
}

/**
 * 单行组件成本：componentQuantity×(movingAveragePrice÷movingPriceUnit)；非参与资格为 0
 * @param row 明细行
 * @returns {number} 行成本（5 位小数）
 */
export function calculateBomMaterialCostItemLineCost(row: BomMaterialCostItemLineCostFields | null | undefined): number {
  if (!row || !countsTowardBomMaterialCostItem(row)) {
    return 0
  }
  const qty = Number(row.componentQuantity)
  const price = Number(row.movingAveragePrice)
  if (!Number.isFinite(qty) || !Number.isFinite(price)) {
    return 0
  }
  const unitPrice = price / resolveMovingPriceUnit(row)
  return roundAwayFromZero(qty * unitPrice, COST_DECIMAL_DIGITS)
}

/**
 * 合计当前页行成本（参与资格行公式求和，结果保留 5 位小数）
 * @param rows 明细行
 * @returns {number} 合计
 */
export function sumBomMaterialCostItemLineCosts(
  rows: ReadonlyArray<BomMaterialCostItemLineCostFields> | null | undefined,
): number {
  if (!rows?.length) {
    return 0
  }
  let total = 0
  for (const row of rows) {
    total += calculateBomMaterialCostItemLineCost(row)
  }
  return roundAwayFromZero(total, COST_DECIMAL_DIGITS)
}

/**
 * 金额展示（固定 5 位小数；空/非法为 —）
 * @param value 数值
 * @returns {string} 展示文本
 */
export function formatBomMaterialCostAmount(value: unknown): string {
  if (value == null || value === '') {
    return '—'
  }
  const n = typeof value === 'number' ? value : Number(value)
  if (!Number.isFinite(n)) {
    return '—'
  }
  return n.toFixed(COST_DECIMAL_DIGITS)
}

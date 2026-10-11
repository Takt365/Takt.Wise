// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/views/logistics/manufacturing/bom/material-cost/utils
// 文件名称：bomMaterialCostItemSort.ts
// 创建时间：2026-08-19
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 物料成本明细固定序：ProductCode 升序，再行号 LineNumber 升序
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 参与固定序的明细行字段 */
export type BomMaterialCostItemSortFields = {
  productCode?: string | null
  lineNumber?: number | string | null
}

/**
 * ProductCode 升序，再 LineNumber 升序（与后端 CompareProductCodeThenLineNumber 一致）
 * @param a 行 A
 * @param b 行 B
 * @returns 比较结果
 */
export function compareBomMaterialCostItemProductCodeLineNumber(
  a: BomMaterialCostItemSortFields,
  b: BomMaterialCostItemSortFields,
): number {
  const productA = String(a.productCode ?? '').trim()
  const productB = String(b.productCode ?? '').trim()
  const productCmp = productA.localeCompare(productB, undefined, { sensitivity: 'accent' })
  if (productCmp !== 0) {
    return productCmp
  }
  return Number(a.lineNumber ?? 0) - Number(b.lineNumber ?? 0)
}

/**
 * 按 ProductCode + 行号升序排序（返回新数组）
 * @param rows 明细行
 * @returns 排序后列表
 */
export function sortBomMaterialCostItemRowsByProductCodeLineNumber<T extends BomMaterialCostItemSortFields>(
  rows: readonly T[],
): T[] {
  return [...rows].sort(compareBomMaterialCostItemProductCodeLineNumber)
}

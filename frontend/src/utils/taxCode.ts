// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taxCode.ts
// 创建时间：2026-08-11
// 创建人：Takt365(Cursor AI)
// 功能描述：与后端 TaktTaxCodeHelper 对齐：税码 accounting_financial_tax_code → 税率百分比整数
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 内置税码→税率%（与种子 accounting_financial_tax_code.ExtValue 一致） */
const BUILT_IN_TAX_CODE_RATES: Readonly<Record<string, number>> = {
  J0: 0,
  J1: 17,
  J2: 13,
  J3: 11,
  J4: 6,
  J5: 3,
  J6: 16,
  J7: 10,
  J8: 1,
  L1: 5,
  X0: 0,
  X1: 17,
  X2: 13,
  X3: 16,
  A0: 0,
  A1: 0,
  A2: 0,
  A5: 5,
  A8: 8,
  AA: 25,
  AB: 19,
  AC: 19,
  AD: 16,
  AJ: 10,
  AZ: 18,
  E0: 0,
  I8: 8,
  IJ: 10,
  N8: 8,
  V0: 0,
  V1: 0,
  V2: 0,
  V3: 3,
  V4: 5,
  V5: 5,
  V8: 8,
  VA: 25,
  VB: 19,
  VC: 19,
  VD: 16,
  VH: 8,
  VJ: 10,
  VL: 8,
  VM: 10,
  VZ: 18,
}

/**
 * 由税码解析税率百分比整数（如 J2→13）。无法识别返回 null。
 * @param {string | null | undefined} taxCode 税码 DictValue
 * @returns {number | null} 税率百分比
 */
export function tryResolveTaxRatePercent(taxCode: string | null | undefined): number | null {
  if (!taxCode || typeof taxCode !== 'string') {
    return null
  }
  const code = taxCode.trim().toUpperCase()
  if (!code) {
    return null
  }
  const rate = BUILT_IN_TAX_CODE_RATES[code]
  return typeof rate === 'number' ? rate : null
}

/**
 * 有税码时用税码覆盖税率；无税码或无法识别时保留 currentTaxRate
 * @param {string | null | undefined} taxCode 税码
 * @param {number} currentTaxRate 当前税率
 * @returns {number} 应用后的税率百分比
 */
export function applyTaxRateFromTaxCode(
  taxCode: string | null | undefined,
  currentTaxRate: number,
): number {
  const resolved = tryResolveTaxRatePercent(taxCode)
  return resolved ?? currentTaxRate
}

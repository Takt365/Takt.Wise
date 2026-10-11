// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/utils/taktCostElementKatyp
// 文件名称：taktCostElementKatyp.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：成本要素类别与初级/次级类型推导（与 TaktCostElementKatypConstants 对齐）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 初级 KATYP 整型值（对应 01/03/04/11/12/22/90） */
export const PRIMARY_COST_ELEMENT_KATYP = [1, 3, 4, 11, 12, 22, 90] as const

/** 全部有效 KATYP 整型值 */
export const ALL_COST_ELEMENT_KATYP = [
  1, 3, 4, 11, 12, 22, 90,
  21, 31, 41, 42, 43, 50, 51, 52, 61, 66,
] as const

/** 成本要素类型：0=初级，1=次级（字典 accounting_controlling_cost_element_type） */
export const COST_ELEMENT_TYPE_PRIMARY = 0
export const COST_ELEMENT_TYPE_SECONDARY = 1

/**
 * 由 KATYP 类别推导成本要素类型
 * @param category 成本要素类别整型值
 * @returns 0=初级，1=次级；无效输入返回 undefined
 */
export function resolveCostElementTypeFromCategory(
  category: number | string | null | undefined,
): number | undefined {
  if (category === null || category === undefined || category === '') {
    return undefined
  }
  const katyp = typeof category === 'number' ? category : Number(category)
  if (!Number.IsFinite(katyp) || !(ALL_COST_ELEMENT_KATYP as readonly number[]).includes(katyp)) {
    return undefined
  }
  return (PRIMARY_COST_ELEMENT_KATYP as readonly number[]).includes(katyp)
    ? COST_ELEMENT_TYPE_PRIMARY
    : COST_ELEMENT_TYPE_SECONDARY
}

/**
 * 判断 KATYP 类别是否有效
 * @param category 成本要素类别
 * @returns 是否在字典种子定义范围内
 */
export function isValidCostElementKatyp(
  category: number | string | null | undefined,
): boolean {
  if (category === null || category === undefined || category === '') {
    return false
  }
  const katyp = typeof category === 'number' ? category : Number(category)
  return Number.isFinite(katyp) && (ALL_COST_ELEMENT_KATYP as readonly number[]).includes(katyp)
}

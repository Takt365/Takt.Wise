// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/views/logistics/manufacturing/bom/material-zero-price/utils
// 文件名称：bomMaterialCostPeriod.ts
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：核算月默认上月、禁用当月及以后；yyyy-MM → costingDateStart/End（对齐成本分析）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 单月转为核算日期起止
 * @param month 核算月 yyyy-MM
 * @returns costingDateStart / costingDateEnd
 */
export function costingMonthToDateQuery(
  month: string | null | undefined,
): { costingDateStart?: string; costingDateEnd?: string } {
  const value = month?.trim()
  if (!value) {
    return {}
  }
  return periodRangeToCostingDateQuery([value, value])
}

/**
 * 年月区间转为核算日期起止（yyyy-MM → yyyy-MM-dd）
 * @param range 年月区间 [起, 止]
 * @returns costingDateStart / costingDateEnd
 */
export function periodRangeToCostingDateQuery(
  range: [string, string] | null | undefined,
): { costingDateStart?: string; costingDateEnd?: string } {
  if (!range?.[0]) {
    return {}
  }
  const costingDateStart = `${range[0]}-01`
  if (!range[1]) {
    return { costingDateStart }
  }
  const parts = range[1].split('-').map(Number)
  const year = parts[0]
  const month = parts[1]
  if (!year || !month) {
    return { costingDateStart }
  }
  const lastDay = new Date(year, month, 0).getDate()
  const costingDateEnd = `${range[1]}-${String(lastDay).padStart(2, '0')}`
  return { costingDateStart, costingDateEnd }
}

/**
 * 默认核算单月（截止上月；当月无实绩，不含当月）
 * @returns yyyy-MM
 */
export function buildDefaultCostingMonth(): string {
  const now = new Date()
  const end = new Date(now.getFullYear(), now.getMonth() - 1, 1)
  return `${end.getFullYear()}-${String(end.getMonth() + 1).padStart(2, '0')}`
}

/**
 * 年月选择禁用：当月及以后（当月无实绩）
 * @param current Ant Design month picker 当前格（dayjs 兼容：year/month）
 * @returns true=禁用
 */
export function isCostingPeriodMonthDisabled(
  current: { year: () => number; month: () => number } | null | undefined,
): boolean {
  if (!current) {
    return false
  }
  const now = new Date()
  const y = current.year()
  const m = current.month()
  return y > now.getFullYear() || (y === now.getFullYear() && m >= now.getMonth())
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/views/logistics/manufacturing/bom/material-cost/utils
// 文件名称：bomMaterialCostPeriod.ts
// 创建时间：2026-07-14
// 创建人：Takt365(Cursor AI)
// 功能描述：核算单月默认值与 costingDateStart/End 纯转换（分析页默认期间截止上月）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 分析页查询期间最大月数（含起止月；超过易超时）
 */
export const TAKT_ANALYSIS_PERIOD_MAX_MONTHS = 12

/**
 * 默认核算单月（当月 yyyy-MM；浏览/重算页可用）
 * @returns {string} yyyy-MM
 */
export function buildDefaultCostingMonth(): string {
  const now = new Date()
  return `${now.getFullYear()}-${String(now.getMonth() + 1).padStart(2, '0')}`
}

/**
 * 默认核算年月区间（截止上月共 monthCount 个月；当月无实绩，不含当月）
 * @param monthCount 月数（≥1）
 * @returns {[string, string]} [起 yyyy-MM, 止 yyyy-MM]
 */
export function buildDefaultCostingPeriodRange(monthCount = 3): [string, string] {
  const count = Math.max(1, Math.floor(monthCount))
  const now = new Date()
  const end = new Date(now.getFullYear(), now.getMonth() - 1, 1)
  const start = new Date(end.getFullYear(), end.getMonth() - (count - 1), 1)
  const fmt = (d: Date) => `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}`
  return [fmt(start), fmt(end)]
}

/**
 * 设变统计默认年月区间（截止本月共 monthCount 个月，含当月）
 * @param monthCount 月数（≥1）
 * @returns {[string, string]} [起 yyyy-MM, 止 yyyy-MM]
 */
export function buildDefaultThroughCurrentPeriodRange(monthCount = 3): [string, string] {
  const count = Math.max(1, Math.floor(monthCount))
  const now = new Date()
  const end = new Date(now.getFullYear(), now.getMonth(), 1)
  const start = new Date(end.getFullYear(), end.getMonth() - (count - 1), 1)
  const fmt = (d: Date) => `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}`
  return [fmt(start), fmt(end)]
}

/**
 * 将核算月 yyyy-MM 平移指定月数
 * @param month 核算月
 * @param deltaMonths 平移月数（负=往前）
 * @returns {string} yyyy-MM；无法解析则原值
 */
export function shiftCostingMonth(month: string, deltaMonths: number): string {
  const value = month?.trim()
  if (!value) {
    return month
  }
  const parts = value.split('-').map(Number)
  const year = parts[0]
  const mon = parts[1]
  if (!year || !mon) {
    return value
  }
  const next = new Date(year, mon - 1 + deltaMonths, 1)
  return `${next.getFullYear()}-${String(next.getMonth() + 1).padStart(2, '0')}`
}

/**
 * 成本差异推移默认：基准月=前月，比较月=基准月减一月
 * @returns 基准月 / 比较月 yyyy-MM
 */
export function buildDefaultPriceDeltaMonths(
  range?: [string, string] | null,
): { basePeriod: string; comparePeriod: string } {
  const period = range?.[0] && range[1] ? range : buildDefaultCostingPeriodRange(3)
  const start = period[0]
  const end = period[1]
  const previous = shiftCostingMonth(end, -1)
  return {
    basePeriod: end,
    comparePeriod: previous < start ? start : previous,
  }
}

/**
 * 核算月是否落在期间闭区间内（yyyy-MM 字典序）
 * @param month 核算月
 * @param range 期间 [起, 止]
 * @returns {boolean} 在区间内
 */
export function isCostingMonthInRange(
  month: string | null | undefined,
  range: [string, string] | null | undefined,
): boolean {
  const value = month?.trim()
  if (!value || !range?.[0] || !range[1]) {
    return false
  }
  return value >= range[0] && value <= range[1]
}

/**
 * 成本差异推移基准月/比较月禁用：当月及以后，或不在核算期间列内
 * @param current Ant Design month picker 当前格
 * @param range 核算期间
 * @returns {boolean} true=禁用
 */
export function isPriceDeltaMonthDisabled(
  current: { year: () => number; month: () => number } | null | undefined,
  range: [string, string] | null | undefined,
): boolean {
  if (isCostingPeriodMonthDisabled(current)) {
    return true
  }
  if (!current || !range?.[0] || !range[1]) {
    return true
  }
  const key = `${current.year()}-${String(current.month() + 1).padStart(2, '0')}`
  return key < range[0] || key > range[1]
}

/**
 * 年月选择禁用：下月及以后。当月可选
 * @param current Ant Design month picker 当前格（dayjs 兼容：year/month）
 * @returns {boolean} true=禁用
 */
export function isMonthAfterCurrentDisabled(
  current: { year: () => number; month: () => number } | null | undefined,
): boolean {
  if (!current) {
    return false
  }
  const now = new Date()
  const y = current.year()
  const m = current.month()
  return y > now.getFullYear() || (y === now.getFullYear() && m > now.getMonth())
}

/**
 * 年月选择禁用：当月及以后（分析页当月无实绩）
 * @param current Ant Design month picker 当前格（dayjs 兼容：year/month）
 * @returns {boolean} true=禁用
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

/**
 * 闭区间年月数（含起止月）；非法或缺省返回 0
 * @param range [起 yyyy-MM, 止 yyyy-MM]
 * @returns {number} 月数
 */
export function countCostingPeriodMonths(
  range: [string, string] | null | undefined,
): number {
  const start = range?.[0]?.trim()
  const end = range?.[1]?.trim()
  if (!start || !end) {
    return 0
  }
  const startParts = start.split('-').map(Number)
  const endParts = end.split('-').map(Number)
  const sy = startParts[0]
  const sm = startParts[1]
  const ey = endParts[0]
  const em = endParts[1]
  if (!sy || !sm || !ey || !em) {
    return 0
  }
  return (ey - sy) * 12 + (em - sm) + 1
}

/**
 * 分析查询期间是否超过上限（默认 12 个月）
 * @param range 年月区间
 * @param maxMonths 最大月数（含起止）
 * @returns {boolean} true=超限
 */
export function isAnalysisPeriodRangeExceedingMax(
  range: [string, string] | null | undefined,
  maxMonths: number = TAKT_ANALYSIS_PERIOD_MAX_MONTHS,
): boolean {
  const count = countCostingPeriodMonths(range)
  return count > Math.max(1, Math.floor(maxMonths))
}

/**
 * 将任意可解析为 yyyy-MM 的值规范为键（RangePicker calendarChange / dayjs）
 * @param value 字符串或带 year/month 的对象
 * @returns {string | null} yyyy-MM
 */
export function toCostingMonthKey(
  value: string | { year: () => number; month: () => number } | null | undefined,
): string | null {
  if (value == null) {
    return null
  }
  if (typeof value === 'string') {
    const trimmed = value.trim()
    return /^\d{4}-\d{2}/.test(trimmed) ? trimmed.slice(0, 7) : null
  }
  if (typeof value.year === 'function' && typeof value.month === 'function') {
    return `${value.year()}-${String(value.month() + 1).padStart(2, '0')}`
  }
  return null
}

/**
 * 分析期间 RangePicker：默认禁用当月及以后；allowCurrentMonth 时只禁用下月及以后。
 * 已选一端时，另一端须落在「锚点 ±(maxMonths-1)」内
 *（允许先点较晚月再点较早月，由 RangePicker 交换起止；闭区间仍 ≤ maxMonths）
 * @param current 当前格
 * @param pickingRange calendarChange 暂存的起止（可能只有一端）
 * @param maxMonths 最大月数（含起止）
 * @param allowCurrentMonth true 时本月可选
 * @returns {boolean} true=禁用
 */
export function isAnalysisPeriodMonthDisabled(
  current: { year: () => number; month: () => number } | null | undefined,
  pickingRange: [string | null, string | null] | null | undefined,
  maxMonths: number = TAKT_ANALYSIS_PERIOD_MAX_MONTHS,
  allowCurrentMonth = false,
): boolean {
  if (allowCurrentMonth ? isMonthAfterCurrentDisabled(current) : isCostingPeriodMonthDisabled(current)) {
    return true
  }
  const key = toCostingMonthKey(current)
  if (!key) {
    return false
  }
  const maxOffset = Math.max(0, Math.floor(maxMonths) - 1)
  const start = pickingRange?.[0] ? toCostingMonthKey(pickingRange[0]) : null
  const end = pickingRange?.[1] ? toCostingMonthKey(pickingRange[1]) : null
  const anchor = start && !end ? start : end && !start ? end : null
  if (!anchor) {
    return false
  }
  const min = shiftCostingMonth(anchor, -maxOffset)
  const max = shiftCostingMonth(anchor, maxOffset)
  return key < min || key > max
}

/**
 * 核算单月转为核算日期起止（yyyy-MM → 当月首末日）
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

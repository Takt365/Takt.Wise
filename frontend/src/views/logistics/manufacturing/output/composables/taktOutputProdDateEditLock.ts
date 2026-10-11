// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/output/composables
// 文件名称：taktOutputProdDateEditLock.ts
// 功能描述：制造产出模块生产日期编辑锁定与日历可选范围（与后端 TaktAssyOutputProdDateEditLockHelper 对齐）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { Dayjs } from 'dayjs'

/** 默认编辑截止日：生产日期次月的第几天（含当日仍可编辑） */
export const OUTPUT_PROD_DATE_EDIT_CUTOFF_DAY = 5

/**
 * 格式化为 YYYY-MM-DD（本地）
 * @param date 日期
 * @returns 日期文本
 */
export function formatOutputProdDateYmd(date: Date): string {
  const y = date.getFullYear()
  const m = String(date.getMonth() + 1).padStart(2, '0')
  const d = String(date.getDate()).padStart(2, '0')
  return `${y}-${m}-${d}`
}

/**
 * 解析 YYYY-MM-DD 为本地日期（无效时返回 null）
 * @param prodDateYmd 生产日期
 * @returns 日期对象或 null
 */
export function parseOutputProdDateYmd(prodDateYmd: string): Date | null {
  const text = prodDateYmd?.trim().slice(0, 10)
  if (!/^\d{4}-\d{2}-\d{2}$/.test(text)) {
    return null
  }
  const [yearText, monthText, dayText] = text.split('-')
  const year = Number(yearText)
  const month = Number(monthText)
  const day = Number(dayText)
  if (!Number.isFinite(year) || !Number.isFinite(month) || !Number.isFinite(day)) {
    return null
  }
  const date = new Date(year, month - 1, day)
  if (date.getFullYear() !== year || date.getMonth() !== month - 1 || date.getDate() !== day) {
    return null
  }
  return date
}

/**
 * 解析生产日期对应的编辑截止日（所属月份的下月 cutoff 日）
 * @param prodDateYmd 生产日期 YYYY-MM-DD
 * @param cutoffDayOfNextMonth 次月截止日（1～28，含当日仍可编辑）
 * @returns 截止日或 null（生产日期无效时）
 */
export function resolveOutputProdDateEditDeadline(
  prodDateYmd: string,
  cutoffDayOfNextMonth: number = OUTPUT_PROD_DATE_EDIT_CUTOFF_DAY,
): Date | null {
  if (cutoffDayOfNextMonth < 1 || cutoffDayOfNextMonth > 28) {
    throw new RangeError('cutoffDayOfNextMonth must be between 1 and 28')
  }
  const prodDate = parseOutputProdDateYmd(prodDateYmd)
  if (!prodDate) {
    return null
  }
  const nextMonthFirst = new Date(prodDate.getFullYear(), prodDate.getMonth() + 1, 1)
  return new Date(nextMonthFirst.getFullYear(), nextMonthFirst.getMonth(), cutoffDayOfNextMonth)
}

/**
 * 生产日期是否已锁定（不可新增/修改/删除）
 * @param prodDateYmd 生产日期 YYYY-MM-DD
 * @param referenceDate 参考日期（默认当前业务日）
 * @param cutoffDayOfNextMonth 次月截止日（含当日仍可编辑）
 * @returns 已锁定时为 true
 */
export function isOutputProdDateLocked(
  prodDateYmd: string,
  referenceDate: Date = new Date(),
  cutoffDayOfNextMonth: number = OUTPUT_PROD_DATE_EDIT_CUTOFF_DAY,
): boolean {
  const deadline = resolveOutputProdDateEditDeadline(prodDateYmd, cutoffDayOfNextMonth)
  if (!deadline) {
    return false
  }
  const ref = new Date(referenceDate.getFullYear(), referenceDate.getMonth(), referenceDate.getDate())
  return ref.getTime() > deadline.getTime()
}

/**
 * 取日期部分（本地 0 点）
 * @param date 日期
 * @returns 仅日期部分
 */
function startOfLocalDay(date: Date): Date {
  return new Date(date.getFullYear(), date.getMonth(), date.getDate())
}

/**
 * 月初（本地）
 * @param date 日期
 * @returns 当月 1 日
 */
function startOfLocalMonth(date: Date): Date {
  return new Date(date.getFullYear(), date.getMonth(), 1)
}

/**
 * 解析生产日期可选范围（与日历 disabledDate 一致）
 * @param referenceDate 参考日期（默认当前业务日）
 * @param cutoffDayOfNextMonth 次月截止日（含当日仍可编辑上月数据）
 * @returns 可选最小/最大日期（仅日期部分；最大为今日，不可选未来）
 */
export function resolveOutputSelectableProdDateRange(
  referenceDate: Date = new Date(),
  cutoffDayOfNextMonth: number = OUTPUT_PROD_DATE_EDIT_CUTOFF_DAY,
): { minDate: Date; maxDate: Date } {
  const ref = startOfLocalDay(referenceDate)
  const maxDate = ref
  const minDate =
    ref.getDate() > cutoffDayOfNextMonth
      ? startOfLocalMonth(ref)
      : startOfLocalMonth(new Date(ref.getFullYear(), ref.getMonth() - 1, 1))
  return { minDate, maxDate }
}

/**
 * 生产日期是否可在表单/日历中选择（未锁定、在允许月份内且不超过今日）
 * @param prodDateYmd 生产日期 YYYY-MM-DD
 * @param referenceDate 参考日期
 * @param cutoffDayOfNextMonth 次月截止日
 * @returns 可选时为 true
 */
export function isOutputProdDateSelectable(
  prodDateYmd: string,
  referenceDate: Date = new Date(),
  cutoffDayOfNextMonth: number = OUTPUT_PROD_DATE_EDIT_CUTOFF_DAY,
): boolean {
  const date = parseOutputProdDateYmd(prodDateYmd)
  if (!date) {
    return false
  }
  if (isOutputProdDateLocked(prodDateYmd, referenceDate, cutoffDayOfNextMonth)) {
    return false
  }
  const { minDate, maxDate } = resolveOutputSelectableProdDateRange(referenceDate, cutoffDayOfNextMonth)
  const day = startOfLocalDay(date).getTime()
  return day >= startOfLocalDay(minDate).getTime() && day <= startOfLocalDay(maxDate).getTime()
}

/**
 * 新增默认生产日期：昨天；若不可选则回退到今天
 * @returns YYYY-MM-DD
 */
export function resolveDefaultOutputProdDateYmd(): string {
  const today = new Date()
  today.setHours(0, 0, 0, 0)
  const yesterday = new Date(today)
  yesterday.setDate(yesterday.getDate() - 1)
  const yesterdayYmd = formatOutputProdDateYmd(yesterday)
  if (isOutputProdDateSelectable(yesterdayYmd)) {
    return yesterdayYmd
  }
  const todayYmd = formatOutputProdDateYmd(today)
  if (isOutputProdDateSelectable(todayYmd)) {
    return todayYmd
  }
  return todayYmd
}

/**
 * 从主表行读取生产日期 YYYY-MM-DD
 * @param record 主表行
 * @returns 生产日期文本
 */
export function getOutputProdDateYmdFromRecord(
  record: Record<string, unknown> | null | undefined,
): string {
  const raw = record?.prodDate
  if (raw == null) {
    return ''
  }
  return String(raw).trim().slice(0, 10)
}

/**
 * a-date-picker disabledDate：锁定/跨月/上月（5 日后）/未来日期不可选
 * @param current 日历单元格日期
 * @param referenceDate 参考日期
 * @returns 是否禁用
 */
export function outputProdDatePickerDisabledDate(
  current: Dayjs,
  referenceDate: Date = new Date(),
): boolean {
  if (!current) {
    return false
  }
  return !isOutputProdDateSelectable(current.format('YYYY-MM-DD'), referenceDate)
}

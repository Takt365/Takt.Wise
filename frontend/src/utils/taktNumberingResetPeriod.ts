// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Frontend.Utils
// 文件名称：taktNumberingResetPeriod.ts
// 功能描述：编码规则 DateFormat ↔ ResetPeriod 对齐（与 TaktNumberingHelper 一致）
// ========================================

/** 日期格式 → 必须使用的重置周期（sys_reset_period dictValue） */
const DATE_FORMAT_TO_RESET_PERIOD: Readonly<Record<string, string>> = {
  none: 'none',
  yyyy: 'year',
  yyyyMM: 'month',
  yyyyMMdd: 'day',
  yyyyMMddHH: 'hour',
}

/** 重置周期别名 → 标准 dictValue */
const RESET_PERIOD_ALIASES: Readonly<Record<string, string>> = {
  none: 'none',
  day: 'day',
  daily: 'day',
  month: 'month',
  monthly: 'month',
  year: 'year',
  yearly: 'year',
  hour: 'hour',
  hourly: 'hour',
  minute: 'hour',
  minutely: 'hour',
}

/**
 * 归一化并迁移已废弃日期格式（yyyyMMddHHmm → yyyyMMddHH）
 * @param dateFormat 日期格式
 * @returns 标准键；空/none 为 none
 */
export function normalizeNumberingDateFormatKey(dateFormat?: string | null): string {
  if (!dateFormat?.trim()) {
    return 'none'
  }
  const trimmed = dateFormat.trim()
  if (trimmed.toLowerCase() === 'none') {
    return 'none'
  }
  if (trimmed.toLowerCase() === 'yyyyMMddHHmm') {
    return 'yyyyMMddHH'
  }
  return trimmed
}

/**
 * 归一化重置周期为字典 dictValue
 * @param resetPeriod 重置周期
 * @returns sys_reset_period dictValue
 */
export function normalizeNumberingResetPeriod(resetPeriod?: string | null): string {
  const key = resetPeriod?.trim().toLowerCase() ?? 'none'
  return RESET_PERIOD_ALIASES[key] ?? 'none'
}

/**
 * 根据 DateFormat 解析必须使用的重置周期
 * @param dateFormat 日期格式
 * @returns 匹配的重置周期；不支持时返回 null
 */
export function resolveRequiredResetPeriod(dateFormat?: string | null): string | null {
  const key = normalizeNumberingDateFormatKey(dateFormat)
  return DATE_FORMAT_TO_RESET_PERIOD[key] ?? null
}

/**
 * 重置周期是否与 DateFormat 粒度匹配
 * @param dateFormat 日期格式
 * @param resetPeriod 重置周期
 * @returns 是否匹配
 */
export function isNumberingResetPeriodMatched(
  dateFormat?: string | null,
  resetPeriod?: string | null,
): boolean {
  const required = resolveRequiredResetPeriod(dateFormat)
  if (!required) {
    return false
  }
  return normalizeNumberingResetPeriod(resetPeriod) === required
}

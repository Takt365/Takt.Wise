// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/views/dashboard/dataBoard.types/utils
// 文件名称：formatDashboardAmount.ts
// 功能描述：数据看板金额按量级缩放（亿/万/千），供 KPI 卡展示
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 金额单位键（对应 dashboard.data-board.page.amountunit.*） */
export type DashboardAmountUnitKey = 'yi' | 'wan' | 'qian'

/** 缩放后的金额展示 */
export interface DashboardScaledAmount {
  /** 缩放后的数值（保留符号） */
  value: number
  /** 单位键；未达千级时为 null */
  unitKey: DashboardAmountUnitKey | null
  /** 小数位 */
  precision: number
}

/**
 * 是否为货币前缀（¥ / ￥），用于触发金额量级缩放
 * @param prefix a-statistic prefix
 * @returns {boolean} 是否按金额缩放
 */
export function isDashboardCurrencyPrefix(prefix?: string): boolean {
  if (!prefix || typeof prefix !== 'string') {
    return false
  }
  return prefix.includes('¥') || prefix.includes('￥')
}

/**
 * 按绝对值量级缩放金额：≥亿→亿，≥万→万，≥千→千（例：10015434 → 1001.5434 万）
 * @param amount 原始金额（元）
 * @param fallbackPrecision 未缩放时的小数位，默认 2
 * @returns {DashboardScaledAmount} 缩放后的 value / unitKey / precision
 */
export function scaleDashboardCurrencyAmount(
  amount: number,
  fallbackPrecision = 2,
): DashboardScaledAmount {
  if (!Number.isFinite(amount)) {
    return { value: 0, unitKey: null, precision: fallbackPrecision }
  }
  const abs = Math.abs(amount)
  const sign = amount < 0 ? -1 : 1
  // 缩放后保留 4 位小数（如 1001.5434 万）
  const scaledPrecision = 4
  if (abs >= 100_000_000) {
    return { value: sign * (abs / 100_000_000), unitKey: 'yi', precision: scaledPrecision }
  }
  if (abs >= 10_000) {
    return { value: sign * (abs / 10_000), unitKey: 'wan', precision: scaledPrecision }
  }
  if (abs >= 1_000) {
    return { value: sign * (abs / 1_000), unitKey: 'qian', precision: scaledPrecision }
  }
  return { value: amount, unitKey: null, precision: fallbackPrecision }
}

/**
 * 去掉末尾无意义的 0 与小数点（1000.0000 → 1000；1001.5430 → 1001.543）
 * @param value 数值
 * @param precision 最大小数位
 * @returns {string} 格式化数字串
 */
export function formatDashboardScaledNumber(value: number, precision: number): string {
  if (!Number.isFinite(value)) {
    return '0'
  }
  const fixed = value.toFixed(Math.max(0, precision))
  if (!fixed.includes('.')) {
    return fixed
  }
  return fixed.replace(/\.?0+$/, '')
}

/**
 * 金额文案：数值 + 单位（如 1000万、5.432千）；未达千级为「元」金额两位小数
 * @param amount 原始金额（元）
 * @param resolveUnit 单位键 → 文案（如 wan→万）；无单位时返回空串
 * @returns {string} 如 1000万
 */
export function formatDashboardAmountLabel(
  amount: number,
  resolveUnit: (unitKey: DashboardAmountUnitKey) => string,
): string {
  const scaled = scaleDashboardCurrencyAmount(amount)
  const num = formatDashboardScaledNumber(scaled.value, scaled.precision)
  if (!scaled.unitKey) {
    return num
  }
  return `${num}${resolveUnit(scaled.unitKey)}`
}

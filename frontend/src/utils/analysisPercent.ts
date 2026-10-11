// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：analysisPercent.ts
// 创建时间：2026-09-21
// 创建人：Takt365(Cursor AI)
// 功能描述：分析页「期间%/环比%」格式化：展示为 0.00% 时该单元格留空；不筛选、不隐藏整行记录
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 涨跌百分比数值形态：ratio=小数比率（0.12→12%）；points=百分点（12→12%） */
export type AnalysisVariancePercentMode = 'ratio' | 'points'

/**
 * 是否为展示层 0.00%（有值且四舍五入到 2 位小数后为 0.00）
 * 注意：null/NaN 不是 0%，返回 false（由调用方用占位符处理空值）
 * @param {number | null | undefined} value 涨跌百分比数值
 * @param {AnalysisVariancePercentMode} [mode='ratio'] 数值形态
 * @returns {boolean} 仅当有效数值展示为 0.00% 时 true
 */
export function isAnalysisVariancePercentZero(
  value?: number | null,
  mode: AnalysisVariancePercentMode = 'ratio',
): boolean {
  if (value == null || Number.isNaN(value)) {
    return false
  }
  const display = mode === 'ratio' ? value * 100 : value
  return Math.abs(display) < 0.005
}

/**
 * 分析页期间%/环比% 文案：0.00% 返回空串（仅该格不显示，记录行仍保留）
 * @param {number | null | undefined} value 涨跌百分比数值
 * @param {AnalysisVariancePercentMode} [mode='ratio'] 数值形态
 * @param {string} [nullText='—'] 空值占位（无数据时）
 * @returns {string} 百分比文本；0.00% 为空串；无数据为 nullText
 */
export function formatAnalysisVariancePercent(
  value?: number | null,
  mode: AnalysisVariancePercentMode = 'ratio',
  nullText = '—',
): string {
  if (value == null || Number.isNaN(value)) {
    return nullText
  }
  if (isAnalysisVariancePercentZero(value, mode)) {
    return ''
  }
  const display = mode === 'ratio' ? value * 100 : value
  return `${display.toFixed(2)}%`
}

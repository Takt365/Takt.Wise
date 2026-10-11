// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/utils
// 文件名称：taktSequence.ts
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：行号/排序号序列生成，与后端 TaktSequenceGenerator、TaktSequenceDefaults 对齐
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 明细行号默认起始值（currentMax 为 0 时） */
export const LINE_NUMBER_DEFAULT_START = 10

/** 明细行号递增步长 */
export const LINE_NUMBER_STEP = 10

/**
 * 计算下一个序列值
 * @param currentMax 当前最大值
 * @param defaultStart 首条默认值
 * @param step 步长
 * @returns 下一个值
 */
function computeNext(currentMax: number, defaultStart: number, step: number): number {
  if (currentMax <= 0) {
    return defaultStart
  }
  return currentMax + step
}

/**
 * 生成下一个明细行号（与后端 GenerateNextLineNumber 一致）
 * @param currentMaxLineNumber 当前最大行号（0 表示第一行，从 10 开始）
 * @returns 下一个行号
 */
export function generateNextLineNumber(currentMaxLineNumber = 0): number {
  return computeNext(currentMaxLineNumber, LINE_NUMBER_DEFAULT_START, LINE_NUMBER_STEP)
}

/**
 * 批量生成明细行号序列（与后端 GenerateLineNumberSequence 一致）
 * @param count 数量
 * @param startFrom 起始基准值（0 表示从 10 开始）
 * @returns 行号数组
 */
export function generateLineNumberSequence(count: number, startFrom = 0): number[] {
  if (count <= 0) {
    return []
  }
  const result: number[] = []
  let current = startFrom
  for (let index = 0; index < count; index += 1) {
    current = computeNext(current, LINE_NUMBER_DEFAULT_START, LINE_NUMBER_STEP)
    result.push(current)
  }
  return result
}

/**
 * 从行号列表取当前最大值
 * @param values 行号集合
 * @returns 最大值（空集合为 0）
 */
export function resolveMaxLineNumber(values: readonly number[]): number {
  if (!values.length) {
    return 0
  }
  return values.reduce((max, value) => (Number.isFinite(value) ? Math.max(max, value) : max), 0)
}

/**
 * 解析子表下一行号（已提交最大行号 + 当前未保存行号）
 * @param committedMaxLineNumber 后端返回的已占用最大行号（IsObsolete 子表传 0）
 * @param currentRows 当前子表行
 * @param lineNumberField 行号字段名
 * @returns 下一可用行号
 */
export function resolveNextDetailLineNumber(
  committedMaxLineNumber: number,
  currentRows: readonly Record<string, unknown>[],
  lineNumberField = 'lineNumber',
): number {
  const rowMax = resolveMaxLineNumber(
    currentRows.map((row) => Number(row[lineNumberField]) || 0),
  )
  return generateNextLineNumber(Math.max(committedMaxLineNumber, rowMax))
}

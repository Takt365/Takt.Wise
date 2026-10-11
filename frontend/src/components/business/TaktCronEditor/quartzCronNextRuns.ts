// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/components/business/TaktCronEditor
// 文件名称：quartzCronNextRuns.ts
// 创建时间：2026-06-28
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz Cron 最近运行时间预览（独立模块，按需动态加载 cron-parser）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

let cronParserModulePromise: Promise<typeof import('cron-parser')> | undefined

/**
 * 懒加载 cron-parser 模块
 * @returns cron-parser 模块
 */
async function loadCronParserModule(): Promise<typeof import('cron-parser')> {
  if (!cronParserModulePromise) {
    cronParserModulePromise = import('cron-parser')
  }
  return cronParserModulePromise
}

/**
 * 格式化为 YYYY-MM-DD HH:mm:ss
 * @param date 运行时间
 * @returns 格式化字符串
 */
function formatCronRunTime(date: Date): string {
  const pad = (value: number) => String(value).padStart(2, '0')
  return `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())} ${pad(date.getHours())}:${pad(date.getMinutes())}:${pad(date.getSeconds())}`
}

/**
 * 将误写在「日」字段的 nL（如 1L）纠正到「周」字段，便于 Quartz / cron-parser 解析
 * @param expression 原始 Cron 表达式
 * @returns 规范化后的六段表达式
 */
export function normalizeQuartzCronExpression(expression: string): string {
  const trimmed = String(expression ?? '').trim()
  if (!trimmed) {
    return trimmed
  }
  const parts = trimmed.split(/\s+/)
  if (parts.length < 6) {
    return trimmed
  }
  const [second, minute, hour, day, month, week] = parts
  const dayWeekMatch = day.match(/^(\d+)L$/)
  if (dayWeekMatch && week === '?') {
    return `${second} ${minute} ${hour} ? ${month} ${dayWeekMatch[1]}L`
  }
  return trimmed
}

/**
 * 计算最近 N 次运行时间（Quartz 6 字段）
 * @param expression Cron 表达式
 * @param count 条数，默认 5
 * @returns 格式化时间列表；解析失败返回空数组
 */
export async function getQuartzCronNextRunTimes(expression: string, count = 5): Promise<string[]> {
  const normalized = normalizeQuartzCronExpression(expression)
  if (!normalized || count < 1) {
    return []
  }
  try {
    const { CronExpressionParser } = await loadCronParserModule()
    const interval = CronExpressionParser.parse(normalized, {
      currentDate: new Date(),
    })
    const results: string[] = []
    for (let i = 0; i < count; i += 1) {
      results.push(formatCronRunTime(interval.next().toDate()))
    }
    return results
  } catch {
    return []
  }
}

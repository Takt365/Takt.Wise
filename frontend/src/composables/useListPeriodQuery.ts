// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/composables
// 文件名称：useListPeriodQuery.ts
// 创建时间：2026-10-30
// 创建人：Takt365(Cursor AI)
// 功能描述：列表查询栏期间：默认当前月，写入日期起止（结束日含当天）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { ref } from 'vue'
import { useAnalysisPeriodRangeLimit } from '@/composables/useAnalysisPeriodRangeLimit'
import { TAKT_ANALYSIS_PERIOD_MAX_MONTHS } from '@/views/logistics/manufacturing/bom/material-cost/utils/bomMaterialCostPeriod'

/** 制造页生产期间文案（流水线生成页使用 common.page.form.periodRange） */
export const PROD_PERIOD_I18N_KEY = 'logistics.manufacturing.output.prod-date.page.periodRange'

/**
 * 当前自然月，起止相同
 * @returns {[string, string]} [yyyy-MM, yyyy-MM]
 */
export function buildCurrentMonthPeriodRange(): [string, string] {
  const now = new Date()
  const month = `${now.getFullYear()}-${String(now.getMonth() + 1).padStart(2, '0')}`
  return [month, month]
}

/**
 * 期间年月转日期起止。结束日取该月最后一天（不含时分秒）
 * @param range 年月区间
 * @returns 日期起止；缺一端时只返回已有的一侧
 */
export function prodPeriodRangeToQuery(range: [string, string] | null): { start?: string; end?: string } {
  if (!range?.[0] && !range?.[1]) {
    return {}
  }
  const start = range?.[0] ? `${range[0]}-01` : undefined
  if (!range?.[1]) {
    return { start }
  }
  const parts = range[1].split('-').map(Number)
  const year = parts[0]
  const month = parts[1]
  if (!year || !month) {
    return { start }
  }
  const lastDay = new Date(year, month, 0).getDate()
  return {
    start,
    end: `${range[1]}-${String(lastDay).padStart(2, '0')}`,
  }
}

/**
 * 查询栏期间：默认当前月，可选到本月，最多 12 个月
 * @returns 期间值、日期禁用、重置与写入查询 DTO
 */
export function useListPeriodQuery() {
  /** 年月区间，默认当前月 */
  const range = ref<[string, string] | null>(buildCurrentMonthPeriodRange())
  /** 可选到本月，闭区间最多 12 个月 */
  const limit = useAnalysisPeriodRangeLimit(TAKT_ANALYSIS_PERIOD_MAX_MONTHS, { allowCurrentMonth: true })

  /**
   * 恢复为当前月
   * @returns {void}
   */
  function reset(): void {
    range.value = buildCurrentMonthPeriodRange()
  }

  /**
   * 起止年月是否都已选择
   * @returns {boolean}
   */
  function hasPeriod(): boolean {
    return Boolean(range.value?.[0] && range.value?.[1])
  }

  /**
   * 把期间写成日期起止（开始月 1 日 0 点～结束月最后一天 23:59:59）
   * @param query 列表查询
   * @param startKey 开始字段名
   * @param endKey 结束字段名
   * @returns {void}
   */
  function applyDateRange(query: object, startKey: string, endKey: string): void {
    const dates = prodPeriodRangeToQuery(range.value)
    const target = query as Record<string, string | undefined>
    if (dates.start) {
      target[startKey] = dates.start
    }
    if (dates.end) {
      target[endKey] = `${dates.end}T23:59:59`
    }
  }

  /**
   * 把期间写成 yyyy-MM 起止（业务年月一类字段）
   * @param query 列表查询
   * @param startKey 开始年月字段名
   * @param endKey 结束年月字段名
   * @returns {void}
   */
  function applyMonthRange(query: object, startKey: string, endKey: string): void {
    const target = query as Record<string, string | undefined>
    if (range.value?.[0]) {
      target[startKey] = range.value[0]
    }
    if (range.value?.[1]) {
      target[endKey] = range.value[1]
    }
  }

  /**
   * 把生产期间写入查询 DTO 的 prodDateStart / prodDateEnd（覆盖高级查询里的同名字段）
   * @param query 列表查询
   * @returns {void}
   */
  function applyToQuery(query: { prodDateStart?: string; prodDateEnd?: string }): void {
    applyDateRange(query, 'prodDateStart', 'prodDateEnd')
  }

  return { range, limit, reset, hasPeriod, applyToQuery, applyDateRange, applyMonthRange }
}

/** 与 useListPeriodQuery 同一实现，供已有制造/品质页继续引用 */
export const useProdPeriodQuery = useListPeriodQuery

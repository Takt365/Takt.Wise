// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/composables
// 文件名称：useAnalysisPeriodRangeLimit.ts
// 创建时间：2026-09-21
// 创建人：Takt365(Cursor AI)
// 功能描述：分析/推移页年月 RangePicker：最大可选上月；闭区间最多 N 个月（默认 12），超限月份不可选
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { message } from 'ant-design-vue'
import { ref } from 'vue'
import { useI18n } from 'vue-i18n'
import {
  TAKT_ANALYSIS_PERIOD_MAX_MONTHS,
  isAnalysisPeriodMonthDisabled,
  isAnalysisPeriodRangeExceedingMax,
  toCostingMonthKey,
} from '@/views/logistics/manufacturing/bom/material-cost/utils/bomMaterialCostPeriod'

/**
 * 分析页期间 RangePicker 上限（表头禁用超限月 + 查询前校验）
 * - 最大月=上月（当月及以后禁用）
 * - 已选一端后，另一端仅可在锚点前后共 maxMonths 个月内点选
 * @param maxMonths 最大月数（含起止），默认 12
 * @returns disabledDate / calendarChange / openChange / assertPeriodRangeOk
 */
export function useAnalysisPeriodRangeLimit(maxMonths: number = TAKT_ANALYSIS_PERIOD_MAX_MONTHS) {
  const { t } = useI18n()
  /** calendarChange 暂存（选一端时限制另一端） */
  const pickingRange = ref<[string | null, string | null] | null>(null)

  /**
   * RangePicker disabled-date：当月及以后禁用；相对锚点超 maxMonths 的月份禁用
   * @param current 当前月格（dayjs）
   * @returns {boolean} true=禁用（不可选中）
   */
  function disabledDate(current: { year: () => number; month: () => number } | null): boolean {
    return isAnalysisPeriodMonthDisabled(current, pickingRange.value, maxMonths)
  }

  /**
   * 选月过程回调（优先 dateStrings，兼容 value-format=YYYY-MM）
   * @param dates Dayjs / string 起止
   * @param dateStrings 格式化后的起止字符串
   */
  function onCalendarChange(
    dates: [unknown, unknown] | null,
    dateStrings?: [string, string] | null,
  ): void {
    const fromStrings = dateStrings
      ? [
          dateStrings[0]?.trim() ? toCostingMonthKey(dateStrings[0]) : null,
          dateStrings[1]?.trim() ? toCostingMonthKey(dateStrings[1]) : null,
        ] as [string | null, string | null]
      : null
    if (fromStrings && (fromStrings[0] || fromStrings[1])) {
      pickingRange.value = fromStrings
      return
    }
    if (!dates) {
      pickingRange.value = null
      return
    }
    pickingRange.value = [toCostingMonthKey(dates[0] as never), toCostingMonthKey(dates[1] as never)]
  }

  /**
   * 面板开关：开/关均清空锚点，避免沿用上次选择误禁月份
   * @param _open 是否打开
   */
  function onOpenChange(_open: boolean): void {
    pickingRange.value = null
  }

  /**
   * 查询前校验期间是否超限；超限则 warning 并返回 false
   * @param range 年月区间
   * @returns {boolean} true=可通过
   */
  function assertPeriodRangeOk(range: [string, string] | null | undefined): boolean {
    if (!isAnalysisPeriodRangeExceedingMax(range, maxMonths)) {
      return true
    }
    message.warning(t('common.page.form.periodRangeMaxMonths', { max: maxMonths }))
    return false
  }

  return {
    maxMonths,
    disabledDate,
    onCalendarChange,
    onOpenChange,
    assertPeriodRangeOk,
  }
}

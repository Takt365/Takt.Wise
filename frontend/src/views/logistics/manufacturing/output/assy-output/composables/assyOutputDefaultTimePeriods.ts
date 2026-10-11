// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/output/assy-output/composables
// 文件名称：assyOutputDefaultTimePeriods.ts
// 功能描述：组立日报新增固定 13 生产时段（主表 StdCapacity>0 时生成；与后端 TaktAssyOutputTimePeriodConstants.DefaultTimePeriods 对齐）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 新增组立日报时固定的生产时段（共 13 条）
 * @description 须与 backend TaktAssyOutputTimePeriodConstants.DefaultTimePeriods 保持一致
 */
export const ASSY_OUTPUT_DEFAULT_TIME_PERIODS = [
  '08:00:00~09:00:00',
  '09:00:00~10:00:00',
  '10:10:00~11:10:00',
  '11:10:00~12:10:00',
  '13:30:00~14:30:00',
  '14:30:00~15:30:00',
  '15:40:00~16:40:00',
  '16:40:00~17:40:00',
  '18:30:00~19:30:00',
  '19:30:00~20:30:00',
  '20:30:00~21:30:00',
  '21:30:00~22:30:00',
  '22:30:00~23:30:00',
] as const

/**
 * 生成新增态默认子表行（13 时段，行号 10/20/…；仅主表标准产能 > 0 时由表单调用）
 * @param prodOrderCode 工单号（可空，提交时由主表回填）
 * @returns 子表行数组
 */
export function buildDefaultAssyOutputDetailRows(
  prodOrderCode?: string | null
): Record<string, unknown>[] {
  const orderCode = String(prodOrderCode ?? '').trim()
  return ASSY_OUTPUT_DEFAULT_TIME_PERIODS.map((timePeriod, index) => ({
    lineNumber: (index + 1) * 10,
    timePeriod,
    prodOrderCode: orderCode,
    stdCapacity: 0,
    prodActualQty: 0,
    downtimeMinutes: 0,
    downtimeReason: '',
    downtimeDescription: '',
    unachievedReason: '',
    unachievedDescription: '',
    confirmMinutes: 0,
    mixedProd: 0,
    isObsolete: 0,
  }))
}

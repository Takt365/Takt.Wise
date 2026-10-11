// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/utils/taktProductionStat
// 文件名称：taktProductionStat.ts
// 功能描述：生产统计派生计算（与 backend TaktProductionStatHelper 语义对齐）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 组立标准生产稼动率类型：人员 */
export const ASSY_STANDARD_OPERATION_RATE_TYPE_PERSONNEL = 1

/** 标准生产稼动率启用状态 */
export const STANDARD_OPERATION_RATE_STATUS_ENABLED = 1

/**
 * 将标准生产稼动率规范为比例（0.85=85%）；历史百分数（如 85）自动除以 100
 * @param operationRate 稼动率
 * @returns 比例值
 */
export function normalizeStandardOperationRate(operationRate: number): number {
  const rate = Number(operationRate)
  if (!Number.isFinite(rate) || rate <= 0) {
    return 0
  }
  return rate > 1 ? rate / 100 : rate
}

/**
 * 计算达成率（%，保留 2 位小数；标准产能为 0 时返回 0）
 * @param prodActualQty 实际生产数量
 * @param stdCapacity 标准产能
 * @returns 达成率(%)
 */
export function calculateAchievementRatePercent(prodActualQty: number, stdCapacity: number): number {
  if (!Number.isFinite(stdCapacity) || stdCapacity <= 0) {
    return 0
  }
  const qty = Number.isFinite(prodActualQty) ? prodActualQty : 0
  const rate = (qty / stdCapacity) * 100
  return roundDecimal(rate, 2)
}

/**
 * 组立日报明细是否无产量且无报工
 * @param prodActualQty 实际生产数量
 * @param confirmMinutes 报工工时(分钟)
 * @returns 无产量且无报工时为 true
 */
export function isAssyDetailWithoutProduction(prodActualQty: number, confirmMinutes: number): boolean {
  const prodQty = Number.isFinite(prodActualQty) ? prodActualQty : 0
  const confirm = Number.isFinite(confirmMinutes) ? confirmMinutes : 0
  return prodQty <= 0 && confirm <= 0
}

/**
 * 组立日报明细投入工时（分钟）：无产量且无报工时为 0；报工工时大于 0 时等于报工工时，否则为直接人员 × 60
 * @param directLabor 主表直接人员
 * @param confirmMinutes 报工工时(分钟)
 * @param prodActualQty 实际生产数量
 * @returns 投入工时(分钟)
 */
export function calculateAssyInputMinutes(
  directLabor: number,
  confirmMinutes = 0,
  prodActualQty = 0
): number {
  if (isAssyDetailWithoutProduction(prodActualQty, confirmMinutes)) {
    return 0
  }
  const confirm = Number.isFinite(confirmMinutes) ? confirmMinutes : 0
  if (confirm > 0) {
    return confirm
  }
  const labor = Number.isFinite(directLabor) ? Math.trunc(directLabor) : 0
  return labor * 60
}

/**
 * 组立日报明细实际工时（分钟）
 * @param inputMinutes 投入工时
 * @param confirmMinutes 报工工时
 * @param downtimeMinutes 停线时间
 * @param prodActualQty 实际生产数量（>0 时不允许实际工时为 0 或负数）
 * @returns 实际工时(分钟)
 */
export function calculateAssyActualMinutes(
  inputMinutes: number,
  confirmMinutes: number,
  downtimeMinutes: number,
  prodActualQty = 0
): number {
  const prodQty = Number.isFinite(prodActualQty) ? prodActualQty : 0
  if (isAssyDetailWithoutProduction(prodQty, confirmMinutes)) {
    return 0
  }
  const confirm = Number.isFinite(confirmMinutes) ? confirmMinutes : 0
  const input = Number.isFinite(inputMinutes) ? inputMinutes : 0
  const downtime = Number.isFinite(downtimeMinutes) ? Math.trunc(downtimeMinutes) : 0
  const baseMinutes = confirm > 0 ? confirm : input
  let actual = baseMinutes - downtime
  if (actual < 0) {
    actual = 0
  }
  if (prodQty > 0 && actual <= 0 && input > 0) {
    actual = input - downtime
    if (actual < 0) {
      actual = 0
    }
  }
  return actual
}

/**
 * 组立日报明细间接工时（分钟）：无产量且无报工时为 0；否则为间接人数×向下取整(实际工时÷直接人数)
 * @param indirectLabor 主表间接人员
 * @param directLabor 主表直接人员
 * @param actualMinutes 实际工时(分钟)
 * @param confirmMinutes 报工工时(分钟)
 * @param prodActualQty 实际生产数量
 * @returns 间接工时(分钟)
 */
export function calculateAssyIndirectMinutes(
  indirectLabor: number,
  directLabor: number,
  actualMinutes: number,
  confirmMinutes = 0,
  prodActualQty = 0
): number {
  if (isAssyDetailWithoutProduction(prodActualQty, confirmMinutes)) {
    return 0
  }
  const indirect = Number.isFinite(indirectLabor) ? Math.trunc(indirectLabor) : 0
  const direct = Number.isFinite(directLabor) ? Math.trunc(directLabor) : 0
  if (indirect <= 0 || direct <= 0) {
    return 0
  }
  const actual = Number.isFinite(actualMinutes) ? actualMinutes : 0
  const perDirectLabor = Math.floor(actual / direct)
  return indirect * perDirectLabor
}

/**
 * 组立日报明细标准产能：默认快照主表 StdCapacity；有报工工时时按报工工时÷标准工时×稼动率重算该行
 * @param stdMinutes 主表标准工时(分钟)
 * @param masterHourlyStdCapacity 主表小时标准产能（表头 StdCapacity）
 * @param confirmMinutes 报工工时(分钟)
 * @param operationRate 标准生产稼动率（比例或历史百分数）
 * @param _prodActualQty 实际生产数量（保留参数，与后端签名对齐；本公式不参与分支）
 * @returns 明细标准产能（保留 2 位小数，四舍五入）
 */
export function calculateAssyDetailStdCapacity(
  stdMinutes: number,
  masterHourlyStdCapacity: number,
  confirmMinutes: number,
  operationRate: number,
  _prodActualQty = 0
): number {
  const confirm = Number.isFinite(confirmMinutes) ? confirmMinutes : 0
  if (confirm > 0) {
    const minutes = Number.isFinite(stdMinutes) ? stdMinutes : 0
    const rate = normalizeStandardOperationRate(Number(operationRate))
    if (minutes <= 0 || rate <= 0) {
      return 0
    }
    return roundDecimal((confirm / minutes) * rate, 2)
  }
  const hourly = Number.isFinite(masterHourlyStdCapacity) ? masterHourlyStdCapacity : 0
  return hourly > 0 ? roundDecimal(hourly, 2) : 0
}

/**
 * 同桶有产量/报工明细总数 → 混合生产笔数
 * @param activeDetailCount 桶内有产量/报工明细总数
 * @returns 0=非混合；N≥2 表示同时段共有 N 笔
 */
export function calculateAssyMixedProdCount(activeDetailCount: number): number {
  const count = Number.isFinite(activeDetailCount) ? Math.trunc(activeDetailCount) : 0
  return count >= 2 ? count : 0
}

/** @deprecated 使用 calculateAssyMixedProdCount */
export function calculateAssyMixedProdPeerCount(bucketDetailCount: number): number {
  return calculateAssyMixedProdCount(bucketDetailCount)
}

/**
 * 组立日报小时标准产能
 * @param directLabor 直接人员
 * @param stdMinutes 标准工时(分钟)
 * @param operationRate 稼动率（比例，如 0.85 表示 85%）
 * @returns 标准产能（保留 2 位小数，四舍五入）
 */
export function calculateAssyStdCapacity(
  directLabor: number,
  stdMinutes: number,
  operationRate: number
): number {
  const labor = Number.isFinite(Number(directLabor)) ? Math.trunc(Number(directLabor)) : 0
  const minutes = Number.isFinite(Number(stdMinutes)) ? Number(stdMinutes) : 0
  const rate = normalizeStandardOperationRate(Number(operationRate))
  if (labor <= 0 || minutes <= 0 || rate <= 0) {
    return 0
  }
  const capacity = (labor * 60) / minutes * rate
  return roundDecimal(capacity, 2)
}

/**
 * 汇总标准工序时间得到标准工时（分钟）
 * @param rows 标准工序时间行（ConvertedMinutes 优先，为 0 时取 StandardMinutes）
 * @returns 标准工时
 */
export function calculateStdMinutesFromOperationTimes(
  rows: ReadonlyArray<{ convertedMinutes?: number; standardMinutes?: number; ConvertedMinutes?: number; StandardMinutes?: number }>
): number {
  if (!rows?.length) {
    return 0
  }
  let total = 0
  for (const row of rows) {
    const converted = Number(row.convertedMinutes ?? row.ConvertedMinutes)
    const standard = Number(row.standardMinutes ?? row.StandardMinutes)
    total += converted > 0 ? converted : (Number.isFinite(standard) ? standard : 0)
  }
  return roundDecimal(total, 2)
}

/** 组立日报固定清洁停线生产时段 */
export const ASSY_CLEANING_TIME_PERIODS = [
  '11:10:00~12:10:00',
  '16:40:00~17:40:00',
] as const

/** 清洁时段停线原因字典标签 */
export const ASSY_CLEANING_STOP_REASON_LABEL = '清洁'

/** 清洁时段停线原因字典项 DictValue（logistics_manufacturing_stop_reason · 清洁） */
export const ASSY_CLEANING_STOP_REASON_DICT_VALUE = '12'

/** 清洁时段每位直接人员停线分钟数 */
export const ASSY_CLEANING_DOWNTIME_MINUTES_PER_DIRECT_LABOR = 4

/**
 * 规范化组立日报生产时段分隔符（~ / - / -- / ～ 等统一为 ~）
 * @param timePeriod 生产时段
 * @returns 规范化后的时段字符串
 */
export function normalizeAssyTimePeriod(timePeriod: string | null | undefined): string {
  const trimmed = (timePeriod ?? '').trim()
  if (!trimmed) {
    return ''
  }
  return trimmed.replace(
    /(\d{1,2}:\d{2}:\d{2})\s*[-–—～~]+\s*(\d{1,2}:\d{2}:\d{2})/,
    '$1~$2',
  )
}

/**
 * 是否为组立日报固定清洁停线生产时段
 * @param timePeriod 生产时段
 * @returns 是清洁时段时为 true
 */
export function isAssyCleaningTimePeriod(timePeriod: string | null | undefined): boolean {
  const normalized = normalizeAssyTimePeriod(timePeriod)
  if (!normalized) {
    return false
  }
  return (ASSY_CLEANING_TIME_PERIODS as readonly string[]).includes(normalized)
}

/**
 * 计算清洁时段停线时间（分钟）：直接人员×4
 * @param directLabor 主表直接人员
 * @returns 停线时间(分钟)
 */
export function calculateAssyCleaningDowntimeMinutes(directLabor: number): number {
  const labor = Number.isFinite(Number(directLabor)) ? Math.max(0, Math.trunc(Number(directLabor))) : 0
  return labor * ASSY_CLEANING_DOWNTIME_MINUTES_PER_DIRECT_LABOR
}

/**
 * 清洁时段：仅当实际生产数量 > 0 时写入停线原因「清洁」与停线时间（直接人员×4）；否则清空停线字段
 * @param row 子表明细行
 * @param directLabor 主表直接人员
 */
export function applyAssyCleaningPeriodDefaults(
  row: {
    timePeriod?: unknown
    prodActualQty?: unknown
    downtimeMinutes?: unknown
    downtimeReason?: unknown
  },
  directLabor: number,
): void {
  if (!isAssyCleaningTimePeriod(String(row.timePeriod ?? ''))) {
    return
  }
  const prodQty = Number(row.prodActualQty) || 0
  if (prodQty <= 0) {
    row.downtimeMinutes = 0
    row.downtimeReason = ''
    return
  }
  row.downtimeReason = ASSY_CLEANING_STOP_REASON_LABEL
  row.downtimeMinutes = calculateAssyCleaningDowntimeMinutes(directLabor)
}

/**
 * @param value 数值
 * @param digits 小数位
 * @returns 四舍五入结果
 */
function roundDecimal(value: number, digits: number): number {
  if (!Number.isFinite(value)) {
    return 0
  }
  const factor = 10 ** digits
  return Math.round(value * factor) / factor
}

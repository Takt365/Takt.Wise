// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/output/assy-output/composables
// 文件名称：useAssyOutputDerivedCalc.ts
// 功能描述：组立日报主表/明细派生字段前端预览计算（与后端 TaktProductionStatHelper 对齐）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { getStandardOperationTimeByMaterial } from '@/api/logistics/manufacturing/bom/standardOperationTime.api'
import { getEffectiveStandardOperationRatePercent } from '@/api/logistics/manufacturing/mps/standardOperationRate.api'
import {
  ASSY_STANDARD_OPERATION_RATE_TYPE_PERSONNEL,
  calculateAchievementRatePercent,
  calculateAssyActualMinutes,
  calculateAssyDetailStdCapacity,
  calculateAssyIndirectMinutes,
  calculateAssyInputMinutes,
  calculateStdMinutesFromOperationTimes,
} from '@/utils/taktProductionStat'

/** 主表派生计算所需快照 */
export interface AssyOutputMasterCalcSnapshot {
  directLabor: number
  indirectLabor: number
  stdCapacity: number
  stdMinutes: number
  operationRatePercent: number
}

/** 明细派生计算入参 */
export interface AssyOutputDetailCalcInput {
  prodActualQty?: number
  downtimeMinutes?: number
  confirmMinutes?: number
  mixedProd?: number
}

/** 明细派生计算结果 */
export interface AssyOutputDetailCalcResult {
  inputMinutes: number
  actualMinutes: number
  indirectMinutes: number
  stdCapacity: number
  achievementRate: number
}

/**
 * 按生产日期解析人员标准生产稼动率（%）
 * @param plantCode 工厂代码
 * @param prodDate 生产日期 YYYY-MM-DD
 * @returns 稼动率(%)
 */
export async function resolvePersonnelOperationRatePercent(
  plantCode: string,
  prodDate: string
): Promise<number> {
  const plant = plantCode?.trim()
  const dateText = prodDate?.trim()
  if (!plant || !dateText) {
    return 0
  }
  const rate = await getEffectiveStandardOperationRatePercent(
    plant,
    dateText.slice(0, 10),
    ASSY_STANDARD_OPERATION_RATE_TYPE_PERSONNEL
  )
  const numericRate = Number(rate)
  return Number.isFinite(numericRate) ? numericRate : 0
}

/**
 * 按物料解析标准工时（GET TaktStandardOperationTimes/by-material）
 * @param materialCode 物料编码
 * @param plantCode 工厂代码
 * @param prodDate 生产日期 YYYY-MM-DD
 * @returns 标准工时(分钟)
 */
export async function resolveStdMinutesByMaterial(
  materialCode: string,
  plantCode: string,
  prodDate: string
): Promise<number> {
  const material = materialCode?.trim()
  const dateText = prodDate?.trim().slice(0, 10)
  if (!material || !dateText) {
    return 0
  }
  const rows = await getStandardOperationTimeByMaterial(
    material,
    plantCode?.trim() || undefined,
    dateText
  )
  return calculateStdMinutesFromOperationTimes(rows ?? [])
}

/**
 * 计算单条组立日报明细派生字段（预览；MixedProd 编辑态沿用服务端值）
 * @param master 主表快照
 * @param input 明细输入
 * @returns 派生结果
 */
export function calculateAssyOutputDetailDerived(
  master: AssyOutputMasterCalcSnapshot,
  input: AssyOutputDetailCalcInput
): AssyOutputDetailCalcResult {
  const directLabor = Number.isFinite(master.directLabor) ? master.directLabor : 0
  const indirectLabor = Number.isFinite(master.indirectLabor) ? master.indirectLabor : 0
  const stdMinutes = Number.isFinite(master.stdMinutes) ? master.stdMinutes : 0
  const masterHourlyStdCapacity = Number.isFinite(master.stdCapacity) ? master.stdCapacity : 0
  const confirmMinutes = Number(input.confirmMinutes) || 0
  const prodActualQty = Number(input.prodActualQty) || 0
  const inputMinutes = calculateAssyInputMinutes(directLabor, confirmMinutes, prodActualQty)
  const stdCapacity = calculateAssyDetailStdCapacity(
    stdMinutes,
    masterHourlyStdCapacity,
    confirmMinutes,
    Number.isFinite(master.operationRatePercent) ? master.operationRatePercent : 0,
    prodActualQty
  )
  const actualMinutes = calculateAssyActualMinutes(
    inputMinutes,
    confirmMinutes,
    Number(input.downtimeMinutes) || 0,
    prodActualQty
  )
  const indirectMinutes = calculateAssyIndirectMinutes(
    indirectLabor,
    directLabor,
    actualMinutes,
    confirmMinutes,
    prodActualQty
  )
  const achievementRate = calculateAchievementRatePercent(prodActualQty, stdCapacity)
  return { inputMinutes, actualMinutes, indirectMinutes, stdCapacity, achievementRate }
}

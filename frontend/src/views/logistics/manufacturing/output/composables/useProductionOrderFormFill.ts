// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/output/composables
// 文件名称：useProductionOrderFormFill.ts
// 功能描述：制造产出表单按工单回填（独立 composable；generate-vue 不会覆盖）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { getProductionOrderFormFill } from '@/api/logistics/manufacturing/aps/productionOrderFormFill.api'
import type {
  ProductionOrderFormFill,
  ProductionOrderFormFillDefaultDetail,
} from '@/types/logistics/manufacturing/aps/productionOrderFormFill.types'

/** 回填选项 */
export interface ApplyProductionOrderFormFillOptions {
  /**
   * 是否请求默认明细预览（PCBA）
   */
  includeDefaultDetails?: boolean
  /**
   * 直接作业人数（组立：用于 stdCapacity）
   */
  directLabor?: number
  /**
   * 工厂代码（可选精确匹配）
   */
  plantCode?: string
  /**
   * 生产日期 YYYY-MM-DD
   */
  prodDate?: string
}

/**
 * 将回填结果写入主表 formState（工单类别/机种/物料/批次/数量/序号/工时）
 * @param target 表单模型
 * @param fill 回填 DTO
 * @param applyStdHours 是否写入 stdMinutes/stdCapacity（组立）
 */
export function applyProductionOrderFormFillToMaster(
  target: Record<string, unknown>,
  fill: ProductionOrderFormFill,
  applyStdHours = true
): void {
  if (fill.plantCode) {
    target.plantCode = fill.plantCode
  }
  target.prodOrderType = fill.prodOrderType ?? ''
  target.modelCode = fill.modelCode ?? ''
  target.materialCode = fill.materialCode ?? ''
  target.batchCode = fill.batchCode ?? ''
  target.prodOrderQty = fill.prodOrderQty
  target.serialCode = fill.serialCode ?? ''
  if (applyStdHours) {
    target.stdMinutes = fill.stdMinutes
    // null/undefined 表示无法计算（如直接人数未填），须清空以免沿用旧产能
    target.stdCapacity = fill.stdCapacity ?? undefined
  }
}

/**
 * 将默认明细预览映射为 PCBA 子表行
 * @param details 默认明细
 * @param prodOrderCode 工单号
 * @returns 子表行
 */
export function mapFormFillDefaultDetailsToPcbaRows(
  details: readonly ProductionOrderFormFillDefaultDetail[] | null | undefined,
  prodOrderCode: string
): Record<string, unknown>[] {
  if (!details?.length) {
    return []
  }
  return details.map((row) => ({
    prodOrderCode,
    lineNumber: row.lineNumber,
    timePeriod: row.workCenter,
    teamCode: '',
    prodEquipCode: '',
    directLabor: 0,
    indirectLabor: 0,
    shiftNo: 1,
    stdMinutes: Number(row.standardMinutes) || 0,
    stdLaborCapacity: 0,
    stdShorts: row.standardShorts ?? 0,
    stdEquipmentCapacity: 0,
    pcbBoardType: '',
    panelSide: '',
    batchQty: 0,
    dailyCompletedQty: 0,
    totalCompletedQty: 0,
    completedStatus: 0,
    serialCode: '',
    defectCount: 0,
    downtimeMinutes: 0,
    downtimeReason: '',
    downtimeDescription: '',
    inputMinutes: 0,
    actualMinutes: 0,
    repairMinutes: 0,
    switchCount: 0,
    switchTime: 0,
    stopTime: 0,
    totalMinutes: 0,
    unachievedReason: '',
    unachievedDescription: '',
    confirmMinutes: 0,
    mixedProd: 0,
    achievementRate: 0,
    isObsolete: 0,
  }))
}

/**
 * 拉取工单表单回填（失败返回 null，不抛错）
 * @param prodOrderCode 工单号
 * @param options 选项
 * @returns 回填 DTO 或 null
 */
export async function fetchProductionOrderFormFill(
  prodOrderCode: string,
  options?: ApplyProductionOrderFormFillOptions
): Promise<ProductionOrderFormFill | null> {
  const code = prodOrderCode?.trim()
  if (!code) {
    return null
  }
  try {
    return await getProductionOrderFormFill({
      prodOrderCode: code,
      plantCode: options?.plantCode,
      prodDate: options?.prodDate,
      directLabor: options?.directLabor,
      includeDefaultDetails: options?.includeDefaultDetails === true,
    })
  } catch {
    return null
  }
}

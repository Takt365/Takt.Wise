// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/bom
// 文件名称：calculate.api.ts
// 创建时间：2026-08-14
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 计算 API（计算成本 / 重算成本 / 计算平均成本 / 回填采购价 / 计算最近采购成本；对应 TaktBomCalculates）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request'
import type {
  BomCalculateAverageQuery,
  BomCalculateAverageResult,
  BomCalculateCostResult,
  BomCalculatePurchaseInfoBackfillResult,
  BomCalculateQuery,
  BomCalculateSubmitted,
} from '@/types/logistics/manufacturing/bom/calculate.types'

/** API 路由前缀（对应 TaktBomCalculatesController） */
const BOM_CALCULATE_API_BASE = 'TaktBomCalculates'

/**
 * 工厂下拉选项 URL（查询栏 TaktSelect）
 * @returns {string} 相对路径
 */
export function getBomCalculatePlantOptionsUrl(): string {
  return `${BOM_CALCULATE_API_BASE}/plant-options`
}

/**
 * 提交后台计算成本（明细合计写入主表；完成后 SignalR 通知）
 * @param {BomCalculateQuery} queryDto 工厂/物料类型/机种可选；须单个核算月
 * @returns {Promise<BomCalculateSubmitted>} 已提交回执
 */
export function sumBomCalculateCost(queryDto: BomCalculateQuery): Promise<BomCalculateSubmitted> {
  return request<BomCalculateSubmitted>({
    url: `${BOM_CALCULATE_API_BASE}/sum`,
    method: 'put',
    data: queryDto,
  })
}

/**
 * 提交后台重算成本（归档旧成本后按所选物料类型重写；完成后 SignalR 通知）
 * @param {BomCalculateQuery} queryDto 工厂/物料类型/机种可选；须单个核算月
 * @returns {Promise<BomCalculateSubmitted>} 已提交回执
 */
export function recalculateBomCalculateCost(queryDto: BomCalculateQuery): Promise<BomCalculateSubmitted> {
  return request<BomCalculateSubmitted>({
    url: `${BOM_CALCULATE_API_BASE}/recalculate`,
    method: 'put',
    data: queryDto,
  })
}

/**
 * 计算平均成本（先回填空机种/空物料类型，再按类型+机种写月均；始终全部物料类型）
 * @param {BomCalculateAverageQuery} queryDto 工厂 + 核算期间；机种可选；materialType 忽略
 * @returns {Promise<BomCalculateAverageResult>} 平均结果
 */
export function calculateBomCalculateAverage(
  queryDto: BomCalculateAverageQuery,
): Promise<BomCalculateAverageResult> {
  return request<BomCalculateAverageResult>({
    url: `${BOM_CALCULATE_API_BASE}/average`,
    method: 'post',
    data: queryDto,
    timeout: 300000,
  })
}

/**
 * 按核算日回填 BOM 明细采购组织/采购组/供应商/净价/采购货币/采购价格单位
 * @param {BomCalculateQuery} queryDto 工厂/物料类型/机种可选；须单个核算月
 * @returns {Promise<BomCalculatePurchaseInfoBackfillResult>} 回填统计
 */
export function backfillBomCalculatePurchaseInfo(
  queryDto: BomCalculateQuery,
): Promise<BomCalculatePurchaseInfoBackfillResult> {
  return request<BomCalculatePurchaseInfoBackfillResult>({
    url: `${BOM_CALCULATE_API_BASE}/purchase-price`,
    method: 'put',
    data: queryDto,
    timeout: 300000,
  })
}

/**
 * 计算最近采购成本（与产品月成本同一快照；行金额=组件数量×(净价÷采购价格单位)）
 * @param {BomCalculateQuery} queryDto 工厂/物料类型/机种可选；须单个核算月
 * @returns {Promise<BomCalculateCostResult>} 合计统计
 */
export function sumBomCalculateLatestPurchaseCost(
  queryDto: BomCalculateQuery,
): Promise<BomCalculateCostResult> {
  return request<BomCalculateCostResult>({
    url: `${BOM_CALCULATE_API_BASE}/latest-purchase-cost`,
    method: 'put',
    data: queryDto,
    timeout: 300000,
  })
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/bom
// 文件名称：costOption.api.ts
// 创建时间：2026-08-21
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 成本查询栏共用选项 API（TaktBomCostOptions）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request'
import type { TaktSelectOption } from '@/types/common.types'
import type { BomCostOptionQuery } from '@/types/logistics/manufacturing/bom/costOption.types'

/** API 路由前缀（对应 TaktBomCostOptionsController） */
const BOM_COST_OPTION_API_BASE = 'TaktBomCostOptions'

/**
 * 工厂选项 URL
 * @returns {string} 相对 API 路径
 */
export function getBomCostOptionPlantOptionsUrl(): string {
  return `${BOM_COST_OPTION_API_BASE}/plant-options`
}

/**
 * 物料类型去重选项 URL
 * @returns {string} 相对 API 路径
 */
export function getBomCostOptionMaterialTypeOptionsUrl(): string {
  return `${BOM_COST_OPTION_API_BASE}/material-type-options`
}

/**
 * 机种去重选项 URL
 * @returns {string} 相对 API 路径
 */
export function getBomCostOptionModelOptionsUrl(): string {
  return `${BOM_COST_OPTION_API_BASE}/model-options`
}

/**
 * 产品去重选项 URL
 * @returns {string} 相对 API 路径
 */
export function getBomCostOptionProductOptionsUrl(): string {
  return `${BOM_COST_OPTION_API_BASE}/product-options`
}

/**
 * 物料/组件去重选项 URL
 * @returns {string} 相对 API 路径
 */
export function getBomCostOptionMaterialOptionsUrl(): string {
  return `${BOM_COST_OPTION_API_BASE}/material-options`
}

/**
 * 拉取工厂选项（当前公司 RelatedPlant ∩ 头表未删除）
 * @returns {Promise<TaktSelectOption[]>} 工厂选项
 */
export function getBomCostOptionPlantOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${BOM_COST_OPTION_API_BASE}/plant-options`,
    method: 'get',
  })
}

/**
 * 拉取物料类型去重选项（须工厂 + 期间）
 * @param {BomCostOptionQuery} query 工厂 + 期间
 * @returns {Promise<TaktSelectOption[]>} 物料类型选项
 */
export function getBomCostOptionMaterialTypeOptions(
  query: BomCostOptionQuery,
): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${BOM_COST_OPTION_API_BASE}/material-type-options`,
    method: 'get',
    params: query,
  })
}

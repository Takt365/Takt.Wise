// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/materials
// 文件名称：materialMovingTrend.api.ts
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：物料移动价格推移转置分析 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { TaktSelectOption } from '@/types/common.types';
import type {
  MaterialMovingTrendQuery,
  MaterialMovingTrendResult,
} from '@/types/logistics/materials/materialMovingTrend.types';

/** API 路由前缀（对应 TaktMaterialMovingTrendsController） */
const MATERIAL_MOVING_TREND_API_BASE = 'TaktMaterialMovingTrends';

/**
 * 推移查询栏：本表工厂去重选项 URL（供 TaktSelect api-url）
 * @returns {string} 相对 API 路径
 */
export function getMaterialMovingTrendPlantOptionsUrl(): string {
  return `${MATERIAL_MOVING_TREND_API_BASE}/plant-options`;
}

/**
 * 拉取本表工厂去重选项（供默认选中当前公司 RelatedPlant 交集校验）
 * @returns {Promise<TaktSelectOption[]>} 工厂选项
 */
export function getMaterialMovingTrendPlantOptions(): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${MATERIAL_MOVING_TREND_API_BASE}/plant-options`,
    method: 'get',
  });
}

/**
 * 推移查询栏：评估类别去重选项 URL
 * @returns {string} 相对 API 路径
 */
export function getMaterialMovingTrendValuationOptionsUrl(): string {
  return `${MATERIAL_MOVING_TREND_API_BASE}/valuation-options`;
}

/**
 * 推移查询栏：物料去重选项 URL
 * @returns {string} 相对 API 路径
 */
export function getMaterialMovingTrendMaterialOptionsUrl(): string {
  return `${MATERIAL_MOVING_TREND_API_BASE}/material-options`;
}

/**
 * 物料移动价格推移分析
 * @param {MaterialMovingTrendQuery} queryDto 查询条件
 * @returns {Promise<MaterialMovingTrendResult>} 转置结果
 */
export function getMaterialMovingTrendAnalysis(
  queryDto: MaterialMovingTrendQuery
): Promise<MaterialMovingTrendResult> {
  return request<MaterialMovingTrendResult>({
    url: `${MATERIAL_MOVING_TREND_API_BASE}/trend-analysis`,
    method: 'get',
    params: queryDto,
    timeout: 120000,
  });
}

/**
 * 导出物料移动价格推移分析
 * @param {MaterialMovingTrendQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportMaterialMovingTrendAnalysis(
  query: MaterialMovingTrendQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MATERIAL_MOVING_TREND_API_BASE}/trend-analysis/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName,
    },
    responseType: 'blob',
    returnBinaryMeta: true,
    timeout: 300000,
  });
}

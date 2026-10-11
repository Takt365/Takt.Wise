// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/output
// 文件名称：productionMonthly.api.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：月生产推移转置分析 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  ProductionMonthlyTrendQuery,
  ProductionMonthlyTrendResult,
} from '@/types/logistics/manufacturing/output/productionMonthlyTrend.types';

/** API 路由前缀（对应 TaktProductionMonthlyTrendsController） */
const PRODUCTION_MONTHLY_TREND_API_BASE = 'TaktProductionMonthlyTrends';

/**
 * 推移查询栏：本表工厂去重选项 URL（供 TaktSelect api-url）
 * @returns {string} 相对 API 路径
 */
export function getProductionMonthlyTrendPlantOptionsUrl(): string {
  return `${PRODUCTION_MONTHLY_TREND_API_BASE}/plant-options`;
}

/**
 * 推移查询栏：产出类别去重选项 URL
 * @returns {string} 相对 API 路径
 */
export function getProductionMonthlyTrendOutputCategoryOptionsUrl(): string {
  return `${PRODUCTION_MONTHLY_TREND_API_BASE}/output-category-options`;
}

/**
 * 推移查询栏：机种去重选项 URL
 * @returns {string} 相对 API 路径
 */
export function getProductionMonthlyTrendModelOptionsUrl(): string {
  return `${PRODUCTION_MONTHLY_TREND_API_BASE}/model-options`;
}

/**
 * 月生产推移分析
 * @param {ProductionMonthlyTrendQuery} queryDto 查询条件
 * @returns {Promise<ProductionMonthlyTrendResult>} 转置结果
 */
export function getProductionMonthlyTrendAnalysis(
  queryDto: ProductionMonthlyTrendQuery
): Promise<ProductionMonthlyTrendResult> {
  return request<ProductionMonthlyTrendResult>({
    url: `${PRODUCTION_MONTHLY_TREND_API_BASE}/monthly-trend-analysis`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 导出月生产推移分析
 * @param {ProductionMonthlyTrendQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportProductionMonthlyTrendAnalysis(
  query: ProductionMonthlyTrendQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PRODUCTION_MONTHLY_TREND_API_BASE}/monthly-trend-analysis/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName,
    },
    responseType: 'blob',
    returnBinaryMeta: true,
  });
}

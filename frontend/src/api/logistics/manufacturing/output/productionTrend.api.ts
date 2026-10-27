// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/output
// 文件名称：productionTrend.api.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：生产推移 API（组立 / PCBA 按机种）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  ProductionMonthlyTrendQuery,
  ProductionMonthlyTrendResult,
} from '@/types/logistics/manufacturing/output/productionMonthlyTrend.types';

/** API 路由前缀（对应 TaktProductionTrendsController） */
const PRODUCTION_TREND_API_BASE = 'TaktProductionTrends';

/**
 * 当前页签工厂选项 URL
 * @returns {string} 相对 API 路径
 */
export function getProductionTrendPlantOptionsUrl(): string {
  return `${PRODUCTION_TREND_API_BASE}/plant-options`;
}

/**
 * 当前页签机种选项 URL
 * @returns {string} 相对 API 路径
 */
export function getProductionTrendModelOptionsUrl(): string {
  return `${PRODUCTION_TREND_API_BASE}/model-options`;
}

/**
 * 生产推移分析
 * @param {ProductionMonthlyTrendQuery} queryDto 查询条件
 * @returns {Promise<ProductionMonthlyTrendResult>} 转置结果
 */
export function getProductionTrendAnalysis(
  queryDto: ProductionMonthlyTrendQuery
): Promise<ProductionMonthlyTrendResult> {
  return request<ProductionMonthlyTrendResult>({
    url: `${PRODUCTION_TREND_API_BASE}/analysis`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 导出生产推移
 * @param {ProductionMonthlyTrendQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportProductionTrendAnalysis(
  query: ProductionMonthlyTrendQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PRODUCTION_TREND_API_BASE}/analysis/export`,
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

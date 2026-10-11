// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/sales
// 文件名称：salesActualTrend.api.ts
// 创建时间：2026-09-16
// 创建人：Takt365(Cursor AI)
// 功能描述：销售实绩推移转置分析 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  PurchasePriceTrendQuery,
  PurchasePriceTrendResult,
} from '@/types/logistics/procurement/purchasePriceTrend.types';

/** API 路由前缀（对应 TaktSalesActualTrendsController） */
const SALES_ACTUAL_TREND_API_BASE = 'TaktSalesActualTrends';

/**
 * 推移查询栏：本表工厂去重选项 URL（供 TaktSelect api-url）
 * @returns {string} 相对 API 路径
 */
export function getSalesActualTrendPlantOptionsUrl(): string {
  return `${SALES_ACTUAL_TREND_API_BASE}/plant-options`;
}

/**
 * 推移查询栏：客户去重选项 URL
 * @returns {string} 相对 API 路径
 */
export function getSalesActualTrendCustomerOptionsUrl(): string {
  return `${SALES_ACTUAL_TREND_API_BASE}/customer-options`;
}

/**
 * 推移查询栏：物料去重选项 URL
 * @returns {string} 相对 API 路径
 */
export function getSalesActualTrendMaterialOptionsUrl(): string {
  return `${SALES_ACTUAL_TREND_API_BASE}/material-options`;
}

/**
 * 销售实绩推移转置分析
 * @param {PurchasePriceTrendQuery} queryDto 查询条件
 * @returns {Promise<PurchasePriceTrendResult>} 转置结果
 */
export function getSalesActualTrendAnalysis(
  queryDto: PurchasePriceTrendQuery
): Promise<PurchasePriceTrendResult> {
  return request<PurchasePriceTrendResult>({
    url: `${SALES_ACTUAL_TREND_API_BASE}/trend-analysis`,
    method: 'get',
    params: queryDto,
    timeout: 120000,
  });
}

/**
 * 导出销售实绩推移转置分析
 * @param {PurchasePriceTrendQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportSalesActualTrendAnalysis(
  query: PurchasePriceTrendQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_ACTUAL_TREND_API_BASE}/trend-analysis/export`,
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

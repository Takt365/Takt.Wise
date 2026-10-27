// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：ecMonthlyTrend.api.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：设变推移转置分析 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  EcImplementationMonthlyTrendQuery,
  EcImplementationMonthlyTrendResult,
  EcMonthlyTrendQuery,
  EcMonthlyTrendResult,
} from '@/types/logistics/manufacturing/engineering-change/ecMonthlyTrend.types';

/** API 路由前缀（对应 TaktEcMonthlyTrendsController） */
const EC_MONTHLY_TREND_API_BASE = 'TaktEcMonthlyTrends';

/**
 * 推移查询栏：工厂去重选项 URL（供 TaktSelect api-url）
 * @returns {string} 相对 API 路径
 */
export function getEcMonthlyTrendPlantOptionsUrl(): string {
  return `${EC_MONTHLY_TREND_API_BASE}/plant-options`;
}

/**
 * 推移查询栏：部门去重选项 URL
 * @returns {string} 相对 API 路径
 */
export function getEcMonthlyTrendDeptOptionsUrl(): string {
  return `${EC_MONTHLY_TREND_API_BASE}/dept-options`;
}

/**
 * 推移查询栏：设变单号去重选项 URL
 * @returns {string} 相对 API 路径
 */
export function getEcMonthlyTrendEcCodeOptionsUrl(): string {
  return `${EC_MONTHLY_TREND_API_BASE}/ec-code-options`;
}

/**
 * 设变推移分析
 * @param {EcMonthlyTrendQuery} queryDto 查询条件
 * @returns {Promise<EcMonthlyTrendResult>} 转置结果
 */
export function getEcMonthlyTrendAnalysis(
  queryDto: EcMonthlyTrendQuery
): Promise<EcMonthlyTrendResult> {
  return request<EcMonthlyTrendResult>({
    url: `${EC_MONTHLY_TREND_API_BASE}/monthly-trend-analysis`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 导出设变推移分析
 * @param {EcMonthlyTrendQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportEcMonthlyTrendAnalysis(
  query: EcMonthlyTrendQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EC_MONTHLY_TREND_API_BASE}/monthly-trend-analysis/export`,
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

/**
 * 月实施推移分析
 * @param {EcImplementationMonthlyTrendQuery} queryDto 查询条件
 * @returns {Promise<EcImplementationMonthlyTrendResult>} 转置结果
 */
export function getEcImplementationMonthlyTrendAnalysis(
  queryDto: EcImplementationMonthlyTrendQuery
): Promise<EcImplementationMonthlyTrendResult> {
  return request<EcImplementationMonthlyTrendResult>({
    url: `${EC_MONTHLY_TREND_API_BASE}/implementation-monthly-trend-analysis`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 导出月实施推移分析
 * @param {EcImplementationMonthlyTrendQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportEcImplementationMonthlyTrendAnalysis(
  query: EcImplementationMonthlyTrendQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EC_MONTHLY_TREND_API_BASE}/implementation-monthly-trend-analysis/export`,
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

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/defect
// 文件名称：defectMonthly.api.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：月生产不良推移转置分析 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  DefectMonthlyTrendQuery,
  DefectMonthlyTrendResult,
} from '@/types/logistics/manufacturing/defect/defectMonthlyTrend.types';

/** API 路由前缀（对应 TaktDefectMonthlyTrendsController） */
const DEFECT_MONTHLY_TREND_API_BASE = 'TaktDefectMonthlyTrends';

/**
 * 推移查询栏：工厂去重选项 URL（供 TaktSelect api-url）
 * @returns {string} 相对 API 路径
 */
export function getDefectMonthlyTrendPlantOptionsUrl(): string {
  return `${DEFECT_MONTHLY_TREND_API_BASE}/plant-options`;
}

/**
 * 推移查询栏：不良类别去重选项 URL
 * @returns {string} 相对 API 路径
 */
export function getDefectMonthlyTrendDefectCategoryOptionsUrl(): string {
  return `${DEFECT_MONTHLY_TREND_API_BASE}/defect-category-options`;
}

/**
 * 推移查询栏：机种去重选项 URL
 * @returns {string} 相对 API 路径
 */
export function getDefectMonthlyTrendModelOptionsUrl(): string {
  return `${DEFECT_MONTHLY_TREND_API_BASE}/model-options`;
}

/**
 * 月生产不良推移分析
 * @param {DefectMonthlyTrendQuery} queryDto 查询条件
 * @returns {Promise<DefectMonthlyTrendResult>} 转置结果
 */
export function getDefectMonthlyTrendAnalysis(
  queryDto: DefectMonthlyTrendQuery
): Promise<DefectMonthlyTrendResult> {
  return request<DefectMonthlyTrendResult>({
    url: `${DEFECT_MONTHLY_TREND_API_BASE}/monthly-trend-analysis`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 导出月生产不良推移分析
 * @param {DefectMonthlyTrendQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportDefectMonthlyTrendAnalysis(
  query: DefectMonthlyTrendQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${DEFECT_MONTHLY_TREND_API_BASE}/monthly-trend-analysis/export`,
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

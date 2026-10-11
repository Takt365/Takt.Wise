// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/quality/cost
// 文件名称：costTrend.api.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：质量成本月推移转置分析 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  QualityCostTrendQuery,
  QualityCostTrendResult,
} from '@/types/logistics/quality/cost/costTrend.types';

/** API 路由前缀 */
const QUALITY_COST_TREND_API_BASE = 'TaktQualityCostTrends';

/**
 * 质量成本月推移分析
 * @param {QualityCostTrendQuery} queryDto 查询条件
 * @returns {Promise<QualityCostTrendResult>} 转置结果
 */
export function getQualityCostMonthlyTrendAnalysis(
  queryDto: QualityCostTrendQuery
): Promise<QualityCostTrendResult> {
  return request<QualityCostTrendResult>({
    url: `${QUALITY_COST_TREND_API_BASE}/monthly-trend-analysis`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 导出质量成本月推移分析
 * @param {QualityCostTrendQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportQualityCostMonthlyTrendAnalysis(
  query: QualityCostTrendQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${QUALITY_COST_TREND_API_BASE}/monthly-trend-analysis/export`,
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

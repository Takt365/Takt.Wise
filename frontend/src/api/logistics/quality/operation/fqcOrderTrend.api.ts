// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/quality/operation
// 文件名称：fqcOrderTrend.api.ts
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：FQC 成品检验月推移转置分析 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  FqcOrderMonthlyTrend,
  FqcOrderMonthlyTrendQuery,
  QualityInspectionMonthlyTrendResult,
} from '@/types/logistics/quality/operation/inspectionTrend.types';

/** API 路由前缀（对应 TaktFqcOrderTrendsController） */
const FQC_ORDER_TREND_API_BASE = 'TaktFqcOrderTrends';

/**
 * FQC 成品检验月推移转置分析
 * @param {FqcOrderMonthlyTrendQuery} queryDto 查询条件
 * @returns {Promise<QualityInspectionMonthlyTrendResult<FqcOrderMonthlyTrend>>} 分析结果
 */
export function getFqcOrderMonthlyTrendAnalysis(
  queryDto: FqcOrderMonthlyTrendQuery
): Promise<QualityInspectionMonthlyTrendResult<FqcOrderMonthlyTrend>> {
  return request<QualityInspectionMonthlyTrendResult<FqcOrderMonthlyTrend>>({
    url: `${FQC_ORDER_TREND_API_BASE}/monthly-trend-analysis`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 导出 FQC 成品检验月推移
 * @param {FqcOrderMonthlyTrendQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportFqcOrderMonthlyTrendAnalysis(
  query: FqcOrderMonthlyTrendQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${FQC_ORDER_TREND_API_BASE}/monthly-trend-analysis/export`,
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

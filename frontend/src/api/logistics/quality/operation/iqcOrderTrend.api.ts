// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/quality/operation
// 文件名称：iqcOrderTrend.api.ts
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：IQC 进货检验月推移转置分析 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  IqcOrderMonthlyTrend,
  IqcOrderMonthlyTrendQuery,
  QualityInspectionMonthlyTrendResult,
} from '@/types/logistics/quality/operation/inspectionTrend.types';

/** API 路由前缀（对应 TaktIqcOrderTrendsController） */
const IQC_ORDER_TREND_API_BASE = 'TaktIqcOrderTrends';

/**
 * IQC 进货检验月推移转置分析
 * @param {IqcOrderMonthlyTrendQuery} queryDto 查询条件
 * @returns {Promise<QualityInspectionMonthlyTrendResult<IqcOrderMonthlyTrend>>} 分析结果
 */
export function getIqcOrderMonthlyTrendAnalysis(
  queryDto: IqcOrderMonthlyTrendQuery
): Promise<QualityInspectionMonthlyTrendResult<IqcOrderMonthlyTrend>> {
  return request<QualityInspectionMonthlyTrendResult<IqcOrderMonthlyTrend>>({
    url: `${IQC_ORDER_TREND_API_BASE}/monthly-trend-analysis`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 导出 IQC 进货检验月推移
 * @param {IqcOrderMonthlyTrendQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportIqcOrderMonthlyTrendAnalysis(
  query: IqcOrderMonthlyTrendQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${IQC_ORDER_TREND_API_BASE}/monthly-trend-analysis/export`,
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

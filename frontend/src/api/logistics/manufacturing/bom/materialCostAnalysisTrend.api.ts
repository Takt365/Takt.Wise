// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/bom
// 文件名称：materialCostAnalysisTrend.api.ts
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 成本分析月度涨跌 API（与转置/差异分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  BomMaterialCostItemMonthlyTrendQuery,
  BomMaterialCostItemMonthlyTrendResult,
} from '@/types/logistics/manufacturing/bom/materialCostAnalysis.types';

/** API 路由前缀（对应 TaktBomMaterialCostAnalysisTrendsController） */
const BOM_MATERIAL_COST_ANALYSIS_TREND_API_BASE = 'TaktBomMaterialCostAnalysisTrends';

/**
 * 获取 BOM 成本分析月度涨跌
 * @param {BomMaterialCostItemMonthlyTrendQuery} queryDto 查询条件
 * @returns {Promise<BomMaterialCostItemMonthlyTrendResult>} 月度涨跌结果
 */
export function getBomMaterialCostItemMonthlyTrendAnalysis(
  queryDto: BomMaterialCostItemMonthlyTrendQuery
): Promise<BomMaterialCostItemMonthlyTrendResult> {
  return request<BomMaterialCostItemMonthlyTrendResult>({
    url: `${BOM_MATERIAL_COST_ANALYSIS_TREND_API_BASE}/monthly-trend-analysis`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 导出 BOM 成本分析月度涨跌
 * @param {BomMaterialCostItemMonthlyTrendQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportBomMaterialCostItemMonthlyTrendAnalysis(
  query: BomMaterialCostItemMonthlyTrendQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${BOM_MATERIAL_COST_ANALYSIS_TREND_API_BASE}/monthly-trend-analysis/export`,
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

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/bom
// 文件名称：materialCostAnalysis.api.ts
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 成本分析 API（转置 / 差异 / 月度涨跌）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  BomMaterialCostItemTransposedQuery,
  BomMaterialCostItemTransposedResult,
  BomMaterialCostItemVarianceQuery,
  BomMaterialCostItemVarianceResult,
} from '@/types/logistics/manufacturing/bom/materialCostAnalysis.types';

/** API 路由前缀（对应 TaktBomMaterialCostAnalysesController） */
const BOM_MATERIAL_COST_ANALYSIS_API_BASE = 'TaktBomMaterialCostAnalyses';

/**
 * 获取 BOM 成本分析转置列表
 * @param {BomMaterialCostItemTransposedQuery} queryDto 查询条件
 * @returns {Promise<BomMaterialCostItemTransposedResult>} 转置分页结果
 */
export function getBomMaterialCostItemTransposedList(
  queryDto: BomMaterialCostItemTransposedQuery
): Promise<BomMaterialCostItemTransposedResult> {
  return request<BomMaterialCostItemTransposedResult>({
    url: `${BOM_MATERIAL_COST_ANALYSIS_API_BASE}/transposed`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 导出 BOM 成本分析转置报表
 * @param {Partial<BomMaterialCostItemTransposedQuery>} [query] 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportBomMaterialCostItemTransposed(
  query?: Partial<BomMaterialCostItemTransposedQuery>,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${BOM_MATERIAL_COST_ANALYSIS_API_BASE}/transposed/export`,
    method: 'get',
    params: { ...query, sheetName, exportName },
    responseType: 'blob',
    returnBinaryMeta: true,
  });
}

/**
 * 获取 BOM 成本分析差异
 * @param {BomMaterialCostItemVarianceQuery} queryDto 查询条件
 * @returns {Promise<BomMaterialCostItemVarianceResult>} 差异结果
 */
export function getBomMaterialCostItemVarianceAnalysis(
  queryDto: BomMaterialCostItemVarianceQuery
): Promise<BomMaterialCostItemVarianceResult> {
  return request<BomMaterialCostItemVarianceResult>({
    url: `${BOM_MATERIAL_COST_ANALYSIS_API_BASE}/variance-analysis`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 导出 BOM 成本分析差异报表
 * @param {BomMaterialCostItemVarianceQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportBomMaterialCostItemVarianceAnalysis(
  query: BomMaterialCostItemVarianceQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${BOM_MATERIAL_COST_ANALYSIS_API_BASE}/variance-analysis/export`,
    method: 'get',
    params: { ...query, sheetName, exportName },
    responseType: 'blob',
  });
}

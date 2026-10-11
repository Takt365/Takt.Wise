// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/bom
// 文件名称：materialCostTrend.api.ts
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 产品成本推移分析 API（组件移动价）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  BomMaterialCostItemComponentMovingPriceQuery,
  BomMaterialCostItemComponentMovingPriceResult,
} from '@/types/logistics/manufacturing/bom/materialCostTrend.types';

/** API 路由前缀（对应 TaktBomMaterialCostTrendsController） */
const BOM_MATERIAL_COST_TREND_API_BASE = 'TaktBomMaterialCostTrends';

/**
 * 产品成本推移：单个产品下明细组件 × 月材料成本
 * @param {BomMaterialCostItemComponentMovingPriceQuery} queryDto 查询条件
 * @returns {Promise<BomMaterialCostItemComponentMovingPriceResult>} 分析结果
 */
export function getBomMaterialCostItemComponentMovingPriceAnalysis(
  queryDto: BomMaterialCostItemComponentMovingPriceQuery
): Promise<BomMaterialCostItemComponentMovingPriceResult> {
  return request<BomMaterialCostItemComponentMovingPriceResult>({
    url: `${BOM_MATERIAL_COST_TREND_API_BASE}/component-moving-price-analysis`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 导出产品成本推移
 * @param {BomMaterialCostItemComponentMovingPriceQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportBomMaterialCostItemComponentMovingPriceAnalysis(
  query: BomMaterialCostItemComponentMovingPriceQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${BOM_MATERIAL_COST_TREND_API_BASE}/component-moving-price-analysis/export`,
    method: 'get',
    params: { ...query, sheetName, exportName },
    responseType: 'blob',
    returnBinaryMeta: true,
  });
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/sales
// 文件名称：priceTrend.api.ts
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：销售价格推移转置分析 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  SalesPriceTrendQuery,
  SalesPriceTrendResult,
} from '@/types/logistics/sales/priceTrend.types';

/** API 路由前缀（对应 TaktSalesPriceTrendsController） */
const SALES_PRICE_TREND_API_BASE = 'TaktSalesPriceTrends';

/**
 * 推移查询栏：本表工厂去重选项 URL（供 TaktSelect api-url）
 * @returns {string} 相对 API 路径
 */
export function getSalesPriceTrendPlantOptionsUrl(): string {
  return `${SALES_PRICE_TREND_API_BASE}/plant-options`;
}

/**
 * 推移查询栏：条件类型去重选项 URL
 * @returns {string} 相对 API 路径
 */
export function getSalesPriceTrendPriceTypeOptionsUrl(): string {
  return `${SALES_PRICE_TREND_API_BASE}/price-type-options`;
}

/**
 * 推移查询栏：客户去重选项 URL
 * @returns {string} 相对 API 路径
 */
export function getSalesPriceTrendCustomerOptionsUrl(): string {
  return `${SALES_PRICE_TREND_API_BASE}/customer-options`;
}

/**
 * 推移查询栏：物料去重选项 URL
 * @returns {string} 相对 API 路径
 */
export function getSalesPriceTrendMaterialOptionsUrl(): string {
  return `${SALES_PRICE_TREND_API_BASE}/material-options`;
}

/**
 * 销售价格推移转置分析
 * @param {SalesPriceTrendQuery} queryDto 查询条件
 * @returns {Promise<SalesPriceTrendResult>} 转置结果
 */
export function getSalesPriceTrendAnalysis(
  queryDto: SalesPriceTrendQuery
): Promise<SalesPriceTrendResult> {
  return request<SalesPriceTrendResult>({
    url: `${SALES_PRICE_TREND_API_BASE}/trend-analysis`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 导出销售价格推移转置分析
 * @param {SalesPriceTrendQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportSalesPriceTrendAnalysis(
  query: SalesPriceTrendQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_PRICE_TREND_API_BASE}/trend-analysis/export`,
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


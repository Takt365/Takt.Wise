// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/sales
// 文件名称：soPriceScrDeviation.api.ts
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：SO 价偏离 SCR 分析 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  SoPriceScrDeviationQuery,
  SoPriceScrDeviationResult,
} from '@/types/logistics/sales/soPriceScrDeviation.types';

/** API 路由前缀（对应 TaktSoPriceScrDeviationsController） */
const SO_PRICE_SCR_DEVIATION_API_BASE = 'TaktSoPriceScrDeviations';

/**
 * 查询栏工厂选项 URL
 * @returns {string} 相对 API 路径
 */
export function getSoPriceScrDeviationPlantOptionsUrl(): string {
  return `${SO_PRICE_SCR_DEVIATION_API_BASE}/plant-options`;
}

/**
 * 查询栏客户选项 URL
 * @returns {string} 相对 API 路径
 */
export function getSoPriceScrDeviationCustomerOptionsUrl(): string {
  return `${SO_PRICE_SCR_DEVIATION_API_BASE}/customer-options`;
}

/**
 * 查询栏物料选项 URL
 * @returns {string} 相对 API 路径
 */
export function getSoPriceScrDeviationMaterialOptionsUrl(): string {
  return `${SO_PRICE_SCR_DEVIATION_API_BASE}/material-options`;
}

/**
 * SO 价偏离 SCR 分析
 * @param {SoPriceScrDeviationQuery} queryDto 查询条件
 * @returns {Promise<SoPriceScrDeviationResult>} 分析结果
 */
export function getSoPriceScrDeviationAnalysis(
  queryDto: SoPriceScrDeviationQuery
): Promise<SoPriceScrDeviationResult> {
  return request<SoPriceScrDeviationResult>({
    url: `${SO_PRICE_SCR_DEVIATION_API_BASE}/deviation-analysis`,
    method: 'get',
    params: queryDto,
    timeout: 120000,
  });
}

/**
 * 导出 SO 价偏离 SCR 清单
 * @param {SoPriceScrDeviationQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportSoPriceScrDeviationAnalysis(
  query: SoPriceScrDeviationQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SO_PRICE_SCR_DEVIATION_API_BASE}/export`,
    method: 'get',
    params: { ...query, sheetName, exportName },
    responseType: 'blob',
    timeout: 180000,
  });
}

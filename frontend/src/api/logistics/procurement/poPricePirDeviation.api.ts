// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/procurement
// 文件名称：poPricePirDeviation.api.ts
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：PO 价偏离 PIR 分析 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  PoPricePirDeviationQuery,
  PoPricePirDeviationResult,
} from '@/types/logistics/procurement/poPricePirDeviation.types';

/** API 路由前缀（对应 TaktPoPricePirDeviationsController） */
const PO_PRICE_PIR_DEVIATION_API_BASE = 'TaktPoPricePirDeviations';

/**
 * 查询栏工厂选项 URL
 * @returns {string} 相对 API 路径
 */
export function getPoPricePirDeviationPlantOptionsUrl(): string {
  return `${PO_PRICE_PIR_DEVIATION_API_BASE}/plant-options`;
}

/**
 * 查询栏供应商选项 URL
 * @returns {string} 相对 API 路径
 */
export function getPoPricePirDeviationSupplierOptionsUrl(): string {
  return `${PO_PRICE_PIR_DEVIATION_API_BASE}/supplier-options`;
}

/**
 * 查询栏物料选项 URL
 * @returns {string} 相对 API 路径
 */
export function getPoPricePirDeviationMaterialOptionsUrl(): string {
  return `${PO_PRICE_PIR_DEVIATION_API_BASE}/material-options`;
}

/**
 * PO 价偏离 PIR 分析
 * @param {PoPricePirDeviationQuery} queryDto 查询条件
 * @returns {Promise<PoPricePirDeviationResult>} 分析结果
 */
export function getPoPricePirDeviationAnalysis(
  queryDto: PoPricePirDeviationQuery
): Promise<PoPricePirDeviationResult> {
  return request<PoPricePirDeviationResult>({
    url: `${PO_PRICE_PIR_DEVIATION_API_BASE}/deviation-analysis`,
    method: 'get',
    params: queryDto,
    timeout: 120000,
  });
}

/**
 * 导出 PO 价偏离 PIR 清单
 * @param {PoPricePirDeviationQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportPoPricePirDeviationAnalysis(
  query: PoPricePirDeviationQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PO_PRICE_PIR_DEVIATION_API_BASE}/export`,
    method: 'get',
    params: { ...query, sheetName, exportName },
    responseType: 'blob',
    timeout: 180000,
  });
}

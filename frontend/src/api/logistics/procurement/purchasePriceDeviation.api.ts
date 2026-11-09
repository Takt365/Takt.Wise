// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/procurement
// 文件名称：purchasePriceDeviation.api.ts
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：PO/MIRO 价偏离 PCR 分析 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  MiroPricePcrDeviationQuery,
  MiroPricePcrDeviationResult,
  PurchasePriceDeviationQuery,
  PurchasePriceDeviationResult,
} from '@/types/logistics/procurement/purchasePriceDeviation.types';

/** API 路由前缀（对应 TaktPurchasePriceDeviationsController） */
const PURCHASE_PRICE_DEVIATION_API_BASE = 'TaktPurchasePriceDeviations';

/**
 * 查询栏工厂选项 URL
 * @returns {string} 相对 API 路径
 */
export function getPurchasePriceDeviationPlantOptionsUrl(): string {
  return `${PURCHASE_PRICE_DEVIATION_API_BASE}/plant-options`;
}

/**
 * 查询栏供应商选项 URL
 * @returns {string} 相对 API 路径
 */
export function getPurchasePriceDeviationSupplierOptionsUrl(): string {
  return `${PURCHASE_PRICE_DEVIATION_API_BASE}/supplier-options`;
}

/**
 * 查询栏物料选项 URL
 * @returns {string} 相对 API 路径
 */
export function getPurchasePriceDeviationMaterialOptionsUrl(): string {
  return `${PURCHASE_PRICE_DEVIATION_API_BASE}/material-options`;
}

/**
 * PO 价偏离 PCR 分析
 * @param {PurchasePriceDeviationQuery} queryDto 查询条件
 * @returns {Promise<PurchasePriceDeviationResult>} 分析结果
 */
export function getPurchasePriceDeviationAnalysis(
  queryDto: PurchasePriceDeviationQuery
): Promise<PurchasePriceDeviationResult> {
  return request<PurchasePriceDeviationResult>({
    url: `${PURCHASE_PRICE_DEVIATION_API_BASE}/deviation-analysis`,
    method: 'get',
    params: queryDto,
    timeout: 120000,
  });
}

/**
 * 导出 PO 价偏离 PCR 清单
 * @param {PurchasePriceDeviationQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportPurchasePriceDeviationAnalysis(
  query: PurchasePriceDeviationQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_PRICE_DEVIATION_API_BASE}/export`,
    method: 'get',
    params: { ...query, sheetName, exportName },
    responseType: 'blob',
    timeout: 180000,
  });
}

/**
 * MIRO 查询栏工厂选项 URL
 * @returns {string} 相对 API 路径
 */
export function getMiroPricePcrDeviationPlantOptionsUrl(): string {
  return `${PURCHASE_PRICE_DEVIATION_API_BASE}/miro/plant-options`;
}

/**
 * MIRO 查询栏供应商选项 URL
 * @returns {string} 相对 API 路径
 */
export function getMiroPricePcrDeviationSupplierOptionsUrl(): string {
  return `${PURCHASE_PRICE_DEVIATION_API_BASE}/miro/supplier-options`;
}

/**
 * MIRO 查询栏物料选项 URL
 * @returns {string} 相对 API 路径
 */
export function getMiroPricePcrDeviationMaterialOptionsUrl(): string {
  return `${PURCHASE_PRICE_DEVIATION_API_BASE}/miro/material-options`;
}

/**
 * MIRO 价偏离 PCR 分析
 * @param {MiroPricePcrDeviationQuery} queryDto 查询条件
 * @returns {Promise<MiroPricePcrDeviationResult>} 分析结果
 */
export function getMiroPricePcrDeviationAnalysis(
  queryDto: MiroPricePcrDeviationQuery
): Promise<MiroPricePcrDeviationResult> {
  return request<MiroPricePcrDeviationResult>({
    url: `${PURCHASE_PRICE_DEVIATION_API_BASE}/miro/deviation-analysis`,
    method: 'get',
    params: queryDto,
    timeout: 120000,
  });
}

/**
 * 导出 MIRO 价偏离 PCR 清单
 * @param {MiroPricePcrDeviationQuery} query 查询条件
 * @param {string} [sheetName] 工作表名
 * @param {string} [exportName] 导出文件名
 * @returns {Promise<Blob>} Excel 文件
 */
export function exportMiroPricePcrDeviationAnalysis(
  query: MiroPricePcrDeviationQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_PRICE_DEVIATION_API_BASE}/miro/export`,
    method: 'get',
    params: { ...query, sheetName, exportName },
    responseType: 'blob',
    timeout: 180000,
  });
}

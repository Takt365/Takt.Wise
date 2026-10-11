// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/procurement
// 文件名称：purchaseForecastItem.api.ts
// 创建时间：2026-08-06
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/procurement 模块 API（自动生成，请勿手改路由常量）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  TaktPagedResult,
  TaktSelectOption
} from '@/types/common.types';
import type {
  PurchaseForecastItem,
  PurchaseForecastItemCreate,
  PurchaseForecastItemObsolete,
  PurchaseForecastItemUpdate
} from '@/types/logistics/procurement/purchaseForecastItem.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPurchaseForecastItems
 */
const PURCHASE_FORECAST_ITEM_API_BASE = 'TaktPurchaseForecastItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取采购预测明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PurchaseForecastItem>>} 分页结果
 */
export function getPurchaseForecastItemList(queryDto: any): Promise<TaktPagedResult<PurchaseForecastItem>> {
  return request<TaktPagedResult<PurchaseForecastItem>>({
    url: `${PURCHASE_FORECAST_ITEM_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取采购预测明细
 * @param {string} id 采购预测明细ID
 * @returns {Promise<PurchaseForecastItem>} 采购预测明细DTO
 */
export function getPurchaseForecastItemById(id: string): Promise<PurchaseForecastItem> {
  return request<PurchaseForecastItem>({
    url: `${PURCHASE_FORECAST_ITEM_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建采购预测明细
 * @param {PurchaseForecastItemCreate} dto 创建DTO
 * @returns {Promise<PurchaseForecastItem>} 采购预测明细DTO
 */
export function createPurchaseForecastItem(dto: PurchaseForecastItemCreate): Promise<PurchaseForecastItem> {
  return request<PurchaseForecastItem>({
    url: `${PURCHASE_FORECAST_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新采购预测明细
 * @param {string} id 采购预测明细ID
 * @param {PurchaseForecastItemUpdate} dto 更新DTO
 * @returns {Promise<PurchaseForecastItem>} 采购预测明细DTO
 */
export function updatePurchaseForecastItem(id: string, dto: PurchaseForecastItemUpdate): Promise<PurchaseForecastItem> {
  return request<PurchaseForecastItem>({
    url: `${PURCHASE_FORECAST_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除采购预测明细
 * @param {string} id 采购预测明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchaseForecastItemById(id: string): Promise<void> {
  return request({
    url: `${PURCHASE_FORECAST_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除采购预测明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePurchaseForecastItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PURCHASE_FORECAST_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新采购预测明细作废状态
 * @param {PurchaseForecastItemObsolete} dto 作废 DTO
 * @returns {Promise<PurchaseForecastItem>} 采购预测明细DTO
 */
export function updatePurchaseForecastItemObsolete(dto: PurchaseForecastItemObsolete): Promise<PurchaseForecastItem> {
  return request<PurchaseForecastItem>({
    url: `${PURCHASE_FORECAST_ITEM_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取采购预测明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPurchaseForecastItemOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${PURCHASE_FORECAST_ITEM_API_BASE}/options`,
    method: 'get',
    params: {
      
      ...(plant ? { plantCode: plant } : {}),
      ...(kw ? { keyword: kw } : {}),
    },
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 获取导入模板
 * @param {string} sheetName sheetName
 * @param {string} templateName templateName
 * @returns {Promise<Blob>} Excel文件
 */
export function getPurchaseForecastItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_FORECAST_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入采购预测明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPurchaseForecastItem(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PURCHASE_FORECAST_ITEM_API_BASE}/import`,
    method: 'post',
    data: formData,
    headers: {
      'Content-Type': 'multipart/form-data',
    },
    params: {
      sheetName
    },
  });
}

/**
 * 导出采购预测明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPurchaseForecastItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PURCHASE_FORECAST_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

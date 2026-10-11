// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/mds
// 文件名称：salesForecastItem.api.ts
// 创建时间：2026-07-29
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/mds 模块 API（自动生成，请勿手改路由常量）
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
  SalesForecastItem,
  SalesForecastItemCreate,
  SalesForecastItemObsolete,
  SalesForecastItemUpdate
} from '@/types/logistics/manufacturing/mds/salesForecastItem.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSalesForecastItems
 */
const SALES_FORECAST_ITEM_API_BASE = 'TaktSalesForecastItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取销售预测明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SalesForecastItem>>} 分页结果
 */
export function getSalesForecastItemList(queryDto: any): Promise<TaktPagedResult<SalesForecastItem>> {
  return request<TaktPagedResult<SalesForecastItem>>({
    url: `${SALES_FORECAST_ITEM_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取销售预测明细
 * @param {string} id 销售预测明细ID
 * @returns {Promise<SalesForecastItem>} 销售预测明细DTO
 */
export function getSalesForecastItemById(id: string): Promise<SalesForecastItem> {
  return request<SalesForecastItem>({
    url: `${SALES_FORECAST_ITEM_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建销售预测明细
 * @param {SalesForecastItemCreate} dto 创建DTO
 * @returns {Promise<SalesForecastItem>} 销售预测明细DTO
 */
export function createSalesForecastItem(dto: SalesForecastItemCreate): Promise<SalesForecastItem> {
  return request<SalesForecastItem>({
    url: `${SALES_FORECAST_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新销售预测明细
 * @param {string} id 销售预测明细ID
 * @param {SalesForecastItemUpdate} dto 更新DTO
 * @returns {Promise<SalesForecastItem>} 销售预测明细DTO
 */
export function updateSalesForecastItem(id: string, dto: SalesForecastItemUpdate): Promise<SalesForecastItem> {
  return request<SalesForecastItem>({
    url: `${SALES_FORECAST_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除销售预测明细
 * @param {string} id 销售预测明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesForecastItemById(id: string): Promise<void> {
  return request({
    url: `${SALES_FORECAST_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除销售预测明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesForecastItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SALES_FORECAST_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新销售预测明细作废状态
 * @param {SalesForecastItemObsolete} dto 作废 DTO
 * @returns {Promise<SalesForecastItem>} 销售预测明细DTO
 */
export function updateSalesForecastItemObsolete(dto: SalesForecastItemObsolete): Promise<SalesForecastItem> {
  return request<SalesForecastItem>({
    url: `${SALES_FORECAST_ITEM_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取销售计划明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSalesForecastItemOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${SALES_FORECAST_ITEM_API_BASE}/options`,
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
export function getSalesForecastItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_FORECAST_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入销售预测明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSalesForecastItem(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SALES_FORECAST_ITEM_API_BASE}/import`,
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
 * 导出销售预测明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSalesForecastItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_FORECAST_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

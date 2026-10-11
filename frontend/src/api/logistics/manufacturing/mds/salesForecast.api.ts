// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/mds
// 文件名称：salesForecast.api.ts
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
  SalesForecast,
  SalesForecastCreate,
  SalesForecastStatus,
  SalesForecastUpdate
} from '@/types/logistics/manufacturing/mds/salesForecast.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSalesForecasts
 */
const SALES_FORECAST_API_BASE = 'TaktSalesForecasts';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取销售预测列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SalesForecast>>} 分页结果
 */
export function getSalesForecastList(queryDto: any): Promise<TaktPagedResult<SalesForecast>> {
  return request<TaktPagedResult<SalesForecast>>({
    url: `${SALES_FORECAST_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取销售预测
 * @param {string} id 销售预测ID
 * @returns {Promise<SalesForecast>} 销售预测DTO
 */
export function getSalesForecastById(id: string): Promise<SalesForecast> {
  return request<SalesForecast>({
    url: `${SALES_FORECAST_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建销售预测
 * @param {SalesForecastCreate} dto 创建DTO
 * @returns {Promise<SalesForecast>} 销售预测DTO
 */
export function createSalesForecast(dto: SalesForecastCreate): Promise<SalesForecast> {
  return request<SalesForecast>({
    url: `${SALES_FORECAST_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新销售预测
 * @param {string} id 销售预测ID
 * @param {SalesForecastUpdate} dto 更新DTO
 * @returns {Promise<SalesForecast>} 销售预测DTO
 */
export function updateSalesForecast(id: string, dto: SalesForecastUpdate): Promise<SalesForecast> {
  return request<SalesForecast>({
    url: `${SALES_FORECAST_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除销售预测
 * @param {string} id 销售预测ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesForecastById(id: string): Promise<void> {
  return request({
    url: `${SALES_FORECAST_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除销售预测
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesForecastBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SALES_FORECAST_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新销售预测状态
 * @param {SalesForecastStatus} dto 状态 DTO
 * @returns {Promise<SalesForecast>} 销售预测DTO
 */
export function updateSalesForecastStatus(dto: SalesForecastStatus): Promise<SalesForecast> {
  return request<SalesForecast>({
    url: `${SALES_FORECAST_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取销售计划选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSalesForecastOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${SALES_FORECAST_API_BASE}/options`,
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
export function getSalesForecastTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_FORECAST_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入销售预测
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSalesForecast(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SALES_FORECAST_API_BASE}/import`,
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
 * 导出销售预测
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSalesForecast(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_FORECAST_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/accounting/financial
// 文件名称：exchangeRate.api.ts
// 创建时间：2026-07-02
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/financial 模块 API（自动生成，请勿手改路由常量）
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
  ExchangeRate,
  ExchangeRateCreate,
  ExchangeRateStatus,
  ExchangeRateUpdate
} from '@/types/accounting/financial/exchangeRate.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktExchangeRates
 */
const EXCHANGE_RATE_API_BASE = 'TaktExchangeRates';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取汇率列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ExchangeRate>>} 分页结果
 */
export function getExchangeRateList(queryDto: any): Promise<TaktPagedResult<ExchangeRate>> {
  return request<TaktPagedResult<ExchangeRate>>({
    url: `${EXCHANGE_RATE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取汇率
 * @param {string} id 汇率ID
 * @returns {Promise<ExchangeRate>} 汇率DTO
 */
export function getExchangeRateById(id: string): Promise<ExchangeRate> {
  return request<ExchangeRate>({
    url: `${EXCHANGE_RATE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建汇率
 * @param {ExchangeRateCreate} dto 创建DTO
 * @returns {Promise<ExchangeRate>} 汇率DTO
 */
export function createExchangeRate(dto: ExchangeRateCreate): Promise<ExchangeRate> {
  return request<ExchangeRate>({
    url: `${EXCHANGE_RATE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新汇率
 * @param {string} id 汇率ID
 * @param {ExchangeRateUpdate} dto 更新DTO
 * @returns {Promise<ExchangeRate>} 汇率DTO
 */
export function updateExchangeRate(id: string, dto: ExchangeRateUpdate): Promise<ExchangeRate> {
  return request<ExchangeRate>({
    url: `${EXCHANGE_RATE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除汇率
 * @param {string} id 汇率ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteExchangeRateById(id: string): Promise<void> {
  return request({
    url: `${EXCHANGE_RATE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除汇率
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteExchangeRateBatch(ids: string[]): Promise<void> {
  return request({
    url: `${EXCHANGE_RATE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新汇率状态
 * @param {ExchangeRateStatus} dto 状态 DTO
 * @returns {Promise<ExchangeRate>} 汇率DTO
 */
export function updateExchangeRateStatus(dto: ExchangeRateStatus): Promise<ExchangeRate> {
  return request<ExchangeRate>({
    url: `${EXCHANGE_RATE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取汇率选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getExchangeRateOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${EXCHANGE_RATE_API_BASE}/options`,
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
export function getExchangeRateTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${EXCHANGE_RATE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入汇率
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importExchangeRate(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${EXCHANGE_RATE_API_BASE}/import`,
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
 * 导出汇率
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportExchangeRate(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EXCHANGE_RATE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

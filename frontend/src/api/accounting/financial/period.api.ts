// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/accounting/financial
// 文件名称：period.api.ts
// 创建时间：2026-07-20
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
  FinancialPeriod,
  FinancialPeriodCreate,
  FinancialPeriodUpdate
} from '@/types/accounting/financial/period.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktFinancialPeriods
 */
const FINANCIAL_PERIOD_API_BASE = 'TaktFinancialPeriods';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取财务期间列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<FinancialPeriod>>} 分页结果
 */
export function getFinancialPeriodList(queryDto: any): Promise<TaktPagedResult<FinancialPeriod>> {
  return request<TaktPagedResult<FinancialPeriod>>({
    url: `${FINANCIAL_PERIOD_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取财务期间
 * @param {string} id 财务期间ID
 * @returns {Promise<FinancialPeriod>} 财务期间DTO
 */
export function getFinancialPeriodById(id: string): Promise<FinancialPeriod> {
  return request<FinancialPeriod>({
    url: `${FINANCIAL_PERIOD_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建财务期间
 * @param {FinancialPeriodCreate} dto 创建DTO
 * @returns {Promise<FinancialPeriod>} 财务期间DTO
 */
export function createFinancialPeriod(dto: FinancialPeriodCreate): Promise<FinancialPeriod> {
  return request<FinancialPeriod>({
    url: `${FINANCIAL_PERIOD_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新财务期间
 * @param {string} id 财务期间ID
 * @param {FinancialPeriodUpdate} dto 更新DTO
 * @returns {Promise<FinancialPeriod>} 财务期间DTO
 */
export function updateFinancialPeriod(id: string, dto: FinancialPeriodUpdate): Promise<FinancialPeriod> {
  return request<FinancialPeriod>({
    url: `${FINANCIAL_PERIOD_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除财务期间
 * @param {string} id 财务期间ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteFinancialPeriodById(id: string): Promise<void> {
  return request({
    url: `${FINANCIAL_PERIOD_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除财务期间
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteFinancialPeriodBatch(ids: string[]): Promise<void> {
  return request({
    url: `${FINANCIAL_PERIOD_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取财务期间选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getFinancialPeriodOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${FINANCIAL_PERIOD_API_BASE}/options`,
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
export function getFinancialPeriodTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${FINANCIAL_PERIOD_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入财务期间
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importFinancialPeriod(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${FINANCIAL_PERIOD_API_BASE}/import`,
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
 * 导出财务期间
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportFinancialPeriod(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${FINANCIAL_PERIOD_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/accounting/controlling
// 文件名称：standardWageRate.api.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/controlling 模块 API（自动生成，请勿手改路由常量）
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
  StandardWageRate,
  StandardWageRateCreate,
  StandardWageRateUpdate
} from '@/types/accounting/controlling/standardWageRate.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktStandardWageRates
 */
const STANDARD_WAGE_RATE_API_BASE = 'TaktStandardWageRates';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取标准工资率列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<StandardWageRate>>} 分页结果
 */
export function getStandardWageRateList(queryDto: any): Promise<TaktPagedResult<StandardWageRate>> {
  return request<TaktPagedResult<StandardWageRate>>({
    url: `${STANDARD_WAGE_RATE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取标准工资率
 * @param {string} id 标准工资率ID
 * @returns {Promise<StandardWageRate>} 标准工资率DTO
 */
export function getStandardWageRateById(id: string): Promise<StandardWageRate> {
  return request<StandardWageRate>({
    url: `${STANDARD_WAGE_RATE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建标准工资率
 * @param {StandardWageRateCreate} dto 创建DTO
 * @returns {Promise<StandardWageRate>} 标准工资率DTO
 */
export function createStandardWageRate(dto: StandardWageRateCreate): Promise<StandardWageRate> {
  return request<StandardWageRate>({
    url: `${STANDARD_WAGE_RATE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新标准工资率
 * @param {string} id 标准工资率ID
 * @param {StandardWageRateUpdate} dto 更新DTO
 * @returns {Promise<StandardWageRate>} 标准工资率DTO
 */
export function updateStandardWageRate(id: string, dto: StandardWageRateUpdate): Promise<StandardWageRate> {
  return request<StandardWageRate>({
    url: `${STANDARD_WAGE_RATE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除标准工资率
 * @param {string} id 标准工资率ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteStandardWageRateById(id: string): Promise<void> {
  return request({
    url: `${STANDARD_WAGE_RATE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除标准工资率
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteStandardWageRateBatch(ids: string[]): Promise<void> {
  return request({
    url: `${STANDARD_WAGE_RATE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取标准工资率选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getStandardWageRateOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${STANDARD_WAGE_RATE_API_BASE}/options`,
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
export function getStandardWageRateTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${STANDARD_WAGE_RATE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入标准工资率
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importStandardWageRate(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${STANDARD_WAGE_RATE_API_BASE}/import`,
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
 * 导出标准工资率
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportStandardWageRate(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${STANDARD_WAGE_RATE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

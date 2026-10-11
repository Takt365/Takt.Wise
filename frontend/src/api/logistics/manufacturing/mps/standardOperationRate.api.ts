// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/mps
// 文件名称：standardOperationRate.api.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/mps 模块 API（自动生成，请勿手改路由常量）
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
  StandardOperationRate,
  StandardOperationRateCreate,
  StandardOperationRateStatus,
  StandardOperationRateUpdate
} from '@/types/logistics/manufacturing/mps/standardOperationRate.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktStandardOperationRates
 */
const STANDARD_OPERATION_RATE_API_BASE = 'TaktStandardOperationRates';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取标准生产稼动率列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<StandardOperationRate>>} 分页结果
 */
export function getStandardOperationRateList(queryDto: any): Promise<TaktPagedResult<StandardOperationRate>> {
  return request<TaktPagedResult<StandardOperationRate>>({
    url: `${STANDARD_OPERATION_RATE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取标准生产稼动率
 * @param {string} id 标准生产稼动率ID
 * @returns {Promise<StandardOperationRate>} 标准生产稼动率DTO
 */
export function getStandardOperationRateById(id: string): Promise<StandardOperationRate> {
  return request<StandardOperationRate>({
    url: `${STANDARD_OPERATION_RATE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建标准生产稼动率
 * @param {StandardOperationRateCreate} dto 创建DTO
 * @returns {Promise<StandardOperationRate>} 标准生产稼动率DTO
 */
export function createStandardOperationRate(dto: StandardOperationRateCreate): Promise<StandardOperationRate> {
  return request<StandardOperationRate>({
    url: `${STANDARD_OPERATION_RATE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新标准生产稼动率
 * @param {string} id 标准生产稼动率ID
 * @param {StandardOperationRateUpdate} dto 更新DTO
 * @returns {Promise<StandardOperationRate>} 标准生产稼动率DTO
 */
export function updateStandardOperationRate(id: string, dto: StandardOperationRateUpdate): Promise<StandardOperationRate> {
  return request<StandardOperationRate>({
    url: `${STANDARD_OPERATION_RATE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除标准生产稼动率
 * @param {string} id 标准生产稼动率ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteStandardOperationRateById(id: string): Promise<void> {
  return request({
    url: `${STANDARD_OPERATION_RATE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除标准生产稼动率
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteStandardOperationRateBatch(ids: string[]): Promise<void> {
  return request({
    url: `${STANDARD_OPERATION_RATE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新标准生产稼动率状态
 * @param {StandardOperationRateStatus} dto 状态 DTO
 * @returns {Promise<StandardOperationRate>} 标准生产稼动率DTO
 */
export function updateStandardOperationRateStatus(dto: StandardOperationRateStatus): Promise<StandardOperationRate> {
  return request<StandardOperationRate>({
    url: `${STANDARD_OPERATION_RATE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取标准生产稼动率选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getStandardOperationRateOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${STANDARD_OPERATION_RATE_API_BASE}/options`,
    method: 'get',
    params: {
      
      ...(plant ? { plantCode: plant } : {}),
      ...(kw ? { keyword: kw } : {}),
    },
  });
}

/**
 * 按生产日期解析有效标准生产稼动率（%）
 * @param {string} plantCode 工厂代码
 * @param {string} prodDate 生产日期 YYYY-MM-DD
 * @param {number} operationType 稼动率类型（默认 1=人员）
 * @returns {Promise<number>} 稼动率(%)
 */
export function getEffectiveStandardOperationRatePercent(
  plantCode: string,
  prodDate: string,
  operationType = 1
): Promise<number> {
  return request<number>({
    url: `${STANDARD_OPERATION_RATE_API_BASE}/effective-rate`,
    method: 'get',
    params: {
      plantCode,
      prodDate,
      operationType,
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
export function getStandardOperationRateTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${STANDARD_OPERATION_RATE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入标准生产稼动率
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importStandardOperationRate(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${STANDARD_OPERATION_RATE_API_BASE}/import`,
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
 * 导出标准生产稼动率
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportStandardOperationRate(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${STANDARD_OPERATION_RATE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

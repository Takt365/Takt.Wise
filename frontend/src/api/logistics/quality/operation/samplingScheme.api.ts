// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/quality/operation
// 文件名称：samplingScheme.api.ts
// 创建时间：2026-06-30
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/operation 模块 API（自动生成，请勿手改路由常量）
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
  SamplingScheme,
  SamplingSchemeCreate,
  SamplingSchemeStatus,
  SamplingSchemeUpdate
} from '@/types/logistics/quality/operation/samplingScheme.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSamplingSchemes
 */
const SAMPLING_SCHEME_API_BASE = 'TaktSamplingSchemes';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取抽样方案列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SamplingScheme>>} 分页结果
 */
export function getSamplingSchemeList(queryDto: any): Promise<TaktPagedResult<SamplingScheme>> {
  return request<TaktPagedResult<SamplingScheme>>({
    url: `${SAMPLING_SCHEME_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取抽样方案
 * @param {string} id 抽样方案ID
 * @returns {Promise<SamplingScheme>} 抽样方案DTO
 */
export function getSamplingSchemeById(id: string): Promise<SamplingScheme> {
  return request<SamplingScheme>({
    url: `${SAMPLING_SCHEME_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建抽样方案
 * @param {SamplingSchemeCreate} dto 创建DTO
 * @returns {Promise<SamplingScheme>} 抽样方案DTO
 */
export function createSamplingScheme(dto: SamplingSchemeCreate): Promise<SamplingScheme> {
  return request<SamplingScheme>({
    url: `${SAMPLING_SCHEME_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新抽样方案
 * @param {string} id 抽样方案ID
 * @param {SamplingSchemeUpdate} dto 更新DTO
 * @returns {Promise<SamplingScheme>} 抽样方案DTO
 */
export function updateSamplingScheme(id: string, dto: SamplingSchemeUpdate): Promise<SamplingScheme> {
  return request<SamplingScheme>({
    url: `${SAMPLING_SCHEME_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除抽样方案
 * @param {string} id 抽样方案ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSamplingSchemeById(id: string): Promise<void> {
  return request({
    url: `${SAMPLING_SCHEME_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除抽样方案
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSamplingSchemeBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SAMPLING_SCHEME_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新抽样方案状态
 * @param {SamplingSchemeStatus} dto 状态 DTO
 * @returns {Promise<SamplingScheme>} 抽样方案DTO
 */
export function updateSamplingSchemeStatus(dto: SamplingSchemeStatus): Promise<SamplingScheme> {
  return request<SamplingScheme>({
    url: `${SAMPLING_SCHEME_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取抽样方案选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSamplingSchemeOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${SAMPLING_SCHEME_API_BASE}/options`,
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
export function getSamplingSchemeTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SAMPLING_SCHEME_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入抽样方案
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSamplingScheme(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SAMPLING_SCHEME_API_BASE}/import`,
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
 * 导出抽样方案
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSamplingScheme(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SAMPLING_SCHEME_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

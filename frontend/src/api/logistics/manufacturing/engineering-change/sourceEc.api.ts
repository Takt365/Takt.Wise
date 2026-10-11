// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：sourceEc.api.ts
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/engineering-change 模块 API（自动生成，请勿手改路由常量）
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
  SourceEc,
  SourceEcCreate,
  SourceEcStatus,
  SourceEcUpdate
} from '@/types/logistics/manufacturing/engineering-change/sourceEc.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSourceEcs
 */
const SOURCE_EC_API_BASE = 'TaktSourceEcs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取设变来源主列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SourceEc>>} 分页结果
 */
export function getSourceEcList(queryDto: any): Promise<TaktPagedResult<SourceEc>> {
  return request<TaktPagedResult<SourceEc>>({
    url: `${SOURCE_EC_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取设变来源主
 * @param {string} id 设变来源主ID
 * @returns {Promise<SourceEc>} 设变来源主DTO
 */
export function getSourceEcById(id: string): Promise<SourceEc> {
  return request<SourceEc>({
    url: `${SOURCE_EC_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建设变来源主
 * @param {SourceEcCreate} dto 创建DTO
 * @returns {Promise<SourceEc>} 设变来源主DTO
 */
export function createSourceEc(dto: SourceEcCreate): Promise<SourceEc> {
  return request<SourceEc>({
    url: `${SOURCE_EC_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新设变来源主
 * @param {string} id 设变来源主ID
 * @param {SourceEcUpdate} dto 更新DTO
 * @returns {Promise<SourceEc>} 设变来源主DTO
 */
export function updateSourceEc(id: string, dto: SourceEcUpdate): Promise<SourceEc> {
  return request<SourceEc>({
    url: `${SOURCE_EC_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除设变来源主
 * @param {string} id 设变来源主ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSourceEcById(id: string): Promise<void> {
  return request({
    url: `${SOURCE_EC_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除设变来源主
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSourceEcBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SOURCE_EC_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新设变来源主状态
 * @param {SourceEcStatus} dto 状态 DTO
 * @returns {Promise<SourceEc>} 设变来源主DTO
 */
export function updateSourceEcStatus(dto: SourceEcStatus): Promise<SourceEc> {
  return request<SourceEc>({
    url: `${SOURCE_EC_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取设变来源主选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSourceEcOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${SOURCE_EC_API_BASE}/options`,
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
export function getSourceEcTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SOURCE_EC_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入设变来源主
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSourceEc(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SOURCE_EC_API_BASE}/import`,
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
 * 导出设变来源主
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSourceEc(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SOURCE_EC_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

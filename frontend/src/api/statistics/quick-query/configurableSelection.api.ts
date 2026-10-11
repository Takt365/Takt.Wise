// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/statistics/quick-query
// 文件名称：configurableSelection.api.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：statistics/quick-query 模块 API（自动生成，请勿手改路由常量）
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
  ConfigurableSelection,
  ConfigurableSelectionCreate,
  ConfigurableSelectionSort,
  ConfigurableSelectionUpdate
} from '@/types/statistics/quick-query/configurableSelection.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktConfigurableSelections
 */
const CONFIGURABLE_SELECTION_API_BASE = 'TaktConfigurableSelections';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取定制报表筛选列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ConfigurableSelection>>} 分页结果
 */
export function getConfigurableSelectionList(queryDto: any): Promise<TaktPagedResult<ConfigurableSelection>> {
  return request<TaktPagedResult<ConfigurableSelection>>({
    url: `${CONFIGURABLE_SELECTION_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取定制报表筛选
 * @param {string} id 定制报表筛选ID
 * @returns {Promise<ConfigurableSelection>} 定制报表筛选DTO
 */
export function getConfigurableSelectionById(id: string): Promise<ConfigurableSelection> {
  return request<ConfigurableSelection>({
    url: `${CONFIGURABLE_SELECTION_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建定制报表筛选
 * @param {ConfigurableSelectionCreate} dto 创建DTO
 * @returns {Promise<ConfigurableSelection>} 定制报表筛选DTO
 */
export function createConfigurableSelection(dto: ConfigurableSelectionCreate): Promise<ConfigurableSelection> {
  return request<ConfigurableSelection>({
    url: `${CONFIGURABLE_SELECTION_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新定制报表筛选
 * @param {string} id 定制报表筛选ID
 * @param {ConfigurableSelectionUpdate} dto 更新DTO
 * @returns {Promise<ConfigurableSelection>} 定制报表筛选DTO
 */
export function updateConfigurableSelection(id: string, dto: ConfigurableSelectionUpdate): Promise<ConfigurableSelection> {
  return request<ConfigurableSelection>({
    url: `${CONFIGURABLE_SELECTION_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除定制报表筛选
 * @param {string} id 定制报表筛选ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteConfigurableSelectionById(id: string): Promise<void> {
  return request({
    url: `${CONFIGURABLE_SELECTION_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除定制报表筛选
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteConfigurableSelectionBatch(ids: string[]): Promise<void> {
  return request({
    url: `${CONFIGURABLE_SELECTION_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新定制报表筛选排序
 * @param {ConfigurableSelectionSort} dto 排序DTO
 * @returns {Promise<ConfigurableSelection>} 定制报表筛选DTO
 */
export function updateConfigurableSelectionSort(dto: ConfigurableSelectionSort): Promise<ConfigurableSelection> {
  return request<ConfigurableSelection>({
    url: `${CONFIGURABLE_SELECTION_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取定制报表筛选选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getConfigurableSelectionOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${CONFIGURABLE_SELECTION_API_BASE}/options`,
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
export function getConfigurableSelectionTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${CONFIGURABLE_SELECTION_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入定制报表筛选
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importConfigurableSelection(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${CONFIGURABLE_SELECTION_API_BASE}/import`,
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
 * 导出定制报表筛选
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportConfigurableSelection(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${CONFIGURABLE_SELECTION_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

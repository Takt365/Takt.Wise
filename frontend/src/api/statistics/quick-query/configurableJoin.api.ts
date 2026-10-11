// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/statistics/quick-query
// 文件名称：configurableJoin.api.ts
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
  ConfigurableJoin,
  ConfigurableJoinCreate,
  ConfigurableJoinSort,
  ConfigurableJoinUpdate
} from '@/types/statistics/quick-query/configurableJoin.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktConfigurableJoins
 */
const CONFIGURABLE_JOIN_API_BASE = 'TaktConfigurableJoins';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取定制报表关联列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ConfigurableJoin>>} 分页结果
 */
export function getConfigurableJoinList(queryDto: any): Promise<TaktPagedResult<ConfigurableJoin>> {
  return request<TaktPagedResult<ConfigurableJoin>>({
    url: `${CONFIGURABLE_JOIN_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取定制报表关联
 * @param {string} id 定制报表关联ID
 * @returns {Promise<ConfigurableJoin>} 定制报表关联DTO
 */
export function getConfigurableJoinById(id: string): Promise<ConfigurableJoin> {
  return request<ConfigurableJoin>({
    url: `${CONFIGURABLE_JOIN_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建定制报表关联
 * @param {ConfigurableJoinCreate} dto 创建DTO
 * @returns {Promise<ConfigurableJoin>} 定制报表关联DTO
 */
export function createConfigurableJoin(dto: ConfigurableJoinCreate): Promise<ConfigurableJoin> {
  return request<ConfigurableJoin>({
    url: `${CONFIGURABLE_JOIN_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新定制报表关联
 * @param {string} id 定制报表关联ID
 * @param {ConfigurableJoinUpdate} dto 更新DTO
 * @returns {Promise<ConfigurableJoin>} 定制报表关联DTO
 */
export function updateConfigurableJoin(id: string, dto: ConfigurableJoinUpdate): Promise<ConfigurableJoin> {
  return request<ConfigurableJoin>({
    url: `${CONFIGURABLE_JOIN_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除定制报表关联
 * @param {string} id 定制报表关联ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteConfigurableJoinById(id: string): Promise<void> {
  return request({
    url: `${CONFIGURABLE_JOIN_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除定制报表关联
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteConfigurableJoinBatch(ids: string[]): Promise<void> {
  return request({
    url: `${CONFIGURABLE_JOIN_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新定制报表关联排序
 * @param {ConfigurableJoinSort} dto 排序DTO
 * @returns {Promise<ConfigurableJoin>} 定制报表关联DTO
 */
export function updateConfigurableJoinSort(dto: ConfigurableJoinSort): Promise<ConfigurableJoin> {
  return request<ConfigurableJoin>({
    url: `${CONFIGURABLE_JOIN_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取定制报表关联选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getConfigurableJoinOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${CONFIGURABLE_JOIN_API_BASE}/options`,
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
export function getConfigurableJoinTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${CONFIGURABLE_JOIN_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入定制报表关联
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importConfigurableJoin(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${CONFIGURABLE_JOIN_API_BASE}/import`,
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
 * 导出定制报表关联
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportConfigurableJoin(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${CONFIGURABLE_JOIN_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/routine/help-desk
// 文件名称：itAsset.api.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/help-desk 模块 API（自动生成，请勿手改路由常量）
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
  ItAsset,
  ItAssetCreate,
  ItAssetUpdate
} from '@/types/routine/help-desk/itAsset.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktItAssets
 */
const IT_ASSET_API_BASE = 'TaktItAssets';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取IT设备保修扩展列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ItAsset>>} 分页结果
 */
export function getItAssetList(queryDto: any): Promise<TaktPagedResult<ItAsset>> {
  return request<TaktPagedResult<ItAsset>>({
    url: `${IT_ASSET_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取IT设备保修扩展
 * @param {string} id IT设备保修扩展ID
 * @returns {Promise<ItAsset>} IT设备保修扩展DTO
 */
export function getItAssetById(id: string): Promise<ItAsset> {
  return request<ItAsset>({
    url: `${IT_ASSET_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建IT设备保修扩展
 * @param {ItAssetCreate} dto 创建DTO
 * @returns {Promise<ItAsset>} IT设备保修扩展DTO
 */
export function createItAsset(dto: ItAssetCreate): Promise<ItAsset> {
  return request<ItAsset>({
    url: `${IT_ASSET_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新IT设备保修扩展
 * @param {string} id IT设备保修扩展ID
 * @param {ItAssetUpdate} dto 更新DTO
 * @returns {Promise<ItAsset>} IT设备保修扩展DTO
 */
export function updateItAsset(id: string, dto: ItAssetUpdate): Promise<ItAsset> {
  return request<ItAsset>({
    url: `${IT_ASSET_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除IT设备保修扩展
 * @param {string} id IT设备保修扩展ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteItAssetById(id: string): Promise<void> {
  return request({
    url: `${IT_ASSET_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除IT设备保修扩展
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteItAssetBatch(ids: string[]): Promise<void> {
  return request({
    url: `${IT_ASSET_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取IT设备保修扩展选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getItAssetOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${IT_ASSET_API_BASE}/options`,
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
export function getItAssetTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${IT_ASSET_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入IT设备保修扩展
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importItAsset(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${IT_ASSET_API_BASE}/import`,
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
 * 导出IT设备保修扩展
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportItAsset(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${IT_ASSET_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

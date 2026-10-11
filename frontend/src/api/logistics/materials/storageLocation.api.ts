// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/materials
// 文件名称：storageLocation.api.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/materials 模块 API（自动生成，请勿手改路由常量）
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
  StorageLocation,
  StorageLocationCreate,
  StorageLocationSort,
  StorageLocationStatus,
  StorageLocationUpdate
} from '@/types/logistics/materials/storageLocation.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktStorageLocations
 */
const STORAGE_LOCATION_API_BASE = 'TaktStorageLocations';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取库位主数据列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<StorageLocation>>} 分页结果
 */
export function getStorageLocationList(queryDto: any): Promise<TaktPagedResult<StorageLocation>> {
  return request<TaktPagedResult<StorageLocation>>({
    url: `${STORAGE_LOCATION_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取库位主数据
 * @param {string} id 库位主数据ID
 * @returns {Promise<StorageLocation>} 库位主数据DTO
 */
export function getStorageLocationById(id: string): Promise<StorageLocation> {
  return request<StorageLocation>({
    url: `${STORAGE_LOCATION_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建库位主数据
 * @param {StorageLocationCreate} dto 创建DTO
 * @returns {Promise<StorageLocation>} 库位主数据DTO
 */
export function createStorageLocation(dto: StorageLocationCreate): Promise<StorageLocation> {
  return request<StorageLocation>({
    url: `${STORAGE_LOCATION_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新库位主数据
 * @param {string} id 库位主数据ID
 * @param {StorageLocationUpdate} dto 更新DTO
 * @returns {Promise<StorageLocation>} 库位主数据DTO
 */
export function updateStorageLocation(id: string, dto: StorageLocationUpdate): Promise<StorageLocation> {
  return request<StorageLocation>({
    url: `${STORAGE_LOCATION_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除库位主数据
 * @param {string} id 库位主数据ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteStorageLocationById(id: string): Promise<void> {
  return request({
    url: `${STORAGE_LOCATION_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除库位主数据
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteStorageLocationBatch(ids: string[]): Promise<void> {
  return request({
    url: `${STORAGE_LOCATION_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新库位主数据状态
 * @param {StorageLocationStatus} dto 状态 DTO
 * @returns {Promise<StorageLocation>} 库位主数据DTO
 */
export function updateStorageLocationStatus(dto: StorageLocationStatus): Promise<StorageLocation> {
  return request<StorageLocation>({
    url: `${STORAGE_LOCATION_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新库位主数据排序
 * @param {StorageLocationSort} dto 排序DTO
 * @returns {Promise<StorageLocation>} 库位主数据DTO
 */
export function updateStorageLocationSort(dto: StorageLocationSort): Promise<StorageLocation> {
  return request<StorageLocation>({
    url: `${STORAGE_LOCATION_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取库位主数据选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getStorageLocationOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${STORAGE_LOCATION_API_BASE}/options`,
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
export function getStorageLocationTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${STORAGE_LOCATION_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入库位主数据
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importStorageLocation(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${STORAGE_LOCATION_API_BASE}/import`,
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
 * 导出库位主数据
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportStorageLocation(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${STORAGE_LOCATION_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

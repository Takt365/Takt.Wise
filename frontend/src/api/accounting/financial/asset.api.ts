// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/accounting/financial
// 文件名称：asset.api.ts
// 创建时间：2026-08-30
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
  Asset,
  AssetCreate,
  AssetStatus,
  AssetUpdate
} from '@/types/accounting/financial/asset.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktAssets
 */
const ASSET_API_BASE = 'TaktAssets';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取资产列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Asset>>} 分页结果
 */
export function getAssetList(queryDto: any): Promise<TaktPagedResult<Asset>> {
  return request<TaktPagedResult<Asset>>({
    url: `${ASSET_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取资产
 * @param {string} id 资产ID
 * @returns {Promise<Asset>} 资产DTO
 */
export function getAssetById(id: string): Promise<Asset> {
  return request<Asset>({
    url: `${ASSET_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建资产
 * @param {AssetCreate} dto 创建DTO
 * @returns {Promise<Asset>} 资产DTO
 */
export function createAsset(dto: AssetCreate): Promise<Asset> {
  return request<Asset>({
    url: `${ASSET_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新资产
 * @param {string} id 资产ID
 * @param {AssetUpdate} dto 更新DTO
 * @returns {Promise<Asset>} 资产DTO
 */
export function updateAsset(id: string, dto: AssetUpdate): Promise<Asset> {
  return request<Asset>({
    url: `${ASSET_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除资产
 * @param {string} id 资产ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteAssetById(id: string): Promise<void> {
  return request({
    url: `${ASSET_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除资产
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteAssetBatch(ids: string[]): Promise<void> {
  return request({
    url: `${ASSET_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新资产状态
 * @param {AssetStatus} dto 状态 DTO
 * @returns {Promise<Asset>} 资产DTO
 */
export function updateAssetStatus(dto: AssetStatus): Promise<Asset> {
  return request<Asset>({
    url: `${ASSET_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取资产选项列表
 * @param {string} plantCode 工厂代码（可选，用于按工厂过滤）
 * @param {string} keyword 搜索关键字（可选，模糊匹配）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getAssetOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${ASSET_API_BASE}/options`,
    method: 'get',
    params: {
      plantCode,
      keyword
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
export function getAssetTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${ASSET_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入资产
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importAsset(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${ASSET_API_BASE}/import`,
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
 * 导出资产
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportAsset(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${ASSET_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

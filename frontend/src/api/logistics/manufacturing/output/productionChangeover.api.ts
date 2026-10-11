// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/output
// 文件名称：productionChangeover.api.ts
// 创建时间：2026-07-06
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/output 模块 API（自动生成，请勿手改路由常量）
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
  ProductionChangeover,
  ProductionChangeoverCreate,
  ProductionChangeoverUpdate
} from '@/types/logistics/manufacturing/output/productionChangeover.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktProductionChangeovers
 */
const PRODUCTION_CHANGEOVER_API_BASE = 'TaktProductionChangeovers';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取生产切换记录列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ProductionChangeover>>} 分页结果
 */
export function getProductionChangeoverList(queryDto: any): Promise<TaktPagedResult<ProductionChangeover>> {
  return request<TaktPagedResult<ProductionChangeover>>({
    url: `${PRODUCTION_CHANGEOVER_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取生产切换记录
 * @param {string} id 生产切换记录ID
 * @returns {Promise<ProductionChangeover>} 生产切换记录DTO
 */
export function getProductionChangeoverById(id: string): Promise<ProductionChangeover> {
  return request<ProductionChangeover>({
    url: `${PRODUCTION_CHANGEOVER_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建生产切换记录
 * @param {ProductionChangeoverCreate} dto 创建DTO
 * @returns {Promise<ProductionChangeover>} 生产切换记录DTO
 */
export function createProductionChangeover(dto: ProductionChangeoverCreate): Promise<ProductionChangeover> {
  return request<ProductionChangeover>({
    url: `${PRODUCTION_CHANGEOVER_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新生产切换记录
 * @param {string} id 生产切换记录ID
 * @param {ProductionChangeoverUpdate} dto 更新DTO
 * @returns {Promise<ProductionChangeover>} 生产切换记录DTO
 */
export function updateProductionChangeover(id: string, dto: ProductionChangeoverUpdate): Promise<ProductionChangeover> {
  return request<ProductionChangeover>({
    url: `${PRODUCTION_CHANGEOVER_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除生产切换记录
 * @param {string} id 生产切换记录ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteProductionChangeoverById(id: string): Promise<void> {
  return request({
    url: `${PRODUCTION_CHANGEOVER_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除生产切换记录
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteProductionChangeoverBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PRODUCTION_CHANGEOVER_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取生产切换记录选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getProductionChangeoverOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${PRODUCTION_CHANGEOVER_API_BASE}/options`,
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
export function getProductionChangeoverTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PRODUCTION_CHANGEOVER_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入生产切换记录
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importProductionChangeover(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PRODUCTION_CHANGEOVER_API_BASE}/import`,
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
 * 导出生产切换记录
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportProductionChangeover(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PRODUCTION_CHANGEOVER_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

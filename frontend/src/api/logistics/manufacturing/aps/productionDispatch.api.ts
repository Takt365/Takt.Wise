// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/aps
// 文件名称：productionDispatch.api.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/aps 模块 API（自动生成，请勿手改路由常量）
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
  ProductionDispatch,
  ProductionDispatchCreate,
  ProductionDispatchStatus,
  ProductionDispatchUpdate
} from '@/types/logistics/manufacturing/aps/productionDispatch.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktProductionDispatches
 */
const PRODUCTION_DISPATCH_API_BASE = 'TaktProductionDispatches';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取生产派工单列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ProductionDispatch>>} 分页结果
 */
export function getProductionDispatchList(queryDto: any): Promise<TaktPagedResult<ProductionDispatch>> {
  return request<TaktPagedResult<ProductionDispatch>>({
    url: `${PRODUCTION_DISPATCH_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取生产派工单
 * @param {string} id 生产派工单ID
 * @returns {Promise<ProductionDispatch>} 生产派工单DTO
 */
export function getProductionDispatchById(id: string): Promise<ProductionDispatch> {
  return request<ProductionDispatch>({
    url: `${PRODUCTION_DISPATCH_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建生产派工单
 * @param {ProductionDispatchCreate} dto 创建DTO
 * @returns {Promise<ProductionDispatch>} 生产派工单DTO
 */
export function createProductionDispatch(dto: ProductionDispatchCreate): Promise<ProductionDispatch> {
  return request<ProductionDispatch>({
    url: `${PRODUCTION_DISPATCH_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新生产派工单
 * @param {string} id 生产派工单ID
 * @param {ProductionDispatchUpdate} dto 更新DTO
 * @returns {Promise<ProductionDispatch>} 生产派工单DTO
 */
export function updateProductionDispatch(id: string, dto: ProductionDispatchUpdate): Promise<ProductionDispatch> {
  return request<ProductionDispatch>({
    url: `${PRODUCTION_DISPATCH_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除生产派工单
 * @param {string} id 生产派工单ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteProductionDispatchById(id: string): Promise<void> {
  return request({
    url: `${PRODUCTION_DISPATCH_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除生产派工单
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteProductionDispatchBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PRODUCTION_DISPATCH_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新生产派工单状态
 * @param {ProductionDispatchStatus} dto 状态 DTO
 * @returns {Promise<ProductionDispatch>} 生产派工单DTO
 */
export function updateProductionDispatchStatus(dto: ProductionDispatchStatus): Promise<ProductionDispatch> {
  return request<ProductionDispatch>({
    url: `${PRODUCTION_DISPATCH_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取生产派工单选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getProductionDispatchOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${PRODUCTION_DISPATCH_API_BASE}/options`,
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
export function getProductionDispatchTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PRODUCTION_DISPATCH_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入生产派工单
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importProductionDispatch(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PRODUCTION_DISPATCH_API_BASE}/import`,
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
 * 导出生产派工单
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportProductionDispatch(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PRODUCTION_DISPATCH_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

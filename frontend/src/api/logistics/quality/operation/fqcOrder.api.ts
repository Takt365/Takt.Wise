// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/quality/operation
// 文件名称：fqcOrder.api.ts
// 创建时间：2026-07-23
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
  FqcOrder,
  FqcOrderCreate,
  FqcOrderStatus,
  FqcOrderUpdate
} from '@/types/logistics/quality/operation/fqcOrder.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktFqcOrders
 */
const FQC_ORDER_API_BASE = 'TaktFqcOrders';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取出货检验单列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<FqcOrder>>} 分页结果
 */
export function getFqcOrderList(queryDto: any): Promise<TaktPagedResult<FqcOrder>> {
  return request<TaktPagedResult<FqcOrder>>({
    url: `${FQC_ORDER_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取出货检验单
 * @param {string} id 出货检验单ID
 * @returns {Promise<FqcOrder>} 出货检验单DTO
 */
export function getFqcOrderById(id: string): Promise<FqcOrder> {
  return request<FqcOrder>({
    url: `${FQC_ORDER_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建出货检验单
 * @param {FqcOrderCreate} dto 创建DTO
 * @returns {Promise<FqcOrder>} 出货检验单DTO
 */
export function createFqcOrder(dto: FqcOrderCreate): Promise<FqcOrder> {
  return request<FqcOrder>({
    url: `${FQC_ORDER_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新出货检验单
 * @param {string} id 出货检验单ID
 * @param {FqcOrderUpdate} dto 更新DTO
 * @returns {Promise<FqcOrder>} 出货检验单DTO
 */
export function updateFqcOrder(id: string, dto: FqcOrderUpdate): Promise<FqcOrder> {
  return request<FqcOrder>({
    url: `${FQC_ORDER_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除出货检验单
 * @param {string} id 出货检验单ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteFqcOrderById(id: string): Promise<void> {
  return request({
    url: `${FQC_ORDER_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除出货检验单
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteFqcOrderBatch(ids: string[]): Promise<void> {
  return request({
    url: `${FQC_ORDER_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新出货检验单状态
 * @param {FqcOrderStatus} dto 状态 DTO
 * @returns {Promise<FqcOrder>} 出货检验单DTO
 */
export function updateFqcOrderStatus(dto: FqcOrderStatus): Promise<FqcOrder> {
  return request<FqcOrder>({
    url: `${FQC_ORDER_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取出货检验单选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getFqcOrderOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${FQC_ORDER_API_BASE}/options`,
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
export function getFqcOrderTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${FQC_ORDER_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入出货检验单
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importFqcOrder(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${FQC_ORDER_API_BASE}/import`,
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
 * 导出出货检验单
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportFqcOrder(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${FQC_ORDER_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

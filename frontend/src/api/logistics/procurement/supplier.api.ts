// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/procurement
// 文件名称：supplier.api.ts
// 创建时间：2026-08-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/procurement 模块 API（自动生成，请勿手改路由常量）
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
  Supplier,
  SupplierCreate,
  SupplierSort,
  SupplierStatus,
  SupplierUpdate
} from '@/types/logistics/procurement/supplier.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSuppliers
 */
const SUPPLIER_API_BASE = 'TaktSuppliers';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取供货商信息列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Supplier>>} 分页结果
 */
export function getSupplierList(queryDto: any): Promise<TaktPagedResult<Supplier>> {
  return request<TaktPagedResult<Supplier>>({
    url: `${SUPPLIER_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取供货商信息
 * @param {string} id 供货商信息ID
 * @returns {Promise<Supplier>} 供货商信息DTO
 */
export function getSupplierById(id: string): Promise<Supplier> {
  return request<Supplier>({
    url: `${SUPPLIER_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建供货商信息
 * @param {SupplierCreate} dto 创建DTO
 * @returns {Promise<Supplier>} 供货商信息DTO
 */
export function createSupplier(dto: SupplierCreate): Promise<Supplier> {
  return request<Supplier>({
    url: `${SUPPLIER_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新供货商信息
 * @param {string} id 供货商信息ID
 * @param {SupplierUpdate} dto 更新DTO
 * @returns {Promise<Supplier>} 供货商信息DTO
 */
export function updateSupplier(id: string, dto: SupplierUpdate): Promise<Supplier> {
  return request<Supplier>({
    url: `${SUPPLIER_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除供货商信息
 * @param {string} id 供货商信息ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSupplierById(id: string): Promise<void> {
  return request({
    url: `${SUPPLIER_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除供货商信息
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSupplierBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SUPPLIER_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新供货商信息状态
 * @param {SupplierStatus} dto 状态 DTO
 * @returns {Promise<Supplier>} 供货商信息DTO
 */
export function updateSupplierStatus(dto: SupplierStatus): Promise<Supplier> {
  return request<Supplier>({
    url: `${SUPPLIER_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新供货商信息排序
 * @param {SupplierSort} dto 排序DTO
 * @returns {Promise<Supplier>} 供货商信息DTO
 */
export function updateSupplierSort(dto: SupplierSort): Promise<Supplier> {
  return request<Supplier>({
    url: `${SUPPLIER_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取供货商信息选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSupplierOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${SUPPLIER_API_BASE}/options`,
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
export function getSupplierTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SUPPLIER_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入供货商信息
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSupplier(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SUPPLIER_API_BASE}/import`,
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
 * 导出供货商信息
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSupplier(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SUPPLIER_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

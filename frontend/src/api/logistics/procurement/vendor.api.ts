// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/procurement
// 文件名称：vendor.api.ts
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
  Vendor,
  VendorCreate,
  VendorSort,
  VendorStatus,
  VendorUpdate
} from '@/types/logistics/procurement/vendor.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktVendors
 */
const VENDOR_API_BASE = 'TaktVendors';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取经销商信息列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Vendor>>} 分页结果
 */
export function getVendorList(queryDto: any): Promise<TaktPagedResult<Vendor>> {
  return request<TaktPagedResult<Vendor>>({
    url: `${VENDOR_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取经销商信息
 * @param {string} id 经销商信息ID
 * @returns {Promise<Vendor>} 经销商信息DTO
 */
export function getVendorById(id: string): Promise<Vendor> {
  return request<Vendor>({
    url: `${VENDOR_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建经销商信息
 * @param {VendorCreate} dto 创建DTO
 * @returns {Promise<Vendor>} 经销商信息DTO
 */
export function createVendor(dto: VendorCreate): Promise<Vendor> {
  return request<Vendor>({
    url: `${VENDOR_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新经销商信息
 * @param {string} id 经销商信息ID
 * @param {VendorUpdate} dto 更新DTO
 * @returns {Promise<Vendor>} 经销商信息DTO
 */
export function updateVendor(id: string, dto: VendorUpdate): Promise<Vendor> {
  return request<Vendor>({
    url: `${VENDOR_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除经销商信息
 * @param {string} id 经销商信息ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteVendorById(id: string): Promise<void> {
  return request({
    url: `${VENDOR_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除经销商信息
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteVendorBatch(ids: string[]): Promise<void> {
  return request({
    url: `${VENDOR_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新经销商信息状态
 * @param {VendorStatus} dto 状态 DTO
 * @returns {Promise<Vendor>} 经销商信息DTO
 */
export function updateVendorStatus(dto: VendorStatus): Promise<Vendor> {
  return request<Vendor>({
    url: `${VENDOR_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新经销商信息排序
 * @param {VendorSort} dto 排序DTO
 * @returns {Promise<Vendor>} 经销商信息DTO
 */
export function updateVendorSort(dto: VendorSort): Promise<Vendor> {
  return request<Vendor>({
    url: `${VENDOR_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取经销商信息选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getVendorOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${VENDOR_API_BASE}/options`,
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
export function getVendorTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${VENDOR_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入经销商信息
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importVendor(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${VENDOR_API_BASE}/import`,
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
 * 导出经销商信息
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportVendor(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${VENDOR_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

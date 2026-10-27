// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/sales
// 文件名称：customer.api.ts
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/sales 模块 API（自动生成，请勿手改路由常量）
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
  Customer,
  CustomerCreate,
  CustomerSort,
  CustomerStatus,
  CustomerUpdate
} from '@/types/logistics/sales/customer.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktCustomers
 */
const CUSTOMER_API_BASE = 'TaktCustomers';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取客户信息列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Customer>>} 分页结果
 */
export function getCustomerList(queryDto: any): Promise<TaktPagedResult<Customer>> {
  return request<TaktPagedResult<Customer>>({
    url: `${CUSTOMER_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取客户信息
 * @param {string} id 客户信息ID
 * @returns {Promise<Customer>} 客户信息DTO
 */
export function getCustomerById(id: string): Promise<Customer> {
  return request<Customer>({
    url: `${CUSTOMER_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 创建客户信息
 * @param {CustomerCreate} dto 创建DTO
 * @returns {Promise<Customer>} 客户信息DTO
 */
export function createCustomer(dto: CustomerCreate): Promise<Customer> {
  return request<Customer>({
    url: `${CUSTOMER_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新客户信息
 * @param {string} id 客户信息ID
 * @param {CustomerUpdate} dto 更新DTO
 * @returns {Promise<Customer>} 客户信息DTO
 */
export function updateCustomer(id: string, dto: CustomerUpdate): Promise<Customer> {
  return request<Customer>({
    url: `${CUSTOMER_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除客户信息
 * @param {string} id 客户信息ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteCustomerById(id: string): Promise<void> {
  return request({
    url: `${CUSTOMER_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除客户信息
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteCustomerBatch(ids: string[]): Promise<void> {
  return request({
    url: `${CUSTOMER_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新客户信息状态
 * @param {CustomerStatus} dto 状态 DTO
 * @returns {Promise<Customer>} 客户信息DTO
 */
export function updateCustomerStatus(dto: CustomerStatus): Promise<Customer> {
  return request<Customer>({
    url: `${CUSTOMER_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新客户信息排序
 * @param {CustomerSort} dto 排序DTO
 * @returns {Promise<Customer>} 客户信息DTO
 */
export function updateCustomerSort(dto: CustomerSort): Promise<Customer> {
  return request<Customer>({
    url: `${CUSTOMER_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取客户信息选项列表
 * @param {string} plantCode 工厂代码（可选，用于按工厂过滤）
 * @param {string} keyword 搜索关键字（可选，模糊匹配）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getCustomerOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${CUSTOMER_API_BASE}/options`,
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
export function getCustomerTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${CUSTOMER_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入客户信息
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importCustomer(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${CUSTOMER_API_BASE}/import`,
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
 * 导出客户信息
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportCustomer(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${CUSTOMER_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

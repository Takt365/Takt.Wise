// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/quality/complaint
// 文件名称：customerComplaintHandling.api.ts
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/complaint 模块 API（自动生成，请勿手改路由常量）
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
  CustomerComplaintHandling,
  CustomerComplaintHandlingCreate,
  CustomerComplaintHandlingStatus,
  CustomerComplaintHandlingUpdate
} from '@/types/logistics/quality/complaint/customerComplaintHandling.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktCustomerComplaintHandlings
 */
const CUSTOMER_COMPLAINT_HANDLING_API_BASE = 'TaktCustomerComplaintHandlings';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取客诉处理记录列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<CustomerComplaintHandling>>} 分页结果
 */
export function getCustomerComplaintHandlingList(queryDto: any): Promise<TaktPagedResult<CustomerComplaintHandling>> {
  return request<TaktPagedResult<CustomerComplaintHandling>>({
    url: `${CUSTOMER_COMPLAINT_HANDLING_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取客诉处理记录
 * @param {string} id 客诉处理记录ID
 * @returns {Promise<CustomerComplaintHandling>} 客诉处理记录DTO
 */
export function getCustomerComplaintHandlingById(id: string): Promise<CustomerComplaintHandling> {
  return request<CustomerComplaintHandling>({
    url: `${CUSTOMER_COMPLAINT_HANDLING_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建客诉处理记录
 * @param {CustomerComplaintHandlingCreate} dto 创建DTO
 * @returns {Promise<CustomerComplaintHandling>} 客诉处理记录DTO
 */
export function createCustomerComplaintHandling(dto: CustomerComplaintHandlingCreate): Promise<CustomerComplaintHandling> {
  return request<CustomerComplaintHandling>({
    url: `${CUSTOMER_COMPLAINT_HANDLING_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新客诉处理记录
 * @param {string} id 客诉处理记录ID
 * @param {CustomerComplaintHandlingUpdate} dto 更新DTO
 * @returns {Promise<CustomerComplaintHandling>} 客诉处理记录DTO
 */
export function updateCustomerComplaintHandling(id: string, dto: CustomerComplaintHandlingUpdate): Promise<CustomerComplaintHandling> {
  return request<CustomerComplaintHandling>({
    url: `${CUSTOMER_COMPLAINT_HANDLING_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除客诉处理记录
 * @param {string} id 客诉处理记录ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteCustomerComplaintHandlingById(id: string): Promise<void> {
  return request({
    url: `${CUSTOMER_COMPLAINT_HANDLING_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除客诉处理记录
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteCustomerComplaintHandlingBatch(ids: string[]): Promise<void> {
  return request({
    url: `${CUSTOMER_COMPLAINT_HANDLING_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新客诉处理记录状态
 * @param {CustomerComplaintHandlingStatus} dto 状态 DTO
 * @returns {Promise<CustomerComplaintHandling>} 客诉处理记录DTO
 */
export function updateCustomerComplaintHandlingStatus(dto: CustomerComplaintHandlingStatus): Promise<CustomerComplaintHandling> {
  return request<CustomerComplaintHandling>({
    url: `${CUSTOMER_COMPLAINT_HANDLING_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取客诉处理记录选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getCustomerComplaintHandlingOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${CUSTOMER_COMPLAINT_HANDLING_API_BASE}/options`,
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
export function getCustomerComplaintHandlingTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${CUSTOMER_COMPLAINT_HANDLING_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入客诉处理记录
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importCustomerComplaintHandling(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${CUSTOMER_COMPLAINT_HANDLING_API_BASE}/import`,
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
 * 导出客诉处理记录
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportCustomerComplaintHandling(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${CUSTOMER_COMPLAINT_HANDLING_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/quality/complaint
// 文件名称：customerComplaint.api.ts
// 创建时间：2026-09-04
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
  CustomerComplaint,
  CustomerComplaintCreate,
  CustomerComplaintSort,
  CustomerComplaintStatus,
  CustomerComplaintUpdate
} from '@/types/logistics/quality/complaint/customerComplaint.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktCustomerComplaints
 */
const CUSTOMER_COMPLAINT_API_BASE = 'TaktCustomerComplaints';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取客诉主列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<CustomerComplaint>>} 分页结果
 */
export function getCustomerComplaintList(queryDto: any): Promise<TaktPagedResult<CustomerComplaint>> {
  return request<TaktPagedResult<CustomerComplaint>>({
    url: `${CUSTOMER_COMPLAINT_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取客诉主
 * @param {string} id 客诉主ID
 * @returns {Promise<CustomerComplaint>} 客诉主DTO
 */
export function getCustomerComplaintById(id: string): Promise<CustomerComplaint> {
  return request<CustomerComplaint>({
    url: `${CUSTOMER_COMPLAINT_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 创建客诉主
 * @param {CustomerComplaintCreate} dto 创建DTO
 * @returns {Promise<CustomerComplaint>} 客诉主DTO
 */
export function createCustomerComplaint(dto: CustomerComplaintCreate): Promise<CustomerComplaint> {
  return request<CustomerComplaint>({
    url: `${CUSTOMER_COMPLAINT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新客诉主
 * @param {string} id 客诉主ID
 * @param {CustomerComplaintUpdate} dto 更新DTO
 * @returns {Promise<CustomerComplaint>} 客诉主DTO
 */
export function updateCustomerComplaint(id: string, dto: CustomerComplaintUpdate): Promise<CustomerComplaint> {
  return request<CustomerComplaint>({
    url: `${CUSTOMER_COMPLAINT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除客诉主
 * @param {string} id 客诉主ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteCustomerComplaintById(id: string): Promise<void> {
  return request({
    url: `${CUSTOMER_COMPLAINT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除客诉主
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteCustomerComplaintBatch(ids: string[]): Promise<void> {
  return request({
    url: `${CUSTOMER_COMPLAINT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新客诉主状态
 * @param {CustomerComplaintStatus} dto 状态 DTO
 * @returns {Promise<CustomerComplaint>} 客诉主DTO
 */
export function updateCustomerComplaintStatus(dto: CustomerComplaintStatus): Promise<CustomerComplaint> {
  return request<CustomerComplaint>({
    url: `${CUSTOMER_COMPLAINT_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新客诉主排序
 * @param {CustomerComplaintSort} dto 排序DTO
 * @returns {Promise<CustomerComplaint>} 客诉主DTO
 */
export function updateCustomerComplaintSort(dto: CustomerComplaintSort): Promise<CustomerComplaint> {
  return request<CustomerComplaint>({
    url: `${CUSTOMER_COMPLAINT_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取客诉主选项列表
 * @param {string} plantCode 工厂代码（可选，用于按工厂过滤）
 * @param {string} keyword 搜索关键字（可选，模糊匹配）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getCustomerComplaintOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${CUSTOMER_COMPLAINT_API_BASE}/options`,
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
export function getCustomerComplaintTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${CUSTOMER_COMPLAINT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入客诉主
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importCustomerComplaint(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${CUSTOMER_COMPLAINT_API_BASE}/import`,
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
 * 导出客诉主
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportCustomerComplaint(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${CUSTOMER_COMPLAINT_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

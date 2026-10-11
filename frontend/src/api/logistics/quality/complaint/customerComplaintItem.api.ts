// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/quality/complaint
// 文件名称：customerComplaintItem.api.ts
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
  CustomerComplaintItem,
  CustomerComplaintItemCreate,
  CustomerComplaintItemObsolete,
  CustomerComplaintItemStatus,
  CustomerComplaintItemUpdate
} from '@/types/logistics/quality/complaint/customerComplaintItem.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktCustomerComplaintItems
 */
const CUSTOMER_COMPLAINT_ITEM_API_BASE = 'TaktCustomerComplaintItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取客诉明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<CustomerComplaintItem>>} 分页结果
 */
export function getCustomerComplaintItemList(queryDto: any): Promise<TaktPagedResult<CustomerComplaintItem>> {
  return request<TaktPagedResult<CustomerComplaintItem>>({
    url: `${CUSTOMER_COMPLAINT_ITEM_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取客诉明细
 * @param {string} id 客诉明细ID
 * @returns {Promise<CustomerComplaintItem>} 客诉明细DTO
 */
export function getCustomerComplaintItemById(id: string): Promise<CustomerComplaintItem> {
  return request<CustomerComplaintItem>({
    url: `${CUSTOMER_COMPLAINT_ITEM_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 创建客诉明细
 * @param {CustomerComplaintItemCreate} dto 创建DTO
 * @returns {Promise<CustomerComplaintItem>} 客诉明细DTO
 */
export function createCustomerComplaintItem(dto: CustomerComplaintItemCreate): Promise<CustomerComplaintItem> {
  return request<CustomerComplaintItem>({
    url: `${CUSTOMER_COMPLAINT_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新客诉明细
 * @param {string} id 客诉明细ID
 * @param {CustomerComplaintItemUpdate} dto 更新DTO
 * @returns {Promise<CustomerComplaintItem>} 客诉明细DTO
 */
export function updateCustomerComplaintItem(id: string, dto: CustomerComplaintItemUpdate): Promise<CustomerComplaintItem> {
  return request<CustomerComplaintItem>({
    url: `${CUSTOMER_COMPLAINT_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除客诉明细
 * @param {string} id 客诉明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteCustomerComplaintItemById(id: string): Promise<void> {
  return request({
    url: `${CUSTOMER_COMPLAINT_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除客诉明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteCustomerComplaintItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${CUSTOMER_COMPLAINT_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新客诉明细状态
 * @param {CustomerComplaintItemStatus} dto 状态 DTO
 * @returns {Promise<CustomerComplaintItem>} 客诉明细DTO
 */
export function updateCustomerComplaintItemStatus(dto: CustomerComplaintItemStatus): Promise<CustomerComplaintItem> {
  return request<CustomerComplaintItem>({
    url: `${CUSTOMER_COMPLAINT_ITEM_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新客诉明细作废状态
 * @param {CustomerComplaintItemObsolete} dto 作废 DTO
 * @returns {Promise<CustomerComplaintItem>} 客诉明细DTO
 */
export function updateCustomerComplaintItemObsolete(dto: CustomerComplaintItemObsolete): Promise<CustomerComplaintItem> {
  return request<CustomerComplaintItem>({
    url: `${CUSTOMER_COMPLAINT_ITEM_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取客诉明细选项列表
 * @param {string} plantCode 工厂代码（可选，用于按工厂过滤）
 * @param {string} keyword 搜索关键字（可选，模糊匹配）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getCustomerComplaintItemOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${CUSTOMER_COMPLAINT_ITEM_API_BASE}/options`,
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
export function getCustomerComplaintItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${CUSTOMER_COMPLAINT_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入客诉明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importCustomerComplaintItem(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${CUSTOMER_COMPLAINT_ITEM_API_BASE}/import`,
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
 * 导出客诉明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportCustomerComplaintItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${CUSTOMER_COMPLAINT_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

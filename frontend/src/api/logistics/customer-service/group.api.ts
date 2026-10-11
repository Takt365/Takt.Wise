// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/customer-service
// 文件名称：group.api.ts
// 创建时间：2026-10-08
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/customer-service 模块 API（自动生成，请勿手改路由常量）
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
  CustomerServiceGroup,
  CustomerServiceGroupCreate,
  CustomerServiceGroupSort,
  CustomerServiceGroupStatus,
  CustomerServiceGroupUpdate
} from '@/types/logistics/customer-service/group.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktCustomerServiceGroups
 */
const CUSTOMER_SERVICE_GROUP_API_BASE = 'TaktCustomerServiceGroups';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取客户服务组主数据列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<CustomerServiceGroup>>} 分页结果
 */
export function getCustomerServiceGroupList(queryDto: any): Promise<TaktPagedResult<CustomerServiceGroup>> {
  return request<TaktPagedResult<CustomerServiceGroup>>({
    url: `${CUSTOMER_SERVICE_GROUP_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取客户服务组主数据
 * @param {string} id 客户服务组主数据ID
 * @returns {Promise<CustomerServiceGroup>} 客户服务组主数据DTO
 */
export function getCustomerServiceGroupById(id: string): Promise<CustomerServiceGroup> {
  return request<CustomerServiceGroup>({
    url: `${CUSTOMER_SERVICE_GROUP_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 创建客户服务组主数据
 * @param {CustomerServiceGroupCreate} dto 创建DTO
 * @returns {Promise<CustomerServiceGroup>} 客户服务组主数据DTO
 */
export function createCustomerServiceGroup(dto: CustomerServiceGroupCreate): Promise<CustomerServiceGroup> {
  return request<CustomerServiceGroup>({
    url: `${CUSTOMER_SERVICE_GROUP_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新客户服务组主数据
 * @param {string} id 客户服务组主数据ID
 * @param {CustomerServiceGroupUpdate} dto 更新DTO
 * @returns {Promise<CustomerServiceGroup>} 客户服务组主数据DTO
 */
export function updateCustomerServiceGroup(id: string, dto: CustomerServiceGroupUpdate): Promise<CustomerServiceGroup> {
  return request<CustomerServiceGroup>({
    url: `${CUSTOMER_SERVICE_GROUP_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除客户服务组主数据
 * @param {string} id 客户服务组主数据ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteCustomerServiceGroupById(id: string): Promise<void> {
  return request({
    url: `${CUSTOMER_SERVICE_GROUP_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除客户服务组主数据
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteCustomerServiceGroupBatch(ids: string[]): Promise<void> {
  return request({
    url: `${CUSTOMER_SERVICE_GROUP_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新客户服务组主数据状态
 * @param {CustomerServiceGroupStatus} dto 状态 DTO
 * @returns {Promise<CustomerServiceGroup>} 客户服务组主数据DTO
 */
export function updateCustomerServiceGroupStatus(dto: CustomerServiceGroupStatus): Promise<CustomerServiceGroup> {
  return request<CustomerServiceGroup>({
    url: `${CUSTOMER_SERVICE_GROUP_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新客户服务组主数据排序
 * @param {CustomerServiceGroupSort} dto 排序DTO
 * @returns {Promise<CustomerServiceGroup>} 客户服务组主数据DTO
 */
export function updateCustomerServiceGroupSort(dto: CustomerServiceGroupSort): Promise<CustomerServiceGroup> {
  return request<CustomerServiceGroup>({
    url: `${CUSTOMER_SERVICE_GROUP_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取客户服务组主数据选项列表
 * @param {string} plantCode 工厂代码（可选，用于按工厂过滤）
 * @param {string} keyword 搜索关键字（可选，模糊匹配）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getCustomerServiceGroupOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${CUSTOMER_SERVICE_GROUP_API_BASE}/options`,
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
export function getCustomerServiceGroupTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${CUSTOMER_SERVICE_GROUP_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入客户服务组主数据
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importCustomerServiceGroup(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${CUSTOMER_SERVICE_GROUP_API_BASE}/import`,
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
 * 导出客户服务组主数据
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportCustomerServiceGroup(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${CUSTOMER_SERVICE_GROUP_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

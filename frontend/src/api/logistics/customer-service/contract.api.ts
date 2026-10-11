// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/customer-service
// 文件名称：contract.api.ts
// 创建时间：2026-08-11
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
  CustomerServiceContract,
  CustomerServiceContractCreate,
  CustomerServiceContractSort,
  CustomerServiceContractStatus,
  CustomerServiceContractUpdate
} from '@/types/logistics/customer-service/contract.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktCustomerServiceContracts
 */
const CUSTOMER_SERVICE_CONTRACT_API_BASE = 'TaktCustomerServiceContracts';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取服务合同列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<CustomerServiceContract>>} 分页结果
 */
export function getCustomerServiceContractList(queryDto: any): Promise<TaktPagedResult<CustomerServiceContract>> {
  return request<TaktPagedResult<CustomerServiceContract>>({
    url: `${CUSTOMER_SERVICE_CONTRACT_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取服务合同
 * @param {string} id 服务合同ID
 * @returns {Promise<CustomerServiceContract>} 服务合同DTO
 */
export function getCustomerServiceContractById(id: string): Promise<CustomerServiceContract> {
  return request<CustomerServiceContract>({
    url: `${CUSTOMER_SERVICE_CONTRACT_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建服务合同
 * @param {CustomerServiceContractCreate} dto 创建DTO
 * @returns {Promise<CustomerServiceContract>} 服务合同DTO
 */
export function createCustomerServiceContract(dto: CustomerServiceContractCreate): Promise<CustomerServiceContract> {
  return request<CustomerServiceContract>({
    url: `${CUSTOMER_SERVICE_CONTRACT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新服务合同
 * @param {string} id 服务合同ID
 * @param {CustomerServiceContractUpdate} dto 更新DTO
 * @returns {Promise<CustomerServiceContract>} 服务合同DTO
 */
export function updateCustomerServiceContract(id: string, dto: CustomerServiceContractUpdate): Promise<CustomerServiceContract> {
  return request<CustomerServiceContract>({
    url: `${CUSTOMER_SERVICE_CONTRACT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除服务合同
 * @param {string} id 服务合同ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteCustomerServiceContractById(id: string): Promise<void> {
  return request({
    url: `${CUSTOMER_SERVICE_CONTRACT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除服务合同
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteCustomerServiceContractBatch(ids: string[]): Promise<void> {
  return request({
    url: `${CUSTOMER_SERVICE_CONTRACT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新服务合同状态
 * @param {CustomerServiceContractStatus} dto 状态 DTO
 * @returns {Promise<CustomerServiceContract>} 服务合同DTO
 */
export function updateCustomerServiceContractStatus(dto: CustomerServiceContractStatus): Promise<CustomerServiceContract> {
  return request<CustomerServiceContract>({
    url: `${CUSTOMER_SERVICE_CONTRACT_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新服务合同排序
 * @param {CustomerServiceContractSort} dto 排序DTO
 * @returns {Promise<CustomerServiceContract>} 服务合同DTO
 */
export function updateCustomerServiceContractSort(dto: CustomerServiceContractSort): Promise<CustomerServiceContract> {
  return request<CustomerServiceContract>({
    url: `${CUSTOMER_SERVICE_CONTRACT_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取服务合同选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getCustomerServiceContractOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${CUSTOMER_SERVICE_CONTRACT_API_BASE}/options`,
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
export function getCustomerServiceContractTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${CUSTOMER_SERVICE_CONTRACT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入服务合同
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importCustomerServiceContract(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${CUSTOMER_SERVICE_CONTRACT_API_BASE}/import`,
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
 * 导出服务合同
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportCustomerServiceContract(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${CUSTOMER_SERVICE_CONTRACT_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

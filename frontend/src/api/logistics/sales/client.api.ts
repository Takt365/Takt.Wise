// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/sales
// 文件名称：client.api.ts
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
  Client,
  ClientCreate,
  ClientSort,
  ClientStatus,
  ClientUpdate
} from '@/types/logistics/sales/client.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktClients
 */
const CLIENT_API_BASE = 'TaktClients';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取客户端信息列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Client>>} 分页结果
 */
export function getClientList(queryDto: any): Promise<TaktPagedResult<Client>> {
  return request<TaktPagedResult<Client>>({
    url: `${CLIENT_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取客户端信息
 * @param {string} id 客户端信息ID
 * @returns {Promise<Client>} 客户端信息DTO
 */
export function getClientById(id: string): Promise<Client> {
  return request<Client>({
    url: `${CLIENT_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 创建客户端信息
 * @param {ClientCreate} dto 创建DTO
 * @returns {Promise<Client>} 客户端信息DTO
 */
export function createClient(dto: ClientCreate): Promise<Client> {
  return request<Client>({
    url: `${CLIENT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新客户端信息
 * @param {string} id 客户端信息ID
 * @param {ClientUpdate} dto 更新DTO
 * @returns {Promise<Client>} 客户端信息DTO
 */
export function updateClient(id: string, dto: ClientUpdate): Promise<Client> {
  return request<Client>({
    url: `${CLIENT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除客户端信息
 * @param {string} id 客户端信息ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteClientById(id: string): Promise<void> {
  return request({
    url: `${CLIENT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除客户端信息
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteClientBatch(ids: string[]): Promise<void> {
  return request({
    url: `${CLIENT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新客户端信息状态
 * @param {ClientStatus} dto 状态 DTO
 * @returns {Promise<Client>} 客户端信息DTO
 */
export function updateClientStatus(dto: ClientStatus): Promise<Client> {
  return request<Client>({
    url: `${CLIENT_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新客户端信息排序
 * @param {ClientSort} dto 排序DTO
 * @returns {Promise<Client>} 客户端信息DTO
 */
export function updateClientSort(dto: ClientSort): Promise<Client> {
  return request<Client>({
    url: `${CLIENT_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取客户端信息选项列表
 * @param {string} plantCode 工厂代码（可选，用于按工厂过滤）
 * @param {string} keyword 搜索关键字（可选，模糊匹配）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getClientOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${CLIENT_API_BASE}/options`,
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
export function getClientTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${CLIENT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入客户端信息
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importClient(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${CLIENT_API_BASE}/import`,
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
 * 导出客户端信息
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportClient(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${CLIENT_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

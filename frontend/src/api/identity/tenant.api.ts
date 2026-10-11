// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/identity
// 文件名称：tenant.api.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：identity 模块 API（自动生成，请勿手改路由常量）
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
  Tenant,
  TenantCreate,
  TenantStatus,
  TenantUpdate
} from '@/types/identity/tenant.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktTenants
 */
const TENANT_API_BASE = 'TaktTenants';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取租户列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Tenant>>} 分页结果
 */
export function getTenantList(queryDto: any): Promise<TaktPagedResult<Tenant>> {
  return request<TaktPagedResult<Tenant>>({
    url: `${TENANT_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取租户
 * @param {string} id 租户ID
 * @returns {Promise<Tenant>} 租户DTO
 */
export function getTenantById(id: string): Promise<Tenant> {
  return request<Tenant>({
    url: `${TENANT_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建租户
 * @param {TenantCreate} dto 创建DTO
 * @returns {Promise<Tenant>} 租户DTO
 */
export function createTenant(dto: TenantCreate): Promise<Tenant> {
  return request<Tenant>({
    url: `${TENANT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新租户
 * @param {string} id 租户ID
 * @param {TenantUpdate} dto 更新DTO
 * @returns {Promise<Tenant>} 租户DTO
 */
export function updateTenant(id: string, dto: TenantUpdate): Promise<Tenant> {
  return request<Tenant>({
    url: `${TENANT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除租户
 * @param {string} id 租户ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteTenantById(id: string): Promise<void> {
  return request({
    url: `${TENANT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除租户
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteTenantBatch(ids: string[]): Promise<void> {
  return request({
    url: `${TENANT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新租户状态
 * @param {TenantStatus} dto 状态 DTO（TaktCommonStatus 枚举）
 * @returns {Promise<Tenant>} 租户DTO
 */
export function updateTenantStatus(dto: TenantStatus): Promise<Tenant> {
  return request<Tenant>({
    url: `${TENANT_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取当前登录会话的租户选项（仅一项，DictValue 为 TenantCode；登录后不可跨租户切换）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getTenantOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${TENANT_API_BASE}/options`,
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
export function getTenantTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${TENANT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入租户
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importTenant(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${TENANT_API_BASE}/import`,
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
 * 导出租户
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportTenant(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${TENANT_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

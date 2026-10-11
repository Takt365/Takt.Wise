// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/identity
// 文件名称：role.api.ts
// 创建时间：2026-06-24
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
  Role,
  RoleCreate,
  RoleSort,
  RoleStatus,
  RoleUpdate
} from '@/types/identity/role.types';
import type {
  RoleBuiltIn
} from '@/types/identity/role.types-built-in';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktRoles
 */
const ROLE_API_BASE = 'TaktRoles';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取角色列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Role>>} 分页结果
 */
export function getRoleList(queryDto: any): Promise<TaktPagedResult<Role>> {
  return request<TaktPagedResult<Role>>({
    url: `${ROLE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取角色
 * @param {string} id 角色ID
 * @returns {Promise<Role>} 角色DTO
 */
export function getRoleById(id: string): Promise<Role> {
  return request<Role>({
    url: `${ROLE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建角色
 * @param {RoleCreate} dto 创建DTO
 * @returns {Promise<Role>} 角色DTO
 */
export function createRole(dto: RoleCreate): Promise<Role> {
  return request<Role>({
    url: `${ROLE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新角色
 * @param {string} id 角色ID
 * @param {RoleUpdate} dto 更新DTO
 * @returns {Promise<Role>} 角色DTO
 */
export function updateRole(id: string, dto: RoleUpdate): Promise<Role> {
  return request<Role>({
    url: `${ROLE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除角色
 * @param {string} id 角色ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteRoleById(id: string): Promise<void> {
  return request({
    url: `${ROLE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除角色
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteRoleBatch(ids: string[]): Promise<void> {
  return request({
    url: `${ROLE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新角色状态
 * @param {RoleStatus} dto 状态 DTO（TaktCommonStatus 枚举）
 * @returns {Promise<Role>} 角色DTO
 */
export function updateRoleStatus(dto: RoleStatus): Promise<Role> {
  return request<Role>({
    url: `${ROLE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新角色内置
 * @param {RoleBuiltIn} dto 内置 DTO
 * @returns {Promise<Role>} 角色DTO
 */
export function updateRoleBuiltIn(dto: RoleBuiltIn): Promise<Role> {
  return request<Role>({
    url: `${ROLE_API_BASE}/built-in`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新角色排序
 * @param {RoleSort} dto 排序DTO
 * @returns {Promise<Role>} 角色DTO
 */
export function updateRoleSort(dto: RoleSort): Promise<Role> {
  return request<Role>({
    url: `${ROLE_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取角色选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getRoleOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${ROLE_API_BASE}/options`,
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
export function getRoleTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${ROLE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入角色
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importRole(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${ROLE_API_BASE}/import`,
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
 * 导出角色
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportRole(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${ROLE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/foundation
// 文件名称：adminDivision.api.ts
// 创建时间：2026-08-21
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块 API（自动生成，请勿手改路由常量）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  TaktPagedResult,
  TaktTreeSelectOption
} from '@/types/common.types';
import type {
  AdminDivision,
  AdminDivisionCreate,
  AdminDivisionSort,
  AdminDivisionStatus,
  AdminDivisionTree,
  AdminDivisionUpdate
} from '@/types/foundation/adminDivision.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktAdminDivisions
 */
const ADMIN_DIVISION_API_BASE = 'TaktAdminDivisions';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取行政区划列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<AdminDivision>>} 分页结果
 */
export function getAdminDivisionList(queryDto: any): Promise<TaktPagedResult<AdminDivision>> {
  return request<TaktPagedResult<AdminDivision>>({
    url: `${ADMIN_DIVISION_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取行政区划
 * @param {string} id 行政区划ID
 * @returns {Promise<AdminDivision>} 行政区划DTO
 */
export function getAdminDivisionById(id: string): Promise<AdminDivision> {
  return request<AdminDivision>({
    url: `${ADMIN_DIVISION_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 获取行政区划树形列表（懒加载：仅 parentId 直接子级一层）
 * @param {string} parentId 父级ID（0=根；懒加载仅返回直接子级一层）
 * @param {boolean} includeDisabled 为 false 时过滤禁用项（按实体 *Status 字段）
 * @returns {Promise<AdminDivisionTree[]>} 树形数据
 */
export function getAdminDivisionTree(parentId: string, includeDisabled: boolean): Promise<AdminDivisionTree[]> {
  return request<AdminDivisionTree[]>({
    url: `${ADMIN_DIVISION_API_BASE}/tree`,
    method: 'get',
    params: {
      parentId,
      includeDisabled
    },
  });
}

/**
 * 创建行政区划
 * @param {AdminDivisionCreate} dto 创建DTO
 * @returns {Promise<AdminDivision>} 行政区划DTO
 */
export function createAdminDivision(dto: AdminDivisionCreate): Promise<AdminDivision> {
  return request<AdminDivision>({
    url: `${ADMIN_DIVISION_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新行政区划
 * @param {string} id 行政区划ID
 * @param {AdminDivisionUpdate} dto 更新DTO
 * @returns {Promise<AdminDivision>} 行政区划DTO
 */
export function updateAdminDivision(id: string, dto: AdminDivisionUpdate): Promise<AdminDivision> {
  return request<AdminDivision>({
    url: `${ADMIN_DIVISION_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除行政区划
 * @param {string} id 行政区划ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteAdminDivisionById(id: string): Promise<void> {
  return request({
    url: `${ADMIN_DIVISION_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除行政区划
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteAdminDivisionBatch(ids: string[]): Promise<void> {
  return request({
    url: `${ADMIN_DIVISION_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新行政区划状态
 * @param {AdminDivisionStatus} dto 状态 DTO
 * @returns {Promise<AdminDivision>} 行政区划DTO
 */
export function updateAdminDivisionStatus(dto: AdminDivisionStatus): Promise<AdminDivision> {
  return request<AdminDivision>({
    url: `${ADMIN_DIVISION_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新行政区划排序
 * @param {AdminDivisionSort} dto 排序DTO
 * @returns {Promise<AdminDivision>} 行政区划DTO
 */
export function updateAdminDivisionSort(dto: AdminDivisionSort): Promise<AdminDivision> {
  return request<AdminDivision>({
    url: `${ADMIN_DIVISION_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取行政区划树形选项（懒加载：仅 parentId 直接子级一层；DictValue=Id 字符串，供表单 parentId）
 * @param {string} parentId 父级ID（0=根；懒加载仅返回直接子级一层）
 * @returns {Promise<TaktTreeSelectOption[]>} 树形选项
 */
export function getAdminDivisionTreeOptions(parentId: string): Promise<TaktTreeSelectOption[]> {
  return request<TaktTreeSelectOption[]>({
    url: `${ADMIN_DIVISION_API_BASE}/tree-options`,
    method: 'get',
    params: {
      parentId
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
export function getAdminDivisionTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${ADMIN_DIVISION_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入行政区划
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importAdminDivision(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${ADMIN_DIVISION_API_BASE}/import`,
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
 * 导出行政区划
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportAdminDivision(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${ADMIN_DIVISION_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

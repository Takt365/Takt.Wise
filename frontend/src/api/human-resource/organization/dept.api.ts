// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/organization
// 文件名称：dept.api.ts
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/organization 模块 API（自动生成，请勿手改路由常量）
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
  Dept,
  DeptCreate,
  DeptSort,
  DeptStatus,
  DeptTree,
  DeptUpdate
} from '@/types/human-resource/organization/dept.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktDepts
 */
const DEPT_API_BASE = 'TaktDepts';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取部门列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Dept>>} 分页结果
 */
export function getDeptList(queryDto: any): Promise<TaktPagedResult<Dept>> {
  return request<TaktPagedResult<Dept>>({
    url: `${DEPT_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取部门
 * @param {string} id 部门ID
 * @returns {Promise<Dept>} 部门DTO
 */
export function getDeptById(id: string): Promise<Dept> {
  return request<Dept>({
    url: `${DEPT_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 获取部门树形列表（懒加载：仅 parentId 直接子级一层）
 * @param {string} parentId 父级ID（0=根；懒加载仅返回直接子级一层）
 * @param {boolean} includeDisabled 为 false 时过滤禁用项（按实体 *Status 字段）
 * @returns {Promise<DeptTree[]>} 树形数据
 */
export function getDeptTree(parentId: string, includeDisabled: boolean): Promise<DeptTree[]> {
  return request<DeptTree[]>({
    url: `${DEPT_API_BASE}/tree`,
    method: 'get',
    params: {
      parentId,
      includeDisabled
    },
  });
}

/**
 * 创建部门
 * @param {DeptCreate} dto 创建DTO
 * @returns {Promise<Dept>} 部门DTO
 */
export function createDept(dto: DeptCreate): Promise<Dept> {
  return request<Dept>({
    url: `${DEPT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新部门
 * @param {string} id 部门ID
 * @param {DeptUpdate} dto 更新DTO
 * @returns {Promise<Dept>} 部门DTO
 */
export function updateDept(id: string, dto: DeptUpdate): Promise<Dept> {
  return request<Dept>({
    url: `${DEPT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除部门
 * @param {string} id 部门ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteDeptById(id: string): Promise<void> {
  return request({
    url: `${DEPT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除部门
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteDeptBatch(ids: string[]): Promise<void> {
  return request({
    url: `${DEPT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新部门状态
 * @param {DeptStatus} dto 状态 DTO
 * @returns {Promise<Dept>} 部门DTO
 */
export function updateDeptStatus(dto: DeptStatus): Promise<Dept> {
  return request<Dept>({
    url: `${DEPT_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新部门排序
 * @param {DeptSort} dto 排序DTO
 * @returns {Promise<Dept>} 部门DTO
 */
export function updateDeptSort(dto: DeptSort): Promise<Dept> {
  return request<Dept>({
    url: `${DEPT_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取部门树形选项列表（懒加载：仅 parentId 直接子级一层）
 * @param {string} parentId 父级ID（0=根；懒加载仅返回直接子级一层）
 * @returns {Promise<TaktTreeSelectOption[]>} 树形选项
 */
export function getDeptTreeOptions(parentId: string): Promise<TaktTreeSelectOption[]> {
  return request<TaktTreeSelectOption[]>({
    url: `${DEPT_API_BASE}/tree-options`,
    method: 'get',
    params: {
      parentId
    },
  });
}

/**
 * 获取部门 ISO 编码树形选项列表（懒加载：DictValue=IsoCode）
 * @param {string} parentId 父级ID（0=根；懒加载仅返回直接子级一层）
 * @returns {Promise<TaktTreeSelectOption[]>} 树形选项
 */
export function getDeptIsoTreeOptions(parentId: string): Promise<TaktTreeSelectOption[]> {
  return request<TaktTreeSelectOption[]>({
    url: `${DEPT_API_BASE}/iso-tree-options`,
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
export function getDeptTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${DEPT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入部门
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importDept(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${DEPT_API_BASE}/import`,
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
 * 导出部门
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportDept(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${DEPT_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

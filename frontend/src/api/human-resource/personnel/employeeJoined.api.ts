// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/personnel
// 文件名称：employeeJoined.api.ts
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/personnel 模块 API（自动生成，请勿手改路由常量）
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
  EmployeeJoined,
  EmployeeJoinedCreate,
  EmployeeJoinedUpdate
} from '@/types/human-resource/personnel/employeeJoined.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktEmployeeJoineds
 */
const EMPLOYEE_JOINED_API_BASE = 'TaktEmployeeJoineds';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取员工入职上岗列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EmployeeJoined>>} 分页结果
 */
export function getEmployeeJoinedList(queryDto: any): Promise<TaktPagedResult<EmployeeJoined>> {
  return request<TaktPagedResult<EmployeeJoined>>({
    url: `${EMPLOYEE_JOINED_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取员工入职上岗
 * @param {string} id 员工入职上岗ID
 * @returns {Promise<EmployeeJoined>} 员工入职上岗DTO
 */
export function getEmployeeJoinedById(id: string): Promise<EmployeeJoined> {
  return request<EmployeeJoined>({
    url: `${EMPLOYEE_JOINED_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建员工入职上岗
 * @param {EmployeeJoinedCreate} dto 创建DTO
 * @returns {Promise<EmployeeJoined>} 员工入职上岗DTO
 */
export function createEmployeeJoined(dto: EmployeeJoinedCreate): Promise<EmployeeJoined> {
  return request<EmployeeJoined>({
    url: `${EMPLOYEE_JOINED_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新员工入职上岗
 * @param {string} id 员工入职上岗ID
 * @param {EmployeeJoinedUpdate} dto 更新DTO
 * @returns {Promise<EmployeeJoined>} 员工入职上岗DTO
 */
export function updateEmployeeJoined(id: string, dto: EmployeeJoinedUpdate): Promise<EmployeeJoined> {
  return request<EmployeeJoined>({
    url: `${EMPLOYEE_JOINED_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除员工入职上岗
 * @param {string} id 员工入职上岗ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteEmployeeJoinedById(id: string): Promise<void> {
  return request({
    url: `${EMPLOYEE_JOINED_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除员工入职上岗
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteEmployeeJoinedBatch(ids: string[]): Promise<void> {
  return request({
    url: `${EMPLOYEE_JOINED_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取员工入职上岗选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getEmployeeJoinedOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${EMPLOYEE_JOINED_API_BASE}/options`,
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
export function getEmployeeJoinedTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${EMPLOYEE_JOINED_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入员工入职上岗
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importEmployeeJoined(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${EMPLOYEE_JOINED_API_BASE}/import`,
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
 * 导出员工入职上岗
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportEmployeeJoined(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EMPLOYEE_JOINED_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

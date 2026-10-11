// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/personnel
// 文件名称：employeeResignation.api.ts
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
  EmployeeResignation,
  EmployeeResignationCreate,
  EmployeeResignationUpdate
} from '@/types/human-resource/personnel/employeeResignation.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktEmployeeResignations
 */
const EMPLOYEE_RESIGNATION_API_BASE = 'TaktEmployeeResignations';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取员工离职列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EmployeeResignation>>} 分页结果
 */
export function getEmployeeResignationList(queryDto: any): Promise<TaktPagedResult<EmployeeResignation>> {
  return request<TaktPagedResult<EmployeeResignation>>({
    url: `${EMPLOYEE_RESIGNATION_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取员工离职
 * @param {string} id 员工离职ID
 * @returns {Promise<EmployeeResignation>} 员工离职DTO
 */
export function getEmployeeResignationById(id: string): Promise<EmployeeResignation> {
  return request<EmployeeResignation>({
    url: `${EMPLOYEE_RESIGNATION_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建员工离职
 * @param {EmployeeResignationCreate} dto 创建DTO
 * @returns {Promise<EmployeeResignation>} 员工离职DTO
 */
export function createEmployeeResignation(dto: EmployeeResignationCreate): Promise<EmployeeResignation> {
  return request<EmployeeResignation>({
    url: `${EMPLOYEE_RESIGNATION_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新员工离职
 * @param {string} id 员工离职ID
 * @param {EmployeeResignationUpdate} dto 更新DTO
 * @returns {Promise<EmployeeResignation>} 员工离职DTO
 */
export function updateEmployeeResignation(id: string, dto: EmployeeResignationUpdate): Promise<EmployeeResignation> {
  return request<EmployeeResignation>({
    url: `${EMPLOYEE_RESIGNATION_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除员工离职
 * @param {string} id 员工离职ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteEmployeeResignationById(id: string): Promise<void> {
  return request({
    url: `${EMPLOYEE_RESIGNATION_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除员工离职
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteEmployeeResignationBatch(ids: string[]): Promise<void> {
  return request({
    url: `${EMPLOYEE_RESIGNATION_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取员工离职选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getEmployeeResignationOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${EMPLOYEE_RESIGNATION_API_BASE}/options`,
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
export function getEmployeeResignationTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${EMPLOYEE_RESIGNATION_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入员工离职
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importEmployeeResignation(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${EMPLOYEE_RESIGNATION_API_BASE}/import`,
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
 * 导出员工离职
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportEmployeeResignation(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EMPLOYEE_RESIGNATION_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

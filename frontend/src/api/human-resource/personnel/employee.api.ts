// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/personnel
// 文件名称：employee.api.ts
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
  Employee,
  EmployeeCreate,
  EmployeeStatus,
  EmployeeUpdate
} from '@/types/human-resource/personnel/employee.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktEmployees
 */
const EMPLOYEE_API_BASE = 'TaktEmployees';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取员工列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Employee>>} 分页结果
 */
export function getEmployeeList(queryDto: any): Promise<TaktPagedResult<Employee>> {
  return request<TaktPagedResult<Employee>>({
    url: `${EMPLOYEE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取员工
 * @param {string} id 员工ID
 * @returns {Promise<Employee>} 员工DTO
 */
export function getEmployeeById(id: string): Promise<Employee> {
  return request<Employee>({
    url: `${EMPLOYEE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建员工
 * @param {EmployeeCreate} dto 创建DTO
 * @returns {Promise<Employee>} 员工DTO
 */
export function createEmployee(dto: EmployeeCreate): Promise<Employee> {
  return request<Employee>({
    url: `${EMPLOYEE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新员工
 * @param {string} id 员工ID
 * @param {EmployeeUpdate} dto 更新DTO
 * @returns {Promise<Employee>} 员工DTO
 */
export function updateEmployee(id: string, dto: EmployeeUpdate): Promise<Employee> {
  return request<Employee>({
    url: `${EMPLOYEE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除员工
 * @param {string} id 员工ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteEmployeeById(id: string): Promise<void> {
  return request({
    url: `${EMPLOYEE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除员工
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteEmployeeBatch(ids: string[]): Promise<void> {
  return request({
    url: `${EMPLOYEE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新员工状态
 * @param {EmployeeStatus} dto 状态 DTO
 * @returns {Promise<Employee>} 员工DTO
 */
export function updateEmployeeStatus(dto: EmployeeStatus): Promise<Employee> {
  return request<Employee>({
    url: `${EMPLOYEE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取员工选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getEmployeeOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${EMPLOYEE_API_BASE}/options`,
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
export function getEmployeeTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${EMPLOYEE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入员工
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importEmployee(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${EMPLOYEE_API_BASE}/import`,
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
 * 导出员工
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportEmployee(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EMPLOYEE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/personnel
// 文件名称：employeeDelegation.api.ts
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
  EmployeeDelegation,
  EmployeeDelegationCreate,
  EmployeeDelegationUpdate
} from '@/types/human-resource/personnel/employeeDelegation.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktEmployeeDelegations
 */
const EMPLOYEE_DELEGATION_API_BASE = 'TaktEmployeeDelegations';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取员工代理关系列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EmployeeDelegation>>} 分页结果
 */
export function getEmployeeDelegationList(queryDto: any): Promise<TaktPagedResult<EmployeeDelegation>> {
  return request<TaktPagedResult<EmployeeDelegation>>({
    url: `${EMPLOYEE_DELEGATION_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取员工代理关系
 * @param {string} id 员工代理关系ID
 * @returns {Promise<EmployeeDelegation>} 员工代理关系DTO
 */
export function getEmployeeDelegationById(id: string): Promise<EmployeeDelegation> {
  return request<EmployeeDelegation>({
    url: `${EMPLOYEE_DELEGATION_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建员工代理关系
 * @param {EmployeeDelegationCreate} dto 创建DTO
 * @returns {Promise<EmployeeDelegation>} 员工代理关系DTO
 */
export function createEmployeeDelegation(dto: EmployeeDelegationCreate): Promise<EmployeeDelegation> {
  return request<EmployeeDelegation>({
    url: `${EMPLOYEE_DELEGATION_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新员工代理关系
 * @param {string} id 员工代理关系ID
 * @param {EmployeeDelegationUpdate} dto 更新DTO
 * @returns {Promise<EmployeeDelegation>} 员工代理关系DTO
 */
export function updateEmployeeDelegation(id: string, dto: EmployeeDelegationUpdate): Promise<EmployeeDelegation> {
  return request<EmployeeDelegation>({
    url: `${EMPLOYEE_DELEGATION_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除员工代理关系
 * @param {string} id 员工代理关系ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteEmployeeDelegationById(id: string): Promise<void> {
  return request({
    url: `${EMPLOYEE_DELEGATION_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除员工代理关系
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteEmployeeDelegationBatch(ids: string[]): Promise<void> {
  return request({
    url: `${EMPLOYEE_DELEGATION_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取员工代理关系选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getEmployeeDelegationOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${EMPLOYEE_DELEGATION_API_BASE}/options`,
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
export function getEmployeeDelegationTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${EMPLOYEE_DELEGATION_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入员工代理关系
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importEmployeeDelegation(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${EMPLOYEE_DELEGATION_API_BASE}/import`,
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
 * 导出员工代理关系
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportEmployeeDelegation(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EMPLOYEE_DELEGATION_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

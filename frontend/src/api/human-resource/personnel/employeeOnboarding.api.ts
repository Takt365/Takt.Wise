// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/personnel
// 文件名称：employeeOnboarding.api.ts
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
  EmployeeOnboarding,
  EmployeeOnboardingCreate,
  EmployeeOnboardingStatus,
  EmployeeOnboardingUpdate
} from '@/types/human-resource/personnel/employeeOnboarding.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktEmployeeOnboardings
 */
const EMPLOYEE_ONBOARDING_API_BASE = 'TaktEmployeeOnboardings';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取入职待办列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EmployeeOnboarding>>} 分页结果
 */
export function getEmployeeOnboardingList(queryDto: any): Promise<TaktPagedResult<EmployeeOnboarding>> {
  return request<TaktPagedResult<EmployeeOnboarding>>({
    url: `${EMPLOYEE_ONBOARDING_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取入职待办
 * @param {string} id 入职待办ID
 * @returns {Promise<EmployeeOnboarding>} 入职待办DTO
 */
export function getEmployeeOnboardingById(id: string): Promise<EmployeeOnboarding> {
  return request<EmployeeOnboarding>({
    url: `${EMPLOYEE_ONBOARDING_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建入职待办
 * @param {EmployeeOnboardingCreate} dto 创建DTO
 * @returns {Promise<EmployeeOnboarding>} 入职待办DTO
 */
export function createEmployeeOnboarding(dto: EmployeeOnboardingCreate): Promise<EmployeeOnboarding> {
  return request<EmployeeOnboarding>({
    url: `${EMPLOYEE_ONBOARDING_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新入职待办
 * @param {string} id 入职待办ID
 * @param {EmployeeOnboardingUpdate} dto 更新DTO
 * @returns {Promise<EmployeeOnboarding>} 入职待办DTO
 */
export function updateEmployeeOnboarding(id: string, dto: EmployeeOnboardingUpdate): Promise<EmployeeOnboarding> {
  return request<EmployeeOnboarding>({
    url: `${EMPLOYEE_ONBOARDING_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除入职待办
 * @param {string} id 入职待办ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteEmployeeOnboardingById(id: string): Promise<void> {
  return request({
    url: `${EMPLOYEE_ONBOARDING_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除入职待办
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteEmployeeOnboardingBatch(ids: string[]): Promise<void> {
  return request({
    url: `${EMPLOYEE_ONBOARDING_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新入职待办状态
 * @param {EmployeeOnboardingStatus} dto 状态 DTO
 * @returns {Promise<EmployeeOnboarding>} 入职待办DTO
 */
export function updateEmployeeOnboardingStatus(dto: EmployeeOnboardingStatus): Promise<EmployeeOnboarding> {
  return request<EmployeeOnboarding>({
    url: `${EMPLOYEE_ONBOARDING_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取入职待办选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getEmployeeOnboardingOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${EMPLOYEE_ONBOARDING_API_BASE}/options`,
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
export function getEmployeeOnboardingTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${EMPLOYEE_ONBOARDING_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入入职待办
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importEmployeeOnboarding(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${EMPLOYEE_ONBOARDING_API_BASE}/import`,
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
 * 导出入职待办
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportEmployeeOnboarding(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EMPLOYEE_ONBOARDING_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

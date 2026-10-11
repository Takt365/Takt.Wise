// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/compensation
// 文件名称：empSalary.api.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/compensation 模块 API（自动生成，请勿手改路由常量）
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
  EmpSalary,
  EmpSalaryCreate,
  EmpSalaryStatus,
  EmpSalaryUpdate
} from '@/types/human-resource/compensation/empSalary.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktEmpSalaries
 */
const EMP_SALARY_API_BASE = 'TaktEmpSalaries';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取员工薪酬列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EmpSalary>>} 分页结果
 */
export function getEmpSalaryList(queryDto: any): Promise<TaktPagedResult<EmpSalary>> {
  return request<TaktPagedResult<EmpSalary>>({
    url: `${EMP_SALARY_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取员工薪酬
 * @param {string} id 员工薪酬ID
 * @returns {Promise<EmpSalary>} 员工薪酬DTO
 */
export function getEmpSalaryById(id: string): Promise<EmpSalary> {
  return request<EmpSalary>({
    url: `${EMP_SALARY_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建员工薪酬
 * @param {EmpSalaryCreate} dto 创建DTO
 * @returns {Promise<EmpSalary>} 员工薪酬DTO
 */
export function createEmpSalary(dto: EmpSalaryCreate): Promise<EmpSalary> {
  return request<EmpSalary>({
    url: `${EMP_SALARY_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新员工薪酬
 * @param {string} id 员工薪酬ID
 * @param {EmpSalaryUpdate} dto 更新DTO
 * @returns {Promise<EmpSalary>} 员工薪酬DTO
 */
export function updateEmpSalary(id: string, dto: EmpSalaryUpdate): Promise<EmpSalary> {
  return request<EmpSalary>({
    url: `${EMP_SALARY_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除员工薪酬
 * @param {string} id 员工薪酬ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteEmpSalaryById(id: string): Promise<void> {
  return request({
    url: `${EMP_SALARY_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除员工薪酬
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteEmpSalaryBatch(ids: string[]): Promise<void> {
  return request({
    url: `${EMP_SALARY_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新员工薪酬状态
 * @param {EmpSalaryStatus} dto 状态 DTO
 * @returns {Promise<EmpSalary>} 员工薪酬DTO
 */
export function updateEmpSalaryStatus(dto: EmpSalaryStatus): Promise<EmpSalary> {
  return request<EmpSalary>({
    url: `${EMP_SALARY_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取员工薪酬选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getEmpSalaryOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${EMP_SALARY_API_BASE}/options`,
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
export function getEmpSalaryTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${EMP_SALARY_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入员工薪酬
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importEmpSalary(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${EMP_SALARY_API_BASE}/import`,
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
 * 导出员工薪酬
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportEmpSalary(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EMP_SALARY_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

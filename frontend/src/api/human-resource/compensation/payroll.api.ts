// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/compensation
// 文件名称：payroll.api.ts
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
  Payroll,
  PayrollCreate,
  PayrollStatus,
  PayrollUpdate
} from '@/types/human-resource/compensation/payroll.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPayrolls
 */
const PAYROLL_API_BASE = 'TaktPayrolls';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取薪酬体系列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Payroll>>} 分页结果
 */
export function getPayrollList(queryDto: any): Promise<TaktPagedResult<Payroll>> {
  return request<TaktPagedResult<Payroll>>({
    url: `${PAYROLL_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取薪酬体系
 * @param {string} id 薪酬体系ID
 * @returns {Promise<Payroll>} 薪酬体系DTO
 */
export function getPayrollById(id: string): Promise<Payroll> {
  return request<Payroll>({
    url: `${PAYROLL_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建薪酬体系
 * @param {PayrollCreate} dto 创建DTO
 * @returns {Promise<Payroll>} 薪酬体系DTO
 */
export function createPayroll(dto: PayrollCreate): Promise<Payroll> {
  return request<Payroll>({
    url: `${PAYROLL_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新薪酬体系
 * @param {string} id 薪酬体系ID
 * @param {PayrollUpdate} dto 更新DTO
 * @returns {Promise<Payroll>} 薪酬体系DTO
 */
export function updatePayroll(id: string, dto: PayrollUpdate): Promise<Payroll> {
  return request<Payroll>({
    url: `${PAYROLL_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除薪酬体系
 * @param {string} id 薪酬体系ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePayrollById(id: string): Promise<void> {
  return request({
    url: `${PAYROLL_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除薪酬体系
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePayrollBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PAYROLL_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新薪酬体系状态
 * @param {PayrollStatus} dto 状态 DTO
 * @returns {Promise<Payroll>} 薪酬体系DTO
 */
export function updatePayrollStatus(dto: PayrollStatus): Promise<Payroll> {
  return request<Payroll>({
    url: `${PAYROLL_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取薪酬体系选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPayrollOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${PAYROLL_API_BASE}/options`,
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
export function getPayrollTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PAYROLL_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入薪酬体系
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPayroll(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PAYROLL_API_BASE}/import`,
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
 * 导出薪酬体系
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPayroll(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PAYROLL_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

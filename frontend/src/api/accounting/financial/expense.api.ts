// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/accounting/financial
// 文件名称：expense.api.ts
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/financial 模块 API（自动生成，请勿手改路由常量）
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
  Expense,
  ExpenseCreate,
  ExpenseStatus,
  ExpenseUpdate
} from '@/types/accounting/financial/expense.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktExpenses
 */
const EXPENSE_API_BASE = 'TaktExpenses';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取费用单列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Expense>>} 分页结果
 */
export function getExpenseList(queryDto: any): Promise<TaktPagedResult<Expense>> {
  return request<TaktPagedResult<Expense>>({
    url: `${EXPENSE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取费用单
 * @param {string} id 费用单ID
 * @returns {Promise<Expense>} 费用单DTO
 */
export function getExpenseById(id: string): Promise<Expense> {
  return request<Expense>({
    url: `${EXPENSE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建费用单
 * @param {ExpenseCreate} dto 创建DTO
 * @returns {Promise<Expense>} 费用单DTO
 */
export function createExpense(dto: ExpenseCreate): Promise<Expense> {
  return request<Expense>({
    url: `${EXPENSE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新费用单
 * @param {string} id 费用单ID
 * @param {ExpenseUpdate} dto 更新DTO
 * @returns {Promise<Expense>} 费用单DTO
 */
export function updateExpense(id: string, dto: ExpenseUpdate): Promise<Expense> {
  return request<Expense>({
    url: `${EXPENSE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除费用单
 * @param {string} id 费用单ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteExpenseById(id: string): Promise<void> {
  return request({
    url: `${EXPENSE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除费用单
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteExpenseBatch(ids: string[]): Promise<void> {
  return request({
    url: `${EXPENSE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新费用单状态
 * @param {ExpenseStatus} dto 状态 DTO
 * @returns {Promise<Expense>} 费用单DTO
 */
export function updateExpenseStatus(dto: ExpenseStatus): Promise<Expense> {
  return request<Expense>({
    url: `${EXPENSE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取费用单选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getExpenseOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${EXPENSE_API_BASE}/options`,
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
export function getExpenseTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${EXPENSE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入费用单
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importExpense(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${EXPENSE_API_BASE}/import`,
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
 * 导出费用单
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportExpense(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EXPENSE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

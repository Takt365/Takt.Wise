// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/accounting/financial
// 文件名称：budgetActual.api.ts
// 创建时间：2026-07-18
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
  BudgetActual,
  BudgetActualCreate,
  BudgetActualSort,
  BudgetActualStatus,
  BudgetActualUpdate
} from '@/types/accounting/financial/budgetActual.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktBudgetActuals
 */
const BUDGET_ACTUAL_API_BASE = 'TaktBudgetActuals';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取预算实绩列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<BudgetActual>>} 分页结果
 */
export function getBudgetActualList(queryDto: any): Promise<TaktPagedResult<BudgetActual>> {
  return request<TaktPagedResult<BudgetActual>>({
    url: `${BUDGET_ACTUAL_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取预算实绩
 * @param {string} id 预算实绩ID
 * @returns {Promise<BudgetActual>} 预算实绩DTO
 */
export function getBudgetActualById(id: string): Promise<BudgetActual> {
  return request<BudgetActual>({
    url: `${BUDGET_ACTUAL_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建预算实绩
 * @param {BudgetActualCreate} dto 创建DTO
 * @returns {Promise<BudgetActual>} 预算实绩DTO
 */
export function createBudgetActual(dto: BudgetActualCreate): Promise<BudgetActual> {
  return request<BudgetActual>({
    url: `${BUDGET_ACTUAL_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新预算实绩
 * @param {string} id 预算实绩ID
 * @param {BudgetActualUpdate} dto 更新DTO
 * @returns {Promise<BudgetActual>} 预算实绩DTO
 */
export function updateBudgetActual(id: string, dto: BudgetActualUpdate): Promise<BudgetActual> {
  return request<BudgetActual>({
    url: `${BUDGET_ACTUAL_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除预算实绩
 * @param {string} id 预算实绩ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteBudgetActualById(id: string): Promise<void> {
  return request({
    url: `${BUDGET_ACTUAL_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除预算实绩
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteBudgetActualBatch(ids: string[]): Promise<void> {
  return request({
    url: `${BUDGET_ACTUAL_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新预算实绩状态
 * @param {BudgetActualStatus} dto 状态 DTO
 * @returns {Promise<BudgetActual>} 预算实绩DTO
 */
export function updateBudgetActualStatus(dto: BudgetActualStatus): Promise<BudgetActual> {
  return request<BudgetActual>({
    url: `${BUDGET_ACTUAL_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新预算实绩排序
 * @param {BudgetActualSort} dto 排序DTO
 * @returns {Promise<BudgetActual>} 预算实绩DTO
 */
export function updateBudgetActualSort(dto: BudgetActualSort): Promise<BudgetActual> {
  return request<BudgetActual>({
    url: `${BUDGET_ACTUAL_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取预算实绩选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getBudgetActualOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${BUDGET_ACTUAL_API_BASE}/options`,
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
export function getBudgetActualTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${BUDGET_ACTUAL_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入预算实绩
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importBudgetActual(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${BUDGET_ACTUAL_API_BASE}/import`,
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
 * 导出预算实绩
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportBudgetActual(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${BUDGET_ACTUAL_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

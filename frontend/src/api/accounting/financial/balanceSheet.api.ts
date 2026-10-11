// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/accounting/financial
// 文件名称：balanceSheet.api.ts
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
  BalanceSheet,
  BalanceSheetCreate,
  BalanceSheetSort,
  BalanceSheetStatus,
  BalanceSheetUpdate
} from '@/types/accounting/financial/balanceSheet.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktBalanceSheets
 */
const BALANCE_SHEET_API_BASE = 'TaktBalanceSheets';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取资产负债列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<BalanceSheet>>} 分页结果
 */
export function getBalanceSheetList(queryDto: any): Promise<TaktPagedResult<BalanceSheet>> {
  return request<TaktPagedResult<BalanceSheet>>({
    url: `${BALANCE_SHEET_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取资产负债
 * @param {string} id 资产负债ID
 * @returns {Promise<BalanceSheet>} 资产负债DTO
 */
export function getBalanceSheetById(id: string): Promise<BalanceSheet> {
  return request<BalanceSheet>({
    url: `${BALANCE_SHEET_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建资产负债
 * @param {BalanceSheetCreate} dto 创建DTO
 * @returns {Promise<BalanceSheet>} 资产负债DTO
 */
export function createBalanceSheet(dto: BalanceSheetCreate): Promise<BalanceSheet> {
  return request<BalanceSheet>({
    url: `${BALANCE_SHEET_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新资产负债
 * @param {string} id 资产负债ID
 * @param {BalanceSheetUpdate} dto 更新DTO
 * @returns {Promise<BalanceSheet>} 资产负债DTO
 */
export function updateBalanceSheet(id: string, dto: BalanceSheetUpdate): Promise<BalanceSheet> {
  return request<BalanceSheet>({
    url: `${BALANCE_SHEET_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除资产负债
 * @param {string} id 资产负债ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteBalanceSheetById(id: string): Promise<void> {
  return request({
    url: `${BALANCE_SHEET_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除资产负债
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteBalanceSheetBatch(ids: string[]): Promise<void> {
  return request({
    url: `${BALANCE_SHEET_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新资产负债状态
 * @param {BalanceSheetStatus} dto 状态 DTO
 * @returns {Promise<BalanceSheet>} 资产负债DTO
 */
export function updateBalanceSheetStatus(dto: BalanceSheetStatus): Promise<BalanceSheet> {
  return request<BalanceSheet>({
    url: `${BALANCE_SHEET_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新资产负债排序
 * @param {BalanceSheetSort} dto 排序DTO
 * @returns {Promise<BalanceSheet>} 资产负债DTO
 */
export function updateBalanceSheetSort(dto: BalanceSheetSort): Promise<BalanceSheet> {
  return request<BalanceSheet>({
    url: `${BALANCE_SHEET_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取资产负债选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getBalanceSheetOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${BALANCE_SHEET_API_BASE}/options`,
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
export function getBalanceSheetTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${BALANCE_SHEET_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入资产负债
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importBalanceSheet(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${BALANCE_SHEET_API_BASE}/import`,
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
 * 导出资产负债
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportBalanceSheet(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${BALANCE_SHEET_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

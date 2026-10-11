// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/accounting/financial
// 文件名称：bank.api.ts
// 创建时间：2026-08-13
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
  Bank,
  BankCreate,
  BankUpdate
} from '@/types/accounting/financial/bank.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktBanks
 */
const BANK_API_BASE = 'TaktBanks';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取银行信息列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Bank>>} 分页结果
 */
export function getBankList(queryDto: any): Promise<TaktPagedResult<Bank>> {
  return request<TaktPagedResult<Bank>>({
    url: `${BANK_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取银行信息
 * @param {string} id 银行信息ID
 * @returns {Promise<Bank>} 银行信息DTO
 */
export function getBankById(id: string): Promise<Bank> {
  return request<Bank>({
    url: `${BANK_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建银行信息
 * @param {BankCreate} dto 创建DTO
 * @returns {Promise<Bank>} 银行信息DTO
 */
export function createBank(dto: BankCreate): Promise<Bank> {
  return request<Bank>({
    url: `${BANK_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新银行信息
 * @param {string} id 银行信息ID
 * @param {BankUpdate} dto 更新DTO
 * @returns {Promise<Bank>} 银行信息DTO
 */
export function updateBank(id: string, dto: BankUpdate): Promise<Bank> {
  return request<Bank>({
    url: `${BANK_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除银行信息
 * @param {string} id 银行信息ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteBankById(id: string): Promise<void> {
  return request({
    url: `${BANK_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除银行信息
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteBankBatch(ids: string[]): Promise<void> {
  return request({
    url: `${BANK_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取银行信息选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getBankOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${BANK_API_BASE}/options`,
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
export function getBankTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${BANK_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入银行信息
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importBank(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${BANK_API_BASE}/import`,
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
 * 导出银行信息
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportBank(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${BANK_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

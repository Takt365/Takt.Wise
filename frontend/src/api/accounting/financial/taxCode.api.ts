// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/accounting/financial
// 文件名称：taxCode.api.ts
// 创建时间：2026-10-14
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
  TaxCode,
  TaxCodeCreate,
  TaxCodeSort,
  TaxCodeStatus,
  TaxCodeUpdate
} from '@/types/accounting/financial/taxCode.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktTaxCodes
 */
const TAX_CODE_API_BASE = 'TaktTaxCodes';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取税收代码列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<TaxCode>>} 分页结果
 */
export function getTaxCodeList(queryDto: any): Promise<TaktPagedResult<TaxCode>> {
  return request<TaktPagedResult<TaxCode>>({
    url: `${TAX_CODE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取税收代码
 * @param {string} id 税收代码ID
 * @returns {Promise<TaxCode>} 税收代码DTO
 */
export function getTaxCodeById(id: string): Promise<TaxCode> {
  return request<TaxCode>({
    url: `${TAX_CODE_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 创建税收代码
 * @param {TaxCodeCreate} dto 创建DTO
 * @returns {Promise<TaxCode>} 税收代码DTO
 */
export function createTaxCode(dto: TaxCodeCreate): Promise<TaxCode> {
  return request<TaxCode>({
    url: `${TAX_CODE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新税收代码
 * @param {string} id 税收代码ID
 * @param {TaxCodeUpdate} dto 更新DTO
 * @returns {Promise<TaxCode>} 税收代码DTO
 */
export function updateTaxCode(id: string, dto: TaxCodeUpdate): Promise<TaxCode> {
  return request<TaxCode>({
    url: `${TAX_CODE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除税收代码
 * @param {string} id 税收代码ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteTaxCodeById(id: string): Promise<void> {
  return request({
    url: `${TAX_CODE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除税收代码
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteTaxCodeBatch(ids: string[]): Promise<void> {
  return request({
    url: `${TAX_CODE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新税收代码状态
 * @param {TaxCodeStatus} dto 状态 DTO
 * @returns {Promise<TaxCode>} 税收代码DTO
 */
export function updateTaxCodeStatus(dto: TaxCodeStatus): Promise<TaxCode> {
  return request<TaxCode>({
    url: `${TAX_CODE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新税收代码排序
 * @param {TaxCodeSort} dto 排序DTO
 * @returns {Promise<TaxCode>} 税收代码DTO
 */
export function updateTaxCodeSort(dto: TaxCodeSort): Promise<TaxCode> {
  return request<TaxCode>({
    url: `${TAX_CODE_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取税收代码选项列表
 * @param {string} plantCode 工厂代码（可选，用于按工厂过滤）
 * @param {string} keyword 搜索关键字（可选，模糊匹配）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getTaxCodeOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${TAX_CODE_API_BASE}/options`,
    method: 'get',
    params: {
      plantCode,
      keyword
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
export function getTaxCodeTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${TAX_CODE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入税收代码
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importTaxCode(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${TAX_CODE_API_BASE}/import`,
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
 * 导出税收代码
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportTaxCode(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${TAX_CODE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

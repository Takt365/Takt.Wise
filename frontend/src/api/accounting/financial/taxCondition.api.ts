// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/accounting/financial
// 文件名称：taxCondition.api.ts
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
  TaxCondition,
  TaxConditionCreate,
  TaxConditionSort,
  TaxConditionStatus,
  TaxConditionUpdate
} from '@/types/accounting/financial/taxCondition.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktTaxConditions
 */
const TAX_CONDITION_API_BASE = 'TaktTaxConditions';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取税务条件列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<TaxCondition>>} 分页结果
 */
export function getTaxConditionList(queryDto: any): Promise<TaktPagedResult<TaxCondition>> {
  return request<TaktPagedResult<TaxCondition>>({
    url: `${TAX_CONDITION_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取税务条件
 * @param {string} id 税务条件ID
 * @returns {Promise<TaxCondition>} 税务条件DTO
 */
export function getTaxConditionById(id: string): Promise<TaxCondition> {
  return request<TaxCondition>({
    url: `${TAX_CONDITION_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 创建税务条件
 * @param {TaxConditionCreate} dto 创建DTO
 * @returns {Promise<TaxCondition>} 税务条件DTO
 */
export function createTaxCondition(dto: TaxConditionCreate): Promise<TaxCondition> {
  return request<TaxCondition>({
    url: `${TAX_CONDITION_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新税务条件
 * @param {string} id 税务条件ID
 * @param {TaxConditionUpdate} dto 更新DTO
 * @returns {Promise<TaxCondition>} 税务条件DTO
 */
export function updateTaxCondition(id: string, dto: TaxConditionUpdate): Promise<TaxCondition> {
  return request<TaxCondition>({
    url: `${TAX_CONDITION_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除税务条件
 * @param {string} id 税务条件ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteTaxConditionById(id: string): Promise<void> {
  return request({
    url: `${TAX_CONDITION_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除税务条件
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteTaxConditionBatch(ids: string[]): Promise<void> {
  return request({
    url: `${TAX_CONDITION_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新税务条件状态
 * @param {TaxConditionStatus} dto 状态 DTO
 * @returns {Promise<TaxCondition>} 税务条件DTO
 */
export function updateTaxConditionStatus(dto: TaxConditionStatus): Promise<TaxCondition> {
  return request<TaxCondition>({
    url: `${TAX_CONDITION_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新税务条件排序
 * @param {TaxConditionSort} dto 排序DTO
 * @returns {Promise<TaxCondition>} 税务条件DTO
 */
export function updateTaxConditionSort(dto: TaxConditionSort): Promise<TaxCondition> {
  return request<TaxCondition>({
    url: `${TAX_CONDITION_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取税务条件选项列表
 * @param {string} plantCode 工厂代码（可选，用于按工厂过滤）
 * @param {string} keyword 搜索关键字（可选，模糊匹配）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getTaxConditionOptions(plantCode?: string, keyword?: string): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${TAX_CONDITION_API_BASE}/options`,
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
export function getTaxConditionTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${TAX_CONDITION_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入税务条件
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importTaxCondition(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${TAX_CONDITION_API_BASE}/import`,
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
 * 导出税务条件
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportTaxCondition(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${TAX_CONDITION_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

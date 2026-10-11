// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/quality/complaint
// 文件名称：supplierEvaluationItem.api.ts
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/complaint 模块 API（自动生成，请勿手改路由常量）
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
  SupplierEvaluationItem,
  SupplierEvaluationItemCreate,
  SupplierEvaluationItemObsolete,
  SupplierEvaluationItemStatus,
  SupplierEvaluationItemUpdate
} from '@/types/logistics/quality/complaint/supplierEvaluationItem.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSupplierEvaluationItems
 */
const SUPPLIER_EVALUATION_ITEM_API_BASE = 'TaktSupplierEvaluationItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取供应商评价考核项目明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SupplierEvaluationItem>>} 分页结果
 */
export function getSupplierEvaluationItemList(queryDto: any): Promise<TaktPagedResult<SupplierEvaluationItem>> {
  return request<TaktPagedResult<SupplierEvaluationItem>>({
    url: `${SUPPLIER_EVALUATION_ITEM_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取供应商评价考核项目明细
 * @param {string} id 供应商评价考核项目明细ID
 * @returns {Promise<SupplierEvaluationItem>} 供应商评价考核项目明细DTO
 */
export function getSupplierEvaluationItemById(id: string): Promise<SupplierEvaluationItem> {
  return request<SupplierEvaluationItem>({
    url: `${SUPPLIER_EVALUATION_ITEM_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建供应商评价考核项目明细
 * @param {SupplierEvaluationItemCreate} dto 创建DTO
 * @returns {Promise<SupplierEvaluationItem>} 供应商评价考核项目明细DTO
 */
export function createSupplierEvaluationItem(dto: SupplierEvaluationItemCreate): Promise<SupplierEvaluationItem> {
  return request<SupplierEvaluationItem>({
    url: `${SUPPLIER_EVALUATION_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新供应商评价考核项目明细
 * @param {string} id 供应商评价考核项目明细ID
 * @param {SupplierEvaluationItemUpdate} dto 更新DTO
 * @returns {Promise<SupplierEvaluationItem>} 供应商评价考核项目明细DTO
 */
export function updateSupplierEvaluationItem(id: string, dto: SupplierEvaluationItemUpdate): Promise<SupplierEvaluationItem> {
  return request<SupplierEvaluationItem>({
    url: `${SUPPLIER_EVALUATION_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除供应商评价考核项目明细
 * @param {string} id 供应商评价考核项目明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSupplierEvaluationItemById(id: string): Promise<void> {
  return request({
    url: `${SUPPLIER_EVALUATION_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除供应商评价考核项目明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSupplierEvaluationItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SUPPLIER_EVALUATION_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新供应商评价考核项目明细状态
 * @param {SupplierEvaluationItemStatus} dto 状态 DTO
 * @returns {Promise<SupplierEvaluationItem>} 供应商评价考核项目明细DTO
 */
export function updateSupplierEvaluationItemStatus(dto: SupplierEvaluationItemStatus): Promise<SupplierEvaluationItem> {
  return request<SupplierEvaluationItem>({
    url: `${SUPPLIER_EVALUATION_ITEM_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新供应商评价考核项目明细作废状态
 * @param {SupplierEvaluationItemObsolete} dto 作废 DTO
 * @returns {Promise<SupplierEvaluationItem>} 供应商评价考核项目明细DTO
 */
export function updateSupplierEvaluationItemObsolete(dto: SupplierEvaluationItemObsolete): Promise<SupplierEvaluationItem> {
  return request<SupplierEvaluationItem>({
    url: `${SUPPLIER_EVALUATION_ITEM_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取供应商评价考核项目明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSupplierEvaluationItemOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${SUPPLIER_EVALUATION_ITEM_API_BASE}/options`,
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
export function getSupplierEvaluationItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SUPPLIER_EVALUATION_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入供应商评价考核项目明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSupplierEvaluationItem(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SUPPLIER_EVALUATION_ITEM_API_BASE}/import`,
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
 * 导出供应商评价考核项目明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSupplierEvaluationItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SUPPLIER_EVALUATION_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

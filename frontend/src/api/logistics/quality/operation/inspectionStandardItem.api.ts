// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/quality/operation
// 文件名称：inspectionStandardItem.api.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/operation 模块 API（自动生成，请勿手改路由常量）
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
  InspectionStandardItem,
  InspectionStandardItemCreate,
  InspectionStandardItemObsolete,
  InspectionStandardItemUpdate
} from '@/types/logistics/quality/operation/inspectionStandardItem.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktInspectionStandardItems
 */
const INSPECTION_STANDARD_ITEM_API_BASE = 'TaktInspectionStandardItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取检验标准明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<InspectionStandardItem>>} 分页结果
 */
export function getInspectionStandardItemList(queryDto: any): Promise<TaktPagedResult<InspectionStandardItem>> {
  return request<TaktPagedResult<InspectionStandardItem>>({
    url: `${INSPECTION_STANDARD_ITEM_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取检验标准明细
 * @param {string} id 检验标准明细ID
 * @returns {Promise<InspectionStandardItem>} 检验标准明细DTO
 */
export function getInspectionStandardItemById(id: string): Promise<InspectionStandardItem> {
  return request<InspectionStandardItem>({
    url: `${INSPECTION_STANDARD_ITEM_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建检验标准明细
 * @param {InspectionStandardItemCreate} dto 创建DTO
 * @returns {Promise<InspectionStandardItem>} 检验标准明细DTO
 */
export function createInspectionStandardItem(dto: InspectionStandardItemCreate): Promise<InspectionStandardItem> {
  return request<InspectionStandardItem>({
    url: `${INSPECTION_STANDARD_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新检验标准明细
 * @param {string} id 检验标准明细ID
 * @param {InspectionStandardItemUpdate} dto 更新DTO
 * @returns {Promise<InspectionStandardItem>} 检验标准明细DTO
 */
export function updateInspectionStandardItem(id: string, dto: InspectionStandardItemUpdate): Promise<InspectionStandardItem> {
  return request<InspectionStandardItem>({
    url: `${INSPECTION_STANDARD_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除检验标准明细
 * @param {string} id 检验标准明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteInspectionStandardItemById(id: string): Promise<void> {
  return request({
    url: `${INSPECTION_STANDARD_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除检验标准明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteInspectionStandardItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${INSPECTION_STANDARD_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新检验标准明细作废状态
 * @param {InspectionStandardItemObsolete} dto 作废 DTO
 * @returns {Promise<InspectionStandardItem>} 检验标准明细DTO
 */
export function updateInspectionStandardItemObsolete(dto: InspectionStandardItemObsolete): Promise<InspectionStandardItem> {
  return request<InspectionStandardItem>({
    url: `${INSPECTION_STANDARD_ITEM_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取检验标准明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getInspectionStandardItemOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${INSPECTION_STANDARD_ITEM_API_BASE}/options`,
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
export function getInspectionStandardItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${INSPECTION_STANDARD_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入检验标准明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importInspectionStandardItem(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${INSPECTION_STANDARD_ITEM_API_BASE}/import`,
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
 * 导出检验标准明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportInspectionStandardItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${INSPECTION_STANDARD_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

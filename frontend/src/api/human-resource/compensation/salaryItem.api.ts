// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/human-resource/compensation
// 文件名称：salaryItem.api.ts
// 创建时间：2026-06-23
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
  SalaryItem,
  SalaryItemCreate,
  SalaryItemSort,
  SalaryItemStatus,
  SalaryItemUpdate
} from '@/types/human-resource/compensation/salaryItem.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSalaryItems
 */
const SALARY_ITEM_API_BASE = 'TaktSalaryItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取薪资项目列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SalaryItem>>} 分页结果
 */
export function getSalaryItemList(queryDto: any): Promise<TaktPagedResult<SalaryItem>> {
  return request<TaktPagedResult<SalaryItem>>({
    url: `${SALARY_ITEM_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取薪资项目
 * @param {string} id 薪资项目ID
 * @returns {Promise<SalaryItem>} 薪资项目DTO
 */
export function getSalaryItemById(id: string): Promise<SalaryItem> {
  return request<SalaryItem>({
    url: `${SALARY_ITEM_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建薪资项目
 * @param {SalaryItemCreate} dto 创建DTO
 * @returns {Promise<SalaryItem>} 薪资项目DTO
 */
export function createSalaryItem(dto: SalaryItemCreate): Promise<SalaryItem> {
  return request<SalaryItem>({
    url: `${SALARY_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新薪资项目
 * @param {string} id 薪资项目ID
 * @param {SalaryItemUpdate} dto 更新DTO
 * @returns {Promise<SalaryItem>} 薪资项目DTO
 */
export function updateSalaryItem(id: string, dto: SalaryItemUpdate): Promise<SalaryItem> {
  return request<SalaryItem>({
    url: `${SALARY_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除薪资项目
 * @param {string} id 薪资项目ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalaryItemById(id: string): Promise<void> {
  return request({
    url: `${SALARY_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除薪资项目
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalaryItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SALARY_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新薪资项目状态
 * @param {SalaryItemStatus} dto 状态 DTO
 * @returns {Promise<SalaryItem>} 薪资项目DTO
 */
export function updateSalaryItemStatus(dto: SalaryItemStatus): Promise<SalaryItem> {
  return request<SalaryItem>({
    url: `${SALARY_ITEM_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新薪资项目排序
 * @param {SalaryItemSort} dto 排序DTO
 * @returns {Promise<SalaryItem>} 薪资项目DTO
 */
export function updateSalaryItemSort(dto: SalaryItemSort): Promise<SalaryItem> {
  return request<SalaryItem>({
    url: `${SALARY_ITEM_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取薪资项目选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSalaryItemOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${SALARY_ITEM_API_BASE}/options`,
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
export function getSalaryItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SALARY_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入薪资项目
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSalaryItem(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SALARY_ITEM_API_BASE}/import`,
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
 * 导出薪资项目
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSalaryItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SALARY_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

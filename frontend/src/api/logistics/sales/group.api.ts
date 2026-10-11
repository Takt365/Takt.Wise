// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/sales
// 文件名称：group.api.ts
// 创建时间：2026-08-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/sales 模块 API（自动生成，请勿手改路由常量）
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
  SalesGroup,
  SalesGroupCreate,
  SalesGroupSort,
  SalesGroupStatus,
  SalesGroupUpdate
} from '@/types/logistics/sales/group.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSalesGroups
 */
const SALES_GROUP_API_BASE = 'TaktSalesGroups';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取销售组主数据列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SalesGroup>>} 分页结果
 */
export function getSalesGroupList(queryDto: any): Promise<TaktPagedResult<SalesGroup>> {
  return request<TaktPagedResult<SalesGroup>>({
    url: `${SALES_GROUP_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取销售组主数据
 * @param {string} id 销售组主数据ID
 * @returns {Promise<SalesGroup>} 销售组主数据DTO
 */
export function getSalesGroupById(id: string): Promise<SalesGroup> {
  return request<SalesGroup>({
    url: `${SALES_GROUP_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建销售组主数据
 * @param {SalesGroupCreate} dto 创建DTO
 * @returns {Promise<SalesGroup>} 销售组主数据DTO
 */
export function createSalesGroup(dto: SalesGroupCreate): Promise<SalesGroup> {
  return request<SalesGroup>({
    url: `${SALES_GROUP_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新销售组主数据
 * @param {string} id 销售组主数据ID
 * @param {SalesGroupUpdate} dto 更新DTO
 * @returns {Promise<SalesGroup>} 销售组主数据DTO
 */
export function updateSalesGroup(id: string, dto: SalesGroupUpdate): Promise<SalesGroup> {
  return request<SalesGroup>({
    url: `${SALES_GROUP_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除销售组主数据
 * @param {string} id 销售组主数据ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesGroupById(id: string): Promise<void> {
  return request({
    url: `${SALES_GROUP_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除销售组主数据
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSalesGroupBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SALES_GROUP_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新销售组主数据状态
 * @param {SalesGroupStatus} dto 状态 DTO
 * @returns {Promise<SalesGroup>} 销售组主数据DTO
 */
export function updateSalesGroupStatus(dto: SalesGroupStatus): Promise<SalesGroup> {
  return request<SalesGroup>({
    url: `${SALES_GROUP_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新销售组主数据排序
 * @param {SalesGroupSort} dto 排序DTO
 * @returns {Promise<SalesGroup>} 销售组主数据DTO
 */
export function updateSalesGroupSort(dto: SalesGroupSort): Promise<SalesGroup> {
  return request<SalesGroup>({
    url: `${SALES_GROUP_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取销售组主数据选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSalesGroupOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${SALES_GROUP_API_BASE}/options`,
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
export function getSalesGroupTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_GROUP_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入销售组主数据
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSalesGroup(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SALES_GROUP_API_BASE}/import`,
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
 * 导出销售组主数据
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSalesGroup(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SALES_GROUP_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

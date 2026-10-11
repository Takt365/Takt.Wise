// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/accounting/controlling
// 文件名称：profitCenter.api.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/controlling 模块 API（自动生成，请勿手改路由常量）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  TaktPagedResult,
  TaktTreeSelectOption
} from '@/types/common.types';
import type {
  ProfitCenter,
  ProfitCenterCreate,
  ProfitCenterSort,
  ProfitCenterStatus,
  ProfitCenterTree,
  ProfitCenterUpdate
} from '@/types/accounting/controlling/profitCenter.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktProfitCenters
 */
const PROFIT_CENTER_API_BASE = 'TaktProfitCenters';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取利润中心列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ProfitCenter>>} 分页结果
 */
export function getProfitCenterList(queryDto: any): Promise<TaktPagedResult<ProfitCenter>> {
  return request<TaktPagedResult<ProfitCenter>>({
    url: `${PROFIT_CENTER_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取利润中心
 * @param {string} id 利润中心ID
 * @returns {Promise<ProfitCenter>} 利润中心DTO
 */
export function getProfitCenterById(id: string): Promise<ProfitCenter> {
  return request<ProfitCenter>({
    url: `${PROFIT_CENTER_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 获取利润中心树形列表（懒加载：仅 parentId 直接子级一层）
 * @param {string} parentId 父级ID（0=根；懒加载仅返回直接子级一层）
 * @param {boolean} includeDisabled 为 false 时过滤禁用项（按实体 *Status 字段）
 * @returns {Promise<ProfitCenterTree[]>} 树形数据
 */
export function getProfitCenterTree(parentId: string, includeDisabled: boolean): Promise<ProfitCenterTree[]> {
  return request<ProfitCenterTree[]>({
    url: `${PROFIT_CENTER_API_BASE}/tree`,
    method: 'get',
    params: {
      parentId,
      includeDisabled
    },
  });
}

/**
 * 创建利润中心
 * @param {ProfitCenterCreate} dto 创建DTO
 * @returns {Promise<ProfitCenter>} 利润中心DTO
 */
export function createProfitCenter(dto: ProfitCenterCreate): Promise<ProfitCenter> {
  return request<ProfitCenter>({
    url: `${PROFIT_CENTER_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新利润中心
 * @param {string} id 利润中心ID
 * @param {ProfitCenterUpdate} dto 更新DTO
 * @returns {Promise<ProfitCenter>} 利润中心DTO
 */
export function updateProfitCenter(id: string, dto: ProfitCenterUpdate): Promise<ProfitCenter> {
  return request<ProfitCenter>({
    url: `${PROFIT_CENTER_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除利润中心
 * @param {string} id 利润中心ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteProfitCenterById(id: string): Promise<void> {
  return request({
    url: `${PROFIT_CENTER_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除利润中心
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteProfitCenterBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PROFIT_CENTER_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新利润中心状态
 * @param {ProfitCenterStatus} dto 状态 DTO
 * @returns {Promise<ProfitCenter>} 利润中心DTO
 */
export function updateProfitCenterStatus(dto: ProfitCenterStatus): Promise<ProfitCenter> {
  return request<ProfitCenter>({
    url: `${PROFIT_CENTER_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新利润中心排序
 * @param {ProfitCenterSort} dto 排序DTO
 * @returns {Promise<ProfitCenter>} 利润中心DTO
 */
export function updateProfitCenterSort(dto: ProfitCenterSort): Promise<ProfitCenter> {
  return request<ProfitCenter>({
    url: `${PROFIT_CENTER_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取利润中心树形选项列表（懒加载：仅 parentId 直接子级一层）
 * @param {string} parentId 父级ID（0=根；懒加载仅返回直接子级一层）
 * @param {string} plantCode 工厂代码（可选，用于按工厂过滤）
 * @param {string} keyword 搜索关键字（可选，模糊匹配）
 * @returns {Promise<TaktTreeSelectOption[]>} 树形选项
 */
export function getProfitCenterTreeOptions(
  parentId: string,
  plantCode?: string,
  keyword?: string
): Promise<TaktTreeSelectOption[]> {
  return request<TaktTreeSelectOption[]>({
    url: `${PROFIT_CENTER_API_BASE}/tree-options`,
    method: 'get',
    params: {
      parentId,
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
export function getProfitCenterTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PROFIT_CENTER_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入利润中心
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importProfitCenter(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PROFIT_CENTER_API_BASE}/import`,
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
 * 导出利润中心
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportProfitCenter(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PROFIT_CENTER_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

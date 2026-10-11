// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/accounting/controlling
// 文件名称：costElement.api.ts
// 创建时间：2026-08-21
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
  CostElement,
  CostElementCreate,
  CostElementSort,
  CostElementStatus,
  CostElementTree,
  CostElementUpdate
} from '@/types/accounting/controlling/costElement.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktCostElements
 */
const COST_ELEMENT_API_BASE = 'TaktCostElements';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取成本要素列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<CostElement>>} 分页结果
 */
export function getCostElementList(queryDto: any): Promise<TaktPagedResult<CostElement>> {
  return request<TaktPagedResult<CostElement>>({
    url: `${COST_ELEMENT_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取成本要素
 * @param {string} id 成本要素ID
 * @returns {Promise<CostElement>} 成本要素DTO
 */
export function getCostElementById(id: string): Promise<CostElement> {
  return request<CostElement>({
    url: `${COST_ELEMENT_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 获取成本要素树形列表（懒加载：仅 parentId 直接子级一层）
 * @param {string} parentId 父级ID（0=根；懒加载仅返回直接子级一层）
 * @param {boolean} includeDisabled 为 false 时过滤禁用项（按实体 *Status 字段）
 * @returns {Promise<CostElementTree[]>} 树形数据
 */
export function getCostElementTree(parentId: string, includeDisabled: boolean): Promise<CostElementTree[]> {
  return request<CostElementTree[]>({
    url: `${COST_ELEMENT_API_BASE}/tree`,
    method: 'get',
    params: {
      parentId,
      includeDisabled
    },
  });
}

/**
 * 创建成本要素
 * @param {CostElementCreate} dto 创建DTO
 * @returns {Promise<CostElement>} 成本要素DTO
 */
export function createCostElement(dto: CostElementCreate): Promise<CostElement> {
  return request<CostElement>({
    url: `${COST_ELEMENT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新成本要素
 * @param {string} id 成本要素ID
 * @param {CostElementUpdate} dto 更新DTO
 * @returns {Promise<CostElement>} 成本要素DTO
 */
export function updateCostElement(id: string, dto: CostElementUpdate): Promise<CostElement> {
  return request<CostElement>({
    url: `${COST_ELEMENT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除成本要素
 * @param {string} id 成本要素ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteCostElementById(id: string): Promise<void> {
  return request({
    url: `${COST_ELEMENT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除成本要素
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteCostElementBatch(ids: string[]): Promise<void> {
  return request({
    url: `${COST_ELEMENT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新成本要素状态
 * @param {CostElementStatus} dto 状态 DTO
 * @returns {Promise<CostElement>} 成本要素DTO
 */
export function updateCostElementStatus(dto: CostElementStatus): Promise<CostElement> {
  return request<CostElement>({
    url: `${COST_ELEMENT_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新成本要素排序
 * @param {CostElementSort} dto 排序DTO
 * @returns {Promise<CostElement>} 成本要素DTO
 */
export function updateCostElementSort(dto: CostElementSort): Promise<CostElement> {
  return request<CostElement>({
    url: `${COST_ELEMENT_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取成本要素树形选项列表（懒加载：仅 parentId 直接子级一层；DictValue 为 CostElementCode）
 * @param {string} parentId 父级ID（0=根；懒加载仅返回直接子级一层）
 * @returns {Promise<TaktTreeSelectOption[]>} 树形选项
 */
export function getCostElementTreeOptions(parentId: string): Promise<TaktTreeSelectOption[]> {
  return request<TaktTreeSelectOption[]>({
    url: `${COST_ELEMENT_API_BASE}/tree-options`,
    method: 'get',
    params: {
      parentId
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
export function getCostElementTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${COST_ELEMENT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入成本要素
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importCostElement(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${COST_ELEMENT_API_BASE}/import`,
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
 * 导出成本要素
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportCostElement(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${COST_ELEMENT_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/quality/cost
// 文件名称：group.api.ts
// 创建时间：2026-10-08
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/cost 模块 API（自动生成，请勿手改路由常量）
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
  CostGroup,
  CostGroupCreate,
  CostGroupSort,
  CostGroupStatus,
  CostGroupUpdate
} from '@/types/logistics/quality/cost/group.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktCostGroups
 */
const COST_GROUP_API_BASE = 'TaktCostGroups';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取成本组主数据列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<CostGroup>>} 分页结果
 */
export function getCostGroupList(queryDto: any): Promise<TaktPagedResult<CostGroup>> {
  return request<TaktPagedResult<CostGroup>>({
    url: `${COST_GROUP_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取成本组主数据
 * @param {string} id 成本组主数据ID
 * @returns {Promise<CostGroup>} 成本组主数据DTO
 */
export function getCostGroupById(id: string): Promise<CostGroup> {
  return request<CostGroup>({
    url: `${COST_GROUP_API_BASE}/{id:long}`,
    method: 'get',
    params: {
      id
    },
  });
}

/**
 * 创建成本组主数据
 * @param {CostGroupCreate} dto 创建DTO
 * @returns {Promise<CostGroup>} 成本组主数据DTO
 */
export function createCostGroup(dto: CostGroupCreate): Promise<CostGroup> {
  return request<CostGroup>({
    url: `${COST_GROUP_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新成本组主数据
 * @param {string} id 成本组主数据ID
 * @param {CostGroupUpdate} dto 更新DTO
 * @returns {Promise<CostGroup>} 成本组主数据DTO
 */
export function updateCostGroup(id: string, dto: CostGroupUpdate): Promise<CostGroup> {
  return request<CostGroup>({
    url: `${COST_GROUP_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除成本组主数据
 * @param {string} id 成本组主数据ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteCostGroupById(id: string): Promise<void> {
  return request({
    url: `${COST_GROUP_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除成本组主数据
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteCostGroupBatch(ids: string[]): Promise<void> {
  return request({
    url: `${COST_GROUP_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新成本组主数据状态
 * @param {CostGroupStatus} dto 状态 DTO
 * @returns {Promise<CostGroup>} 成本组主数据DTO
 */
export function updateCostGroupStatus(dto: CostGroupStatus): Promise<CostGroup> {
  return request<CostGroup>({
    url: `${COST_GROUP_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新成本组主数据排序
 * @param {CostGroupSort} dto 排序DTO
 * @returns {Promise<CostGroup>} 成本组主数据DTO
 */
export function updateCostGroupSort(dto: CostGroupSort): Promise<CostGroup> {
  return request<CostGroup>({
    url: `${COST_GROUP_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取成本组主数据选项列表
 * @param {string} plantCode 工厂代码（可选，用于按工厂过滤）
 * @param {string} keyword 搜索关键字（可选，模糊匹配）
 * @param {number} costGroupCategory 成本组类别（可选；1=记录组 2=废弃组 3=对应组）
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getCostGroupOptions(plantCode?: string, keyword?: string, costGroupCategory?: number): Promise<TaktSelectOption[]> {
  return request<TaktSelectOption[]>({
    url: `${COST_GROUP_API_BASE}/options`,
    method: 'get',
    params: {
      plantCode,
      keyword,
      costGroupCategory,
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
export function getCostGroupTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${COST_GROUP_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入成本组主数据
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importCostGroup(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${COST_GROUP_API_BASE}/import`,
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
 * 导出成本组主数据
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportCostGroup(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${COST_GROUP_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

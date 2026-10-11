// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/bom
// 文件名称：routing.api.ts
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/bom 模块 API（自动生成，请勿手改路由常量）
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
  Routing,
  RoutingCreate,
  RoutingStatus,
  RoutingUpdate
} from '@/types/logistics/manufacturing/bom/routing.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktRoutings
 */
const ROUTING_API_BASE = 'TaktRoutings';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取工艺路线主列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Routing>>} 分页结果
 */
export function getRoutingList(queryDto: any): Promise<TaktPagedResult<Routing>> {
  return request<TaktPagedResult<Routing>>({
    url: `${ROUTING_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取工艺路线主
 * @param {string} id 工艺路线主ID
 * @returns {Promise<Routing>} 工艺路线主DTO
 */
export function getRoutingById(id: string): Promise<Routing> {
  return request<Routing>({
    url: `${ROUTING_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建工艺路线主
 * @param {RoutingCreate} dto 创建DTO
 * @returns {Promise<Routing>} 工艺路线主DTO
 */
export function createRouting(dto: RoutingCreate): Promise<Routing> {
  return request<Routing>({
    url: `${ROUTING_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新工艺路线主
 * @param {string} id 工艺路线主ID
 * @param {RoutingUpdate} dto 更新DTO
 * @returns {Promise<Routing>} 工艺路线主DTO
 */
export function updateRouting(id: string, dto: RoutingUpdate): Promise<Routing> {
  return request<Routing>({
    url: `${ROUTING_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除工艺路线主
 * @param {string} id 工艺路线主ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteRoutingById(id: string): Promise<void> {
  return request({
    url: `${ROUTING_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除工艺路线主
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteRoutingBatch(ids: string[]): Promise<void> {
  return request({
    url: `${ROUTING_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新工艺路线主状态
 * @param {RoutingStatus} dto 状态 DTO
 * @returns {Promise<Routing>} 工艺路线主DTO
 */
export function updateRoutingStatus(dto: RoutingStatus): Promise<Routing> {
  return request<Routing>({
    url: `${ROUTING_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取工艺路线主选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getRoutingOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${ROUTING_API_BASE}/options`,
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
export function getRoutingTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${ROUTING_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入工艺路线主
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importRouting(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${ROUTING_API_BASE}/import`,
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
 * 导出工艺路线主
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportRouting(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${ROUTING_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

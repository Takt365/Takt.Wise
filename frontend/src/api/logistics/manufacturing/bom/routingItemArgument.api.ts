// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/bom
// 文件名称：routingItemArgument.api.ts
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
  RoutingItemArgument,
  RoutingItemArgumentCreate,
  RoutingItemArgumentSort,
  RoutingItemArgumentUpdate
} from '@/types/logistics/manufacturing/bom/routingItemArgument.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktRoutingItemArguments
 */
const ROUTING_ITEM_ARGUMENT_API_BASE = 'TaktRoutingItemArguments';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取工艺路线工序参数列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<RoutingItemArgument>>} 分页结果
 */
export function getRoutingItemArgumentList(queryDto: any): Promise<TaktPagedResult<RoutingItemArgument>> {
  return request<TaktPagedResult<RoutingItemArgument>>({
    url: `${ROUTING_ITEM_ARGUMENT_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取工艺路线工序参数
 * @param {string} id 工艺路线工序参数ID
 * @returns {Promise<RoutingItemArgument>} 工艺路线工序参数DTO
 */
export function getRoutingItemArgumentById(id: string): Promise<RoutingItemArgument> {
  return request<RoutingItemArgument>({
    url: `${ROUTING_ITEM_ARGUMENT_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建工艺路线工序参数
 * @param {RoutingItemArgumentCreate} dto 创建DTO
 * @returns {Promise<RoutingItemArgument>} 工艺路线工序参数DTO
 */
export function createRoutingItemArgument(dto: RoutingItemArgumentCreate): Promise<RoutingItemArgument> {
  return request<RoutingItemArgument>({
    url: `${ROUTING_ITEM_ARGUMENT_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新工艺路线工序参数
 * @param {string} id 工艺路线工序参数ID
 * @param {RoutingItemArgumentUpdate} dto 更新DTO
 * @returns {Promise<RoutingItemArgument>} 工艺路线工序参数DTO
 */
export function updateRoutingItemArgument(id: string, dto: RoutingItemArgumentUpdate): Promise<RoutingItemArgument> {
  return request<RoutingItemArgument>({
    url: `${ROUTING_ITEM_ARGUMENT_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除工艺路线工序参数
 * @param {string} id 工艺路线工序参数ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteRoutingItemArgumentById(id: string): Promise<void> {
  return request({
    url: `${ROUTING_ITEM_ARGUMENT_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除工艺路线工序参数
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteRoutingItemArgumentBatch(ids: string[]): Promise<void> {
  return request({
    url: `${ROUTING_ITEM_ARGUMENT_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新工艺路线工序参数排序
 * @param {RoutingItemArgumentSort} dto 排序DTO
 * @returns {Promise<RoutingItemArgument>} 工艺路线工序参数DTO
 */
export function updateRoutingItemArgumentSort(dto: RoutingItemArgumentSort): Promise<RoutingItemArgument> {
  return request<RoutingItemArgument>({
    url: `${ROUTING_ITEM_ARGUMENT_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取工艺路线工序参数选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getRoutingItemArgumentOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${ROUTING_ITEM_ARGUMENT_API_BASE}/options`,
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
export function getRoutingItemArgumentTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${ROUTING_ITEM_ARGUMENT_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入工艺路线工序参数
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importRoutingItemArgument(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${ROUTING_ITEM_ARGUMENT_API_BASE}/import`,
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
 * 导出工艺路线工序参数
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportRoutingItemArgument(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${ROUTING_ITEM_ARGUMENT_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

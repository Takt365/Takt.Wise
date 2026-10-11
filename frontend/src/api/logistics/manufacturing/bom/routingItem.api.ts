// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/bom
// 文件名称：routingItem.api.ts
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
  RoutingItem,
  RoutingItemCreate,
  RoutingItemObsolete,
  RoutingItemSort,
  RoutingItemUpdate
} from '@/types/logistics/manufacturing/bom/routingItem.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktRoutingItems
 */
const ROUTING_ITEM_API_BASE = 'TaktRoutingItems';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取工艺路线明细列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<RoutingItem>>} 分页结果
 */
export function getRoutingItemList(queryDto: any): Promise<TaktPagedResult<RoutingItem>> {
  return request<TaktPagedResult<RoutingItem>>({
    url: `${ROUTING_ITEM_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取工艺路线明细
 * @param {string} id 工艺路线明细ID
 * @returns {Promise<RoutingItem>} 工艺路线明细DTO
 */
export function getRoutingItemById(id: string): Promise<RoutingItem> {
  return request<RoutingItem>({
    url: `${ROUTING_ITEM_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建工艺路线明细
 * @param {RoutingItemCreate} dto 创建DTO
 * @returns {Promise<RoutingItem>} 工艺路线明细DTO
 */
export function createRoutingItem(dto: RoutingItemCreate): Promise<RoutingItem> {
  return request<RoutingItem>({
    url: `${ROUTING_ITEM_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新工艺路线明细
 * @param {string} id 工艺路线明细ID
 * @param {RoutingItemUpdate} dto 更新DTO
 * @returns {Promise<RoutingItem>} 工艺路线明细DTO
 */
export function updateRoutingItem(id: string, dto: RoutingItemUpdate): Promise<RoutingItem> {
  return request<RoutingItem>({
    url: `${ROUTING_ITEM_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除工艺路线明细
 * @param {string} id 工艺路线明细ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteRoutingItemById(id: string): Promise<void> {
  return request({
    url: `${ROUTING_ITEM_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除工艺路线明细
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteRoutingItemBatch(ids: string[]): Promise<void> {
  return request({
    url: `${ROUTING_ITEM_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新工艺路线明细排序
 * @param {RoutingItemSort} dto 排序DTO
 * @returns {Promise<RoutingItem>} 工艺路线明细DTO
 */
export function updateRoutingItemSort(dto: RoutingItemSort): Promise<RoutingItem> {
  return request<RoutingItem>({
    url: `${ROUTING_ITEM_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新工艺路线明细作废状态
 * @param {RoutingItemObsolete} dto 作废 DTO
 * @returns {Promise<RoutingItem>} 工艺路线明细DTO
 */
export function updateRoutingItemObsolete(dto: RoutingItemObsolete): Promise<RoutingItem> {
  return request<RoutingItem>({
    url: `${ROUTING_ITEM_API_BASE}/obsolete`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取工艺路线明细选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getRoutingItemOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${ROUTING_ITEM_API_BASE}/options`,
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
export function getRoutingItemTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${ROUTING_ITEM_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入工艺路线明细
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importRoutingItem(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${ROUTING_ITEM_API_BASE}/import`,
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
 * 导出工艺路线明细
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportRoutingItem(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${ROUTING_ITEM_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

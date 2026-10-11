// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/aps
// 文件名称：plannedOrder.api.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/aps 模块 API（自动生成，请勿手改路由常量）
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
  PlannedOrder,
  PlannedOrderCreate,
  PlannedOrderStatus,
  PlannedOrderUpdate
} from '@/types/logistics/manufacturing/aps/plannedOrder.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktPlannedOrders
 */
const PLANNED_ORDER_API_BASE = 'TaktPlannedOrders';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取计划订单列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<PlannedOrder>>} 分页结果
 */
export function getPlannedOrderList(queryDto: any): Promise<TaktPagedResult<PlannedOrder>> {
  return request<TaktPagedResult<PlannedOrder>>({
    url: `${PLANNED_ORDER_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取计划订单
 * @param {string} id 计划订单ID
 * @returns {Promise<PlannedOrder>} 计划订单DTO
 */
export function getPlannedOrderById(id: string): Promise<PlannedOrder> {
  return request<PlannedOrder>({
    url: `${PLANNED_ORDER_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建计划订单
 * @param {PlannedOrderCreate} dto 创建DTO
 * @returns {Promise<PlannedOrder>} 计划订单DTO
 */
export function createPlannedOrder(dto: PlannedOrderCreate): Promise<PlannedOrder> {
  return request<PlannedOrder>({
    url: `${PLANNED_ORDER_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新计划订单
 * @param {string} id 计划订单ID
 * @param {PlannedOrderUpdate} dto 更新DTO
 * @returns {Promise<PlannedOrder>} 计划订单DTO
 */
export function updatePlannedOrder(id: string, dto: PlannedOrderUpdate): Promise<PlannedOrder> {
  return request<PlannedOrder>({
    url: `${PLANNED_ORDER_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除计划订单
 * @param {string} id 计划订单ID
 * @returns {Promise<void>} 操作结果
 */
export function deletePlannedOrderById(id: string): Promise<void> {
  return request({
    url: `${PLANNED_ORDER_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除计划订单
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deletePlannedOrderBatch(ids: string[]): Promise<void> {
  return request({
    url: `${PLANNED_ORDER_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新计划订单状态
 * @param {PlannedOrderStatus} dto 状态 DTO
 * @returns {Promise<PlannedOrder>} 计划订单DTO
 */
export function updatePlannedOrderStatus(dto: PlannedOrderStatus): Promise<PlannedOrder> {
  return request<PlannedOrder>({
    url: `${PLANNED_ORDER_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取计划订单选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getPlannedOrderOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${PLANNED_ORDER_API_BASE}/options`,
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
export function getPlannedOrderTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${PLANNED_ORDER_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入计划订单
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importPlannedOrder(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${PLANNED_ORDER_API_BASE}/import`,
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
 * 导出计划订单
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportPlannedOrder(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${PLANNED_ORDER_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

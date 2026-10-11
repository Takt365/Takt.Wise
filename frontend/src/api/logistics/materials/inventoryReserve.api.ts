// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/materials
// 文件名称：inventoryReserve.api.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/materials 模块 API（自动生成，请勿手改路由常量）
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
  InventoryReserve,
  InventoryReserveCreate,
  InventoryReserveSort,
  InventoryReserveStatus,
  InventoryReserveUpdate
} from '@/types/logistics/materials/inventoryReserve.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktInventoryReserves
 */
const INVENTORY_RESERVE_API_BASE = 'TaktInventoryReserves';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取存货跌价准备列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<InventoryReserve>>} 分页结果
 */
export function getInventoryReserveList(queryDto: any): Promise<TaktPagedResult<InventoryReserve>> {
  return request<TaktPagedResult<InventoryReserve>>({
    url: `${INVENTORY_RESERVE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取存货跌价准备
 * @param {string} id 存货跌价准备ID
 * @returns {Promise<InventoryReserve>} 存货跌价准备DTO
 */
export function getInventoryReserveById(id: string): Promise<InventoryReserve> {
  return request<InventoryReserve>({
    url: `${INVENTORY_RESERVE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建存货跌价准备
 * @param {InventoryReserveCreate} dto 创建DTO
 * @returns {Promise<InventoryReserve>} 存货跌价准备DTO
 */
export function createInventoryReserve(dto: InventoryReserveCreate): Promise<InventoryReserve> {
  return request<InventoryReserve>({
    url: `${INVENTORY_RESERVE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新存货跌价准备
 * @param {string} id 存货跌价准备ID
 * @param {InventoryReserveUpdate} dto 更新DTO
 * @returns {Promise<InventoryReserve>} 存货跌价准备DTO
 */
export function updateInventoryReserve(id: string, dto: InventoryReserveUpdate): Promise<InventoryReserve> {
  return request<InventoryReserve>({
    url: `${INVENTORY_RESERVE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除存货跌价准备
 * @param {string} id 存货跌价准备ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteInventoryReserveById(id: string): Promise<void> {
  return request({
    url: `${INVENTORY_RESERVE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除存货跌价准备
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteInventoryReserveBatch(ids: string[]): Promise<void> {
  return request({
    url: `${INVENTORY_RESERVE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新存货跌价准备状态
 * @param {InventoryReserveStatus} dto 状态 DTO
 * @returns {Promise<InventoryReserve>} 存货跌价准备DTO
 */
export function updateInventoryReserveStatus(dto: InventoryReserveStatus): Promise<InventoryReserve> {
  return request<InventoryReserve>({
    url: `${INVENTORY_RESERVE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新存货跌价准备排序
 * @param {InventoryReserveSort} dto 排序DTO
 * @returns {Promise<InventoryReserve>} 存货跌价准备DTO
 */
export function updateInventoryReserveSort(dto: InventoryReserveSort): Promise<InventoryReserve> {
  return request<InventoryReserve>({
    url: `${INVENTORY_RESERVE_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取存货跌价准备选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getInventoryReserveOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${INVENTORY_RESERVE_API_BASE}/options`,
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
export function getInventoryReserveTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${INVENTORY_RESERVE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入存货跌价准备
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importInventoryReserve(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${INVENTORY_RESERVE_API_BASE}/import`,
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
 * 导出存货跌价准备
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportInventoryReserve(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${INVENTORY_RESERVE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

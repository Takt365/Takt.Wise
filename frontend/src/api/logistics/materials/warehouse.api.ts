// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/materials
// 文件名称：warehouse.api.ts
// 创建时间：2026-06-23
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
  Warehouse,
  WarehouseCreate,
  WarehouseSort,
  WarehouseStatus,
  WarehouseUpdate
} from '@/types/logistics/materials/warehouse.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktWarehouses
 */
const WAREHOUSE_API_BASE = 'TaktWarehouses';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取仓库主数据列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<Warehouse>>} 分页结果
 */
export function getWarehouseList(queryDto: any): Promise<TaktPagedResult<Warehouse>> {
  return request<TaktPagedResult<Warehouse>>({
    url: `${WAREHOUSE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取仓库主数据
 * @param {string} id 仓库主数据ID
 * @returns {Promise<Warehouse>} 仓库主数据DTO
 */
export function getWarehouseById(id: string): Promise<Warehouse> {
  return request<Warehouse>({
    url: `${WAREHOUSE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建仓库主数据
 * @param {WarehouseCreate} dto 创建DTO
 * @returns {Promise<Warehouse>} 仓库主数据DTO
 */
export function createWarehouse(dto: WarehouseCreate): Promise<Warehouse> {
  return request<Warehouse>({
    url: `${WAREHOUSE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新品仓库主数据
 * @param {string} id 仓库主数据ID
 * @param {WarehouseUpdate} dto 更新DTO
 * @returns {Promise<Warehouse>} 仓库主数据DTO
 */
export function updateWarehouse(id: string, dto: WarehouseUpdate): Promise<Warehouse> {
  return request<Warehouse>({
    url: `${WAREHOUSE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除仓库主数据
 * @param {string} id 仓库主数据ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteWarehouseById(id: string): Promise<void> {
  return request({
    url: `${WAREHOUSE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除仓库主数据
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteWarehouseBatch(ids: string[]): Promise<void> {
  return request({
    url: `${WAREHOUSE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新品仓库主数据状态
 * @param {WarehouseStatus} dto 状态 DTO
 * @returns {Promise<Warehouse>} 仓库主数据DTO
 */
export function updateWarehouseStatus(dto: WarehouseStatus): Promise<Warehouse> {
  return request<Warehouse>({
    url: `${WAREHOUSE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新品仓库主数据排序
 * @param {WarehouseSort} dto 排序DTO
 * @returns {Promise<Warehouse>} 仓库主数据DTO
 */
export function updateWarehouseSort(dto: WarehouseSort): Promise<Warehouse> {
  return request<Warehouse>({
    url: `${WAREHOUSE_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取仓库主数据选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getWarehouseOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${WAREHOUSE_API_BASE}/options`,
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
export function getWarehouseTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${WAREHOUSE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入仓库主数据
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importWarehouse(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${WAREHOUSE_API_BASE}/import`,
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
 * 导出仓库主数据
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportWarehouse(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${WAREHOUSE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/procurement
// 文件名称：sourceOfSupply.api.ts
// 创建时间：2026-07-21
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/procurement 模块 API（自动生成，请勿手改路由常量）
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
  SourceOfSupply,
  SourceOfSupplyCreate,
  SourceOfSupplySort,
  SourceOfSupplyStatus,
  SourceOfSupplyUpdate
} from '@/types/logistics/procurement/sourceOfSupply.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktSourceOfSupplies
 */
const SOURCE_OF_SUPPLY_API_BASE = 'TaktSourceOfSupplies';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取货源清单清单列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<SourceOfSupply>>} 分页结果
 */
export function getSourceOfSupplyList(queryDto: any): Promise<TaktPagedResult<SourceOfSupply>> {
  return request<TaktPagedResult<SourceOfSupply>>({
    url: `${SOURCE_OF_SUPPLY_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取货源清单清单
 * @param {string} id 货源清单清单ID
 * @returns {Promise<SourceOfSupply>} 货源清单清单DTO
 */
export function getSourceOfSupplyById(id: string): Promise<SourceOfSupply> {
  return request<SourceOfSupply>({
    url: `${SOURCE_OF_SUPPLY_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建货源清单清单
 * @param {SourceOfSupplyCreate} dto 创建DTO
 * @returns {Promise<SourceOfSupply>} 货源清单清单DTO
 */
export function createSourceOfSupply(dto: SourceOfSupplyCreate): Promise<SourceOfSupply> {
  return request<SourceOfSupply>({
    url: `${SOURCE_OF_SUPPLY_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新货源清单清单
 * @param {string} id 货源清单清单ID
 * @param {SourceOfSupplyUpdate} dto 更新DTO
 * @returns {Promise<SourceOfSupply>} 货源清单清单DTO
 */
export function updateSourceOfSupply(id: string, dto: SourceOfSupplyUpdate): Promise<SourceOfSupply> {
  return request<SourceOfSupply>({
    url: `${SOURCE_OF_SUPPLY_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除货源清单清单
 * @param {string} id 货源清单清单ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteSourceOfSupplyById(id: string): Promise<void> {
  return request({
    url: `${SOURCE_OF_SUPPLY_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除货源清单清单
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteSourceOfSupplyBatch(ids: string[]): Promise<void> {
  return request({
    url: `${SOURCE_OF_SUPPLY_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新货源清单清单状态
 * @param {SourceOfSupplyStatus} dto 状态 DTO
 * @returns {Promise<SourceOfSupply>} 货源清单清单DTO
 */
export function updateSourceOfSupplyStatus(dto: SourceOfSupplyStatus): Promise<SourceOfSupply> {
  return request<SourceOfSupply>({
    url: `${SOURCE_OF_SUPPLY_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新货源清单清单排序
 * @param {SourceOfSupplySort} dto 排序DTO
 * @returns {Promise<SourceOfSupply>} 货源清单清单DTO
 */
export function updateSourceOfSupplySort(dto: SourceOfSupplySort): Promise<SourceOfSupply> {
  return request<SourceOfSupply>({
    url: `${SOURCE_OF_SUPPLY_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取货源清单清单选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getSourceOfSupplyOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${SOURCE_OF_SUPPLY_API_BASE}/options`,
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
export function getSourceOfSupplyTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${SOURCE_OF_SUPPLY_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入货源清单清单
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importSourceOfSupply(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${SOURCE_OF_SUPPLY_API_BASE}/import`,
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
 * 导出货源清单清单
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportSourceOfSupply(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${SOURCE_OF_SUPPLY_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

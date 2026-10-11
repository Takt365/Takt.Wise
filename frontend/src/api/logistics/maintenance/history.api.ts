// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/maintenance
// 文件名称：history.api.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/maintenance 模块 API（自动生成，请勿手改路由常量）
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
  MaintenanceHistory,
  MaintenanceHistoryCreate,
  MaintenanceHistoryStatus,
  MaintenanceHistoryUpdate
} from '@/types/logistics/maintenance/history.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktMaintenanceHistories
 */
const MAINTENANCE_HISTORY_API_BASE = 'TaktMaintenanceHistories';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取设备维护履历列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<MaintenanceHistory>>} 分页结果
 */
export function getMaintenanceHistoryList(queryDto: any): Promise<TaktPagedResult<MaintenanceHistory>> {
  return request<TaktPagedResult<MaintenanceHistory>>({
    url: `${MAINTENANCE_HISTORY_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取设备维护履历
 * @param {string} id 设备维护履历ID
 * @returns {Promise<MaintenanceHistory>} 设备维护履历DTO
 */
export function getMaintenanceHistoryById(id: string): Promise<MaintenanceHistory> {
  return request<MaintenanceHistory>({
    url: `${MAINTENANCE_HISTORY_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建设备维护履历
 * @param {MaintenanceHistoryCreate} dto 创建DTO
 * @returns {Promise<MaintenanceHistory>} 设备维护履历DTO
 */
export function createMaintenanceHistory(dto: MaintenanceHistoryCreate): Promise<MaintenanceHistory> {
  return request<MaintenanceHistory>({
    url: `${MAINTENANCE_HISTORY_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新设备维护履历
 * @param {string} id 设备维护履历ID
 * @param {MaintenanceHistoryUpdate} dto 更新DTO
 * @returns {Promise<MaintenanceHistory>} 设备维护履历DTO
 */
export function updateMaintenanceHistory(id: string, dto: MaintenanceHistoryUpdate): Promise<MaintenanceHistory> {
  return request<MaintenanceHistory>({
    url: `${MAINTENANCE_HISTORY_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除设备维护履历
 * @param {string} id 设备维护履历ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteMaintenanceHistoryById(id: string): Promise<void> {
  return request({
    url: `${MAINTENANCE_HISTORY_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除设备维护履历
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteMaintenanceHistoryBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MAINTENANCE_HISTORY_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新设备维护履历状态
 * @param {MaintenanceHistoryStatus} dto 状态 DTO
 * @returns {Promise<MaintenanceHistory>} 设备维护履历DTO
 */
export function updateMaintenanceHistoryStatus(dto: MaintenanceHistoryStatus): Promise<MaintenanceHistory> {
  return request<MaintenanceHistory>({
    url: `${MAINTENANCE_HISTORY_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取设备维护履历选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getMaintenanceHistoryOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${MAINTENANCE_HISTORY_API_BASE}/options`,
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
export function getMaintenanceHistoryTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${MAINTENANCE_HISTORY_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入设备维护履历
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importMaintenanceHistory(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${MAINTENANCE_HISTORY_API_BASE}/import`,
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
 * 导出设备维护履历
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportMaintenanceHistory(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MAINTENANCE_HISTORY_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

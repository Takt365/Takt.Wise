// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/maintenance
// 文件名称：notification.api.ts
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
  MaintenanceNotification,
  MaintenanceNotificationCreate,
  MaintenanceNotificationStatus,
  MaintenanceNotificationUpdate
} from '@/types/logistics/maintenance/notification.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktMaintenanceNotifications
 */
const MAINTENANCE_NOTIFICATION_API_BASE = 'TaktMaintenanceNotifications';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取维护通知单列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<MaintenanceNotification>>} 分页结果
 */
export function getMaintenanceNotificationList(queryDto: any): Promise<TaktPagedResult<MaintenanceNotification>> {
  return request<TaktPagedResult<MaintenanceNotification>>({
    url: `${MAINTENANCE_NOTIFICATION_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取维护通知单
 * @param {string} id 维护通知单ID
 * @returns {Promise<MaintenanceNotification>} 维护通知单DTO
 */
export function getMaintenanceNotificationById(id: string): Promise<MaintenanceNotification> {
  return request<MaintenanceNotification>({
    url: `${MAINTENANCE_NOTIFICATION_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建维护通知单
 * @param {MaintenanceNotificationCreate} dto 创建DTO
 * @returns {Promise<MaintenanceNotification>} 维护通知单DTO
 */
export function createMaintenanceNotification(dto: MaintenanceNotificationCreate): Promise<MaintenanceNotification> {
  return request<MaintenanceNotification>({
    url: `${MAINTENANCE_NOTIFICATION_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新维护通知单
 * @param {string} id 维护通知单ID
 * @param {MaintenanceNotificationUpdate} dto 更新DTO
 * @returns {Promise<MaintenanceNotification>} 维护通知单DTO
 */
export function updateMaintenanceNotification(id: string, dto: MaintenanceNotificationUpdate): Promise<MaintenanceNotification> {
  return request<MaintenanceNotification>({
    url: `${MAINTENANCE_NOTIFICATION_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除维护通知单
 * @param {string} id 维护通知单ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteMaintenanceNotificationById(id: string): Promise<void> {
  return request({
    url: `${MAINTENANCE_NOTIFICATION_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除维护通知单
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteMaintenanceNotificationBatch(ids: string[]): Promise<void> {
  return request({
    url: `${MAINTENANCE_NOTIFICATION_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新维护通知单状态
 * @param {MaintenanceNotificationStatus} dto 状态 DTO
 * @returns {Promise<MaintenanceNotification>} 维护通知单DTO
 */
export function updateMaintenanceNotificationStatus(dto: MaintenanceNotificationStatus): Promise<MaintenanceNotification> {
  return request<MaintenanceNotification>({
    url: `${MAINTENANCE_NOTIFICATION_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取维护通知单选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getMaintenanceNotificationOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${MAINTENANCE_NOTIFICATION_API_BASE}/options`,
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
export function getMaintenanceNotificationTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${MAINTENANCE_NOTIFICATION_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入维护通知单
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importMaintenanceNotification(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${MAINTENANCE_NOTIFICATION_API_BASE}/import`,
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
 * 导出维护通知单
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportMaintenanceNotification(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${MAINTENANCE_NOTIFICATION_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/engineering-change
// 文件名称：ecNotification.api.ts
// 创建时间：2026-06-30
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/engineering-change 模块 API（自动生成，请勿手改路由常量）
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
  EcNotification,
  EcNotificationCreate,
  EcNotificationStatus,
  EcNotificationUpdate
} from '@/types/logistics/manufacturing/engineering-change/ecNotification.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktEcNotifications
 */
const EC_NOTIFICATION_API_BASE = 'TaktEcNotifications';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取工程变更通知单列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<EcNotification>>} 分页结果
 */
export function getEcNotificationList(queryDto: any): Promise<TaktPagedResult<EcNotification>> {
  return request<TaktPagedResult<EcNotification>>({
    url: `${EC_NOTIFICATION_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取工程变更通知单
 * @param {string} id 工程变更通知单ID
 * @returns {Promise<EcNotification>} 工程变更通知单DTO
 */
export function getEcNotificationById(id: string): Promise<EcNotification> {
  return request<EcNotification>({
    url: `${EC_NOTIFICATION_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建工程变更通知单
 * @param {EcNotificationCreate} dto 创建DTO
 * @returns {Promise<EcNotification>} 工程变更通知单DTO
 */
export function createEcNotification(dto: EcNotificationCreate): Promise<EcNotification> {
  return request<EcNotification>({
    url: `${EC_NOTIFICATION_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新工程变更通知单
 * @param {string} id 工程变更通知单ID
 * @param {EcNotificationUpdate} dto 更新DTO
 * @returns {Promise<EcNotification>} 工程变更通知单DTO
 */
export function updateEcNotification(id: string, dto: EcNotificationUpdate): Promise<EcNotification> {
  return request<EcNotification>({
    url: `${EC_NOTIFICATION_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除工程变更通知单
 * @param {string} id 工程变更通知单ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcNotificationById(id: string): Promise<void> {
  return request({
    url: `${EC_NOTIFICATION_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除工程变更通知单
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteEcNotificationBatch(ids: string[]): Promise<void> {
  return request({
    url: `${EC_NOTIFICATION_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新工程变更通知单状态
 * @param {EcNotificationStatus} dto 状态 DTO
 * @returns {Promise<EcNotification>} 工程变更通知单DTO
 */
export function updateEcNotificationStatus(dto: EcNotificationStatus): Promise<EcNotification> {
  return request<EcNotification>({
    url: `${EC_NOTIFICATION_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取工程变更通知单选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getEcNotificationOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${EC_NOTIFICATION_API_BASE}/options`,
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
export function getEcNotificationTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${EC_NOTIFICATION_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入工程变更通知单
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importEcNotification(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${EC_NOTIFICATION_API_BASE}/import`,
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
 * 导出工程变更通知单
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportEcNotification(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${EC_NOTIFICATION_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

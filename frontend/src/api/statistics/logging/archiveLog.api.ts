// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/statistics/logging
// 文件名称：archiveLog.api.ts
// 创建时间：2026-07-19
// 创建人：Takt365(Auto Generated)
// 功能描述：statistics/logging 模块 API（自动生成，请勿手改路由常量）
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
  ArchiveLog,
  ArchiveLogCreate,
  ArchiveLogStatus,
  ArchiveLogUpdate
} from '@/types/statistics/logging/archiveLog.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktArchiveLogs
 */
const ARCHIVE_LOG_API_BASE = 'TaktArchiveLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取归档日志列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<ArchiveLog>>} 分页结果
 */
export function getArchiveLogList(queryDto: any): Promise<TaktPagedResult<ArchiveLog>> {
  return request<TaktPagedResult<ArchiveLog>>({
    url: `${ARCHIVE_LOG_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取归档日志
 * @param {string} id 归档日志ID
 * @returns {Promise<ArchiveLog>} 归档日志DTO
 */
export function getArchiveLogById(id: string): Promise<ArchiveLog> {
  return request<ArchiveLog>({
    url: `${ARCHIVE_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建归档日志
 * @param {ArchiveLogCreate} dto 创建DTO
 * @returns {Promise<ArchiveLog>} 归档日志DTO
 */
export function createArchiveLog(dto: ArchiveLogCreate): Promise<ArchiveLog> {
  return request<ArchiveLog>({
    url: `${ARCHIVE_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新归档日志
 * @param {string} id 归档日志ID
 * @param {ArchiveLogUpdate} dto 更新DTO
 * @returns {Promise<ArchiveLog>} 归档日志DTO
 */
export function updateArchiveLog(id: string, dto: ArchiveLogUpdate): Promise<ArchiveLog> {
  return request<ArchiveLog>({
    url: `${ARCHIVE_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除归档日志
 * @param {string} id 归档日志ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteArchiveLogById(id: string): Promise<void> {
  return request({
    url: `${ARCHIVE_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除归档日志
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteArchiveLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${ARCHIVE_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新归档日志状态
 * @param {ArchiveLogStatus} dto 状态 DTO
 * @returns {Promise<ArchiveLog>} 归档日志DTO
 */
export function updateArchiveLogStatus(dto: ArchiveLogStatus): Promise<ArchiveLog> {
  return request<ArchiveLog>({
    url: `${ARCHIVE_LOG_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取归档日志选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getArchiveLogOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${ARCHIVE_LOG_API_BASE}/options`,
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
 * 导出归档日志
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportArchiveLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${ARCHIVE_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

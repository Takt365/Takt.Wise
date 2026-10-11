// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/statistics/logging
// 文件名称：backupLog.api.ts
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
  BackupLog,
  BackupLogCreate,
  BackupLogStatus,
  BackupLogUpdate
} from '@/types/statistics/logging/backupLog.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktBackupLogs
 */
const BACKUP_LOG_API_BASE = 'TaktBackupLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取备份日志列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<BackupLog>>} 分页结果
 */
export function getBackupLogList(queryDto: any): Promise<TaktPagedResult<BackupLog>> {
  return request<TaktPagedResult<BackupLog>>({
    url: `${BACKUP_LOG_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取备份日志
 * @param {string} id 备份日志ID
 * @returns {Promise<BackupLog>} 备份日志DTO
 */
export function getBackupLogById(id: string): Promise<BackupLog> {
  return request<BackupLog>({
    url: `${BACKUP_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建备份日志
 * @param {BackupLogCreate} dto 创建DTO
 * @returns {Promise<BackupLog>} 备份日志DTO
 */
export function createBackupLog(dto: BackupLogCreate): Promise<BackupLog> {
  return request<BackupLog>({
    url: `${BACKUP_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新备份日志
 * @param {string} id 备份日志ID
 * @param {BackupLogUpdate} dto 更新DTO
 * @returns {Promise<BackupLog>} 备份日志DTO
 */
export function updateBackupLog(id: string, dto: BackupLogUpdate): Promise<BackupLog> {
  return request<BackupLog>({
    url: `${BACKUP_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除备份日志
 * @param {string} id 备份日志ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteBackupLogById(id: string): Promise<void> {
  return request({
    url: `${BACKUP_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除备份日志
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteBackupLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${BACKUP_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新备份日志状态
 * @param {BackupLogStatus} dto 状态 DTO
 * @returns {Promise<BackupLog>} 备份日志DTO
 */
export function updateBackupLogStatus(dto: BackupLogStatus): Promise<BackupLog> {
  return request<BackupLog>({
    url: `${BACKUP_LOG_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取备份日志选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getBackupLogOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${BACKUP_LOG_API_BASE}/options`,
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
 * 导出备份日志
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportBackupLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${BACKUP_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

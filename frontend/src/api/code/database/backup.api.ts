// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/code/database
// 文件名称：backup.api.ts
// 创建时间：2026-07-19
// 创建人：Takt365(Auto Generated)
// 功能描述：code/database 模块 API（自动生成，请勿手改路由常量）
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
  DatabaseBackup,
  DatabaseBackupCreate,
  DatabaseBackupStatus,
  DatabaseBackupUpdate
} from '@/types/code/database/backup.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktDatabaseBackups
 */
const DATABASE_BACKUP_API_BASE = 'TaktDatabaseBackups';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取数据库备份列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<DatabaseBackup>>} 分页结果
 */
export function getDatabaseBackupList(queryDto: any): Promise<TaktPagedResult<DatabaseBackup>> {
  return request<TaktPagedResult<DatabaseBackup>>({
    url: `${DATABASE_BACKUP_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取数据库备份
 * @param {string} id 数据库备份ID
 * @returns {Promise<DatabaseBackup>} 数据库备份DTO
 */
export function getDatabaseBackupById(id: string): Promise<DatabaseBackup> {
  return request<DatabaseBackup>({
    url: `${DATABASE_BACKUP_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建数据库备份
 * @param {DatabaseBackupCreate} dto 创建DTO
 * @returns {Promise<DatabaseBackup>} 数据库备份DTO
 */
export function createDatabaseBackup(dto: DatabaseBackupCreate): Promise<DatabaseBackup> {
  return request<DatabaseBackup>({
    url: `${DATABASE_BACKUP_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新数据库备份
 * @param {string} id 数据库备份ID
 * @param {DatabaseBackupUpdate} dto 更新DTO
 * @returns {Promise<DatabaseBackup>} 数据库备份DTO
 */
export function updateDatabaseBackup(id: string, dto: DatabaseBackupUpdate): Promise<DatabaseBackup> {
  return request<DatabaseBackup>({
    url: `${DATABASE_BACKUP_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除数据库备份
 * @param {string} id 数据库备份ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteDatabaseBackupById(id: string): Promise<void> {
  return request({
    url: `${DATABASE_BACKUP_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除数据库备份
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteDatabaseBackupBatch(ids: string[]): Promise<void> {
  return request({
    url: `${DATABASE_BACKUP_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新数据库备份状态
 * @param {DatabaseBackupStatus} dto 状态 DTO
 * @returns {Promise<DatabaseBackup>} 数据库备份DTO
 */
export function updateDatabaseBackupStatus(dto: DatabaseBackupStatus): Promise<DatabaseBackup> {
  return request<DatabaseBackup>({
    url: `${DATABASE_BACKUP_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取数据库备份选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getDatabaseBackupOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${DATABASE_BACKUP_API_BASE}/options`,
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
export function getDatabaseBackupTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${DATABASE_BACKUP_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入数据库备份
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importDatabaseBackup(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${DATABASE_BACKUP_API_BASE}/import`,
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
 * 导出数据库备份
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportDatabaseBackup(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${DATABASE_BACKUP_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/code/database
// 文件名称：databaseBackup.api.ts
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：数据库备份 API（CRUD、路径选项、按 Id 立即/调度执行）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request'
import type { TaktPagedResult } from '@/types/common.types'
import type {
  DatabaseBackup,
  DatabaseBackupBrowseResult,
  DatabaseBackupCreate,
  DatabaseBackupPathOptions,
  DatabaseBackupQuery,
  DatabaseBackupScheduleByIdDto,
  DatabaseBackupUpdate,
} from '@/types/code/database/backup.types'

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktDatabaseBackups
 */
const DATABASE_BACKUP_API_BASE = 'TaktDatabaseBackups'

/**
 * 分页列表
 * @param query 查询
 * @returns 分页结果
 */
export function getDatabaseBackupList(
  query: DatabaseBackupQuery
): Promise<TaktPagedResult<DatabaseBackup>> {
  return request<TaktPagedResult<DatabaseBackup>>({
    url: `${DATABASE_BACKUP_API_BASE}/list`,
    method: 'get',
    params: query,
  })
}

/**
 * 详情
 * @param id 主键
 * @returns 备份记录
 */
export function getDatabaseBackupById(id: string): Promise<DatabaseBackup> {
  return request<DatabaseBackup>({
    url: `${DATABASE_BACKUP_API_BASE}/${id}`,
    method: 'get',
  })
}

/**
 * 新增备份配置
 * @param dto 创建 DTO
 * @returns 备份记录
 */
export function createDatabaseBackup(dto: DatabaseBackupCreate): Promise<DatabaseBackup> {
  return request<DatabaseBackup>({
    url: `${DATABASE_BACKUP_API_BASE}`,
    method: 'post',
    data: dto,
  })
}

/**
 * 更新备份配置
 * @param id 主键
 * @param dto 更新 DTO
 * @returns 备份记录
 */
export function updateDatabaseBackup(id: string, dto: DatabaseBackupUpdate): Promise<DatabaseBackup> {
  return request<DatabaseBackup>({
    url: `${DATABASE_BACKUP_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  })
}

/**
 * 删除记录
 * @param id 主键
 * @returns void
 */
export function deleteDatabaseBackupById(id: string): Promise<void> {
  return request<void>({
    url: `${DATABASE_BACKUP_API_BASE}/${id}`,
    method: 'delete',
  })
}

/**
 * 批量删除
 * @param ids 主键列表
 * @returns void
 */
export function deleteDatabaseBackupBatch(ids: string[]): Promise<void> {
  return request<void>({
    url: `${DATABASE_BACKUP_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  })
}

/**
 * 导出备份记录
 * @param query 查询条件
 * @param sheetName Excel sheet 名
 * @param exportName 导出文件名前缀
 * @returns Excel blob
 */
export function exportDatabaseBackup(
  query?: DatabaseBackupQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${DATABASE_BACKUP_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName,
    },
    responseType: 'blob',
  })
}

/**
 * 允许的备份路径
 * @returns 路径选项
 */
export function getDatabaseBackupPathOptions(): Promise<DatabaseBackupPathOptions> {
  return request<DatabaseBackupPathOptions>({
    url: `${DATABASE_BACKUP_API_BASE}/path-options`,
    method: 'get',
  })
}

/**
 * 浏览本地目录
 * @param dto 当前路径
 * @returns 浏览结果
 */
export function browseDatabaseBackupLocal(dto: {
  currentPath?: string
}): Promise<DatabaseBackupBrowseResult> {
  return request<DatabaseBackupBrowseResult>({
    url: `${DATABASE_BACKUP_API_BASE}/browse/local`,
    method: 'post',
    data: dto,
  })
}

/**
 * 在 API 宿主创建本地目录（任意路径）
 * @param dto 目录路径
 * @returns 创建后的完整路径
 */
export function createDatabaseBackupLocalDirectory(dto: {
  path: string
}): Promise<{ path: string }> {
  return request<{ path: string }>({
    url: `${DATABASE_BACKUP_API_BASE}/mkdir/local`,
    method: 'post',
    data: dto,
  })
}

/**
 * 浏览网络 UNC 目录
 * @param dto 路径与可选凭据
 * @returns 浏览结果
 */
export function browseDatabaseBackupNetwork(dto: {
  path: string
  userName?: string
  password?: string
  databaseBackupId?: string
}): Promise<DatabaseBackupBrowseResult> {
  return request<DatabaseBackupBrowseResult>({
    url: `${DATABASE_BACKUP_API_BASE}/browse/network`,
    method: 'post',
    data: dto,
  })
}

/**
 * 创建网络 UNC 目录
 * @param dto UNC 与凭据
 * @returns 创建后的路径
 */
export function createDatabaseBackupNetworkDirectory(dto: {
  path: string
  userName?: string
  password?: string
  databaseBackupId?: string
}): Promise<{ path: string }> {
  return request<{ path: string }>({
    url: `${DATABASE_BACKUP_API_BASE}/mkdir/network`,
    method: 'post',
    data: dto,
  })
}

/**
 * 浏览 FTP 目录
 * @param dto FTP 连接与路径
 * @returns 浏览结果
 */
export function browseDatabaseBackupFtp(dto: {
  host: string
  port?: number
  path?: string
  userName: string
  password?: string
  databaseBackupId?: string
}): Promise<DatabaseBackupBrowseResult> {
  return request<DatabaseBackupBrowseResult>({
    url: `${DATABASE_BACKUP_API_BASE}/browse/ftp`,
    method: 'post',
    data: dto,
  })
}

/**
 * 创建 FTP 远程目录
 * @param dto FTP 连接与路径
 * @returns 创建后的远程路径
 */
export function createDatabaseBackupFtpDirectory(dto: {
  host: string
  port?: number
  path?: string
  userName: string
  password?: string
  databaseBackupId?: string
}): Promise<{ path: string }> {
  return request<{ path: string }>({
    url: `${DATABASE_BACKUP_API_BASE}/mkdir/ftp`,
    method: 'post',
    data: dto,
  })
}

/**
 * 立即执行备份（按记录 Id 触发 Quartz）
 * @param id 备份记录主键
 * @returns 备份记录
 */
export function runDatabaseBackupById(id: string): Promise<DatabaseBackup> {
  return request<DatabaseBackup>({
    url: `${DATABASE_BACKUP_API_BASE}/${id}/run`,
    method: 'post',
  })
}

/**
 * 后台调度备份（按记录 Id 创建 Quartz 任务）
 * @param id 备份记录主键
 * @param dto 调度时间
 * @returns 备份记录
 */
export function scheduleDatabaseBackupById(
  id: string,
  dto: DatabaseBackupScheduleByIdDto
): Promise<DatabaseBackup> {
  return request<DatabaseBackup>({
    url: `${DATABASE_BACKUP_API_BASE}/${id}/schedule`,
    method: 'post',
    data: dto,
  })
}

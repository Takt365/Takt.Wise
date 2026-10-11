// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/code/database
// 文件名称：tableArchive.api.ts
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
  TableArchive,
  TableArchiveCreate,
  TableArchiveSort,
  TableArchiveStatus,
  TableArchiveUpdate,
  TableArchiveExecuteDto,
  TableArchiveExecuteResult,
  TableArchivePreviewResult,
  TableArchiveScheduleDto,
  TableArchiveScheduleResult,
  TableEnsureYearTablesDto,
  TableEnsureYearTablesResult
} from '@/types/code/database/tableArchive.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktTableArchives
 */
const TABLE_ARCHIVE_API_BASE = 'TaktTableArchives';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取数据表归档列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<TableArchive>>} 分页结果
 */
export function getTableArchiveList(queryDto: any): Promise<TaktPagedResult<TableArchive>> {
  return request<TaktPagedResult<TableArchive>>({
    url: `${TABLE_ARCHIVE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取数据表归档
 * @param {string} id 数据表归档ID
 * @returns {Promise<TableArchive>} 数据表归档DTO
 */
export function getTableArchiveById(id: string): Promise<TableArchive> {
  return request<TableArchive>({
    url: `${TABLE_ARCHIVE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建数据表归档
 * @param {TableArchiveCreate} dto 创建DTO
 * @returns {Promise<TableArchive>} 数据表归档DTO
 */
export function createTableArchive(dto: TableArchiveCreate): Promise<TableArchive> {
  return request<TableArchive>({
    url: `${TABLE_ARCHIVE_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新数据表归档
 * @param {string} id 数据表归档ID
 * @param {TableArchiveUpdate} dto 更新DTO
 * @returns {Promise<TableArchive>} 数据表归档DTO
 */
export function updateTableArchive(id: string, dto: TableArchiveUpdate): Promise<TableArchive> {
  return request<TableArchive>({
    url: `${TABLE_ARCHIVE_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除数据表归档
 * @param {string} id 数据表归档ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteTableArchiveById(id: string): Promise<void> {
  return request({
    url: `${TABLE_ARCHIVE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除数据表归档
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteTableArchiveBatch(ids: string[]): Promise<void> {
  return request({
    url: `${TABLE_ARCHIVE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新数据表归档状态
 * @param {TableArchiveStatus} dto 状态 DTO
 * @returns {Promise<TableArchive>} 数据表归档DTO
 */
export function updateTableArchiveStatus(dto: TableArchiveStatus): Promise<TableArchive> {
  return request<TableArchive>({
    url: `${TABLE_ARCHIVE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

/**
 * 更新数据表归档排序
 * @param {TableArchiveSort} dto 排序DTO
 * @returns {Promise<TableArchive>} 数据表归档DTO
 */
export function updateTableArchiveSort(dto: TableArchiveSort): Promise<TableArchive> {
  return request<TableArchive>({
    url: `${TABLE_ARCHIVE_API_BASE}/sort`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取数据表归档选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getTableArchiveOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${TABLE_ARCHIVE_API_BASE}/options`,
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
export function getTableArchiveTemplate(sheetName?: string, templateName?: string): Promise<Blob> {
  return request<Blob>({
    url: `${TABLE_ARCHIVE_API_BASE}/template`,
    method: 'get',
    params: {
      sheetName,
      templateName
    },
    responseType: 'blob',
  });
}

/**
 * 导入数据表归档
 * @param {globalThis.File} file Excel文件
 * @param {string} sheetName sheetName
 * @returns {Promise<{ success: number; fail: number; errors: string[] }>} 导入结果
 */
export function importTableArchive(file: globalThis.File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  const formData = new FormData();
  formData.append('file', file);
  
  return request({
    url: `${TABLE_ARCHIVE_API_BASE}/import`,
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
 * 导出数据表归档
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportTableArchive(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${TABLE_ARCHIVE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

// ========================================
// 数据归档编排
// ========================================

/**
 * 预览按年归档行数
 * @param {TableArchiveExecuteDto} dto 归档请求
 * @returns {Promise<TableArchivePreviewResult>} 预览结果
 */
export function previewTableArchive(dto: TableArchiveExecuteDto): Promise<TableArchivePreviewResult> {
  return request<TableArchivePreviewResult>({
    url: `${TABLE_ARCHIVE_API_BASE}/archive/preview`,
    method: 'post',
    data: dto,
  });
}

/**
 * 执行按年归档（同步；一般由 Quartz Job 调用，页面优先用 run-now / schedule）
 * @param {TableArchiveExecuteDto} dto 归档请求
 * @returns {Promise<TableArchiveExecuteResult>} 执行结果
 */
export function executeTableArchive(dto: TableArchiveExecuteDto): Promise<TableArchiveExecuteResult> {
  return request<TableArchiveExecuteResult>({
    url: `${TABLE_ARCHIVE_API_BASE}/archive/execute`,
    method: 'post',
    data: dto,
  });
}

/**
 * 立即归档（创建一次性 Quartz 任务）
 * @param {TableArchiveScheduleDto} dto 归档请求
 * @returns {Promise<TableArchiveScheduleResult>} 调度结果
 */
export function runTableArchiveNow(dto: TableArchiveScheduleDto): Promise<TableArchiveScheduleResult> {
  return request<TableArchiveScheduleResult>({
    url: `${TABLE_ARCHIVE_API_BASE}/archive/run-now`,
    method: 'post',
    data: dto,
  });
}

/**
 * 后台归档（创建一次性 Quartz 任务）
 * @param {TableArchiveScheduleDto} dto 归档请求（须含 scheduledAt）
 * @returns {Promise<TableArchiveScheduleResult>} 调度结果
 */
export function scheduleTableArchive(dto: TableArchiveScheduleDto): Promise<TableArchiveScheduleResult> {
  return request<TableArchiveScheduleResult>({
    url: `${TABLE_ARCHIVE_API_BASE}/archive/schedule`,
    method: 'post',
    data: dto,
  });
}

/**
 * 预建年分表
 * @param {TableEnsureYearTablesDto} dto 建表请求
 * @returns {Promise<TableEnsureYearTablesResult>} 建表结果
 */
export function ensureYearTables(dto: TableEnsureYearTablesDto): Promise<TableEnsureYearTablesResult> {
  return request<TableEnsureYearTablesResult>({
    url: `${TABLE_ARCHIVE_API_BASE}/archive/ensure-year-tables`,
    method: 'post',
    data: dto,
  });
}

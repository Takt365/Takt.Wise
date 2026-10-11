// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/statistics/logging
// 文件名称：durationLog.api.ts
// 创建时间：2026-06-25
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
  DurationLog,
  DurationLogCreate,
  DurationLogUpdate
} from '@/types/statistics/logging/durationLog.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktDurationLogs
 */
const DURATION_LOG_API_BASE = 'TaktDurationLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取在线时长日志列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<DurationLog>>} 分页结果
 */
export function getDurationLogList(queryDto: any): Promise<TaktPagedResult<DurationLog>> {
  return request<TaktPagedResult<DurationLog>>({
    url: `${DURATION_LOG_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取在线时长日志
 * @param {string} id 在线时长日志ID
 * @returns {Promise<DurationLog>} 在线时长日志DTO
 */
export function getDurationLogById(id: string): Promise<DurationLog> {
  return request<DurationLog>({
    url: `${DURATION_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建在线时长日志
 * @param {DurationLogCreate} dto 创建DTO
 * @returns {Promise<DurationLog>} 在线时长日志DTO
 */
export function createDurationLog(dto: DurationLogCreate): Promise<DurationLog> {
  return request<DurationLog>({
    url: `${DURATION_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新在线时长日志
 * @param {string} id 在线时长日志ID
 * @param {DurationLogUpdate} dto 更新DTO
 * @returns {Promise<DurationLog>} 在线时长日志DTO
 */
export function updateDurationLog(id: string, dto: DurationLogUpdate): Promise<DurationLog> {
  return request<DurationLog>({
    url: `${DURATION_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除在线时长日志
 * @param {string} id 在线时长日志ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteDurationLogById(id: string): Promise<void> {
  return request({
    url: `${DURATION_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除在线时长日志
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteDurationLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${DURATION_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取在线时长日志选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getDurationLogOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${DURATION_LOG_API_BASE}/options`,
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
 * 导出在线时长日志
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportDurationLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${DURATION_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

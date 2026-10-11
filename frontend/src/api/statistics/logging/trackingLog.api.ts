// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/statistics/logging
// 文件名称：trackingLog.api.ts
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
  TrackingLog,
  TrackingLogBatchTrack,
  TrackingLogCreate,
  TrackingLogTrackResult,
  TrackingLogUpdate
} from '@/types/statistics/logging/trackingLog.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktTrackingLogs
 */
const TRACKING_LOG_API_BASE = 'TaktTrackingLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取交互日志列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<TrackingLog>>} 分页结果
 */
export function getTrackingLogList(queryDto: any): Promise<TaktPagedResult<TrackingLog>> {
  return request<TaktPagedResult<TrackingLog>>({
    url: `${TRACKING_LOG_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取交互日志
 * @param {string} id 交互日志ID
 * @returns {Promise<TrackingLog>} 交互日志DTO
 */
export function getTrackingLogById(id: string): Promise<TrackingLog> {
  return request<TrackingLog>({
    url: `${TRACKING_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建交互日志
 * @param {TrackingLogCreate} dto 创建DTO
 * @returns {Promise<TrackingLog>} 交互日志DTO
 */
export function createTrackingLog(dto: TrackingLogCreate): Promise<TrackingLog> {
  return request<TrackingLog>({
    url: `${TRACKING_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新交互日志
 * @param {string} id 交互日志ID
 * @param {TrackingLogUpdate} dto 更新DTO
 * @returns {Promise<TrackingLog>} 交互日志DTO
 */
export function updateTrackingLog(id: string, dto: TrackingLogUpdate): Promise<TrackingLog> {
  return request<TrackingLog>({
    url: `${TRACKING_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除交互日志
 * @param {string} id 交互日志ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteTrackingLogById(id: string): Promise<void> {
  return request({
    url: `${TRACKING_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除交互日志
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteTrackingLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${TRACKING_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取交互日志选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getTrackingLogOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${TRACKING_LOG_API_BASE}/options`,
    method: 'get',
    params: {
      
      ...(plant ? { plantCode: plant } : {}),
      ...(kw ? { keyword: kw } : {}),
    },
  });
}

// ========================================
// Long Task 客户端上报
// ========================================

/**
 * 批量上报 Long Task 等客户端性能事件
 * @param {TrackingLogBatchTrack} dto 批量上报 DTO
 * @returns {Promise<TrackingLogTrackResult>} 写入条数
 */
export function trackTrackingLogBatch(dto: TrackingLogBatchTrack): Promise<TrackingLogTrackResult> {
  return request<TrackingLogTrackResult>({
    url: `${TRACKING_LOG_API_BASE}/track-batch`,
    method: 'post',
    data: dto,
    skipApiPerformanceTrack: true,
  });
}

// ========================================
// 导入导出
// ========================================

/**
 * 导出交互日志
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportTrackingLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${TRACKING_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

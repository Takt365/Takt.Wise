// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/statistics/logging
// 文件名称：quartzLog.api.ts
// 创建时间：2026-08-11
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
  QuartzLog,
  QuartzLogCreate,
  QuartzLogStatus,
  QuartzLogUpdate
} from '@/types/statistics/logging/quartzLog.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktQuartzLogs
 */
const QUARTZ_LOG_API_BASE = 'TaktQuartzLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取任务执行日志列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<QuartzLog>>} 分页结果
 */
export function getQuartzLogList(queryDto: any): Promise<TaktPagedResult<QuartzLog>> {
  return request<TaktPagedResult<QuartzLog>>({
    url: `${QUARTZ_LOG_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取任务执行日志
 * @param {string} id 任务执行日志ID
 * @returns {Promise<QuartzLog>} 任务执行日志DTO
 */
export function getQuartzLogById(id: string): Promise<QuartzLog> {
  return request<QuartzLog>({
    url: `${QUARTZ_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建任务执行日志
 * @param {QuartzLogCreate} dto 创建DTO
 * @returns {Promise<QuartzLog>} 任务执行日志DTO
 */
export function createQuartzLog(dto: QuartzLogCreate): Promise<QuartzLog> {
  return request<QuartzLog>({
    url: `${QUARTZ_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新任务执行日志
 * @param {string} id 任务执行日志ID
 * @param {QuartzLogUpdate} dto 更新DTO
 * @returns {Promise<QuartzLog>} 任务执行日志DTO
 */
export function updateQuartzLog(id: string, dto: QuartzLogUpdate): Promise<QuartzLog> {
  return request<QuartzLog>({
    url: `${QUARTZ_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除任务执行日志
 * @param {string} id 任务执行日志ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteQuartzLogById(id: string): Promise<void> {
  return request({
    url: `${QUARTZ_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除任务执行日志
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteQuartzLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${QUARTZ_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新任务执行日志状态
 * @param {QuartzLogStatus} dto 状态 DTO（TaktExecuteStatus 枚举）
 * @returns {Promise<QuartzLog>} 任务执行日志DTO
 */
export function updateQuartzLogStatus(dto: QuartzLogStatus): Promise<QuartzLog> {
  return request<QuartzLog>({
    url: `${QUARTZ_LOG_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取任务执行日志选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getQuartzLogOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${QUARTZ_LOG_API_BASE}/options`,
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
 * 导出任务执行日志
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportQuartzLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${QUARTZ_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

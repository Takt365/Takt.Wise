// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/statistics/logging
// 文件名称：visitLog.api.ts
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
  VisitLog,
  VisitLogCreate,
  VisitLogUpdate
} from '@/types/statistics/logging/visitLog.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktVisitLogs
 */
const VISIT_LOG_API_BASE = 'TaktVisitLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取用户日访问量列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<VisitLog>>} 分页结果
 */
export function getVisitLogList(queryDto: any): Promise<TaktPagedResult<VisitLog>> {
  return request<TaktPagedResult<VisitLog>>({
    url: `${VISIT_LOG_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取用户日访问量
 * @param {string} id 用户日访问量ID
 * @returns {Promise<VisitLog>} 用户日访问量DTO
 */
export function getVisitLogById(id: string): Promise<VisitLog> {
  return request<VisitLog>({
    url: `${VISIT_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建用户日访问量
 * @param {VisitLogCreate} dto 创建DTO
 * @returns {Promise<VisitLog>} 用户日访问量DTO
 */
export function createVisitLog(dto: VisitLogCreate): Promise<VisitLog> {
  return request<VisitLog>({
    url: `${VISIT_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新用户日访问量
 * @param {string} id 用户日访问量ID
 * @param {VisitLogUpdate} dto 更新DTO
 * @returns {Promise<VisitLog>} 用户日访问量DTO
 */
export function updateVisitLog(id: string, dto: VisitLogUpdate): Promise<VisitLog> {
  return request<VisitLog>({
    url: `${VISIT_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除用户日访问量
 * @param {string} id 用户日访问量ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteVisitLogById(id: string): Promise<void> {
  return request({
    url: `${VISIT_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除用户日访问量
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteVisitLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${VISIT_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取用户日访问量选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getVisitLogOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${VISIT_LOG_API_BASE}/options`,
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
 * 导出用户日访问量
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportVisitLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${VISIT_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

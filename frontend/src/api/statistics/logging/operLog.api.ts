// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/statistics/logging
// 文件名称：operLog.api.ts
// 创建时间：2026-06-12
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
  OperLog,
  OperLogCreate,
  OperLogStatus,
  OperLogUpdate
} from '@/types/statistics/logging/operLog.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktOperLogs
 */
const OPER_LOG_API_BASE = 'TaktOperLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取操作日志列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<OperLog>>} 分页结果
 */
export function getOperLogList(queryDto: any): Promise<TaktPagedResult<OperLog>> {
  return request<TaktPagedResult<OperLog>>({
    url: `${OPER_LOG_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 获取当前登录用户的操作日志（分页；个人中心，无需管理端 list 权限）
 * @param query 与 list 相同的查询形态（pageIndex/pageSize/keyWords/高级字段；UserName 由后端强制）
 * @returns {Promise<TaktPagedResult<OperLog>>} 分页结果
 */
export function getMyOperLogList(query: {
  pageIndex: number;
  pageSize: number;
  keyWords?: string;
  operModule?: string;
  operType?: string;
  operMethod?: string;
  requestMethod?: string;
  operUrl?: string;
  requestParam?: string;
  jsonResult?: string;
  operStatus?: number;
  errorMsg?: string;
  operIp?: string;
  operLocation?: string;
  operTimeStart?: string;
  operTimeEnd?: string;
  elapsedTime?: number;
  createdAtStart?: string;
  createdAtEnd?: string;
  ExtField?: string;
  remark?: string;
}): Promise<TaktPagedResult<OperLog>> {
  return request<TaktPagedResult<OperLog>>({
    url: `${OPER_LOG_API_BASE}/me/list`,
    method: 'get',
    params: query,
  });
}

/**
 * 根据ID获取操作日志
 * @param {string} id 操作日志ID
 * @returns {Promise<OperLog>} 操作日志DTO
 */
export function getOperLogById(id: string): Promise<OperLog> {
  return request<OperLog>({
    url: `${OPER_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建操作日志
 * @param {OperLogCreate} dto 创建DTO
 * @returns {Promise<OperLog>} 操作日志DTO
 */
export function createOperLog(dto: OperLogCreate): Promise<OperLog> {
  return request<OperLog>({
    url: `${OPER_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新操作日志
 * @param {string} id 操作日志ID
 * @param {OperLogUpdate} dto 更新DTO
 * @returns {Promise<OperLog>} 操作日志DTO
 */
export function updateOperLog(id: string, dto: OperLogUpdate): Promise<OperLog> {
  return request<OperLog>({
    url: `${OPER_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除操作日志
 * @param {string} id 操作日志ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteOperLogById(id: string): Promise<void> {
  return request({
    url: `${OPER_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除操作日志
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteOperLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${OPER_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新操作日志状态
 * @param {OperLogStatus} dto 状态 DTO（TaktExecuteStatus 枚举）
 * @returns {Promise<OperLog>} 操作日志DTO
 */
export function updateOperLogStatus(dto: OperLogStatus): Promise<OperLog> {
  return request<OperLog>({
    url: `${OPER_LOG_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取操作日志选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getOperLogOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${OPER_LOG_API_BASE}/options`,
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
 * 导出操作日志
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportOperLog(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${OPER_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

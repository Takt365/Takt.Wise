// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/statistics/logging
// 文件名称：loginLog.api.ts
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
  LoginLog,
  LoginLogCreate,
  LoginLogUpdate,
} from '@/types/statistics/logging/loginLog.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktLoginLogs
 */
const LOGIN_LOG_API_BASE = 'TaktLoginLogs';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取登录日志列表（分页）
 * @param {any} queryDto 查询DTO
 * @returns {Promise<TaktPagedResult<LoginLog>>} 分页结果
 */
export function getLoginLogList(queryDto: any): Promise<TaktPagedResult<LoginLog>> {
  return request<TaktPagedResult<LoginLog>>({
    url: `${LOGIN_LOG_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 获取当前登录用户的登录日志（分页；个人中心，无需管理端 list 权限）
 * @param query 与 list 相同的查询形态（pageIndex/pageSize/keyWords/高级字段；UserName 由后端强制）
 * @returns {Promise<TaktPagedResult<LoginLog>>} 分页结果
 */
export function getMyLoginLogList(query: {
  pageIndex: number;
  pageSize: number;
  keyWords?: string;
  loginType?: string;
  browser?: string;
  os?: string;
  userAgent?: string;
  loginResult?: string;
  loginMessage?: string;
  loginIp?: string;
  loginLocation?: string;
  logoutAtStart?: string;
  logoutAtEnd?: string;
  createdAtStart?: string;
  createdAtEnd?: string;
  ExtField?: string;
  remark?: string;
}): Promise<TaktPagedResult<LoginLog>> {
  return request<TaktPagedResult<LoginLog>>({
    url: `${LOGIN_LOG_API_BASE}/me/list`,
    method: 'get',
    params: query,
  });
}

/**
 * 根据ID获取登录日志
 * @param {string} id 登录日志ID
 * @returns {Promise<LoginLog>} 登录日志DTO
 */
export function getLoginLogById(id: string): Promise<LoginLog> {
  return request<LoginLog>({
    url: `${LOGIN_LOG_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 创建登录日志
 * @param {LoginLogCreate} dto 创建DTO
 * @returns {Promise<LoginLog>} 登录日志DTO
 */
export function createLoginLog(dto: LoginLogCreate): Promise<LoginLog> {
  return request<LoginLog>({
    url: `${LOGIN_LOG_API_BASE}`,
    method: 'post',
    data: dto,
  });
}

/**
 * 更新登录日志
 * @param {string} id 登录日志ID
 * @param {LoginLogUpdate} dto 更新DTO
 * @returns {Promise<LoginLog>} 登录日志DTO
 */
export function updateLoginLog(id: string, dto: LoginLogUpdate): Promise<LoginLog> {
  return request<LoginLog>({
    url: `${LOGIN_LOG_API_BASE}/${id}`,
    method: 'put',
    data: dto,
  });
}

/**
 * 删除登录日志
 * @param {string} id 登录日志ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteLoginLogById(id: string): Promise<void> {
  return request({
    url: `${LOGIN_LOG_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除登录日志
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteLoginLogBatch(ids: string[]): Promise<void> {
  return request({
    url: `${LOGIN_LOG_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取登录日志选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getLoginLogOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${LOGIN_LOG_API_BASE}/options`,
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
 * 导出登录日志
 * @param {any} query query
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportLoginLogData(
  query?: any,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${LOGIN_LOG_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}

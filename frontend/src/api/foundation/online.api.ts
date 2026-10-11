// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/foundation
// 文件名称：online.api.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Auto Generated)
// 功能描述：在线用户 CRUD + SignalR 强退/统计推送 API
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
  Online,
  OnlineBroadcastPush,
  OnlineForceKick,
  OnlineForceKickBatch,
  OnlinePushStatisticsRequest,
  OnlineQuery,
  OnlineDashboardStatistics,
  OnlineStatistics,
  OnlineStatus,
} from '@/types/foundation/online.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktOnline
 */
const ONLINE_API_BASE = 'TaktOnlines';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取在线用户列表（分页）
 * @param {OnlineQuery} queryDto 查询参数
 * @returns {Promise<TaktPagedResult<Online>>} 分页结果
 */
export function getOnlineList(queryDto: OnlineQuery): Promise<TaktPagedResult<Online>> {
  return request<TaktPagedResult<Online>>({
    url: `${ONLINE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 根据ID获取在线用户
 * @param {string} id 在线用户ID
 * @returns {Promise<Online>} 在线用户
 */
export function getOnlineById(id: string): Promise<Online> {
  return request<Online>({
    url: `${ONLINE_API_BASE}/${id}`,
    method: 'get',
  });
}

/**
 * 删除在线用户
 * @param {string} id 在线用户ID
 * @returns {Promise<void>} 操作结果
 */
export function deleteOnlineById(id: string): Promise<void> {
  return request({
    url: `${ONLINE_API_BASE}/${id}`,
    method: 'delete',
  });
}

/**
 * 批量删除在线用户
 * @param {string[]} ids ID列表
 * @returns {Promise<void>} 操作结果
 */
export function deleteOnlineBatch(ids: string[]): Promise<void> {
  return request({
    url: `${ONLINE_API_BASE}/batch`,
    method: 'delete',
    data: ids,
  });
}

/**
 * 更新在线用户状态
 * @param {OnlineStatus} dto 状态参数
 * @returns {Promise<Online>} 在线用户
 */
export function updateOnlineStatus(dto: OnlineStatus): Promise<Online> {
  return request<Online>({
    url: `${ONLINE_API_BASE}/status`,
    method: 'put',
    data: dto,
  });
}

// ========================================
// 选项
// ========================================

/**
 * 获取在线用户选项列表
 * @returns {Promise<TaktSelectOption[]>} 下拉选项
 */
export function getOnlineOptions(
  plantCode?: string,
  keyword?: string
): Promise<TaktSelectOption[]> {
  const plant = plantCode?.trim()
  const kw = keyword?.trim()
  return request<TaktSelectOption[]>({
    url: `${ONLINE_API_BASE}/options`,
    method: 'get',
    params: {
      
      ...(plant ? { plantCode: plant } : {}),
      ...(kw ? { keyword: kw } : {}),
    },
  });
}

// ========================================
// 统计
// ========================================

/**
 * 在线统计查询参数
 */
export interface OnlineStatisticsQuery {
  /** 用户名（可选；为空取当前用户） */
  userName?: string;
}

/**
 * 获取在线用户统计
 * @param {OnlineStatisticsQuery} [queryDto] 查询参数
 * @returns {Promise<OnlineStatistics>} 统计结果
 */
export function getOnlineStatistics(queryDto?: OnlineStatisticsQuery): Promise<OnlineStatistics> {
  return request<OnlineStatistics>({
    url: `${ONLINE_API_BASE}/statistics`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 获取在线看板统计（公司维度：在线人数、当日总访问量、当前会话）
 * @returns {Promise<OnlineDashboardStatistics>} 看板统计结果
 */
export function getOnlineDashboardStatistics(): Promise<OnlineDashboardStatistics> {
  return request<OnlineDashboardStatistics>({
    url: `${ONLINE_API_BASE}/statistics/dashboard`,
    method: 'get',
  });
}

// ========================================
// SignalR 推送调度
// ========================================

/**
 * 强制踢出在线用户（强退）
 * @param {string} onlineId 在线用户 ID
 * @param {OnlineForceKick} [dto] 强退参数
 * @returns {Promise<void>} 操作结果
 */
export function forceKickOnlineById(onlineId: string, dto?: OnlineForceKick): Promise<void> {
  return request({
    url: `${ONLINE_API_BASE}/${onlineId}/force-kick`,
    method: 'post',
    data: dto ?? {},
  });
}

/**
 * 批量强制踢出在线用户
 * @param {OnlineForceKickBatch} dto 批量强退参数
 * @returns {Promise<void>} 操作结果
 */
export function forceKickOnlineBatch(dto: OnlineForceKickBatch): Promise<void> {
  return request({
    url: `${ONLINE_API_BASE}/force-kick/batch`,
    method: 'post',
    data: dto,
  });
}

/**
 * 向在线用户广播消息
 * @param {OnlineBroadcastPush} dto 广播内容
 * @returns {Promise<void>} 操作结果
 */
export function pushBroadcastMessage(dto: OnlineBroadcastPush): Promise<void> {
  return request({
    url: `${ONLINE_API_BASE}/messages/broadcast`,
    method: 'post',
    data: dto,
  });
}

/**
 * 向指定用户推送最新在线统计
 * @param {OnlinePushStatisticsRequest} dto 目标用户
 * @returns {Promise<void>} 操作结果
 */
export function pushOnlineStatistics(dto: OnlinePushStatisticsRequest): Promise<void> {
  return request({
    url: `${ONLINE_API_BASE}/statistics/online/push`,
    method: 'post',
    data: dto,
  });
}

/**
 * 向指定用户推送最新消息统计
 * @param {OnlinePushStatisticsRequest} dto 目标用户
 * @returns {Promise<void>} 操作结果
 */
export function pushMessageStatistics(dto: OnlinePushStatisticsRequest): Promise<void> {
  return request({
    url: `${ONLINE_API_BASE}/statistics/message/push`,
    method: 'post',
    data: dto,
  });
}

// ========================================
// 导出
// ========================================

/**
 * 导出在线用户
 * @param {OnlineQuery} queryDto 查询参数
 * @param {string} sheetName sheetName
 * @param {string} exportName exportName
 * @returns {Promise<Blob>} Excel文件
 */
export function exportOnline(
  queryDto?: OnlineQuery,
  sheetName?: string,
  exportName?: string
): Promise<Blob> {
  return request<Blob>({
    url: `${ONLINE_API_BASE}/export`,
    method: 'get',
    params: {
      ...queryDto,
      sheetName,
      exportName
    },
    responseType: 'blob',
  });
}
// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/statistics/logging
// 文件名称：serverMonitor.api.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：statistics/logging 模块 API（自动生成，请勿手改路由常量）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  AppStatus,
  ServerHardware
} from '@/types/statistics/logging/serverMonitor.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktServerMonitors
 */
const SERVER_MONITOR_API_BASE = 'TaktServerMonitors';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 获取服务器硬件信息
 * @returns {Promise<ServerHardware>} 服务器硬件信息 DTO
 */
export function getServerHardware(): Promise<ServerHardware> {
  return request<ServerHardware>({
    url: `${SERVER_MONITOR_API_BASE}/hardware`,
    method: 'get',
  });
}

/**
 * 获取应用运行状态
 * @returns {Promise<AppStatus>} 应用运行状态 DTO
 */
export function getAppStatus(): Promise<AppStatus> {
  return request<AppStatus>({
    url: `${SERVER_MONITOR_API_BASE}/app-status`,
    method: 'get',
  });
}

/**
 * 刷新服务器硬件信息缓存
 * @returns {Promise<void>}
 */
export function refreshHardwareCache(): Promise<void> {
  return request<void>({
    url: `${SERVER_MONITOR_API_BASE}/refresh-cache`,
    method: 'post',
  });
}

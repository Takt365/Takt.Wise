// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/foundation
// 文件名称：cache.api.ts
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：缓存管理 API（配置、统计、键存在检查与删除）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  TaktCacheInfoDto,
  TaktCacheKeyExistsDto,
  TaktCacheStatisticsDto,
} from '@/types/foundation/cache.types';

export type { TaktCacheInfoDto, TaktCacheKeyExistsDto, TaktCacheStatisticsDto };

/**
 * API 路径前缀（相对 request baseURL，对应后端 TaktCachesController）
 * @description TaktCaches
 */
const CACHE_API_BASE = 'TaktCaches';

/**
 * 获取缓存配置信息
 * @returns {Promise<TaktCacheInfoDto>} 缓存配置
 */
export function getCacheInfo(): Promise<TaktCacheInfoDto> {
  return request<TaktCacheInfoDto>({
    url: `${CACHE_API_BASE}/info`,
    method: 'get',
  });
}

/**
 * 获取缓存统计信息
 * @returns {Promise<TaktCacheStatisticsDto>} 缓存统计
 */
export function getCacheStatistics(): Promise<TaktCacheStatisticsDto> {
  return request<TaktCacheStatisticsDto>({
    url: `${CACHE_API_BASE}/statistics`,
    method: 'get',
  });
}

/**
 * 检查缓存键是否存在
 * @param {string} key 缓存键
 * @returns {Promise<TaktCacheKeyExistsDto>} 存在性结果
 */
export function existsCacheKey(key: string): Promise<TaktCacheKeyExistsDto> {
  return request<TaktCacheKeyExistsDto>({
    url: `${CACHE_API_BASE}/exists`,
    method: 'get',
    params: { key },
  });
}

/**
 * 移除指定缓存键
 * @param {string} key 缓存键
 * @returns {Promise<void>}
 */
export function removeCacheKey(key: string): Promise<void> {
  return request<void>({
    url: `${CACHE_API_BASE}/key`,
    method: 'delete',
    params: { key },
  });
}

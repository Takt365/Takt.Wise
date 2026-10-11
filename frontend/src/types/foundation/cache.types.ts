// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/foundation
// 文件名称：cache.types.ts
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：缓存管理 API 类型（对齐后端 TaktCache*Dto）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 缓存配置信息
 * @description 对应后端 TaktCacheInfoDto
 */
export interface TaktCacheInfoDto {
  /** 缓存提供者（Memory / Redis） */
  provider: string;
  /** 默认过期时间（分钟） */
  defaultExpirationMinutes: number;
  /** 是否启用滑动过期 */
  enableSlidingExpiration: boolean;
  /** 是否启用多级缓存（Memory + Redis） */
  enableMultiLevelCache: boolean;
  /** Redis 实例名前缀（Provider 为 Redis 时） */
  redisInstanceName?: string;
}

/**
 * 缓存统计信息
 * @description 对应后端 TaktCacheStatisticsDto
 */
export interface TaktCacheStatisticsDto {
  /** 当前提供者是否支持统计 */
  supported: boolean;
  /** 不支持或说明文案 */
  message?: string;
  /** 当前条目数 */
  currentEntryCount?: number;
  /** 总命中次数 */
  totalHits?: number;
  /** 总未命中次数 */
  totalMisses?: number;
  /** 命中率（0~1） */
  hitRate?: number;
  /** 估算占用字节数 */
  currentEstimatedSizeBytes?: number;
}

/**
 * 缓存键存在性检查结果
 * @description 对应后端 TaktCacheKeyExistsDto
 */
export interface TaktCacheKeyExistsDto {
  /** 键是否存在 */
  exists: boolean;
}

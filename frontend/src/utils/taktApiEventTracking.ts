// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktApiEventTracking.ts
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：CRUD API 性能事件纯工具（慢请求 / HTTP 错误判定与 DTO 映射）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import {
  getApiErrorThresholdMs,
  getApiSlowThresholdMs,
  TAKT_API_TRACK_EXCLUDED_PATH,
  TAKT_EVENT_CATEGORY_NETWORK,
  TAKT_EVENT_TYPE_API_ERROR,
  TAKT_EVENT_TYPE_API_SLOW,
  TAKT_TRACKING_LEVEL_ERROR,
  TAKT_TRACKING_LEVEL_WARN,
} from '@/config/eventTracking';
import type { TrackingLogTrackItem } from '@/types/statistics/logging/trackingLog.types';
import { buildEventTrackingTrackItem } from '@/utils/taktEventTrackingReporter';

/**
 * 是否应跳过 API 性能监控
 * @param url 请求 URL
 * @param skipFlag 请求级跳过标记
 * @returns {boolean} 跳过时为 true
 */
export function shouldSkipApiPerformanceTrack(url: string | undefined, skipFlag?: boolean): boolean {
  if (skipFlag) {
    return true;
  }
  if (!url?.trim()) {
    return true;
  }
  return url.includes(TAKT_API_TRACK_EXCLUDED_PATH);
}

/**
 * HTTP 状态是否为成功 2xx
 * @param status HTTP 状态码
 * @returns {boolean} 2xx 为 true
 */
export function isHttpSuccessStatus(status: number): boolean {
  return status >= 200 && status < 300;
}

/**
 * 构建 API 慢请求上报条目
 * @param method HTTP 方法
 * @param url 请求 URL
 * @param status HTTP 状态码
 * @param durationMs 耗时毫秒
 * @returns {TrackingLogTrackItem | null} 未达阈值返回 null
 */
export function buildApiSlowTrackItem(
  method: string,
  url: string,
  status: number,
  durationMs: number
): TrackingLogTrackItem | null {
  const slowMs = getApiSlowThresholdMs();
  if (durationMs < slowMs) {
    return null;
  }
  const errorMs = getApiErrorThresholdMs();
  const trackingLevel = durationMs >= errorMs ? TAKT_TRACKING_LEVEL_ERROR : TAKT_TRACKING_LEVEL_WARN;
  return buildEventTrackingTrackItem({
    eventTrackingType: TAKT_EVENT_TYPE_API_SLOW,
    eventTrackingCategory: TAKT_EVENT_CATEGORY_NETWORK,
    durationMs: Math.round(durationMs),
    performanceStartMs: 0,
    entryName: method.toUpperCase().slice(0, 40),
    trackingLevel,
    containerType: 'xhr',
    containerName: String(status),
    containerSrc: url.slice(0, 500),
    containerId: '',
    attributionJson: JSON.stringify({ status, slowThresholdMs: slowMs }),
  });
}

/**
 * 构建 API HTTP 错误上报条目
 * @param method HTTP 方法
 * @param url 请求 URL
 * @param status HTTP 状态码（无响应时为 0）
 * @param durationMs 耗时毫秒
 * @param message 错误摘要
 * @returns {TrackingLogTrackItem | null} 2xx 返回 null
 */
export function buildApiErrorTrackItem(
  method: string,
  url: string,
  status: number,
  durationMs: number,
  message?: string
): TrackingLogTrackItem | null {
  if (status >= 200 && status < 300) {
    return null;
  }
  return buildEventTrackingTrackItem({
    eventTrackingType: TAKT_EVENT_TYPE_API_ERROR,
    eventTrackingCategory: TAKT_EVENT_CATEGORY_NETWORK,
    durationMs: Math.max(0, Math.round(durationMs)),
    performanceStartMs: 0,
    entryName: method.toUpperCase().slice(0, 40),
    trackingLevel: TAKT_TRACKING_LEVEL_ERROR,
    containerType: 'xhr',
    containerName: status > 0 ? String(status) : 'network-error',
    containerSrc: url.slice(0, 500),
    containerId: '',
    attributionJson: JSON.stringify({ status, message: message ?? '' }),
  });
}

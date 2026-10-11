// ========================================

// 项目名称：节拍智控·TaktWise

// 命名空间：frontend/src/utils

// 文件名称：taktApiPerformanceTracker.ts

// 创建时间：2026-06-25

// 创建人：Takt365(Cursor AI)

// 功能描述：Axios CRUD API 性能监控（慢请求、HTTP 错误；热路径预算 ≤20ms）

//

// 版权信息：Copyright (c) 2025 Takt  All rights reserved.

// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。

// ========================================



import axios from 'axios';

import type { AxiosError, AxiosResponse, InternalAxiosRequestConfig } from 'axios';

import {

  isApiPerformanceTrackEnabled,

  isEventTrackingReportEnabled,

  TAKT_API_RESPONSE_PROCESS_BUDGET_MS,

} from '@/config/eventTracking';

import {

  buildApiErrorTrackItem,

  buildApiSlowTrackItem,

  shouldSkipApiPerformanceTrack,

} from '@/utils/taktApiEventTracking';

import { enqueuePerformanceEvent } from '@/utils/taktEventTrackingReporter';

import { createLogger } from '@/utils/logger';



/** API 性能监控日志 */

const apiPerfLogger = createLogger('takt-api-performance-tracker');



/**

 * 解析请求方法与 URL

 * @param config Axios 配置

 * @returns {{ method: string; url: string }} 方法与 URL

 */

function resolveMethodAndUrl(config: InternalAxiosRequestConfig): { method: string; url: string } {

  const method = (config.method ?? 'GET').toUpperCase();

  const url = config.url ?? '';

  return { method, url };

}



/**

 * 计算请求耗时

 * @param config Axios 配置

 * @returns {number} 毫秒

 */

function resolveDurationMs(config: InternalAxiosRequestConfig): number {

  const start = config._taktPerfStartMs;

  if (start == null) {

    return 0;

  }

  return Math.max(0, performance.now() - start);

}



/**

 * 处理成功响应的性能监控（异步，避免阻塞 unwrap 拦截器）

 * @param response Axios 响应

 */

function observeApiSuccess(response: AxiosResponse): void {

  if (!isApiPerformanceTrackEnabled() || !isEventTrackingReportEnabled()) {

    return;

  }

  const config = response.config;

  const { method, url } = resolveMethodAndUrl(config);

  if (shouldSkipApiPerformanceTrack(url, config.skipApiPerformanceTrack)) {

    return;

  }

  const status = response.status;

  const apiEndMs = performance.now();

  const apiStartMs = config._taktPerfStartMs ?? apiEndMs;

  queueMicrotask(() => {

    const startedAt = performance.now();

    const durationMs = resolveDurationMs(config);

    const errorItem = buildApiErrorTrackItem(method, url, status, durationMs);

    if (errorItem) {

      enqueuePerformanceEvent(errorItem, { apiStartMs, apiEndMs });

      apiPerfLogger.warn('API HTTP 错误', { action: 'api-error', method, url, status, durationMs });

      logResponseProcessBudget(startedAt, method, url);

      return;

    }

    const slowItem = buildApiSlowTrackItem(method, url, status, durationMs);

    if (slowItem) {

      enqueuePerformanceEvent(slowItem, { apiStartMs, apiEndMs });

      apiPerfLogger.warn('API 慢请求', { action: 'api-slow', method, url, status, durationMs });

    }

    logResponseProcessBudget(startedAt, method, url);

  });

}



/**

 * 处理失败响应的性能监控（异步）

 * @param error Axios 错误

 */

function observeApiFailure(error: unknown): void {

  if (!isApiPerformanceTrackEnabled() || !isEventTrackingReportEnabled()) {

    return;

  }

  if (!axios.isAxiosError(error)) {

    return;

  }

  const axiosError = error as AxiosError;

  const config = axiosError.config;

  if (!config) {

    return;

  }

  const { method, url } = resolveMethodAndUrl(config);

  if (shouldSkipApiPerformanceTrack(url, config.skipApiPerformanceTrack)) {

    return;

  }

  const status = axiosError.response?.status ?? 0;

  const message = axiosError.message;

  const apiEndMs = performance.now();

  const apiStartMs = config._taktPerfStartMs ?? apiEndMs;

  queueMicrotask(() => {

    const startedAt = performance.now();

    const durationMs = resolveDurationMs(config);

    const errorItem = buildApiErrorTrackItem(method, url, status, durationMs, message);

    if (errorItem) {

      enqueuePerformanceEvent(errorItem, { apiStartMs, apiEndMs });

      apiPerfLogger.warn('API 请求失败', { action: 'api-error', method, url, status, durationMs });

      logResponseProcessBudget(startedAt, method, url);

      return;

    }

    const slowItem = buildApiSlowTrackItem(method, url, status, durationMs);

    if (slowItem) {

      enqueuePerformanceEvent(slowItem, { apiStartMs, apiEndMs });

    }

    logResponseProcessBudget(startedAt, method, url);

  });

}



/**

 * 记录响应监控处理耗时是否超出预算

 * @param startedAt performance.now 起点

 * @param method HTTP 方法

 * @param url 请求 URL

 */

function logResponseProcessBudget(startedAt: number, method: string, url: string): void {

  const elapsedMs = performance.now() - startedAt;

  if (elapsedMs > TAKT_API_RESPONSE_PROCESS_BUDGET_MS) {

    apiPerfLogger.debug('API 监控处理超时', {

      action: 'api-perf-budget',

      method,

      url,

      elapsedMs: Math.round(elapsedMs),

      budgetMs: TAKT_API_RESPONSE_PROCESS_BUDGET_MS,

    });

  }

}



/**

 * 在 request.ts 内联调用的请求拦截增强

 * @param config Axios 请求配置

 * @returns {InternalAxiosRequestConfig} 增强后的配置

 */

export function attachApiPerformanceStart(config: InternalAxiosRequestConfig): InternalAxiosRequestConfig {

  if (!isApiPerformanceTrackEnabled()) {

    return config;

  }

  const { url } = resolveMethodAndUrl(config);

  if (shouldSkipApiPerformanceTrack(url, config.skipApiPerformanceTrack)) {

    return config;

  }

  config._taktPerfStartMs = performance.now();

  return config;

}



/**

 * 在 request.ts 内联调用的成功响应监控

 * @param response Axios 原始响应

 * @returns {AxiosResponse} 原样返回

 */

export function observeApiPerformanceResponse(response: AxiosResponse): AxiosResponse {

  observeApiSuccess(response);

  return response;

}



/**

 * 在 request.ts 内联调用的失败响应监控

 * @param error 捕获异常

 * @returns {Promise<unknown>} 继续 reject

 */

export function observeApiPerformanceError(error: unknown): Promise<unknown> {

  observeApiFailure(error);

  return Promise.reject(error);

}


// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktEventTrackingReporter.ts
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：三位一体监控统一批量上报 TaktTrackingLogs/track-batch（运行时网关）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { trackTrackingLogBatch } from '@/api/statistics/logging/trackingLog.api';
import {
  getEventTrackingBatchSize,
  getEventTrackingFlushMs,
  isEventTrackingReportEnabled,
  TAKT_EVENT_TRACK_PROCESS_BUDGET_MS,
} from '@/config/eventTracking';
import { useUserStore } from '@/stores/identity/useUserStore';
import type { TrackingLogTrackItem } from '@/types/statistics/logging/trackingLog.types';
import { createLogger } from '@/utils/logger';
import {
  buildEventTrackingDiagnosis,
  mergeDiagnosisIntoAttributionJson,
} from '@/utils/taktEventTrackingDiagnosis';
import {
  buildApiSlowDiagnosisContext,
  buildFpsDiagnosisContext,
  recordCorrelatorApiSlow,
  recordCorrelatorFpsWindow,
  recordCorrelatorLongTask,
} from '@/utils/taktEventTrackingCorrelator';
import {
  TAKT_EVENT_TYPE_API_SLOW,
  TAKT_EVENT_TYPE_FPS,
  TAKT_EVENT_TYPE_LONG_TASK,
} from '@/config/eventTracking';
import { mergeRuntimeContext } from '@/utils/runtimeContext';

/** 性能事件上报上下文 */
export interface PerformanceEventContext {
  /** API 请求开始 performance.now() / entry.startTime 同源 */
  apiStartMs?: number;
  /** API 请求结束 performance.now() */
  apiEndMs?: number;
  /** FPS 采样窗口结束时刻 */
  fpsWindowEndMs?: number;
  /** FPS 采样窗口长度 */
  fpsWindowMs?: number;
  /** Long Task startTime（PerformanceEntry） */
  longTaskStartMs?: number;
}

/** 上报模块日志 */
const reporterLogger = createLogger('takt-event-tracking-reporter');

/** 待上报队列 */
const queue: TrackingLogTrackItem[] = [];

/** 定时 flush 句柄 */
let flushTimer: ReturnType<typeof setInterval> | null = null;

/** 是否正在 flush */
let isFlushing = false;

/** 是否已注册页面生命周期监听 */
let lifecycleRegistered = false;

/**
 * 构建交互日志上报条目公共字段
 * @param partial 业务字段
 * @returns {TrackingLogTrackItem} 上报条目
 */
export function buildEventTrackingTrackItem(
  partial: Omit<
    TrackingLogTrackItem,
    'routePath' | 'pageUrl' | 'userAgent' | 'eventTime'
  > & { eventTime?: string }
): TrackingLogTrackItem {
  const context = mergeRuntimeContext({ module: 'event-tracking', action: partial.eventTrackingType });
  return {
    eventTime: partial.eventTime ?? new Date().toISOString(),
    routePath: typeof context.route === 'string' ? context.route : '',
    pageUrl: typeof window !== 'undefined' ? window.location.href : '',
    userAgent: typeof navigator !== 'undefined' ? navigator.userAgent : '',
    ...partial,
  };
}

/**
 * 统一性能事件上报入口（关联诊断 + 批量队列；异步执行，RAIL 事件处理预算 ≤50ms）
 * @param item 上报条目
 * @param context 关联上下文
 */
export function enqueuePerformanceEvent(
  item: TrackingLogTrackItem,
  context: PerformanceEventContext = {}
): void {
  if (!isEventTrackingReportEnabled()) {
    return;
  }
  queueMicrotask(() => {
    const startedAt = performance.now();
    const diagnosisContext = resolveDiagnosisContext(item, context);
    const diagnosis = buildEventTrackingDiagnosis(item.eventTrackingType, {
      durationMs: item.durationMs,
      performanceStartMs: item.performanceStartMs,
      entryName: item.entryName,
      containerName: item.containerName,
    }, diagnosisContext);
    const enriched: TrackingLogTrackItem = diagnosis
      ? { ...item, attributionJson: mergeDiagnosisIntoAttributionJson(item.attributionJson, diagnosis) }
      : item;
    recordCorrelatorFromItem(enriched, context);
    enqueueEventTrackingItem(enriched);
    if (diagnosis) {
      reporterLogger.info(`性能事件诊断 | ${diagnosis.reportResult} | 定位 ${diagnosis.problemLocation} | 建议 ${diagnosis.action}`, {
        action: item.eventTrackingType,
        reportResult: diagnosis.reportResult,
        problemLocation: diagnosis.problemLocation,
        actionHint: diagnosis.action,
        entryName: item.entryName,
        durationMs: item.durationMs,
      });
    }
    const elapsedMs = performance.now() - startedAt;
    if (elapsedMs > TAKT_EVENT_TRACK_PROCESS_BUDGET_MS) {
      reporterLogger.debug('交互日志处理超时', {
        action: item.eventTrackingType,
        elapsedMs: Math.round(elapsedMs),
        budgetMs: TAKT_EVENT_TRACK_PROCESS_BUDGET_MS,
      });
    }
  });
}

/**
 * 解析诊断关联上下文
 * @param item 上报条目
 * @param context 上报上下文
 */
function resolveDiagnosisContext(
  item: TrackingLogTrackItem,
  context: PerformanceEventContext
) {
  const type = item.eventTrackingType.trim().toLowerCase();
  if (type === TAKT_EVENT_TYPE_API_SLOW && context.apiStartMs != null && context.apiEndMs != null) {
    return buildApiSlowDiagnosisContext(context.apiStartMs, context.apiEndMs);
  }
  if (type === TAKT_EVENT_TYPE_FPS && context.fpsWindowEndMs != null && context.fpsWindowMs != null) {
    return buildFpsDiagnosisContext(context.fpsWindowEndMs, context.fpsWindowMs);
  }
  return {};
}

/**
 * 写入关联时间线
 * @param item 上报条目
 * @param context 上报上下文
 */
function recordCorrelatorFromItem(
  item: TrackingLogTrackItem,
  context: PerformanceEventContext
): void {
  const type = item.eventTrackingType.trim().toLowerCase();
  if (type === TAKT_EVENT_TYPE_LONG_TASK) {
    const startMs = context.longTaskStartMs ?? item.performanceStartMs;
    recordCorrelatorLongTask(startMs, item.durationMs);
    return;
  }
  if (type === TAKT_EVENT_TYPE_API_SLOW && context.apiStartMs != null && context.apiEndMs != null) {
    recordCorrelatorApiSlow(context.apiStartMs, context.apiEndMs, item.durationMs);
    return;
  }
  if (type === TAKT_EVENT_TYPE_FPS && context.fpsWindowEndMs != null) {
    recordCorrelatorFpsWindow(context.fpsWindowEndMs, item.durationMs, item.performanceStartMs);
  }
}

/**
 * 入队交互日志条目（内部队列，不含诊断）
 * @param item 上报条目
 */
export function enqueueEventTrackingItem(item: TrackingLogTrackItem): void {
  if (!isEventTrackingReportEnabled()) {
    return;
  }
  queue.push(item);
  if (queue.length >= getEventTrackingBatchSize()) {
    void flushEventTrackingQueue(false);
  }
}

/**
 * 启动交互日志上报定时 flush 与卸载兜底
 */
export function startEventTrackingReporter(): void {
  if (!isEventTrackingReportEnabled() || typeof window === 'undefined') {
    return;
  }
  if (!flushTimer) {
    flushTimer = setInterval(() => {
      void flushEventTrackingQueue(false);
    }, getEventTrackingFlushMs());
  }
  if (!lifecycleRegistered) {
    window.addEventListener('beforeunload', handleBeforeUnload);
    document.addEventListener('visibilitychange', handleVisibilityChange);
    lifecycleRegistered = true;
  }
}

/**
 * 停止交互日志上报
 */
export function stopEventTrackingReporter(): void {
  if (flushTimer) {
    clearInterval(flushTimer);
    flushTimer = null;
  }
  if (typeof window !== 'undefined' && lifecycleRegistered) {
    window.removeEventListener('beforeunload', handleBeforeUnload);
    document.removeEventListener('visibilitychange', handleVisibilityChange);
    lifecycleRegistered = false;
  }
}

/**
 * 立即 flush 上报队列
 * @param _useBeacon 页面卸载标记（预留）
 * @returns {Promise<void>}
 */
export async function flushEventTrackingQueue(_useBeacon = false): Promise<void> {
  if (!isEventTrackingReportEnabled() || queue.length === 0 || isFlushing) {
    return;
  }
  const userStore = useUserStore();
  if (!userStore.isLoggedIn) {
    queue.length = 0;
    return;
  }
  const batchSize = getEventTrackingBatchSize();
  const items = queue.splice(0, batchSize);
  isFlushing = true;
  try {
    await trackTrackingLogBatch({ items });
    reporterLogger.debug('交互日志上报成功', { action: 'track-batch', count: items.length });
  } catch (error: unknown) {
    queue.unshift(...items);
    reporterLogger.warn('交互日志上报失败', { action: 'track-batch', count: items.length }, error);
  } finally {
    isFlushing = false;
  }
}

function handleBeforeUnload(): void {
  void flushEventTrackingQueue(true);
}

function handleVisibilityChange(): void {
  if (document.visibilityState === 'hidden') {
    void flushEventTrackingQueue(true);
  }
}

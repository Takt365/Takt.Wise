// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/bootstrap
// 文件名称：taktLongTaskMonitor.ts
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：Long Task API 监控（W3C 采集 ≥50ms，Warning ≥300ms）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import {
  getLongTaskErrorThresholdMs,
  getLongTaskWarnThresholdMs,
  isEventTrackingReportEnabled,
  isLongTaskMonitorEnabled,
  TAKT_EVENT_CATEGORY_PERFORMANCE,
  TAKT_EVENT_TYPE_LONG_TASK,
  TAKT_TRACKING_LEVEL_ERROR,
  TAKT_TRACKING_LEVEL_WARN,
} from '@/config/eventTracking';
import {
  buildEventTrackingTrackItem,
  enqueuePerformanceEvent,
} from '@/utils/taktEventTrackingReporter';
import {
  buildLongTaskLogContext,
  buildLongTaskLogPayload,
  isLongTaskApiSupported,
} from '@/utils/taktLongTask';
import { createLogger } from '@/utils/logger';

/** Long Task 监控模块日志 */
const longTaskLogger = createLogger('takt-long-task-monitor');

/** PerformanceObserver 实例 */
let longTaskObserver: PerformanceObserver | null = null;

/** 是否已启动观察器 */
let observerStarted = false;

/**
 * 解析 Long Task 追踪级别
 * @param durationMs 阻塞时长
 * @returns {number} 0=忽略 1=warn 2=error
 */
function resolveLongTaskTrackingLevel(durationMs: number): number {
  const warnMs = getLongTaskWarnThresholdMs();
  const errorMs = getLongTaskErrorThresholdMs();
  if (durationMs >= errorMs) {
    return TAKT_TRACKING_LEVEL_ERROR;
  }
  if (durationMs >= warnMs) {
    return TAKT_TRACKING_LEVEL_WARN;
  }
  return 0;
}

/**
 * 处理单条 Long Task 条目
 * @param entry PerformanceObserver 回调条目
 */
function handleLongTaskEntry(entry: PerformanceEntry): void {
  const payload = buildLongTaskLogPayload(entry);
  const trackingLevel = resolveLongTaskTrackingLevel(payload.durationMs);
  if (trackingLevel <= 0) {
    return;
  }
  const context = buildLongTaskLogContext(payload);
  const primaryAttribution = payload.attributions[0];
  const attributionHint = primaryAttribution?.containerName || primaryAttribution?.containerSrc
    ? `归因 ${primaryAttribution.containerType || 'unknown'}:${primaryAttribution.containerName || primaryAttribution.containerSrc}`
    : '';
  const message = [
    `主线程长任务 ${payload.durationMs.toFixed(1)}ms`,
    '定位 主线程阻塞',
    '建议 拆分长任务 / Web Worker',
    attributionHint,
    `路由 ${typeof window !== 'undefined' ? window.location.pathname : ''}`,
  ].filter(Boolean).join(' | ');
  if (trackingLevel === TAKT_TRACKING_LEVEL_ERROR) {
    longTaskLogger.error(message, { ...context, problemLocation: '主线程阻塞', actionHint: '拆分长任务 / Web Worker' });
  } else {
    longTaskLogger.warn(message, { ...context, problemLocation: '主线程阻塞', actionHint: '拆分长任务 / Web Worker' });
  }
  if (isEventTrackingReportEnabled()) {
    const trackItem = buildEventTrackingTrackItem({
      eventTrackingType: TAKT_EVENT_TYPE_LONG_TASK,
      eventTrackingCategory: TAKT_EVENT_CATEGORY_PERFORMANCE,
      durationMs: Math.round(payload.durationMs),
      performanceStartMs: payload.startTimeMs,
      entryName: payload.entryName,
      trackingLevel,
      containerType: primaryAttribution?.containerType ?? '',
      containerName: primaryAttribution?.containerName ?? '',
      containerSrc: primaryAttribution?.containerSrc ?? '',
      containerId: primaryAttribution?.containerId ?? '',
      attributionJson: JSON.stringify(payload.attributions),
    });
    enqueuePerformanceEvent(trackItem, { longTaskStartMs: payload.startTimeMs });
  }
}

/**
 * 启动 Long Task 监控
 */
export function initTaktLongTaskMonitor(): void {
  if (typeof window === 'undefined' || observerStarted) {
    return;
  }
  if (!isLongTaskMonitorEnabled()) {
    longTaskLogger.debug('Long Task 监控已禁用', { action: 'skip' });
    return;
  }
  if (!isLongTaskApiSupported()) {
    longTaskLogger.debug('浏览器不支持 Long Task API', { action: 'skip' });
    return;
  }
  try {
    longTaskObserver = new PerformanceObserver((list) => {
      for (const entry of list.getEntries()) {
        handleLongTaskEntry(entry);
      }
    });
    longTaskObserver.observe({ type: 'longtask', buffered: true });
    observerStarted = true;
    longTaskLogger.info('Long Task 监控已启动', {
      action: 'init',
      warnThresholdMs: getLongTaskWarnThresholdMs(),
      errorThresholdMs: getLongTaskErrorThresholdMs(),
    });
  } catch (error: unknown) {
    longTaskLogger.warn('Long Task 监控启动失败', { action: 'init-failed' }, error);
    longTaskObserver = null;
  }
}

/**
 * 停止 Long Task 监控
 */
export function stopTaktLongTaskMonitor(): void {
  if (longTaskObserver) {
    longTaskObserver.disconnect();
    longTaskObserver = null;
  }
  observerStarted = false;
}

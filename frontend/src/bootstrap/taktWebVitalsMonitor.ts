// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/bootstrap
// 文件名称：taktWebVitalsMonitor.ts
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：Web Vitals 监控（FCP / LCP / INP / CLS，对齐 Google Core Web Vitals）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import {
  getClsErrorThreshold,
  getClsWarnThreshold,
  getFcpWarnThresholdMs,
  getInpErrorThresholdMs,
  getInpWarnThresholdMs,
  getLcpErrorThresholdMs,
  getLcpWarnThresholdMs,
  isEventTrackingReportEnabled,
  isWebVitalsMonitorEnabled,
  TAKT_EVENT_CATEGORY_LOADING,
  TAKT_EVENT_TYPE_WEB_VITAL,
  TAKT_TRACKING_LEVEL_ERROR,
  TAKT_TRACKING_LEVEL_WARN,
} from '@/config/eventTracking';
import {
  buildEventTrackingTrackItem,
  enqueuePerformanceEvent,
} from '@/utils/taktEventTrackingReporter';
import { diagnoseWebVitalEvent } from '@/utils/taktEventTrackingDiagnosis';
import {
  buildWebVitalLogContext,
  buildWebVitalPerformanceInsight,
  formatWebVitalConsoleMessage,
} from '@/utils/taktWebVitalsInsight';
import {
  isClsAtOrAboveWarn,
  isEventTimingSupported,
  isLcpSupported,
  isLayoutShiftSupported,
  isPaintTimingSupported,
  isWebVitalMsAtOrAboveWarn,
  resolveClsTrackingLevel,
  resolveWebVitalMsTrackingLevel,
  type TaktEventTimingEntry,
  type TaktLayoutShiftEntry,
  type TaktWebVitalMetric,
} from '@/utils/taktWebVitals';
import type { LogContext } from '@/types/logger.types';
import { createLogger } from '@/utils/logger';

/** Web Vitals 监控日志 */
const webVitalsLogger = createLogger('takt-web-vitals-monitor');

/** Paint Observer */
let paintObserver: PerformanceObserver | null = null;

/** LCP Observer */
let lcpObserver: PerformanceObserver | null = null;

/** Layout Shift Observer */
let layoutShiftObserver: PerformanceObserver | null = null;

/** Event Timing Observer（INP 近似） */
let eventTimingObserver: PerformanceObserver | null = null;

/** 是否已上报 FCP */
let fcpReported = false;

/** 是否已上报 LCP */
let lcpReported = false;

/** 是否已上报 INP */
let inpReported = false;

/** 是否已上报 CLS */
let clsReported = false;

/** 最新 LCP 毫秒 */
let latestLcpMs = 0;

/** 累计 CLS（不含 hadRecentInput） */
let cumulativeCls = 0;

/** 当前页 INP 近似值（交互 entry 最大 duration） */
let latestInpMs = 0;

/**
 * 上报 Web Vital 指标
 * @param metric 指标名
 * @param trackingLevel 追踪级别
 * @param valueMs 毫秒值（CLS 时为 score×1000 便于整数存储）
 * @param rawValue 原始值（CLS 分数或毫秒）
 * @param warnThreshold Warning 阈值
 * @param errorThreshold Error 阈值
 */
function reportWebVitalMetric(
  metric: TaktWebVitalMetric,
  trackingLevel: number,
  valueMs: number,
  rawValue: number,
  warnThreshold: number,
  errorThreshold: number
): void {
  if (trackingLevel <= 0) {
    return;
  }
  const insight = buildWebVitalPerformanceInsight(metric, rawValue, warnThreshold, errorThreshold);
  const diagnosis = diagnoseWebVitalEvent(
    metric,
    rawValue,
    warnThreshold,
    errorThreshold,
    insight.likelyCause,
    insight.suggestions
  );
  const message = formatWebVitalConsoleMessage(
    metric,
    rawValue,
    warnThreshold,
    errorThreshold,
    diagnosis,
    insight
  );
  const logContext = buildWebVitalLogContext(
    metric,
    rawValue,
    warnThreshold,
    errorThreshold,
    diagnosis,
    insight
  );
  const contextForLog = logContext as LogContext;
  if (trackingLevel === TAKT_TRACKING_LEVEL_ERROR) {
    webVitalsLogger.error(message, contextForLog);
  } else {
    webVitalsLogger.warn(message, contextForLog);
  }
  if (isEventTrackingReportEnabled()) {
    enqueuePerformanceEvent(buildEventTrackingTrackItem({
      eventTrackingType: TAKT_EVENT_TYPE_WEB_VITAL,
      eventTrackingCategory: TAKT_EVENT_CATEGORY_LOADING,
      durationMs: valueMs,
      performanceStartMs: 0,
      entryName: metric,
      trackingLevel,
      containerType: 'navigation',
      containerName: metric,
      containerSrc: typeof window !== 'undefined' ? window.location.pathname : '',
      containerId: '',
      attributionJson: JSON.stringify({
        metric,
        rawValue,
        warnThreshold,
        errorThreshold,
        grade: insight.grade,
        likelyCause: insight.likelyCause,
        suggestions: insight.suggestions,
        navigation: insight.navigation,
        slowResources: insight.slowResources,
        longTasksBefore: insight.longTasksBefore,
        longTaskTotalMsBefore: insight.longTaskTotalMsBefore,
        isDev: insight.isDev,
      }),
    }));
  }
}

/**
 * 上报 FCP（延迟一帧采样 Navigation/Resource，避免 Observer 回调时 buffer 未就绪）
 * @param valueMs FCP 毫秒
 */
function reportFcp(valueMs: number): void {
  const warnMs = getFcpWarnThresholdMs();
  if (!isWebVitalMsAtOrAboveWarn(valueMs, warnMs)) {
    return;
  }
  requestAnimationFrame(() => {
    reportWebVitalMetric('fcp', TAKT_TRACKING_LEVEL_WARN, Math.round(valueMs), valueMs, warnMs, warnMs);
  });
}

/**
 * 刷新 LCP 上报
 */
function flushLcpReport(): void {
  if (lcpReported || latestLcpMs <= 0) {
    return;
  }
  lcpReported = true;
  const warnMs = getLcpWarnThresholdMs();
  const errorMs = getLcpErrorThresholdMs();
  const level = resolveWebVitalMsTrackingLevel(latestLcpMs, warnMs, errorMs);
  reportWebVitalMetric('lcp', level, Math.round(latestLcpMs), latestLcpMs, warnMs, errorMs);
}

/**
 * 刷新 INP 上报
 */
function flushInpReport(): void {
  if (inpReported || latestInpMs <= 0) {
    return;
  }
  inpReported = true;
  const warnMs = getInpWarnThresholdMs();
  const errorMs = getInpErrorThresholdMs();
  const level = resolveWebVitalMsTrackingLevel(latestInpMs, warnMs, errorMs);
  reportWebVitalMetric('inp', level, Math.round(latestInpMs), latestInpMs, warnMs, errorMs);
}

/**
 * 刷新 CLS 上报
 */
function flushClsReport(): void {
  if (clsReported || cumulativeCls <= 0) {
    return;
  }
  if (!isClsAtOrAboveWarn(cumulativeCls, getClsWarnThreshold())) {
    return;
  }
  clsReported = true;
  const warnThreshold = getClsWarnThreshold();
  const errorThreshold = getClsErrorThreshold();
  const level = resolveClsTrackingLevel(cumulativeCls, warnThreshold, errorThreshold);
  reportWebVitalMetric(
    'cls',
    level,
    Math.round(cumulativeCls * 1000),
    cumulativeCls,
    warnThreshold,
    errorThreshold
  );
}

/**
 * 页面隐藏时 flush 会话级指标
 */
function flushSessionWebVitals(): void {
  flushLcpReport();
  flushInpReport();
  flushClsReport();
}

function handleVisibilityChangeForWebVitals(): void {
  if (document.visibilityState === 'hidden') {
    flushSessionWebVitals();
  }
}

/**
 * 启动 Web Vitals 监控
 */
export function initTaktWebVitalsMonitor(): void {
  if (typeof window === 'undefined') {
    return;
  }
  if (!isWebVitalsMonitorEnabled()) {
    webVitalsLogger.debug('Web Vitals 监控已禁用', { action: 'skip' });
    return;
  }
  if (isPaintTimingSupported() && !fcpReported) {
    try {
      paintObserver = new PerformanceObserver((list) => {
        for (const entry of list.getEntries()) {
          if (entry.name !== 'first-contentful-paint' || fcpReported) {
            continue;
          }
          fcpReported = true;
          reportFcp(entry.startTime);
        }
      });
      paintObserver.observe({ type: 'paint', buffered: true });
    } catch (error: unknown) {
      webVitalsLogger.warn('FCP 监控启动失败', { action: 'fcp-init-failed' }, error);
    }
  }
  if (isLcpSupported()) {
    try {
      lcpObserver = new PerformanceObserver((list) => {
        const entries = list.getEntries();
        const last = entries[entries.length - 1];
        if (last) {
          latestLcpMs = last.startTime;
        }
      });
      lcpObserver.observe({ type: 'largest-contentful-paint', buffered: true });
    } catch (error: unknown) {
      webVitalsLogger.warn('LCP 监控启动失败', { action: 'lcp-init-failed' }, error);
    }
  }
  if (isLayoutShiftSupported()) {
    try {
      layoutShiftObserver = new PerformanceObserver((list) => {
        for (const entry of list.getEntries()) {
          const shift = entry as TaktLayoutShiftEntry;
          if (shift.hadRecentInput) {
            continue;
          }
          cumulativeCls += shift.value;
        }
      });
      layoutShiftObserver.observe({ type: 'layout-shift', buffered: true });
    } catch (error: unknown) {
      webVitalsLogger.warn('CLS 监控启动失败', { action: 'cls-init-failed' }, error);
    }
  }
  if (isEventTimingSupported()) {
    try {
      eventTimingObserver = new PerformanceObserver((list) => {
        for (const entry of list.getEntries()) {
          const eventEntry = entry as TaktEventTimingEntry;
          if (!eventEntry.interactionId || eventEntry.duration <= 0) {
            continue;
          }
          if (eventEntry.duration > latestInpMs) {
            latestInpMs = eventEntry.duration;
          }
        }
      });
      eventTimingObserver.observe({ type: 'event', buffered: true, durationThreshold: 16 });
    } catch (error: unknown) {
      webVitalsLogger.warn('INP 监控启动失败', { action: 'inp-init-failed' }, error);
    }
  }
  document.addEventListener('visibilitychange', handleVisibilityChangeForWebVitals);
  window.addEventListener('pagehide', flushSessionWebVitals);
  webVitalsLogger.info('Web Vitals 监控已启动', {
    action: 'init',
    fcpWarnMs: getFcpWarnThresholdMs(),
    lcpWarnMs: getLcpWarnThresholdMs(),
    lcpErrorMs: getLcpErrorThresholdMs(),
    inpWarnMs: getInpWarnThresholdMs(),
    inpErrorMs: getInpErrorThresholdMs(),
    clsWarn: getClsWarnThreshold(),
    clsError: getClsErrorThreshold(),
  });
}

/**
 * 停止 Web Vitals 监控
 */
export function stopTaktWebVitalsMonitor(): void {
  paintObserver?.disconnect();
  lcpObserver?.disconnect();
  layoutShiftObserver?.disconnect();
  eventTimingObserver?.disconnect();
  paintObserver = null;
  lcpObserver = null;
  layoutShiftObserver = null;
  eventTimingObserver = null;
  document.removeEventListener('visibilitychange', handleVisibilityChangeForWebVitals);
  window.removeEventListener('pagehide', flushSessionWebVitals);
}

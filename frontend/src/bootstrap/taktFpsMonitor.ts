// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/bootstrap
// 文件名称：taktFpsMonitor.ts
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：FPS 监控（页面停留会话 p50/p75；visibility hidden + 路由切换结束；可选掉帧告警）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { Router } from 'vue-router';
import {
  getFpsDwellMinMs,
  getFpsReportCooldownMs,
  getFpsSampleMs,
  getFpsWarnThreshold,
  isEventTrackingReportEnabled,
  isFpsDropAlertEnabled,
  isFpsMonitorEnabled,
  TAKT_EVENT_CATEGORY_EXPERIENCE,
  TAKT_EVENT_CATEGORY_PERFORMANCE,
  TAKT_EVENT_TYPE_FPS,
  TAKT_EVENT_TYPE_FPS_DWELL,
  TAKT_TRACKING_LEVEL_ERROR,
  TAKT_TRACKING_LEVEL_EXPERIENCE,
  TAKT_TRACKING_LEVEL_WARN,
} from '@/config/eventTracking';
import {
  buildEventTrackingTrackItem,
  enqueuePerformanceEvent,
} from '@/utils/taktEventTrackingReporter';
import {
  formatFpsDwellConsoleMessage,
  summarizeFpsDwellSession,
  type TaktFpsDwellEndReason,
  type TaktFpsDwellSession,
} from '@/utils/taktFpsDwell';
import { calculateFps, isFpsBelowThreshold } from '@/utils/taktFps';
import { createLogger } from '@/utils/logger';

/** FPS 监控日志 */
const fpsLogger = createLogger('takt-fps-monitor');

/** rAF 句柄 */
let rafId = 0;

/** 是否已启动 */
let monitorStarted = false;

/** 采样窗口起始 */
let windowStartMs = 0;

/** 采样窗口帧数 */
let frameCount = 0;

/** 掉帧即时告警上次时间 */
let lastDropReportAtMs = 0;

/** 当前页面停留 FPS 会话 */
let dwellSession: TaktFpsDwellSession | null = null;

/** 路由 afterEach 卸载 */
let removeRouterHook: (() => void) | null = null;

/** 绑定的 router 引用（stop 时解绑 visibility/pagehide） */
let boundRouter: Router | undefined;

function onVisibilityChangeForFps(): void {
  handleVisibilityChangeForFps(boundRouter);
}

function onPageHideForFps(): void {
  handlePageHideForFps(boundRouter);
}

/**
 * 读取当前 SPA 路由路径
 * @param router Vue Router
 * @returns {string} 路径
 */
function resolveRoutePath(router?: Router): string {
  if (router?.currentRoute?.value?.path) {
    return router.currentRoute.value.path;
  }
  if (typeof window !== 'undefined') {
    return window.location.pathname;
  }
  return '';
}

/**
 * 开始页面停留 FPS 会话
 * @param router Vue Router
 */
function startDwellSession(router?: Router): void {
  if (typeof document !== 'undefined' && document.visibilityState === 'hidden') {
    dwellSession = null;
    return;
  }
  dwellSession = {
    routePath: resolveRoutePath(router),
    startedAtMs: performance.now(),
    fpsSamples: [],
  };
}

/**
 * 结束并上报页面停留 FPS 会话
 * @param endReason 结束原因
 * @param router Vue Router
 */
function flushDwellSession(endReason: TaktFpsDwellEndReason, router?: Router): void {
  const session = dwellSession;
  dwellSession = null;
  if (!session) {
    return;
  }
  const endedAtMs = performance.now();
  const minDwellMs = getFpsDwellMinMs();
  const summary = summarizeFpsDwellSession(session, endedAtMs, endReason);
  if (!summary || summary.dwellMs < minDwellMs) {
    return;
  }
  const message = formatFpsDwellConsoleMessage(summary);
  fpsLogger.info(message, {
    action: 'fps-dwell',
    routePath: summary.routePath,
    dwellMs: summary.dwellMs,
    fpsP50: summary.fpsP50,
    fpsP75: summary.fpsP75,
    fpsMin: summary.fpsMin,
    fpsMax: summary.fpsMax,
    sampleCount: summary.sampleCount,
    endReason: summary.endReason,
    problemLocation: '页面停留流畅度（体验补充）',
    actionHint: '线上看 p50/p75 分位，结合跳出率与交互转化判断；不作性能告警',
  });
  if (isEventTrackingReportEnabled()) {
    enqueuePerformanceEvent(buildEventTrackingTrackItem({
      eventTrackingType: TAKT_EVENT_TYPE_FPS_DWELL,
      eventTrackingCategory: TAKT_EVENT_CATEGORY_EXPERIENCE,
      durationMs: summary.dwellMs,
      performanceStartMs: Math.round(summary.fpsP50 * 10),
      entryName: 'fps-dwell',
      trackingLevel: TAKT_TRACKING_LEVEL_EXPERIENCE,
      containerType: 'dwell',
      containerName: summary.endReason,
      containerSrc: summary.routePath,
      containerId: `p75-${Math.round(summary.fpsP75 * 10)}`,
      attributionJson: JSON.stringify({
        ...summary,
        metric: 'fps-dwell',
        note: 'experience-supplement-not-alert',
      }),
    }));
  }
  if (endReason === 'route') {
    startDwellSession(router);
  }
}

function handleVisibilityChangeForFps(router?: Router): void {
  if (document.visibilityState === 'hidden') {
    flushDwellSession('visibility', router);
    return;
  }
  if (document.visibilityState === 'visible' && !dwellSession) {
    startDwellSession(router);
  }
}

function handlePageHideForFps(router?: Router): void {
  flushDwellSession('unload', router);
}

/**
 * 可选：即时掉帧告警（默认关闭）
 * @param fps 当前 FPS
 * @param elapsedMs 采样窗口毫秒
 * @param now performance.now()
 */
function maybeReportFpsDrop(fps: number, elapsedMs: number, now: number): void {
  if (!isFpsDropAlertEnabled()) {
    return;
  }
  const threshold = getFpsWarnThreshold();
  if (!isFpsBelowThreshold(fps, threshold)) {
    return;
  }
  const cooldownMs = getFpsReportCooldownMs();
  if (now - lastDropReportAtMs < cooldownMs) {
    return;
  }
  lastDropReportAtMs = now;
  const trackingLevel = fps < threshold / 2 ? TAKT_TRACKING_LEVEL_ERROR : TAKT_TRACKING_LEVEL_WARN;
  const message = `FPS 掉帧 ${fps.toFixed(1)} (< ${threshold})`;
  if (trackingLevel === TAKT_TRACKING_LEVEL_ERROR) {
    fpsLogger.error(message, { action: 'fps-drop', fps, threshold, sampleMs: elapsedMs });
  } else {
    fpsLogger.warn(message, { action: 'fps-drop', fps, threshold, sampleMs: elapsedMs });
  }
  if (isEventTrackingReportEnabled()) {
    enqueuePerformanceEvent(buildEventTrackingTrackItem({
      eventTrackingType: TAKT_EVENT_TYPE_FPS,
      eventTrackingCategory: TAKT_EVENT_CATEGORY_PERFORMANCE,
      durationMs: Math.round(elapsedMs),
      performanceStartMs: Math.round(fps * 100) / 100,
      entryName: 'fps-drop',
      trackingLevel,
      containerType: 'raf',
      containerName: `threshold-${threshold}`,
      containerSrc: '',
      containerId: '',
      attributionJson: JSON.stringify({ fps, threshold, sampleMs: elapsedMs }),
    }), { fpsWindowEndMs: now, fpsWindowMs: elapsedMs });
  }
}

/**
 * 处理一帧采样
 * @param now 当前 performance.now()
 * @param router Vue Router
 */
function handleFpsFrame(now: number, router?: Router): void {
  if (typeof document !== 'undefined' && document.visibilityState === 'hidden') {
    rafId = requestAnimationFrame((t) => handleFpsFrame(t, router));
    return;
  }
  if (!dwellSession) {
    startDwellSession(router);
  }
  if (windowStartMs <= 0) {
    windowStartMs = now;
  }
  frameCount += 1;
  const sampleMs = getFpsSampleMs();
  const elapsedMs = now - windowStartMs;
  if (elapsedMs < sampleMs) {
    rafId = requestAnimationFrame((t) => handleFpsFrame(t, router));
    return;
  }
  const fps = calculateFps(frameCount, elapsedMs);
  if (dwellSession) {
    dwellSession.fpsSamples.push(Math.round(fps * 10) / 10);
  }
  maybeReportFpsDrop(fps, elapsedMs, now);
  frameCount = 0;
  windowStartMs = now;
  rafId = requestAnimationFrame((t) => handleFpsFrame(t, router));
}

/**
 * 启动 FPS 监控
 * @param router Vue Router（SPA 路由切换结束页面停留）
 */
export function initTaktFpsMonitor(router?: Router): void {
  if (typeof window === 'undefined' || monitorStarted) {
    return;
  }
  if (!isFpsMonitorEnabled()) {
    fpsLogger.debug('FPS 监控已禁用', { action: 'skip' });
    return;
  }
  windowStartMs = 0;
  frameCount = 0;
  lastDropReportAtMs = 0;
  boundRouter = router;
  startDwellSession(router);
  if (router) {
    removeRouterHook = router.afterEach((to, from) => {
      if (to.path === from.path) {
        return;
      }
      flushDwellSession('route', router);
    });
  }
  document.addEventListener('visibilitychange', onVisibilityChangeForFps);
  window.addEventListener('pagehide', onPageHideForFps);
  rafId = requestAnimationFrame((t) => handleFpsFrame(t, router));
  monitorStarted = true;
  fpsLogger.info('FPS 监控已启动', {
    action: 'init',
    dwellMinMs: getFpsDwellMinMs(),
    sampleMs: getFpsSampleMs(),
    dropAlert: isFpsDropAlertEnabled(),
  });
}

/**
 * 停止 FPS 监控
 */
export function stopTaktFpsMonitor(): void {
  if (rafId) {
    cancelAnimationFrame(rafId);
    rafId = 0;
  }
  removeRouterHook?.();
  removeRouterHook = null;
  document.removeEventListener('visibilitychange', onVisibilityChangeForFps);
  window.removeEventListener('pagehide', onPageHideForFps);
  boundRouter = undefined;
  dwellSession = null;
  monitorStarted = false;
}

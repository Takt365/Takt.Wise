// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktWebVitals.ts
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：Web Vitals 纯工具（FCP / LCP / INP / CLS 阈值判定）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** Layout Shift PerformanceEntry 扩展 */
export interface TaktLayoutShiftEntry extends PerformanceEntry {
  value: number;
  hadRecentInput?: boolean;
}

/** Event Timing PerformanceEntry 扩展（INP 近似） */
export interface TaktEventTimingEntry extends PerformanceEntry {
  interactionId?: number;
  duration: number;
}

/** Web Vital 指标名 */
export type TaktWebVitalMetric = 'fcp' | 'lcp' | 'inp' | 'cls';

/**
 * 是否支持 Paint Timing（FCP）
 * @returns {boolean} 支持时为 true
 */
export function isPaintTimingSupported(): boolean {
  return typeof PerformanceObserver !== 'undefined'
    && typeof performance !== 'undefined'
    && 'getEntriesByType' in performance;
}

/**
 * 是否支持 LCP
 * @returns {boolean} 支持时为 true
 */
export function isLcpSupported(): boolean {
  if (typeof PerformanceObserver === 'undefined') {
    return false;
  }
  try {
    return PerformanceObserver.supportedEntryTypes?.includes('largest-contentful-paint') ?? false;
  } catch {
    return false;
  }
}

/**
 * 是否支持 Layout Shift（CLS）
 * @returns {boolean} 支持时为 true
 */
export function isLayoutShiftSupported(): boolean {
  if (typeof PerformanceObserver === 'undefined') {
    return false;
  }
  try {
    return PerformanceObserver.supportedEntryTypes?.includes('layout-shift') ?? false;
  } catch {
    return false;
  }
}

/**
 * 是否支持 Event Timing（INP 近似）
 * @returns {boolean} 支持时为 true
 */
export function isEventTimingSupported(): boolean {
  if (typeof PerformanceObserver === 'undefined') {
    return false;
  }
  try {
    return PerformanceObserver.supportedEntryTypes?.includes('event') ?? false;
  } catch {
    return false;
  }
}

/**
 * 毫秒类 Web Vital 是否达到 Warning
 * @param valueMs 指标毫秒
 * @param warnMs Warning 阈值毫秒
 * @returns {boolean} 达到 Warning 时为 true
 */
export function isWebVitalMsAtOrAboveWarn(valueMs: number, warnMs: number): boolean {
  if (Number.isNaN(valueMs) || valueMs <= 0 || Number.isNaN(warnMs) || warnMs <= 0) {
    return false;
  }
  return valueMs >= warnMs;
}

/**
 * 毫秒类 Web Vital 是否达到 Error / Poor
 * @param valueMs 指标毫秒
 * @param errorMs Error 阈值毫秒
 * @returns {boolean} 达到 Error 时为 true
 */
export function isWebVitalMsAtOrAboveError(valueMs: number, errorMs: number): boolean {
  if (Number.isNaN(valueMs) || valueMs <= 0 || Number.isNaN(errorMs) || errorMs <= 0) {
    return false;
  }
  return valueMs >= errorMs;
}

/**
 * CLS 是否达到 Warning
 * @param value CLS 累计分数
 * @param warnThreshold Warning 阈值
 * @returns {boolean} 达到 Warning 时为 true
 */
export function isClsAtOrAboveWarn(value: number, warnThreshold: number): boolean {
  if (Number.isNaN(value) || value < 0 || Number.isNaN(warnThreshold) || warnThreshold <= 0) {
    return false;
  }
  return value >= warnThreshold;
}

/**
 * CLS 是否达到 Error / Poor
 * @param value CLS 累计分数
 * @param errorThreshold Error 阈值
 * @returns {boolean} 达到 Error 时为 true
 */
export function isClsAtOrAboveError(value: number, errorThreshold: number): boolean {
  if (Number.isNaN(value) || value < 0 || Number.isNaN(errorThreshold) || errorThreshold <= 0) {
    return false;
  }
  return value >= errorThreshold;
}

/**
 * 解析毫秒类 Web Vital 追踪级别
 * @param valueMs 指标毫秒
 * @param warnMs Warning 阈值
 * @param errorMs Error 阈值
 * @returns {number} 0=忽略 1=warn 2=error
 */
export function resolveWebVitalMsTrackingLevel(
  valueMs: number,
  warnMs: number,
  errorMs: number
): number {
  if (isWebVitalMsAtOrAboveError(valueMs, errorMs)) {
    return 2;
  }
  if (isWebVitalMsAtOrAboveWarn(valueMs, warnMs)) {
    return 1;
  }
  return 0;
}

/**
 * 解析 CLS 追踪级别
 * @param value CLS 分数
 * @param warnThreshold Warning 阈值
 * @param errorThreshold Error 阈值
 * @returns {number} 0=忽略 1=warn 2=error
 */
export function resolveClsTrackingLevel(
  value: number,
  warnThreshold: number,
  errorThreshold: number
): number {
  if (isClsAtOrAboveError(value, errorThreshold)) {
    return 2;
  }
  if (isClsAtOrAboveWarn(value, warnThreshold)) {
    return 1;
  }
  return 0;
}

/** @deprecated 使用 isWebVitalMsAtOrAboveWarn */
export function isWebVitalAboveThreshold(valueMs: number, warnMs: number): boolean {
  return isWebVitalMsAtOrAboveWarn(valueMs, warnMs);
}

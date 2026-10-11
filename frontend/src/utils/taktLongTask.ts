// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktLongTask.ts
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：Long Task API 纯工具（能力检测、归因解析、日志上下文构建）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { LogContext } from '@/types/logger.types';

/** Task Attribution Timing（Chrome / Edge 等支持） */
export interface TaktTaskAttributionTiming {
  name: string;
  entryType: string;
  startTime: number;
  duration: number;
  containerType: string;
  containerSrc: string;
  containerId: string;
  containerName: string;
}

/** Long Task PerformanceEntry 扩展字段 */
export interface TaktLongTaskEntry extends PerformanceEntry {
  attribution?: TaktTaskAttributionTiming[];
}

/** 长任务日志上下文 */
export interface TaktLongTaskLogPayload {
  durationMs: number;
  startTimeMs: number;
  entryName: string;
  attributions: ReadonlyArray<{
    containerType: string;
    containerName: string;
    containerSrc: string;
    containerId: string;
    durationMs: number;
  }>;
}

/**
 * 当前浏览器是否支持 Long Task PerformanceObserver
 * @returns {boolean} 支持时为 true
 */
export function isLongTaskApiSupported(): boolean {
  if (typeof PerformanceObserver === 'undefined') {
    return false;
  }
  const supported = PerformanceObserver.supportedEntryTypes;
  return Array.isArray(supported) && supported.includes('longtask');
}

/**
 * 将 PerformanceEntry 转为长任务日志载荷
 * @param entry PerformanceObserver 回调条目
 * @returns {TaktLongTaskLogPayload} 结构化载荷
 */
export function buildLongTaskLogPayload(entry: PerformanceEntry): TaktLongTaskLogPayload {
  const longTask = entry as TaktLongTaskEntry;
  const attributions = (longTask.attribution ?? []).map((item) => ({
    containerType: item.containerType ?? '',
    containerName: item.containerName ?? '',
    containerSrc: item.containerSrc ?? '',
    containerId: item.containerId ?? '',
    durationMs: item.duration ?? 0,
  }));
  return {
    durationMs: entry.duration,
    startTimeMs: entry.startTime,
    entryName: entry.name || 'longtask',
    attributions,
  };
}

/**
 * 构建写入 logger 的 LogContext
 * @param payload 长任务载荷
 * @returns {LogContext} 日志上下文
 */
export function buildLongTaskLogContext(payload: TaktLongTaskLogPayload): LogContext {
  const primaryAttribution = payload.attributions[0];
  return {
    module: 'performance',
    action: 'longtask',
    durationMs: payload.durationMs,
    startTimeMs: payload.startTimeMs,
    entryName: payload.entryName,
    containerType: primaryAttribution?.containerType,
    containerName: primaryAttribution?.containerName,
    containerSrc: primaryAttribution?.containerSrc,
    containerId: primaryAttribution?.containerId,
    attributions: payload.attributions,
  };
}

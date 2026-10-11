// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktEventTrackingCorrelator.ts
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：性能事件时间线关联（Long Task / API / FPS 窗口判定；运行时网关）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import {
  getApiSlowThresholdMs,
  getCorrelationWindowMs,
  TAKT_EVENT_TYPE_API_SLOW,
  TAKT_EVENT_TYPE_FPS,
  TAKT_EVENT_TYPE_LONG_TASK,
} from '@/config/eventTracking';

/** 时间线事件类型 */
type CorrelatorEventKind = typeof TAKT_EVENT_TYPE_LONG_TASK | typeof TAKT_EVENT_TYPE_API_SLOW | typeof TAKT_EVENT_TYPE_FPS;

/** 时间线记录 */
interface CorrelatorTimelineRecord {
  kind: CorrelatorEventKind;
  atMs: number;
  durationMs: number;
  apiStartMs?: number;
  apiEndMs?: number;
  fps?: number;
}

/** 关联快照 */
export interface CorrelatorSnapshot {
  hasLongTaskInWindow: boolean;
  hasApiSlowInWindow: boolean;
}

/** 时间线（模块级可变，运行时网关） */
const timeline: CorrelatorTimelineRecord[] = [];

/**
 * 裁剪过期时间线
 * @param nowMs 当前 performance.now()
 */
function pruneTimeline(nowMs: number): void {
  const maxAgeMs = getCorrelationWindowMs() * 3;
  while (timeline.length > 0 && nowMs - timeline[0].atMs > maxAgeMs) {
    timeline.shift();
  }
}

/**
 * 记录 Long Task
 * @param atMs 发生时刻 performance.now()
 * @param durationMs 阻塞毫秒
 */
export function recordCorrelatorLongTask(atMs: number, durationMs: number): void {
  pruneTimeline(atMs);
  timeline.push({ kind: TAKT_EVENT_TYPE_LONG_TASK, atMs, durationMs });
}

/**
 * 记录 API 慢请求
 * @param apiStartMs 请求开始
 * @param apiEndMs 请求结束
 * @param durationMs 耗时
 */
export function recordCorrelatorApiSlow(apiStartMs: number, apiEndMs: number, durationMs: number): void {
  pruneTimeline(apiEndMs);
  timeline.push({
    kind: TAKT_EVENT_TYPE_API_SLOW,
    atMs: apiEndMs,
    durationMs,
    apiStartMs,
    apiEndMs,
  });
}

/**
 * 记录 FPS 采样窗口
 * @param windowEndMs 窗口结束时刻
 * @param windowMs 窗口长度
 * @param fps 帧率
 */
export function recordCorrelatorFpsWindow(windowEndMs: number, windowMs: number, fps: number): void {
  pruneTimeline(windowEndMs);
  timeline.push({
    kind: TAKT_EVENT_TYPE_FPS,
    atMs: windowEndMs,
    durationMs: windowMs,
    fps,
  });
}

/**
 * 请求窗口内是否存在 Long Task
 * @param startMs 开始时刻
 * @param endMs 结束时刻
 * @returns {boolean} 存在时为 true
 */
export function hasLongTaskBetween(startMs: number, endMs: number): boolean {
  const padMs = 100;
  return timeline.some((item) => {
    if (item.kind !== TAKT_EVENT_TYPE_LONG_TASK) {
      return false;
    }
    const taskEndMs = item.atMs;
    const taskStartMs = taskEndMs - item.durationMs;
    return taskStartMs <= endMs + padMs && taskEndMs >= startMs - padMs;
  });
}

/**
 * 时间窗口内是否存在慢接口
 * @param windowStartMs 窗口开始
 * @param windowEndMs 窗口结束
 * @returns {boolean} 存在时为 true
 */
export function hasApiSlowBetween(windowStartMs: number, windowEndMs: number): boolean {
  const slowMs = getApiSlowThresholdMs();
  return timeline.some((item) => {
    if (item.kind !== TAKT_EVENT_TYPE_API_SLOW) {
      return false;
    }
    return item.atMs >= windowStartMs && item.atMs <= windowEndMs && item.durationMs >= slowMs;
  });
}

/**
 * 获取关联快照（默认以当前时刻为窗口终点）
 * @param windowEndMs 窗口结束
 * @param windowMs 窗口长度
 * @returns {CorrelatorSnapshot} 快照
 */
export function getCorrelatorSnapshot(windowEndMs: number, windowMs: number): CorrelatorSnapshot {
  const windowStartMs = windowEndMs - windowMs;
  return {
    hasLongTaskInWindow: hasLongTaskBetween(windowStartMs, windowEndMs),
    hasApiSlowInWindow: hasApiSlowBetween(windowStartMs, windowEndMs),
  };
}

/**
 * 构建 API 慢请求诊断上下文
 * @param apiStartMs 请求开始
 * @param apiEndMs 请求结束
 * @returns {{ hasLongTaskInWindow: boolean; hasApiSlowInWindow: boolean }} 上下文
 */
export function buildApiSlowDiagnosisContext(
  apiStartMs: number,
  apiEndMs: number
): CorrelatorSnapshot {
  return {
    hasLongTaskInWindow: hasLongTaskBetween(apiStartMs, apiEndMs + 500),
    hasApiSlowInWindow: true,
  };
}

/**
 * 构建 FPS 掉帧诊断上下文
 * @param windowEndMs 采样窗口结束
 * @param windowMs 采样窗口长度
 * @returns {CorrelatorSnapshot} 上下文
 */
export function buildFpsDiagnosisContext(windowEndMs: number, windowMs: number): CorrelatorSnapshot {
  return getCorrelatorSnapshot(windowEndMs, windowMs);
}

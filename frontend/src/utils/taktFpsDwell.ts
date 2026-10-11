// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktFpsDwell.ts
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：页面停留 FPS 纯工具（分位 p50/p75、会话汇总；体验指标非性能告警）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 页面停留结束原因 */
export type TaktFpsDwellEndReason = 'route' | 'visibility' | 'unload';

/** 页面停留 FPS 会话 */
export interface TaktFpsDwellSession {
  /** SPA 路由路径 */
  routePath: string;
  /** 会话开始 performance.now() */
  startedAtMs: number;
  /** 采样窗口 FPS 序列 */
  fpsSamples: number[];
}

/** 页面停留 FPS 汇总 */
export interface TaktFpsDwellSummary {
  routePath: string;
  dwellMs: number;
  sampleCount: number;
  fpsP50: number;
  fpsP75: number;
  fpsMin: number;
  fpsMax: number;
  endReason: TaktFpsDwellEndReason;
}

/**
 * 计算 FPS 分位数（线性插值）
 * @param samples 样本（帧/秒）
 * @param percentile 分位 0～100（如 50、75）
 * @returns {number} 分位 FPS
 */
export function computeFpsPercentile(samples: readonly number[], percentile: number): number {
  if (!samples?.length) {
    return 0;
  }
  if (percentile <= 0) {
    return Math.min(...samples);
  }
  if (percentile >= 100) {
    return Math.max(...samples);
  }
  const sorted = [...samples].sort((a, b) => a - b);
  const index = (percentile / 100) * (sorted.length - 1);
  const lower = Math.floor(index);
  const upper = Math.ceil(index);
  if (lower === upper) {
    return sorted[lower];
  }
  const weight = index - lower;
  return sorted[lower] * (1 - weight) + sorted[upper] * weight;
}

/**
 * 汇总页面停留 FPS 会话
 * @param session 会话
 * @param endedAtMs 结束时刻 performance.now()
 * @param endReason 结束原因
 * @returns {TaktFpsDwellSummary | null} 样本为空时 null
 */
export function summarizeFpsDwellSession(
  session: TaktFpsDwellSession,
  endedAtMs: number,
  endReason: TaktFpsDwellEndReason
): TaktFpsDwellSummary | null {
  if (!session.fpsSamples.length) {
    return null;
  }
  const dwellMs = Math.max(0, Math.round(endedAtMs - session.startedAtMs));
  const fpsP50 = Math.round(computeFpsPercentile(session.fpsSamples, 50) * 10) / 10;
  const fpsP75 = Math.round(computeFpsPercentile(session.fpsSamples, 75) * 10) / 10;
  const fpsMin = Math.round(Math.min(...session.fpsSamples) * 10) / 10;
  const fpsMax = Math.round(Math.max(...session.fpsSamples) * 10) / 10;
  return {
    routePath: session.routePath,
    dwellMs,
    sampleCount: session.fpsSamples.length,
    fpsP50,
    fpsP75,
    fpsMin,
    fpsMax,
    endReason,
  };
}

/**
 * 格式化页面停留 FPS 体验日志主行
 * @param summary 汇总
 * @returns {string} 控制台一行摘要
 */
export function formatFpsDwellConsoleMessage(summary: TaktFpsDwellSummary): string {
  return [
    `页面停留 FPS`,
    `路由 ${summary.routePath}`,
    `停留 ${summary.dwellMs}ms`,
    `p50 ${summary.fpsP50}`,
    `p75 ${summary.fpsP75}`,
    `min ${summary.fpsMin}`,
    `样本 ${summary.sampleCount}`,
    `结束 ${summary.endReason}`,
    '体验补充（非性能告警，线上结合跳出率/交互判断）',
  ].join(' | ');
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/config
// 文件名称：eventTracking.ts
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：客户端性能监控阈值（W3C Long Tasks / RAIL / Google Core Web Vitals）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { parseEnvBoolean } from '@/utils/runtimeContext';

/** W3C Long Tasks API 采集下限（固定 50ms，Observer 仅产出 ≥50ms 条目） */
export const TAKT_LONG_TASK_MIN_MS = 50;

/** Long Task 监控 Warning（默认 300ms，严重主线程阻塞） */
export const TAKT_LONG_TASK_WARN_MS_DEFAULT = 300;

/** Long Task 监控 Error（默认 500ms，对齐 Google INP Poor） */
export const TAKT_LONG_TASK_ERROR_MS_DEFAULT = 500;

/** RAIL：单段事件处理预算（毫秒） */
export const TAKT_RAIL_EVENT_HANDLER_BUDGET_MS = 50;

/** RAIL：用户可见反馈预算（毫秒） */
export const TAKT_RAIL_FEEDBACK_BUDGET_MS = 100;

/** 交互日志入队处理预算（同 RAIL 事件处理 50ms） */
export const TAKT_EVENT_TRACK_PROCESS_BUDGET_MS = TAKT_RAIL_EVENT_HANDLER_BUDGET_MS;

/** Axios 响应监控处理预算（同 RAIL 反馈 100ms） */
export const TAKT_API_RESPONSE_PROCESS_BUDGET_MS = TAKT_RAIL_FEEDBACK_BUDGET_MS;

/** FPS 掉帧 warn 阈值（帧/秒；仅 VITE_FPS_DROP_ALERT_ENABLED=true 时告警） */
export const TAKT_FPS_WARN_THRESHOLD_DEFAULT = 30;

/** FPS 采样窗口（毫秒） */
export const TAKT_FPS_SAMPLE_MS_DEFAULT = 1000;

/** FPS 掉帧即时告警冷却（毫秒） */
export const TAKT_FPS_REPORT_COOLDOWN_MS_DEFAULT = 5000;

/** 页面停留 FPS 会话最短时长（毫秒，低于此不汇总上报） */
export const TAKT_FPS_DWELL_MIN_MS_DEFAULT = 1000;

/** CRUD API 慢请求 warn 阈值（毫秒，交互式应用可接受上限参考 Google 1s） */
export const TAKT_API_SLOW_MS_DEFAULT = 1000;

/** CRUD API 极慢 error 阈值（毫秒） */
export const TAKT_API_ERROR_MS_DEFAULT = 3000;

/** 事件类型：Long Task */
export const TAKT_EVENT_TYPE_LONG_TASK = 'longtask';

/** 事件类型：FPS 掉帧（可选即时告警） */
export const TAKT_EVENT_TYPE_FPS = 'fps';

/** 事件类型：页面停留 FPS 汇总（p50/p75，体验补充，非性能告警） */
export const TAKT_EVENT_TYPE_FPS_DWELL = 'fps-dwell';

/** 事件类型：API 慢请求 */
export const TAKT_EVENT_TYPE_API_SLOW = 'api-slow';

/** 事件类型：API HTTP 错误 */
export const TAKT_EVENT_TYPE_API_ERROR = 'api-error';

/** 事件类型：Web Vital（FCP / LCP / INP / CLS） */
export const TAKT_EVENT_TYPE_WEB_VITAL = 'web-vital';

/** 事件分类：性能 */
export const TAKT_EVENT_CATEGORY_PERFORMANCE = 'performance';

/** 事件分类：网络 */
export const TAKT_EVENT_CATEGORY_NETWORK = 'network';

/** 事件分类：加载 */
export const TAKT_EVENT_CATEGORY_LOADING = 'loading';

/** 事件分类：体验（FPS 停留等，非性能告警） */
export const TAKT_EVENT_CATEGORY_EXPERIENCE = 'experience';

/** FCP Warning（毫秒，Paint Timing 辅助指标） */
export const TAKT_FCP_WARN_MS_DEFAULT = 1800;

/** LCP Warning（毫秒，Google Needs Improvement 2.5s） */
export const TAKT_LCP_WARN_MS_DEFAULT = 2500;

/** LCP Error / Poor（毫秒，Google Core Web Vitals 报警线 4s） */
export const TAKT_LCP_ERROR_MS_DEFAULT = 4000;

/** INP Warning（毫秒，Google Needs Improvement / 监控 p75 >200ms） */
export const TAKT_INP_WARN_MS_DEFAULT = 200;

/** INP Error / Poor（毫秒，Google Core Web Vitals 报警线 500ms） */
export const TAKT_INP_ERROR_MS_DEFAULT = 500;

/** CLS Warning（Google Good 上限 0.1） */
export const TAKT_CLS_WARN_DEFAULT = 0.1;

/** CLS Error / Poor（Google Core Web Vitals 报警线 0.25） */
export const TAKT_CLS_ERROR_DEFAULT = 0.25;

/** 关联诊断默认时间窗（毫秒） */
export const TAKT_CORRELATION_WINDOW_MS_DEFAULT = 3000;

/** 追踪级别 warn */
export const TAKT_TRACKING_LEVEL_WARN = 1;

/** 追踪级别 error */
export const TAKT_TRACKING_LEVEL_ERROR = 2;

/** 追踪级别 experience（体验补充，非告警） */
export const TAKT_TRACKING_LEVEL_EXPERIENCE = 0;

/** 上报批量默认条数 */
export const TAKT_EVENT_TRACK_BATCH_SIZE_DEFAULT = 20;

/** 上报 flush 默认间隔（毫秒） */
export const TAKT_EVENT_TRACK_FLUSH_MS_DEFAULT = 10000;

/** 单次上报最大条数（与后端一致） */
export const TAKT_EVENT_TRACK_BATCH_MAX = 50;

/** 排除 API 监控的路径片段（避免 track-batch 递归） */
export const TAKT_API_TRACK_EXCLUDED_PATH = 'TaktTrackingLogs/track-batch';

/**
 * 是否启用客户端性能监控总开关
 * @returns {boolean} 启用时为 true
 */
export function isEventTrackingEnabled(): boolean {
  return parseEnvBoolean(import.meta.env.VITE_EVENT_TRACKING_ENABLED, true);
}

/**
 * 是否启用 Long Task 监控
 * @returns {boolean} 启用时为 true
 */
export function isLongTaskMonitorEnabled(): boolean {
  return isEventTrackingEnabled()
    && parseEnvBoolean(import.meta.env.VITE_LONG_TASK_MONITOR_ENABLED, true);
}

/**
 * 是否启用 FPS 监控
 * @returns {boolean} 启用时为 true
 */
export function isFpsMonitorEnabled(): boolean {
  return isEventTrackingEnabled()
    && parseEnvBoolean(import.meta.env.VITE_FPS_MONITOR_ENABLED, true);
}

/**
 * 是否启用 CRUD API 监控
 * @returns {boolean} 启用时为 true
 */
export function isApiPerformanceTrackEnabled(): boolean {
  return isEventTrackingEnabled()
    && parseEnvBoolean(import.meta.env.VITE_API_PERF_TRACK_ENABLED, true);
}

/**
 * 是否启用后端交互日志上报
 * @returns {boolean} 启用时为 true
 */
export function isEventTrackingReportEnabled(): boolean {
  const reportFlag = import.meta.env.VITE_EVENT_TRACKING_REPORT_ENABLED
    ?? import.meta.env.VITE_LONG_TASK_REPORT_ENABLED;
  return isEventTrackingEnabled()
    && parseEnvBoolean(reportFlag, true);
}

/**
 * 解析 Long Task warn 阈值（毫秒）
 * @returns {number} 毫秒，至少 50
 */
export function getLongTaskWarnThresholdMs(): number {
  const raw = import.meta.env.VITE_LONG_TASK_WARN_MS?.trim();
  if (!raw) {
    return TAKT_LONG_TASK_WARN_MS_DEFAULT;
  }
  const value = Number(raw);
  if (Number.isNaN(value) || value < TAKT_LONG_TASK_MIN_MS) {
    return TAKT_LONG_TASK_MIN_MS;
  }
  return value;
}

/**
 * 解析 Long Task error 阈值（毫秒）
 * @returns {number} 毫秒
 */
export function getLongTaskErrorThresholdMs(): number {
  const warnMs = getLongTaskWarnThresholdMs();
  const raw = import.meta.env.VITE_LONG_TASK_ERROR_MS?.trim();
  if (!raw) {
    return Math.max(TAKT_LONG_TASK_ERROR_MS_DEFAULT, warnMs);
  }
  const value = Number(raw);
  if (Number.isNaN(value) || value < warnMs) {
    return warnMs;
  }
  return value;
}

/**
 * 解析 FPS warn 阈值（帧/秒）
 * @returns {number} FPS，默认 30
 */
export function getFpsWarnThreshold(): number {
  const raw = Number(import.meta.env.VITE_FPS_WARN_THRESHOLD || TAKT_FPS_WARN_THRESHOLD_DEFAULT);
  if (Number.isNaN(raw) || raw <= 0) {
    return TAKT_FPS_WARN_THRESHOLD_DEFAULT;
  }
  return raw;
}

/**
 * 解析 FPS 采样窗口（毫秒）
 * @returns {number} 毫秒
 */
export function getFpsSampleMs(): number {
  const raw = Number(import.meta.env.VITE_FPS_SAMPLE_MS || TAKT_FPS_SAMPLE_MS_DEFAULT);
  if (Number.isNaN(raw) || raw < 500) {
    return TAKT_FPS_SAMPLE_MS_DEFAULT;
  }
  return raw;
}

/**
 * 是否启用 FPS 掉帧即时告警（默认关；主路径为页面停留 p50/p75 体验上报）
 * @returns {boolean} 启用时为 true
 */
export function isFpsDropAlertEnabled(): boolean {
  return isFpsMonitorEnabled()
    && parseEnvBoolean(import.meta.env.VITE_FPS_DROP_ALERT_ENABLED, false);
}

/**
 * 解析页面停留 FPS 最短汇总时长（毫秒）
 * @returns {number} 毫秒
 */
export function getFpsDwellMinMs(): number {
  const raw = Number(import.meta.env.VITE_FPS_DWELL_MIN_MS || TAKT_FPS_DWELL_MIN_MS_DEFAULT);
  if (Number.isNaN(raw) || raw < 500) {
    return TAKT_FPS_DWELL_MIN_MS_DEFAULT;
  }
  return raw;
}

/**
 * 解析 FPS 上报冷却（毫秒）
 * @returns {number} 毫秒
 */
export function getFpsReportCooldownMs(): number {
  const raw = Number(import.meta.env.VITE_FPS_REPORT_COOLDOWN_MS || TAKT_FPS_REPORT_COOLDOWN_MS_DEFAULT);
  if (Number.isNaN(raw) || raw < 1000) {
    return TAKT_FPS_REPORT_COOLDOWN_MS_DEFAULT;
  }
  return raw;
}

/**
 * 解析 API 慢请求阈值（毫秒）
 * @returns {number} 毫秒，默认 1000
 */
export function getApiSlowThresholdMs(): number {
  const raw = Number(import.meta.env.VITE_API_SLOW_MS || TAKT_API_SLOW_MS_DEFAULT);
  if (Number.isNaN(raw) || raw <= 0) {
    return TAKT_API_SLOW_MS_DEFAULT;
  }
  return raw;
}

/**
 * 解析 API 极慢 error 阈值（毫秒）
 * @returns {number} 毫秒
 */
export function getApiErrorThresholdMs(): number {
  const slowMs = getApiSlowThresholdMs();
  const raw = Number(import.meta.env.VITE_API_ERROR_MS || TAKT_API_ERROR_MS_DEFAULT);
  if (Number.isNaN(raw) || raw < slowMs) {
    return Math.max(TAKT_API_ERROR_MS_DEFAULT, slowMs);
  }
  return raw;
}

/**
 * 解析上报批量条数
 * @returns {number} 条数
 */
export function getEventTrackingBatchSize(): number {
  const raw = Number(
    import.meta.env.VITE_EVENT_TRACKING_BATCH_SIZE
    || import.meta.env.VITE_LONG_TASK_REPORT_BATCH_SIZE
    || TAKT_EVENT_TRACK_BATCH_SIZE_DEFAULT
  );
  if (Number.isNaN(raw) || raw <= 0) {
    return TAKT_EVENT_TRACK_BATCH_SIZE_DEFAULT;
  }
  return Math.min(TAKT_EVENT_TRACK_BATCH_MAX, Math.floor(raw));
}

/**
 * 是否启用 Web Vitals 监控（FCP / LCP / INP / CLS）
 * @returns {boolean} 启用时为 true
 */
export function isWebVitalsMonitorEnabled(): boolean {
  return isEventTrackingEnabled()
    && parseEnvBoolean(import.meta.env.VITE_WEB_VITALS_MONITOR_ENABLED, true);
}

/**
 * 解析关联诊断时间窗（毫秒）
 * @returns {number} 毫秒
 */
export function getCorrelationWindowMs(): number {
  const raw = Number(import.meta.env.VITE_CORRELATION_WINDOW_MS || TAKT_CORRELATION_WINDOW_MS_DEFAULT);
  if (Number.isNaN(raw) || raw < 1000) {
    return TAKT_CORRELATION_WINDOW_MS_DEFAULT;
  }
  return raw;
}

/**
 * 解析 FCP Warning 阈值（毫秒）
 * @returns {number} 毫秒
 */
export function getFcpWarnThresholdMs(): number {
  const raw = Number(import.meta.env.VITE_FCP_WARN_MS || TAKT_FCP_WARN_MS_DEFAULT);
  if (Number.isNaN(raw) || raw <= 0) {
    return TAKT_FCP_WARN_MS_DEFAULT;
  }
  return raw;
}

/**
 * 解析 LCP Warning 阈值（毫秒）
 * @returns {number} 毫秒
 */
export function getLcpWarnThresholdMs(): number {
  const raw = Number(import.meta.env.VITE_LCP_WARN_MS || TAKT_LCP_WARN_MS_DEFAULT);
  if (Number.isNaN(raw) || raw <= 0) {
    return TAKT_LCP_WARN_MS_DEFAULT;
  }
  return raw;
}

/**
 * 解析 LCP Error / Poor 阈值（毫秒）
 * @returns {number} 毫秒
 */
export function getLcpErrorThresholdMs(): number {
  const warnMs = getLcpWarnThresholdMs();
  const raw = Number(import.meta.env.VITE_LCP_ERROR_MS || TAKT_LCP_ERROR_MS_DEFAULT);
  if (Number.isNaN(raw) || raw < warnMs) {
    return Math.max(TAKT_LCP_ERROR_MS_DEFAULT, warnMs);
  }
  return raw;
}

/**
 * 解析 INP Warning 阈值（毫秒）
 * @returns {number} 毫秒
 */
export function getInpWarnThresholdMs(): number {
  const raw = Number(import.meta.env.VITE_INP_WARN_MS || TAKT_INP_WARN_MS_DEFAULT);
  if (Number.isNaN(raw) || raw <= 0) {
    return TAKT_INP_WARN_MS_DEFAULT;
  }
  return raw;
}

/**
 * 解析 INP Error / Poor 阈值（毫秒）
 * @returns {number} 毫秒
 */
export function getInpErrorThresholdMs(): number {
  const warnMs = getInpWarnThresholdMs();
  const raw = Number(import.meta.env.VITE_INP_ERROR_MS || TAKT_INP_ERROR_MS_DEFAULT);
  if (Number.isNaN(raw) || raw < warnMs) {
    return Math.max(TAKT_INP_ERROR_MS_DEFAULT, warnMs);
  }
  return raw;
}

/**
 * 解析 CLS Warning 阈值
 * @returns {number} CLS 分数
 */
export function getClsWarnThreshold(): number {
  const raw = Number(import.meta.env.VITE_CLS_WARN || TAKT_CLS_WARN_DEFAULT);
  if (Number.isNaN(raw) || raw <= 0) {
    return TAKT_CLS_WARN_DEFAULT;
  }
  return raw;
}

/**
 * 解析 CLS Error / Poor 阈值
 * @returns {number} CLS 分数
 */
export function getClsErrorThreshold(): number {
  const warn = getClsWarnThreshold();
  const raw = Number(import.meta.env.VITE_CLS_ERROR || TAKT_CLS_ERROR_DEFAULT);
  if (Number.isNaN(raw) || raw < warn) {
    return Math.max(TAKT_CLS_ERROR_DEFAULT, warn);
  }
  return raw;
}

/**
 * 解析上报 flush 间隔（毫秒）
 * @returns {number} 毫秒
 */
export function getEventTrackingFlushMs(): number {
  const raw = Number(
    import.meta.env.VITE_EVENT_TRACKING_FLUSH_MS
    || import.meta.env.VITE_LONG_TASK_REPORT_FLUSH_MS
    || TAKT_EVENT_TRACK_FLUSH_MS_DEFAULT
  );
  if (Number.isNaN(raw) || raw <= 0) {
    return TAKT_EVENT_TRACK_FLUSH_MS_DEFAULT;
  }
  return raw;
}

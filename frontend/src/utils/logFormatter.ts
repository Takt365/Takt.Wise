// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：logFormatter.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：前端日志统一格式化（错误序列化、条目构建、控制台输出）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { LogContext, LogEntry, LogErrorInfo, LoggerConfig } from '@/types/logger.types';
import {
  buildFormatterRuntimeMeta,
  formatEntriesBatchForReport,
  safeStringifyForLog,
} from '@/utils/taktFormatterCommon';
import { maskForLogging } from '@/utils/mask';

/**
 * 前端日志级别（数值越大优先级越高）
 */
export enum LogLevel {
  /** 调试信息（开发环境） */
  Debug = 0,
  /** 一般信息 */
  Info = 1,
  /** 警告（可恢复异常） */
  Warn = 2,
  /** 错误（业务或运行时失败） */
  Error = 3,
  /** 致命错误（需立即上报） */
  Fatal = 4,
}

/**
 * 日志级别名称映射
 */
const LOG_LEVEL_LABELS: Record<LogLevel, string> = {
  [LogLevel.Debug]: 'DEBUG',
  [LogLevel.Info]: 'INFO',
  [LogLevel.Warn]: 'WARN',
  [LogLevel.Error]: 'ERROR',
  [LogLevel.Fatal]: 'FATAL',
};

/**
 * 对日志上下文脱敏（password / token / phone 等敏感字段）
 * @param {LogContext} [context] 原始上下文
 * @returns {LogContext | undefined} 脱敏后的上下文
 */
export function sanitizeLogContext(context?: LogContext): LogContext | undefined {
  if (!context) {
    return undefined;
  }

  return maskForLogging(context) as LogContext;
}

/**
 * 对错误信息脱敏
 * @param {LogErrorInfo} [error] 原始错误信息
 * @returns {LogErrorInfo | undefined} 脱敏后的错误信息
 */
export function sanitizeLogErrorInfo(error?: LogErrorInfo): LogErrorInfo | undefined {
  if (!error) {
    return undefined;
  }

  return maskForLogging(error) as LogErrorInfo;
}

/**
 * 将 unknown 错误序列化为统一结构
 * @param {unknown} error 原始错误
 * @returns {LogErrorInfo | undefined} 序列化结果
 */
export function serializeLogError(error: unknown): LogErrorInfo | undefined {
  if (error === undefined || error === null) {
    return undefined;
  }

  if (error instanceof Error) {
    return sanitizeLogErrorInfo({
      name: error.name,
      message: error.message,
      stack: error.stack,
    });
  }

  if (typeof error === 'string') {
    return sanitizeLogErrorInfo({
      name: 'Error',
      message: error,
    });
  }

  try {
    const maskedPayload = maskForLogging(error);
    return sanitizeLogErrorInfo({
      name: 'Error',
      message: safeStringifyForLog(maskedPayload),
    });
  } catch {
    return sanitizeLogErrorInfo({
      name: 'Error',
      message: String(error),
    });
  }
}

/**
 * 构建标准日志条目
 * @param {LogLevel} level 日志级别
 * @param {string} message 主消息
 * @param {LoggerConfig} config 日志器配置
 * @param {LogContext} [context] 业务上下文
 * @param {unknown} [error] 关联错误
 * @param {string[]} [tags] 标签
 * @returns {LogEntry} 标准日志条目
 */
export function buildLogEntry(
  level: LogLevel,
  message: string,
  config: LoggerConfig,
  context?: LogContext,
  error?: unknown,
  tags?: string[]
): LogEntry {
  if (!message?.trim()) {
    throw new Error('buildLogEntry: message 不能为空')
  }
  if (!config) {
    throw new Error('buildLogEntry: config 不能为空')
  }
  const serializedError = serializeLogError(error);
  const runtimeMeta = buildFormatterRuntimeMeta(config);

  return {
    level,
    message,
    ...runtimeMeta,
    userAgent: typeof navigator !== 'undefined' ? navigator.userAgent : '',
    context: sanitizeLogContext(context),
    error: sanitizeLogErrorInfo(serializedError),
    tags,
  };
}

/**
 * 将 ISO8601 时间戳格式化为 Serilog 文件模板风格
 * @param {string} iso ISO8601 时间戳
 * @returns {string} 如 2026-06-23 14:30:00.123 +08:00
 */
function formatFileTimestamp(iso: string): string {
  const date = new Date(iso);
  if (Number.isNaN(date.getTime())) {
    return iso;
  }
  const pad = (value: number, length = 2): string => String(value).padStart(length, '0');
  const offsetMin = -date.getTimezoneOffset();
  const sign = offsetMin >= 0 ? '+' : '-';
  const absMin = Math.abs(offsetMin);
  const offsetHours = pad(Math.floor(absMin / 60));
  const offsetMinutes = pad(absMin % 60);
  return `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())} ${pad(date.getHours())}:${pad(date.getMinutes())}:${pad(date.getSeconds())}.${pad(date.getMilliseconds(), 3)} ${sign}${offsetHours}:${offsetMinutes}`;
}

/**
 * 格式化日志条目为本地文件行（与后端 Serilog FileOutputTemplate 对齐）
 * @param {LogEntry} entry 日志条目
 * @returns {string} 单行或多行文本（含换行结尾）
 */
export function formatLogEntryForFile(entry: LogEntry): string {
  const levelLabel = LOG_LEVEL_LABELS[entry.level] ?? 'LOG';
  const timestamp = formatFileTimestamp(entry.timestamp);
  let line = `${timestamp} [${levelLabel}] ${entry.message}`;
  const contextParts: string[] = [];
  if (entry.context?.module) {
    contextParts.push(`module=${entry.context.module}`);
  }
  if (entry.context?.action) {
    contextParts.push(`action=${entry.context.action}`);
  }
  if (entry.context?.tenantCode) {
    contextParts.push(`tenant=${entry.context.tenantCode}`);
  }
  if (entry.context?.companyCode) {
    contextParts.push(`company=${entry.context.companyCode}`);
  }
  if (entry.context?.userId) {
    contextParts.push(`uid=${entry.context.userId}`);
  }
  if (contextParts.length > 0) {
    line += ` | ${contextParts.join(' ')}`;
  }
  if (entry.error?.stack) {
    line += `\n${entry.error.stack}`;
  } else if (entry.error) {
    line += `\n${entry.error.name}: ${entry.error.message}`;
  }
  return `${line}\n`;
}

/**
 * 格式化日志条目为控制台可读字符串
 * @param {LogEntry} entry 日志条目
 * @returns {string} 格式化文本
 */
export function formatLogEntryForConsole(entry: LogEntry): string {
  const levelLabel = LOG_LEVEL_LABELS[entry.level] ?? 'LOG';
  const moduleLabel = entry.context?.module ? `[${entry.context.module}]` : '';
  const actionLabel = entry.context?.action ? `(${entry.context.action})` : '';
  const base = `[${levelLabel}]${moduleLabel}${actionLabel} ${entry.message}`;

  if (!entry.error) {
    return base;
  }

  return `${base} | ${entry.error.name}: ${entry.error.message}`;
}

/**
 * 将日志条目格式化为上报 JSON
 * @param {LogEntry[]} entries 日志条目列表
 * @returns {string} JSON 字符串
 */
export function formatLogEntriesForReport(entries: LogEntry[]): string {
  const sanitizedEntries = maskForLogging(entries) as LogEntry[];
  return formatEntriesBatchForReport(sanitizedEntries);
}

/**
 * 解析环境变量中的日志级别
 * @param {string | undefined} value 环境变量值
 * @param {LogLevel} defaultLevel 默认级别
 * @returns {LogLevel} 解析结果
 */
export function parseLogLevelFromEnv(value: string | undefined, defaultLevel: LogLevel): LogLevel {
  switch ((value ?? '').toLowerCase()) {
    case 'debug':
      return LogLevel.Debug;
    case 'info':
      return LogLevel.Info;
    case 'warn':
    case 'warning':
      return LogLevel.Warn;
    case 'error':
      return LogLevel.Error;
    case 'fatal':
      return LogLevel.Fatal;
    default:
      return defaultLevel;
  }
}

/**
 * 是否应输出指定级别日志
 * @param {LogLevel} level 当前级别
 * @param {LogLevel} minLevel 最低级别
 * @returns {boolean} 是否输出
 */
export function shouldLogLevel(level: LogLevel, minLevel: LogLevel): boolean {
  return level >= minLevel;
}

/** 日志 Detail 默认最大采样条数（与后端 TaktLogFormatter.SampleForLog 默认 40 对齐） */
export const LOG_SAMPLE_DEFAULT_MAX = 40

/**
 * 采样列表（用于结构化日志 Detail，避免 JSON 过长；与后端 SampleForLog 对齐）。
 * @template T 元素类型
 * @param items 原始列表
 * @param maxSample 最大条数，默认 40
 * @returns sample 与 total
 */
export function sampleForLog<T>(
  items: readonly T[],
  maxSample: number = LOG_SAMPLE_DEFAULT_MAX
): { sample: T[]; total: number } {
  if (items == null) {
    throw new Error('sampleForLog: items 不能为空')
  }
  if (maxSample <= 0) {
    throw new Error('sampleForLog: maxSample 必须大于 0')
  }
  if (items.length === 0) {
    return { sample: [], total: 0 }
  }
  return { sample: items.slice(0, maxSample), total: items.length }
}

/** 性能监控 action，控制台需展开可读归因 */
const PERFORMANCE_CONSOLE_ACTIONS = new Set([
  'web-vital',
  'longtask',
  'api-slow',
  'api-error',
  'fps-dwell',
]);

/**
 * 将性能监控 context 格式化为控制台多行归因（避免仅显示 {context:…}）
 * @param context 日志上下文
 * @returns {string[]} 归因行
 */
export function formatPerformanceInsightLines(context?: LogContext): string[] {
  if (!context?.action || !PERFORMANCE_CONSOLE_ACTIONS.has(context.action)) {
    return [];
  }
  const lines: string[] = [];
  if (context.action === 'web-vital') {
    if (context.reportResult) {
      lines.push(`结果: ${context.reportResult}`);
    }
    if (context.likelyCause) {
      lines.push(`主因: ${context.likelyCause}`);
    }
    if (context.problemLocation) {
      lines.push(`定位: ${context.problemLocation}`);
    }
    if (context.actionHint) {
      lines.push(`建议: ${context.actionHint}`);
    }
    if (context.grade) {
      const unit = context.metric === 'cls' ? '' : 'ms';
      lines.push(`等级: ${context.grade} | 超出阈值: ${context.excessMs ?? '-'}${unit}`);
    }
    const nav = context.navigation as Record<string, number> | undefined;
    if (nav && typeof nav.ttfbMs === 'number') {
      lines.push(
        `Navigation: TTFB=${nav.ttfbMs}ms DNS=${nav.dnsMs ?? '-'}ms TCP=${nav.tcpMs ?? '-'}ms `
        + `response=${nav.responseMs ?? '-'}ms DOM=${nav.domInteractiveMs ?? '-'}ms `
        + `DCL=${nav.domContentLoadedMs ?? '-'}ms load=${nav.loadEventEndMs ?? '-'}ms`
      );
    } else {
      lines.push('Navigation: 未采样到（请看 Network 瀑布图 / Performance 面板）');
    }
    const resources = context.slowResources as Array<{
      name: string;
      durationMs: number;
      initiatorType: string;
    }> | undefined;
    if (resources?.length) {
      lines.push(
        `慢资源: ${resources.map((item) => `${item.name} ${item.durationMs}ms [${item.initiatorType}]`).join(' · ')}`
      );
    } else if (context.metric === 'fcp' || context.metric === 'lcp') {
      lines.push('慢资源: dev 下多为 Vite .ts 模块编译排队；生产请用 build 后 Network 看 .js chunk');
    }
    if (typeof context.longTaskTotalMsBefore === 'number' && context.longTaskTotalMsBefore > 0) {
      lines.push(`LongTask(FCP前累计): ${context.longTaskTotalMsBefore}ms`);
    }
    if (context.isDev) {
      lines.push('环境: development — Vite 编译/HMR 会拉高 FCP，请用 production build 复测');
    }
    if (context.routePath) {
      lines.push(`路由: ${context.routePath}`);
    }
    return lines;
  }
  if (context.action === 'fps-dwell') {
    if (context.routePath) {
      lines.push(`路由: ${context.routePath}`);
    }
    if (typeof context.dwellMs === 'number') {
      lines.push(`停留: ${context.dwellMs}ms`);
    }
    if (typeof context.fpsP50 === 'number') {
      lines.push(`FPS p50: ${context.fpsP50} | p75: ${context.fpsP75 ?? '-'} | min: ${context.fpsMin ?? '-'}`);
    }
    if (context.endReason) {
      lines.push(`结束: ${context.endReason}（visibility hidden / 路由切换 / 卸载）`);
    }
    if (context.actionHint) {
      lines.push(`解读: ${context.actionHint}`);
    }
    return lines;
  }
  if (context.action === 'longtask') {
    if (context.problemLocation) {
      lines.push(`定位: ${context.problemLocation}`);
    }
    if (context.actionHint) {
      lines.push(`建议: ${context.actionHint}`);
    }
    if (typeof context.durationMs === 'number') {
      lines.push(`阻塞: ${context.durationMs}ms`);
    }
    const attributions = context.attributions as Array<{
      containerType?: string;
      containerName?: string;
      containerSrc?: string;
      durationMs?: number;
    }> | undefined;
    if (attributions?.length) {
      const primary = attributions[0];
      lines.push(
        `归因: ${primary.containerType ?? 'unknown'} / ${primary.containerName || primary.containerSrc || '-'}`
      );
    }
    if (context.route) {
      lines.push(`路由: ${context.route}`);
    }
    return lines;
  }
  if (context.action === 'api-slow' || context.action === 'api-error') {
    if (context.problemLocation) {
      lines.push(`定位: ${context.problemLocation}`);
    }
    if (context.actionHint) {
      lines.push(`建议: ${context.actionHint}`);
    }
    if (context.method && context.url) {
      lines.push(`接口: ${context.method} ${context.url}`);
    }
    if (typeof context.durationMs === 'number') {
      lines.push(`耗时: ${context.durationMs}ms`);
    }
    if (context.status) {
      lines.push(`状态: ${context.status}`);
    }
    return lines;
  }
  return lines;
}

/**
 * 性能类日志写入控制台（主行 + 归因块 + 慢资源表）
 * @param text 主行文本
 * @param entry 日志条目
 * @param writer 控制台方法
 */
function writePerformanceLogToConsole(
  text: string,
  entry: LogEntry,
  writer: (...args: unknown[]) => void
): void {
  const insightLines = formatPerformanceInsightLines(entry.context);
  writer(text);
  if (insightLines.length > 0) {
    writer('[性能归因 — 可直接阅读，无需展开 context]');
    insightLines.forEach((line) => writer(`  ${line}`));
    const resources = entry.context?.slowResources;
    if (Array.isArray(resources) && resources.length > 0) {
      writer('[慢资源明细]');
      console.table(resources);
    }
  }
  if (entry.error) {
    writer(`${entry.error.name}: ${entry.error.message}`);
    if (entry.error.stack) {
      writer(entry.error.stack);
    }
  }
}

/**
 * 输出到浏览器控制台
 * @param {LogEntry} entry 日志条目
 */
export function writeLogEntryToConsole(entry: LogEntry): void {
  const text = formatLogEntryForConsole(entry);
  const isPerformanceLog = entry.context?.action
    && PERFORMANCE_CONSOLE_ACTIONS.has(entry.context.action);
  const detail =
    !isPerformanceLog && (entry.context || entry.error)
      ? (maskForLogging({ context: entry.context, error: entry.error }) as {
          context?: LogContext;
          error?: LogErrorInfo;
        })
      : undefined;

  switch (entry.level) {
    case LogLevel.Debug:
      if (isPerformanceLog) {
        writePerformanceLogToConsole(text, entry, console.debug.bind(console));
      } else {
        console.debug(text, detail);
      }
      break;
    case LogLevel.Info:
      if (isPerformanceLog) {
        writePerformanceLogToConsole(text, entry, console.info.bind(console));
      } else {
        console.info(text, detail);
      }
      break;
    case LogLevel.Warn:
      if (isPerformanceLog) {
        writePerformanceLogToConsole(text, entry, console.warn.bind(console));
      } else {
        console.warn(text, detail);
      }
      break;
    case LogLevel.Error:
    case LogLevel.Fatal:
      if (isPerformanceLog) {
        writePerformanceLogToConsole(text, entry, console.error.bind(console));
      } else {
        console.error(text, detail);
      }
      break;
    default:
      console.log(text, detail);
  }
}

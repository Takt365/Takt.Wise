// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/config
// 文件名称：viteLogFileWriter.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：Vite 开发服务器本地日志落盘（分级目录 logs/，与后端 Serilog 结构对齐）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { appendFileSync, existsSync, mkdirSync, readdirSync, unlinkSync } from 'node:fs';
import { dirname, join } from 'node:path';

/** 客户端日志采集端点（仅 Vite dev serve 内部使用） */
export const TAKT_CLIENT_LOG_INGEST_PATH = '/__takt/client-logs';

/** 日志级别（与 frontend LogLevel 数值对齐） */
const LogLevel = {
  Debug: 0,
  Info: 1,
  Warn: 2,
  Error: 3,
  Fatal: 4,
} as const;

/** 落盘用日志条目（与 LogEntry 结构对齐） */
interface ClientLogEntry {
  level: number;
  message: string;
  timestamp: string;
  context?: {
    module?: string;
    action?: string;
    tenantCode?: string;
    companyCode?: string;
    userId?: string;
  };
  error?: {
    name: string;
    message: string;
    stack?: string;
  };
}

const LEVEL_LABELS = ['DEBUG', 'INFO', 'WARN', 'ERROR', 'FATAL'];

/** 各级别日志保留天数（与后端 appsettings Serilog retainedFileCountLimit 对齐） */
const RETENTION_DAYS = {
  combined: 30,
  debug: 7,
  information: 30,
  warning: 60,
  error: 90,
} as const;

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
 * 格式化单条日志为文件行（与后端 Serilog FileOutputTemplate 对齐）
 * @param {ClientLogEntry} entry 日志条目
 * @returns {string} 文本行
 */
function formatLogEntryForFile(entry: ClientLogEntry): string {
  const levelLabel = LEVEL_LABELS[entry.level] ?? 'LOG';
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
 * 确保目录存在
 * @param {string} dir 目录路径
 */
function ensureDir(dir: string): void {
  if (!existsSync(dir)) {
    mkdirSync(dir, { recursive: true });
  }
}

/**
 * 从 ISO8601 时间戳提取日期键（yyyy-MM-dd）
 * @param {string} iso ISO8601 时间戳
 * @returns {string} 日期键
 */
function dateKeyFromTimestamp(iso: string): string {
  const date = new Date(iso);
  if (Number.isNaN(date.getTime())) {
    return new Date().toISOString().slice(0, 10);
  }
  return date.toISOString().slice(0, 10);
}

/**
 * 解析当日各级别日志文件路径
 * @param {string} logsDir 日志根目录
 * @param {string} dateKey 日期键 yyyy-MM-dd
 * @returns {Record<string, string>} 路径映射
 */
function resolveLogPaths(logsDir: string, dateKey: string): Record<string, string> {
  return {
    combined: join(logsDir, `takt-${dateKey}.log`),
    debug: join(logsDir, 'debug', `debug-${dateKey}.log`),
    information: join(logsDir, 'information', `information-${dateKey}.log`),
    warning: join(logsDir, 'warning', `warning-${dateKey}.log`),
    error: join(logsDir, 'error', `error-${dateKey}.log`),
  };
}

/**
 * 追加文本到日志文件
 * @param {string} filePath 文件路径
 * @param {string} content 文本内容
 */
function appendToFile(filePath: string, content: string): void {
  ensureDir(dirname(filePath));
  appendFileSync(filePath, content, 'utf8');
}

/**
 * 按前缀与保留天数清理过期日志文件
 * @param {string} dir 目录
 * @param {number} maxDays 保留天数
 * @param {string} prefix 文件名前缀
 */
function pruneDir(dir: string, maxDays: number, prefix: string): void {
  if (!existsSync(dir)) {
    return;
  }
  const now = Date.now();
  for (const name of readdirSync(dir)) {
    if (!name.startsWith(prefix) || !name.endsWith('.log')) {
      continue;
    }
    const match = name.match(/(\d{4}-\d{2}-\d{2})/);
    if (!match) {
      continue;
    }
    const fileTime = new Date(match[1]).getTime();
    if (Number.isNaN(fileTime)) {
      continue;
    }
    const ageDays = (now - fileTime) / 86_400_000;
    if (ageDays > maxDays) {
      try {
        unlinkSync(join(dir, name));
      } catch {
        // 忽略清理失败
      }
    }
  }
}

/**
 * 清理过期日志文件
 * @param {string} logsDir 日志根目录
 */
function pruneOldLogFiles(logsDir: string): void {
  pruneDir(logsDir, RETENTION_DAYS.combined, 'takt-');
  pruneDir(join(logsDir, 'debug'), RETENTION_DAYS.debug, 'debug-');
  pruneDir(join(logsDir, 'information'), RETENTION_DAYS.information, 'information-');
  pruneDir(join(logsDir, 'warning'), RETENTION_DAYS.warning, 'warning-');
  pruneDir(join(logsDir, 'error'), RETENTION_DAYS.error, 'error-');
}

/**
 * 将客户端日志条目写入本地 logs 目录（分级 + 综合）
 * @param {string} logsDir 日志根目录（通常为 frontend/logs）
 * @param {ClientLogEntry[]} entries 日志条目
 */
export function writeClientLogEntriesToFiles(logsDir: string, entries: ClientLogEntry[]): void {
  if (!logsDir?.trim()) {
    throw new Error('writeClientLogEntriesToFiles: logsDir 不能为空');
  }
  if (!entries?.length) {
    return;
  }
  ensureDir(logsDir);
  for (const entry of entries) {
    const line = formatLogEntryForFile(entry);
    const dateKey = dateKeyFromTimestamp(entry.timestamp);
    const paths = resolveLogPaths(logsDir, dateKey);
    appendToFile(paths.combined, line);
    if (entry.level >= LogLevel.Debug) {
      appendToFile(paths.debug, line);
    }
    if (entry.level >= LogLevel.Info) {
      appendToFile(paths.information, line);
    }
    if (entry.level >= LogLevel.Warn) {
      appendToFile(paths.warning, line);
    }
    if (entry.level >= LogLevel.Error) {
      appendToFile(paths.error, line);
    }
  }
  pruneOldLogFiles(logsDir);
}

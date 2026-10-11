// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：logger.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：前端通用日志器（运行时网关：统一采集、格式化脱敏、统一上报；非纯工具模块）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { App } from 'vue';
import type { LogContext, Logger, LoggerConfig } from '@/types/logger.types';
import {
  LogLevel,
  buildLogEntry,
  parseLogLevelFromEnv,
  shouldLogLevel,
  writeLogEntryToConsole,
} from '@/utils/logFormatter';
import type { LogReporter } from '@/utils/logReporter';
import type { LogFileSink } from '@/utils/logFileSink';
import {
  createAppMeta,
  mergeRuntimeContext,
  parseEnvBoolean,
  readViteEnv,
  setRuntimeRouter,
} from '@/utils/runtimeContext';
import type { Router } from 'vue-router';

/** 开发环境默认本地落盘 URL（与 log-file-sink 一致，避免静态 import 该模块） */
const TAKT_CLIENT_LOG_DEFAULT_FILE_URL = '/__takt/client-logs';

/**
 * 读取默认日志配置
 * @returns {LoggerConfig} 默认配置
 */
function createDefaultLoggerConfig(): LoggerConfig {
  const env = readViteEnv();
  const isDev = Boolean(env.DEV);
  return {
    minLevel: parseLogLevelFromEnv(env.VITE_LOG_MIN_LEVEL, isDev ? LogLevel.Debug : LogLevel.Info),
    enableConsole: parseEnvBoolean(env.VITE_LOG_ENABLE_CONSOLE, isDev),
    enableFile: parseEnvBoolean(env.VITE_LOG_ENABLE_FILE, isDev),
    fileUrl: env.VITE_LOG_FILE_URL?.trim() || (isDev ? TAKT_CLIENT_LOG_DEFAULT_FILE_URL : undefined),
    enableReport: parseEnvBoolean(env.VITE_LOG_ENABLE_REPORT, !isDev),
    reportUrl: env.VITE_LOG_REPORT_URL || undefined,
    batchSize: Number(env.VITE_LOG_BATCH_SIZE || 20),
    flushIntervalMs: Number(env.VITE_LOG_FLUSH_INTERVAL_MS || 10000),
    ...createAppMeta(),
  };
}

let loggerConfig: LoggerConfig | undefined;
let logReporter: LogReporter | undefined;
let logFileSink: LogFileSink | undefined;
let loggerSinksInitPromise: Promise<void> | null = null;
let globalHandlersRegistered = false;

/**
 * 异步加载日志落盘/上报 sink（避免 main 入口静态拉取 log-file-sink / event-reporter 链）
 * @returns {Promise<void>}
 */
function ensureLoggerSinksAsync(): Promise<void> {
  if (logReporter && logFileSink) {
    return Promise.resolve();
  }
  if (loggerSinksInitPromise) {
    return loggerSinksInitPromise;
  }
  loggerSinksInitPromise = (async () => {
    if (!loggerConfig) {
      loggerConfig = createDefaultLoggerConfig();
    }
    const [{ LogReporter }, { LogFileSink }] = await Promise.all([
      import('@/utils/logReporter'),
      import('@/utils/logFileSink'),
    ]);
    if (!logReporter) {
      logReporter = new LogReporter(loggerConfig);
      logReporter.start();
    } else {
      logReporter.updateConfig(loggerConfig);
    }
    if (!logFileSink) {
      logFileSink = new LogFileSink(loggerConfig);
      logFileSink.start();
    } else {
      logFileSink.updateConfig(loggerConfig);
    }
  })();
  return loggerSinksInitPromise;
}

/**
 * 确保日志运行时（控制台可立即写；落盘/上报 sink 异步就绪）
 */
function ensureLoggerRuntime(): { config: LoggerConfig } {
  if (!loggerConfig) {
    loggerConfig = createDefaultLoggerConfig();
  }
  void ensureLoggerSinksAsync();
  return { config: loggerConfig };
}

/**
 * 写入单条日志（采集 → 格式化 → mask 脱敏 → 控制台 / 上报）
 * @param {LogLevel} level 级别
 * @param {string} message 消息
 * @param {LogContext} [context] 上下文
 * @param {unknown} [error] 错误
 * @param {string[]} [tags] 标签
 */
function writeLog(
  level: LogLevel,
  message: string,
  context?: LogContext,
  error?: unknown,
  tags?: string[]
): void {
  const { config } = ensureLoggerRuntime();

  if (!shouldLogLevel(level, config.minLevel)) {
    return;
  }

  const mergedContext = mergeRuntimeContext(context);
  const entry = buildLogEntry(level, message, config, mergedContext, error, tags);

  if (config.enableConsole) {
    writeLogEntryToConsole(entry);
  }

  if (logFileSink && logReporter) {
    logFileSink.enqueue(entry);
    logReporter.enqueue(entry);
  } else {
    void ensureLoggerSinksAsync().then(() => {
      logFileSink?.enqueue(entry);
      logReporter?.enqueue(entry);
    });
  }
}

/** 全局默认日志器 */
export const logger: Logger = {
  debug(message, context) {
    writeLog(LogLevel.Debug, message, context);
  },
  info(message, context) {
    writeLog(LogLevel.Info, message, context);
  },
  warn(message, context, error) {
    writeLog(LogLevel.Warn, message, context, error);
  },
  error(message, context, error) {
    writeLog(LogLevel.Error, message, context, error);
  },
  fatal(message, context, error) {
    writeLog(LogLevel.Fatal, message, context, error, ['fatal']);
    void ensureLoggerSinksAsync().then(() => {
      void logFileSink?.flush(false);
      void logReporter?.flush(false);
    });
  },
  flush() {
    void ensureLoggerSinksAsync().then(() => {
      void logFileSink?.flush(false);
      void logReporter?.flush(false);
    });
  },
};

/**
 * 创建带模块前缀的子日志器
 * @param {string} moduleName 模块名
 * @returns {Logger} 子日志器
 */
export function createLogger(moduleName: string): Logger {
  const withModule = (context?: LogContext): LogContext => ({
    module: moduleName,
    ...context,
  });

  return {
    debug: (message, context) => logger.debug(message, withModule(context)),
    info: (message, context) => logger.info(message, withModule(context)),
    warn: (message, context, error) => logger.warn(message, withModule(context), error),
    error: (message, context, error) => logger.error(message, withModule(context), error),
    fatal: (message, context, error) => logger.fatal(message, withModule(context), error),
    flush: () => logger.flush(),
  };
}

/** ResizeObserver 无害错误文案（Ant Design Vue 等组件常见，非业务故障） */
const BENIGN_WINDOW_ERROR_MESSAGES = [
  'ResizeObserver loop completed with undelivered notifications',
  'ResizeObserver loop limit exceeded',
] as const;

/**
 * 是否为可忽略的 window error（避免误报 ERROR）
 * @param {ErrorEvent} event 浏览器 error 事件
 * @returns {boolean} 是否忽略
 */
function isBenignWindowError(event: ErrorEvent): boolean {
  const message = event.message || '';
  return BENIGN_WINDOW_ERROR_MESSAGES.some((fragment) => message.includes(fragment));
}

/**
 * 注册 Vue / Window 全局错误采集
 * @param {App} app Vue 应用实例
 */
function registerGlobalErrorHandlers(app: App): void {
  if (globalHandlersRegistered) {
    return;
  }
  globalHandlersRegistered = true;

  app.config.errorHandler = (err, _instance, info) => {
    logger.error('Vue 运行时错误', { module: 'vue', action: info }, err);
  };

  window.addEventListener('error', (event) => {
    if (isBenignWindowError(event)) {
      event.preventDefault();
      return;
    }
    logger.error(
      event.message || '脚本错误',
      {
        module: 'window',
        action: 'error',
        filename: event.filename,
        lineno: event.lineno,
        colno: event.colno,
      },
      event.error
    );
  });

  window.addEventListener('unhandledrejection', (event) => {
    logger.error('未处理的 Promise 拒绝', { module: 'window', action: 'unhandledrejection' }, event.reason);
  });
}

/**
 * 初始化全局日志（在 app.use(pinia) 与 app.use(router) 之后调用）
 * @param {App} app Vue 应用实例
 * @param {Partial<LoggerConfig>} [override] 覆盖配置
 * @returns {Logger} 全局日志器
 */
export function initLogger(app: App, override?: Partial<LoggerConfig>, router?: Router): Logger {
  loggerConfig = { ...createDefaultLoggerConfig(), ...override };
  void ensureLoggerSinksAsync();
  registerGlobalErrorHandlers(app);

  if (router) {
    setRuntimeRouter(router);
    router.afterEach((to, from) => {
      logger.debug('路由切换', {
        module: 'router',
        action: 'navigate',
        from: from.fullPath,
        to: to.fullPath,
      });
    });
  }

  logger.info('日志系统已初始化', {
    module: 'logger',
    action: 'init',
    enableFile: loggerConfig.enableFile,
    fileUrl: loggerConfig.fileUrl,
  });
  return logger;
}

export { LogLevel } from '@/utils/logFormatter';

export default logger;

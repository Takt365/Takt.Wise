// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：logReporter.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：前端日志统一上报（内存队列、批量 flush、sendBeacon 兜底）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { LogLevel } from '@/utils/logFormatter';
import type { LogEntry, LoggerConfig } from '@/types/logger.types';
import { formatLogEntriesForReport, shouldLogLevel } from '@/utils/logFormatter';

/**
 * 日志上报器（独立于 Axios，避免与 request 拦截器循环依赖）
 */
export class LogReporter {
  private readonly queue: LogEntry[] = [];

  private flushTimer: ReturnType<typeof setInterval> | null = null;

  private isFlushing = false;

  /**
   * @param {LoggerConfig} config 日志器配置
   */
  constructor(private config: LoggerConfig) {}

  /**
   * 更新配置（初始化时可覆盖默认值）
   * @param {Partial<LoggerConfig>} partial 部分配置
   */
  updateConfig(partial: Partial<LoggerConfig>): void {
    this.config = { ...this.config, ...partial };
    this.restartFlushTimer();
  }

  /**
   * 入队待上报日志
   * @param {LogEntry} entry 日志条目
   */
  enqueue(entry: LogEntry): void {
    if (!this.config.enableReport || !this.config.reportUrl) {
      return;
    }

    if (!shouldLogLevel(entry.level, LogLevel.Warn)) {
      return;
    }

    this.queue.push(entry);

    if (this.queue.length >= this.config.batchSize) {
      void this.flush(false);
    }
  }

  /**
   * 启动定时 flush
   */
  start(): void {
    this.restartFlushTimer();

    if (typeof window !== 'undefined') {
      window.addEventListener('beforeunload', this.handleBeforeUnload);
      document.addEventListener('visibilitychange', this.handleVisibilityChange);
    }
  }

  /**
   * 停止定时 flush 并移除监听
   */
  stop(): void {
    if (this.flushTimer) {
      clearInterval(this.flushTimer);
      this.flushTimer = null;
    }

    if (typeof window !== 'undefined') {
      window.removeEventListener('beforeunload', this.handleBeforeUnload);
      document.removeEventListener('visibilitychange', this.handleVisibilityChange);
    }
  }

  /**
   * 立即 flush 队列
   * @param {boolean} [useBeacon=true] 页面卸载时是否使用 sendBeacon
   * @returns {Promise<void>}
   */
  async flush(useBeacon = false): Promise<void> {
    if (!this.config.enableReport || !this.config.reportUrl || this.queue.length === 0 || this.isFlushing) {
      return;
    }

    const entries = this.queue.splice(0, this.queue.length);
    const body = formatLogEntriesForReport(entries);
    const reportUrl = this.config.reportUrl;

    if (useBeacon && typeof navigator !== 'undefined' && typeof navigator.sendBeacon === 'function') {
      const blob = new Blob([body], { type: 'application/json' });
      navigator.sendBeacon(reportUrl, blob);
      return;
    }

    this.isFlushing = true;
    try {
      await fetch(reportUrl, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body,
        keepalive: true,
      });
    } catch {
      this.queue.unshift(...entries);
    } finally {
      this.isFlushing = false;
    }
  }

  private restartFlushTimer(): void {
    if (this.flushTimer) {
      clearInterval(this.flushTimer);
      this.flushTimer = null;
    }

    if (!this.config.enableReport || !this.config.reportUrl) {
      return;
    }

    this.flushTimer = setInterval(() => {
      void this.flush(false);
    }, this.config.flushIntervalMs);
  }

  private handleBeforeUnload = (): void => {
    void this.flush(true);
  };

  private handleVisibilityChange = (): void => {
    if (document.visibilityState === 'hidden') {
      void this.flush(true);
    }
  };
}

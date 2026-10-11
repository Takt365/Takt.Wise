// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：logFileSink.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：前端日志本地落盘采集（开发环境经 Vite 写入 frontend/logs）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { LogEntry, LoggerConfig } from '@/types/logger.types';
import { formatLogEntriesForReport } from '@/utils/logFormatter';

/** 默认本地落盘端点（Vite dev 内部路由） */
export const TAKT_CLIENT_LOG_DEFAULT_FILE_URL = '/__takt/client-logs';

/**
 * 日志本地落盘采集器（批量 POST 至 Vite 开发服务器或自定义落盘端点）
 */
export class LogFileSink {
  private readonly queue: LogEntry[] = [];

  private flushTimer: ReturnType<typeof setInterval> | null = null;

  private isFlushing = false;

  /**
   * @param {LoggerConfig} config 日志器配置
   */
  constructor(private config: LoggerConfig) {}

  /**
   * 更新配置
   * @param {Partial<LoggerConfig>} partial 部分配置
   */
  updateConfig(partial: Partial<LoggerConfig>): void {
    this.config = { ...this.config, ...partial };
    this.restartFlushTimer();
  }

  /**
   * 入队待落盘日志
   * @param {LogEntry} entry 日志条目
   */
  enqueue(entry: LogEntry): void {
    if (!this.config.enableFile || !this.config.fileUrl) {
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
   * 立即 flush 队列到落盘端点
   * @param {boolean} [useBeacon=true] 页面卸载时是否使用 sendBeacon
   * @returns {Promise<void>}
   */
  async flush(useBeacon = false): Promise<void> {
    if (!this.config.enableFile || !this.config.fileUrl || this.queue.length === 0 || this.isFlushing) {
      return;
    }
    const entries = this.queue.splice(0, this.queue.length);
    const body = formatLogEntriesForReport(entries);
    const fileUrl = this.config.fileUrl;
    if (useBeacon && typeof navigator !== 'undefined' && typeof navigator.sendBeacon === 'function') {
      const blob = new Blob([body], { type: 'application/json' });
      navigator.sendBeacon(fileUrl, blob);
      return;
    }
    this.isFlushing = true;
    try {
      await fetch(fileUrl, {
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
    if (!this.config.enableFile || !this.config.fileUrl) {
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

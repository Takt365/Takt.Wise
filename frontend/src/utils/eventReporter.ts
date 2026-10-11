// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：eventReporter.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：前端事件统一上报（内存队列、批量 flush、sendBeacon 兜底）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { EventBusConfig, EventEntry } from '@/types/event.types';
import { formatEventEntriesForReport } from '@/utils/eventFormatter';

/** 需上报的关键事件前缀 */
const REPORTABLE_EVENT_PREFIXES = ['auth:', 'user:', 'tenant:', 'company:'];

/**
 * 事件上报器（独立于 Axios，避免循环依赖）
 */
export class EventReporter {
  private readonly queue: EventEntry[] = [];

  private flushTimer: ReturnType<typeof setInterval> | null = null;

  private isFlushing = false;

  /**
   * @param {EventBusConfig} config 事件总线配置
   */
  constructor(private config: EventBusConfig) {}

  /**
   * 更新配置
   * @param {Partial<EventBusConfig>} partial 部分配置
   */
  updateConfig(partial: Partial<EventBusConfig>): void {
    this.config = { ...this.config, ...partial };
    this.restartFlushTimer();
  }

  /**
   * 入队待上报事件
   * @param {EventEntry} entry 事件条目
   */
  enqueue(entry: EventEntry): void {
    if (!this.config.enableReport || !this.config.reportUrl) {
      return;
    }

    if (!shouldReportEvent(entry.eventName)) {
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
   * 停止定时 flush
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
    const body = formatEventEntriesForReport(entries);
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

/**
 * 是否应上报该事件
 * @param {string} eventName 事件名
 * @returns {boolean} 是否上报
 */
function shouldReportEvent(eventName: string): boolean {
  return REPORTABLE_EVENT_PREFIXES.some((prefix) => eventName.startsWith(prefix));
}

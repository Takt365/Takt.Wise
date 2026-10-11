// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：eventBus.ts
// 创建时间：2026-05-22
// 创建人：Takt365(Cursor AI)
// 功能描述：全局事件总线（运行时网关：统一采集、统一格式化、统一上报；非纯工具模块）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import mitt from 'mitt';
import type { EventBusConfig, EventPhase, Events, NotificationType } from '@/types/event.types';
import { buildEventEntry, writeEventEntryToConsole } from '@/utils/eventFormatter';
import { EventReporter } from '@/utils/eventReporter';
import { createAppMeta, mergeRuntimeContext, parseEnvBoolean, readViteEnv } from '@/utils/runtimeContext';

export type { Events, NotificationType } from '@/types/event.types';

/**
 * 读取默认事件总线配置
 * @returns {EventBusConfig} 默认配置
 */
function createDefaultEventBusConfig(): EventBusConfig {
  const env = readViteEnv();
  const isDev = Boolean(env.DEV);
  return {
    enableConsole: parseEnvBoolean(env.VITE_EVENT_ENABLE_CONSOLE, isDev),
    enableReport: parseEnvBoolean(env.VITE_EVENT_ENABLE_REPORT, !isDev),
    reportUrl: env.VITE_EVENT_REPORT_URL || undefined,
    batchSize: Number(env.VITE_EVENT_BATCH_SIZE || env.VITE_LOG_BATCH_SIZE || 20),
    flushIntervalMs: Number(env.VITE_EVENT_FLUSH_INTERVAL_MS || env.VITE_LOG_FLUSH_INTERVAL_MS || 10000),
    ...createAppMeta(),
  };
}

let eventBusConfig: EventBusConfig | undefined;
let eventReporter: EventReporter | undefined;

/**
 * 确保事件总线运行时（延迟初始化，避免模块顶层读取未注入的 import.meta.env）
 */
function ensureEventBusRuntime(): { config: EventBusConfig; reporter: EventReporter } {
  if (!eventBusConfig || !eventReporter) {
    eventBusConfig = createDefaultEventBusConfig();
    eventReporter = new EventReporter(eventBusConfig);
  }

  return { config: eventBusConfig, reporter: eventReporter };
}
const emitter = mitt<Record<string, unknown>>();
const handlerMap = new WeakMap<object, (payload: unknown) => void>();

/**
 * 采集并分发事件记录
 * @param {EventPhase} phase 阶段
 * @param {string} eventName 事件名
 * @param {unknown} payload 载荷
 */
function collectEvent(phase: EventPhase, eventName: string, payload?: unknown): void {
  const { config, reporter } = ensureEventBusRuntime();
  const context = mergeRuntimeContext({ module: 'event-bus', action: phase });
  const entry = buildEventEntry(eventName, phase, config, payload, context);

  if (config.enableConsole) {
    writeEventEntryToConsole(entry);
  }

  reporter.enqueue(entry);
}

/**
 * 事件总线（类型安全的 publish / subscribe）
 */
export const EventBus = {
  /**
   * 订阅事件
   * @param event 事件名称
   * @param handler 处理函数
   */
  on<K extends keyof Events>(event: K, handler: (payload: Events[K]) => void) {
    const wrappedHandler = (payload: unknown) => {
      collectEvent('handle', event as string, payload);
      handler(payload as Events[K]);
    };
    handlerMap.set(handler, wrappedHandler);
    emitter.on(event as string, wrappedHandler);
  },

  /**
   * 取消订阅
   * @param event 事件名称
   * @param handler 不传则移除该事件全部监听
   */
  off<K extends keyof Events>(event: K, handler?: (payload: Events[K]) => void) {
    if (handler) {
      const wrappedHandler = handlerMap.get(handler);
      if (wrappedHandler) {
        emitter.off(event as string, wrappedHandler);
        handlerMap.delete(handler);
      }
      return;
    }
    emitter.off(event as string);
  },

  /**
   * 发布事件
   * @param event 事件名称
   * @param payload 载荷（无载荷事件传 undefined）
   */
  emit<K extends keyof Events>(
    event: K,
    ...args: Events[K] extends undefined ? [payload?: undefined] : [payload: Events[K]]
  ) {
    const payload = args[0] as Events[K];
    collectEvent('emit', event as string, payload);
    emitter.emit(event as string, payload as unknown);
  },

  /**
   * 移除全部监听（仅用于测试或热重载场景）
   */
  clear() {
    emitter.all.clear();
  },

  /**
   * 立即 flush 上报队列
   */
  flush() {
    void ensureEventBusRuntime().reporter.flush(false);
  },
};

/**
 * 发布全局 Toast（供 request 等非 UI 层使用）
 * @param type 通知类型
 * @param message 主文案
 * @param description 副文案
 */
export function emitNotification(
  type: NotificationType,
  message: string,
  description?: string
): void {
  EventBus.emit('notification:show', { type, message, description });
}

/**
 * 初始化事件总线（在 app.use(pinia) 与 app.use(router) 之后调用）
 * @param {Partial<EventBusConfig>} [override] 覆盖配置
 */
export function initEventBus(override?: Partial<EventBusConfig>): void {
  eventBusConfig = { ...createDefaultEventBusConfig(), ...override };
  if (!eventReporter) {
    eventReporter = new EventReporter(eventBusConfig);
  } else {
    eventReporter.updateConfig(eventBusConfig);
  }
  eventReporter.start();
}

/**
 * 组合式 API：在组件内订阅（须在 onUnmounted 中 off）
 */
export function useEventBus() {
  return {
    on: EventBus.on,
    off: EventBus.off,
    emit: EventBus.emit,
    emitNotification,
    flush: EventBus.flush,
  };
}

export default EventBus;

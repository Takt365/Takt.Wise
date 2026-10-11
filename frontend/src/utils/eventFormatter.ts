// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：eventFormatter.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：前端事件统一格式化（条目构建、控制台输出、上报 JSON）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { LogContext } from '@/types/logger.types';
import type { EventBusConfig, EventEntry, EventPhase } from '@/types/event.types';
import {
  buildFormatterRuntimeMeta,
  formatEntriesBatchForReport,
  serializeJsonSafe,
} from '@/utils/taktFormatterCommon';

/**
 * 序列化事件载荷（避免循环引用）
 * @param {unknown} payload 原始载荷
 * @returns {unknown} 可序列化载荷
 */
export function serializeEventPayload(payload: unknown): unknown {
  return serializeJsonSafe(payload);
}

/**
 * 构建标准事件条目
 * @param {string} eventName 事件名
 * @param {EventPhase} phase 阶段
 * @param {EventBusConfig} config 配置
 * @param {unknown} [payload] 载荷
 * @param {LogContext} [context] 上下文
 * @returns {EventEntry} 标准事件条目
 */
export function buildEventEntry(
  eventName: string,
  phase: EventPhase,
  config: EventBusConfig,
  payload?: unknown,
  context?: LogContext
): EventEntry {
  return {
    eventName,
    phase,
    ...buildFormatterRuntimeMeta(config),
    payload: serializeEventPayload(payload),
    context,
  };
}

/**
 * 格式化事件条目为控制台可读字符串
 * @param {EventEntry} entry 事件条目
 * @returns {string} 格式化文本
 */
export function formatEventEntryForConsole(entry: EventEntry): string {
  const moduleLabel = entry.context?.module ? `[${entry.context.module}]` : '';
  return `[EVENT:${entry.phase.toUpperCase()}]${moduleLabel} ${entry.eventName}`;
}

/**
 * 将事件条目格式化为上报 JSON
 * @param {EventEntry[]} entries 事件条目列表
 * @returns {string} JSON 字符串
 */
export function formatEventEntriesForReport(entries: EventEntry[]): string {
  return formatEntriesBatchForReport(entries);
}

/**
 * 输出到浏览器控制台
 * @param {EventEntry} entry 事件条目
 */
export function writeEventEntryToConsole(entry: EventEntry): void {
  const text = formatEventEntryForConsole(entry);
  const detail = { payload: entry.payload, context: entry.context };
  console.debug(text, detail);
}

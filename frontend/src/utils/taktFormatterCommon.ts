// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktFormatterCommon.ts
// 创建时间：2026-05-29
// 创建人：Takt365(Cursor AI)
// 功能描述：日志/事件格式化公共序列化与 batch 上报（07-overflow：深度/数组采样/字符串截断）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 日志/事件条目共用的运行时元数据 */
export interface TaktFormatterRuntimeMeta {
  timestamp: string;
  appName: string;
  appVersion: string;
  environment: string;
  url: string;
}

/**
 * 构建日志/事件条目共用的运行时元数据
 * @param config 应用标识配置
 * @returns 运行时元数据
 */
export function buildFormatterRuntimeMeta(config: {
  appName: string;
  appVersion: string;
  environment: string;
}): TaktFormatterRuntimeMeta {
  return {
    timestamp: new Date().toISOString(),
    appName: config.appName,
    appVersion: config.appVersion,
    environment: config.environment,
    url: typeof window !== 'undefined' ? window.location.href : '',
  };
}

/** JSON 安全序列化最大嵌套深度 */
export const TAKT_JSON_SAFE_MAX_DEPTH = 5;

/** JSON 安全序列化数组最大采样条数（与 log-formatter sampleForLog 默认 40 对齐） */
export const TAKT_JSON_SAFE_MAX_ARRAY_ITEMS = 40;

/** 日志/上报 JSON 字符串最大长度（07-overflow-vue） */
export const TAKT_JSON_LOG_MAX_LENGTH = 4096;

/**
 * 递归安全序列化（限制深度与数组长度，避免循环引用与巨型 payload）
 * @param payload 原始载荷
 * @param depth 当前深度
 * @param maxDepth 最大深度
 * @param maxArrayItems 数组最大采样条数
 * @returns 可序列化载荷
 */
function serializeJsonSafeInner(
  payload: unknown,
  depth: number,
  maxDepth: number,
  maxArrayItems: number
): unknown {
  if (payload === undefined || payload === null) {
    return payload;
  }
  if (typeof payload !== 'object') {
    return payload;
  }
  if (depth > maxDepth) {
    return '[MaxDepth]';
  }
  if (payload instanceof Date) {
    return payload.toISOString();
  }
  if (Array.isArray(payload)) {
    const total = payload.length;
    const sample = payload
      .slice(0, maxArrayItems)
      .map((item) => serializeJsonSafeInner(item, depth + 1, maxDepth, maxArrayItems));
    if (total > maxArrayItems) {
      return { __sample: sample, __total: total };
    }
    return sample;
  }
  const result: Record<string, unknown> = {};
  for (const [key, value] of Object.entries(payload as Record<string, unknown>)) {
    result[key] = serializeJsonSafeInner(value, depth + 1, maxDepth, maxArrayItems);
  }
  return result;
}

/**
 * JSON 安全序列化（避免循环引用；数组采样、深度上限见 07-overflow-vue）
 * @param payload 原始载荷
 * @returns 可序列化载荷
 */
export function serializeJsonSafe(payload: unknown): unknown {
  if (payload === undefined) {
    return undefined;
  }
  try {
    return serializeJsonSafeInner(
      payload,
      0,
      TAKT_JSON_SAFE_MAX_DEPTH,
      TAKT_JSON_SAFE_MAX_ARRAY_ITEMS
    );
  } catch {
    return String(payload);
  }
}

/**
 * 将未知载荷格式化为日志消息字符串（截断过长 JSON）
 * @param payload 原始载荷
 * @param maxLength 最大字符数，默认 TAKT_JSON_LOG_MAX_LENGTH
 * @returns JSON 或字符串表示
 */
export function safeStringifyForLog(payload: unknown, maxLength = TAKT_JSON_LOG_MAX_LENGTH): string {
  if (payload === undefined) {
    return '';
  }
  if (payload === null) {
    return 'null';
  }
  if (typeof payload === 'string') {
    return payload.length > maxLength ? `${payload.slice(0, maxLength)}…` : payload;
  }
  try {
    const text = JSON.stringify(serializeJsonSafe(payload));
    if (text.length <= maxLength) {
      return text;
    }
    return `${text.slice(0, maxLength)}…(${text.length} chars)`;
  } catch {
    const fallback = String(payload);
    return fallback.length > maxLength ? `${fallback.slice(0, maxLength)}…` : fallback;
  }
}

/**
 * 将条目列表格式化为上报 JSON（含 batchId、reportedAt）
 * @param entries 条目列表
 * @returns JSON 字符串
 */
export function formatEntriesBatchForReport<T>(entries: T[]): string {
  if (entries == null) {
    throw new Error('formatEntriesBatchForReport: entries 不能为空');
  }
  const safeEntries = serializeJsonSafe(entries) as T[];
  return JSON.stringify({
    batchId: crypto.randomUUID(),
    reportedAt: new Date().toISOString(),
    entries: safeEntries,
  });
}

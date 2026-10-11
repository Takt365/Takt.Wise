// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/utils
// 文件名称：taktFileTags.ts
// 创建时间：2025-06-12
// 创建人：Takt
// 功能描述：文件标签解析、规范化与序列化（配色委托 takt-tag-color / color-base）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import {
  TAKT_TAG_COLOR_CLASS_COUNT,
  TAKT_TAG_COLOR_DISPLAY_MAX,
  TAKT_TAG_COLOR_TOKEN_KEYS,
  normalizeTagList,
  parseCommaSeparatedTags,
  resolveTaktTagColorClass,
} from '@/utils/taktTagColor';

/** 文件标签最多数量 */
export const FILE_TAG_MAX_COUNT = 5;

/** 列表展示截断长度（与 takt-tag-color 对齐） */
export const FILE_TAG_DISPLAY_MAX = TAKT_TAG_COLOR_DISPLAY_MAX;

/** 色板 token（与 color-base.css 对齐） */
export const FILE_TAG_COLOR_TOKEN_KEYS = TAKT_TAG_COLOR_TOKEN_KEYS;

/** 预设色数量 */
export const FILE_TAG_COLOR_CLASS_COUNT = TAKT_TAG_COLOR_CLASS_COUNT;

/**
 * 解析逗号分隔标签字符串
 * @param raw 原始字符串
 * @returns 标签数组
 */
export function parseFileTags(raw?: string | null): string[] {
  if (raw == null || typeof raw !== 'string' || !raw.trim()) {
    return [];
  }
  return normalizeFileTagList(parseCommaSeparatedTags(raw));
}

/**
 * 规范化标签列表：去空白、去重、截断至上限
 * @param tags 原始标签数组
 * @returns 规范化后的标签
 */
export function normalizeFileTagList(tags: readonly string[]): string[] {
  return normalizeTagList(tags, FILE_TAG_MAX_COUNT);
}

/**
 * 标签数组序列化为逗号分隔字符串
 * @param tags 标签数组
 * @returns 逗号分隔字符串；无标签时返回空串
 */
export function joinFileTags(tags: readonly string[]): string {
  return normalizeFileTagList(tags).join(',');
}

/**
 * 按标签在列表中的序号返回 tag-base 样式类名
 * @param _tag 标签文本（保留参数便于调用方语义）
 * @param index 列表序号（0 起）
 * @returns CSS 类名
 */
export function resolveFileTagColor(_tag: string, index: number): string {
  return resolveTaktTagColorClass(index);
}

// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/utils
// 文件名称：taktTagColor.ts
// 创建时间：2026-06-13
// 创建人：Takt365(Cursor AI)
// 功能描述：预设色 Tag 工具（序号映射 color-base.css token / tag-base.css 类名）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 列表展示截断长度（超出用 tooltip 展示全文） */
export const TAKT_TAG_COLOR_DISPLAY_MAX = 20;

/**
 * 与 color-base.css、tag-base.css 序号对齐的色板 token
 * 样式见 frontend/src/styles/tag-base.css
 */
export const TAKT_TAG_COLOR_TOKEN_KEYS = [
  '--takt-cn-hong',
  '--takt-cn-chengse',
  '--takt-cn-qiuhuang',
  '--takt-cn-youlv',
  '--takt-cn-feicui',
  '--takt-cn-dianqing',
  '--takt-cn-baolan',
  '--takt-cn-zise',
  '--takt-jp-momo',
  '--takt-jp-asagi',
  '--takt-klein-blue',
  '--takt-tiffany-blue',
] as const;

/** 预设色数量（循环上限） */
export const TAKT_TAG_COLOR_CLASS_COUNT = TAKT_TAG_COLOR_TOKEN_KEYS.length;

/** 基础样式类名 */
export const TAKT_TAG_COLOR_BASE_CLASS = 'takt-tag-color';

/**
 * 规范化配色序号
 * @param index 列表序号（0 起）
 * @returns 0 至 TAKT_TAG_COLOR_CLASS_COUNT-1
 */
export function resolveTaktTagColorIndex(index: number): number {
  if (!Number.isFinite(index)) {
    return 0;
  }
  return Math.abs(Math.trunc(index)) % TAKT_TAG_COLOR_CLASS_COUNT;
}

/**
 * 按序号返回 tag-base 样式类名（如 takt-tag-color-0）
 * @param index 列表序号（0 起）
 * @returns CSS 类名
 */
export function resolveTaktTagColorClass(index: number): string {
  return `${TAKT_TAG_COLOR_BASE_CLASS}-${resolveTaktTagColorIndex(index)}`;
}

/**
 * 按序号返回 color-base.css 变量名
 * @param index 列表序号（0 起）
 * @returns CSS 自定义属性名
 */
export function resolveTaktTagColorToken(index: number): string {
  return TAKT_TAG_COLOR_TOKEN_KEYS[resolveTaktTagColorIndex(index)];
}

/**
 * 解析逗号分隔标签字符串
 * @param raw 原始字符串
 * @param separator 分隔符
 * @returns 标签数组（未去重）
 */
export function parseCommaSeparatedTags(
  raw?: string | null,
  separator = ','
): string[] {
  if (raw == null || typeof raw !== 'string' || !raw.trim()) {
    return [];
  }
  return raw.split(separator);
}

/**
 * 规范化标签列表：去空白、去重、可选上限
 * @param tags 原始标签数组
 * @param maxCount 最大数量；未传则不截断
 * @returns 规范化后的标签
 */
export function normalizeTagList(tags: readonly string[], maxCount?: number): string[] {
  const seen = new Set<string>();
  const result: string[] = [];
  for (const raw of tags) {
    const tag = raw.trim();
    if (!tag || seen.has(tag)) {
      continue;
    }
    seen.add(tag);
    result.push(tag);
    if (maxCount != null && result.length >= maxCount) {
      break;
    }
  }
  return result;
}

/**
 * 标签展示截断
 * @param tag 标签文本
 * @param maxLength 最大长度
 * @returns 截断后文本
 */
export function truncateTagLabel(tag: string, maxLength = TAKT_TAG_COLOR_DISPLAY_MAX): string {
  if (tag.length <= maxLength) {
    return tag;
  }
  return `${tag.slice(0, maxLength)}...`;
}

/**
 * 标签是否需截断展示
 * @param tag 标签文本
 * @param maxLength 最大长度
 * @returns 是否截断
 */
export function shouldTruncateTagLabel(
  tag: string,
  maxLength = TAKT_TAG_COLOR_DISPLAY_MAX
): boolean {
  return tag.length > maxLength;
}

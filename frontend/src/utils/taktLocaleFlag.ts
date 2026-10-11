// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktLocaleFlag.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：语言图标 CSS 类名解析（flag-icons / 数据库 icon 字段）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** cultureCode 与 flag-icons 默认映射 */
const DEFAULT_FLAG_CLASS_MAP: Record<string, string> = {
  'zh-CN': 'fi fi-cn',
  'zh-HK': 'fi fi-hk',
  'en-US': 'fi fi-us',
  'ja-JP': 'fi fi-jp',
};

/**
 * 解析语言项对应的 flag-icons CSS 类名
 * @param cultureCode 区域文化编码
 * @param icon 数据库 icon 字段（可选）
 * @returns flag-icons 类名；无匹配时返回空字符串
 */
export function resolveCultureFlagClass(cultureCode: string, icon?: string): string {
  if (icon) {
    if (icon.startsWith('fi ')) {
      return icon;
    }

    if (icon.startsWith('fi-')) {
      return `fi ${icon}`;
    }

    if (icon.startsWith('flag-')) {
      return `fi fi-${icon.slice(5)}`;
    }
  }

  return DEFAULT_FLAG_CLASS_MAP[cultureCode] ?? '';
}

/**
 * 判断 icon 字段是否为 emoji 等非 CSS 类图标
 * @param icon 数据库 icon 字段
 * @returns 是否为文本图标
 */
export function isCultureEmojiIcon(icon?: string): boolean {
  if (!icon) {
    return false;
  }

  return !icon.startsWith('fi') && !icon.startsWith('flag-');
}

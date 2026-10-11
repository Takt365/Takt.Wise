// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src
// 文件名称：setting.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：全局配置（默认值、持久化）；主题色映射权威源见 @/utils/theme
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { AppSetting, ThemeColor, ThemeColorConfig } from '@/types/setting.types';
import {
  appSettingThemeColorToPreset,
  getThemeColorValue as getPresetThemeColorValue,
  themeColorPresetI18nKeyMap,
} from '@/utils/theme';
import { createLogger } from '@/utils/logger';

export type { ThemeColor, ThemeColorConfig, AppSetting } from '@/types/setting.types';

const settingLogger = createLogger('setting');

/** 默认设置 */
export const defaultSetting: AppSetting = {
  layout: 'side',
  theme: 'dark',
  themeColor: { type: 'blue' },
  /** 仅主题外观：true 表示用户手动选过明暗/主色，假日主题不再覆盖主色 */
  appearanceUserOverride: false,
  borderRadius: 5,
  fontSize: 15,
  colorWeak: false,
  grayscale: false,
  fixedHeader: true,
  fixedSider: true,
  showLogo: true,
  siderWidth: 200,
  siderCollapsedWidth: 64,
  showBreadcrumb: true,
  breadcrumbIcon: false,
  showTabs: true,
  tabStyle: 'google',
  persistTabs: false,
  maxTabs: 10,
  showFooter: true,
  copyright: '© 2026 Takt Technologies Co., Ltd. All rights reserved.',
  contentWidth: 'fluid',
  multiTab: true,
  watermark: false,
  watermarkContent: 'TaktWise',
  demo: false,
  menuAccordion: true,
  menuStyle: 'plain',
  logo: '@/assets/images/takt.svg',
  logoText: 'TaktWise',
  logoCollapsedText: 'T',
  showForgotPassword: false,
  showRegister: false,
};

/** localStorage 键名 */
export const STORAGE_KEY = 'app-setting';

/** AppSetting 短键 → preset（与 utils/theme 同源） */
export const themeColorToPreset = appSettingThemeColorToPreset;

/**
 * 主题色短键 → hex（由 utils/theme 预设派生，禁止重复维护色值）
 */
export const themeColorMap: Record<Exclude<ThemeColor, 'custom'>, string> = Object.fromEntries(
  Object.entries(appSettingThemeColorToPreset).map(([shortKey, preset]) => [
    shortKey,
    getPresetThemeColorValue(preset),
  ])
) as Record<Exclude<ThemeColor, 'custom'>, string>;

/**
 * 主题色短键 → common.page.color.* 后缀（由 preset i18n 映射派生）
 */
export const themeColorI18nKeyMap: Record<Exclude<ThemeColor, 'custom'>, string> = Object.fromEntries(
  Object.entries(appSettingThemeColorToPreset).map(([shortKey, preset]) => [
    shortKey,
    themeColorPresetI18nKeyMap[preset],
  ])
) as Record<Exclude<ThemeColor, 'custom'>, string>;

/**
 * 解析 AppSetting 主题色为 hex
 * @param {ThemeColorConfig} config 主题色配置
 * @returns {string} 十六进制色值
 */
export function getThemeColorValue(config: ThemeColorConfig): string {
  if (config.type === 'custom' && config.customColor) {
    return config.customColor;
  }
  const preset = appSettingThemeColorToPreset[config.type as keyof typeof appSettingThemeColorToPreset];
  if (preset) {
    return getPresetThemeColorValue(preset);
  }
  return getPresetThemeColorValue('klein-blue');
}

/**
 * 校验字号范围（15–22）
 * @param {number} size 字号
 * @returns {number} 合法字号
 */
export function validateFontSize(size: number): number {
  if (size < 15) {
    return 15;
  }
  if (size > 22) {
    return 22;
  }
  return size;
}

/**
 * 是否为历史废弃版权文案（须回写为法定公司名）
 * @param {string | undefined} copyright 当前版权串
 * @returns {boolean} 是否废弃
 */
function isObsoleteCopyright(copyright: string | undefined): boolean {
  if (!copyright?.trim()) {
    return true;
  }
  const text = copyright.trim();
  if (/Takt\s+Digital\s+Factory/i.test(text)) {
    return true;
  }
  if (/Takt\s+Plat\.?\s+All\s+rights\s+reserved/i.test(text)) {
    return true;
  }
  return false;
}

/**
 * 合并并规范化应用设置
 * @param {Partial<AppSetting>} raw 原始片段
 * @returns {AppSetting} 有效设置
 */
export function normalizeSetting(raw: Partial<AppSetting>): AppSetting {
  const base: AppSetting = { ...defaultSetting, ...raw };

  if (raw.themeColor && typeof raw.themeColor === 'object') {
    base.themeColor = { ...defaultSetting.themeColor, ...raw.themeColor };
  }

  if (isObsoleteCopyright(base.copyright)) {
    base.copyright = defaultSetting.copyright;
  }

  if (typeof base.fontSize === 'number') {
    base.fontSize = validateFontSize(base.fontSize);
  } else {
    const sizeMap: Record<string, number> = { small: 15, medium: 16, large: 18 };
    base.fontSize = sizeMap[String(base.fontSize)] ?? 15;
  }

  if (!base.themeColor || typeof base.themeColor !== 'object') {
    base.themeColor = { ...defaultSetting.themeColor };
  } else if (
    base.themeColor.type !== 'custom'
    && !(base.themeColor.type in appSettingThemeColorToPreset)
  ) {
    base.themeColor = { ...defaultSetting.themeColor };
  }

  if (base.theme !== 'light' && base.theme !== 'dark') {
    base.theme = defaultSetting.theme;
  }

  if (base.tabStyle !== 'card' && base.tabStyle !== 'google') {
    base.tabStyle = defaultSetting.tabStyle;
  }

  if (base.menuStyle !== 'plain' && base.menuStyle !== 'rounded') {
    base.menuStyle = defaultSetting.menuStyle;
  }

  if (typeof base.menuAccordion !== 'boolean') {
    base.menuAccordion = defaultSetting.menuAccordion;
  }
  if (typeof base.showBreadcrumb !== 'boolean') {
    base.showBreadcrumb = defaultSetting.showBreadcrumb;
  }
  if (typeof base.breadcrumbIcon !== 'boolean') {
    base.breadcrumbIcon = defaultSetting.breadcrumbIcon;
  }

  const allowedRadii = [0, 5, 10, 15, 20];
  if (typeof base.borderRadius !== 'number' || !allowedRadii.includes(base.borderRadius)) {
    base.borderRadius = defaultSetting.borderRadius;
  }

  if (typeof base.showTabs !== 'boolean') {
    base.showTabs = defaultSetting.showTabs;
  }
  if (typeof base.persistTabs !== 'boolean') {
    base.persistTabs = defaultSetting.persistTabs;
  }
  if (typeof base.multiTab !== 'boolean') {
    base.multiTab = defaultSetting.multiTab;
  }
  if (typeof base.demo !== 'boolean') {
    base.demo = defaultSetting.demo;
  }
  if (typeof base.watermark !== 'boolean') {
    base.watermark = defaultSetting.watermark;
  }
  // Demo 开启时强制启用水印（演示标识联动）
  if (base.demo) {
    base.watermark = true;
  }
  if (typeof base.showFooter !== 'boolean') {
    base.showFooter = defaultSetting.showFooter;
  }
  if (typeof base.maxTabs !== 'number' || !Number.isFinite(base.maxTabs)) {
    base.maxTabs = defaultSetting.maxTabs;
  } else {
    base.maxTabs = Math.min(50, Math.max(5, Math.trunc(base.maxTabs)));
  }

  base.appearanceUserOverride = raw.appearanceUserOverride === true;

  return base;
}

/**
 * 从 localStorage 读取并合并为有效 AppSetting
 * @returns {AppSetting} 当前应用设置
 */
export function readSettingFromStorage(): AppSetting {
  if (typeof window === 'undefined' || !localStorage) {
    return defaultSetting;
  }
  try {
    const stored = localStorage.getItem(STORAGE_KEY);
    if (!stored) {
      return defaultSetting;
    }
    const parsed = JSON.parse(stored) as Partial<AppSetting>;
    const normalized = normalizeSetting(parsed);
    // 废弃版权等迁移：写回 localStorage，避免下次仍读到旧串
    if (isObsoleteCopyright(parsed.copyright)) {
      localStorage.setItem(STORAGE_KEY, JSON.stringify(normalized));
    }
    return normalized;
  } catch (error) {
    settingLogger.warn('读取/解析偏好设置失败，使用默认值', { action: 'read' }, error);
    return defaultSetting;
  }
}

/** 非响应式读取当前设置 */
export const getSetting = readSettingFromStorage;

/**
 * 持久化应用设置
 * @param {AppSetting} setting 完整设置
 */
export function saveSettingToStorage(setting: AppSetting): void {
  if (typeof window === 'undefined' || !localStorage) {
    return;
  }
  const normalized = normalizeSetting(setting);
  localStorage.setItem(STORAGE_KEY, JSON.stringify(normalized));
}

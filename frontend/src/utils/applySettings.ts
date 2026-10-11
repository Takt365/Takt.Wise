// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：applySettings.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：将 AppSetting 应用到 DOM；主题/主色同步至 Pinia Store（权威渲染见 App.vue ConfigProvider）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { getActivePinia } from 'pinia';
import type { AppSetting } from '@/types/setting.types';
import { getSetting, themeColorToPreset } from '@/setting';
import { readStoredThemeMode } from '@/utils/theme';
import { useThemeColorStore } from '@/stores/common/useThemeColorStore';
import { useThemeStore } from '@/stores/common/useThemeStore';

/** 设置变更自定义事件名（供非 Pinia 模块监听） */
export const APP_SETTING_CHANGED_EVENT = 'takt:app-setting-changed';

/**
 * 将布局/字号/滤镜等偏好写入 DOM（不含主题 algorithm 与主色 token）
 * @param setting 当前偏好
 */
function applyLayoutSettings(setting: AppSetting): void {
  const root = document.documentElement;
  const body = document.body;

  root.style.fontSize = `${setting.fontSize}px`;
  root.style.setProperty('--takt-font-size', `${String(setting.fontSize)}px`);
  // Ant Design 圆角由 App.vue ConfigProvider token.borderRadius 权威写入；此处仅保留业务 CSS 变量
  root.style.setProperty('--takt-border-radius', `${String(setting.borderRadius)}px`);

  root.dataset.layout = setting.layout;
  root.dataset.contentWidth = setting.contentWidth;
  root.dataset.tabStyle = setting.tabStyle;
  root.dataset.menuStyle = setting.menuStyle;

  const filters: string[] = [];
  if (setting.colorWeak) {
    filters.push('invert(80%)');
  }
  if (setting.grayscale) {
    filters.push('grayscale(100%)');
  }
  body.style.filter = filters.length > 0 ? filters.join(' ') : '';
}

/**
 * 将 AppSetting 中的主题/主色同步到 Pinia Store（与工具栏 toggle 共用一套状态）
 * 生效主色由 theme-color Store 按「系统默认 → 假日适配 → 用户自定义」叠加解析
 * @param setting 当前偏好
 */
function syncThemeStoresFromSetting(setting: AppSetting): void {
  if (!getActivePinia()) {
    return;
  }
  const themeStore = useThemeStore();
  const themeColorStore = useThemeColorStore();
  const syncOptions = { userInitiated: false, silent: true } as const;

  if (setting.appearanceUserOverride) {
    const preferredMode = readStoredThemeMode();
    if (themeStore.mode !== preferredMode) {
      themeStore.setThemeMode(preferredMode, syncOptions);
    }
  } else if (setting.theme === 'light' || setting.theme === 'dark') {
    if (themeStore.mode !== setting.theme) {
      themeStore.setThemeMode(setting.theme, syncOptions);
    }
  }

  if (setting.themeColor.type !== 'custom') {
    const preset = themeColorToPreset[setting.themeColor.type];
    if (preset && themeColorStore.preset !== preset) {
      themeColorStore.setColorPreset(preset, syncOptions);
    }
  }
}

/**
 * 将当前 AppSetting 应用到 document 并同步主题 Store
 */
export function applySettings(): void {
  const setting = getSetting();
  applyLayoutSettings(setting);
  syncThemeStoresFromSetting(setting);
}

/**
 * 广播偏好设置已变更（布局组件可监听后刷新本地状态）
 */
export function notifySettingsChanged(): void {
  if (typeof window !== 'undefined') {
    window.dispatchEvent(new CustomEvent(APP_SETTING_CHANGED_EVENT, { detail: getSetting() }));
  }
}

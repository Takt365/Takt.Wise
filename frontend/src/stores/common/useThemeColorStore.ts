// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/stores/common
// 文件名称：useThemeColorStore.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：主题色预设状态（ConfigProvider token.colorPrimary）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { AppSetting, ThemeColor } from '@/types/setting.types';
import { useSettingStore } from '@/stores/common/useSettingStore';
import { useUserStore } from '@/stores/identity/useUserStore';
import { EventBus } from '@/utils/eventBus';
import { TAKT_THEME_COLOR_STORAGE_KEY } from '@/utils/common';
import {
  getThemeColorValue,
  presetToAppSettingThemeColor,
  readStoredThemeColorPreset,
  resolveEffectiveColorPreset,
  resolveEffectiveColorPrimary,
  systemDefaultThemeColorConfig,
  themeColorPresetKeys,
  type TaktThemeColorPreset,
} from '@/utils/theme';

/** setColorPreset 选项 */
export interface TaktSetColorPresetOptions {
  /** 是否为用户主动切换（false 时仅同步 preset，不锁定假日主题） */
  userInitiated?: boolean;
  /** 为 true 时不广播 theme-color:change（applySettings 同步用） */
  silent?: boolean;
}

/**
 * 主题色预设状态管理
 */
export const useThemeColorStore = defineStore('theme-color', () => {
  /** 当前主题色预设键（初始化自 localStorage） */
  const preset = ref<TaktThemeColorPreset>(readStoredThemeColorPreset());

  /**
   * 实际生效主色（叠加：系统默认 → 假日适配 → 用户自定义）
   */
  const colorPrimary = computed(() => {
    const settingStore = useSettingStore();
    const userStore = useUserStore();
    return resolveEffectiveColorPrimary(
      settingStore.setting,
      userStore.holidayFromToken,
      systemDefaultThemeColorConfig,
    );
  });

  /**
   * 色板选中态 preset（与 colorPrimary 同源优先级）
   */
  const effectivePreset = computed(() => {
    const settingStore = useSettingStore();
    const userStore = useUserStore();
    return resolveEffectiveColorPreset(
      settingStore.setting,
      userStore.holidayFromToken,
      preset.value,
    );
  });

  /**
   * 设置主题色预设
   * @param {TaktThemeColorPreset} next 预设键名
   * @param {TaktSetColorPresetOptions} options 是否用户主动切换
   * @returns {void}
   */
  function setColorPreset(next: TaktThemeColorPreset, options?: TaktSetColorPresetOptions): void {
    const userInitiated = options?.userInitiated !== false;
    const silent = options?.silent === true;
    const settingStore = useSettingStore();

    if (preset.value === next) {
      if (userInitiated && !settingStore.setting.appearanceUserOverride) {
        settingStore.patchSetting({ appearanceUserOverride: true });
      }
      return;
    }

    preset.value = next;
    localStorage.setItem(TAKT_THEME_COLOR_STORAGE_KEY, next);

    const shortKey = presetToAppSettingThemeColor[next];
    if (shortKey && userInitiated) {
      settingStore.patchSetting({
        themeColor: { type: shortKey as ThemeColor },
        appearanceUserOverride: true,
      });
    } else if (shortKey && !userInitiated && settingStore.setting.themeColor.type !== shortKey) {
      settingStore.patchSetting({
        themeColor: { type: shortKey as ThemeColor },
      });
    } else if (userInitiated) {
      settingStore.patchSetting({ appearanceUserOverride: true });
    }

    if (!silent) {
      EventBus.emit('theme-color:change', { preset: next, color: getThemeColorValue(next) });
    }
  }

  /**
   * 循环切换到下一个预设
   * @returns {void}
   */
  function toggleColorPreset(): void {
    /** 当前预设在列表中的下标 */
    const currentIndex = themeColorPresetKeys.indexOf(preset.value);
    /** 下一个预设（环状） */
    const next = themeColorPresetKeys[(currentIndex + 1) % themeColorPresetKeys.length];

    if (next) {
      setColorPreset(next);
    }
  }

  return {
    preset,
    effectivePreset,
    colorPrimary,
    setColorPreset,
    toggleColorPreset,
  };
});

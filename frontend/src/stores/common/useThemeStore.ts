// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/stores/common
// 文件名称：useThemeStore.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：主题状态管理（Ant Design Vue ConfigProvider 算法 + Tailwind CSS 变量联动）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { defineStore } from 'pinia';
import { ref, computed, watch } from 'vue';
import { usePreferredDark } from '@vueuse/core';
import type { AppSetting } from '@/types/setting.types';
import { readSettingFromStorage } from '@/setting';
import { useSettingStore } from '@/stores/common/useSettingStore';
import { EventBus } from '@/utils/eventBus';
import { TAKT_THEME_STORAGE_KEY } from '@/utils/common';
import {
  applyThemeDom,
  readStoredThemeMode,
  type TaktResolvedTheme,
  type TaktThemeMode,
} from '@/utils/theme';

/** setThemeMode 选项 */
export interface TaktSetThemeModeOptions {
  /** 是否为用户主动切换（false 时仅同步状态，不锁定系统默认） */
  userInitiated?: boolean;
  /** 为 true 时不广播 theme:change（applySettings 同步用） */
  silent?: boolean;
}

/**
 * 主题状态管理
 */
export const useThemeStore = defineStore('theme', () => {
  const initialSetting = readSettingFromStorage();
  /** 未手动选外观时用 defaultSetting.theme；已选手动时用 TAKT_THEME / app-setting */
  const mode = ref<TaktThemeMode>(
    initialSetting.appearanceUserOverride
      ? readStoredThemeMode()
      : initialSetting.theme,
  );
  /** 系统是否偏好深色（跟随 OS） */
  const prefersDark = usePreferredDark();

  /**
   * 实际生效的主题（light / dark）
   */
  const resolvedTheme = computed<TaktResolvedTheme>(() => {
    if (mode.value === 'system') {
      return prefersDark.value ? 'dark' : 'light';
    }
    return mode.value;
  });

  watch(
    resolvedTheme,
    (resolved) => {
      applyThemeDom(resolved);
    },
    { immediate: true },
  );

  /**
   * 设置主题模式
   * @param {TaktThemeMode} next 目标模式
   * @param {TaktSetThemeModeOptions} options 是否用户主动切换
   * @returns {void}
   */
  function setThemeMode(next: TaktThemeMode, options?: TaktSetThemeModeOptions): void {
    const userInitiated = options?.userInitiated !== false;
    const silent = options?.silent === true;
    const settingStore = useSettingStore();

    if (mode.value === next) {
      if (userInitiated && !settingStore.setting.appearanceUserOverride) {
        settingStore.patchSetting({ appearanceUserOverride: true });
      }
      return;
    }

    mode.value = next;
    localStorage.setItem(TAKT_THEME_STORAGE_KEY, next);

    if (userInitiated) {
      const patch: Partial<AppSetting> = {
        appearanceUserOverride: true,
      };
      if (next === 'light' || next === 'dark') {
        patch.theme = next;
      }
      settingStore.patchSetting(patch);
    } else if (next === 'light' || next === 'dark') {
      if (settingStore.setting.appearanceUserOverride && settingStore.setting.theme !== next) {
        settingStore.patchSetting({ theme: next });
      }
    }

    if (!silent) {
      EventBus.emit('theme:change', { theme: resolvedTheme.value });
    }
  }

  /**
   * 在浅色与深色之间切换（不经过 system）
   * @returns {void}
   */
  function toggleThemeMode(): void {
    setThemeMode(resolvedTheme.value === 'dark' ? 'light' : 'dark');
  }

  return {
    mode,
    resolvedTheme,
    setThemeMode,
    toggleThemeMode,
  };
});

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/stores/common
// 文件名称：useSettingStore.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：全局应用偏好 Pinia 状态（localStorage + 登录后同步 TaktUserPreferences/me）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { AppSetting } from '@/types/setting.types';
import {
  defaultSetting,
  normalizeSetting,
  readSettingFromStorage,
  saveSettingToStorage,
} from '@/setting';
import {
  getUserPreferenceMe,
  updateUserPreferenceMe,
} from '@/api/identity/userPreference.api';
import { applySettings } from '@/utils/applySettings';
import { TAKT_ACCESS_TOKEN_STORAGE_KEY, TAKT_LOCALE_STORAGE_KEY } from '@/utils/common';
import { createLogger } from '@/utils/logger';
import {
  beginDemoSettingTransition,
  commitDemoReadonlyAfterPersist,
} from '@/utils/taktDemoMode';
import i18n from '@/locales';

export type { AppSetting, ThemeColor, ThemeColorConfig } from '@/types/setting.types';

export {
  defaultSetting,
  getSetting,
  getThemeColorValue,
  normalizeSetting,
  STORAGE_KEY,
  themeColorI18nKeyMap,
  themeColorMap,
  themeColorToPreset,
  validateFontSize,
} from '@/setting';

const settingSyncLogger = createLogger('setting-sync');

/** 服务端 upsert debounce（ms） */
const SERVER_SYNC_DEBOUNCE_MS = 500;

/**
 * 全局应用偏好 Store
 */
export const useSettingStore = defineStore('setting', () => {
  /** 当前应用偏好（初始化自 localStorage） */
  const setting = ref<AppSetting>(readSettingFromStorage());

  /** 服务端同步定时器 */
  let syncTimer: ReturnType<typeof setTimeout> | null = null;

  /** hydrate 进行中时跳过回写，避免拉库触发 PUT */
  let hydrating = false;

  /** 本次 Demo 开→关/关→开 后，落库成功时是否提示（仅打开 Demo 为 true） */
  let pendingDemoOnNotify = false;

  /**
   * 是否已登录（有 access token 才同步服务端；避免与 useUserStore 循环依赖）
   * @returns {boolean} 已登录为 true
   */
  function canSyncToServer(): boolean {
    try {
      return !!localStorage.getItem(TAKT_ACCESS_TOKEN_STORAGE_KEY)?.trim();
    } catch {
      return false;
    }
  }

  /**
   * 组装写入服务端的 JSON（AppSetting + locale）
   * @param payload AppSetting
   * @returns {string} JSON
   */
  function buildPreferenceJson(payload: AppSetting): string {
    const locale =
      String(i18n.global.locale.value || '').trim() ||
      localStorage.getItem(TAKT_LOCALE_STORAGE_KEY)?.trim() ||
      'zh-CN';
    return JSON.stringify({ ...payload, locale });
  }

  /**
   * 防抖写入当前用户偏好到服务端；Demo 开关变更时落库成功后再生效只读并提示
   * @param payload 规范化后的 AppSetting
   */
  function scheduleServerSync(payload: AppSetting): void {
    if (hydrating) {
      return;
    }

    if (!canSyncToServer()) {
      const notifyDemoOn = pendingDemoOnNotify && !!payload.demo;
      pendingDemoOnNotify = false;
      commitDemoReadonlyAfterPersist(!!payload.demo, { notify: notifyDemoOn });
      return;
    }

    if (syncTimer != null) {
      clearTimeout(syncTimer);
    }

    syncTimer = setTimeout(() => {
      syncTimer = null;
      const notifyDemoOn = pendingDemoOnNotify && !!payload.demo;
      pendingDemoOnNotify = false;
      const json = buildPreferenceJson(payload);
      void updateUserPreferenceMe(json)
        .then(() => {
          commitDemoReadonlyAfterPersist(!!payload.demo, { notify: notifyDemoOn });
        })
        .catch((error: unknown) => {
          settingSyncLogger.warn('upsert user preference failed', error);
          commitDemoReadonlyAfterPersist(!!payload.demo, { notify: false });
        });
    }, SERVER_SYNC_DEBOUNCE_MS);
  }

  /**
   * 语言切换后触发偏好落库（不改 AppSetting 本体）
   */
  function scheduleLocaleSync(): void {
    scheduleServerSync(setting.value);
  }

  /**
   * 更新并持久化设置（localStorage；登录态 debounce 写库）
   * @param next 完整或合并后的设置对象
   */
  function setSetting(next: AppSetting): void {
    const prevDemo = setting.value.demo;
    const normalized = normalizeSetting(next);
    if (prevDemo !== normalized.demo) {
      beginDemoSettingTransition();
      pendingDemoOnNotify = !!normalized.demo;
    }
    setting.value = normalized;
    saveSettingToStorage(normalized);
    scheduleServerSync(normalized);
  }

  /**
   * 局部更新并持久化（不触发 applySettings，供 theme Store 回写）
   * @param partial 局部字段
   */
  function patchSetting(partial: Partial<AppSetting>): void {
    setSetting({ ...setting.value, ...partial });
  }

  /**
   * 恢复默认设置并持久化
   */
  function resetSetting(): void {
    setSetting(defaultSetting);
  }

  /**
   * 标记用户已手动选择主题外观（明暗/主色）；布局等其他偏好不受影响
   */
  function markAppearanceUserOverride(): void {
    if (!setting.value.appearanceUserOverride) {
      patchSetting({ appearanceUserOverride: true });
    }
  }

  /**
   * 从服务端拉取当前用户偏好并合并到本地（登录后调用）
   * @description 不直接调用 locale Store（避免循环依赖）；若有语言偏好则经 onLocale 回调交由编排层应用（须在 hydrating 期间同步调用，避免回写）
   * @param onLocale 可选：应用偏好中的语言（如 useLocaleStore().setLocale）
   * @returns {Promise<void>}
   */
  async function hydrateFromServer(onLocale?: (locale: string) => void): Promise<void> {
    if (!canSyncToServer()) {
      return;
    }

    hydrating = true;
    try {
      const remote = await getUserPreferenceMe();
      const raw = remote?.preferenceJson?.trim();
      if (!raw) {
        scheduleServerSync(setting.value);
        return;
      }

      let parsed: unknown;
      try {
        parsed = JSON.parse(raw) as unknown;
      } catch (error: unknown) {
        settingSyncLogger.warn('invalid preferenceJson from server', error);
        return;
      }

      if (parsed == null || typeof parsed !== 'object') {
        return;
      }

      const bag = parsed as Partial<AppSetting> & { locale?: string };
      const normalized = normalizeSetting({
        ...defaultSetting,
        ...bag,
      });
      setting.value = normalized;
      saveSettingToStorage(normalized);
      applySettings();
      commitDemoReadonlyAfterPersist(!!normalized.demo, { notify: false });

      const locale =
        remote?.userLocale?.trim() ||
        (typeof bag.locale === 'string' ? bag.locale.trim() : '');
      if (locale) {
        onLocale?.(locale);
      }
    } catch (error: unknown) {
      settingSyncLogger.warn('hydrate user preference failed', error);
    } finally {
      hydrating = false;
    }
  }

  return {
    setting,
    setSetting,
    patchSetting,
    resetSetting,
    markAppearanceUserOverride,
    hydrateFromServer,
    scheduleLocaleSync,
  };
});

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/stores/foundation
// 文件名称：useLocaleStore.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：语言区域状态管理（vue-i18n + 后端 GetCultureOptionsAsync / TaktSelectOption）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import i18n from '@/locales';
import { getCultureOptions } from '@/api/foundation/culture.api';
import { getSessionCultureOptions } from '@/api/identity/auth.api';
import type { TaktSelectOption } from '@/types/common.types';
import { EventBus } from '@/utils/eventBus';
import { useUserStore } from '@/stores/identity/useUserStore';
import { useTenantStore } from '@/stores/identity/useTenantStore';
import { useTranslationStore } from '@/stores/foundation/useTranslationStore';
import {
  TAKT_DEFAULT_LOCALE,
  TAKT_LOCALE_STORAGE_KEY,
} from '@/utils/common';
import {
  readStoredLocale,
  resolveLoginPreviewLocale,
  resolveTaktCultureCode,
  setRuntimeSupportedCultureCodes,
  syncTaktComponentLocales,
} from '@/utils/taktLocaleSync';
import { translateLocaleMessage } from '@/utils/taktI18nMessage';

/**
 * 解析发往后端的 Accept-Language（与 vue-i18n 当前 locale 一致）
 * @returns 区域文化编码
 */
export function resolveRequestLocale(): string {
  const current = i18n.global.locale.value;

  if (typeof current === 'string') {
    return resolveTaktCultureCode(current);
  }

  return readStoredLocale();
}

/**
 * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
 * @param option 语言选项
 */
export function resolveCultureCode(option: TaktSelectOption): string {
  return resolveTaktCultureCode(String(option.dictValue));
}

/**
 * 语言图标（extValue = Icon）
 * @param option 语言选项
 */
export function resolveCultureIcon(option: TaktSelectOption): string | undefined {
  if (option.extValue === undefined || option.extValue === null) {
    return undefined;
  }

  const icon = String(option.extValue).trim();

  return icon ? icon : undefined;
}

/**
 * 默认语言（extLabel = IsDefault，字典 sys_yes_no；1=是）
 * @param option 语言选项
 */
export function resolveCultureIsDefault(option: TaktSelectOption): boolean {
  return String(option.extLabel ?? '') === '1';
}

/**
 * 解析加载语言选项失败文案
 * @param error 捕获的错误
 */
function resolveCultureLoadErrorMessage(error: unknown): string {
  if (error instanceof Error && error.message) {
    return error.message;
  }

  return translateLocaleMessage('common.feedback.load.failed', {
    target: translateLocaleMessage('common.page.entity.culturelist'),
  });
}

/**
 * 语言区域状态管理
 */
export const useLocaleStore = defineStore('locale', () => {
  const cultureOptions = ref<TaktSelectOption[]>([]);
  const loading = ref(false);
  const loaded = ref(false);
  /** 上次加载语言列表时的上下文键（登录态 / 租户编码 / 会话合并） */
  const cultureOptionsContextKey = ref('');

  /**
   * 登录页用户是否已手动切换语言（为 true 时不再被用户默认语言覆盖）
   */
  const loginLocaleUserOverride = ref(false);

  /**
   * 当前 vue-i18n locale（与选项 dictValue / CultureCode 一致）
   */
  const currentLocale = computed(() => i18n.global.locale.value);

  /**
   * 当前选中的语言选项
   */
  const currentCultureOption = computed(() =>
    cultureOptions.value.find((item) => resolveCultureCode(item) === currentLocale.value)
  );

  /**
   * 解析当前语言列表加载上下文键
   * @param tenantCode 显式租户（未登录会话接口用）
   */
  function resolveCultureOptionsContextKey(tenantCode?: string): string {
    const userStore = useUserStore();
    if (userStore.isLoggedIn) {
      return `auth:${useTenantStore().tenantCode || ''}`;
    }

    const code = tenantCode?.trim() || useTenantStore().tenantCode?.trim() || '';
    return code ? `session:${code}` : 'session:all';
  }

  /**
   * 从后端加载启用语言选项（已登录：GetCultureOptions；登录页：SessionCultureOptions）
   * @param options.force 为 true 时强制刷新（租户切换等）
   * @param options.tenantCode 未登录时显式指定租户（可选）
   */
  /**
   * 清除登录页会话语言选项（租户未校验或切换租户前）
   */
  function clearSessionCultureOptions(): void {
    cultureOptions.value = [];
    cultureOptionsContextKey.value = '';
    loaded.value = false;
    setRuntimeSupportedCultureCodes([]);
  }

  async function loadCultureOptionsAsync(options: { force?: boolean; tenantCode?: string } = {}): Promise<void> {
    const userStore = useUserStore();
    const explicitTenant = options.tenantCode?.trim() || useTenantStore().tenantCode?.trim() || '';
    if (!userStore.isLoggedIn && !explicitTenant) {
      clearSessionCultureOptions();
      return;
    }

    const contextKey = resolveCultureOptionsContextKey(options.tenantCode);
    if (loading.value) {
      return;
    }

    if (loaded.value && !options.force && cultureOptionsContextKey.value === contextKey) {
      return;
    }

    loading.value = true;

    try {
      const list = userStore.isLoggedIn
        ? await getCultureOptions()
        : await getSessionCultureOptions(explicitTenant);

      if (list.length === 0) {
        throw new Error(
          translateLocaleMessage('common.feedback.load.empty', {
            target: translateLocaleMessage('common.page.entity.culturelist'),
          }),
        );
      }

      cultureOptions.value = list;
      cultureOptionsContextKey.value = contextKey;
      loaded.value = true;
      setRuntimeSupportedCultureCodes(list.map((item) => String(item.dictValue ?? '').trim()));
    } catch (error) {
      cultureOptions.value = [];
      cultureOptionsContextKey.value = '';
      loaded.value = false;

      EventBus.emit('notification:show', {
        type: 'error',
        message: resolveCultureLoadErrorMessage(error),
      });

      throw error;
    } finally {
      loading.value = false;
    }
  }

  /**
   * 当前租户可选 CultureCode 列表（与 cultureOptions.dictValue 一致）
   */
  function listAvailableCultureCodes(): string[] {
    return cultureOptions.value
      .map((item) => String(item.dictValue ?? '').trim())
      .filter((code) => code.length > 0);
  }

  /**
   * 应用语言（vue-i18n + localStorage + 后续请求 Accept-Language）
   * @param cultureCode 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   * @param persist 是否写入 localStorage（用户主动选择时为 true）
   */
  function applyLocale(cultureCode: string, persist = true): void {
    const availableCodes = listAvailableCultureCodes();
    const normalized = resolveTaktCultureCode(
      cultureCode,
      availableCodes.length > 0 ? availableCodes : undefined,
    );

    if (i18n.global.locale.value !== normalized) {
      i18n.global.locale.value = normalized as typeof i18n.global.locale.value;
    }

    syncTaktComponentLocales(normalized);

    if (persist) {
      localStorage.setItem(TAKT_LOCALE_STORAGE_KEY, normalized);
    }
  }

  /**
   * 切换当前语言（用户选择；同步后端 Accept-Language）
   * @param cultureCode 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  function setLocale(cultureCode: string): void {
    const availableCodes = listAvailableCultureCodes();
    const normalized = resolveTaktCultureCode(
      cultureCode,
      availableCodes.length > 0 ? availableCodes : undefined,
    );
    const isLoggedIn = useUserStore().isLoggedIn;

    if (!isLoggedIn) {
      loginLocaleUserOverride.value = true;
    }

    if (i18n.global.locale.value === normalized) {
      if (isLoggedIn) {
        const translationStore = useTranslationStore();
        if (translationStore.isLocaleLoaded(normalized)) {
          translationStore.bumpDynamicRevision();
        } else {
          void translationStore.loadTranslationMessagesAsync(normalized);
        }
      } else {
        void useTranslationStore().loadPublicTranslationMessagesAsync(normalized);
      }
      return;
    }

    applyLocale(normalized, true);
    // 偏好落库由 bootstrap 订阅 locale:change → setting.scheduleLocaleSync（禁止 store 互引）
    EventBus.emit('locale:change', { locale: normalized });

    if (isLoggedIn) {
      void useTranslationStore().loadTranslationMessagesAsync(normalized);
    } else {
      void useTranslationStore().loadPublicTranslationMessagesAsync(normalized);
    }
  }

  /**
   * 启动时对齐默认语言（无 localStorage 记录时为 en-US）
   */
  function initLocaleFromStorage(): void {
    applyLocale(readStoredLocale(), Boolean(localStorage.getItem(TAKT_LOCALE_STORAGE_KEY)));
  }

  /**
   * 登录页进入时重置界面语言（清除上次会话残留，待用户默认语言解析后再写入）
   */
  function resetLocaleForLoginPage(): void {
    loginLocaleUserOverride.value = false;
    localStorage.removeItem(TAKT_LOCALE_STORAGE_KEY);
    clearSessionCultureOptions();
    applyLocale(TAKT_DEFAULT_LOCALE, false);
  }

  /**
   * 清除登录页手动语言覆盖（租户或用户名变更、重新解析默认公司前调用）
   */
  function clearLoginLocaleUserOverride(): void {
    loginLocaleUserOverride.value = false;
  }

  /**
   * 按用户 DefaultCulture 应用登录预览语言（写入 localStorage）
   * 浏览器语言与用户默认不一致时优先浏览器；浏览器语言不在租户可选列表时回退用户默认
   * @param cultureCode 用户区域文化编码 BCP47
   */
  function applyLoginUserLocale(
    cultureCode: string,
    _context?: {
      tenantCode?: string;
      userName?: string;
      companyCode?: string;
      previewKey?: string;
    },
  ): void {
    if (loginLocaleUserOverride.value) {
      return;
    }

    const availableCultureCodes = cultureOptions.value.map((item) => resolveCultureCode(item));
    const normalized = resolveLoginPreviewLocale(cultureCode, availableCultureCodes);
    applyLocale(normalized, true);
    void useTranslationStore().loadPublicTranslationMessagesAsync(normalized);
  }

  /**
   * 读取用户已持久化的语言偏好（localStorage）；无有效记录时返回 null
   * @returns 区域文化编码或 null
   */
  function readPersistedLocalePreference(): string | null {
    const stored = localStorage.getItem(TAKT_LOCALE_STORAGE_KEY)?.trim();
    if (!stored) {
      return null;
    }

    const availableCodes = listAvailableCultureCodes();
    if (availableCodes.length === 0) {
      return stored;
    }

    const inList = availableCodes.find((code) => code.toLowerCase() === stored.toLowerCase());
    if (!inList) {
      return null;
    }

    return inList;
  }

  /**
   * 登录后对齐界面语言：优先保留登录页已写入 localStorage 的语言，无记录时才用用户 DefaultCulture
   * @param cultureCode 用户区域文化编码 BCP47
   */
  function applyUserProfileLocale(cultureCode: string): void {
    const availableCodes = listAvailableCultureCodes();
    const persisted = readPersistedLocalePreference();

    if (persisted) {
      if (i18n.global.locale.value !== persisted) {
        applyLocale(persisted, true);
      }

      void useTranslationStore().loadTranslationMessagesAsync(persisted);
      clearLoginLocaleUserOverride();
      return;
    }

    if (loginLocaleUserOverride.value) {
      void useTranslationStore().loadTranslationMessagesAsync(String(i18n.global.locale.value));
      clearLoginLocaleUserOverride();
      return;
    }

    const normalizedProfile = resolveTaktCultureCode(
      cultureCode.trim(),
      availableCodes.length > 0 ? availableCodes : undefined,
    );

    if (i18n.global.locale.value === normalizedProfile) {
      void useTranslationStore().loadTranslationMessagesAsync(normalizedProfile);
      return;
    }

    applyLocale(normalizedProfile, true);
    void useTranslationStore().loadTranslationMessagesAsync(normalizedProfile);
  }

  return {
    cultureOptions,
    loading,
    loaded,
    currentLocale,
    currentCultureOption,
    loadCultureOptionsAsync,
    clearSessionCultureOptions,
    initLocaleFromStorage,
    resetLocaleForLoginPage,
    clearLoginLocaleUserOverride,
    applyLoginUserLocale,
    applyUserProfileLocale,
    applyLocale,
    setLocale,
  };
});

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/stores/foundation
// 文件名称：useTranslationStore.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：动态翻译 Pinia 缓存（takt_foundation_translation → vue-i18n mergeLocaleMessage）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import i18n, { mergeDynamicLocaleMessages } from '@/locales';
import { getTranslationMessages } from '@/api/foundation/translation.api';
import { resolveTaktCultureCode } from '@/utils/taktLocaleSync';
import { useUserStore } from '@/stores/identity/useUserStore';
import { useTenantStore } from '@/stores/identity/useTenantStore';
import { createLogger } from '@/utils/logger';
import { resolveHttpErrorMessage } from '@/utils/taktHttpErrorMessage';
import { emitNotification } from '@/utils/eventBus';

const translationLogger = createLogger('translation');

/**
 * 动态翻译状态管理（与 locale store 联动，覆盖/补充静态 locales 包）
 */
export const useTranslationStore = defineStore('translation', () => {
  const loadedLocales = ref<Record<string, boolean>>({});
  const loading = ref(false);

  /** 动态翻译合并版本（驱动侧栏菜单等依赖 menu.* 的视图重算） */
  const dynamicRevision = ref(0);

  const loadingPromises = new Map<string, Promise<void>>();

  /**
   * 是否至少已加载一种语言的动态翻译
   */
  const isLoaded = computed(() => Object.keys(loadedLocales.value).length > 0);

  /**
   * 构建动态翻译加载缓存键（租户 + 语言；登录页须在租户校验通过后再拉取 entity.*）
   * @param cultureCode 文化编码
   */
  function buildLoadedCacheKey(cultureCode: string): string {
    const tenantCode = useTenantStore().tenantCode?.trim() || '';
    return `${tenantCode}|${cultureCode}`;
  }

  /**
   * 指定语言是否已加载动态翻译
   * @param cultureCode 文化编码
   */
  function isLocaleLoaded(cultureCode: string): boolean {
    return loadedLocales.value[buildLoadedCacheKey(cultureCode)] === true;
  }

  /**
   * 递增动态翻译版本，触发依赖 menu.* / entity.* 的 computed 重算
   */
  function bumpDynamicRevision(): void {
    dynamicRevision.value += 1;
  }

  /**
   * 解析 API 返回的扁平翻译（兼容 camelCase / PascalCase）
   * @param result 后端 DTO
   */
  function extractTranslationMessages(result: Awaited<ReturnType<typeof getTranslationMessages>>): Record<string, string> {
    if (result.messages && typeof result.messages === 'object') {
      return result.messages;
    }

    const legacyMessages = (result as { Messages?: Record<string, string> }).Messages;
    return legacyMessages ?? {};
  }

  /**
   * 从后端拉取指定语言的前端翻译键值（GET /messages）
   * @param cultureCode 文化编码
   */
  async function fetchFrontendTranslationMapAsync(cultureCode: string): Promise<Record<string, string>> {
    const normalizedCulture = resolveTaktCultureCode(cultureCode);
    const result = await getTranslationMessages(normalizedCulture);
    return extractTranslationMessages(result);
  }

  /**
   * 加载指定语言的动态翻译并合并到 vue-i18n
   * @param cultureCode 文化编码，默认当前 locale
   * @param options.allowAnonymous 为 true 时未登录也可拉取（登录页等公开场景）
   */
  async function loadTranslationMessagesAsync(
    cultureCode?: string,
    options?: { allowAnonymous?: boolean }
  ): Promise<void> {
    const locale = resolveTaktCultureCode(cultureCode ?? String(i18n.global.locale));
    const userStore = useUserStore();
    const cacheKey = buildLoadedCacheKey(locale);

    if (!options?.allowAnonymous && !userStore.isLoggedIn) {
      return;
    }

    if (loadedLocales.value[cacheKey]) {
      return;
    }

    const pending = loadingPromises.get(cacheKey);

    if (pending) {
      return pending;
    }

    loading.value = true;

    const promise = (async () => {
      try {
        const flatMessages = await fetchFrontendTranslationMapAsync(locale);
        mergeDynamicLocaleMessages(locale, flatMessages);
        loadedLocales.value = { ...loadedLocales.value, [cacheKey]: true };
        bumpDynamicRevision();
      } catch (error) {
        const errMsg = resolveHttpErrorMessage(error);
        translationLogger.warn(
          '加载动态翻译失败',
          { action: 'loadTranslationMessages', cultureCode: locale, cacheKey, message: errMsg },
          error
        );
        if (userStore.isLoggedIn && errMsg) {
          emitNotification('error', errMsg);
        }
      } finally {
        loadingPromises.delete(cacheKey);
        loading.value = loadingPromises.size > 0;
      }
    })();

    loadingPromises.set(cacheKey, promise);
    return promise;
  }

  /**
   * 清除未登录场景的动态翻译加载缓存（租户变更/作废时须重拉 entity.*）
   */
  function resetPublicTranslationCache(): void {
    if (useUserStore().isLoggedIn) {
      return;
    }
    loadedLocales.value = {};
    loadingPromises.clear();
    loading.value = false;
  }

  /**
   * 预加载多种语言的动态翻译
   * @param cultureCodes 文化编码列表
   */
  async function loadTranslationMessagesBatchAsync(cultureCodes: string[]): Promise<void> {
    const uniqueCodes = [...new Set(cultureCodes.filter(Boolean))];

    await Promise.all(uniqueCodes.map((code) => loadTranslationMessagesAsync(code)));
  }

  /**
   * 未登录场景加载动态翻译（登录/注册/忘记密码页使用后端 common.* / entity.*）
   * @param cultureCode 文化编码，默认当前 locale
   */
  async function loadPublicTranslationMessagesAsync(cultureCode?: string): Promise<void> {
    return loadTranslationMessagesAsync(cultureCode, { allowAnonymous: true });
  }

  /**
   * 重置动态翻译加载状态（登出、租户切换等场景；静态 locales 包不受影响）
   */
  function resetTranslationMessages(): void {
    loadedLocales.value = {};
    loadingPromises.clear();
    loading.value = false;
    dynamicRevision.value = 0;
  }

  return {
    loadedLocales,
    loading,
    dynamicRevision,
    isLoaded,
    isLocaleLoaded,
    bumpDynamicRevision,
    loadTranslationMessagesAsync,
    loadPublicTranslationMessagesAsync,
    loadTranslationMessagesBatchAsync,
    resetPublicTranslationCache,
    resetTranslationMessages,
  };
});

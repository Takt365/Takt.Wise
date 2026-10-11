// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/stores/identity
// 文件名称：useTenantStore.ts
// 创建时间：2026-05-22
// 创建人：Takt365(Cursor AI)
// 功能描述：租户与公司状态管理（动态选项 + 请求头同步；登录前须用户手动选择租户）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import { getSessionTenantOptions, getUserCompanyOptions } from '@/api/identity/auth.api';
import { resolveHttpErrorMessage } from '@/utils/taktHttpErrorMessage';
import type { UserInfoResponse } from '@/types/identity/login.types';
import type { TaktSelectOption } from '@/types/common.types';
import { EventBus } from '@/utils/eventBus';
import {
  TAKT_ACCESS_TOKEN_STORAGE_KEY,
  TAKT_COMPANY_CODE_STORAGE_KEY,
  TAKT_COMPANY_USER_PICKED_STORAGE_KEY,
  TAKT_OAUTH_PENDING_TENANT_STORAGE_KEY,
  TAKT_TENANT_CODE_STORAGE_KEY,
} from '@/utils/common';
import { translateLocaleMessage } from '@/utils/taktI18nMessage';

/**
 * 是否已持有访问令牌（避免 tenant ↔ user store 循环依赖）
 */
function hasAccessToken(): boolean {
  return !!localStorage.getItem(TAKT_ACCESS_TOKEN_STORAGE_KEY);
}

/**
 * 选项键值（dictValue）
 * @param option 下拉选项
 */
export function resolveSelectOptionValue(option: TaktSelectOption): string {
  return String(option.dictValue);
}

/**
 * 是否默认项（extLabel = 1）
 * @param option 下拉选项
 */
export function resolveSelectOptionIsDefault(option: TaktSelectOption): boolean {
  return String(option.extLabel ?? '') === '1';
}

/**
 * 租户与公司状态管理
 */
export const useTenantStore = defineStore('tenant', () => {
  /** 租户编码：初始为空，不从 localStorage 恢复；仅选择器或登录后 /me 写入 */
  const tenantCode = ref<string>('');
  /** 当前公司：初始为空（与 tenantCode 一致，不从 localStorage 恢复；由 /me 或用户切换写入） */
  const companyCode = ref<string>('');
  const tenantOptions = ref<TaktSelectOption[]>([]);
  const companyOptions = ref<TaktSelectOption[]>([]);
  const tenantLoading = ref(false);
  const companyLoading = ref(false);
  const tenantLoaded = ref(false);

  const currentTenantOption = computed(() =>
    tenantOptions.value.find((item) => resolveSelectOptionValue(item) === tenantCode.value)
  );

  const currentCompanyOption = computed(() =>
    companyOptions.value.find((item) => resolveSelectOptionValue(item) === companyCode.value)
  );

  /**
   * 当前登录公司关联工厂（来自公司选项 ExtValue = TaktCompany.RelatedPlant）
   */
  const currentCompanyRelatedPlant = computed(() =>
    String(currentCompanyOption.value?.extValue ?? '').trim(),
  );

  /**
   * 清除当前租户选择（不预选、不恢复 localStorage）
   */
  function resetTenantSelection(): void {
    tenantCode.value = '';
    localStorage.removeItem(TAKT_TENANT_CODE_STORAGE_KEY);
  }

  /**
   * 应用默认公司（extLabel=1，来自后端 UserCompany.is_default）
   */
  function applyDefaultCompany(): void {
    if (companyOptions.value.length === 0) {
      return;
    }

    const exists = companyOptions.value.some(
      (item) => resolveSelectOptionValue(item) === companyCode.value
    );

    if (exists) {
      return;
    }

    const defaultOption = companyOptions.value.find((item) => resolveSelectOptionIsDefault(item));
    if (!defaultOption) {
      return;
    }
    setCompany(resolveSelectOptionValue(defaultOption), { silent: true });
  }

  /**
   * 设置租户（仅用户在下拉中选择后调用；已登录后不可切换）
   * @param code 租户编码
   */
  function setTenant(code: string): void {
    if (!code) {
      return;
    }

    if (hasAccessToken() && tenantCode.value && code !== tenantCode.value) {
      return;
    }

    const trimmed = code.trim();
    if (tenantCode.value === trimmed) {
      return;
    }

    tenantCode.value = trimmed;
    localStorage.setItem(TAKT_TENANT_CODE_STORAGE_KEY, trimmed);
    EventBus.emit('tenant:change', { tenantCode: trimmed });

    if (hasAccessToken()) {
      void loadCompanyOptionsAsync().catch(() => undefined);
    }
  }

  /**
   * 清除公司选择（保留租户；OAuth 回调拉 /me 前调用，避免旧 localStorage 覆盖默认公司）
   */
  function resetCompanySelection(): void {
    companyCode.value = '';
    localStorage.removeItem(TAKT_COMPANY_CODE_STORAGE_KEY);
    sessionStorage.removeItem(TAKT_COMPANY_USER_PICKED_STORAGE_KEY);
  }

  /**
   * 设置公司
   * @param code 公司编码
   * @param options.rememberSession 用户在公司切换器手动选择时为 true（同会话刷新可恢复）
   * @param options.silent 为 true 时不触发 company:change（/me 同步公司等场景）
   */
  function setCompany(code: string, options?: { rememberSession?: boolean; silent?: boolean }): void {
    if (!code) {
      return;
    }
    const normalized = code.trim();
    const unchanged = companyCode.value.trim().toLowerCase() === normalized.toLowerCase();
    companyCode.value = normalized;
    localStorage.setItem(TAKT_COMPANY_CODE_STORAGE_KEY, normalized);
    if (options?.rememberSession) {
      sessionStorage.setItem(TAKT_COMPANY_USER_PICKED_STORAGE_KEY, normalized);
    }
    if (options?.silent || unchanged) {
      return;
    }
    EventBus.emit('company:change', { companyCode: normalized });
  }

  /**
   * 根据 /me 解析当前公司（会话内手动选择优先，否则后端 UserCompany.is_default）
   * @param profile 当前用户资料
   */
  function applyCompanyFromUserProfile(profile: UserInfoResponse): void {
    const accessible = profile.accessibleCompanies ?? [];
    const sessionPick = sessionStorage.getItem(TAKT_COMPANY_USER_PICKED_STORAGE_KEY)?.trim() ?? '';
    if (
      sessionPick
      && accessible.some((item) => item.toLowerCase() === sessionPick.toLowerCase())
    ) {
      setCompany(sessionPick, { silent: true });
      return;
    }
    const serverCode = profile.companyCode?.trim();
    if (
      serverCode
      && accessible.some((item) => item.toLowerCase() === serverCode.toLowerCase())
    ) {
      setCompany(serverCode, { silent: true });
    }
  }

  /**
   * 清除公司选项列表
   */
  function clearCompanyOptions(): void {
    companyOptions.value = [];
  }

  /**
   * 清除租户与公司选择
   */
  function clearTenant(): void {
    resetTenantSelection();
    clearOAuthTenantCode();
    companyCode.value = '';
    companyOptions.value = [];
    tenantOptions.value = [];
    tenantLoaded.value = false;
    localStorage.removeItem(TAKT_COMPANY_CODE_STORAGE_KEY);
    sessionStorage.removeItem(TAKT_COMPANY_USER_PICKED_STORAGE_KEY);
  }

  /**
   * 清除当前租户选择（登录页校验失败等场景）
   */
  function clearTenantCode(): void {
    resetTenantSelection();
    sessionStorage.removeItem(TAKT_OAUTH_PENDING_TENANT_STORAGE_KEY);
  }

  /**
   * 登录跳转 OAuth 前暂存租户（授权回调整页刷新后恢复请求头）
   * @param code 租户编码
   */
  function persistOAuthTenantCode(code: string): void {
    const trimmed = code.trim();
    if (!trimmed) {
      return;
    }
    sessionStorage.setItem(TAKT_OAUTH_PENDING_TENANT_STORAGE_KEY, trimmed);
    tenantCode.value = trimmed;
    localStorage.setItem(TAKT_TENANT_CODE_STORAGE_KEY, trimmed);
  }

  /**
   * 从 sessionStorage / localStorage 恢复租户与公司到 Pinia（内存为空时；刷新页面后 /me 前须调用）
   */
  function restoreTenantCodeFromStorage(): void {
    if (!tenantCode.value.trim()) {
      const saved =
        sessionStorage.getItem(TAKT_OAUTH_PENDING_TENANT_STORAGE_KEY)?.trim() ||
        localStorage.getItem(TAKT_TENANT_CODE_STORAGE_KEY)?.trim() ||
        '';

      if (saved) {
        tenantCode.value = saved;
        localStorage.setItem(TAKT_TENANT_CODE_STORAGE_KEY, saved);
      }
    }

    if (!companyCode.value.trim()) {
      const savedCompany = localStorage.getItem(TAKT_COMPANY_CODE_STORAGE_KEY)?.trim() ?? '';
      if (savedCompany) {
        companyCode.value = savedCompany;
      }
    }
  }

  /**
   * 清除 OAuth 登录暂存租户
   */
  function clearOAuthTenantCode(): void {
    sessionStorage.removeItem(TAKT_OAUTH_PENDING_TENANT_STORAGE_KEY);
  }

  /**
   * 登录页租户白名单是否在配置列表中（与 GET session/tenant-options 的 dictValue 一致）
   * @param code 租户编码
   */
  function isLoginTenantCodeAllowed(code: string): boolean {
    const trimmed = code.trim();
    if (!trimmed) {
      return false;
    }

    return tenantOptions.value.some(
      (item) => resolveSelectOptionValue(item).toLowerCase() === trimmed.toLowerCase(),
    );
  }

  /**
   * 加载登录页租户白名单（仅缓存选项，不修改 tenantCode、不应用默认项）
   */
  async function loadLoginTenantCatalogAsync(): Promise<void> {
    if (tenantLoaded.value && tenantOptions.value.length > 0) {
      return;
    }

    if (tenantLoading.value) {
      return;
    }

    tenantLoading.value = true;

    try {
      const options = await getSessionTenantOptions();

      if (options.length === 0) {
        throw new Error(
          translateLocaleMessage('common.feedback.load.empty', {
            target: translateLocaleMessage('common.page.entity.tenantlist'),
          }),
        );
      }

      tenantOptions.value = options;
      tenantLoaded.value = true;
    } catch (error) {
      tenantOptions.value = [];
      tenantLoaded.value = false;
      throw new Error(resolveHttpErrorMessage(error));
    } finally {
      tenantLoading.value = false;
    }
  }

  /**
   * 加载租户选项（未登录供登录页选择；不自动预选，须用户手动选择）
   * @param forLoginPage 登录页为 true 时，即使本地残留 Token 也拉取租户列表
   */
  async function loadTenantOptionsAsync(forLoginPage = false): Promise<void> {
    if (hasAccessToken() && !forLoginPage) {
      return;
    }

    if (tenantLoading.value) {
      return;
    }

    tenantLoading.value = true;
    resetTenantSelection();

    try {
      const options = await getSessionTenantOptions();

      if (options.length === 0) {
        throw new Error(
          translateLocaleMessage('common.feedback.load.empty', {
            target: translateLocaleMessage('common.page.entity.tenantlist'),
          }),
        );
      }

      tenantOptions.value = options;
      tenantLoaded.value = true;
    } catch (error) {
      tenantOptions.value = [];
      tenantLoaded.value = false;
      resetTenantSelection();
      throw new Error(resolveHttpErrorMessage(error));
    } finally {
      tenantLoading.value = false;
    }
  }

  /**
   * 从用户资料同步当前租户与公司编码（登录后 /me，非登录页静态默认）
   * @param profile 当前用户资料
   */
  function syncFromUserProfile(profile: UserInfoResponse): void {
    if (profile.tenantCode) {
      tenantCode.value = profile.tenantCode;
      localStorage.setItem(TAKT_TENANT_CODE_STORAGE_KEY, profile.tenantCode);
    }
    applyCompanyFromUserProfile(profile);
  }

  /**
   * 加载公司选项（已登录：GetUserCompanyOptionsAsync，dictValue 为公司编码；不覆盖 /me 已写入的 companyCode）
   */
  async function loadCompanyOptionsAsync(): Promise<void> {
    if (!hasAccessToken() || !tenantCode.value) {
      companyOptions.value = [];
      return;
    }

    if (companyLoading.value) {
      return;
    }

    companyLoading.value = true;

    try {
      const options = await getUserCompanyOptions();
      companyOptions.value = options;
    } catch (error) {
      companyOptions.value = [];
      throw new Error(resolveHttpErrorMessage(error));
    } finally {
      companyLoading.value = false;
    }
  }

  /**
   * 登录后刷新公司选项（租户在登录时绑定，登录后不可切换租户）
   */
  async function refreshBusinessContextAsync(): Promise<void> {
    await loadCompanyOptionsAsync();
    if (!companyCode.value && companyOptions.value.length > 0) {
      applyDefaultCompany();
      return;
    }
    const existsInOptions = companyOptions.value.some(
      (item) => resolveSelectOptionValue(item) === companyCode.value,
    );
    if (companyCode.value && companyOptions.value.length > 0 && !existsInOptions) {
      applyDefaultCompany();
    }
  }

  return {
    tenantCode,
    companyCode,
    tenantOptions,
    companyOptions,
    tenantLoading,
    companyLoading,
    tenantLoaded,
    currentTenantOption,
    currentCompanyOption,
    currentCompanyRelatedPlant,
    resetCompanySelection,
    setTenant,
    setCompany,
    clearTenant,
    clearTenantCode,
    persistOAuthTenantCode,
    restoreTenantCodeFromStorage,
    clearOAuthTenantCode,
    clearCompanyOptions,
    isLoginTenantCodeAllowed,
    loadLoginTenantCatalogAsync,
    loadTenantOptionsAsync,
    loadCompanyOptionsAsync,
    refreshBusinessContextAsync,
    syncFromUserProfile,
  };
});

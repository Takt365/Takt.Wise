// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/composables
// 文件名称：useLoginFieldSync.ts
// 创建时间：2026-05-29
// 创建人：Takt365(Cursor AI)
// 功能描述：登录表单字段同步（租户校验 → 登录预览 单链路）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { ref } from 'vue';
import { message } from 'ant-design-vue';
import { useI18n } from 'vue-i18n';
import { validateSessionTenantCode } from '@/api/identity/auth.api';
import { TaktApiError } from '@/api/request';
import { resolveHttpErrorMessage } from '@/utils/taktHttpErrorMessage';
import { useLocaleStore } from '@/stores/foundation/useLocaleStore';
import { useTenantStore } from '@/stores/identity/useTenantStore';
import { useUserStore } from '@/stores/identity/useUserStore';
import type { UseLoginFieldSyncOptions } from '@/types/common.types';
import {
  TAKT_LOGIN_PREVIEW_DEBOUNCE_MS,
  TAKT_LOGIN_TENANT_VALIDATE_DEBOUNCE_MS,
  TAKT_TENANT_CODE_LENGTH,
} from '@/utils/common';
import { isValidLoginUserName, isValidTenantCode, LOGIN_USER_NAME_MAX_LENGTH } from '@/utils/regex';

export type { UseLoginFieldSyncOptions };

/**
 * 登录表单字段同步
 * @description
 * 链路 1：租户输入 → 规范化 → 远程校验 TaktTenant → 写入 tenantStore
 * 链路 2：租户校验通过 + 用户名格式完整 → 登录预览（默认公司 / 语言 / 假日）
 * @param options 表单读写与 formRef
 */
export function useLoginFieldSync(options: UseLoginFieldSyncOptions) {
  const { t } = useI18n();
  const tenantStore = useTenantStore();
  const userStore = useUserStore();
  const localeStore = useLocaleStore();

  /** 租户编码是否已通过后端校验 */
  const tenantValidated = ref(false);
  /** 租户编码是否正在校验 */
  const tenantValidating = ref(false);

  let tenantGeneration = 0;
  let tenantDebounceTimer: ReturnType<typeof setTimeout> | null = null;
  let tenantToastKey = '';
  let previewDebounceTimer: ReturnType<typeof setTimeout> | null = null;
  let previewToastKey = '';
  let lastPreviewKey = '';

  /**
   * 规范化租户编码（仅数字，最多 3 位）
   * @param value 原始输入
   */
  function normalizeTenantCode(value: string): string {
    return String(value ?? '').replace(/\D/g, '').slice(0, TAKT_TENANT_CODE_LENGTH);
  }

  /**
   * 规范化用户名（小写字母与数字）
   * @param value 原始输入
   */
  function normalizeUserName(value: string): string {
    return String(value ?? '').toLowerCase().replace(/[^a-z0-9]/g, '').slice(0, LOGIN_USER_NAME_MAX_LENGTH);
  }

  /**
   * 构建登录预览上下文键
   * @param tenantCode 租户编码
   * @param userName 用户名
   */
  function buildPreviewKey(tenantCode: string, userName: string): string {
    return `${tenantCode.trim()}|${userName.trim()}`;
  }

  /**
   * 取消租户校验防抖
   */
  function cancelTenantDebounce(): void {
    if (tenantDebounceTimer) {
      clearTimeout(tenantDebounceTimer);
      tenantDebounceTimer = null;
    }
  }

  /**
   * 取消登录预览防抖
   */
  function cancelPreviewDebounce(): void {
    if (previewDebounceTimer) {
      clearTimeout(previewDebounceTimer);
      previewDebounceTimer = null;
    }
  }

  /**
   * 弹出一次租户 Toast
   * @param text 文案
   * @param level 级别
   */
  function notifyTenantToast(text: string, level: 'error' | 'warning' = 'error'): void {
    const key = `${options.getTenantCode().trim()}|${text}|${level}`;
    if (tenantToastKey === key) {
      return;
    }
    tenantToastKey = key;
    if (level === 'error') {
      message.error(text);
      return;
    }
    message.warning(text);
  }

  /**
   * 弹出一次登录预览 Toast
   * @param text 文案
   * @param level 级别
   */
  function notifyPreviewToast(text: string, level: 'error' | 'warning' = 'warning'): void {
    const key = `${options.getTenantCode().trim()}|${options.getUserName().trim()}|${text}|${level}`;
    if (previewToastKey === key) {
      return;
    }
    previewToastKey = key;
    if (level === 'error') {
      message.error(text);
      return;
    }
    message.warning(text);
  }

  /**
   * 作废租户上下文与登录预览缓存
   */
  function invalidateTenantContext(): void {
    tenantGeneration += 1;
    cancelTenantDebounce();
    tenantValidated.value = false;
    tenantValidating.value = false;
    tenantToastKey = '';
    tenantStore.clearTenantCode();
    previewToastKey = '';
    lastPreviewKey = '';
    userStore.invalidateLoginPreview();
    localeStore.clearSessionCultureOptions();
  }

  /**
   * 租户格式不合法或未通过远程校验时，刷新表单项错误展示
   */
  async function showTenantFieldValidation(): Promise<void> {
    await syncTenantFormField();
  }

  /**
   * 作废登录预览缓存（用户名变更）
   */
  function invalidatePreviewContext(): void {
    cancelPreviewDebounce();
    previewToastKey = '';
    lastPreviewKey = '';
    userStore.invalidateLoginPreview();
  }

  /**
   * 同步租户表单项校验状态
   */
  async function syncTenantFormField(): Promise<void> {
    await options.formRef.value?.validateFields(['tenantCode']).catch(() => undefined);
  }

  /**
   * 解析租户校验失败文案（优先展示后端返回的库/表缺失说明）
   * @param error 捕获的异常
   */
  function resolveTenantValidateErrorMessage(error: unknown): string {
    if (error instanceof TaktApiError && error.message.trim()) {
      return error.message.trim();
    }

    return resolveHttpErrorMessage(error) || t('login.page.message.tenant.validate.fail');
  }

  /**
   * 远程校验租户编码
   * @param code 已规范化的租户编码
   * @param generation 发起时的世代号
   */
  async function remoteValidateTenant(code: string, generation: number): Promise<boolean> {
    tenantValidating.value = true;
    try {
      const ok = await validateSessionTenantCode(code);
      if (generation !== tenantGeneration) {
        return tenantValidated.value;
      }
      if (ok) {
        tenantValidated.value = true;
        tenantStore.setTenant(code);
        await syncTenantFormField();
        void localeStore.loadCultureOptionsAsync({ force: true, tenantCode: code }).catch(() => undefined);
        scheduleLoginPreview(false);
        return true;
      }
      tenantValidated.value = false;
      tenantStore.clearTenantCode();
      notifyTenantToast(t('login.page.message.tenant.not.found'));
      await syncTenantFormField();
      return false;
    } catch (error) {
      if (generation !== tenantGeneration) {
        return tenantValidated.value;
      }
      tenantValidated.value = false;
      tenantStore.clearTenantCode();
      notifyTenantToast(resolveTenantValidateErrorMessage(error));
      await syncTenantFormField();
      return false;
    } finally {
      if (generation === tenantGeneration) {
        tenantValidating.value = false;
      }
    }
  }

  /**
   * 执行登录预览同步
   * @param generation 预览发起时的租户世代号
   */
  async function runLoginPreview(generation: number): Promise<void> {
    if (generation !== tenantGeneration || !tenantValidated.value || !tenantStore.tenantCode) {
      return;
    }
    const tenantCode = tenantStore.tenantCode.trim();
    const userName = options.getUserName().trim();
    if (!tenantCode || !userName || !isValidLoginUserName(userName)) {
      return;
    }
    const previewKey = buildPreviewKey(tenantCode, userName);
    if (previewKey !== lastPreviewKey) {
      localeStore.clearLoginLocaleUserOverride();
    }
    const result = await userStore.syncLoginPreviewAsync(tenantCode, userName, previewKey).catch(() => null);
    if (!result || generation !== tenantGeneration) {
      return;
    }
    lastPreviewKey = previewKey;
  }

  /**
   * 调度或立即执行登录预览
   * @param immediate 是否立即执行
   */
  function scheduleLoginPreview(immediate: boolean): void {
    if (!tenantValidated.value || !tenantStore.tenantCode) {
      return;
    }
    const userName = options.getUserName().trim();
    if (!userName || !isValidLoginUserName(userName)) {
      return;
    }
    const generation = tenantGeneration;
    cancelPreviewDebounce();
    if (immediate) {
      void runLoginPreview(generation);
      return;
    }
    previewDebounceTimer = setTimeout(() => {
      previewDebounceTimer = null;
      void runLoginPreview(generation);
    }, TAKT_LOGIN_PREVIEW_DEBOUNCE_MS);
  }

  /**
   * 立即提交租户校验（失焦 / 回车 / 登录前）
   * @returns 校验通过为 true
   */
  async function commitTenantAsync(): Promise<boolean> {
    cancelTenantDebounce();
    const raw = options.getTenantCode();
    const code = normalizeTenantCode(raw);
    if (code !== raw) {
      options.setTenantCode(code);
      await showTenantFieldValidation();
      return false;
    }
    if (code.length !== TAKT_TENANT_CODE_LENGTH || !isValidTenantCode(code)) {
      invalidateTenantContext();
      await showTenantFieldValidation();
      return false;
    }
    if (tenantValidated.value && tenantStore.tenantCode === code) {
      return true;
    }
    return remoteValidateTenant(code, tenantGeneration);
  }

  /**
   * 租户输入变更：规范化 → 作废旧状态 → 输满 3 位则防抖远程校验
   */
  function onTenantInputChange(): void {
    const raw = options.getTenantCode();
    const code = normalizeTenantCode(raw);
    if (code !== raw) {
      options.setTenantCode(code);
      return;
    }
    if (code.length !== TAKT_TENANT_CODE_LENGTH || !isValidTenantCode(code)) {
      invalidateTenantContext();
      if (code.length > 0) {
        void showTenantFieldValidation();
      }
      return;
    }
    if (tenantValidated.value && tenantStore.tenantCode === code) {
      return;
    }
    invalidateTenantContext();
    const generation = tenantGeneration;
    tenantDebounceTimer = setTimeout(() => {
      tenantDebounceTimer = null;
      void remoteValidateTenant(code, generation);
    }, TAKT_LOGIN_TENANT_VALIDATE_DEBOUNCE_MS);
  }

  /**
   * 用户名输入变更：租户已校验通过时防抖拉取登录预览
   */
  function onUserNameInputChange(): void {
    const raw = options.getUserName();
    const userName = normalizeUserName(raw);
    if (userName !== raw) {
      options.setUserName(userName);
      return;
    }
    invalidatePreviewContext();
    if (!tenantValidated.value || !tenantStore.tenantCode) {
      return;
    }
    if (!isValidLoginUserName(userName)) {
      return;
    }
    scheduleLoginPreview(false);
  }

  /**
   * 立即提交登录预览（用户名失焦）
   */
  async function commitUserNamePreviewAsync(): Promise<void> {
    cancelPreviewDebounce();
    if (!tenantValidated.value || !tenantStore.tenantCode) {
      return;
    }
    const userName = normalizeUserName(options.getUserName());
    if (userName !== options.getUserName()) {
      options.setUserName(userName);
      return;
    }
    if (!isValidLoginUserName(userName)) {
      return;
    }
    await runLoginPreview(tenantGeneration);
    await options.formRef.value?.validateFields(['userName']).catch(() => undefined);
  }

  /**
   * 释放定时器与进行中的世代
   */
  function dispose(): void {
    tenantGeneration += 1;
    cancelTenantDebounce();
    cancelPreviewDebounce();
  }

  return {
    tenantValidated,
    tenantValidating,
    onTenantInputChange,
    onUserNameInputChange,
    commitTenantAsync,
    commitUserNamePreviewAsync,
    dispose,
  };
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/composables
// 文件名称：useTaktLoginCaptcha.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：登录验证码弹窗状态（拉取挑战、Slider/Behavior 面板、提交载荷）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { computed, nextTick, ref, watch, type Ref } from 'vue';
import { getSessionCaptcha } from '@/api/identity/auth.api';
import type { TaktCaptchaChallengeDto, TaktCaptchaSubmitPayload } from '@/types/identity/captcha.types';
import type { TaktCaptchaPanelExpose, UseTaktLoginCaptchaOptions } from '@/types/common.types';
import { TAKT_CAPTCHA_DISABLED_HINTS } from '@/utils/common';
import { isTaktCaptchaSliderType } from '@/utils/taktCaptchaType';

export type { TaktCaptchaPanelExpose, UseTaktLoginCaptchaOptions };

/**
 * 判断异常是否表示验证码功能未启用（可跳过弹窗直接提交业务）
 * @param {unknown} error 拉取挑战时的异常
 * @returns {boolean} 未启用时为 true
 */
export function isTaktCaptchaDisabledError(error: unknown): boolean {
  const message =
    error instanceof Error
      ? error.message
      : typeof error === 'string'
        ? error
        : '';
  if (!message) {
    return false;
  }
  return TAKT_CAPTCHA_DISABLED_HINTS.some((hint) => message.includes(hint));
}

/**
 * 探测当前是否需要展示登录会话验证码弹窗
 * @returns {Promise<boolean>} 需要验证码时为 true；未启用时为 false
 * @throws 网络或其他非「未启用」错误
 */
export async function probeSessionCaptchaRequiredAsync(): Promise<boolean> {
  try {
    await getSessionCaptcha();
    return true;
  } catch (error) {
    if (isTaktCaptchaDisabledError(error)) {
      return false;
    }
    throw error;
  }
}

/**
 * 登录页验证码弹窗逻辑（配合 takt-modal 与 Slider/Behavior 子组件）
 * @param modalOpen 弹窗是否打开
 * @param options 自动提交等选项
 * @returns 挑战数据、加载状态与确认/取消处理
 */
export function useTaktLoginCaptcha(
  modalOpen: Ref<boolean>,
  options?: UseTaktLoginCaptchaOptions,
) {
  const loading = ref(false);
  const submitting = ref(false);
  const challenge = ref<TaktCaptchaChallengeDto | null>(null);
  const canSubmit = ref(false);
  const panelRef = ref<TaktCaptchaPanelExpose | null>(null);
  /** 防止 can-submit 重复触发自动提交 */
  const autoConfirming = ref(false);
  let onVerifiedHandler: (() => void | Promise<void>) | undefined = options?.onVerified;
  let onCaptchaSkippedHandler: (() => void | Promise<void>) | undefined = options?.onCaptchaSkipped;

  const isSlider = computed(() => isTaktCaptchaSliderType(challenge.value?.captchaType));

  /**
   * 验证码未启用时静默跳过并执行业务回调
   * @param error 拉取挑战异常
   */
  async function handleCaptchaDisabledAsync(error: unknown): Promise<void> {
    if (!isTaktCaptchaDisabledError(error)) {
      throw error;
    }
    modalOpen.value = false;
    canSubmit.value = false;
    autoConfirming.value = false;
    if (onCaptchaSkippedHandler) {
      await onCaptchaSkippedHandler();
    }
  }

  /**
   * 加载验证码挑战
   */
  async function loadChallengeAsync(): Promise<void> {
    loading.value = true;
    canSubmit.value = false;
    try {
      challenge.value = await getSessionCaptcha();
    } catch (error) {
      await handleCaptchaDisabledAsync(error);
    } finally {
      loading.value = false;
    }
  }

  /**
   * 注册验证通过后的回调（可在 composable 初始化之后绑定）
   * @param handler 验证通过后执行的逻辑
   */
  function registerOnVerified(handler: () => void | Promise<void>): void {
    onVerifiedHandler = handler;
  }

  /**
   * 注册验证码未启用时的跳过回调（可在 composable 初始化之后绑定）
   * @param handler 跳过验证码后执行的逻辑
   */
  function registerOnCaptchaSkipped(handler: () => void | Promise<void>): void {
    onCaptchaSkippedHandler = handler;
  }

  /**
   * 验证通过后自动触发 onVerified
   */
  function scheduleAutoConfirmAsync(): void {
    if (!onVerifiedHandler || autoConfirming.value || !modalOpen.value || loading.value) {
      return;
    }

    autoConfirming.value = true;
    void nextTick(async () => {
      try {
        if (canSubmit.value && modalOpen.value && onVerifiedHandler) {
          await onVerifiedHandler();
        }
      } finally {
        autoConfirming.value = false;
      }
    });
  }

  /**
   * 子组件可提交状态变化
   * @param {boolean} value 是否可提交
   */
  function handleCanSubmitChange(value: boolean): void {
    canSubmit.value = value;
    if (value) {
      scheduleAutoConfirmAsync();
    }
  }

  /**
   * 确认验证码并返回提交载荷
   * @returns {TaktCaptchaSubmitPayload | null} 可提交时返回载荷，否则 null
   */
  function confirmCaptcha(): TaktCaptchaSubmitPayload | null {
    if (!challenge.value || !panelRef.value || !canSubmit.value) {
      return null;
    }

    submitting.value = true;
    const payload: TaktCaptchaSubmitPayload = {
      captchaId: challenge.value.captchaId,
      captchaCode: panelRef.value.buildCaptchaCode(),
    };
    submitting.value = false;
    modalOpen.value = false;
    return payload;
  }

  /**
   * 关闭弹窗并重置可提交状态
   */
  function cancelCaptcha(): void {
    modalOpen.value = false;
    canSubmit.value = false;
    autoConfirming.value = false;
  }

  watch(modalOpen, (visible) => {
    if (visible) {
      void loadChallengeAsync();
      return;
    }
    canSubmit.value = false;
    autoConfirming.value = false;
  });

  return {
    loading,
    submitting,
    challenge,
    canSubmit,
    panelRef,
    isSlider,
    loadChallengeAsync,
    handleCanSubmitChange,
    registerOnVerified,
    registerOnCaptchaSkipped,
    confirmCaptcha,
    cancelCaptcha,
  };
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/bootstrap
// 文件名称：taktLogoutFlow.ts
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：统一登出编排（服务端 signOut 防重入；空闲/手动/过期共用）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { signOutSession } from '@/api/identity/auth.api';
import router from '@/router';
import { useUserStore } from '@/stores/identity/useUserStore';
import type { NotificationType } from '@/types/event.types';
import {
  TAKT_ACCESS_TOKEN_STORAGE_KEY,
  TAKT_IDLE_LAST_ACTIVITY_STORAGE_KEY,
  TAKT_REFRESH_TOKEN_STORAGE_KEY,
  TAKT_TOKEN_EXPIRES_STORAGE_KEY,
} from '@/utils/common';

/** 登出后硬跳转登录页时，在登录页展示一次性提示（sessionStorage） */
export const TAKT_LOGOUT_FLASH_STORAGE_KEY = 'takt.logout.flash';

/** 多标签页持久化键（登出时清除，避免恢复无效动态路由） */
export const TAKT_TABS_STORAGE_KEY = 'takt-tabs';

/** 登出流程进行中（防止 idle / 401 并发重复清态） */
let logoutInProgress = false;

/**
 * 是否正在执行登出清态
 * @returns 登出进行中为 true
 */
export function isLogoutInProgress(): boolean {
  return logoutInProgress;
}

/**
 * 尽力清除服务端 Cookie 会话（失败不抛出；须 token 仍在时使用）
 * @returns {Promise<void>}
 */
export async function runServerSignOutIfLoggedInAsync(): Promise<void> {
  if (!useUserStore().isLoggedIn) {
    return;
  }

  try {
    await signOutSession();
  } catch {
    //  intentional logout：401/网络失败均不阻断前端清态
  }
}

/**
 * 在登出防重入守卫内执行动作
 * @param action 登出步骤（可 async）
 * @returns 已在登出中则 undefined，否则为 action 返回值
 */
export async function withLogoutInProgress<T>(
  action: () => T | Promise<T>
): Promise<T | undefined> {
  if (logoutInProgress) {
    return undefined;
  }

  logoutInProgress = true;
  try {
    return await action();
  } finally {
    logoutInProgress = false;
  }
}

/**
 * 强退/会话失效：仅清 localStorage + 整页跳转，避免 Pinia 清态触发 SPA 渲染错误
 * @param toastMessage 登录页 flash 提示
 * @param toastType 提示类型
 */
export function performHardLogoutRedirect(
  toastMessage?: string,
  toastType: NotificationType = 'warning',
): void {
  if (typeof window === 'undefined') {
    return;
  }

  if (toastMessage && typeof sessionStorage !== 'undefined') {
    sessionStorage.setItem(
      TAKT_LOGOUT_FLASH_STORAGE_KEY,
      JSON.stringify({ type: toastType, message: toastMessage }),
    );
  }

  localStorage.removeItem(TAKT_ACCESS_TOKEN_STORAGE_KEY);
  localStorage.removeItem(TAKT_REFRESH_TOKEN_STORAGE_KEY);
  localStorage.removeItem(TAKT_TOKEN_EXPIRES_STORAGE_KEY);
  localStorage.removeItem(TAKT_TABS_STORAGE_KEY);
  try {
    sessionStorage.removeItem(TAKT_IDLE_LAST_ACTIVITY_STORAGE_KEY);
  } catch {
    // sessionStorage 不可用时忽略
  }

  const loginHref = router.resolve({ name: 'Login' }).href;
  window.location.replace(loginHref);
}

/**
 * SignalR 强退：服务端已吊销会话，直接硬跳转（不经 EventBus，避免 payload 丢失与重复监听）
 * @param message 强退提示
 */
export async function executeForceLogoutAsync(message?: string): Promise<void> {
  await withLogoutInProgress(async () => {
    performHardLogoutRedirect(message, 'warning');
  });
}

/**
 * 空闲超时登出：异步尽力 signOut + 同步硬跳登录页（不 await、不依赖 logoutInProgress）
 * @param toastMessage 登录页 flash 提示
 */
export function executeIdleLogoutNow(toastMessage?: string): void {
  void runServerSignOutIfLoggedInAsync();
  performHardLogoutRedirect(toastMessage, 'warning');
}

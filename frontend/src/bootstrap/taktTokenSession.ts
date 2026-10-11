// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/bootstrap
// 文件名称：taktTokenSession.ts
// 创建时间：2026-07-15
// 创建人：Takt365(Cursor AI)
// 功能描述：访问令牌 wall-clock 巡检；到期前主动刷新，失败则自动会话过期登出（无需点击界面才触发）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { effectScope, watch } from 'vue';
import { storeToRefs } from 'pinia';
import { isLogoutInProgress } from '@/bootstrap/taktLogoutFlow';
import { useUserStore } from '@/stores/identity/useUserStore';
import { EventBus } from '@/utils/eventBus';
import { createLogger } from '@/utils/logger';
import {
  ensureValidAccessToken,
  isAccessTokenExpiringSoon,
  TOKEN_REFRESH_BUFFER_MS,
} from '@/utils/oauth';
import { translateLocaleMessage } from '@/utils/taktI18nMessage';

/** 令牌会话模块日志 */
const tokenSessionLogger = createLogger('takt-token-session');

/** 精确到期前刷新的 setTimeout 句柄 */
let tokenRefreshTimer: ReturnType<typeof setTimeout> | null = null;

/** wall-clock 巡检间隔句柄 */
let tokenWatchInterval: ReturnType<typeof setInterval> | null = null;

/** 是否正在执行刷新/过期处理（防重入） */
let isEnforcingToken = false;

/** 是否已注册可见性/恢复监听 */
let resumeListenersAttached = false;

/** 巡检间隔（毫秒） */
const TOKEN_WATCH_INTERVAL_MS = 15_000;

/**
 * 清除精确刷新计时器
 */
function clearTokenRefreshTimer(): void {
  if (tokenRefreshTimer !== null) {
    clearTimeout(tokenRefreshTimer);
    tokenRefreshTimer = null;
  }
}

/**
 * 停止 wall-clock 巡检
 */
function stopTokenWatchInterval(): void {
  if (tokenWatchInterval !== null) {
    clearInterval(tokenWatchInterval);
    tokenWatchInterval = null;
  }
}

/**
 * 启动 wall-clock 巡检
 */
function startTokenWatchInterval(): void {
  if (tokenWatchInterval !== null) {
    return;
  }
  tokenWatchInterval = setInterval(() => {
    void enforceAccessTokenPolicy();
  }, TOKEN_WATCH_INTERVAL_MS);
}

/**
 * 广播会话过期（由 takt-event-handlers 统一清态跳转）
 */
function emitTokenSessionExpired(): void {
  EventBus.emit('auth:session-expired', {
    message: translateLocaleMessage('layouts.page.session.expired'),
  });
}

/**
 * 访问令牌即将/已过期：主动刷新；失败则自动登出
 * @returns {Promise<void>}
 */
async function enforceAccessTokenPolicy(): Promise<void> {
  if (isEnforcingToken || isLogoutInProgress()) {
    return;
  }

  const userStore = useUserStore();
  if (!userStore.isLoggedIn || !userStore.token) {
    return;
  }

  if (!isAccessTokenExpiringSoon()) {
    return;
  }

  isEnforcingToken = true;
  try {
    const ok = await ensureValidAccessToken();
    if (ok) {
      scheduleTokenRefreshTimer();
      return;
    }

    if (!useUserStore().isLoggedIn || isLogoutInProgress()) {
      return;
    }

    tokenSessionLogger.warn('访问令牌刷新失败，自动会话过期登出', {
      action: 'token-expired-logout',
    });
    emitTokenSessionExpired();
  } finally {
    isEnforcingToken = false;
  }
}

/**
 * 按 tokenExpiresAt - buffer 调度下一次主动刷新
 */
function scheduleTokenRefreshTimer(): void {
  clearTokenRefreshTimer();

  const userStore = useUserStore();
  if (!userStore.isLoggedIn || !userStore.token) {
    return;
  }

  const expiresAt = userStore.tokenExpiresAt;
  if (!expiresAt || !Number.isFinite(expiresAt) || expiresAt <= 0) {
    return;
  }

  const dueAt = expiresAt - TOKEN_REFRESH_BUFFER_MS;
  const delayMs = dueAt - Date.now();

  if (delayMs <= 0) {
    void enforceAccessTokenPolicy();
    return;
  }

  tokenRefreshTimer = setTimeout(() => {
    void enforceAccessTokenPolicy();
  }, delayMs);
}

/**
 * 标签页恢复可见：立即按 wall-clock 校验令牌（缓解后台节流 / 休眠后计时器延迟）
 */
function onTokenSessionResume(): void {
  if (document.hidden) {
    return;
  }
  void enforceAccessTokenPolicy();
  scheduleTokenRefreshTimer();
}

/**
 * 注册恢复监听
 */
function attachTokenResumeListeners(): void {
  if (resumeListenersAttached) {
    return;
  }
  resumeListenersAttached = true;
  document.addEventListener('visibilitychange', onTokenSessionResume);
  window.addEventListener('focus', onTokenSessionResume);
  window.addEventListener('pageshow', onTokenSessionResume);
}

/**
 * 初始化访问令牌会话监控（须在 app.use(pinia) 之后；与空闲登出独立）
 */
export function initTaktTokenSession(): void {
  attachTokenResumeListeners();

  const scope = effectScope(true);
  scope.run(() => {
    const userStore = useUserStore();
    const { token, tokenExpiresAt } = storeToRefs(userStore);

    watch(
      [token, tokenExpiresAt],
      ([accessToken]) => {
        if (!accessToken) {
          clearTokenRefreshTimer();
          stopTokenWatchInterval();
          isEnforcingToken = false;
          return;
        }
        scheduleTokenRefreshTimer();
        startTokenWatchInterval();
        void enforceAccessTokenPolicy();
      },
      { immediate: true },
    );
  });

  EventBus.on('user:login', () => {
    scheduleTokenRefreshTimer();
    startTokenWatchInterval();
    void enforceAccessTokenPolicy();
  });

  tokenSessionLogger.info('访问令牌自动刷新/过期巡检已启用', {
    action: 'init',
    refreshBufferMs: TOKEN_REFRESH_BUFFER_MS,
    watchIntervalMs: TOKEN_WATCH_INTERVAL_MS,
  });
}

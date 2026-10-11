// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/bootstrap
// 文件名称：taktIdleSession.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：空闲超时自动登出；预警期非阻断提示，有操作无感续期，无操作到点硬跳登录页
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { effectScope, watch } from 'vue';
import { storeToRefs } from 'pinia';
import { notification } from 'ant-design-vue';
import {
  getAuthIdleTimeoutMs,
  getAuthIdleWarningMinutes,
  getAuthIdleWarningMs,
  isAuthIdleLogoutEnabled,
} from '@/config/authIdle';
import { executeIdleLogoutNow } from '@/bootstrap/taktLogoutFlow';
import { useUserStore } from '@/stores/identity/useUserStore';
import {
  TAKT_IDLE_ACTIVITY_EVENTS,
  TAKT_IDLE_ACTIVITY_THROTTLE_MS,
  TAKT_IDLE_LAST_ACTIVITY_STORAGE_KEY,
} from '@/utils/common';
import { translateLocaleMessage } from '@/utils/taktI18nMessage';
import { EventBus } from '@/utils/eventBus';
import { createLogger } from '@/utils/logger';

/** 空闲会话模块日志实例 */
const idleSessionLogger = createLogger('takt-idle-session');

/** 空闲超时 / 预警 setTimeout 句柄 */
let idleTimer: ReturnType<typeof setTimeout> | null = null;

/** 预警通知是否已展示（本轮空闲周期内只提示一次） */
let idleWarningVisible = false;

/** 预警通知 key（用于关闭） */
const IDLE_WARNING_NOTIFICATION_KEY = 'takt-idle-session-warning';

/** 内存中的上次活动时间（每次巡检以 sessionStorage 为准同步） */
let lastActivityAt = 0;

/** 是否正在执行空闲超时登出，防止重入 */
let isHandlingIdle = false;

/** 是否已向 window/document 注册活动与可见性监听 */
let listenersAttached = false;

/** 空闲 wall-clock 巡检句柄 */
let idleWatchInterval: ReturnType<typeof setInterval> | null = null;

/** 巡检间隔（毫秒） */
const IDLE_WATCH_INTERVAL_MS = 5_000;

/**
 * 从 sessionStorage 读取上次活动时间
 * @returns {number} 毫秒时间戳；无效时为 0
 */
function readPersistedLastActivityAt(): number {
  if (typeof sessionStorage === 'undefined') {
    return 0;
  }
  try {
    const raw = sessionStorage.getItem(TAKT_IDLE_LAST_ACTIVITY_STORAGE_KEY);
    if (!raw) {
      return 0;
    }
    const value = Number(raw);
    return Number.isFinite(value) && value > 0 ? value : 0;
  } catch {
    return 0;
  }
}

/**
 * 写入上次活动时间（内存 + sessionStorage）
 * @param at 毫秒时间戳
 */
function writeLastActivityAt(at: number): void {
  lastActivityAt = at;
  if (typeof sessionStorage === 'undefined') {
    return;
  }
  try {
    sessionStorage.setItem(TAKT_IDLE_LAST_ACTIVITY_STORAGE_KEY, String(at));
  } catch {
    // 配额或隐私模式：仅用内存时钟
  }
}

/**
 * 清除持久化活动时钟
 */
function clearPersistedLastActivityAt(): void {
  lastActivityAt = 0;
  if (typeof sessionStorage === 'undefined') {
    return;
  }
  try {
    sessionStorage.removeItem(TAKT_IDLE_LAST_ACTIVITY_STORAGE_KEY);
  } catch {
    // ignore
  }
}

/**
 * 以 sessionStorage 为权威源同步内存时钟
 */
function syncLastActivityFromStorage(): void {
  const persisted = readPersistedLastActivityAt();
  if (persisted > 0) {
    lastActivityAt = persisted;
    return;
  }
  if (lastActivityAt <= 0) {
    writeLastActivityAt(Date.now());
  }
}

/**
 * 绝对登出截止时间（上次活动 + 总超时）
 * @returns {number} 毫秒时间戳
 */
function getIdleDeadlineAt(): number {
  const timeoutMs = getAuthIdleTimeoutMs();
  if (timeoutMs <= 0) {
    return Number.POSITIVE_INFINITY;
  }
  syncLastActivityFromStorage();
  return lastActivityAt + timeoutMs;
}

/**
 * 自上次真实活动以来经过的毫秒数
 */
function getIdleElapsedMs(): number {
  syncLastActivityFromStorage();
  if (lastActivityAt <= 0) {
    return 0;
  }
  return Date.now() - lastActivityAt;
}

/**
 * 距离空闲总超时还剩多少毫秒（≤0 表示应立刻登出）
 */
function getIdleRemainingMs(): number {
  const timeoutMs = getAuthIdleTimeoutMs();
  if (timeoutMs <= 0) {
    return Number.POSITIVE_INFINITY;
  }
  return getIdleDeadlineAt() - Date.now();
}

/**
 * 清除空闲计时器
 */
function clearIdleTimer(): void {
  if (idleTimer !== null) {
    clearTimeout(idleTimer);
    idleTimer = null;
  }
}

/**
 * 关闭预警通知（不续期、不登出）
 */
function dismissIdleWarning(): void {
  if (!idleWarningVisible) {
    return;
  }
  idleWarningVisible = false;
  notification.close(IDLE_WARNING_NOTIFICATION_KEY);
}

/**
 * 自上次真实活动以来经过的毫秒数对应的预警展示
 * @param warningMinutes 文案分钟数
 */
function showIdleWarningNotification(warningMinutes: number): void {
  if (idleWarningVisible || isHandlingIdle) {
    return;
  }

  const userStore = useUserStore();
  if (!userStore.isLoggedIn) {
    return;
  }

  const minutes = Math.max(1, warningMinutes);
  idleWarningVisible = true;

  notification.warning({
    key: IDLE_WARNING_NOTIFICATION_KEY,
    message: translateLocaleMessage('layouts.page.session.title'),
    description: translateLocaleMessage('layouts.page.session.content', { minutes }),
    duration: 0,
    placement: 'topRight',
  });

  idleSessionLogger.info('已展示空闲预警（非阻断）', {
    action: 'idle-warning',
    warningMinutes: minutes,
    remainingSec: Math.round(getIdleRemainingMs() / 1000),
  });
}

/**
 * 按绝对截止时间校验预警 / 登出
 */
function enforceIdlePolicyByWallClock(): void {
  const userStore = useUserStore();
  if (!userStore.isLoggedIn || isHandlingIdle) {
    return;
  }

  const timeoutMs = getAuthIdleTimeoutMs();
  if (timeoutMs <= 0) {
    return;
  }

  const remainingMs = getIdleRemainingMs();
  const remainingSec = Math.round(remainingMs / 1000);

  if (remainingSec <= 60 || remainingSec % 60 === 0) {
    idleSessionLogger.debug('空闲巡检', {
      action: 'idle-watch',
      remainingSec,
      elapsedSec: Math.round(getIdleElapsedMs() / 1000),
    });
  }

  if (remainingMs <= 0) {
    handleIdleTimeout();
    return;
  }

  const warningMs = getAuthIdleWarningMs();
  if (warningMs > 0 && !idleWarningVisible) {
    const elapsedMs = getIdleElapsedMs();
    if (elapsedMs >= timeoutMs - warningMs) {
      const warningMinutes = Math.max(1, Math.ceil(remainingMs / 60_000));
      showIdleWarningNotification(warningMinutes);
    }
  }
}

/**
 * 按绝对截止时间调度（预警 → 登出）
 */
function scheduleIdleTimer(): void {
  const timeoutMs = getAuthIdleTimeoutMs();

  if (timeoutMs <= 0) {
    clearIdleTimer();
    dismissIdleWarning();
    return;
  }

  clearIdleTimer();

  const remainingMs = getIdleRemainingMs();
  if (remainingMs <= 0) {
    handleIdleTimeout();
    return;
  }

  const warningMs = getAuthIdleWarningMs();

  if (warningMs > 0) {
    const elapsedMs = getIdleElapsedMs();
    const untilWarningMs = timeoutMs - warningMs - elapsedMs;

    if (untilWarningMs <= 0) {
      if (!idleWarningVisible && !isHandlingIdle) {
        const warningMinutes = Math.max(1, Math.ceil(remainingMs / 60_000));
        showIdleWarningNotification(warningMinutes);
      }
      // 预警已进入：再挂一次到绝对截止时间的登出定时器
      idleTimer = setTimeout(() => {
        handleIdleTimeout();
      }, Math.max(1_000, getIdleDeadlineAt() - Date.now()));
      return;
    }

    idleTimer = setTimeout(() => {
      const msLeft = getIdleRemainingMs();
      if (msLeft <= 0) {
        handleIdleTimeout();
        return;
      }
      showIdleWarningNotification(Math.max(1, Math.ceil(msLeft / 60_000)));
      scheduleIdleTimer();
    }, untilWarningMs);
    return;
  }

  idleTimer = setTimeout(() => {
    handleIdleTimeout();
  }, remainingMs);
}

/**
 * 空闲超时：同步硬跳登录页
 */
function handleIdleTimeout(): void {
  if (isHandlingIdle) {
    return;
  }

  const userStore = useUserStore();
  if (!userStore.isLoggedIn) {
    clearIdleTimer();
    dismissIdleWarning();
    clearPersistedLastActivityAt();
    return;
  }

  isHandlingIdle = true;
  clearIdleTimer();
  dismissIdleWarning();
  stopIdleWatchInterval();

  idleSessionLogger.info('用户空闲超时，执行自动登出', {
    action: 'idle-timeout',
    elapsedSec: Math.round(getIdleElapsedMs() / 1000),
  });

  const message = translateLocaleMessage('layouts.page.session.idlelogout');
  clearPersistedLastActivityAt();
  executeIdleLogoutNow(message);

  setTimeout(() => {
    isHandlingIdle = false;
  }, 1_000);
}

/**
 * 有操作：无感续期（重置时钟、关掉预警、重新调度）
 */
function recordUserActivityAndReschedule(): void {
  const hadWarning = idleWarningVisible;
  writeLastActivityAt(Date.now());
  dismissIdleWarning();
  scheduleIdleTimer();
  if (hadWarning) {
    idleSessionLogger.info('预警期内检测到操作，已无感续期', { action: 'idle-silent-renew' });
  }
}

/**
 * 记录用户活动并重置空闲计时
 * @param event 可选 DOM 事件；非 isTrusted 不计入
 */
function onUserActivity(event?: Event): void {
  if (event && !event.isTrusted) {
    return;
  }

  const userStore = useUserStore();
  if (!userStore.isLoggedIn || isHandlingIdle) {
    return;
  }

  if (getIdleRemainingMs() <= 0) {
    handleIdleTimeout();
    return;
  }

  syncLastActivityFromStorage();
  const now = Date.now();
  if (lastActivityAt > 0 && now - lastActivityAt < TAKT_IDLE_ACTIVITY_THROTTLE_MS) {
    return;
  }

  recordUserActivityAndReschedule();
}

/**
 * 标签页重新可见 / 窗口聚焦 / pageshow：按绝对截止时间校验
 */
function onVisibilityOrFocusResume(): void {
  if (typeof document !== 'undefined' && document.hidden) {
    return;
  }

  const userStore = useUserStore();
  if (!userStore.isLoggedIn || isHandlingIdle) {
    return;
  }

  enforceIdlePolicyByWallClock();
  if (!isHandlingIdle) {
    scheduleIdleTimer();
  }
}

/**
 * 启动 wall-clock 巡检
 */
function startIdleWatchInterval(): void {
  if (idleWatchInterval !== null) {
    return;
  }

  idleWatchInterval = setInterval(() => {
    enforceIdlePolicyByWallClock();
  }, IDLE_WATCH_INTERVAL_MS);
}

/**
 * 停止 wall-clock 巡检
 */
function stopIdleWatchInterval(): void {
  if (idleWatchInterval !== null) {
    clearInterval(idleWatchInterval);
    idleWatchInterval = null;
  }
}

/**
 * 注册全局活动监听
 */
function attachActivityListeners(): void {
  if (listenersAttached) {
    return;
  }

  listenersAttached = true;

  TAKT_IDLE_ACTIVITY_EVENTS.forEach((eventName) => {
    window.addEventListener(eventName, onUserActivity, { passive: true, capture: true });
  });

  document.addEventListener('visibilitychange', onVisibilityOrFocusResume);
  window.addEventListener('focus', onVisibilityOrFocusResume);
  window.addEventListener('pageshow', onVisibilityOrFocusResume);
}

/**
 * 登录态开始：恢复或初始化活动时钟并调度
 */
function onSessionActive(): void {
  const persisted = readPersistedLastActivityAt();
  if (persisted > 0) {
    lastActivityAt = persisted;
  } else {
    writeLastActivityAt(Date.now());
  }
  dismissIdleWarning();
  scheduleIdleTimer();
  startIdleWatchInterval();
  enforceIdlePolicyByWallClock();
}

/**
 * 初始化空闲会话监控（须在 app.use(pinia) 之后调用）
 */
export function initTaktIdleSession(): void {
  if (!isAuthIdleLogoutEnabled()) {
    idleSessionLogger.info('空闲自动登出已禁用', { action: 'init-skip' });
    return;
  }

  attachActivityListeners();

  const scope = effectScope(true);
  scope.run(() => {
    const userStore = useUserStore();
    const { token } = storeToRefs(userStore);

    watch(
      token,
      (accessToken, previousToken) => {
        if (accessToken) {
          if (!previousToken) {
            onSessionActive();
            return;
          }
          syncLastActivityFromStorage();
          scheduleIdleTimer();
          startIdleWatchInterval();
          return;
        }

        clearIdleTimer();
        dismissIdleWarning();
        stopIdleWatchInterval();
        isHandlingIdle = false;
        clearPersistedLastActivityAt();
      },
      { immediate: true },
    );
  });

  EventBus.on('user:login', () => {
    writeLastActivityAt(Date.now());
    dismissIdleWarning();
    scheduleIdleTimer();
    startIdleWatchInterval();
  });

  idleSessionLogger.info('空闲自动登出已启用', {
    action: 'init',
    timeoutMinutes: getAuthIdleTimeoutMs() / 60_000,
    warningMinutes: getAuthIdleWarningMinutes(),
    watchIntervalMs: IDLE_WATCH_INTERVAL_MS,
  });
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/bootstrap
// 文件名称：taktEventHandlers.ts
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：全局事件总线订阅注册（连接 request 副作用与 Store / Router / UI）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { message, Modal } from 'ant-design-vue';
import router, { resetRouterDynamicRoutes } from '@/router';
import { useUserStore } from '@/stores/identity/useUserStore';
import { useTenantStore } from '@/stores/identity/useTenantStore';
import { useLocaleStore } from '@/stores/foundation/useLocaleStore';
import { useDictDataStore } from '@/stores/foundation/useDictDataStore';
import { useTranslationStore } from '@/stores/foundation/useTranslationStore';
import { useThemeStore } from '@/stores/common/useThemeStore';
import { useSettingStore } from '@/stores/common/useSettingStore';
import { useMenuStore } from '@/stores/identity/useMenuStore';
import { usePermissionStore } from '@/stores/identity/usePermissionStore';
import { useSignalRStore } from '@/stores/foundation/useSignalrStore';
import { useWorkflowTodoCountStore } from '@/stores/workflow/useTodoCountStore';
import { useHeaderNotificationStore } from '@/stores/navigation/useHeaderNotificationStore';
import { EventBus, type NotificationType } from '@/utils/eventBus';
import { translateLocaleMessage } from '@/utils/taktI18nMessage';
import { ensureTaktPaginationConfigAsync, resetTaktPaginationConfig } from '@/config/taktPagination';
import {
  executeIdleLogoutNow,
  isLogoutInProgress,
  performHardLogoutRedirect,
  runServerSignOutIfLoggedInAsync,
  TAKT_LOGOUT_FLASH_STORAGE_KEY,
  TAKT_TABS_STORAGE_KEY,
  withLogoutInProgress,
} from '@/bootstrap/taktLogoutFlow';

export { executeIdleLogoutNow };

export { TAKT_LOGOUT_FLASH_STORAGE_KEY };

/** 登出跳转选项 */
interface PerformLogoutOptions {
  /** 为 true 时使用 location.replace 整页跳转（空闲/过期场景，避免后台标签页 UI 不刷新） */
  hardRedirect?: boolean;
}

/**
 * 跳转登录页；硬跳转用于空闲/会话过期，避免后台标签页仅 router 切换但界面不绘制
 * @param hard 是否整页刷新
 */
function redirectToLoginPage(hard: boolean): void {
  if (typeof window === 'undefined') {
    return;
  }

  if (router.currentRoute.value.path === '/login' && !hard) {
    return;
  }

  if (hard || document.hidden) {
    const loginHref = router.resolve({ name: 'Login' }).href;
    window.location.replace(loginHref);
    return;
  }

  void router.replace({ name: 'Login' }).catch(() => {
    window.location.replace('/login');
  });
}

/**
 * 执行登出清理并跳转登录页
 * @description 重置用户/租户/字典/翻译/菜单/权限/SignalR 状态；已在 /login 时不重复 push
 * @param {string} [toastMessage] 可选提示（Ant Design Message）；hardRedirect 时写入 sessionStorage 供登录页展示
 * @param {NotificationType} [toastType='error'] 提示类型；空闲登出用 warning
 * @param {PerformLogoutOptions} [options] 跳转选项
 * @returns {void}
 */
function performLogout(
  toastMessage?: string,
  toastType: NotificationType = 'error',
  options?: PerformLogoutOptions,
): void {
  if (options?.hardRedirect === true) {
    performHardLogoutRedirect(toastMessage, toastType);
    return;
  }

  /** 用户身份与令牌 Store */
  const userStore = useUserStore();
  /** 当前租户/公司上下文 Store */
  const tenantStore = useTenantStore();

  Modal.destroyAll();

  // 清除本地 token 与用户资料
  userStore.logout();
  // 清除租户/公司选择与请求头上下文
  tenantStore.clearTenant();
  resetTaktPaginationConfig();

  // 重置依赖登录/租户的全局缓存（字典、翻译、菜单、权限）
  // 清空字典缓存，避免下一用户看到旧租户数据
  useDictDataStore().resetDictData();
  // 清空动态翻译缓存
  useTranslationStore().resetTranslationMessages();
  // 清空侧栏菜单缓存
  useMenuStore().resetMenuList();
  // 清空按钮/路由权限缓存
  usePermissionStore().resetPermissions();

  // 断开实时连接并清空 Hub 状态，防止下一用户复用旧连接
  // 异步断开 SignalR，失败不阻断登出
  void useSignalRStore().disconnectSignalRAsync().catch(() => undefined);
  // 重置 Hub 连接状态与订阅
  useSignalRStore().resetSignalRState();
  // 清空顶栏通知中心
  useHeaderNotificationStore().resetHeaderNotifications();

  resetRouterDynamicRoutes();
  localStorage.removeItem(TAKT_TABS_STORAGE_KEY);

  redirectToLoginPage(false);

  if (toastMessage) {
    showAntdMessage(toastType, toastMessage);
  }
}

/**
 * 将 notification:show 映射到 Ant Design Vue Message
 * @param {NotificationType} type 通知类型
 * @param {string} content 主文案
 * @param {string} [description] 副文案；error 类型时与主文案拼接展示
 * @returns {void}
 */
function showAntdMessage(type: NotificationType, content: string, description?: string): void {
  /** Message 自动关闭时长（秒） */
  const duration = 3;
  switch (type) {
    case 'success':
      // 成功提示
      message.success(content, duration);
      break;
    case 'warning':
      // 警告提示
      message.warning(content, duration);
      break;
    case 'info':
      // 信息提示
      message.info(content, duration);
      break;
    case 'error':
    default:
      // error 与未知类型统一走 error 样式；有 description 时拼接副文案
      message.error(description ? `${content} — ${description}` : content, duration);
      break;
  }
}

/**
 * 空闲超时登出（兼容旧调用方）
 * @param message 提示文案；缺省 layouts.page.session.idlelogout
 * @returns {Promise<void>}
 */
export async function executeIdleLogoutAsync(message?: string): Promise<void> {
  const logoutMessage = message ?? translateLocaleMessage('layouts.page.session.idlelogout');
  executeIdleLogoutNow(logoutMessage);
}

/**
 * 主动登出（手动 / EventBus user:logout）：先服务端 signOut，再清前端
 * @param payload 登出选项
 * @returns {Promise<void>}
 */
export async function executeUserLogoutAsync(
  payload?: import('@/types/event.types').Events['user:logout'],
): Promise<void> {
  if (isLogoutInProgress()) {
    return;
  }

  EventBus.emit('user:logout', payload);

  const hardRedirect = payload?.hardRedirect === true;
  if (!hardRedirect) {
    await runServerSignOutIfLoggedInAsync();
  }

  await withLogoutInProgress(async () => {
    const toastMessage = payload?.message;
    performLogout(
      toastMessage,
      toastMessage ? 'warning' : 'error',
      hardRedirect ? { hardRedirect: true } : undefined,
    );
  });
}

/**
 * 注册全局事件订阅（在 createApp 且 app.use(pinia)、app.use(router) 之后调用一次）
 * @description 订阅 auth:session-expired、auth:idle-timeout、notification:show、
 *   user:login、menu:refresh、tenant:change、company:change、theme:change、locale:change
 * @returns {void}
 */
export function registerTaktEventHandlers(): void {
  // 401 / 业务未授权：request 拦截器仅发事件，此处统一清状态并跳转
  EventBus.on('auth:session-expired', (payload) => {
    if (isLogoutInProgress()) {
      return;
    }

    const logoutMessage = payload?.message ?? translateLocaleMessage('layouts.page.session.expired');
    const userStore = useUserStore();
    if (!userStore.isLoggedIn && router.currentRoute.value.path === '/login') {
      if (logoutMessage) {
        message.error(logoutMessage);
      }
      return;
    }

    void (async () => {
      if (isLogoutInProgress()) {
        return;
      }
      await runServerSignOutIfLoggedInAsync();
      await withLogoutInProgress(async () => {
        performLogout(logoutMessage, 'error', { hardRedirect: true });
      });
    })();
  });

  EventBus.on('auth:idle-timeout', (payload) => {
    const logoutMessage = payload?.message ?? translateLocaleMessage('layouts.page.session.idlelogout');
    executeIdleLogoutNow(logoutMessage);
  });

  // 全局 Toast：request / Store 等非 UI 层通过 EventBus 触发
  EventBus.on('notification:show', ({ type, message: content, description, silent }) => {
    if (!silent) {
      showAntdMessage(type, content, description);
    }
  });

  // 登录成功：预热菜单、语言选项、字典、翻译与 SignalR
  EventBus.on('user:login', () => {
    // 触发菜单重新加载
    EventBus.emit('menu:refresh', undefined);
    // 加载可选语言列表
    void useLocaleStore().loadCultureOptionsAsync().catch(() => undefined);
    // 预加载全量字典
    void useDictDataStore().loadAllDictDataAsync();
    // 加载动态翻译
    void useTranslationStore().loadTranslationMessagesAsync();
    // 从落库拉取未读至通知中心（不依赖 SignalR）
    void useHeaderNotificationStore().hydratePersistedUnreadAsync().catch(() => undefined);
    // 建立 SignalR 连接（连接成功后 signalr store 会 HTTP 补拉待办数量）
    void useSignalRStore().connectSignalRAsync().catch(() => undefined);
    // 首屏 HTTP 拉取待办数量（不依赖 SignalR 是否已连上）
    void useWorkflowTodoCountStore().refreshTodoCountAsync().catch(() => undefined);
  });

  // 菜单刷新：force=false 保留折叠态等 UI 状态
  EventBus.on('menu:refresh', () => {
    // 非强制刷新侧栏菜单
    void useMenuStore().loadMenuListAsync(false).catch(() => undefined);
  });

  // 租户切换：翻译与菜单按新租户重建，并通知各列表页刷新
  EventBus.on('tenant:change', () => {
    resetTaktPaginationConfig();
    void ensureTaktPaginationConfigAsync().catch(() => undefined);
    // 清空旧租户翻译
    useTranslationStore().resetTranslationMessages();
    // 清空旧租户字典（CultureCode 过滤依赖 Accept-Language + 租户数据）
    useDictDataStore().resetDictData();
    // 按新租户重建菜单
    EventBus.emit('menu:refresh', undefined);
    // 通知各列表页刷新数据
    EventBus.emit('table:refresh', {});
    // 加载新租户翻译
    void useTranslationStore().loadTranslationMessagesAsync();
    // 预加载新租户字典
    void useDictDataStore().loadAllDictDataAsync();
    // 强制刷新菜单（租户级权限变化）
    void useMenuStore().loadMenuListAsync(true).catch(() => undefined);
  });

  // 公司切换：除租户级资源外，还需重载用户资料、字典与 SignalR 连接
  EventBus.on('company:change', () => {
    // 清空旧公司上下文下的翻译
    useTranslationStore().resetTranslationMessages();
    // 按新公司重建菜单
    EventBus.emit('menu:refresh', undefined);
    // 通知各列表页刷新数据
    EventBus.emit('table:refresh', {});
    // 强制刷新用户资料（公司级数据权限）
    void useUserStore().loadUserProfile(true).catch(() => undefined);
    // 加载新公司翻译
    void useTranslationStore().loadTranslationMessagesAsync();
    // 强制刷新菜单
    void useMenuStore().loadMenuListAsync(true).catch(() => undefined);
    // 按当前公司与租户同步当日假日主题色
    void useUserStore().loadHolidayThemeForCurrentSession().catch(() => undefined);
    // 重载字典（Accept-Language 下 eo + 区域项；公司切换后强制刷新）
    void useDictDataStore().reloadAllDictDataAsync();
    // 按新公司上下文重连 SignalR
    void useSignalRStore().reconnectSignalRAsync().catch(() => undefined);
  });

  // 主题同步：避免与 Store 当前值相同时重复写入 DOM
  EventBus.on('theme:change', ({ theme }) => {
    /** 主题模式 Store（light / dark） */
    const themeStore = useThemeStore();
    // 与当前 Store 值相同则跳过，减少 DOM 写入
    if (themeStore.mode !== theme) {
      // 写入主题并同步 DOM / localStorage
      themeStore.setThemeMode(theme);
    }
  });

  // 语言切换：重载字典 + 偏好落库（locale ↔ setting 经 EventBus 解耦，禁止 store 互引）
  EventBus.on('locale:change', () => {
    void useDictDataStore().reloadAllDictDataAsync();
    useSettingStore().scheduleLocaleSync();
  });

  // 页签重新可见时校验 SignalR（后端重启后 WebSocket 常已断开但 Store 仍显示已连接）
  if (typeof document !== 'undefined') {
    document.addEventListener('visibilitychange', () => {
      if (document.visibilityState !== 'visible') {
        return;
      }
      const userStore = useUserStore();
      if (!userStore.isLoggedIn) {
        if (router.currentRoute.value.path !== '/login') {
          window.location.replace(router.resolve({ name: 'Login' }).href);
        }
        return;
      }
      void useSignalRStore().connectSignalRAsync().catch(() => undefined);
    });
  }
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/router
// 文件名称：index.ts
// 创建时间：2026-05-22
// 创建人：Takt365(Cursor AI)
// 功能描述：路由配置、菜单动态路由注册与守卫（登录态 + RBAC）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { createRouter, createWebHistory } from 'vue-router';
import type { RouteRecordRaw } from 'vue-router';
import { useUserStore } from '@/stores/identity/useUserStore';
import { useTranslationStore } from '@/stores/foundation/useTranslationStore';
import { useMenuStore } from '@/stores/identity/useMenuStore';
import { usePermissionStore } from '@/stores/identity/usePermissionStore';
import { useSignalRStore } from '@/stores/foundation/useSignalrStore';
import { useHeaderNotificationStore } from '@/stores/navigation/useHeaderNotificationStore';
import { useTenantStore } from '@/stores/identity/useTenantStore';
import i18n from '@/locales';
import {
  LAYOUT_ROUTE_NAME,
  NOT_FOUND_ROUTE_NAME,
  registerDynamicRoutes,
  resetDynamicRoutes,
  resolveDefaultMenuPath,
} from '@/router/menuRoutes';
import { translateLocaleMessage } from '@/utils/taktI18nMessage';
import { ensureTaktPaginationConfigAsync } from '@/config/taktPagination';
import { useLocaleStore } from '@/stores/foundation/useLocaleStore';

/** 静态常量路由（业务页由菜单动态注册；404 在动态路由之后追加） */
const constantRoutes: RouteRecordRaw[] = [
  {
    path: '/login',
    name: 'Login',
    component: () => import('@/views/login/index.vue'),
    meta: { titleKey: 'login.page.login.title', requiresAuth: false },
  },
  {
    path: '/meeting-notification/confirm',
    name: 'MeetingNotificationConfirm',
    component: () => import('@/views/routine/meeting-center/meeting-notification/confirm.vue'),
    meta: {
      titleKey: 'routine.meeting-center.meeting-notification.page.confirm.title',
      requiresAuth: false,
    },
  },
  {
    path: '/auth/callback',
    name: 'OAuthCallback',
    component: () => import('@/views/login/components/Callback.vue'),
    meta: { titleKey: 'login.page.callback.title', requiresAuth: false },
  },
  {
    path: '/',
    name: LAYOUT_ROUTE_NAME,
    component: () => import('@/layouts/index.vue'),
    meta: { requiresAuth: true },
  },
  {
    path: '/403',
    name: 'Forbidden',
    component: () => import('@/views/error/403.vue'),
    meta: { titleKey: 'error.page.title403', requiresAuth: true },
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes: constantRoutes,
});

/**
 * 加载菜单并注册动态路由
 * @param {boolean} [force=false] 是否强制重建
 */
export async function ensureMenuAndRoutesLoaded(force = false): Promise<void> {
  const menuStore = useMenuStore();
  const userStore = useUserStore();

  if (!userStore.isLoggedIn) {
    return;
  }

  if (!menuStore.isLoaded || force) {
    const hasMenusInUserStore = (userStore.menus?.length ?? 0) > 0;
    if (!force && hasMenusInUserStore) {
      menuStore.syncMenusFromUserProfile();
    } else {
      await menuStore.loadMenuListAsync(force);
    }
  }

  if (!force && menuStore.isDynamicRoutesRegistered) {
    return;
  }

  registerDynamicRoutes(router, menuStore.menuList);
  menuStore.markDynamicRoutesRegistered();
}

/**
 * 登出或租户切换时清除动态路由
 */
export function resetRouterDynamicRoutes(): void {
  resetDynamicRoutes(router);
  useMenuStore().clearDynamicRoutesRegistered();
}

router.beforeEach(async (to) => {
  const userStore = useUserStore();

  const titleKey = typeof to.meta.titleKey === 'string' ? to.meta.titleKey : undefined;
  const appTitle = translateLocaleMessage('common.page.app.title');
  document.title = titleKey ? `${translateLocaleMessage(titleKey)} - ${appTitle}` : appTitle;

  if (to.meta.requiresAuth === false) {
    if (to.name === 'Login' || to.path === '/login') {
      useLocaleStore().resetLocaleForLoginPage();
    }

    return true;
  }

  if (!userStore.isLoggedIn) {
    return { name: 'Login', query: { redirect: to.fullPath } };
  }

  useTenantStore().restoreTenantCodeFromStorage();
  await ensureTaktPaginationConfigAsync();

  try {
    await userStore.loadUserProfile();
    await useTranslationStore().loadTranslationMessagesAsync(String(i18n.global.locale.value));
    await ensureMenuAndRoutesLoaded();
    void useHeaderNotificationStore().hydratePersistedUnreadAsync().catch(() => undefined);
    void useSignalRStore().connectSignalRAsync().catch(() => undefined);
  } catch {
    userStore.logout();
    resetRouterDynamicRoutes();
    return { name: 'Login', query: { redirect: to.fullPath } };
  }

  const menuStore = useMenuStore();
  const defaultPath = resolveDefaultMenuPath(menuStore.menuList);

  if (to.path === '/' || to.name === LAYOUT_ROUTE_NAME) {
    if (defaultPath) {
      return { path: defaultPath, replace: true };
    }
  }

  if ((to.path === '/dashboard' || to.path === '/dashboard/') && defaultPath) {
    return { path: defaultPath, replace: true };
  }

  if (to.matched.length === 0 && to.name !== NOT_FOUND_ROUTE_NAME && to.name !== 'Forbidden') {
    return { ...to, replace: true };
  }

  const permissionStore = usePermissionStore();
  const permission = typeof to.meta.permission === 'string' ? to.meta.permission : undefined;
  const skipRoutePathCheck = to.meta.skipRoutePathCheck === true;
  const allowed = permissionStore.canActivate({
    permission,
    path: to.path,
    skipRoutePathCheck,
  });

  if (!allowed && to.name !== 'Forbidden') {
    return { name: 'Forbidden' };
  }

  return true;
});

export default router;

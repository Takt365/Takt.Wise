// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/stores/identity
// 文件名称：useMenuStore.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：菜单树状态管理（同步 /me 菜单、Ant Design 侧栏项）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { defineStore } from 'pinia';
import { ref, computed, h, type Component } from 'vue';
import type { MenuProps } from 'ant-design-vue';
import i18n from '@/locales';
import type { MenuTree } from '@/types/identity/menu.types';
import { useUserStore } from '@/stores/identity/useUserStore';
import { usePermissionStore } from '@/stores/identity/usePermissionStore';
import { useTranslationStore } from '@/stores/foundation/useTranslationStore';
import { EventBus } from '@/utils/eventBus';
import { translateLocaleMessage } from '@/utils/taktI18nMessage';
import {
  clearRemixIconCache,
  collectMenuIconNames,
  getRemixIconComponent,
  preloadRemixIcons,
} from '@/utils/taktRemixIcon';
import {
  TaktMenuType,
  TAKT_MENU_STATUS_ENABLED,
  TAKT_MENU_VISIBLE_YES,
  TAKT_REMIX_ICON_CLASS,
} from '@/utils/common';

/** 菜单加载单飞 Promise */
let menuLoadPromise: Promise<void> | null = null;

/** Remix 图标预加载完成后递增，驱动 menuItems 重算 icon 槽 */
const remixIconRevision = ref(0);

/** 菜单图标预加载单飞 Promise */
let menuIconPreloadPromise: Promise<void> | null = null;

/** 叶子菜单项（快捷入口、顶栏搜索等） */
export interface LeafMenuItem {
  path: string;
  title: string;
  menuCode: string;
  icon?: Component;
}

/**
 * 获取菜单展示文案
 * @param menu 菜单节点
 */
function getMenuLabel(menu: MenuTree): string {
  if (menu.i18nKey) {
    return translateLocaleMessage(menu.i18nKey);
  }

  return menu.menuName;
}

/**
 * 菜单是否可用于侧栏展示
 * @param menu 菜单节点
 */
function isNavigableMenu(menu: MenuTree): boolean {
  if (menu.menuType === TaktMenuType.Button) {
    return false;
  }

  if (menu.menuStatus !== undefined && menu.menuStatus !== TAKT_MENU_STATUS_ENABLED) {
    return false;
  }

  if (menu.isVisible !== undefined && menu.isVisible !== TAKT_MENU_VISIBLE_YES) {
    return false;
  }

  return true;
}

/**
 * 预加载菜单树中的 Remix 图标并刷新侧栏 items
 * @param menus 菜单树
 */
async function preloadMenuTreeIcons(menus: MenuTree[]): Promise<void> {
  const iconNames = collectMenuIconNames(menus);
  if (iconNames.length === 0) {
    return;
  }

  if (menuIconPreloadPromise) {
    await menuIconPreloadPromise;
    return;
  }

  menuIconPreloadPromise = (async () => {
    try {
      await preloadRemixIcons(iconNames);
      remixIconRevision.value += 1;
    } finally {
      menuIconPreloadPromise = null;
    }
  })();

  await menuIconPreloadPromise;
}

/**
 * 构建 Ant Design Menu 项图标渲染函数
 * @param menu 菜单节点
 */
function buildMenuItemIcon(menu: MenuTree): (() => ReturnType<typeof h>) | undefined {
  const iconComponent = getRemixIconComponent(menu.icon);
  if (!iconComponent) {
    return undefined;
  }

  return () => h(iconComponent, { class: TAKT_REMIX_ICON_CLASS });
}

/**
 * 菜单状态管理
 */
export const useMenuStore = defineStore('menu', () => {
  const translationStore = useTranslationStore();
  const menuList = ref<MenuTree[]>([]);
  const loading = ref(false);
  const loaded = ref(false);
  const dynamicRoutesRegistered = ref(false);

  /**
   * 是否已加载菜单
   */
  const isLoaded = computed(() => loaded.value);

  /**
   * 动态路由是否已注册到 Vue Router
   */
  const isDynamicRoutesRegistered = computed(() => dynamicRoutesRegistered.value);

  /**
   * 将菜单树转为 Ant Design Menu items
   * @param menus 菜单树
   */
  function formatMenuItems(menus: MenuTree[]): MenuProps['items'] {
    if (!menus.length) {
      return [];
    }

    return menus
      .filter(isNavigableMenu)
      .sort((a, b) => (a.sortOrder ?? 0) - (b.sortOrder ?? 0))
      .map((menu) => {
        const children = menu.children?.length ? formatMenuItems(menu.children) : undefined;
        const routePath = menu.routePath?.startsWith('/') ? menu.routePath : `/${menu.routePath ?? ''}`;
        const key = menu.menuType === TaktMenuType.Menu && menu.routePath ? routePath : menu.menuCode;

        const item = {
          key,
          label: getMenuLabel(menu),
          title: getMenuLabel(menu),
          icon: buildMenuItemIcon(menu),
          children: undefined as MenuProps['items'],
        };

        if (children && children.length > 0) {
          item.children = children;
        }

        return item;
      });
  }

  /**
   * 侧栏菜单项（响应 i18n locale 变化）
   */
  const menuItems = computed(() => {
    void i18n.global.locale.value;
    void remixIconRevision.value;
    void translationStore.dynamicRevision;
    return formatMenuItems(menuList.value);
  });

  /**
   * 扁平化叶子菜单（routePath 可用于快捷入口）
   */
  const leafMenus = computed((): LeafMenuItem[] => {
    void i18n.global.locale.value;
    void remixIconRevision.value;
    void translationStore.dynamicRevision;

    const result: LeafMenuItem[] = [];

    const walk = (menus: MenuTree[]): void => {
      menus.filter(isNavigableMenu).forEach((menu) => {
        if (menu.menuType === TaktMenuType.Menu && menu.routePath) {
          result.push({
            path: menu.routePath.startsWith('/') ? menu.routePath : `/${menu.routePath}`,
            title: getMenuLabel(menu),
            menuCode: menu.menuCode,
            icon: getRemixIconComponent(menu.icon),
          });
        }

        if (menu.children?.length) {
          walk(menu.children);
        }
      });
    };

    walk(menuList.value);
    return result;
  });

  /**
   * 从用户 Store 已加载的菜单树同步（避免登录流程重复请求 /me）
   */
  function syncMenusFromUserProfile(): void {
    const menus = useUserStore().menus ?? [];
    if (menus.length === 0) {
      throw new Error(
        translateLocaleMessage('common.feedback.load.empty', {
          target: translateLocaleMessage('common.page.entity.menulist'),
        }),
      );
    }

    applyMenuList(menus);
  }

  /**
   * 从用户 Store 同步菜单与权限
   * @param menus 菜单树
   */
  function applyMenuList(menus: MenuTree[]): void {
    menuList.value = menus;
    loaded.value = true;

    void preloadMenuTreeIcons(menus);

    const userStore = useUserStore();
    const permissionStore = usePermissionStore();

    permissionStore.syncFromMenuTree(
      menus,
      userStore.permissions,
      userStore.routePaths,
      userStore.roles
    );
  }

  /**
   * 加载当前用户菜单（刷新 /me 并同步菜单、权限）
   * @param force 是否强制刷新
   */
  async function loadMenuListAsync(force = false): Promise<void> {
    const userStore = useUserStore();

    if (!userStore.isLoggedIn) {
      return;
    }

    if (loaded.value && !force) {
      return;
    }

    if (menuLoadPromise) {
      return menuLoadPromise;
    }

    loading.value = true;

    menuLoadPromise = (async () => {
      try {
        const hasMenusInUserStore = (userStore.menus?.length ?? 0) > 0;

        if (!force && hasMenusInUserStore) {
          syncMenusFromUserProfile();
        } else {
          await userStore.loadUserProfile(force);

          const menus = userStore.menus ?? [];
          if (menus.length === 0) {
            throw new Error(
        translateLocaleMessage('common.feedback.load.empty', {
          target: translateLocaleMessage('common.page.entity.menulist'),
        }),
      );
          }

          applyMenuList(menus);
        }
      } catch (error) {
        menuList.value = [];
        loaded.value = false;

        EventBus.emit('notification:show', {
          type: 'error',
          message:
            error instanceof Error
              ? error.message
              : translateLocaleMessage('common.feedback.load.failed', {
                  target: translateLocaleMessage('common.page.entity.menulist'),
                }),
        });

        throw error;
      } finally {
        loading.value = false;
        menuLoadPromise = null;
      }
    })();

    return menuLoadPromise;
  }

  /**
   * 重置菜单状态（登出等场景）
   */
  function resetMenuList(): void {
    menuList.value = [];
    loaded.value = false;
    loading.value = false;
    dynamicRoutesRegistered.value = false;
    menuLoadPromise = null;
    menuIconPreloadPromise = null;
    remixIconRevision.value = 0;
    clearRemixIconCache();
  }

  /**
   * 标记动态路由已注册
   */
  function markDynamicRoutesRegistered(): void {
    dynamicRoutesRegistered.value = true;
  }

  /**
   * 清除动态路由注册标记（登出或菜单刷新前调用）
   */
  function clearDynamicRoutesRegistered(): void {
    dynamicRoutesRegistered.value = false;
  }

  /**
   * 菜单刷新后强制下次导航重建动态路由
   */
  function invalidateDynamicRoutes(): void {
    dynamicRoutesRegistered.value = false;
  }

  return {
    menuList,
    loading,
    loaded,
    isLoaded,
    isDynamicRoutesRegistered,
    menuItems,
    leafMenus,
    formatMenuItems,
    loadMenuListAsync,
    syncMenusFromUserProfile,
    resetMenuList,
    markDynamicRoutesRegistered,
    clearDynamicRoutesRegistered,
    invalidateDynamicRoutes,
  };
});

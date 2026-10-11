// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/stores/identity
// 文件名称：usePermissionStore.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：RBAC 权限状态管理（功能码 + 路由路径，与菜单树联动）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { MenuTree } from '@/types/identity/menu.types';
import {
  canAccessRoute,
  canActivateRoute,
  hasAnyPermission,
  hasPermissionCode,
} from '@/utils/permission';

/**
 * 权限状态管理
 */
export const usePermissionStore = defineStore('permission', () => {
  const permissions = ref<string[]>([]);
  const routePaths = ref<string[]>([]);
  const roles = ref<string[]>([]);
  const loaded = ref(false);

  /**
   * 是否已加载权限上下文
   */
  const isLoaded = computed(() => loaded.value);

  /**
   * 从菜单树递归提取 permission 字段
   * @param menus 菜单树
   * @returns 权限码列表
   */
  function extractPermissionsFromMenuTree(menus: MenuTree[]): string[] {
    const result: string[] = [];

    const traverse = (menuList: MenuTree[]): void => {
      menuList.forEach((menu) => {
        const permission = menu.permission?.trim();

        if (permission) {
          result.push(permission);
        }

        if (menu.children?.length) {
          traverse(menu.children);
        }
      });
    };

    traverse(menus);
    return result;
  }

  /**
   * 合并菜单权限与用户权限并写入 Store
   * @param menuPermissions 从菜单树提取的权限
   * @param userPermissions 用户资料中的权限
   * @param userRoutePaths 可访问路由路径
   * @param userRoles 角色列表
   */
  function setPermissions(
    menuPermissions: string[],
    userPermissions: string[] = [],
    userRoutePaths: string[] = [],
    userRoles: string[] = []
  ): void {
    permissions.value = [...new Set([...userPermissions, ...menuPermissions])];
    routePaths.value = userRoutePaths;
    roles.value = userRoles;
    loaded.value = true;
  }

  /**
   * 根据菜单树与用户资料同步权限上下文
   * @param menus 菜单树
   * @param userPermissions 用户权限
   * @param userRoutePaths 路由路径
   * @param userRoles 角色
   */
  function syncFromMenuTree(
    menus: MenuTree[],
    userPermissions: string[] = [],
    userRoutePaths: string[] = [],
    userRoles: string[] = []
  ): void {
    const menuPermissions = extractPermissionsFromMenuTree(menus);
    setPermissions(menuPermissions, userPermissions, userRoutePaths, userRoles);
  }

  /**
   * 是否拥有指定功能权限码
   * @param permission 权限码
   */
  function hasPermission(permission: string): boolean {
    return hasPermissionCode(permissions.value, permission);
  }

  /**
   * 是否拥有任意一项权限
   * @param required 单个或多个权限码
   */
  function hasAny(required: string | string[]): boolean {
    return hasAnyPermission(permissions.value, required);
  }

  /**
   * 是否可访问指定路由路径
   * @param targetPath 目标路径
   */
  function canAccess(targetPath: string): boolean {
    return canAccessRoute(routePaths.value, targetPath);
  }

  /**
   * 路由守卫：是否允许进入目标路由
   * @param options 路由参数
   */
  function canActivate(options: {
    permission?: string;
    path: string;
    skipRoutePathCheck?: boolean;
  }): boolean {
    return canActivateRoute({
      permissions: permissions.value,
      routePaths: routePaths.value,
      permission: options.permission,
      path: options.path,
      skipRoutePathCheck: options.skipRoutePathCheck,
    });
  }

  /**
   * 重置权限上下文（登出等场景）
   */
  function resetPermissions(): void {
    permissions.value = [];
    routePaths.value = [];
    roles.value = [];
    loaded.value = false;
  }

  return {
    permissions,
    routePaths,
    roles,
    loaded,
    isLoaded,
    extractPermissionsFromMenuTree,
    setPermissions,
    syncFromMenuTree,
    hasPermission,
    hasAny,
    canAccess,
    canActivate,
    resetPermissions,
  };
});

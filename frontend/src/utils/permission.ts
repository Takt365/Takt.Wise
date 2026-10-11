// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：permission.ts
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：前端功能权限与路由访问判断
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 规范化路由路径（与后端 RoutePath 一致）
 * @param {string} path 原始路径
 * @returns {string} 规范化路径
 */
export function normalizeRoutePath(path: string): string {
  const trimmed = path.trim();
  if (!trimmed) {
    return '';
  }
  const withSlash = trimmed.startsWith('/') ? trimmed : `/${trimmed}`;
  return withSlash.replace(/\/+$/, '') || '/';
}

/**
 * 是否拥有指定功能权限码
 * @param {string[]} permissions 用户权限码列表
 * @param {string} permission 待校验权限码
 * @returns {boolean} 是否拥有
 */
export function hasPermissionCode(permissions: string[], permission: string): boolean {
  if (!permission) {
    return true;
  }
  return permissions.some((item) => item.toLowerCase() === permission.toLowerCase());
}

/**
 * 是否拥有权限列表中的任意一项
 * @param {string[]} permissions 用户权限码列表
 * @param {string | string[]} required 所需权限（单个或数组）
 * @returns {boolean} 是否拥有
 */
export function hasAnyPermission(permissions: string[], required: string | string[]): boolean {
  const requiredList = Array.isArray(required) ? required : [required];
  if (requiredList.length === 0) {
    return true;
  }
  return requiredList.some((code) => hasPermissionCode(permissions, code));
}

/**
 * 判断路由路径是否在可访问列表中（支持前缀匹配）
 * @param {string[]} routePaths 可访问路由列表
 * @param {string} targetPath 目标路径
 * @returns {boolean} 是否可访问
 */
export function canAccessRoute(routePaths: string[], targetPath: string): boolean {
  const normalizedTarget = normalizeRoutePath(targetPath);
  if (!normalizedTarget || normalizedTarget === '/') {
    return true;
  }

  return routePaths.some((route) => {
    const normalizedRoute = normalizeRoutePath(route);
    if (!normalizedRoute) {
      return false;
    }
    return (
      normalizedTarget === normalizedRoute ||
      normalizedTarget.startsWith(`${normalizedRoute}/`)
    );
  });
}

/**
 * 路由守卫：是否允许进入目标路由
 * @param {object} options 参数
 * @param {string[]} options.permissions 权限码列表
 * @param {string[]} options.routePaths 可访问路由列表
 * @param {string} [options.permission] 路由 meta.permission
 * @param {string} options.path 目标 path
 * @param {boolean} [options.skipRoutePathCheck] 跳过菜单 routePaths 校验（个人中心等登录即可访问页）
 * @returns {boolean} 是否允许
 */
export function canActivateRoute(options: {
  permissions: string[];
  routePaths: string[];
  permission?: string;
  path: string;
  skipRoutePathCheck?: boolean;
}): boolean {
  if (options.permission && !hasPermissionCode(options.permissions, options.permission)) {
    return false;
  }

  if (options.skipRoutePathCheck) {
    return true;
  }

  if (options.routePaths.length === 0 && options.permissions.length === 0) {
    const path = normalizeRoutePath(options.path);
    return path === '/dashboard' || path === '/403' || path === '/profile';
  }

  if (options.routePaths.length === 0) {
    return true;
  }

  return canAccessRoute(options.routePaths, options.path);
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktMenuOpenKeys.ts
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：侧栏菜单 openKeys 解析（路由祖先链、手风琴祖先链）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { MenuProps } from 'ant-design-vue';

type TaktMenuItem = NonNullable<MenuProps['items']>[number];

/**
 * 判断是否为可遍历的菜单项
 * @param item 菜单项
 */
function isMenuItemNode(item: TaktMenuItem): item is { key: string | number; children?: MenuProps['items'] } {
  return !!item && typeof item === 'object' && 'key' in item;
}

/** 菜单树遍历最大深度（07-overflow-vue：栈溢出防护） */
export const TAKT_MAX_MENU_TREE_DEPTH = 10;

/**
 * 构建菜单 key → 父级 key 映射（目录节点 key 多为 menuCode，页面为 routePath）
 * @param items Ant Design Menu items
 * @param parentKey 父级 key
 * @param map 累积映射
 * @param depth 当前深度
 */
export function buildMenuParentKeyMap(
  items: MenuProps['items'] | undefined,
  parentKey: string | null = null,
  map: Map<string, string | null> = new Map(),
  depth = 0
): Map<string, string | null> {
  if (!items?.length || depth > TAKT_MAX_MENU_TREE_DEPTH) {
    return map;
  }

  items.forEach((raw) => {
    if (!isMenuItemNode(raw)) {
      return;
    }

    const key = String(raw.key);
    map.set(key, parentKey);

    if (raw.children?.length) {
      buildMenuParentKeyMap(raw.children, key, map, depth + 1);
    }
  });

  return map;
}

/**
 * 手风琴模式：保留「最后展开项」到根节点的完整祖先链（同级其他分支收起）
 * @param keys 当前 openKeys
 * @param parentByKey key → 父 key
 */
export function getMenuAccordionOpenKeys(keys: string[], parentByKey: Map<string, string | null>): string[] {
  if (keys.length === 0) {
    return [];
  }

  const last = keys[keys.length - 1];
  if (!last) {
    return [];
  }

  const chain: string[] = [];
  const visited = new Set<string>();
  let current: string | null | undefined = last;

  while (current && !visited.has(current)) {
    visited.add(current);
    chain.unshift(current);
    current = parentByKey.get(current) ?? null;
  }

  return chain;
}

/**
 * 移除 openKeys 中「祖先未展开」的孤儿 key（收起父级时同步收起全部子孙 SubMenu）
 * @param keys 当前 openKeys
 * @param parentByKey key → 父 key
 */
export function pruneOpenKeysWithoutAncestors(
  keys: string[],
  parentByKey: Map<string, string | null>
): string[] {
  if (keys.length === 0) {
    return [];
  }

  const keySet = new Set(keys);
  return keys.filter((key) => {
    let parent = parentByKey.get(key) ?? null;
    while (parent) {
      if (!keySet.has(parent)) {
        return false;
      }
      parent = parentByKey.get(parent) ?? null;
    }
    return true;
  });
}

/**
 * 规范化用户操作后的 openKeys：先级联收拢孤儿 key，再应用手风琴
 * @param keys 当前 openKeys
 * @param parentByKey key → 父 key
 * @param accordion 是否手风琴（只保留最后展开分支的祖先链）
 */
export function normalizeMenuOpenKeys(
  keys: string[],
  parentByKey: Map<string, string | null>,
  accordion: boolean
): string[] {
  let next = pruneOpenKeysWithoutAncestors(keys, parentByKey);
  if (accordion && next.length > 1) {
    next = getMenuAccordionOpenKeys(next, parentByKey);
  }
  return next;
}

/**
 * 按当前路由 path 解析应展开的父级 SubMenu keys（与 formatMenuItems 的 key 规则一致）
 * @param items Ant Design Menu items
 * @param path 当前路由 path
 * @param trail 祖先 key 链
 * @param depth 当前深度
 */
export function resolveMenuOpenKeysForPath(
  items: MenuProps['items'] | undefined,
  path: string,
  trail: string[] = [],
  depth = 0
): string[] {
  if (!items?.length || depth > TAKT_MAX_MENU_TREE_DEPTH) {
    return [];
  }

  for (const raw of items) {
    if (!isMenuItemNode(raw)) {
      continue;
    }

    const key = String(raw.key);
    const children = raw.children;

    if (children?.length) {
      const nested = resolveMenuOpenKeysForPath(children, path, [...trail, key], depth + 1);
      if (nested.length > 0) {
        return nested;
      }
    }

    if (key === path) {
      return trail;
    }
  }

  return [];
}

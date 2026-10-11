// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktRemixIcon.ts
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：将后端菜单 Icon 字段（如 RiGridLine）映射为 @remixicon/vue 组件（运行时网关：模块懒加载与组件缓存）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { markRaw, type Component } from 'vue';
import type { MenuTree } from '@/types/identity/menu.types';

/** @remixicon/vue 模块缓存（单例动态导入） */
let remixIconModulePromise: Promise<Record<string, unknown>> | null = null;

/** 已解析的 Remix 图标组件缓存（键为规范化组件名，如 RiGridLine） */
const remixIconComponentCache = new Map<string, Component>();

/** 全量 Remix 导出名缓存（排序后） */
let remixIconNameListCache: string[] | null = null;

/** 图标变体：Line / Fill / 全部 */
export type TaktRemixIconVariant = 'line' | 'fill' | 'all';

/**
 * 规范化 Remix 图标组件名（后端种子多为 RiGridLine；兼容 ri-grid-line）
 * @param raw 原始图标名
 * @returns {string | undefined} 组件导出名，无法识别时 undefined
 */
export function normalizeRemixIconName(raw: string | undefined | null): string | undefined {
  const trimmed = String(raw ?? '').trim();
  if (!trimmed) {
    return undefined;
  }

  if (/^Ri[A-Za-z0-9]+$/.test(trimmed)) {
    return trimmed;
  }

  const parts = trimmed.split('-').filter(Boolean);
  if (parts.length === 0) {
    return undefined;
  }

  return parts
    .map((part) => part.charAt(0).toUpperCase() + part.slice(1).toLowerCase())
    .join('');
}

/**
 * 加载 @remixicon/vue 模块
 * @returns {Promise<Record<string, unknown>>} 图标模块
 */
async function loadRemixIconModule(): Promise<Record<string, unknown>> {
  if (!remixIconModulePromise) {
    remixIconModulePromise = import('@remixicon/vue').then((module) => module as Record<string, unknown>);
  }

  return remixIconModulePromise;
}

/**
 * 预加载一批 Remix 图标到缓存
 * @param iconNames 后端 Icon 字段列表
 */
export async function preloadRemixIcons(iconNames: Iterable<string>): Promise<void> {
  const normalizedNames = [...new Set(
    [...iconNames]
      .map((name) => normalizeRemixIconName(name))
      .filter((name): name is string => !!name),
  )];

  if (normalizedNames.length === 0) {
    return;
  }

  const module = await loadRemixIconModule();

  normalizedNames.forEach((name) => {
    if (remixIconComponentCache.has(name)) {
      return;
    }

    const candidate = module[name];
    if (candidate && typeof candidate === 'object') {
      remixIconComponentCache.set(name, markRaw(candidate as Component));
    }
  });
}

/**
 * 从缓存获取 Remix 图标组件
 * @param rawIconName 后端 Icon 字段
 * @returns {Component | undefined} Vue 组件
 */
export function getRemixIconComponent(rawIconName: string | undefined | null): Component | undefined {
  const name = normalizeRemixIconName(rawIconName);
  if (!name) {
    return undefined;
  }

  return remixIconComponentCache.get(name);
}

/**
 * 确保图标已加载并返回组件（未命中缓存时按需预加载）
 * @param rawIconName 后端 Icon 字段
 * @returns {Promise<Component | undefined>} Vue 组件
 */
export async function ensureRemixIconLoaded(
  rawIconName: string | undefined | null,
): Promise<Component | undefined> {
  const name = normalizeRemixIconName(rawIconName);
  if (!name) {
    return undefined;
  }

  const cached = remixIconComponentCache.get(name);
  if (cached) {
    return cached;
  }

  await preloadRemixIcons([name]);
  return remixIconComponentCache.get(name);
}

/**
 * 列出 @remixicon/vue 导出名（可按 Line/Fill 过滤；结果缓存）
 * @param options.variant 变体，默认 line（菜单种子多为 *Line）
 * @returns {Promise<string[]>} 图标组件名列表
 */
export async function listRemixIconNames(options?: {
  variant?: TaktRemixIconVariant
}): Promise<string[]> {
  if (!remixIconNameListCache) {
    const module = await loadRemixIconModule();
    remixIconNameListCache = Object.keys(module)
      .filter((key) => /^Ri[A-Za-z0-9]+$/.test(key))
      .sort((a, b) => a.localeCompare(b));
  }

  const variant = options?.variant ?? 'line';
  if (variant === 'all') {
    return remixIconNameListCache;
  }

  if (variant === 'fill') {
    return remixIconNameListCache.filter((name) => name.endsWith('Fill'));
  }

  return remixIconNameListCache.filter((name) => name.endsWith('Line'));
}

/**
 * 按关键字过滤图标名（忽略大小写；空关键字返回原列表）
 * @param names 候选列表
 * @param keyword 搜索关键字
 * @returns {string[]} 过滤后列表
 */
export function filterRemixIconNames(names: readonly string[], keyword: string | undefined | null): string[] {
  if (!names?.length) {
    return [];
  }

  const q = String(keyword ?? '').trim().toLowerCase();
  if (!q) {
    return [...names];
  }

  return names.filter((name) => name.toLowerCase().includes(q));
}

/** 菜单树遍历最大深度（07-overflow-vue） */
const TAKT_MAX_MENU_TREE_DEPTH = 10;

/**
 * 收集菜单树中的 Icon 字段
 * @param menus 菜单树
 * @returns {string[]} 图标名列表
 */
export function collectMenuIconNames(menus: MenuTree[]): string[] {
  const names: string[] = [];

  const walk = (nodes: MenuTree[], depth: number): void => {
    if (depth > TAKT_MAX_MENU_TREE_DEPTH) {
      return;
    }
    nodes.forEach((menu) => {
      if (menu.icon?.trim()) {
        names.push(menu.icon.trim());
      }

      if (menu.children?.length) {
        walk(menu.children, depth + 1);
      }
    });
  };

  walk(menus, 0);
  return names;
}

/**
 * 清空 Remix 图标缓存（登出等场景）
 */
export function clearRemixIconCache(): void {
  remixIconComponentCache.clear();
  remixIconModulePromise = null;
  remixIconNameListCache = null;
}

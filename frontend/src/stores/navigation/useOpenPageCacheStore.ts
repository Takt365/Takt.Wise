// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/stores/navigation
// 文件名称：useOpenPageCacheStore.ts
// 创建时间：2026-11-04
// 创建人：Takt365(Cursor AI)
// 功能描述：仍打开的标签 path。页面出口只缓存这些 path；关闭标签后从列表移除，实例随之销毁
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { defineStore } from 'pinia';
import { ref } from 'vue';

/**
 * 仍打开的页面路径（与标签栏 1:1，不含查询串）
 */
export const useOpenPageCacheStore = defineStore('openPageCache', () => {
  /** 当前仍打开的路由 path */
  const paths = ref<string[]>([]);

  /**
   * 用标签栏的当前列表覆盖缓存名单
   * @param next 标签 path
   */
  function sync(next: readonly string[]): void {
    const unique: string[] = [];
    const seen = new Set<string>();
    for (const raw of next) {
      const path = raw.trim();
      if (!path || seen.has(path)) {
        continue;
      }
      seen.add(path);
      unique.push(path);
    }
    const current = paths.value;
    if (unique.length === current.length && unique.every((path, index) => path === current[index])) {
      return;
    }
    paths.value = unique;
  }

  /**
   * 登出时清空，避免下一会话保住上一用户的页面实例
   */
  function clear(): void {
    paths.value = [];
  }

  return { paths, sync, clear };
});

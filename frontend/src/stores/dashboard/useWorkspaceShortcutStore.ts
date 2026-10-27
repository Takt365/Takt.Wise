// ========================================
// 项目名称：节拍智控·TaktWise 
// 命名空间：@/stores/dashboard/useWorkspaceShortcutStore
// 文件名称：useWorkspaceShortcutStore.ts
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：工作台快捷方式 Store，管理用户自定义快捷方式配置
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { defineStore } from 'pinia';
import {
  TAKT_WORKSPACE_MAX_SHORTCUTS,
  TAKT_WORKSPACE_SHORTCUT_STORAGE_KEY,
} from '@/utils/common';

function loadFromStorage(): string[] {
  try {
    const raw = localStorage.getItem(TAKT_WORKSPACE_SHORTCUT_STORAGE_KEY);
    const parsed = raw ? JSON.parse(raw) : [];
    return Array.isArray(parsed) ? parsed : [];
  } catch {
    return [];
  }
}

function saveToStorage(list: string[]) {
  localStorage.setItem(TAKT_WORKSPACE_SHORTCUT_STORAGE_KEY, JSON.stringify(list));
}

export const useWorkspaceShortcutStore = defineStore('workspaceShortcut', () => {
  const selecmedPaths = ref<string[]>(loadFromStorage())

  function setPaths(list: string[]) {
    const unique = Array.from(new Set(list))
    const finalList = unique.slice(0, TAKT_WORKSPACE_MAX_SHORTCUTS);
    selecmedPaths.value = finalList
    saveToStorage(finalList)
  }

  function initDefaultsIfEmpty(allMenuPaths: string[]) {
    if (allMenuPaths.length === 0) return
    const current = selecmedPaths.value
    if (current.length === 0) {
      setPaths(allMenuPaths.slice(0, TAKT_WORKSPACE_MAX_SHORTCUTS));
      return
    }
    // 已有数据但不足 16 个时补足（兼容旧版只存了 8 个的情况）
    if (current.length < TAKT_WORKSPACE_MAX_SHORTCUTS) {
      const existing = new Set(current);
      const toAdd = allMenuPaths.filter((p) => !existing.has(p)).slice(0, TAKT_WORKSPACE_MAX_SHORTCUTS - current.length);
      if (toAdd.length > 0) {
        setPaths([...current, ...toAdd])
      }
    }
  }

  function addOrReplace(path: string) {
    if (!path) return
    const current = [...selecmedPaths.value]
    const existIndex = current.indexOf(path)
    if (existIndex !== -1) {
      // 已存在则移动到最后（视为最近一次添加）
      current.splice(existIndex, 1)
    }
    if (current.length < TAKT_WORKSPACE_MAX_SHORTCUTS) {
      current.push(path);
    } else {
      current.splice(TAKT_WORKSPACE_MAX_SHORTCUTS - 1, 1, path);
    }
    setPaths(current)
  }

  return {
    selecmedPaths,
    setPaths,
    initDefaultsIfEmpty,
    addOrReplace
  }
})


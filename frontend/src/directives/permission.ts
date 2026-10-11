// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/directives
// 文件名称：permission.ts
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：v-permission 指令（按钮级功能权限）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { App, Directive, DirectiveBinding } from 'vue';
import { watch, type WatchStopHandle } from 'vue';
import { usePermissionStore } from '@/stores/identity/usePermissionStore';
import type { TaktPermissionBindingValue } from '@/types/common.types';
import { hasAnyPermission } from '@/utils/permission';

/** 指令卸载时停止 store 监听的扩展属性 */
type PermissionElement = HTMLElement & {
  _permissionStop?: WatchStopHandle;
};

/**
 * 根据当前用户权限更新元素可见性
 * @description 无权限时 display:none；有权限时恢复默认显示（清空 inline display）
 * @param {HTMLElement} el 绑定 v-permission 的 DOM 元素
 * @param {DirectiveBinding<TaktPermissionBindingValue>} binding 指令绑定（value 为权限码或数组）
 * @returns {void}
 */
function updatePermissionVisibility(
  el: HTMLElement,
  binding: DirectiveBinding<TaktPermissionBindingValue>
): void {
  /** 登录后合并的权限码列表（菜单 + 用户资料） */
  const permissionStore = usePermissionStore();
  /** 是否满足 binding.value 中任一权限码（空值视为通过） */
  const allowed = hasAnyPermission(permissionStore.permissions, binding.value);
  // 无权限隐藏；有权限移除指令写入的 display 以恢复 CSS 默认
  el.style.display = allowed ? '' : 'none';
}

/**
 * 功能权限指令（v-permission）
 * @description 挂载时校验并订阅 permissions 变化；仅控制 UI 显隐，不替代后端 [TaktPermission]
 */
const permissionDirective: Directive<PermissionElement, TaktPermissionBindingValue> = {
  /**
   * 元素挂载：初次显隐 + 监听权限列表变更
   * @param {PermissionElement} el 绑定元素
   * @param {DirectiveBinding<TaktPermissionBindingValue>} binding 指令绑定
   */
  mounted(el, binding) {
    // 按当前权限快照设置显隐
    updatePermissionVisibility(el, binding);
    /** 权限 Store，permissions 在登录/租户切换后更新 */
    const permissionStore = usePermissionStore();
    // 权限列表变化时重新计算显隐（deep 以兼容数组替换）
    el._permissionStop = watch(
      () => permissionStore.permissions,
      () => updatePermissionVisibility(el, binding),
      { deep: true }
    );
  },
  /**
   * 绑定值变更：同步更新显隐（如动态切换权限码）
   * @param {PermissionElement} el 绑定元素
   * @param {DirectiveBinding<TaktPermissionBindingValue>} binding 指令绑定
   */
  updated(el, binding) {
    // binding.value 变化时立即刷新，无需等待 watch 下一 tick
    updatePermissionVisibility(el, binding);
  },
  /**
   * 元素卸载：停止 watch，避免泄漏
   * @param {PermissionElement} el 绑定元素
   */
  unmounted(el) {
    // 停止 permissions 订阅
    el._permissionStop?.();
    // 清理扩展字段
    delete el._permissionStop;
  },
};

/**
 * 注册 v-permission 指令
 * @description 须在 app.use(pinia) 之后调用（指令内使用 usePermissionStore）
 * @param {App} app Vue 应用实例
 * @returns {void}
 */
export function registerPermissionDirective(app: App): void {
  // 全局注册 permission 指令名
  app.directive('permission', permissionDirective);
}

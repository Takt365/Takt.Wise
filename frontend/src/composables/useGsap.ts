// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/composables
// 文件名称：useGsap.ts
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：Vue 3 组合式 GSAP 封装（gsap.context + 卸载 revert，对齐 GreenSock Vue 指南）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import gsap from 'gsap';
import { onMounted, onUnmounted, ref, watch, type Ref } from 'vue';

/** useGsap 入参 */
export interface UseGsapOptions {
  /** 动画作用域根节点；未传时由 composable 创建 container ref 供模板绑定 */
  scope?: Ref<HTMLElement | null>;
  /** 变化时重建 context（类似 @gsap/react useGSAP 的 dependencies） */
  dependencies?: ReadonlyArray<unknown>;
  /** 为 false 时不自动 onMounted 执行（需手动调用 refresh） */
  immediate?: boolean;
}

/** useGsap 返回值 */
export interface UseGsapReturn {
  /** 绑定到模板根容器的 ref（未传 scope 时使用） */
  container: Ref<HTMLElement | null>;
  /** 手动重建动画 context */
  refresh: () => void;
  /** 手动 revert 当前 context */
  revert: () => void;
}

/**
 * Vue 3 GSAP 组合式：在 onMounted 内创建 gsap.context，onUnmounted 自动 revert
 * @param callback 在 context 内创建 tween / ScrollTrigger
 * @param options 作用域与依赖
 * @returns {UseGsapReturn} container / refresh / revert
 */
export function useGsap(
  callback: () => void,
  options: UseGsapOptions = {},
): UseGsapReturn {
  const container = options.scope ?? ref<HTMLElement | null>(null);
  const immediate = options.immediate !== false;
  let ctx: gsap.Context | undefined;

  /**
   * 销毁当前 context
   */
  function revert(): void {
    ctx?.revert();
    ctx = undefined;
  }

  /**
   * 在 scope 根节点下重建动画
   */
  function refresh(): void {
    revert();
    const root = container.value;
    if (!root) {
      return;
    }
    ctx = gsap.context(callback, root);
  }

  if (immediate) {
    onMounted(refresh);
  }

  if (options.dependencies?.length) {
    watch(options.dependencies, refresh);
  }

  onUnmounted(revert);

  return {
    container,
    refresh,
    revert,
  };
}

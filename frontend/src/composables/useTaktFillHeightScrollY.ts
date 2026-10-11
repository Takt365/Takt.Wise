// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/composables
// 文件名称：useTaktFillHeightScrollY.ts
// 创建时间：2026-08-21
// 创建人：Takt365(Cursor AI)
// 功能描述：测量已撑满父级的容器高度，供左树 a-tree.height / 右表 scroll.y；左右表外框等高
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import {
  nextTick,
  onBeforeUnmount,
  onMounted,
  ref,
  unref,
  watch,
  type MaybeRef,
  type Ref,
} from 'vue';
import {
  TAKT_TABLE_SCROLL_Y_MIN,
  measureFillHeightScrollYPx,
} from '@/utils/tableScroll';

/** 填满高度测量选项 */
export interface UseTaktFillHeightScrollYOptions {
  /** 容器尚未布局时的回退高度（px） */
  fallbackPx?: MaybeRef<number>;
  /** 右表：从容器高度中扣除 .ant-table-header */
  subtractTableHeader?: MaybeRef<boolean>;
  /** 变化时重新测量（如 loading、列数、行数） */
  recalcToken?: MaybeRef<unknown>;
}

/**
 * 监听容器尺寸，输出填满父级后的纵向滚动高度
 * @param hostRef 左树 viewport 或右表 __body
 * @param options 表头扣除与回退高度
 * @returns {Ref<number>} scroll.y / a-tree.height（px）
 */
export function useTaktFillHeightScrollY(
  hostRef: Ref<HTMLElement | null | undefined>,
  options: UseTaktFillHeightScrollYOptions = {},
): Ref<number> {
  /** 实测（或回退）纵向高度（px） */
  const heightPx = ref(resolveFallbackPx(options.fallbackPx));
  /** 容器尺寸观察器 */
  let resizeObserver: ResizeObserver | null = null;

  /**
   * 解析回退高度
   * @param fallback 可选回退
   * @returns {number} px
   */
  function resolveFallbackPx(fallback: MaybeRef<number> | undefined): number {
    const value = unref(fallback);
    if (typeof value === 'number' && Number.isFinite(value) && value > 0) {
      return Math.max(TAKT_TABLE_SCROLL_Y_MIN, Math.floor(value));
    }
    return TAKT_TABLE_SCROLL_Y_MIN;
  }

  /** 按当前 DOM 重算高度 */
  function recalcFillHeight(): void {
    const el = hostRef.value;
    const next =
      el != null && el.clientHeight > 0
        ? measureFillHeightScrollYPx(el, {
            subtractTableHeader: unref(options.subtractTableHeader) === true,
          })
        : resolveFallbackPx(options.fallbackPx);
    if (next !== heightPx.value) {
      heightPx.value = next;
    }
  }

  /** 停止观察 */
  function stopObserve(): void {
    resizeObserver?.disconnect();
    resizeObserver = null;
  }

  /** 开始观察并立即测量 */
  function startObserve(): void {
    stopObserve();
    void nextTick(() => {
      recalcFillHeight();
      const el = hostRef.value;
      if (!el || typeof ResizeObserver === 'undefined') {
        return;
      }
      resizeObserver = new ResizeObserver(() => {
        recalcFillHeight();
      });
      resizeObserver.observe(el);
    });
  }

  watch(
    () => hostRef.value,
    () => {
      startObserve();
    },
  );
  watch(
    () => [unref(options.subtractTableHeader), unref(options.fallbackPx), unref(options.recalcToken)],
    () => {
      void nextTick(() => {
        recalcFillHeight();
      });
    },
  );

  onMounted(() => {
    startObserve();
  });
  onBeforeUnmount(() => {
    stopObserve();
  });

  return heightPx;
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/composables
// 文件名称：useTaktTableContainerScrollY.ts
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：ResizeObserver 测量表格容器高度，输出 scroll.y（用于左右主子表右栏等非整页 flex 区域）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { onBeforeUnmount, onMounted, ref, watch, type Ref } from 'vue';
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/tableScroll';

/** Ant Design Table 表头高度回退（size=middle） */
const ANT_TABLE_HEADER_FALLBACK_PX = 56;

/**
 * 监听表格容器尺寸，输出 Ant Design Table scroll.y（容器高度减表头）
 * @param containerRef 绑定 takt-single-table__body 的 DOM
 * @param enabled 是否启用（如 masterDetailLr 且非 virtual）
 * @returns 响应式 scroll.y（px）
 */
export function useTaktTableContainerScrollY(
  containerRef: Ref<HTMLElement | null | undefined>,
  enabled: Ref<boolean>,
) {
  /** 当前容器计算得到的 scroll.y（px） */
  const scrollYPx = ref(TAKT_TABLE_SCROLL_Y_MIN);

  let resizeObserver: ResizeObserver | null = null;

  /**
   * 按容器 clientHeight 与表头高度重算 scroll.y
   */
  function recalcContainerScrollY(): void {
    const el = containerRef.value;
    if (!el || !enabled.value) {
      return;
    }
    if (el.clientHeight <= 0) {
      return;
    }
    const headerEl = el.querySelector('.ant-table-header') as HTMLElement | null;
    const headerHeight = headerEl?.offsetHeight ?? ANT_TABLE_HEADER_FALLBACK_PX;
    const next = Math.max(TAKT_TABLE_SCROLL_Y_MIN, el.clientHeight - headerHeight);
    if (next !== scrollYPx.value) {
      scrollYPx.value = next;
    }
  }

  /** 开始监听容器尺寸 */
  function startObserve(): void {
    stopObserve();
    const el = containerRef.value;
    if (!el || !enabled.value) {
      return;
    }
    recalcContainerScrollY();
    resizeObserver = new ResizeObserver(() => {
      recalcContainerScrollY();
    });
    resizeObserver.observe(el);
  }

  /** 停止监听 */
  function stopObserve(): void {
    resizeObserver?.disconnect();
    resizeObserver = null;
  }

  watch(
    enabled,
    (active) => {
      if (active) {
        startObserve();
      } else {
        stopObserve();
      }
    },
  );

  watch(containerRef, () => {
    if (enabled.value) {
      startObserve();
    }
  });

  onMounted(() => {
    if (enabled.value) {
      startObserve();
    }
  });

  onBeforeUnmount(() => {
    stopObserve();
  });

  return scrollYPx;
}

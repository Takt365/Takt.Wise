// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/composables
// 文件名称：useTaktTableViewportScrollY.ts
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：按 window.innerHeight 减去固定 header/footer 与页面壳预留（300px）计算 scroll.y，resize 时更新
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { onBeforeUnmount, onMounted, ref, unref, watch, type MaybeRef } from 'vue';
import {
  computeTableScrollYPx,
  type TaktTableScrollLayout,
} from '@/utils/tableScroll';

/**
 * 监听视口尺寸，输出 Ant Design Table 可用的 scroll.y 像素高度（innerHeight - 固定顶底栏与页壳预留）
 * @param scrollLayout 表格布局场景（与 takt-single-table scrollLayout 一致）
 * @returns 响应式 scroll.y（px）
 */
export function useTaktTableViewportScrollY(scrollLayout: MaybeRef<TaktTableScrollLayout> = 'page') {
  /** 当前视口计算得到的 scroll.y（px） */
  const scrollYPx = ref(computeTableScrollYPx(unref(scrollLayout)));

  /** 按当前 window.innerHeight 重算 scroll.y */
  function recalcViewportScrollY(): void {
    const vh = typeof window !== 'undefined' ? window.innerHeight : undefined;
    scrollYPx.value = computeTableScrollYPx(unref(scrollLayout), vh);
  }

  onMounted(() => {
    recalcViewportScrollY();
    window.addEventListener('resize', recalcViewportScrollY);
  });

  onBeforeUnmount(() => {
    window.removeEventListener('resize', recalcViewportScrollY);
  });

  watch(
    () => unref(scrollLayout),
    () => {
      recalcViewportScrollY();
    },
  );

  return scrollYPx;
}

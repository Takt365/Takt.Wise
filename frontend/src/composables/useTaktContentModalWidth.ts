// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/composables
// 文件名称：useTaktContentModalWidth.ts
// 创建时间：2026-09-01
// 创建人：Takt365(Cursor AI)
// 功能描述：弹出窗体宽度随视口与左侧菜单实测变化：（innerWidth − .layout-sider）× 80%
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { nextTick, onBeforeUnmount, onMounted, ref, watch } from 'vue'
import { useSettingStore } from '@/stores/common/useSettingStore'
import {
  computeTaktContentModalWidthPx,
  measureTaktLeftSiderWidth,
  TAKT_CONTENT_MODAL_WIDTH_RATIO,
} from '@/utils/taktContentModalWidth'

/**
 * 监听视口与左侧菜单宽度，输出弹窗 width（px）
 * @param ratio 占「视口 − 侧栏」的比例，默认 0.8
 * @returns 响应式弹窗宽度（px）
 */
export function useTaktContentModalWidth(ratio: number = TAKT_CONTENT_MODAL_WIDTH_RATIO) {
  const settingStore = useSettingStore()
  /** 当前弹窗宽度（px） */
  const widthPx = ref(computeTaktContentModalWidthPx(
    typeof window !== 'undefined' ? window.innerWidth : 0,
    0,
    ratio,
  ))
  /** 左侧菜单 ResizeObserver */
  let siderResizeObserver: ResizeObserver | null = null

  /**
   * 按当前视口与实测侧栏重算弹窗宽度
   */
  function recalcContentModalWidth(): void {
    const viewportWidth = typeof window !== 'undefined' ? window.innerWidth : 0
    widthPx.value = computeTaktContentModalWidthPx(
      viewportWidth,
      measureTaktLeftSiderWidth(),
      ratio,
    )
  }

  /**
   * 绑定/重绑左侧菜单 ResizeObserver（折叠、改宽、切布局时更新）
   */
  function bindSiderResizeObserver(): void {
    siderResizeObserver?.disconnect()
    siderResizeObserver = null
    if (typeof document === 'undefined' || typeof ResizeObserver === 'undefined') {
      return
    }
    const sider = document.querySelector('.layout-sider') as HTMLElement | null
    if (sider == null) {
      return
    }
    siderResizeObserver = new ResizeObserver(() => {
      recalcContentModalWidth()
    })
    siderResizeObserver.observe(sider)
  }

  onMounted(() => {
    recalcContentModalWidth()
    bindSiderResizeObserver()
    window.addEventListener('resize', recalcContentModalWidth)
  })

  watch(
    () => [
      settingStore.setting.layout,
      settingStore.setting.siderWidth,
      settingStore.setting.siderCollapsedWidth,
    ],
    () => {
      void nextTick(() => {
        bindSiderResizeObserver()
        recalcContentModalWidth()
      })
    },
  )

  onBeforeUnmount(() => {
    window.removeEventListener('resize', recalcContentModalWidth)
    siderResizeObserver?.disconnect()
    siderResizeObserver = null
  })

  return widthPx
}

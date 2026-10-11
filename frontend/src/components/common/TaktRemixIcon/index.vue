<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/components/common/TaktRemixIcon -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：按名称渲染 @remixicon/vue 图标（后端菜单 Icon 如 RiGridLine）；依赖 utils/taktRemixIcon 懒加载缓存 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <component
    :is="iconComponent"
    v-if="iconComponent"
    class="takt-remix-icon inline-block shrink-0"
    :class="iconClass"
    :style="iconStyle"
  />
  <span
    v-else-if="showPlaceholder"
    class="takt-remix-icon-placeholder inline-flex shrink-0 items-center justify-center text-text-secondary"
    :style="iconStyle"
    :title="normalizedName || undefined"
  >
    <RiQuestionLine class="takt-remix-icon size-full" />
  </span>
</template>

<script setup lang="ts">
/**
 * Remix 图标展示：按组件名异步解析并渲染
 */
import { RiQuestionLine } from '@remixicon/vue'
import type { Component, CSSProperties } from 'vue'
import {
  ensureRemixIconLoaded,
  normalizeRemixIconName,
} from '@/utils/taktRemixIcon'

interface Props {
  /** Remix 组件名（如 RiGridLine）或兼容 ri-grid-line */
  name?: string | null
  /** 边长（px）；默认 18 */
  size?: number
  /** 附加 class */
  iconClass?: string
  /** 未解析到组件时是否显示占位问号 */
  showPlaceholder?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  name: undefined,
  size: 18,
  iconClass: undefined,
  showPlaceholder: true,
})

/** 已解析图标组件 */
const iconComponent = shallowRef<Component | undefined>()

/** 规范化后的图标名 */
const normalizedName = computed(() => normalizeRemixIconName(props.name))

/** 尺寸样式 */
const iconStyle = computed<CSSProperties>(() => ({
  width: `${props.size}px`,
  height: `${props.size}px`,
}))

watch(
  () => props.name,
  async (raw) => {
    iconComponent.value = await ensureRemixIconLoaded(raw)
  },
  { immediate: true },
)
</script>

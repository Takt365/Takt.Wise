<!-- ========================================
项目名称:TaktWise
命名空间:@/components/navigation/TaktWatermark
文件名称:index.vue
创建时间:2025-01-20
创建人:Takt365(Cursor AI)
功能描述:水印组件,显示用户信息水印以防泄露

版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->
<template>
  <!-- 始终用同一层 a-watermark 包裹，仅通过 content 控制显隐，避免开关水印时布局重挂载导致抽屉被关 -->
  <a-watermark
    :content="show ? watermarkContent : ''"
    :gap="gap"
    :offset="offset"
    :font="fontByTheme"
    :rotate="rotate"
    :z-index="zIndex"
    :width="width"
    :height="height"
    v-bind="show && image ? { image } : {}"
    class="takt-watermark"
  >
    <div class="takt-watermark-content">
      <slot />
    </div>
  </a-watermark>
</template>

<script setup lang="ts">
import { useSettingStore } from '@/stores/common/useSettingStore'
import { useThemeStore } from '@/stores/common/useThemeStore'

interface FontConfig {
  fontSize?: number | string
  color?: string
  fontFamily?: string
  fontWeight?: 'normal' | 'light' | 'weight' | number
  fontStyle?: 'none' | 'normal' | 'italic' | 'oblique'
}

interface Props {
  /** 水印文字内容 */
  content?: string | string[] | undefined
  /** 水印之间的间距 [x, y] */
  gap?: [number, number]
  /** 水印距离容器左上角的偏移量 [x, y] */
  offset?: [number, number]
  /** 字体配置(不传 color 时按主题自动适配) */
  font?: FontConfig | undefined
  /** 旋转角度 */
  rotate?: number
  /** z-index */
  zIndex?: number
  /** 水印宽度 */
  width?: number
  /** 水印高度 */
  height?: number
  /** 图片水印地址 */
  image?: string | undefined
}

const props = withDefaults(defineProps<Props>(), {
  content: undefined,
  gap: () => [100, 100],
  offset: () => [0, 0],
  font: undefined,
  rotate: -22,
  zIndex: 1000,
  width: 120,
  height: 64,
  image: undefined
})

const { setting } = storeToRefs(useSettingStore())
const themeStore = useThemeStore()
const show = computed(() => setting.value.watermark)
const watermarkContent = computed(() => {
  return props.content ?? setting.value.watermarkContent
})

/** 按主题适配水印颜色：亮色用深色半透明，暗色用浅色半透明 */
const fontByTheme = computed(() => {
  const base = props.font ?? { fontSize: 16 }
  if (base.color !== undefined) return { ...base }
  const color =
    themeStore.resolvedTheme === 'dark'
      ? 'rgba(255, 255, 255, 0.12)'
      : 'rgba(0, 0, 0, 0.15)'
  return { ...base, color }
})
</script>

<style scoped>
.takt-watermark {
  position: relative;
}
/* 给水印容器明确占满视口，水印层才能铺满整页（Ant Design 水印相对容器定位） */
.takt-watermark-content {
  min-height: 100vh;
  width: 100%;
}
</style>

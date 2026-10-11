<!-- ========================================
项目名称:TaktWise
命名空间:@/components/navigation/TaktHeaderFull
文件名称:index.vue
创建时间:2025-01-20
创建人:Takt365(Cursor AI)
功能描述:全屏切换组件,用于切换浏览器全屏模式

版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->
<template>
  <a-button
    type="text"
    class="takt-header-full"
    :title="isFullscreen ? $t('common.page.button.exitfullscreen') : $t('common.page.button.fullscreen')"
    @click="handleToggleFullscreen"
  >
    <template #icon>
      <RiFullscreenExitLine class="takt-remix-icon" v-if="isFullscreen" />
      <RiFullscreenLine class="takt-remix-icon" v-else />
    </template>
  </a-button>
</template>

<script setup lang="ts">
import { RiFullscreenLine, RiFullscreenExitLine } from '@remixicon/vue'
import { createLogger } from '@/utils/logger'

const headerFullLogger = createLogger('takt-header-full')

const isFullscreen = ref(false)

const handleToggleFullscreen = () => {
  if (!document.fullscreenElement) {
    document.documentElement.requestFullscreen().then(() => {
      isFullscreen.value = true
    }).catch(() => {
      headerFullLogger.error('无法进入全屏模式', { action: 'enterFullscreen' })
    })
  } else {
    document.exitFullscreen().then(() => {
      isFullscreen.value = false
    }).catch(() => {
      headerFullLogger.error('无法退出全屏模式', { action: 'exitFullscreen' })
    })
  }
}

const handleFullscreenChange = () => {
  isFullscreen.value = !!document.fullscreenElement
}

onMounted(() => {
  document.addEventListener('fullscreenchange', handleFullscreenChange)
})

onUnmounted(() => {
  document.removeEventListener('fullscreenchange', handleFullscreenChange)
})
</script>

<style scoped>

</style>

<!-- ========================================
项目名称：节拍智控·TaktWise
命名空间：@/layouts
文件名称：SideLayout.vue
创建时间：2025-01-20
创建人：Takt365(Cursor AI)
功能描述：侧边栏布局，包含侧边菜单和主内容区

版权信息：Copyright (c) 2025 Takt  All rights reserved.
免责声明：此软件使用 MIT License，作者不承担任何使用风险。
======================================== -->

<template>
  <a-layout class="side-layout">
    <a-layout-sider
      v-model:collapsed="collapsed"
      :width="settingSafe.siderWidth"
      :collapsed-width="settingSafe.siderCollapsedWidth"
      class="layout-sider"
      :theme="themeStore.resolvedTheme === 'dark' ? 'dark' : 'light'"
      :style="{ position: settingSafe.fixedSider ? 'fixed' : 'relative', height: '100vh', left: 0, top: 0 }"
    >
      <div
        class="sider-header"
        :class="{ collapsed }"
      >
        <img
          v-if="logoUrl && settingSafe.showLogo"
          :src="logoUrl"
          :alt="settingSafe.logoText"
          class="logo-image"
          @error="handleLogoError"
          @load="handleLogoLoad"
        >
        <span
          v-if="!collapsed"
          class="title-text"
        >{{ settingSafe.logoText }}</span>
        <span
          v-else
          class="title-text collapsed"
        >{{ settingSafe.logoCollapsedText }}</span>
      </div>
      <div class="sider-menu-scroll">
        <TaktSideMenu
          :collapsed="collapsed"
        />
      </div>
    </a-layout-sider>
    <a-layout :style="{ marginLeft: settingSafe.fixedSider ? (collapsed ? settingSafe.siderCollapsedWidth + 'px' : settingSafe.siderWidth + 'px') : 0 }">
      <TaktHeader
        v-model:collapsed="collapsed"
        :fixed="settingSafe.fixedHeader"
        :height="headerHeight"
        :left-offset="settingSafe.fixedSider ? (collapsed ? settingSafe.siderCollapsedWidth + 'px' : settingSafe.siderWidth + 'px') : '0px'"
      />
      <div 
        v-if="settingSafe.showTabs"
        :style="settingSafe.fixedHeader ? {
          position: 'fixed',
          top: `${headerHeight}px`,
          left: settingSafe.fixedSider ? (collapsed ? settingSafe.siderCollapsedWidth + 'px' : settingSafe.siderWidth + 'px') : 0,
          right: 0,
          zIndex: 99
        } : {
          marginTop: 0,
          marginLeft: 0
        }"
      >
        <TaktTabs />
      </div>
      <a-layout-content 
        :class="['layout-content', 'layout-content-fixed', `content-width-${settingSafe.contentWidth}`]"
        :style="{
          marginTop: settingSafe.fixedHeader ? `${headerHeight + tabsBarHeight}px` : `${tabsBarHeight}px`,
          marginBottom: '2px',
          maxWidth: settingSafe.contentWidth === 'fixed' ? '1200px' : 'none',
          marginLeft: settingSafe.contentWidth === 'fixed' ? 'auto' : '0',
          marginRight: settingSafe.contentWidth === 'fixed' ? 'auto' : '0',
          height: contentMaxHeight,
          maxHeight: contentMaxHeight,
          overflow: 'hidden'
        }"
      >
        <div class="layout-content-inner">
          <RouterView :key="businessScopeRouteKey" />
        </div>
      </a-layout-content>
      <TaktFooter :height="40" />
    </a-layout>
  </a-layout>
</template>

<script setup lang="ts">
import { storeToRefs } from 'pinia'
import { useThemeStore } from '@/stores/common/useThemeStore'
import { defaultSetting, useSettingStore } from '@/stores/common/useSettingStore'
import { useUserStore } from '@/stores/identity/useUserStore'
import { useMenuStore } from '@/stores/identity/useMenuStore'
import { ensureMenuAndRoutesLoaded } from '@/router'
import { useBusinessScopeRouteKey } from '@/composables/useBusinessScopeRouteKey'
type HeaderHeight = 32 | 40 | 48

const themeStore = useThemeStore()
const { setting } = storeToRefs(useSettingStore())
const userStore = useUserStore()
const menuStore = useMenuStore()
/** 租户/公司切换时重建 RouterView，触发各业务页重新拉数 */
const businessScopeRouteKey = useBusinessScopeRouteKey()

// 首次登录后若布局先于守卫完成挂载，可能出现左侧菜单为空；挂载时补救一次
onMounted(async () => {
  if (userStore.token && (!menuStore.menuList || menuStore.menuList.length === 0)) {
    await ensureMenuAndRoutesLoaded()
  }
})
const settingSafe = computed(() => setting.value ?? defaultSetting)
/** 标签栏占位高度（关闭「显示标签页」时为 0，避免内容区顶出空白） */
const tabsBarHeight = computed(() => (settingSafe.value.showTabs ? 40 : 0))

const collapsed = ref(false)
const logoError = ref(false)

// Logo 图片 URL
const logoUrl = computed(() => {
  const s = settingSafe.value
  const logoPath = s.logo
  const showLogo = s.showLogo
  if (!showLogo) return null
  if (!logoPath || logoPath.trim() === '') return null
  try {
    if (logoPath.startsWith('@/')) return logoPath.replace('@/', '/src/')
    if (logoPath.startsWith('/')) return logoPath
    return `/src/${logoPath}`
  } catch {
    return null
  }
})

// Logo 图片加载错误处理
const handleLogoError = () => {
  logoError.value = true
}

// Logo 图片加载成功处理（预留，可扩展）
const handleLogoLoad = () => {}

// Header 高度，可以从设置中获取，默认 40px
const headerHeight = computed(() => {
  const headerHeightValue = (settingSafe.value as Record<string, unknown>)?.headerHeight
  if (headerHeightValue === 32 || headerHeightValue === 40 || headerHeightValue === 48) {
    return headerHeightValue
  }
  return 40 as HeaderHeight
})

// Content 最大高度计算（用于滚动条）
const contentMaxHeight = computed(() => {
  const footerAndGap = 44
  if (settingSafe.value.fixedHeader) {
    return `calc(100vh - ${headerHeight.value}px - ${tabsBarHeight.value}px - ${footerAndGap}px)`
  }
  return `calc(100vh - ${tabsBarHeight.value}px - ${footerAndGap}px)`
})
</script>

<style scoped lang="css">
.side-layout {
  height: 100vh;

  .layout-sider :deep(.ant-layout-sider-children) {
    display: flex;
    flex-direction: column;
    height: 100%;
    overflow: hidden;
  }

  .sider-menu-scroll {
    flex: 1;
    min-height: 0;
    overflow-x: hidden;
    overflow-y: auto;
  }

  .sider-header {
    display: flex;
    align-items: center;
    justify-content: center;
    height: 48px;
    padding: 0 16px;
    background: var(--ant-color-primary);
    transition: all 0.2s;
    gap: 8px;

    .logo-image {
      width: 32px;
      height: 32px;
      min-width: 32px;
      min-height: 32px;
      flex-shrink: 0;
      object-fit: contain;
      display: block !important;
      visibility: visible !important;
      opacity: 1 !important;
      background: transparent;
    }

    .title-text {
      color: var(--ant-color-text-inverse);
      font-size: 18px;
      font-weight: bold;
      white-space: nowrap;
      overflow: hidden;
      text-overflow: ellipsis;
      transition: all 0.2s;

      &.collapsed {
        font-size: 20px;
      }
    }

    &.collapsed {
      .title-text:not(.collapsed) {
        display: none;
      }
    }
  }
}
</style>

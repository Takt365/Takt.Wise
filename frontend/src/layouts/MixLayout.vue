<!-- ========================================
项目名称：节拍智控·TaktWise
命名空间：@/layouts
文件名称：MixLayout.vue
创建时间：2025-01-20
创建人：Takt365(Cursor AI)
功能描述：混合布局，顶部导航+侧边菜单和主内容区

版权信息：Copyright (c) 2025 Takt  All rights reserved.
免责声明：此软件使用 MIT License，作者不承担任何使用风险。
======================================== -->

<template>
  <a-layout class="mix-layout">
    <!-- 顶部导航栏 -->
    <TaktHeader
      v-model:collapsed="collapsed"
      :fixed="settingSafe.fixedHeader"
      :height="headerHeight"
      left-offset="0px"
    >
      <template #left>
        <div class="header-title">
          <img
            v-if="logoUrl && settingSafe.showLogo && !logoError"
            :src="logoUrl"
            :alt="settingSafe.logoText"
            class="logo-image"
            @error="handleLogoError"
            @load="handleLogoLoad"
          >
          <span class="title-text">{{ settingSafe.logoText }}</span>
        </div>
        <TaktBreadcrumb />
      </template>
    </TaktHeader>
    
    <div 
      v-if="settingSafe.showTabs"
      :style="settingSafe.fixedHeader ? {
        position: 'fixed',
        top: `${headerHeight}px`,
        left: 0,
        right: 0,
        zIndex: 99
      } : {
        marginTop: 0
      }"
    >
      <TaktTabs />
    </div>
    
    <a-layout :style="{ marginTop: settingSafe.fixedHeader ? `${headerHeight + tabsBarHeight}px` : `${tabsBarHeight}px` }">
      <a-layout-sider
        v-model:collapsed="collapsed"
        :width="settingSafe.siderWidth"
        :collapsed-width="settingSafe.siderCollapsedWidth"
        class="layout-sider"
        :theme="themeStore.resolvedTheme === 'dark' ? 'dark' : 'light'"
        :style="{ position: settingSafe.fixedSider ? 'fixed' : 'relative', height: `calc(100vh - ${headerHeight}px)`, left: 0, top: settingSafe.fixedHeader ? `${headerHeight}px` : 0 }"
      >
        <div class="sider-menu-scroll">
          <TaktMixMenu
            :collapsed="collapsed"
          />
        </div>
      </a-layout-sider>
      
      <!-- 内容区域 -->
      <a-layout :style="{ marginLeft: settingSafe.fixedSider ? (collapsed ? settingSafe.siderCollapsedWidth + 'px' : settingSafe.siderWidth + 'px') : 0 }">
        <a-layout-content
          :class="['layout-content', `content-width-${settingSafe.contentWidth}`]"
          :style="{ marginBottom: '2px', maxWidth: settingSafe.contentWidth === 'fixed' ? '1200px' : 'none', marginLeft: settingSafe.contentWidth === 'fixed' ? 'auto' : '0', marginRight: settingSafe.contentWidth === 'fixed' ? 'auto' : '0', maxHeight: contentMaxHeight }"
        >
          <RouterView :key="businessScopeRouteKey" />
        </a-layout-content>
        <TaktFooter :height="40" />
      </a-layout>
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

onMounted(async () => {
  if (userStore.token && (!menuStore.menuList || menuStore.menuList.length === 0)) {
    await ensureMenuAndRoutesLoaded()
  }
})
const settingSafe = computed(() => setting.value ?? defaultSetting)
/** 标签栏占位高度（关闭「显示标签页」时为 0） */
const tabsBarHeight = computed(() => (settingSafe.value.showTabs ? 40 : 0))
const logoError = ref(false)
const collapsed = ref(false)

const handleLogoError = () => {
  logoError.value = true
}

const handleLogoLoad = () => {}

const logoUrl = computed(() => {
  const s = settingSafe.value
  const logoPath = s.logo
  if (!logoPath || logoPath.trim() === '') return null
  try {
    if (logoPath.startsWith('@/')) return logoPath.replace('@/', '/src/')
    if (logoPath.startsWith('/')) return logoPath
    return `/src/${logoPath}`
  } catch {
    return null
  }
})

const headerHeight = computed(() => {
  const headerHeightValue = (settingSafe.value as Record<string, unknown>)?.headerHeight
  if (headerHeightValue === 32 || headerHeightValue === 40 || headerHeightValue === 48) {
    return headerHeightValue
  }
  return 40 as HeaderHeight
})

const contentMaxHeight = computed(() => {
  const footerAndGap = 44
  return `calc(100vh - ${tabsBarHeight.value}px - ${footerAndGap}px)`
})
</script>

<style scoped lang="css">
.mix-layout {
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

  .header-title {
    display: flex;
    align-items: center;
    height: 48px;
    gap: 8px;
    margin-right: 16px;

    .logo-image {
      width: 32px;
      height: 32px;
      object-fit: contain;
    }

    .title-text {
      font-size: 18px;
      font-weight: bold;
      color: var(--ant-color-text);
      white-space: nowrap;
    }
  }
}
</style>

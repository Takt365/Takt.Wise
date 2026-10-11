<!-- ========================================
项目名称：节拍智控·TaktWise
命名空间：frontend/src/layouts
文件名称：index.vue
创建时间：2025-01-20
创建人：Takt365(Cursor AI)
功能描述：主布局入口，根据偏好设置切换侧边栏/顶部/混合布局

版权信息：Copyright (c) 2025 Takt  All rights reserved.
免责声明：此软件使用 MIT License，作者不承担任何使用风险。
======================================== -->

<template>
  <TaktWatermark>
    <TaktForceLogoutBanner />
    <side-layout v-if="settingSafe.layout === 'side'" />
    <top-layout v-else-if="settingSafe.layout === 'top'" />
    <mix-layout v-else-if="settingSafe.layout === 'mix'" />
    <side-layout v-else />
  </TaktWatermark>
</template>

<script setup lang="ts">
import { storeToRefs } from 'pinia';
import { defaultSetting, useSettingStore } from '@/stores/common/useSettingStore';
import { useEventBus } from '@/utils/eventBus';
import { createLogger } from '@/utils/logger';
import { ensureMenuAndRoutesLoaded } from '@/router';
import TaktWatermark from '@/components/navigation/TaktWatermark/index.vue';
import SideLayout from './SideLayout.vue';
import TopLayout from './TopLayout.vue';
import MixLayout from './MixLayout.vue';

const layoutLogger = createLogger('layout');
const { setting } = storeToRefs(useSettingStore());
const settingSafe = computed(() => setting.value ?? defaultSetting);
const { on, off } = useEventBus();

/**
 * 登录成功后确保菜单与动态路由就绪
 */
async function handleLoginSuccess(): Promise<void> {
  await ensureMenuAndRoutesLoaded();
  layoutLogger.info('用户登录成功，菜单路由已就绪', { action: 'login' });
}

/**
 * 语言切换
 * @param {{ locale: string }} payload 语言代码
 */
function handleLocaleChange(payload: { locale: string }): void {
  layoutLogger.info('语言已切换', { action: 'locale-change', locale: payload.locale });
}

/**
 * 主题切换
 * @param {{ theme: 'light' | 'dark' }} payload 主题
 */
function handleThemeChange(payload: { theme: 'light' | 'dark' }): void {
  layoutLogger.info('主题已切换', { action: 'theme-change', theme: payload.theme });
}

/**
 * 菜单刷新后重建动态路由
 */
async function handleMenuRefresh(): Promise<void> {
  await ensureMenuAndRoutesLoaded(true);
}

onMounted(() => {
  on('user:login', handleLoginSuccess);
  on('locale:change', handleLocaleChange);
  on('theme:change', handleThemeChange);
  on('menu:refresh', handleMenuRefresh);
});

onUnmounted(() => {
  off('user:login', handleLoginSuccess);
  off('locale:change', handleLocaleChange);
  off('theme:change', handleThemeChange);
  off('menu:refresh', handleMenuRefresh);
});
</script>

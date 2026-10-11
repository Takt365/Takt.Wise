<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src -->
<!-- 文件名称：App.vue -->
<!-- 功能描述：根组件，包含路由视图和全局布局 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-config-provider :locale="locale" :theme="antdTheme">
    <a-app class="h-full">
      <TaktThemeSync />
      <router-view />
    </a-app>
  </a-config-provider>
</template>

<script setup lang="ts">
/**
 * 根组件
 * 提供全局 ConfigProvider（国际化、主题算法）与 CSS 变量同步
 */
import { theme as antdThemeApi } from 'ant-design-vue';
import type { ThemeConfig } from 'ant-design-vue/es/config-provider/context';
import { useTaktComponentLocale } from '@/composables/useTaktComponentLocale';
import { useThemeStore } from '@/stores/common/useThemeStore';
import { useThemeColorStore } from '@/stores/common/useThemeColorStore';
import { useSettingStore } from '@/stores/common/useSettingStore';

const { antDesignVueLocale } = useTaktComponentLocale();
const themeStore = useThemeStore();
const themeColorStore = useThemeColorStore();
const { setting } = storeToRefs(useSettingStore());

/** Ant Design Vue ConfigProvider 语言包（与 vue-i18n 同步） */
const locale = antDesignVueLocale;

/**
 * ConfigProvider 主题（仅依赖明暗/主色/圆角等基元；避免 patchSetting 换新 setting 对象就整树重算）
 */
const antdTheme = shallowRef<ThemeConfig>({
  algorithm: antdThemeApi.defaultAlgorithm,
  token: {},
});

watch(
  () =>
    [
      themeStore.resolvedTheme,
      themeColorStore.colorPrimary,
      setting.value.borderRadius,
    ] as const,
  ([resolved, colorPrimary, borderRadius]) => {
    const algorithm =
      resolved === 'dark'
        ? antdThemeApi.darkAlgorithm
        : antdThemeApi.defaultAlgorithm;
    const prev = antdTheme.value;
    if (
      prev.algorithm === algorithm
      && prev.token?.colorPrimary === colorPrimary
      && prev.token?.borderRadius === borderRadius
    ) {
      return;
    }
    antdTheme.value = {
      algorithm,
      token: {
        colorPrimary,
        borderRadius,
      },
    };
  },
  { immediate: true },
);
</script>

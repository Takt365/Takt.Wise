<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/components/common/TaktThemeSync -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：监听 Ant Design Vue token 并同步为全局 CSS 变量 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <span class="hidden" aria-hidden="true" />
</template>

<script setup lang="ts">
/**
 * 将 ConfigProvider 上下文中的 Design Token 同步到 :root CSS 变量
 */
import { theme } from 'ant-design-vue';
import { syncAntDesignCssVariables } from '@/utils/theme';

const { token } = theme.useToken();

/**
 * 监听会随明暗/主色变化的关键 token；不全量 deep 订阅 Object.keys(token)
 * 实际写入仍走 sync（内含同值跳过）
 */
watch(
  () =>
    [
      token.value.colorPrimary,
      token.value.colorBgContainer,
      token.value.colorText,
      token.value.colorBorder,
    ] as const,
  () => {
    syncAntDesignCssVariables(token.value);
  },
  { immediate: true, flush: 'post' },
);
</script>

<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/error -->
<!-- 文件名称：coming-soon.vue -->
<!-- 功能描述：菜单占位页；业务未实现时展示「即将推出」（与 401/403/404 同属 error） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <ErrorResult
    status="info"
    :title="pageTitle"
    subtitle-key="error.page.comingsoon.subtitle"
    :show-back="true"
    :show-home="true"
  />
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import ErrorResult from './components/ErrorResult.vue'

const { t } = useI18n()
/** 当前路由 */
const route = useRoute()
/** 页面标题（取自菜单 titleKey；缺省用占位文案） */
const pageTitle = computed(() => {
  const titleKey = route.meta.titleKey
  return typeof titleKey === 'string' && titleKey.trim() ? t(titleKey) : t('error.page.comingsoon.title')
})
</script>

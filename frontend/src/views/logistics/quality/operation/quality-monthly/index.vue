<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/quality-monthly -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：品质月报页面占位（菜单 logistics:quality:operation:monthly:list）；业务报表待接入 -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 flex-col p-4">
    <div class="mb-3 text-base font-medium text-text">
      {{ t('logistics.quality.operation.quality-monthly.page.title') }}
    </div>
    <a-empty :description="t('logistics.quality.operation.quality-monthly.page.placeholder')" />
  </div>
</template>

<script setup lang="ts">
/**
 * 品质月报占位页
 */
import { useI18n } from 'vue-i18n'

const { t } = useI18n()
</script>

<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/dashboard/dataBoard.types/modules -->
<!-- 文件名称：StatsCustomModule.vue -->
<!-- 功能描述：数据看板自定义模块占位（dashboard 空态卡） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div
    class="takt-stats-metric-card takt-stats-metric-card--dashboard takt-stats-metric-card--dashboard-compact relative flex min-h-[72px] items-center justify-center rounded-[10px] border border-border px-3 py-5"
    style="--stats-metric-accent: var(--takt-memorial-gray)"
  >
    <span
      class="takt-stats-metric-card__top-accent"
      aria-hidden="true"
    />
    <a-typography-paragraph class="mb-0 text-center text-sm text-text-secondary">
      {{ t('dashboard.data-board.page.customplaceholder') }}
    </a-typography-paragraph>
  </div>
</template>

<script setup lang="ts">
/**
 * 自定义模块占位：与 dashboard KPI 卡同圆角/顶色条，无业务指标
 */
import { useI18n } from 'vue-i18n'

const { t } = useI18n()
</script>

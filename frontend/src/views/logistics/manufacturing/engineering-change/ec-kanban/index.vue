<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-kanban -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变看板三页签：看板（ECharts 统计）、实施（执行勾选表）、批次（阶段日期批次） -->
<!-- 版权信息：Copyright (c) 2026 Takt All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="ec-list-layout box-border flex w-full min-w-0 max-w-full flex-col overflow-x-hidden p-4">
    <TaktPaneTabs
      v-model:active-key="kanbanTab"
      :items="kanbanTabItems"
      destroy-inactive-tab-pane
    >
      <template #board>
        <EcKanbanStatsPanel />
      </template>
      <template #impl>
        <EcKanbanImplPanel />
      </template>
      <template #batch>
        <EcKanbanBatchPanel />
      </template>
    </TaktPaneTabs>
  </div>
</template>

<script setup lang="ts">
/**
 * 设变看板：看板统计 / 实施明细 / 投入批次
 */
import '../ec-list-layout.css'
import { useI18n } from 'vue-i18n'
import type { TaktPaneTabItem } from '@/components/business/TaktPaneTabs/index.vue'
import EcKanbanStatsPanel from './components/EcKanbanStatsPanel.vue'
import EcKanbanImplPanel from './components/EcKanbanImplPanel.vue'
import EcKanbanBatchPanel from './components/EcKanbanBatchPanel.vue'

const { t } = useI18n()
const localePrefix = 'logistics.manufacturing.engineering-change.ec-kanban.page'
/** 页签：board 看板，impl 实施，batch 批次 */
const kanbanTab = ref('board')
/** 看板页签定义（可按 show/permission 删减） */
const kanbanTabItems = computed<TaktPaneTabItem[]>(() => [
  { key: 'board', label: t(`${localePrefix}.tabs.board`) },
  { key: 'impl', label: t(`${localePrefix}.tabs.impl`) },
  { key: 'batch', label: t(`${localePrefix}.tabs.batch`) },
])
</script>

<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-kanban -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变看板：看板统计 + 批次/采购/受检/部管/物料（后四者以本部门完整列 + 列设置） -->
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
      <template #production>
        <EcKanbanBatchPanel panel-key="production" />
      </template>
      <template #pur>
        <EcKanbanDeptDetailPanel panel-key="pur" />
      </template>
      <template #iqc>
        <EcKanbanDeptDetailPanel panel-key="iqc" />
      </template>
      <template #mc>
        <EcKanbanDeptDetailPanel panel-key="mc" />
      </template>
      <template #mtl>
        <EcKanbanDeptDetailPanel panel-key="mtl" />
      </template>
    </TaktPaneTabs>
  </div>
</template>

<script setup lang="ts">
/**
 * 设变看板：看板统计 + 批次（生产组）+ 采购/受检/部管/物料完整列
 */
import '../ec-list-layout.css'
import { useI18n } from 'vue-i18n'
import type { TaktPaneTabItem } from '@/components/business/TaktPaneTabs/index.vue'
import EcKanbanStatsPanel from './components/EcKanbanStatsPanel.vue'
import EcKanbanBatchPanel from './components/EcKanbanBatchPanel.vue'
import EcKanbanDeptDetailPanel from './components/EcKanbanDeptDetailPanel.vue'

const { t } = useI18n()
const localePrefix = 'logistics.manufacturing.engineering-change.ec-kanban.page'
/** 页签：board 看板，production 批次，其余为本部门完整列 */
const kanbanTab = ref('board')
/** 看板页签定义 */
const kanbanTabItems = computed<TaktPaneTabItem[]>(() => [
  { key: 'board', label: t(`${localePrefix}.tabs.board`) },
  { key: 'production', label: t(`${localePrefix}.tabs.production`) },
  { key: 'pur', label: t(`${localePrefix}.tabs.pur`) },
  { key: 'iqc', label: t(`${localePrefix}.tabs.iqc`) },
  { key: 'mc', label: t(`${localePrefix}.tabs.mc`) },
  { key: 'mtl', label: t(`${localePrefix}.tabs.mtl`) },
])
</script>

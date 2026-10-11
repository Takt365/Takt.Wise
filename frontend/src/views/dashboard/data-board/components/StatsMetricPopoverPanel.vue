<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/dashboard/dataBoard.types/components -->
<!-- 文件名称：StatsMetricPopoverPanel.vue -->
<!-- 功能描述：数据看板指标 Popover 预览列表（空态 + 查看全部） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div
    class="stats-metric-popover-panel"
    :style="panelStyle"
  >
    <a-spin :spinning="loading">
      <a-empty
        v-if="!loading && rows.length === 0"
        :description="emptyText"
        :image="Empty.PRESENTED_IMAGE_SIMPLE"
      />
      <a-list
        v-else
        size="small"
        :split="false"
        :data-source="rows"
      >
        <template #renderItem="{ item }">
          <a-list-item class="stats-metric-popover-panel__item">
            <a-list-item-meta>
              <template #title>
                <span class="stats-metric-popover-panel__title">{{ item.title }}</span>
              </template>
              <template
                v-if="item.description"
                #description
              >
                <span class="stats-metric-popover-panel__desc">{{ item.description }}</span>
              </template>
            </a-list-item-meta>
          </a-list-item>
        </template>
      </a-list>
      <div
        v-if="showViewAll && total > 0"
        class="stats-metric-popover-panel__footer"
      >
        <a-button
          type="link"
          size="small"
          @click="emit('view-all')"
        >
          {{ viewAllText }}
          <span v-if="total > rows.length"> ({{ total }})</span>
        </a-button>
      </div>
    </a-spin>
  </div>
</template>

<script setup lang="ts">
/**
 * 数据看板指标 Popover 内容面板
 */
import { Empty } from 'ant-design-vue'

/** Popover 列表行 */
export interface StatsMetricPopoverRow {
  title: string
  description?: string
}

const props = defineProps<{
  /** 预览行 */
  rows: StatsMetricPopoverRow[]
  /** 服务端总数 */
  total: number
  /** 加载中 */
  loading?: boolean
  /** 空态文案 */
  emptyText: string
  /** 查看全部文案 */
  viewAllText: string
  /** 是否显示查看全部 */
  showViewAll?: boolean
  /** 著名色 CSS 变量（Popover 顶栏与列表强调） */
  accentColor?: string
}>()

/** Popover 面板行内样式 */
const panelStyle = computed(() => {
  if (!props.accentColor?.trim()) {
    return undefined
  }
  return { '--stats-popover-accent': props.accentColor }
})

const emit = defineEmits<{
  'view-all': []
}>()
</script>

<style scoped lang="css">
.stats-metric-popover-panel {
  width: 280px;
  max-width: min(280px, calc(100vw - 32px));
}
.stats-metric-popover-panel__item {
  padding-inline: 0 !important;
}
.stats-metric-popover-panel__title {
  display: block;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  color: var(--ant-color-text);
  font-size: 13px;
}
.stats-metric-popover-panel__desc {
  display: block;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  font-size: 12px;
  color: var(--ant-color-text-secondary);
}
.stats-metric-popover-panel__footer {
  margin-top: 4px;
  text-align: right;
  border-top: 1px solid var(--ant-color-border-secondary);
  padding-top: 4px;
}
.stats-metric-popover-panel__footer :deep(.ant-btn-link) {
  color: var(--stats-popover-accent, var(--ant-color-primary));
}
</style>

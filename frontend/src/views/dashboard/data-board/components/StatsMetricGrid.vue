<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/dashboard/dataBoard.types/components -->
<!-- 文件名称：StatsMetricGrid.vue -->
<!-- 功能描述：数据看板统计指标栅格（著名色强调 + Popover 预览） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="stats-metric-grid">
    <a-spin :spinning="loading">
      <p
        v-if="periodLabel"
        class="stats-metric-grid__period"
      >
        {{ periodLabel }}
      </p>
      <a-row :gutter="gutter">
        <a-col
          v-for="item in items"
          :key="item.key"
          :xs="colXs"
          :sm="colSm"
          :md="colMd"
          :lg="colLg"
        >
          <a-popover
            v-if="isMetricPopoverEnabled(item)"
            trigger="click"
            placement="bottom"
            :overlay-class-name="'stats-metric-grid-popover'"
            :overlay-inner-style="resolvePopoverInnerStyle(item)"
            destroy-tooltip-on-hide
            :get-popup-container="getPopoverContainer"
            @open-change="(open: boolean) => handlePopoverOpenChange(item, open)"
          >
            <template #content>
              <slot
                name="popover"
                :item="item"
              />
            </template>
            <StatsMetricCard
              :item="item"
              :card-variant="cardVariant"
              :clickable="true"
              :value-style="valueStyle"
            />
          </a-popover>
          <div
            v-else
            class="stats-metric-grid__wrapper"
            :role="!usePopover && isMetricNavigable(item) ? 'button' : undefined"
            :tabindex="!usePopover && isMetricNavigable(item) ? 0 : undefined"
            @click="!usePopover ? handleMetricClick(item) : undefined"
            @keydown.enter="!usePopover ? handleMetricKeydown($event, item) : undefined"
            @keydown.space.prevent="!usePopover ? handleMetricKeydown($event, item) : undefined"
          >
            <StatsMetricCard
              :item="item"
              :card-variant="cardVariant"
              :clickable="!usePopover && isMetricNavigable(item)"
              :value-style="valueStyle"
            />
          </div>
        </a-col>
      </a-row>
    </a-spin>
  </div>
</template>

<script setup lang="ts">
/**
 * 数据看板 a-statistic 指标栅格（著名色 + Popover / 直跳）
 */
import type { CSSProperties, Component } from 'vue'
import { useRouter } from 'vue-router'
import { usePermissionStore } from '@/stores/identity/usePermissionStore'
import StatsMetricCard from './StatsMetricCard.vue'

/** 单个统计指标 */
export interface StatsMetricItem {
  key: string
  title: string
  value: number
  prefix?: string
  suffix?: string
  precision?: number
  /** 自定义数值文案（有则取代 a-statistic 数字） */
  valueText?: string
  /** 悬停提示（如总金额） */
  tooltip?: string
  /** 点击跳转路由（须与菜单 RoutePath 一致） */
  routePath?: string
  /** 著名色 CSS 变量（如 var(--takt-klein-blue)） */
  accentColor?: string
  /** dashboard 布局左侧图标 */
  icon?: Component
}

const router = useRouter()
const permissionStore = usePermissionStore()

const props = withDefaults(defineProps<{
  /** 指标列表 */
  items: StatsMetricItem[]
  /** 加载中 */
  loading?: boolean
  /** 使用 Popover 预览（否则点击直跳） */
  usePopover?: boolean
  /** 周期说明（如「统计周期：本月」） */
  periodLabel?: string
  /** 卡片布局：default 左色条；dashboard 概览大卡；dashboard-compact 业务紧凑卡 */
  cardVariant?: 'default' | 'dashboard' | 'dashboard-compact'
  /** 栅格 gutter */
  gutter?: number | [number, number]
  /** 响应式列宽 */
  colXs?: number
  colSm?: number
  colMd?: number
  colLg?: number
  /** 数值样式 */
  valueStyle?: CSSProperties
}>(), {
  loading: false,
  usePopover: false,
  periodLabel: '',
  cardVariant: 'default',
  gutter: 16,
  colXs: 12,
  colSm: 8,
  colMd: 8,
  colLg: 8,
  valueStyle: () => ({ fontSize: '20px', lineHeight: '1.2' }),
})

const emit = defineEmits<{
  'popover-open-change': [key: string, open: boolean]
}>()

/**
 * 指标是否可访问目标路由
 * @param item 指标项
 * @returns {boolean} 是否可导航
 */
function isMetricNavigable(item: StatsMetricItem): boolean {
  const path = item.routePath?.trim()
  if (!path) {
    return false
  }
  return permissionStore.canAccess(path)
}

/**
 * 是否启用 Popover
 * @param item 指标项
 * @returns {boolean} 是否启用
 */
function isMetricPopoverEnabled(item: StatsMetricItem): boolean {
  return props.usePopover && isMetricNavigable(item)
}

/**
 * Popover 挂载容器（避免卡片 overflow 裁剪）
 * @returns {HTMLElement} body
 */
function getPopoverContainer(): HTMLElement {
  return document.body
}

/**
 * Popover 内层样式（著名色顶栏）
 * @param item 指标项
 * @returns {Record<string, string>} overlay inner style
 */
function resolvePopoverInnerStyle(item: StatsMetricItem): Record<string, string> | undefined {
  if (!item.accentColor?.trim()) {
    return undefined
  }
  return {
    '--stats-popover-accent': item.accentColor,
    borderTop: `3px solid ${item.accentColor}`,
    paddingTop: '8px',
  }
}

/**
 * 点击指标跳转关联视图（非 Popover 模式）
 * @param item 指标项
 * @returns {void}
 */
function handleMetricClick(item: StatsMetricItem): void {
  const path = item.routePath?.trim()
  if (!path || !permissionStore.canAccess(path)) {
    return
  }
  void router.push(path)
}

/**
 * 键盘激活指标跳转
 * @param event 键盘事件
 * @param item 指标项
 * @returns {void}
 */
function handleMetricKeydown(event: KeyboardEvent, item: StatsMetricItem): void {
  if (event.key !== 'Enter' && event.key !== ' ') {
    return
  }
  handleMetricClick(item)
}

/**
 * Popover 显隐
 * @param item 指标项
 * @param open 是否打开
 * @returns {void}
 */
function handlePopoverOpenChange(item: StatsMetricItem, open: boolean): void {
  if (!isMetricPopoverEnabled(item)) {
    return
  }
  emit('popover-open-change', item.key, open)
}
</script>

<style scoped lang="css">
.stats-metric-grid {
  min-height: 64px;
}
.stats-metric-grid__period {
  margin: 0 0 8px;
  font-size: 12px;
  line-height: 1.25;
  color: var(--ant-color-text-secondary);
}
.stats-metric-grid__wrapper {
  display: block;
}
.stats-metric-grid :deep(.ant-col) {
  display: flex;
}
.stats-metric-grid :deep(.ant-popover-open),
.stats-metric-grid :deep(.takt-stats-metric-card) {
  width: 100%;
}
</style>

<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/dashboard/dataBoard.types/components -->
<!-- 文件名称：StatsMetricCard.vue -->
<!-- 功能描述：数据看板 KPI 单卡（默认 / 仪表盘 icon 布局） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-tooltip
    :title="item.tooltip"
    :trigger="item.tooltip ? ['hover'] : []"
  >
    <div
      class="takt-stats-metric-card"
      :class="cardClass"
      :style="cellStyle"
    >
      <span
        v-if="showTopAccent"
        class="takt-stats-metric-card__top-accent"
        aria-hidden="true"
      />
      <div
        v-if="isDashboardLayout && item.icon"
        class="takt-stats-metric-card__icon-wrap"
        aria-hidden="true"
      >
        <component
          :is="item.icon"
          class="takt-stats-metric-card__icon"
        />
      </div>
      <div class="takt-stats-metric-card__content">
        <template v-if="item.valueText">
          <div class="ant-statistic">
            <div class="ant-statistic-title">
              {{ item.title }}
            </div>
            <div
              class="ant-statistic-content"
              :style="resolvedValueStyle"
            >
              {{ item.valueText }}
            </div>
          </div>
        </template>
        <a-statistic
          v-else
          :title="item.title"
          :value="displayValue"
          :prefix="item.prefix"
          :suffix="displaySuffix"
          :precision="displayPrecision"
          :value-style="resolvedValueStyle"
        />
      </div>
    </div>
  </a-tooltip>
</template>

<script setup lang="ts">
/**
 * KPI 指标单卡：default 左色条；dashboard 顶色条 + 左侧图标区（概览六项）
 * 货币金额（prefix 含 ¥）按量级自动缩放为亿/万/千；支持 valueText / tooltip
 */
import type { CSSProperties, Component } from 'vue'
import { useI18n } from 'vue-i18n'
import {
  isDashboardCurrencyPrefix,
  scaleDashboardCurrencyAmount,
} from '../utils/formatDashboardAmount'

/** 单个统计指标（含可选图标） */
export interface StatsMetricCardItem {
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
  routePath?: string
  accentColor?: string
  icon?: Component
}

const props = withDefaults(defineProps<{
  item: StatsMetricCardItem
  /** default：左色条；dashboard：概览大卡；dashboard-compact：业务模块紧凑卡 */
  cardVariant?: 'default' | 'dashboard' | 'dashboard-compact'
  clickable?: boolean
  valueStyle?: CSSProperties
}>(), {
  cardVariant: 'default',
  clickable: false,
  valueStyle: () => ({ fontSize: '20px', lineHeight: '1.2' }),
})

const { t } = useI18n()

/** 货币金额缩放结果；非货币为 null */
const scaledAmount = computed(() => {
  if (!isDashboardCurrencyPrefix(props.item.prefix)) {
    return null
  }
  return scaleDashboardCurrencyAmount(props.item.value, props.item.precision ?? 2)
})

/** 展示数值（金额已按量级缩放） */
const displayValue = computed(() => scaledAmount.value?.value ?? props.item.value)

/** 展示小数位 */
const displayPrecision = computed(() => scaledAmount.value?.precision ?? props.item.precision)

/** 展示后缀：单位（万/百万…）+ 原有 suffix */
const displaySuffix = computed(() => {
  const base = props.item.suffix ?? ''
  const unitKey = scaledAmount.value?.unitKey
  if (!unitKey) {
    return base || undefined
  }
  const unit = t(`dashboard.data-board.page.amountunit.${unitKey}`)
  return `${unit}${base}`
})

/** 是否仪表盘 icon 布局（概览 / 紧凑） */
const isDashboardLayout = computed(() =>
  props.cardVariant === 'dashboard' || props.cardVariant === 'dashboard-compact',
)

/** 是否业务模块紧凑布局 */
const isCompactDashboard = computed(() => props.cardVariant === 'dashboard-compact')

/** 卡片 class */
const cardClass = computed(() => ({
  'takt-stats-metric-card--dashboard': isDashboardLayout.value,
  'takt-stats-metric-card--dashboard-compact': isCompactDashboard.value,
  'takt-stats-metric-card--accent': !isDashboardLayout.value && !!props.item.accentColor,
  'takt-stats-metric-card--clickable': props.clickable,
}))

/** 是否显示顶色条（dashboard）或左色条（default） */
const showTopAccent = computed(() => isDashboardLayout.value && !!props.item.accentColor)

/** 著名色 CSS 变量 */
const cellStyle = computed(() => {
  if (!props.item.accentColor) {
    return undefined
  }
  return { '--stats-metric-accent': props.item.accentColor }
})

/** 数值样式：compact 用 clamp；dashboard 用大号字；default 可用强调色 */
const resolvedValueStyle = computed((): CSSProperties => {
  const base = { ...props.valueStyle }
  if (isCompactDashboard.value) {
    base.fontSize = 'clamp(13px, 5cqi, 17px)'
    base.lineHeight = '1.15'
    base.fontWeight = 700
    base.color = 'var(--ant-color-text)'
    return base
  }
  if (isDashboardLayout.value) {
    base.fontSize = '22px'
    base.lineHeight = '1.2'
    base.fontWeight = 700
    base.color = 'var(--ant-color-text)'
    return base
  }
  if (props.item.accentColor) {
    base.color = props.item.accentColor
  }
  return base
})
</script>

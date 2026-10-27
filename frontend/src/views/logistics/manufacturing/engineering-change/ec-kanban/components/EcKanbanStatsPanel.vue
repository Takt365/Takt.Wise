<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-kanban/components -->
<!-- 文件名称：EcKanbanStatsPanel.vue -->
<!-- 功能描述：设变看板·看板页签：KPI + 实施状态饼图 + 部门实施柱图 + 按月折线 -->
<!-- 版权信息：Copyright (c) 2026 Takt All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="box-border flex w-full min-w-0 max-w-full flex-col gap-3 overflow-x-hidden">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="t('common.page.form.placeholder.search')"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    >
      <template #leading>
        <span class="whitespace-nowrap text-sm text-text-secondary">{{ t('logistics.manufacturing.engineering-change.ec-stat.page.entryPeriodRange') }}</span>
        <a-range-picker
          v-model:value="entryPeriod"
          picker="month"
          format="YYYY-MM"
          value-format="YYYY-MM"
          class="w-64 min-w-56"
          :placeholder="[
            t('logistics.manufacturing.engineering-change.ec-stat.page.entryPeriodRange'),
            t('logistics.manufacturing.engineering-change.ec-stat.page.entryPeriodRange'),
          ]"
          :disabled-date="entryPeriodLimit.disabledDate"
          @calendar-change="entryPeriodLimit.onCalendarChange"
          @open-change="entryPeriodLimit.onOpenChange"
        />
      </template>
    </TaktQueryBar>

    <a-spin :spinning="loading">
      <div class="grid grid-cols-2 gap-3 md:grid-cols-5">
        <div
          v-for="card in kpiCards"
          :key="card.key"
          class="rounded border border-border bg-container px-3 py-3"
        >
          <div class="text-xs text-text-secondary">{{ card.title }}</div>
          <div class="mt-1 text-2xl font-semibold tabular-nums text-text">{{ card.value }}</div>
        </div>
      </div>

      <div class="mt-3 grid grid-cols-1 gap-3 xl:grid-cols-2">
        <div class="rounded border border-border bg-container p-3">
          <div class="mb-2 text-sm font-medium text-text">{{ t(`${localePrefix}.charts.statusTitle`) }}</div>
          <div ref="statusChartEl" class="h-72 w-full min-w-0" />
        </div>
        <div class="rounded border border-border bg-container p-3">
          <div class="mb-2 text-sm font-medium text-text">{{ t(`${localePrefix}.charts.monthTitle`) }}</div>
          <div ref="monthChartEl" class="h-72 w-full min-w-0" />
        </div>
        <div class="rounded border border-border bg-container p-3 xl:col-span-2">
          <div class="mb-2 text-sm font-medium text-text">{{ t(`${localePrefix}.charts.deptTitle`) }}</div>
          <div ref="deptChartEl" class="h-80 w-full min-w-0" />
        </div>
      </div>
    </a-spin>
  </div>
</template>

<script setup lang="ts">
/**
 * 设变看板统计页签：KPI 与三张 ECharts
 */
import { useI18n } from 'vue-i18n'
import * as echarts from 'echarts/core'
import { BarChart, LineChart, PieChart } from 'echarts/charts'
import {
  GridComponent,
  LegendComponent,
  TitleComponent,
  TooltipComponent,
} from 'echarts/components'
import { CanvasRenderer } from 'echarts/renderers'
import type { ECharts, EChartsCoreOption } from 'echarts/core'
import { useAnalysisPeriodRangeLimit } from '@/composables/useAnalysisPeriodRangeLimit'
import { useTaktComponentLocale } from '@/composables/useTaktComponentLocale'
import { useThemeStore } from '@/stores/common/useThemeStore'
import { useThemeColorStore } from '@/stores/common/useThemeColorStore'
import { getEcKanbanBoardStat } from '@/api/logistics/manufacturing/engineering-change/ecKanban.api'
import type { EcKanbanBoardStat } from '@/types/logistics/manufacturing/engineering-change/ecKanban.types'
import {
  TAKT_ANALYSIS_PERIOD_MAX_MONTHS,
  buildDefaultThroughCurrentPeriodRange,
} from '@/views/logistics/manufacturing/bom/material-cost/utils/bomMaterialCostPeriod'
import { TaktEcDeptCodes } from '@/constants/logistics/ecDeptCodes.constants'

echarts.use([
  PieChart,
  BarChart,
  LineChart,
  GridComponent,
  LegendComponent,
  TitleComponent,
  TooltipComponent,
  CanvasRenderer,
])

const { t, locale } = useI18n()
const { echartsLocale } = useTaktComponentLocale()
const themeStore = useThemeStore()
const themeColorStore = useThemeColorStore()
const localePrefix = 'logistics.manufacturing.engineering-change.ec-kanban.page'

/** 列表 loading */
const loading = ref(false)
/** 关键词 */
const queryKeyword = ref('')
/** 录入期间 */
const entryPeriod = ref<[string, string] | null>(buildDefaultThroughCurrentPeriodRange(3))
/** 录入期间限制 */
const entryPeriodLimit = useAnalysisPeriodRangeLimit(TAKT_ANALYSIS_PERIOD_MAX_MONTHS, { allowCurrentMonth: true })
/** 统计数据 */
const boardStat = ref<EcKanbanBoardStat>({
  ecTotal: 0,
  notStarted: 0,
  inProgress: 0,
  officiallyCompleted: 0,
  fullyCompleted: 0,
  depts: [],
  months: [],
})

/** 状态饼图容器 */
const statusChartEl = ref<HTMLDivElement | null>(null)
/** 按月折线容器 */
const monthChartEl = ref<HTMLDivElement | null>(null)
/** 部门柱图容器 */
const deptChartEl = ref<HTMLDivElement | null>(null)
/** 状态饼图实例 */
let statusChart: ECharts | null = null
/** 按月折线实例 */
let monthChart: ECharts | null = null
/** 部门柱图实例 */
let deptChart: ECharts | null = null

/** 部门列顺序 */
const DEPT_ORDER = [
  TaktEcDeptCodes.Pc,
  TaktEcDeptCodes.Pur,
  TaktEcDeptCodes.Iqc,
  TaktEcDeptCodes.Mc,
  TaktEcDeptCodes.Mtl,
  TaktEcDeptCodes.Mfg2,
  TaktEcDeptCodes.Mfg1,
  TaktEcDeptCodes.Qc,
  TaktEcDeptCodes.Me,
] as const

/** 部门编码 → 文案键 */
const DEPT_TITLE_KEY: Record<string, string> = {
  [TaktEcDeptCodes.Pc]: 'pc',
  [TaktEcDeptCodes.Pur]: 'pur',
  [TaktEcDeptCodes.Iqc]: 'iqc',
  [TaktEcDeptCodes.Mc]: 'mc',
  [TaktEcDeptCodes.Mtl]: 'mtl',
  [TaktEcDeptCodes.Mfg2]: 'mfg2',
  [TaktEcDeptCodes.Mfg1]: 'mfg1',
  [TaktEcDeptCodes.Qc]: 'qc',
  [TaktEcDeptCodes.Me]: 'me',
}

/** KPI 卡片 */
const kpiCards = computed(() => [
  { key: 'total', title: t(`${localePrefix}.kpi.ecTotal`), value: boardStat.value.ecTotal },
  { key: 'notStarted', title: t(`${localePrefix}.implementationStatus.notStarted`), value: boardStat.value.notStarted },
  { key: 'inProgress', title: t(`${localePrefix}.implementationStatus.inProgress`), value: boardStat.value.inProgress },
  { key: 'official', title: t(`${localePrefix}.implementationStatus.officiallyCompleted`), value: boardStat.value.officiallyCompleted },
  { key: 'fully', title: t(`${localePrefix}.implementationStatus.fullyCompleted`), value: boardStat.value.fullyCompleted },
])

/**
 * 录入期间转查询日期
 * @param range 年月区间
 * @returns 起止日期
 */
function entryPeriodToQuery(range: [string, string] | null): { ecEntryDateStart?: string; ecEntryDateEnd?: string } {
  if (!range?.[0] || !range?.[1]) {
    return {}
  }
  const parts = range[1].split('-').map(Number)
  const year = parts[0]
  const month = parts[1]
  if (!year || !month) {
    return { ecEntryDateStart: `${range[0]}-01` }
  }
  const lastDay = new Date(year, month, 0).getDate()
  return {
    ecEntryDateStart: `${range[0]}-01`,
    ecEntryDateEnd: `${range[1]}-${String(lastDay).padStart(2, '0')}`,
  }
}

/**
 * 文本色
 * @returns CSS 颜色
 */
function textColor(): string {
  return getComputedStyle(document.documentElement).getPropertyValue('--ant-color-text').trim() || '#1f1f1f'
}

/**
 * 次要文本色
 * @returns CSS 颜色
 */
function mutedColor(): string {
  return getComputedStyle(document.documentElement).getPropertyValue('--ant-color-text-secondary').trim() || '#8c8c8c'
}

/**
 * 边框色
 * @returns CSS 颜色
 */
function borderColor(): string {
  return getComputedStyle(document.documentElement).getPropertyValue('--ant-color-border').trim() || '#d9d9d9'
}

/**
 * 主色
 * @returns CSS 颜色
 */
function primaryColor(): string {
  return themeColorStore.colorPrimary || '#1677ff'
}

/**
 * 渲染状态饼图
 */
function renderStatusChart() {
  const el = statusChartEl.value
  if (!el) {
    return
  }
  if (!statusChart) {
    statusChart = echarts.init(el, undefined, { locale: echartsLocale.value })
  }
  const option: EChartsCoreOption = {
    color: [mutedColor(), primaryColor(), '#52c41a', '#13c2c2'],
    tooltip: { trigger: 'item' },
    legend: {
      bottom: 0,
      textStyle: { color: textColor() },
    },
    series: [
      {
        type: 'pie',
        radius: ['42%', '68%'],
        center: ['50%', '46%'],
        avoidLabelOverlap: true,
        itemStyle: { borderRadius: 4, borderColor: borderColor(), borderWidth: 1 },
        label: { color: textColor() },
        data: [
          { name: t(`${localePrefix}.implementationStatus.notStarted`), value: boardStat.value.notStarted },
          { name: t(`${localePrefix}.implementationStatus.inProgress`), value: boardStat.value.inProgress },
          { name: t(`${localePrefix}.implementationStatus.officiallyCompleted`), value: boardStat.value.officiallyCompleted },
          { name: t(`${localePrefix}.implementationStatus.fullyCompleted`), value: boardStat.value.fullyCompleted },
        ],
      },
    ],
  }
  statusChart.setOption(option, true)
}

/**
 * 渲染按月折线
 */
function renderMonthChart() {
  const el = monthChartEl.value
  if (!el) {
    return
  }
  if (!monthChart) {
    monthChart = echarts.init(el, undefined, { locale: echartsLocale.value })
  }
  const months = boardStat.value.months ?? []
  const option: EChartsCoreOption = {
    color: [primaryColor()],
    tooltip: { trigger: 'axis' },
    grid: { left: 40, right: 16, top: 24, bottom: 32 },
    xAxis: {
      type: 'category',
      data: months.map((item) => item.yearMonth),
      axisLabel: { color: mutedColor() },
      axisLine: { lineStyle: { color: borderColor() } },
    },
    yAxis: {
      type: 'value',
      minInterval: 1,
      axisLabel: { color: mutedColor() },
      splitLine: { lineStyle: { color: borderColor(), type: 'dashed' } },
    },
    series: [
      {
        type: 'line',
        smooth: true,
        areaStyle: { opacity: 0.12 },
        data: months.map((item) => item.ecCount),
        name: t(`${localePrefix}.kpi.ecTotal`),
      },
    ],
  }
  monthChart.setOption(option, true)
}

/**
 * 渲染部门柱图
 */
function renderDeptChart() {
  const el = deptChartEl.value
  if (!el) {
    return
  }
  if (!deptChart) {
    deptChart = echarts.init(el, undefined, { locale: echartsLocale.value })
  }
  const byCode = new Map((boardStat.value.depts ?? []).map((item) => [item.deptCode, item]))
  const labels = DEPT_ORDER.map((code) => t(`${localePrefix}.column.${DEPT_TITLE_KEY[code] ?? code}`))
  const implemented = DEPT_ORDER.map((code) => byCode.get(code)?.implemented ?? 0)
  const notImplemented = DEPT_ORDER.map((code) => byCode.get(code)?.notImplemented ?? 0)
  const option: EChartsCoreOption = {
    color: [primaryColor(), mutedColor()],
    tooltip: { trigger: 'axis', axisPointer: { type: 'shadow' } },
    legend: {
      top: 0,
      textStyle: { color: textColor() },
    },
    grid: { left: 48, right: 16, top: 36, bottom: 40 },
    xAxis: {
      type: 'category',
      data: labels,
      axisLabel: { color: mutedColor(), interval: 0, rotate: 20 },
      axisLine: { lineStyle: { color: borderColor() } },
    },
    yAxis: {
      type: 'value',
      minInterval: 1,
      axisLabel: { color: mutedColor() },
      splitLine: { lineStyle: { color: borderColor(), type: 'dashed' } },
    },
    series: [
      {
        type: 'bar',
        name: t(`${localePrefix}.charts.implemented`),
        stack: 'dept',
        barMaxWidth: 36,
        data: implemented,
      },
      {
        type: 'bar',
        name: t(`${localePrefix}.charts.notImplemented`),
        stack: 'dept',
        barMaxWidth: 36,
        data: notImplemented,
      },
    ],
  }
  deptChart.setOption(option, true)
}

/**
 * 刷新全部图表
 */
function renderAllCharts() {
  renderStatusChart()
  renderMonthChart()
  renderDeptChart()
}

/**
 * 加载统计
 */
async function loadData() {
  loading.value = true
  try {
    boardStat.value = await getEcKanbanBoardStat({
      keyWords: queryKeyword.value.trim() || undefined,
      ...entryPeriodToQuery(entryPeriod.value),
    })
    await nextTick()
    renderAllCharts()
  } finally {
    loading.value = false
  }
}

/** 查询 */
function handleSearch() {
  void loadData()
}

/** 重置（关键字由 TaktQueryBar 清空） */
function handleReset() {
  entryPeriod.value = buildDefaultThroughCurrentPeriodRange(3)
  void loadData()
}

/**
 * 窗口尺寸变化时重绘
 */
function handleResize() {
  statusChart?.resize()
  monthChart?.resize()
  deptChart?.resize()
}

useTableRefresh(loadData)

onMounted(() => {
  void loadData()
  window.addEventListener('resize', handleResize)
})

onBeforeUnmount(() => {
  window.removeEventListener('resize', handleResize)
  statusChart?.dispose()
  monthChart?.dispose()
  deptChart?.dispose()
  statusChart = null
  monthChart = null
  deptChart = null
})

watch([locale, echartsLocale], async () => {
  statusChart?.dispose()
  monthChart?.dispose()
  deptChart?.dispose()
  statusChart = null
  monthChart = null
  deptChart = null
  await nextTick()
  renderAllCharts()
})

watch(
  [() => themeStore.resolvedTheme, () => themeColorStore.colorPrimary],
  () => {
    renderAllCharts()
  },
)
</script>

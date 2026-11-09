<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-kanban/components -->
<!-- 文件名称：EcKanbanStatsPanel.vue -->
<!-- 功能描述：设变看板·看板页签：KPI + 旭日图 + 各部门实施状态柱 + 按月折线 -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
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
        <a-range-picker
          v-model:value="entryPeriod"
          picker="month"
          format="YYYY-MM"
          value-format="YYYY-MM"
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
        <div class="rounded border border-border bg-container p-3 xl:col-span-2">
          <div class="mb-2 text-sm font-medium text-text">{{ t(`${localePrefix}.charts.sunburstTitle`) }}</div>
          <div ref="sunburstChartEl" class="h-[28rem] w-full min-w-0" />
        </div>
        <div class="rounded border border-border bg-container p-3 xl:col-span-2">
          <div class="mb-2 text-sm font-medium text-text">{{ t(`${localePrefix}.charts.statusTitle`) }}</div>
          <div ref="statusChartEl" class="h-[28rem] w-full min-w-0" />
        </div>
        <div class="rounded border border-border bg-container p-3 xl:col-span-2">
          <div class="mb-2 text-sm font-medium text-text">{{ t(`${localePrefix}.charts.monthTitle`) }}</div>
          <div ref="monthChartEl" class="h-72 w-full min-w-0" />
        </div>
      </div>
    </a-spin>
  </div>
</template>

<script setup lang="ts">
/**
 * 设变看板统计页签：KPI、旭日图、各部门实施状态柱、按月折线
 */
import { useI18n } from 'vue-i18n'
import * as echarts from 'echarts/core'
import { BarChart, LineChart, SunburstChart } from 'echarts/charts'
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
  BarChart,
  LineChart,
  SunburstChart,
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
  detailTotal: 0,
  notStarted: 0,
  inProgress: 0,
  officiallyCompleted: 0,
  fullyCompleted: 0,
  depts: [],
  months: [],
})

/** 旭日图容器 */
const sunburstChartEl = ref<HTMLDivElement | null>(null)
/** 状态饼图容器 */
const statusChartEl = ref<HTMLDivElement | null>(null)
/** 按月折线容器 */
const monthChartEl = ref<HTMLDivElement | null>(null)
/** 旭日图实例 */
let sunburstChart: ECharts | null = null
/** 状态饼图实例 */
let statusChart: ECharts | null = null
/** 按月折线实例 */
let monthChart: ECharts | null = null

/** 图表执行部门顺序：与实施/批次页签一致（不含制技） */
const DEPT_ORDER = [
  TaktEcDeptCodes.Pc,
  TaktEcDeptCodes.Mfg2,
  TaktEcDeptCodes.Mfg1,
  TaktEcDeptCodes.Qc,
  TaktEcDeptCodes.Pur,
  TaktEcDeptCodes.Iqc,
  TaktEcDeptCodes.Mc,
  TaktEcDeptCodes.Mtl,
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
 * 读取 CSS 变量（优先 color-base / Ant Design token）
 * @param varName 如 --takt-cn-baolan
 * @param fallback 未定义时的兜底色
 * @returns 解析后的颜色值
 */
function cssColor(varName: string, fallback: string): string {
  const raw = getComputedStyle(document.documentElement).getPropertyValue(varName).trim()
  return raw || fallback
}

/**
 * 文本色
 * @returns CSS 颜色
 */
function textColor(): string {
  return cssColor('--ant-color-text', '#1f1f1f')
}

/**
 * 次要文本色
 * @returns CSS 颜色
 */
function mutedColor(): string {
  return cssColor('--ant-color-text-secondary', '#8c8c8c')
}

/**
 * 边框色
 * @returns CSS 颜色
 */
function borderColor(): string {
  return cssColor('--ant-color-border', '#d9d9d9')
}

/**
 * 主色
 * @returns CSS 颜色
 */
function primaryColor(): string {
  return themeColorStore.colorPrimary || cssColor('--takt-klein-blue', '#002fa7')
}

/**
 * 部门旭日配色：一律取自 color-base.css（base=部门 / done=已实施 / open=未实施）
 * @param code 部门编码
 * @returns 解析后的三色
 */
function deptSunburstPalette(code: string): { base: string; done: string; open: string } {
  /** [baseToken, doneToken, openToken] → color-base 变量 */
  const tokenMap: Record<string, readonly [string, string, string]> = {
    [TaktEcDeptCodes.Pc]: ['--takt-cn-baolan', '--takt-klein-blue', '--takt-cn-lanse'],
    [TaktEcDeptCodes.Pur]: ['--takt-cn-zise', '--takt-cn-qinglian', '--takt-cn-xueqing'],
    [TaktEcDeptCodes.Iqc]: ['--takt-cn-feicui', '--takt-mars-green', '--takt-tiffany-blue'],
    [TaktEcDeptCodes.Mc]: ['--takt-cn-chengse', '--takt-cn-juhuang', '--takt-cn-jianghuang'],
    [TaktEcDeptCodes.Mtl]: ['--takt-cn-youlv', '--takt-cn-songbeilv', '--takt-cn-nenlv'],
    [TaktEcDeptCodes.Mfg2]: ['--takt-jp-murasaki', '--takt-jp-wakamurasaki', '--takt-jp-usumurasaki'],
    [TaktEcDeptCodes.Mfg1]: ['--takt-cn-zhusha', '--takt-chinese-red', '--takt-cn-fenhong'],
    [TaktEcDeptCodes.Qc]: ['--takt-cn-caolv', '--takt-jp-moegi', '--takt-cn-liulv'],
  }
  const fallback: readonly [string, string, string] = [
    '--takt-cn-dianqing',
    '--takt-cn-zangqing',
    '--takt-cn-weilan',
  ]
  const tokens = tokenMap[code] ?? fallback
  return {
    base: cssColor(tokens[0], '#4b5cc4'),
    done: cssColor(tokens[1], '#002fa7'),
    open: cssColor(tokens[2], '#70f3ff'),
  }
}

/** sunburst 节点 */
type EcKanbanSunburstNode = {
  name: string
  value?: number
  itemStyle?: { color: string }
  children?: EcKanbanSunburstNode[]
}

/**
 * 扇区标签数字：主表/明细用真实记录数；其余用节点 value（子项合计由 ECharts 汇总）
 * @param name 节点名
 * @param value 节点 value
 * @returns 显示用数字文本
 */
function sunburstLabelNumber(name: string | undefined, value: number | undefined): string {
  if (name === t(`${localePrefix}.charts.masterRing`)) {
    return String(Math.max(0, boardStat.value.ecTotal))
  }
  if (name === t(`${localePrefix}.charts.detailRing`)) {
    return String(Math.max(0, boardStat.value.detailTotal))
  }
  return String(Number(value ?? 0))
}

/**
 * 按部门显示名反查统计行
 * @param deptLabel 部门标签（如 生管课）
 * @returns 部门统计；未命中 null
 */
function findDeptStatByLabel(deptLabel: string) {
  const code = DEPT_ORDER.find(
    (item) => t(`${localePrefix}.column.${DEPT_TITLE_KEY[item] ?? item}`) === deptLabel,
  )
  if (!code) {
    return null
  }
  return (boardStat.value.depts ?? []).find((item) => item.deptCode === code) ?? null
}

/**
 * 从路径解析当前部门显示名（跳过主表/明细/实施/未实施）
 * @param pathNames 路径节点名
 * @returns 部门名；无则空串
 */
function resolveDeptLabelFromPath(pathNames: string[]): string {
  const masterName = t(`${localePrefix}.charts.masterRing`)
  const detailName = t(`${localePrefix}.charts.detailRing`)
  const doneName = t(`${localePrefix}.charts.implemented`)
  const openName = t(`${localePrefix}.charts.notImplemented`)
  const skip = new Set([masterName, detailName, doneName, openName])
  return pathNames.find((name) => name && !skip.has(name)) ?? ''
}

/**
 * 旭日图滑过全量提示：路径 + 主表/明细行数 + 部门行数（含已实施/未实施）
 * @param params ECharts tooltip params
 * @returns HTML 提示文本
 */
function formatCountChartTooltip(params: unknown): string {
  const p = params as { name?: string; value?: number; treePathInfo?: Array<{ name: string }> }
  const pathNames = (p.treePathInfo ?? []).map((x) => x.name).filter(Boolean)
  const path = pathNames.join(' / ') || (p.name ?? '')
  const lines: string[] = [path]
  lines.push(`${t(`${localePrefix}.charts.tooltipMaster`)}: ${Math.max(0, boardStat.value.ecTotal)}`)
  lines.push(`${t(`${localePrefix}.charts.tooltipDetail`)}: ${Math.max(0, boardStat.value.detailTotal)}`)
  const deptLabel = resolveDeptLabelFromPath(pathNames.length > 0 ? pathNames : [p.name ?? ''])
  if (deptLabel) {
    const row = findDeptStatByLabel(deptLabel)
    const done = Math.max(0, row?.implemented ?? 0)
    const open = Math.max(0, row?.notImplemented ?? 0)
    const total = done + open
    lines.push(`${t(`${localePrefix}.charts.tooltipDept`)}: ${deptLabel}`)
    lines.push(`${t(`${localePrefix}.charts.tooltipDeptRows`)}: ${total}`)
    lines.push(`${t(`${localePrefix}.charts.tooltipImplemented`)}: ${done}`)
    lines.push(`${t(`${localePrefix}.charts.tooltipNotImplemented`)}: ${open}`)
  }
  return lines.join('<br/>')
}

/**
 * 单部门节点（对齐官方示例）
 * - 有实施拆分：只挂 children（不写父 value，避免与子合计冲突叠扇区）
 * - 无拆分：仅 value
 * @param code 部门编码
 * @param done 已实施记录数
 * @param open 未实施记录数
 * @returns 部门节点；无记录时 null
 */
function buildDeptCountNode(code: string, done: number, open: number): EcKanbanSunburstNode | null {
  const safeDone = Math.max(0, done)
  const safeOpen = Math.max(0, open)
  const total = safeDone + safeOpen
  if (total <= 0) {
    return null
  }
  const palette = deptSunburstPalette(code)
  const node: EcKanbanSunburstNode = {
    name: t(`${localePrefix}.column.${DEPT_TITLE_KEY[code] ?? code}`),
    itemStyle: { color: palette.base },
  }
  const leaves: EcKanbanSunburstNode[] = []
  if (safeDone > 0) {
    leaves.push({
      name: t(`${localePrefix}.charts.implemented`),
      value: safeDone,
      itemStyle: { color: palette.done },
    })
  }
  if (safeOpen > 0) {
    leaves.push({
      name: t(`${localePrefix}.charts.notImplemented`),
      value: safeOpen,
      itemStyle: { color: palette.open },
    })
  }
  if (leaves.length > 0) {
    // 与示例「采购」同构：部门扇区面积由实施+未实施之和决定
    node.children = leaves
  } else {
    node.value = total
  }
  return node
}

/**
 * 旭日图数据（严格对齐官方示例：主表/明细无 value，仅结构环）
 * 主表 → 明细 → 各部门 → 实施/未实施
 * @returns ECharts sunburst data
 */
function buildSunburstData(): EcKanbanSunburstNode[] {
  const byCode = new Map((boardStat.value.depts ?? []).map((item) => [item.deptCode, item]))
  const deptNodes = DEPT_ORDER.map((code) => {
    const row = byCode.get(code)
    return buildDeptCountNode(code, row?.implemented ?? 0, row?.notImplemented ?? 0)
  }).filter((node): node is EcKanbanSunburstNode => node != null)
  return [
    {
      name: t(`${localePrefix}.charts.masterRing`),
      children: [
        {
          name: t(`${localePrefix}.charts.detailRing`),
          children: deptNodes,
        },
      ],
    },
  ]
}

/**
 * 渲染旭日图：扇区上显示数字，滑过 tooltip 显示文本路径
 */
function renderSunburstChart() {
  const el = sunburstChartEl.value
  if (!el) {
    return
  }
  if (!sunburstChart) {
    sunburstChart = echarts.init(el, undefined, { locale: echartsLocale.value })
  }
  const option: EChartsCoreOption = {
    tooltip: {
      trigger: 'item',
      formatter: formatCountChartTooltip,
    },
    series: {
      type: 'sunburst',
      data: buildSunburstData(),
      radius: [0, '90%'],
      sort: undefined,
      label: {
        // 正向水平显示，避免 radial 左旋 90°
        rotate: 0,
        color: textColor(),
        minAngle: 4,
        formatter: (params: unknown) => {
          const p = params as { name?: string; value?: number }
          const name = p.name ?? ''
          const masterName = t(`${localePrefix}.charts.masterRing`)
          const detailName = t(`${localePrefix}.charts.detailRing`)
          const doneName = t(`${localePrefix}.charts.implemented`)
          const openName = t(`${localePrefix}.charts.notImplemented`)
          // 中圈执行部门：显示名称；主表/明细/实施/未实施：显示数字
          if (
            name === masterName
            || name === detailName
            || name === doneName
            || name === openName
          ) {
            return sunburstLabelNumber(name, p.value)
          }
          return name
        },
      },
      itemStyle: {
        borderWidth: 1,
        borderColor: borderColor(),
      },
    },
  }
  sunburstChart.setOption(option, true)
}

/**
 * 渲染各部门实施状态：横向分组柱（已实施行 / 未实施行并排，数值标在柱上）
 * 完成语义仍由后端 Ec* 提供；本图直接对照行级实施/未实施，避免堆叠难辨
 */
function renderStatusChart() {
  const el = statusChartEl.value
  if (!el) {
    return
  }
  if (!statusChart) {
    statusChart = echarts.init(el, undefined, { locale: echartsLocale.value })
  }
  const byCode = new Map((boardStat.value.depts ?? []).map((item) => [item.deptCode, item]))
  const categories = DEPT_ORDER.map((code) => t(`${localePrefix}.column.${DEPT_TITLE_KEY[code] ?? code}`))
  const implemented = DEPT_ORDER.map((code) => Math.max(0, byCode.get(code)?.implemented ?? 0))
  const notImplemented = DEPT_ORDER.map((code) => Math.max(0, byCode.get(code)?.notImplemented ?? 0))
  const doneLabel = t(`${localePrefix}.charts.implemented`)
  const openLabel = t(`${localePrefix}.charts.notImplemented`)
  const option: EChartsCoreOption = {
    color: [
      cssColor('--takt-cn-youlv', '#00bc12'),
      cssColor('--takt-cn-chengse', '#fa8c35'),
    ],
    tooltip: {
      trigger: 'axis',
      axisPointer: { type: 'shadow' },
      formatter: (params: unknown) => {
        const list = Array.isArray(params) ? params : [params]
        const rows = list as Array<{ seriesName?: string; name?: string; value?: number; marker?: string }>
        if (rows.length === 0) {
          return ''
        }
        const title = String(rows[0]?.name ?? '')
        const lines = rows.map((item) => `${item.marker ?? ''}${item.seriesName}: ${Number(item.value ?? 0)}`)
        const done = Number(rows.find((item) => item.seriesName === doneLabel)?.value ?? 0)
        const open = Number(rows.find((item) => item.seriesName === openLabel)?.value ?? 0)
        const total = done + open
        const rate = total > 0 ? `${Math.round((done * 100) / total)}%` : '—'
        return [title, ...lines, `${t(`${localePrefix}.charts.tooltipDeptRows`)}: ${total}`, `${t(`${localePrefix}.implementationStatus.deptCompleted`)}: ${rate}`].join('<br/>')
      },
    },
    legend: {
      top: 0,
      textStyle: { color: textColor() },
    },
    grid: { left: 72, right: 48, top: 36, bottom: 24 },
    // 横向：部门在 Y，已实施/未实施并排，高低一目了然
    yAxis: {
      type: 'category',
      data: categories,
      inverse: true,
      axisLabel: { color: textColor() },
      axisLine: { lineStyle: { color: borderColor() } },
      axisTick: { show: false },
    },
    xAxis: {
      type: 'value',
      minInterval: 1,
      axisLabel: { color: mutedColor() },
      splitLine: { lineStyle: { color: borderColor(), type: 'dashed' } },
    },
    series: [
      {
        type: 'bar',
        name: doneLabel,
        data: implemented,
        barGap: '20%',
        barCategoryGap: '35%',
        label: {
          show: true,
          position: 'right',
          color: textColor(),
          formatter: (params: unknown) => {
            const p = params as { value?: number }
            const value = Number(p.value ?? 0)
            return value > 0 ? String(value) : ''
          },
        },
        itemStyle: { borderRadius: [0, 4, 4, 0], color: cssColor('--takt-cn-youlv', '#00bc12') },
      },
      {
        type: 'bar',
        name: openLabel,
        data: notImplemented,
        label: {
          show: true,
          position: 'right',
          color: textColor(),
          formatter: (params: unknown) => {
            const p = params as { value?: number }
            const value = Number(p.value ?? 0)
            return value > 0 ? String(value) : ''
          },
        },
        itemStyle: { borderRadius: [0, 4, 4, 0], color: cssColor('--takt-cn-chengse', '#fa8c35') },
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
 * 刷新全部图表
 */
function renderAllCharts() {
  renderSunburstChart()
  renderStatusChart()
  renderMonthChart()
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
  sunburstChart?.resize()
  statusChart?.resize()
  monthChart?.resize()
}

useTableRefresh(loadData)

onMounted(() => {
  void loadData()
  window.addEventListener('resize', handleResize)
})

onBeforeUnmount(() => {
  window.removeEventListener('resize', handleResize)
  sunburstChart?.dispose()
  statusChart?.dispose()
  monthChart?.dispose()
  sunburstChart = null
  statusChart = null
  monthChart = null
})

watch([locale, echartsLocale], async () => {
  sunburstChart?.dispose()
  statusChart?.dispose()
  monthChart?.dispose()
  sunburstChart = null
  statusChart = null
  monthChart = null
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

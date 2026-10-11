<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/ipqc-trend/components -->
<!-- 文件名称：IpqcTrendPanel.vue -->
<!-- 功能描述：过程质量推移转置表（工厂×工序×月份不良率） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 flex-1 flex-col overflow-hidden">
    <div
      ref="tableWrapRef"
      class="min-h-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        analysis-table
        class="h-full min-h-0"
        entity-scope="company"
        table-mode="single"
        :columns="columns"
        :visible-column-keys="visibleColumnKeys"
        :id-column-key="'processCode'"
        :data-source="rows"
        :loading="loading"
        :stripe="true"
        :row-key="getRowKey"
        :pagination="false"
        :show-row-selection="false"
        :scroll="{ x: 'max-content', y: tableScrollY }"
      >
        <template #bodyCell="{ column, record, text }">
          <template v-if="String(column.key).startsWith('period_')">
            {{ formatPeriodDefectRate(record as IpqcOrderMonthlyTrend, String(column.key)) }}
          </template>
          <template v-else-if="column.key === 'trend'">
            <span :class="trendClass((record as IpqcOrderMonthlyTrend).trend || 'none')">
              {{ trendLabel((record as IpqcOrderMonthlyTrend).trend || 'none') }}
            </span>
          </template>
          <template v-else-if="column.key === 'varianceAmount'">
            <span :class="varianceClass((record as IpqcOrderMonthlyTrend).varianceAmount)">
              {{ formatRateDelta((record as IpqcOrderMonthlyTrend).varianceAmount) }}
            </span>
          </template>
          <template v-else-if="column.key === 'variancePercent'">
            <span :class="varianceClass((record as IpqcOrderMonthlyTrend).varianceAmount)">
              {{ formatPercent((record as IpqcOrderMonthlyTrend).variancePercent) }}
            </span>
          </template>
          <template v-else>
            {{ text }}
          </template>
        </template>
        <template #footerRemark>
          <div class="space-y-1">
            <div class="truncate text-sm text-text-secondary">
              {{ summaryText }}
            </div>
            <div
              v-if="hasQuery && comparePeriod"
              class="text-sm text-text"
            >
              {{ t(`${localePrefix}.trendSummary`, {
                base: basePeriod || '—',
                compare: comparePeriod,
                up: upCount,
                down: downCount,
                flat: flatCount,
              }) }}
            </div>
          </div>
        </template>
      </TaktSingleTable>
    </div>
    <TaktPagination
      v-if="total > 0"
      class="mt-2 shrink-0"
      :current="pageIndex"
      :page-size="pageSize"
      :total="total"
      @change="handlePageChange"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 过程质量推移转置分析表
 */
import { formatAnalysisVariancePercent } from '@/utils/analysisPercent'
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import {
  exportIpqcOrderMonthlyTrendAnalysis,
  getIpqcOrderMonthlyTrendAnalysis,
} from '@/api/logistics/quality/operation/ipqcOrderTrend.api'
import type { IpqcOrderMonthlyTrend } from '@/types/logistics/quality/operation/inspectionTrend.types'
import { useTableRefresh } from '@/composables/useTableRefresh'
import {
  ensureTaktPaginationConfigAsync,
  getTaktDefaultPageIndex,
  getTaktDefaultPageSize,
} from '@/utils/taktPaged'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { taktTrendExportFileNames } from '@/utils/naming'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/tableScroll'

const props = defineProps<{
  /** 涨跌筛选 */
  trendFilter?: string
  /** 工厂代码 */
  plantCode?: string
  /** 年月区间 */
  periodRange?: [string, string] | null
  /** 工序编码 */
  processCode?: string
}>()

const loading = defineModel<boolean>('loading', { default: false })
const hasRows = defineModel<boolean>('hasRows', { default: false })

const localePrefix = 'logistics.quality.operation.ipqc-trend.page'
const { t } = useI18n()

/** 行数据 */
const rows = ref<IpqcOrderMonthlyTrend[]>([])
/** 期间列 */
const periodOrder = ref<string[]>([])
/** 行总数 */
const rowCount = ref(0)
/** 环比基准月 */
const basePeriod = ref('')
/** 环比对比月 */
const comparePeriod = ref('')
/** 涨跌计数 */
const upCount = ref(0)
const downCount = ref(0)
const flatCount = ref(0)
/** 分页 */
const pageIndex = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
/** 表体外壳 */
const tableWrapRef = ref<HTMLElement | null>(null)
/** 表体 scroll.y */
const tableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
/** ResizeObserver */
let tableScrollResizeObserver: ResizeObserver | null = null

const hasQuery = computed(() => !!props.plantCode?.trim())

const summaryText = computed(() => {
  if (!props.plantCode?.trim()) {
    return t(`${localePrefix}.selectPlantRequired`)
  }
  return t(`${localePrefix}.summary`, { count: rowCount.value })
})

const columns = computed<TableColumnsType>(() => {
  const cols: TableColumnsType = [
    {
      title: t('common.page.entity.plantcode'),
      dataIndex: 'plantCode',
      key: 'plantCode',
      width: 90,
      fixed: 'left',
    },
    {
      title: t('entity.ipqcorder.processcode'),
      dataIndex: 'processCode',
      key: 'processCode',
      width: 120,
      ellipsis: true,
      fixed: 'left',
    },
    {
      title: t('entity.ipqcorder.processname'),
      dataIndex: 'processName',
      key: 'processName',
      width: 160,
      ellipsis: true,
    }]
  for (const period of periodOrder.value) {
    cols.push({
      title: period,
      dataIndex: ['periodDefectRates', period],
      key: `period_${period}`,
      width: 100,
      align: 'right',
    })
  }
  cols.push(
    {
      title: t(`${localePrefix}.columns.trend`),
      dataIndex: 'trend',
      key: 'trend',
      width: 80,
      fixed: 'right',
    },
    {
      title: t(`${localePrefix}.columns.varianceAmount`),
      dataIndex: 'varianceAmount',
      key: 'varianceAmount',
      width: 100,
      align: 'right',
      fixed: 'right',
    },
    {
      title: t(`${localePrefix}.columns.variancePercent`),
      dataIndex: 'variancePercent',
      key: 'variancePercent',
      width: 90,
      align: 'right',
      fixed: 'right',
    },
  )
  return cols
})

const visibleColumnKeys = computed(() => columns.value.map((c) => String(c.key)))

/**
 * 行主键
 * @param {IpqcOrderMonthlyTrend} record 行
 * @returns {string} key
 */
function getRowKey(record: IpqcOrderMonthlyTrend): string {
  return `${record.plantCode}|${record.processCode}`
}

/**
 * 格式化不良率差额（比率）
 * @param {number | null | undefined} value 数值
 * @returns {string} 文本
 */
function formatRateDelta(value?: number | null): string {
  if (value == null || Number.isNaN(value)) return '—'
  return `${(value * 100).toFixed(2)}%`
}

/**
 * 格式化环比变动率
 * @param {number | null | undefined} value 比率
 * @returns {string} 百分比文本
 */
function formatPercent(value?: number | null): string {
  return formatAnalysisVariancePercent(value, 'ratio')
}

/**
 * 涨跌文案
 * @param {string} trend 涨跌码
 * @returns {string} 文案
 */
function trendLabel(trend: string): string {
  return t(`${localePrefix}.trend.${trend || 'none'}`)
}

/**
 * 涨跌样式（不良率上升为红）
 * @param {string} trend 涨跌码
 * @returns {string} class
 */
function trendClass(trend: string): string {
  if (trend === 'up') return 'text-red-500'
  if (trend === 'down') return 'text-green-600'
  return 'text-text-secondary'
}

/**
 * 差额样式
 * @param {number | null | undefined} amount 差额
 * @returns {string} class
 */
function varianceClass(amount?: number | null): string {
  if (amount == null || amount === 0) return 'text-text-secondary'
  return amount > 0 ? 'text-red-500' : 'text-green-600'
}

/**
 * 格式化期间不良率
 * @param {IpqcOrderMonthlyTrend} record 行
 * @param {string} columnKey period_yyyy-MM
 * @returns {string} 文本
 */
function formatPeriodDefectRate(record: IpqcOrderMonthlyTrend, columnKey: string): string {
  const period = columnKey.replace(/^period_/, '')
  const value = record.periodDefectRates?.[period]
  if (value == null || Number.isNaN(value)) return '—'
  return `${(value * 100).toFixed(2)}%`
}

/**
 * 期间区间 → API 参数
 * @param {[string, string] | null | undefined} range 年月区间
 * @returns periodDateStart / periodDateEnd / focusPeriod
 */
function periodRangeToQuery(range: [string, string] | null | undefined) {
  if (!range?.[0]) {
    return {}
  }
  const periodDateStart = `${range[0]}-01`
  if (!range[1]) {
    return { periodDateStart, focusPeriod: range[0] }
  }
  return {
    periodDateStart,
    periodDateEnd: `${range[1]}-01`,
    focusPeriod: range[1],
  }
}

/**
 * 构建查询
 * @returns 查询 DTO 或 null
 */
function buildQuery() {
  const plant = props.plantCode?.trim()
  if (!plant) {
    return null
  }
  return {
    plantCode: plant,
    processCode: props.processCode?.trim() || undefined,
    trendFilter: props.trendFilter || undefined,
    pageIndex: pageIndex.value,
    pageSize: pageSize.value,
    ...periodRangeToQuery(props.periodRange),
  }
}

/** 清空 */
function clear() {
  rows.value = []
  periodOrder.value = []
  rowCount.value = 0
  basePeriod.value = ''
  comparePeriod.value = ''
  upCount.value = 0
  downCount.value = 0
  flatCount.value = 0
  total.value = 0
  pageIndex.value = getTaktDefaultPageIndex()
  hasRows.value = false
}

/** 加载数据 */
async function loadData() {
  const query = buildQuery()
  if (!query) {
    clear()
    return
  }
  loading.value = true
  try {
    const result = await getIpqcOrderMonthlyTrendAnalysis(query)
    rows.value = result.paged?.data ?? []
    total.value = result.paged?.total ?? 0
    periodOrder.value = result.periodOrder ?? []
    rowCount.value = result.rowCount ?? 0
    basePeriod.value = result.basePeriod ?? ''
    comparePeriod.value = result.comparePeriod ?? ''
    upCount.value = result.upCount ?? 0
    downCount.value = result.downCount ?? 0
    flatCount.value = result.flatCount ?? 0
    hasRows.value = rows.value.length > 0
  } catch (error: unknown) {
    clear()
    const err = error as { message?: string }
    message.error(err?.message || t(`${localePrefix}.exportFailed`))
  } finally {
    loading.value = false
    await nextTick()
    recalcTableScrollY()
  }
}

/** 重新加载 */
async function reload() {
  pageIndex.value = getTaktDefaultPageIndex()
  await loadData()
}

/**
 * 分页变更
 * @param {number} page 页码
 * @param {number} size 页大小
 */
async function handlePageChange(page: number, size: number) {
  pageIndex.value = page
  pageSize.value = size
  await loadData()
}

/** 清单导出 */
async function handleExport() {
  const query = buildQuery()
  if (!query) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return
  }
  if (rowCount.value === 0) {
    message.warning(t(`${localePrefix}.exportEmpty`))
    return
  }
  try {
    const { sheetName, exportName, fallbackBase } = taktTrendExportFileNames(
      query.plantCode,
      t(`${localePrefix}.title`),
    )
    const exportMeta = await exportIpqcOrderMonthlyTrendAnalysis(
      { ...query, pageIndex: 1, pageSize: 1 },
      sheetName,
      exportName,
    )
    const blob = (exportMeta as { blob?: Blob }).blob ?? (exportMeta as unknown as Blob)
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as { contentDisposition?: string | null }).contentDisposition ?? null,
      contentType: (exportMeta as { contentType?: string | null }).contentType ?? null,
      fallbackBase,
    })
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t(`${localePrefix}.exportSuccess`))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t(`${localePrefix}.exportFailed`))
  }
}

/** 测算表体高度 */
function recalcTableScrollY(): void {
  const wrap = tableWrapRef.value
  if (!wrap || wrap.clientHeight <= 0) return
  const headerApprox = 55
  tableScrollY.value = Math.max(TAKT_TABLE_SCROLL_Y_MIN, wrap.clientHeight - headerApprox)
}

function startTableScrollObserve(): void {
  stopTableScrollObserve()
  const wrap = tableWrapRef.value
  if (!wrap) return
  recalcTableScrollY()
  tableScrollResizeObserver = new ResizeObserver(() => recalcTableScrollY())
  tableScrollResizeObserver.observe(wrap)
}

function stopTableScrollObserve(): void {
  tableScrollResizeObserver?.disconnect()
  tableScrollResizeObserver = null
}

watch(
  () => props.trendFilter,
  () => {
    if (!props.plantCode?.trim()) {
      return
    }
    void reload()
  },
)

useTableRefresh(reload)

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  pageIndex.value = getTaktDefaultPageIndex()
  pageSize.value = getTaktDefaultPageSize()
  await nextTick()
  startTableScrollObserve()
})

onBeforeUnmount(() => {
  stopTableScrollObserve()
})

defineExpose({ reload, loadData, handleExport, clear })
</script>

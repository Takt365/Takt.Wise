<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-monthly-trend/components -->
<!-- 文件名称：EcMonthlyTrendPanel.vue -->
<!-- 功能描述：月设变/月实施推移转置表 -->
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
        :id-column-key="idColumnKey"
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
            {{ formatPeriodCount(record, String(column.key)) }}
          </template>
          <template v-else-if="column.key === 'trend'">
            <span :class="trendClass(getTrend(record))">
              {{ trendLabel(getTrend(record)) }}
            </span>
          </template>
          <template v-else-if="column.key === 'varianceAmount'">
            <span :class="varianceClass(getVarianceAmount(record))">
              {{ formatCount(getVarianceAmount(record)) }}
            </span>
          </template>
          <template v-else-if="column.key === 'variancePercent'">
            <span :class="varianceClass(getVarianceAmount(record))">
              {{ formatPercent(getVariancePercent(record)) }}
            </span>
          </template>
          <template v-else-if="column.key === 'deptCode'">
            {{ deptLabel(record.deptCode) }}
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
      class="shrink-0"
      :current="pageIndex"
      :page-size="pageSize"
      :total="total"
      @change="handlePageChange"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 月设变/月实施推移转置分析表
 */
import { formatAnalysisVariancePercent } from '@/utils/analysisPercent'
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import {
  exportEcImplementationMonthlyTrendAnalysis,
  exportEcMonthlyTrendAnalysis,
  getEcImplementationMonthlyTrendAnalysis,
  getEcMonthlyTrendAnalysis,
} from '@/api/logistics/manufacturing/engineering-change/ecMonthlyTrend.api'
import type {
  EcImplementationMonthlyTrend,
  EcMonthlyTrend,
} from '@/types/logistics/manufacturing/engineering-change/ecMonthlyTrend.types'
import { useTableRefresh } from '@/composables/useTableRefresh'
import {
  ensureTaktPaginationConfigAsync,
  getTaktDefaultPageIndex,
  getTaktDefaultPageSize,
} from '@/utils/taktPaged'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { taktTrendExportFileNames } from '@/utils/naming'
import { taktOrgDeptI18nKey } from '@/utils/naming'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/tableScroll'
import { useEcTeI18n } from '@/views/logistics/manufacturing/engineering-change/ec-te/composables/useEcTeI18n'

type EcTrendRow = EcMonthlyTrend | EcImplementationMonthlyTrend

const props = defineProps<{
  /** 涨跌筛选 */
  trendFilter?: string
  /** 当前 Tab */
  activeTab: 'issue' | 'implement'
  /** 工厂代码 */
  plantCode?: string
  /** 年月区间 */
  periodRange?: [string, string] | null
  /** 设变单号 */
  ecCode?: string
  /** 区分 */
  ecImplScope?: number
  /** 变更状态 */
  changeStatus?: number
  /** 设变状态 */
  ecStatus?: number
  /** 部门编码 */
  deptCode?: string
}>()

const loading = defineModel<boolean>('loading', { default: false })
const hasRows = defineModel<boolean>('hasRows', { default: false })

const localePrefix = 'logistics.manufacturing.engineering-change.ec-monthly-trend.page'
const { t } = useI18n()
const gi = useEcTeI18n()

/**
 * 部门显示名（TaktDeptI18nSeedData：org.dept.{编码小写}）
 * @param code 部门编码
 * @returns {string} 文案
 */
function deptLabel(code: string | null | undefined): string {
  const key = taktOrgDeptI18nKey(code)
  return key ? t(key) : ''
}

/** 行数据 */
const rows = ref<EcTrendRow[]>([])
/** 期间列 */
const periodOrder = ref<string[]>([])
/** 行总数（筛选后） */
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

/** 工厂必填 */
const hasQuery = computed(() => !!props.plantCode?.trim())

/** 标识列 */
const idColumnKey = computed(() =>
  props.activeTab === 'implement' ? 'deptCode' : 'ecCode',
)

/** 摘要 */
const summaryText = computed(() => {
  if (!props.plantCode?.trim()) {
    return t(`${localePrefix}.selectPlantRequired`)
  }
  return t(
    props.activeTab === 'implement' ? `${localePrefix}.summaryImplement` : `${localePrefix}.summary`,
    { count: rowCount.value },
  )
})

/** 动态列 */
const columns = computed<TableColumnsType>(() => {
  if (props.activeTab === 'implement') {
    const cols: TableColumnsType = [
      {
        title: gi.label('plantCode'),
        dataIndex: 'plantCode',
        key: 'plantCode',
        width: 100,
        ellipsis: true,
        fixed: 'left',
      },
      {
        title: t(`${localePrefix}.deptCode`),
        dataIndex: 'deptCode',
        key: 'deptCode',
        width: 120,
        ellipsis: true,
        fixed: 'left',
      }]
    for (const period of periodOrder.value) {
      cols.push({
        title: period,
        dataIndex: ['periodValues', period],
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
        width: 100,
        align: 'right',
        fixed: 'right',
      },
    )
    return cols
  }
  const cols: TableColumnsType = [
    {
      title: gi.label('plantCode'),
      dataIndex: 'plantCode',
      key: 'plantCode',
      width: 100,
      ellipsis: true,
      fixed: 'left',
    },
    {
      title: gi.label('ecCode'),
      dataIndex: 'ecCode',
      key: 'ecCode',
      width: 140,
      ellipsis: true,
      fixed: 'left',
    },
    {
      title: t(`${localePrefix}.deptCode`),
      dataIndex: 'deptCode',
      key: 'deptCode',
      width: 120,
      ellipsis: true,
      fixed: 'left',
    }]
  for (const period of periodOrder.value) {
    cols.push({
      title: period,
      dataIndex: ['periodValues', period],
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
      width: 100,
      align: 'right',
      fixed: 'right',
    },
  )
  return cols
})

const visibleColumnKeys = computed(() => columns.value.map((c) => String(c.key)))

/**
 * 行主键
 * @param {EcTrendRow} record 行
 * @returns {string} key
 */
function getRowKey(record: EcTrendRow): string {
  if (props.activeTab === 'implement') {
    const row = record as EcImplementationMonthlyTrend
    return `${row.plantCode}|${row.deptCode}`
  }
  const row = record as EcMonthlyTrend
  return `${row.plantCode}|${row.ecCode}|${row.deptCode}`
}

/**
 * 读取涨跌码
 * @param {EcTrendRow} record 行
 * @returns {string} 涨跌码
 */
function getTrend(record: EcTrendRow): string {
  return record.trend || 'none'
}

/**
 * 读取环比差额
 * @param {EcTrendRow} record 行
 * @returns {number | null | undefined} 差额
 */
function getVarianceAmount(record: EcTrendRow): number | null | undefined {
  return record.varianceAmount
}

/**
 * 读取环比变动率
 * @param {EcTrendRow} record 行
 * @returns {number | null | undefined} 比率
 */
function getVariancePercent(record: EcTrendRow): number | null | undefined {
  return record.variancePercent
}

/**
 * 格式化件数
 * @param {number | null | undefined} value 数值
 * @returns {string} 文本
 */
function formatCount(value?: number | null): string {
  if (value == null || Number.isNaN(value)) return '—'
  return String(value)
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
  const key = trend || 'none'
  return t(`${localePrefix}.trend.${key}`)
}

/**
 * 涨跌样式
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
 * 格式化期间件数
 * @param {EcTrendRow} record 行
 * @param {string} columnKey period_yyyy-MM
 * @returns {string} 文本
 */
function formatPeriodCount(record: EcTrendRow, columnKey: string): string {
  const period = columnKey.replace(/^period_/, '')
  const value = record.periodValues?.[period]
  if (value == null || Number.isNaN(value)) return '—'
  return String(value)
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
 * 构建设变推移查询
 * @returns 查询 DTO 或 null
 */
function buildIssueQuery() {
  const plant = props.plantCode?.trim()
  if (!plant) {
    return null
  }
  return {
    plantCode: plant,
    ecCode: props.ecCode?.trim() || undefined,
    deptCode: props.deptCode?.trim() || undefined,
    ecImplScope: props.ecImplScope,
    changeStatus: props.changeStatus,
    ecStatus: props.ecStatus,
    trendFilter: props.trendFilter || undefined,
    pageIndex: pageIndex.value,
    pageSize: pageSize.value,
    ...periodRangeToQuery(props.periodRange),
  }
}

/**
 * 构建实施推移查询
 * @returns 查询 DTO 或 null
 */
function buildImplementQuery() {
  const plant = props.plantCode?.trim()
  if (!plant) {
    return null
  }
  const dept = props.deptCode?.trim()
  return {
    plantCode: plant,
    deptCode: dept || undefined,
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
  if (props.activeTab === 'implement') {
    const query = buildImplementQuery()
    if (!query) {
      clear()
      return
    }
    loading.value = true
    try {
      const result = await getEcImplementationMonthlyTrendAnalysis(query)
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
    return
  }
  const query = buildIssueQuery()
  if (!query) {
    clear()
    return
  }
  loading.value = true
  try {
    const result = await getEcMonthlyTrendAnalysis(query)
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
  if (props.activeTab === 'implement') {
    const query = buildImplementQuery()
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
        t(`${localePrefix}.tabs.implement`),
      )
      const exportMeta = await exportEcImplementationMonthlyTrendAnalysis(
        {
          ...query,
          pageIndex: 1,
          pageSize: 1,
        },
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
    return
  }
  const query = buildIssueQuery()
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
      t(`${localePrefix}.tabs.issue`),
    )
    const exportMeta = await exportEcMonthlyTrendAnalysis(
      {
        ...query,
        pageIndex: 1,
        pageSize: 1,
      },
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
  () => [props.trendFilter, props.activeTab] as const,
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

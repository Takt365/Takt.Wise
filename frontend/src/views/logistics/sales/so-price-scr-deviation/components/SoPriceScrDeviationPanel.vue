<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/sales/so-price-scr-deviation/components -->
<!-- 文件名称：SoPriceScrDeviationPanel.vue -->
<!-- 功能描述：SO价在同周期SCR价目中不存在的明细；defineExpose reload/loadData/handleExport/clear -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex min-h-0 flex-1 flex-col gap-2">
    <div class="text-sm text-text-secondary">
      {{ t(`${localePrefix}.summary`, { count: deviationCount, noScr: noScrCount, absent: priceAbsentCount }) }}
    </div>
    <TaktSingleTable
      analysis-table
      entity-scope="company"
      :include-audit-fields="false"
      table-mode="single"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :data-source="dataSource"
      :loading="loading"
      :pagination="false"
      :row-key="getRowKey"
      class="min-h-0 flex-1"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'soNormalizedUnitPrice' || column.key === 'closestScrNormalizedUnitPrice'">
          <span class="tabular-nums">
            {{ formatUnitPrice((record as SoPriceScrDeviationRow)[column.key as 'soNormalizedUnitPrice' | 'closestScrNormalizedUnitPrice']) }}
          </span>
        </template>
        <template v-else-if="column.key === 'deviationAmount'">
          <span :class="['tabular-nums', trendClass((record as SoPriceScrDeviationRow).deviationTrend)]">
            {{ formatUnitPrice((record as SoPriceScrDeviationRow).deviationAmount) }}
          </span>
        </template>
        <template v-else-if="column.key === 'deviationTrend'">
          <span :class="trendClass((record as SoPriceScrDeviationRow).deviationTrend)">
            {{ trendLabel((record as SoPriceScrDeviationRow).deviationTrend) }}
          </span>
        </template>
        <template v-else-if="column.key === 'matchReason'">
          {{ matchReasonLabel((record as SoPriceScrDeviationRow).matchReason) }}
        </template>
        <template v-else-if="column.key === 'scrPricesText'">
          <span class="whitespace-pre-wrap break-all text-xs">{{ (record as SoPriceScrDeviationRow).scrPricesText || '—' }}</span>
        </template>
        <template v-else-if="column.key === 'customerCode'">
          <span>{{ (record as SoPriceScrDeviationRow).customerCode }} {{ (record as SoPriceScrDeviationRow).customerName }}</span>
        </template>
        <template v-else-if="column.key === 'materialCode'">
          <span>{{ (record as SoPriceScrDeviationRow).materialCode }} {{ (record as SoPriceScrDeviationRow).materialDescription }}</span>
        </template>
      </template>
    </TaktSingleTable>
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePageChange"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * SO价偏离SCR明细面板（同周期价目不存在）
 */
import { message } from 'ant-design-vue'
import type { TableColumnType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import {
  exportSoPriceScrDeviationAnalysis,
  getSoPriceScrDeviationAnalysis,
} from '@/api/logistics/sales/soPriceScrDeviation.api'
import type {
  SoPriceScrDeviationQuery,
  SoPriceScrDeviationRow,
} from '@/types/logistics/sales/soPriceScrDeviation.types'
import { useTableRefresh } from '@/composables/useTableRefresh'
import {
  ensureTaktPaginationConfigAsync,
  getTaktDefaultPageIndex,
  getTaktDefaultPageSize,
} from '@/utils/taktPaged'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { taktTrendExportFileNames } from '@/utils/naming'

const props = defineProps<{
  /** 工厂 */
  plantCode?: string
  /** 期间年月 */
  periodRange?: [string, string] | null
  /** 客户 */
  customerCode?: string
  /** 物料 */
  materialCode?: string
}>()

/** 外层 loading */
const loading = defineModel<boolean>('loading', { default: false })
/** 是否有行 */
const hasRows = defineModel<boolean>('hasRows', { default: false })

const { t } = useI18n()
/** 静态 locales 前缀 */
const localePrefix = 'logistics.sales.so-price-scr-deviation.page'

/** 列表数据 */
const dataSource = ref<SoPriceScrDeviationRow[]>([])
/** 当前页 */
const currentPage = ref(1)
/** 页大小 */
const pageSize = ref(20)
/** 总数 */
const total = ref(0)
/** 命中总数 */
const deviationCount = ref(0)
/** 无 SCR */
const noScrCount = ref(0)
/** 单价不在价目 */
const priceAbsentCount = ref(0)

/** 全部业务列均展示（分析表不做默认 8 列截断） */
const visibleColumnKeys = [
  'plantCode',
  'salesOrderCode',
  'lineNumber',
  'orderDate',
  'pricingDate',
  'customerCode',
  'materialCode',
  'orderQuantity',
  'soNormalizedUnitPrice',
  'matchReason',
  'scrValidFrom',
  'scrValidTo',
  'scrPricesText',
  'closestScrNormalizedUnitPrice',
  'deviationAmount',
  'deviationTrend',
]

/** 表格列 */
const columns = computed<TableColumnType[]>(() => [
  { title: t(`${localePrefix}.columns.plantCode`), dataIndex: 'plantCode', key: 'plantCode', width: 88, ellipsis: true, fixed: 'left' },
  { title: t(`${localePrefix}.columns.salesOrderCode`), dataIndex: 'salesOrderCode', key: 'salesOrderCode', width: 120, ellipsis: true, fixed: 'left' },
  { title: t(`${localePrefix}.columns.lineNumber`), dataIndex: 'lineNumber', key: 'lineNumber', width: 72, align: 'right' },
  { title: t(`${localePrefix}.columns.orderDate`), dataIndex: 'orderDate', key: 'orderDate', width: 110 },
  { title: t(`${localePrefix}.columns.pricingDate`), dataIndex: 'pricingDate', key: 'pricingDate', width: 110 },
  { title: t(`${localePrefix}.columns.customerCode`), dataIndex: 'customerCode', key: 'customerCode', width: 160, ellipsis: true },
  { title: t(`${localePrefix}.columns.materialCode`), dataIndex: 'materialCode', key: 'materialCode', width: 180, ellipsis: true },
  { title: t(`${localePrefix}.columns.orderQuantity`), dataIndex: 'orderQuantity', key: 'orderQuantity', width: 100, align: 'right' },
  { title: t(`${localePrefix}.columns.soNormalizedUnitPrice`), dataIndex: 'soNormalizedUnitPrice', key: 'soNormalizedUnitPrice', width: 120, align: 'right' },
  { title: t(`${localePrefix}.columns.matchReason`), dataIndex: 'matchReason', key: 'matchReason', width: 120 },
  { title: t(`${localePrefix}.columns.scrValidFrom`), dataIndex: 'scrValidFrom', key: 'scrValidFrom', width: 110 },
  { title: t(`${localePrefix}.columns.scrValidTo`), dataIndex: 'scrValidTo', key: 'scrValidTo', width: 110 },
  { title: t(`${localePrefix}.columns.scrPricesText`), dataIndex: 'scrPricesText', key: 'scrPricesText', width: 360, ellipsis: true },
  { title: t(`${localePrefix}.columns.closestScrNormalizedUnitPrice`), dataIndex: 'closestScrNormalizedUnitPrice', key: 'closestScrNormalizedUnitPrice', width: 130, align: 'right' },
  { title: t(`${localePrefix}.columns.deviationAmount`), dataIndex: 'deviationAmount', key: 'deviationAmount', width: 110, align: 'right' },
  { title: t(`${localePrefix}.columns.deviationTrend`), dataIndex: 'deviationTrend', key: 'deviationTrend', width: 80 },
])

/**
 * 行键
 * @param {SoPriceScrDeviationRow} record 行
 * @returns {string} 键
 */
function getRowKey(record: SoPriceScrDeviationRow): string {
  return `${record.salesOrderCode}-${record.lineNumber}-${record.pricingDate}`
}

/**
 * 折算单价格式
 * @param {number | null | undefined} value 数值
 * @returns {string} 文本
 */
function formatUnitPrice(value?: number | null): string {
  if (value == null || Number.isNaN(value)) {
    return '—'
  }
  return value.toLocaleString(undefined, { minimumFractionDigits: 5, maximumFractionDigits: 5 })
}

/**
 * 原因文案
 * @param {string} reason no_scr / price_absent
 * @returns {string} 文案
 */
function matchReasonLabel(reason: string): string {
  if (reason === 'no_scr') {
    return t(`${localePrefix}.reason.noScr`)
  }
  if (reason === 'price_absent') {
    return t(`${localePrefix}.reason.priceAbsent`)
  }
  return reason || '—'
}

/**
 * 偏离方向文案
 * @param {string} trend up/down/none
 * @returns {string} 文案
 */
function trendLabel(trend: string): string {
  if (trend === 'up') {
    return t(`${localePrefix}.trend.up`)
  }
  if (trend === 'down') {
    return t(`${localePrefix}.trend.down`)
  }
  return '—'
}

/**
 * 涨跌样式类
 * @param {string} trend up/down
 * @returns {string} class
 */
function trendClass(trend: string): string {
  if (trend === 'up') {
    return 'text-red-500'
  }
  if (trend === 'down') {
    return 'text-green-600'
  }
  return 'text-text-secondary'
}

/**
 * 组装查询 DTO
 * @returns {SoPriceScrDeviationQuery | null} 查询；缺必填则为 null
 */
function buildQuery(): SoPriceScrDeviationQuery | null {
  const plant = props.plantCode?.trim()
  if (!plant) {
    return null
  }
  const range = props.periodRange
  if (!range?.[0] || !range?.[1]) {
    return null
  }
  return {
    plantCode: plant,
    periodDateStart: `${range[0]}-01`,
    periodDateEnd: `${range[1]}-01`,
    customerCode: props.customerCode?.trim() || undefined,
    materialCode: props.materialCode?.trim() || undefined,
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
  }
}

/**
 * 加载数据
 * @returns {Promise<void>}
 */
async function loadData(): Promise<void> {
  const query = buildQuery()
  if (!query) {
    dataSource.value = []
    total.value = 0
    deviationCount.value = 0
    noScrCount.value = 0
    priceAbsentCount.value = 0
    hasRows.value = false
    return
  }
  loading.value = true
  try {
    const result = await getSoPriceScrDeviationAnalysis(query)
    dataSource.value = result.paged?.data ?? []
    total.value = result.paged?.total ?? 0
    deviationCount.value = result.deviationCount ?? 0
    noScrCount.value = result.noScrCount ?? 0
    priceAbsentCount.value = result.priceAbsentCount ?? 0
    hasRows.value = deviationCount.value > 0
  } catch {
    dataSource.value = []
    total.value = 0
    deviationCount.value = 0
    noScrCount.value = 0
    priceAbsentCount.value = 0
    hasRows.value = false
  } finally {
    loading.value = false
  }
}

/**
 * 查询（重置到第一页）
 * @returns {Promise<void>}
 */
async function reload(): Promise<void> {
  currentPage.value = getTaktDefaultPageIndex()
  await loadData()
}

/**
 * 分页变更
 * @returns {Promise<void>}
 */
async function handlePageChange(): Promise<void> {
  await loadData()
}

/**
 * 清空
 */
function clear(): void {
  dataSource.value = []
  total.value = 0
  deviationCount.value = 0
  noScrCount.value = 0
  priceAbsentCount.value = 0
  hasRows.value = false
  currentPage.value = getTaktDefaultPageIndex()
}

/**
 * 导出
 * @returns {Promise<void>}
 */
async function handleExport(): Promise<void> {
  const query = buildQuery()
  if (!query) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return
  }
  if (deviationCount.value === 0) {
    message.warning(t(`${localePrefix}.exportEmpty`))
    return
  }
  try {
    const { sheetName, exportName, fallbackBase } = taktTrendExportFileNames(
      query.plantCode,
      t(`${localePrefix}.title`),
    )
    const exportMeta = await exportSoPriceScrDeviationAnalysis(
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
    link.click()
    window.URL.revokeObjectURL(url)
    message.success(t(`${localePrefix}.exportSuccess`))
  } catch {
    message.error(t(`${localePrefix}.exportFailed`))
  }
}

useTableRefresh(loadData)

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  pageSize.value = getTaktDefaultPageSize()
  currentPage.value = getTaktDefaultPageIndex()
})

defineExpose({ reload, loadData, handleExport, clear })
</script>

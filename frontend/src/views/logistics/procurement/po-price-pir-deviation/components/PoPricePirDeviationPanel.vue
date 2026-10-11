<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/po-price-pir-deviation/components -->
<!-- 文件名称：PoPricePirDeviationPanel.vue -->
<!-- 功能描述：PO价偏离PIR明细；defineExpose reload/loadData/handleExport/clear -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 flex-1 flex-col gap-2 overflow-hidden">
    <div class="shrink-0 text-sm text-text-secondary">
      {{ t(`${localePrefix}.summary`, { count: deviationCount, noPir: noPirCount, absent: priceAbsentCount }) }}
    </div>
    <div
      ref="tableWrapRef"
      class="min-h-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        analysis-table
        class="h-full min-h-0"
        entity-scope="company"
        :include-audit-fields="false"
        table-mode="single"
        :columns="columns"
        :visible-column-keys="visibleColumnKeys"
        :id-column-key="'__none__'"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :pagination="false"
        :show-row-selection="false"
        :row-key="getRowKey"
        :scroll="{ x: 'max-content', y: tableScrollY }"
      >
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'poUnitPrice'">
            <span class="tabular-nums">
              {{ formatUnitPrice((record as PoPricePirDeviationRow).poUnitPrice) }}
            </span>
          </template>
          <template v-else-if="column.key === 'deviationAmount'">
            <span :class="['tabular-nums', trendClass((record as PoPricePirDeviationRow).deviationTrend)]">
              {{ formatUnitPrice((record as PoPricePirDeviationRow).deviationAmount) }}
            </span>
          </template>
          <template v-else-if="column.key === 'deviationPercent'">
            <span :class="['tabular-nums', trendClass((record as PoPricePirDeviationRow).deviationTrend)]">
              {{ formatDeviationPercent((record as PoPricePirDeviationRow).deviationPercent) }}
            </span>
          </template>
          <template v-else-if="column.key === 'pirPricesText'">
            <span class="whitespace-pre-wrap break-all text-xs">{{ (record as PoPricePirDeviationRow).pirPricesText || '—' }}</span>
          </template>
          <template v-else-if="column.key === 'supplierCode'">
            <span>{{ (record as PoPricePirDeviationRow).supplierCode }} {{ (record as PoPricePirDeviationRow).supplierName }}</span>
          </template>
        </template>
      </TaktSingleTable>
    </div>
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      class="shrink-0"
      :total="total"
      @change="handlePageChange"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * PO价偏离PIR明细面板
 */
import { message } from 'ant-design-vue'
import type { TableColumnType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import {
  exportPoPricePirDeviationAnalysis,
  getPoPricePirDeviationAnalysis,
} from '@/api/logistics/procurement/poPricePirDeviation.api'
import type {
  PoPricePirDeviationQuery,
  PoPricePirDeviationRow,
} from '@/types/logistics/procurement/poPricePirDeviation.types'
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
  /** 工厂 */
  plantCode?: string
  /** 期间年月 */
  periodRange?: [string, string] | null
  /** 供应商 */
  supplierCode?: string
  /** 物料 */
  materialCode?: string
  /** 涨跌筛选：空=全部；changed；up；down */
  trendFilter?: string
}>()

/** 外层 loading */
const loading = defineModel<boolean>('loading', { default: false })
/** 是否有行 */
const hasRows = defineModel<boolean>('hasRows', { default: false })

const { t } = useI18n()
/** 静态 locales 前缀 */
const localePrefix = 'logistics.procurement.po-price-pir-deviation.page'

/** 列表数据 */
const dataSource = ref<PoPricePirDeviationRow[]>([])
/** 当前页 */
const currentPage = ref(1)
/** 页大小 */
const pageSize = ref(20)
/** 总数 */
const total = ref(0)
/** 命中总数 */
const deviationCount = ref(0)
/** 无 PIR */
const noPirCount = ref(0)
/** 单价不在价目 */
const priceAbsentCount = ref(0)
/** 表体容器 */
const tableWrapRef = ref<HTMLElement | null>(null)
/** 表体 scroll.y */
const tableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
/** 表体高度观察 */
let tableScrollResizeObserver: ResizeObserver | null = null

/** 全部业务列（不做默认 8 列截断） */
const visibleColumnKeys = [
  'plantCode',
  'purchaseOrderCode',
  'lineNumber',
  'orderDate',
  'supplierCode',
  'materialCode',
  'materialDescription',
  'orderQuantity',
  'poUnitPrice',
  'pirValidPeriod',
  'pirPricesText',
  'deviationAmount',
  'deviationPercent',
]

/** 表格列 */
const columns = computed<TableColumnType[]>(() => [
  { title: t(`${localePrefix}.columns.plantCode`), dataIndex: 'plantCode', key: 'plantCode', width: 88, ellipsis: true, fixed: 'left' },
  { title: t(`${localePrefix}.columns.purchaseOrderCode`), dataIndex: 'purchaseOrderCode', key: 'purchaseOrderCode', width: 120, ellipsis: true, fixed: 'left' },
  { title: t(`${localePrefix}.columns.lineNumber`), dataIndex: 'lineNumber', key: 'lineNumber', width: 72, align: 'right' },
  { title: t(`${localePrefix}.columns.orderDate`), dataIndex: 'orderDate', key: 'orderDate', width: 110 },
  { title: t(`${localePrefix}.columns.supplierCode`), dataIndex: 'supplierCode', key: 'supplierCode', width: 160, ellipsis: true },
  { title: t(`${localePrefix}.columns.materialCode`), dataIndex: 'materialCode', key: 'materialCode', width: 120, ellipsis: true },
  { title: t(`${localePrefix}.columns.materialDescription`), dataIndex: 'materialDescription', key: 'materialDescription', width: 160, ellipsis: true },
  { title: t(`${localePrefix}.columns.orderQuantity`), dataIndex: 'orderQuantity', key: 'orderQuantity', width: 100, align: 'right' },
  { title: t(`${localePrefix}.columns.poUnitPrice`), dataIndex: 'poUnitPrice', key: 'poUnitPrice', width: 110, align: 'right' },
  { title: t(`${localePrefix}.columns.pirValidPeriod`), dataIndex: 'pirValidPeriod', key: 'pirValidPeriod', width: 200, ellipsis: true },
  { title: t(`${localePrefix}.columns.pirPricesText`), dataIndex: 'pirPricesText', key: 'pirPricesText', width: 360, ellipsis: true },
  { title: t(`${localePrefix}.columns.deviationAmount`), dataIndex: 'deviationAmount', key: 'deviationAmount', width: 110, align: 'right' },
  { title: t(`${localePrefix}.columns.deviationPercent`), dataIndex: 'deviationPercent', key: 'deviationPercent', width: 100, align: 'right' },
])

/**
 * 行键
 * @param {PoPricePirDeviationRow} record 行
 * @returns {string} 键
 */
function getRowKey(record: PoPricePirDeviationRow): string {
  return `${record.purchaseOrderCode}-${record.lineNumber}-${record.pricingDate}`
}

/**
 * 单价格式
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
 * 价差比格式（小数 → 百分数）
 * @param {number | null | undefined} value 比率，如 0.05
 * @returns {string} 文本
 */
function formatDeviationPercent(value?: number | null): string {
  if (value == null || Number.isNaN(value)) {
    return '—'
  }
  return `${(value * 100).toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 })}%`
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
 * 测算表体高度
 */
function recalcTableScrollY(): void {
  const wrap = tableWrapRef.value
  if (!wrap || wrap.clientHeight <= 0) {
    return
  }
  const headerApprox = 55
  tableScrollY.value = Math.max(TAKT_TABLE_SCROLL_Y_MIN, wrap.clientHeight - headerApprox)
}

/**
 * 开始观察表体容器
 */
function startTableScrollObserve(): void {
  stopTableScrollObserve()
  const wrap = tableWrapRef.value
  if (!wrap) {
    return
  }
  recalcTableScrollY()
  tableScrollResizeObserver = new ResizeObserver(() => recalcTableScrollY())
  tableScrollResizeObserver.observe(wrap)
}

/**
 * 停止观察
 */
function stopTableScrollObserve(): void {
  tableScrollResizeObserver?.disconnect()
  tableScrollResizeObserver = null
}

/**
 * 组装查询 DTO
 * @returns {PoPricePirDeviationQuery | null} 查询；缺必填则为 null
 */
function buildQuery(): PoPricePirDeviationQuery | null {
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
    supplierCode: props.supplierCode?.trim() || undefined,
    materialCode: props.materialCode?.trim() || undefined,
    trendFilter: props.trendFilter?.trim() || undefined,
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
    noPirCount.value = 0
    priceAbsentCount.value = 0
    hasRows.value = false
    return
  }
  loading.value = true
  try {
    const result = await getPoPricePirDeviationAnalysis(query)
    dataSource.value = result.paged?.data ?? []
    total.value = result.paged?.total ?? 0
    deviationCount.value = result.deviationCount ?? 0
    noPirCount.value = result.noPirCount ?? 0
    priceAbsentCount.value = result.priceAbsentCount ?? 0
    hasRows.value = deviationCount.value > 0
  } catch {
    dataSource.value = []
    total.value = 0
    deviationCount.value = 0
    noPirCount.value = 0
    priceAbsentCount.value = 0
    hasRows.value = false
  } finally {
    loading.value = false
    await nextTick()
    recalcTableScrollY()
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
  noPirCount.value = 0
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
    const exportMeta = await exportPoPricePirDeviationAnalysis(
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

watch(
  () => props.trendFilter,
  () => {
    if (!props.plantCode?.trim()) {
      return
    }
    void reload()
  },
)

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  pageSize.value = getTaktDefaultPageSize()
  currentPage.value = getTaktDefaultPageIndex()
  await nextTick()
  startTableScrollObserve()
})

onBeforeUnmount(() => {
  stopTableScrollObserve()
})

defineExpose({ reload, loadData, handleExport, clear })
</script>

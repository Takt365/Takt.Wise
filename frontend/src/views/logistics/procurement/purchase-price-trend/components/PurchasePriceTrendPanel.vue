<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/purchase-price-trend/components -->
<!-- 文件名称：PurchasePriceTrendPanel.vue -->
<!-- 功能描述：采购价格推移 / 机种价格推移：每笔 ValidFrom 一行；单价/有效起始日/涨跌分列 -->
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
        :id-column-key="'rowKey'"
        :data-source="displayRows"
        :loading="loading"
        :stripe="true"
        :row-key="getRowKey"
        :pagination="false"
        :show-row-selection="false"
        :scroll="{ x: 'max-content', y: tableScrollY }"
      >
        <template #bodyCell="{ column, record, text }">
          <template v-if="column.key === 'currencyCode'">
            <template v-if="(record as PurchasePriceTrendDisplayRow).isFirst">
              <TaktDictTag
                :value="(record as PurchasePriceTrendDisplayRow).currencyCode"
                dict-type="accounting_financial_currency_code"
              />
            </template>
          </template>
          <template v-else-if="column.key === 'unitPrice'">
            <span class="tabular-nums">
              {{ formatCost((record as PurchasePriceTrendDisplayRow).unitPrice) }}
            </span>
          </template>
          <template v-else-if="column.key === 'validFrom'">
            {{ formatPointDate((record as PurchasePriceTrendDisplayRow).validFrom) }}
          </template>
          <template v-else-if="column.key === 'adjacent'">
            <template v-if="(record as PurchasePriceTrendDisplayRow).adjacentBasePeriod">
              <a-tooltip
                v-if="formatAdjacentDisplay(record as PurchasePriceTrendDisplayRow)"
                :title="t(`${localePrefix}.adjacentVs`, {
                  period: formatPointDate((record as PurchasePriceTrendDisplayRow).adjacentBasePeriod || undefined),
                })"
              >
                <span :class="trendClass((record as PurchasePriceTrendDisplayRow).adjacentTrend || 'none')">
                  {{ formatAdjacentDisplay(record as PurchasePriceTrendDisplayRow) }}
                </span>
              </a-tooltip>
            </template>
          </template>
          <template v-else-if="column.key === 'trend'">
            <template v-if="(record as PurchasePriceTrendDisplayRow).isFirst">
              <span :class="trendClass((record as PurchasePriceTrendDisplayRow).trend || 'none')">
                {{ trendLabel((record as PurchasePriceTrendDisplayRow).trend || 'none') }}
              </span>
            </template>
          </template>
          <template v-else-if="column.key === 'varianceAmount'">
            <template v-if="(record as PurchasePriceTrendDisplayRow).isFirst">
              <span :class="varianceClass((record as PurchasePriceTrendDisplayRow).varianceAmount)">
                {{ formatCost((record as PurchasePriceTrendDisplayRow).varianceAmount) }}
              </span>
            </template>
          </template>
          <template v-else-if="column.key === 'variancePercent'">
            <template v-if="(record as PurchasePriceTrendDisplayRow).isFirst">
              <span :class="varianceClass((record as PurchasePriceTrendDisplayRow).varianceAmount)">
                {{ formatPercent((record as PurchasePriceTrendDisplayRow).variancePercent) }}
              </span>
            </template>
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
              v-if="hasQuery && materialCount > 0"
              class="text-sm text-text"
            >
              {{ t(`${localePrefix}.trendSummaryValidFrom`, {
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
 * 采购价格推移：每笔 ValidFrom 一行；单价 / 有效起始日 / 涨跌分列
 */
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import {
  exportPurchasePriceTrendAnalysis,
  getPurchasePriceTrendAnalysis,
} from '@/api/logistics/procurement/purchasePriceTrend.api'
import type { PurchasePriceTrend, PurchasePriceTrendPoint } from '@/types/logistics/procurement/purchasePriceTrend.types'
import { useTableRefresh } from '@/composables/useTableRefresh'
import { formatAnalysisVariancePercent, isAnalysisVariancePercentZero } from '@/utils/analysisPercent'
import {
  ensureTaktPaginationConfigAsync,
  getTaktDefaultPageIndex,
  getTaktDefaultPageSize,
} from '@/utils/taktPaged'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { taktTrendExportFileNames } from '@/utils/naming'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/tableScroll'

/**
 * 展开后的展示行（组内首行填物料维；续行留空）
 */
interface PurchasePriceTrendDisplayRow {
  /** 行主键 */
  rowKey: string
  /** 是否组内首行 */
  isFirst: boolean
  plantCode: string
  materialCode: string
  materialDescription: string
  supplierCode: string
  supplierName: string
  currencyCode: string
  unit: string
  unitPrice?: number | null
  validFrom?: string
  adjacentTrend?: string
  adjacentVarianceAmount?: number | null
  adjacentVariancePercent?: number | null
  adjacentBasePeriod?: string | null
  trend?: string
  varianceAmount?: number | null
  variancePercent?: number | null
}

const props = defineProps<{
  /** 涨跌筛选 */
  trendFilter?: string
  /** 工厂代码 */
  plantCode?: string
  /** 年月区间 */
  periodRange?: [string, string] | null
  /** 供应商编码 */
  supplierCode?: string
  /** 物料编码关键字 */
  materialCode?: string
  /** 价格类型 */
  priceType?: string
}>()

const loading = defineModel<boolean>('loading', { default: false })
const hasRows = defineModel<boolean>('hasRows', { default: false })

const localePrefix = 'logistics.procurement.purchase-price-trend.page'
const { t } = useI18n()

/** API 物料×供应商行 */
const rows = ref<PurchasePriceTrend[]>([])
/** 期间列 */
const periodOrder = ref<string[]>([])
/** 行总数（筛选后） */
const materialCount = ref(0)
/** 期间基准有效起始日 */
const basePeriod = ref('')
/** 期间对比有效起始日 */
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

/** 摘要 */
const summaryText = computed(() => {
  if (!props.plantCode?.trim()) {
    return t(`${localePrefix}.selectPlantRequired`)
  }
  return t(`${localePrefix}.summary`, { count: materialCount.value })
})

/**
 * 将物料×供应商行按 ValidFrom 笔展开为多行展示
 */
const displayRows = computed<PurchasePriceTrendDisplayRow[]>(() => {
  const result: PurchasePriceTrendDisplayRow[] = []
  for (const row of rows.value) {
    const points = row.periodPoints ?? []
    if (points.length === 0) {
      result.push(createDisplayRow(row, null, 0, true))
      continue
    }
    for (let i = 0; i < points.length; i++) {
      result.push(createDisplayRow(row, points[i], i, i === 0))
    }
  }
  return result
})

/** 动态列 */
const columns = computed<TableColumnsType>(() => {
  const cols: TableColumnsType = [
    {
      title: t('entity.purchaseinforecord.materialcode'),
      dataIndex: 'materialCode',
      key: 'materialCode',
      width: 140,
      ellipsis: true,
      fixed: 'left',
    },
    {
      title: t('entity.purchaseinforecord.materialdescription'),
      dataIndex: 'materialDescription',
      key: 'materialDescription',
      width: 160,
      ellipsis: true,
    },
    {
      title: t('entity.purchaseinforecord.suppliercode'),
      dataIndex: 'supplierCode',
      key: 'supplierCode',
      width: 120,
      ellipsis: true,
    },
    {
      title: t('entity.supplier.name1'),
      dataIndex: 'supplierName',
      key: 'supplierName',
      width: 160,
      ellipsis: true,
    },
    {
      title: t('entity.purchaseinforecorditem.conditioncurrencycode'),
      dataIndex: 'currencyCode',
      key: 'currencyCode',
      width: 80,
    },
    {
      title: t('entity.purchaseinforecorditem.unitofmeasure'),
      dataIndex: 'unit',
      key: 'unit',
      width: 80,
    },
    {
      title: t(`${localePrefix}.columns.unitPrice`),
      dataIndex: 'unitPrice',
      key: 'unitPrice',
      width: 120,
      align: 'right',
    },
    {
      title: t(`${localePrefix}.columns.validFrom`),
      dataIndex: 'validFrom',
      key: 'validFrom',
      width: 120,
    },
    {
      title: t(`${localePrefix}.columns.adjacent`),
      dataIndex: 'adjacent',
      key: 'adjacent',
      width: 110,
    },
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
      width: 110,
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
  ]
  return cols
})

const visibleColumnKeys = computed(() => columns.value.map((c) => String(c.key)))

/**
 * 构建展开展示行
 * @param {PurchasePriceTrend} row 物料×供应商行
 * @param {PurchasePriceTrendPoint | null} point 当前笔
 * @param {number} index 笔序号
 * @param {boolean} isFirst 是否组内首行
 * @returns {PurchasePriceTrendDisplayRow} 展示行
 */
function createDisplayRow(
  row: PurchasePriceTrend,
  point: PurchasePriceTrendPoint | null,
  index: number,
  isFirst: boolean,
): PurchasePriceTrendDisplayRow {
  const baseKey = `${row.plantCode}|${row.materialCode}|${row.supplierCode}`
  return {
    rowKey: `${baseKey}|${point?.validFrom ?? 'none'}|${index}`,
    isFirst,
    plantCode: isFirst ? row.plantCode : '',
    materialCode: isFirst ? row.materialCode : '',
    materialDescription: isFirst ? (row.materialDescription ?? '') : '',
    supplierCode: isFirst ? row.supplierCode : '',
    supplierName: isFirst ? (row.supplierName ?? '') : '',
    currencyCode: isFirst ? (row.currencyCode ?? '') : '',
    unit: isFirst ? (row.unit ?? '') : '',
    unitPrice: point?.unitPrice ?? null,
    validFrom: point?.validFrom,
    adjacentTrend: point?.adjacentTrend,
    adjacentVarianceAmount: point?.adjacentVarianceAmount,
    adjacentVariancePercent: point?.adjacentVariancePercent,
    adjacentBasePeriod: point?.adjacentBasePeriod,
    trend: isFirst ? row.trend : undefined,
    varianceAmount: isFirst ? row.varianceAmount : null,
    variancePercent: isFirst ? row.variancePercent : null,
  }
}

/**
 * 行主键
 * @param {PurchasePriceTrendDisplayRow} record 行
 * @returns {string} key
 */
function getRowKey(record: PurchasePriceTrendDisplayRow): string {
  return record.rowKey
}

/**
 * 格式化金额
 * @param {number | null | undefined} value 数值
 * @returns {string} 文本
 */
function formatCost(value?: number | null): string {
  if (value == null || Number.isNaN(value)) return '—'
  return value.toFixed(5)
}

/**
 * 格式化环比变动率（0.00% 不显示）
 * @param {number | null | undefined} value 比率
 * @returns {string} 百分比文本
 */
function formatPercent(value?: number | null): string {
  return formatAnalysisVariancePercent(value, 'ratio')
}

/**
 * 有效起始日展示为 yyyy/M/d
 * @param {string | undefined} value yyyy-MM-dd
 * @returns {string} 文本
 */
function formatPointDate(value?: string): string {
  if (!value) return '—'
  const parts = value.split('-')
  if (parts.length !== 3) return value
  return `${parts[0]}/${Number(parts[1])}/${Number(parts[2])}`
}

/**
 * 相对前一笔涨跌：仅当变动率为 0.00% 时该格留空（行仍显示物料/单价/有效起始日等）
 * @param {PurchasePriceTrendDisplayRow} record 展示行
 * @returns {string} 文本
 */
function formatAdjacentDisplay(record: PurchasePriceTrendDisplayRow): string {
  if (!record.adjacentBasePeriod) return ''
  const arrow = record.adjacentTrend === 'up' ? '↑' : record.adjacentTrend === 'down' ? '↓' : '→'
  if (record.adjacentVariancePercent != null && !Number.isNaN(record.adjacentVariancePercent)) {
    // 0.00%：期间% 不展示（不隐藏整行）
    if (isAnalysisVariancePercentZero(record.adjacentVariancePercent, 'ratio')) {
      return ''
    }
    return `${arrow} ${formatPercent(record.adjacentVariancePercent)}`
  }
  if (record.adjacentVarianceAmount == null || Number.isNaN(record.adjacentVarianceAmount)) {
    return ''
  }
  // 无比率时用差额；差额为 0 则该格留空
  if (record.adjacentVarianceAmount === 0) {
    return ''
  }
  return `${arrow} ${formatCost(record.adjacentVarianceAmount)}`
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
  const type = props.priceType?.trim()
  if (!plant || !type || !props.periodRange?.[0]) {
    return null
  }
  return {
    plantCode: plant,
    supplierCode: props.supplierCode?.trim() || undefined,
    materialCode: props.materialCode?.trim() || undefined,
    priceType: type,
    onlyEnabled: true,
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
  materialCount.value = 0
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
    const result = await getPurchasePriceTrendAnalysis(query)
    rows.value = result.paged?.data ?? []
    total.value = result.paged?.total ?? 0
    periodOrder.value = result.periodOrder ?? []
    materialCount.value = result.materialCount ?? 0
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
  if (materialCount.value === 0) {
    message.warning(t(`${localePrefix}.exportEmpty`))
    return
  }
  try {
    const { sheetName, exportName, fallbackBase } = taktTrendExportFileNames(
      query.plantCode,
      t(`${localePrefix}.title`),
    )
    const exportMeta = await exportPurchasePriceTrendAnalysis(
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

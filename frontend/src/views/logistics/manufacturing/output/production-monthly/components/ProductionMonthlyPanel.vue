<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/production-monthly/components -->
<!-- 文件名称：ProductionMonthlyPanel.vue -->
<!-- 功能描述：生产统计转置表（当前页签按班组或按机种 × 月份） -->
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
        :id-column-key="groupByTeam ? 'teamCode' : 'modelCode'"
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
            {{ formatPeriodQty(record as ProductionMonthlyTrend, String(column.key)) }}
          </template>
          <template v-else-if="column.key === 'trend'">
            <span :class="trendClass((record as ProductionMonthlyTrend).trend || 'none')">
              {{ trendLabel((record as ProductionMonthlyTrend).trend || 'none') }}
            </span>
          </template>
          <template v-else-if="column.key === 'varianceAmount'">
            <span :class="varianceClass((record as ProductionMonthlyTrend).varianceAmount)">
              {{ formatQty((record as ProductionMonthlyTrend).varianceAmount) }}
            </span>
          </template>
          <template v-else-if="column.key === 'variancePercent'">
            <span :class="varianceClass((record as ProductionMonthlyTrend).varianceAmount)">
              {{ formatPercent((record as ProductionMonthlyTrend).variancePercent) }}
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
 * 生产统计转置表：当前页签一行一个班组或一个机种
 */
import { formatAnalysisVariancePercent } from '@/utils/analysisPercent'
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import {
  exportProductionMonthlyTrendAnalysis,
  getProductionMonthlyTrendAnalysis,
} from '@/api/logistics/manufacturing/output/productionMonthly.api'
import {
  exportProductionTrendAnalysis,
  getProductionTrendAnalysis,
} from '@/api/logistics/manufacturing/output/productionTrend.api'
import type { ProductionMonthlyTrend } from '@/types/logistics/manufacturing/output/productionMonthlyTrend.types'
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
  /** stat 走生产统计接口，trend 走生产推移接口 */
  requestKind?: 'stat' | 'trend'
  /** 页签 */
  statTab?: string
  /** 工厂代码 */
  plantCode?: string
  /** 年月区间 */
  periodRange?: [string, string] | null
  /** 班组 */
  teamCode?: string
  /** 机种 */
  modelCode?: string
  /** 物料 */
  materialCode?: string
}>()

const loading = defineModel<boolean>('loading', { default: false })
const hasRows = defineModel<boolean>('hasRows', { default: false })

const localePrefix = computed(() => props.requestKind === 'trend'
  ? 'logistics.manufacturing.output.production-trend.page'
  : 'logistics.manufacturing.output.production-monthly.page')
const { t } = useI18n()

/** 行数据 */
const rows = ref<ProductionMonthlyTrend[]>([])
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

/** 按班组页签 */
const groupByTeam = computed(() => props.statTab === 'assy-team' || props.statTab === 'pcba-team')

/** 工厂与期间必填 */
const hasQuery = computed(() => !!props.plantCode?.trim() && !!props.periodRange?.[0])

/** 摘要 */
const summaryText = computed(() => {
  if (!props.plantCode?.trim()) {
    return t(`${localePrefix.value}.selectPlantRequired`)
  }
  if (!props.periodRange?.[0]) {
    return t(`${localePrefix.value}.selectPeriodRequired`)
  }
  return t(`${localePrefix.value}.summary`, { count: rowCount.value })
})

/** 动态列 */
const columns = computed<TableColumnsType>(() => {
  const cols: TableColumnsType = [
    {
      title: t('common.page.entity.plantcode'),
      dataIndex: 'plantCode',
      key: 'plantCode',
      width: 100,
      ellipsis: true,
      fixed: 'left',
    },
    {
      title: groupByTeam.value
        ? t(`${localePrefix.value}.teamCode`)
        : t(`${localePrefix.value}.modelCode`),
      dataIndex: groupByTeam.value ? 'teamCode' : 'modelCode',
      key: groupByTeam.value ? 'teamCode' : 'modelCode',
      width: 120,
      ellipsis: true,
      fixed: 'left',
    }]
  for (const period of periodOrder.value) {
    cols.push({
      title: period,
      dataIndex: ['periodValues', period],
      key: `period_${period}`,
      width: 110,
      align: 'right',
    })
  }
  cols.push(
    {
      title: t(`${localePrefix.value}.columns.trend`),
      dataIndex: 'trend',
      key: 'trend',
      width: 80,
      fixed: 'right',
    },
    {
      title: t(`${localePrefix.value}.columns.varianceAmount`),
      dataIndex: 'varianceAmount',
      key: 'varianceAmount',
      width: 120,
      align: 'right',
      fixed: 'right',
    },
    {
      title: t(`${localePrefix.value}.columns.variancePercent`),
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
 * @param {ProductionMonthlyTrend} record 行
 * @returns {string} key
 */
function getRowKey(record: ProductionMonthlyTrend): string {
  return `${record.plantCode}|${record.teamCode || ''}|${record.modelCode || ''}`
}

/**
 * 格式化产量
 * @param {number | null | undefined} value 数值
 * @returns {string} 文本
 */
function formatQty(value?: number | null): string {
  if (value == null || Number.isNaN(value)) return '—'
  return value.toFixed(1)
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
  return t(`${localePrefix.value}.trend.${key}`)
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
 * 格式化期间产量
 * @param {ProductionMonthlyTrend} record 行
 * @param {string} columnKey period_yyyy-MM
 * @returns {string} 文本
 */
function formatPeriodQty(record: ProductionMonthlyTrend, columnKey: string): string {
  const period = columnKey.replace(/^period_/, '')
  const value = record.periodValues?.[period]
  if (value == null || Number.isNaN(value)) return '—'
  return value.toFixed(1)
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
  const statTab = props.statTab?.trim()
  if (!plant || !statTab || !props.periodRange?.[0]) {
    return null
  }
  return {
    plantCode: plant,
    statTab,
    teamCode: props.teamCode?.trim() || undefined,
    modelCode: props.modelCode?.trim() || undefined,
    materialCode: props.materialCode?.trim() || undefined,
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
    const load = props.requestKind === 'trend'
      ? getProductionTrendAnalysis
      : getProductionMonthlyTrendAnalysis
    const result = await load(query)
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
    message.error(err?.message || t(`${localePrefix.value}.exportFailed`))
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
    message.warning(t(`${localePrefix.value}.selectPlantRequired`))
    return
  }
  if (rowCount.value === 0) {
    message.warning(t(`${localePrefix.value}.exportEmpty`))
    return
  }
  try {
    const { sheetName, exportName, fallbackBase } = taktTrendExportFileNames(
      query.plantCode,
      t(`${localePrefix.value}.title`),
    )
    const exportFn = props.requestKind === 'trend'
      ? exportProductionTrendAnalysis
      : exportProductionMonthlyTrendAnalysis
    const exportMeta = await exportFn(
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
    message.success(t(`${localePrefix.value}.exportSuccess`))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t(`${localePrefix.value}.exportFailed`))
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

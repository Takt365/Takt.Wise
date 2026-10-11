<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/material-cost-analysis -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：BOM 成本分析：TaktBomMaterialCost 转置单表（机种/产品/品名/周期/涨跌/环比） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 min-w-0 w-full flex-col overflow-hidden p-4">
    <material-cost-analysis-query-form
      v-model:plant-code="plantCode"
      v-model:material-type="materialType"
      v-model:model-code="modelCode"
      v-model:product-code="productCode"
      v-model:period-range="periodRange"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    />
    <TaktToolsBar
      :show-create="false"
      :show-update="false"
      :show-delete="false"
      :show-import="false"
      :show-export="true"
      :show-expand="false"
      :show-advanced-query="false"
      :show-column-setting="false"
      :show-fullscreen="false"
      :show-refresh="true"
      :export-disabled="!hasQuery || total === 0"
      :export-loading="exportLoading"
      :refresh-loading="loading"
      :right-actions="toolbarRightActions"
      export-permission="logistics:manufacturing:bom:material:cost:analysis:export"
      @export="handleExport"
      @refresh="handleRefresh"
    />
    <div
      ref="tableWrapRef"
      class="min-h-0 min-w-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        analysis-table
        class="h-full min-h-0"
        entity-scope="company"
        table-mode="single"
        :columns="columns"
        :visible-column-keys="visibleColumnKeys"
        :id-column-key="'productCode'"
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
            {{ formatPeriodCost(record as BomMaterialCostItemTransposed, String(column.key)) }}
          </template>
          <template v-else-if="column.key === 'trend'">
            <span :class="trendClass((record as BomMaterialCostItemTransposed).trend || 'none')">
              {{ trendLabel((record as BomMaterialCostItemTransposed).trend || 'none') }}
            </span>
          </template>
          <template v-else-if="column.key === 'varianceAmount'">
            <span :class="varianceClass((record as BomMaterialCostItemTransposed).varianceAmount)">
              {{ formatCost((record as BomMaterialCostItemTransposed).varianceAmount) }}
            </span>
          </template>
          <template v-else-if="column.key === 'variancePercent'">
            <span :class="varianceClass((record as BomMaterialCostItemTransposed).varianceAmount)">
              {{ formatPercent((record as BomMaterialCostItemTransposed).variancePercent) }}
            </span>
          </template>
          <template v-else>
            {{ text }}
          </template>
        </template>
        <template #footerRemark>
          <div
            v-if="total > 0 && periodOrder.length > 0"
            class="flex max-w-full flex-wrap gap-x-3 gap-y-1 text-sm text-text"
          >
            <span class="font-medium shrink-0">{{ summaryLabel }}</span>
            <span
              v-for="period in periodOrder"
              :key="period"
              class="shrink-0"
            >
              {{ period }}: {{ formatCost(periodCostTotals[period]) }}
            </span>
            <span
              class="shrink-0 font-medium"
              :class="varianceClass(varianceAmountTotal)"
            >
              {{ t(`${localePrefix}.columns.varianceAmount`) }}: {{ formatCost(varianceAmountTotal) }}
            </span>
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
 * BOM 成本分析：TaktBomMaterialCost 转置（机种 / 产品 / 品名 / 周期 / 涨跌）
 */
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import type { ToolBarAction } from '@/components/business/TaktToolsBar/index.vue'
import {
  RiArrowDownLine,
  RiArrowUpDownLine,
  RiArrowUpLine,
  RiLineChartLine,
  RiListCheck,
  RiSortAlphabetAsc,
  RiSortNumberDesc,
} from '@remixicon/vue'
import {
  exportBomMaterialCostItemTransposed,
  getBomMaterialCostItemTransposedList,
} from '@/api/logistics/manufacturing/bom/materialCostAnalysis.api'
import { getBomCostOptionPlantOptions } from '@/api/logistics/manufacturing/bom/costOption.api'
import type { BomMaterialCostItemTransposed } from '@/types/logistics/manufacturing/bom/materialCostAnalysis.types'
import {
  ensureTaktPaginationConfigAsync,
  getTaktDefaultPageIndex,
  getTaktDefaultPageSize,
} from '@/utils/taktPaged'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/tableScroll'
import { resolveCurrentCompanyRelatedPlantCode } from '@/composables/useCompanyRelatedPlant'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { buildDefaultCostingPeriodRange } from '@/views/logistics/manufacturing/bom/material-cost/utils/bomMaterialCostPeriod'
import {
  buildBomExportBaseName,
  buildBomExportFileName,
} from '@/views/logistics/manufacturing/bom/material-cost/utils/bomExportFileName'
import MaterialCostAnalysisQueryForm from './components/MaterialCostItemAnalysisQueryForm.vue'
import { useAnalysisPeriodRangeLimit } from '@/composables/useAnalysisPeriodRangeLimit'
import {
  MATERIAL_COST_ANALYSIS_LOCALE_PREFIX,
  periodRangeToCostingDateQuery,
  useMaterialCostAnalysis,
} from './composables/useMaterialCostItemAnalysis'

const { t } = useI18n()

/** 期间最多 12 个月（查询前校验） */
const { assertPeriodRangeOk } = useAnalysisPeriodRangeLimit()
const localePrefix = MATERIAL_COST_ANALYSIS_LOCALE_PREFIX
const { formatCost, formatPercent, trendLabel, trendClass, varianceClass } = useMaterialCostAnalysis()
const tenantStore = useTenantStore()

/** 工厂 */
const plantCode = ref<string | undefined>()
/** 物料类型（本表；可空=全类型） */
const materialType = ref<string | undefined>()
/** 机种 */
const modelCode = ref<string | undefined>()
/** 产品（可空） */
const productCode = ref<string | undefined>()
/** 核算期间 */
const periodRange = ref<[string, string] | null>(buildDefaultCostingPeriodRange(3))
/** 涨跌筛选：空=全部；changed/up/down */
const trendFilter = ref('')
/** 全量排序（分页前） */
const sortBy = ref('productCode')
/** 右侧：全量排序 + 涨跌筛选 */
const toolbarRightActions = computed<ToolBarAction[]>(() => [
  {
    key: 'sort-product-code',
    icon: RiSortAlphabetAsc,
    tooltip: t(`${localePrefix}.sort.productCode`),
    active: sortBy.value === 'productCode',
    onClick: () => setSortBy('productCode'),
  },
  {
    key: 'sort-trend',
    icon: RiLineChartLine,
    tooltip: t(`${localePrefix}.sort.trend`),
    active: sortBy.value === 'trend',
    onClick: () => setSortBy('trend'),
  },
  {
    key: 'sort-variance-desc',
    icon: RiSortNumberDesc,
    tooltip: t(`${localePrefix}.sort.varianceDesc`),
    active: sortBy.value === 'varianceDesc',
    onClick: () => setSortBy('varianceDesc'),
  },
  {
    key: 'trend-all',
    icon: RiListCheck,
    tooltip: t(`${localePrefix}.filter.all`),
    active: trendFilter.value === '',
    onClick: () => setTrendFilter(''),
  },
  {
    key: 'trend-changed',
    icon: RiArrowUpDownLine,
    tooltip: t(`${localePrefix}.filter.changed`),
    active: trendFilter.value === 'changed',
    onClick: () => setTrendFilter('changed'),
  },
  {
    key: 'trend-up',
    icon: RiArrowUpLine,
    tooltip: t(`${localePrefix}.trend.up`),
    active: trendFilter.value === 'up',
    onClick: () => setTrendFilter('up'),
  },
  {
    key: 'trend-down',
    icon: RiArrowDownLine,
    tooltip: t(`${localePrefix}.trend.down`),
    active: trendFilter.value === 'down',
    onClick: () => setTrendFilter('down'),
  }])
/** 转置行 */
const rows = ref<BomMaterialCostItemTransposed[]>([])
/** 期间列 */
const periodOrder = ref<string[]>([])
/** loading */
const loading = ref(false)
const exportLoading = ref(false)
/** 分页（ensure 前用安全默认值） */
const pageIndex = ref(1)
const pageSize = ref(20)
const total = ref(0)
/** 表体 */
const tableWrapRef = ref<HTMLElement | null>(null)
const tableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
let tableScrollResizeObserver: ResizeObserver | null = null

/** 是否已具备查询条件 */
const hasQuery = computed(
  () => !!plantCode.value?.trim() && !!periodRange.value?.[0],
)

/** 列：机种 / 产品 / 品名 / 周期… / 涨跌 / 环比差额 / 环比% */
const columns = computed<TableColumnsType>(() => {
  const cols: TableColumnsType = [
    {
      title: t('entity.bommaterialcost.modelcode'),
      dataIndex: 'modelCode',
      key: 'modelCode',
      width: 120,
      ellipsis: true,
      fixed: 'left',
    },
    {
      title: t('entity.bommaterialcost.productcode'),
      dataIndex: 'productCode',
      key: 'productCode',
      width: 140,
      ellipsis: true,
      fixed: 'left',
    },
    {
      title: t('entity.bommaterialcost.productdescription'),
      dataIndex: 'productDescription',
      key: 'productDescription',
      width: 180,
      ellipsis: true,
    }]
  for (const period of periodOrder.value) {
    cols.push({
      title: period,
      dataIndex: ['periodCosts', period],
      key: `period_${period}`,
      width: 120,
      align: 'right',
    })
  }
  cols.push(
    {
      title: t(`${localePrefix}.columns.trend`),
      dataIndex: 'trend',
      key: 'trend',
      width: 88,
      fixed: 'right',
    },
    {
      title: t(`${localePrefix}.columns.varianceAmount`),
      dataIndex: 'varianceAmount',
      key: 'varianceAmount',
      width: 120,
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

/** 合计首列文案 */
const summaryLabel = computed(() => t('components.business.page.editabletable.summarylabel'))

/** 全量期间成本合计（API：分页前、已应用涨跌筛选） */
const periodCostTotals = ref<Record<string, number>>({})
/** 全量环比差额合计（API：分页前、已应用涨跌筛选） */
const varianceAmountTotal = ref<number | null>(null)

/**
 * 行主键
 * @param {BomMaterialCostItemTransposed} record 行
 * @returns {string} key
 */
function getRowKey(record: BomMaterialCostItemTransposed): string {
  return `${record.modelCode ?? ''}|${record.productCode}`
}

/**
 * 期间成本
 * @param {BomMaterialCostItemTransposed} record 行
 * @param {string} columnKey period_yyyy-MM
 * @returns {string} 文本
 */
function formatPeriodCost(record: BomMaterialCostItemTransposed, columnKey: string): string {
  const period = columnKey.replace(/^period_/, '')
  const value = record.periodCosts?.[period]
  if (value == null || Number.isNaN(value)) return '—'
  return formatCost(value)
}

/**
 * 构建转置查询（数据源 TaktBomMaterialCost）
 * @param trendFilterOverride 涨跌筛选覆盖值（点工具栏时显式传入，避免与状态不同步）
 * @param sortByOverride 全量排序覆盖值
 * @returns 查询或 null
 */
function buildQuery(trendFilterOverride?: string, sortByOverride?: string) {
  const plant = plantCode.value?.trim()
  const type = materialType.value?.trim()
  if (!plant || !type || !periodRange.value?.[0]) {
    return null
  }
  const rangeEnd = periodRange.value?.[1]?.trim() || periodRange.value[0]
  const filter =
    trendFilterOverride !== undefined ? trendFilterOverride : trendFilter.value
  const sort =
    (sortByOverride !== undefined ? sortByOverride : sortBy.value)?.trim()
    || 'productCode'
  return {
    plantCode: plant,
    materialType: type,
    modelCode: modelCode.value?.trim() || undefined,
    productCode: productCode.value?.trim() || undefined,
    focusPeriod: rangeEnd,
    trendFilter: filter || undefined,
    sortBy: sort,
    pageIndex: pageIndex.value,
    pageSize: pageSize.value,
    ...periodRangeToCostingDateQuery(periodRange.value),
  }
}

/** 清空 */
function clearData() {
  rows.value = []
  periodOrder.value = []
  periodCostTotals.value = {}
  varianceAmountTotal.value = null
  total.value = 0
}

/**
 * 加载
 * @param trendFilterOverride 涨跌筛选覆盖值（与工具栏点击同步）
 * @param sortByOverride 全量排序覆盖值
 */
async function loadData(trendFilterOverride?: string, sortByOverride?: string) {
  const query = buildQuery(trendFilterOverride, sortByOverride)
  if (!query) {
    clearData()
    return
  }
  loading.value = true
  try {
    const result = await getBomMaterialCostItemTransposedList(query)
    rows.value = result.paged?.data ?? []
    total.value = result.paged?.total ?? 0
    periodOrder.value = result.periodOrder ?? []
    periodCostTotals.value = result.periodCostTotals ?? {}
    varianceAmountTotal.value = result.varianceAmountTotal ?? null
  } catch (error: unknown) {
    clearData()
    const err = error as { message?: string }
    message.error(err?.message || t(`${localePrefix}.queryFailed`))
  } finally {
    loading.value = false
    await nextTick()
    recalcTableScrollY()
  }
}

/** 查询 */
function handleSearch() {
  if (!plantCode.value?.trim()) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return
  }
  if (!materialType.value?.trim()) {
    message.warning(t(`${localePrefix}.selectMaterialTypeRequired`))
    return
  }
  if (!periodRange.value?.[0]) {
    message.warning(t(`${localePrefix}.selectPeriodRequired`))
    return
  }
  if (!assertPeriodRangeOk(periodRange.value)) {
    return
  }
  pageIndex.value = getTaktDefaultPageIndex()
  void loadData()
}

/**
 * 涨跌筛选：点涨→up，点跌→down；请求显式带筛选码
 * @param value 空 / changed / up / down
 */
function setTrendFilter(value: string) {
  if (trendFilter.value === value) {
    return
  }
  trendFilter.value = value
  pageIndex.value = getTaktDefaultPageIndex()
  void loadData(value, sortBy.value)
}

/**
 * 全量排序（分页前作用于整表）
 * @param value productCode / trend / varianceDesc
 */
function setSortBy(value: string) {
  if (sortBy.value === value) {
    return
  }
  sortBy.value = value
  if (!hasQuery.value) {
    return
  }
  pageIndex.value = getTaktDefaultPageIndex()
  void loadData(trendFilter.value, value)
}

/** 刷新 */
function handleRefresh() {
  void loadData()
}

/**
 * 默认工厂：当前公司 RelatedPlant 仅当出现在 plant-options（RelatedPlant∩本表）时选中；无则清空
 * @returns {Promise<void>}
 */
async function applyDefaultPlantFromCompany(): Promise<void> {
  const related = (await resolveCurrentCompanyRelatedPlantCode()).trim()
  let matched: string | undefined
  if (related) {
    try {
      const plants = await getBomCostOptionPlantOptions()
      const hit = (plants ?? []).find(
        (o) => String(o.dictValue ?? '').trim().toLowerCase() === related.toLowerCase(),
      )
      matched = hit ? String(hit.dictValue).trim() : undefined
    } catch {
      matched = undefined
    }
  }
  plantCode.value = matched
  materialType.value = undefined
  modelCode.value = undefined
  productCode.value = undefined
}

/** 重置 */
async function handleReset() {
  await applyDefaultPlantFromCompany()
  materialType.value = undefined
  periodRange.value = buildDefaultCostingPeriodRange(3)
  trendFilter.value = ''
  sortBy.value = 'productCode'
  pageIndex.value = getTaktDefaultPageIndex()
  clearData()
}

/**
 * 分页
 * @param {number} page 页码
 * @param {number} size 页大小
 */
async function handlePageChange(page: number, size: number) {
  pageIndex.value = page
  pageSize.value = size
  await loadData()
}

/** 导出 */
async function handleExport() {
  const query = buildQuery()
  if (!query) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return
  }
  exportLoading.value = true
  try {
    const sheetTitle = t(`${localePrefix}.title`)
    const exportBase = buildBomExportBaseName(sheetTitle, [
      query.plantCode,
      query.modelCode,
      query.productCode])
    const exportMeta = await exportBomMaterialCostItemTransposed(
      {
        ...query,
        pageIndex: 1,
        pageSize: 100000,
      },
      sheetTitle,
      buildBomExportFileName(sheetTitle, [
        query.plantCode,
        query.modelCode,
        query.productCode]),
    )
    const blob = (exportMeta as { blob?: Blob }).blob ?? (exportMeta as unknown as Blob)
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as { contentDisposition?: string | null }).contentDisposition ?? null,
      contentType: (exportMeta as { contentType?: string | null }).contentType ?? null,
      fallbackBase: exportBase,
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
  } finally {
    exportLoading.value = false
  }
}

function recalcTableScrollY(): void {
  const wrap = tableWrapRef.value
  if (!wrap || wrap.clientHeight <= 0) return
  tableScrollY.value = Math.max(TAKT_TABLE_SCROLL_Y_MIN, wrap.clientHeight - 47)
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
  () => tenantStore.companyCode,
  () => {
    void (async () => {
      await applyDefaultPlantFromCompany()
      trendFilter.value = ''
      pageIndex.value = getTaktDefaultPageIndex()
      clearData()
    })()
  },
)

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  pageIndex.value = getTaktDefaultPageIndex()
  pageSize.value = getTaktDefaultPageSize()
  await applyDefaultPlantFromCompany()
  await nextTick()
  startTableScrollObserve()
})

onBeforeUnmount(() => {
  stopTableScrollObserve()
})
</script>

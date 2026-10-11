<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/model-cost-trend/components -->
<!-- 文件名称：ModelCostTrendPanel.vue -->
<!-- 功能描述：机种成本推移表（机种、产品、各月成本、相邻月差额、组件差异） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 min-w-0 flex-1 flex-col overflow-hidden">
    <div
      ref="tableWrapRef"
      class="min-h-0 min-w-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        analysis-table
        class="h-full min-h-0"
        entity-scope="company"
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
            {{ formatCost(periodCost(record as BomMaterialCostItemModelCostTrend, String(column.key))) }}
          </template>
          <template v-else-if="column.key === 'varianceAmount'">
            <span :class="varianceClass((record as BomMaterialCostItemModelCostTrend).varianceAmount)">
              {{ formatCost((record as BomMaterialCostItemModelCostTrend).varianceAmount) }}
            </span>
          </template>
          <template v-else-if="column.key === 'commonPrice'">
            {{ formatCost((record as BomMaterialCostItemModelCostTrend).commonPrice) }}
          </template>
          <template v-else-if="column.key === 'variancePrice'">
            {{ formatCost((record as BomMaterialCostItemModelCostTrend).variancePrice) }}
          </template>
          <template v-else>
            {{ text }}
          </template>
        </template>
        <template #footerRemark>
          <div class="space-y-1">
            <div class="truncate">
              {{ summaryText }}
            </div>
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
                {{ t(`${localePrefix}.columns.variance`) }}: {{ formatCost(varianceAmountTotal) }}
              </span>
              <span class="shrink-0">
                {{ t(`${localePrefix}.columns.commonPrice`) }}: {{ formatCost(commonPriceTotal) }}
              </span>
              <span class="shrink-0">
                {{ t(`${localePrefix}.columns.variancePrice`) }}: {{ formatCost(variancePriceTotal) }}
              </span>
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
 * 机种成本推移：一行一个成品，列为各月成本、相邻月差额、组件差异
 */
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/tableScroll'
import {
  exportBomMaterialCostItemModelCostTrendAnalysis,
  getBomMaterialCostItemModelCostTrendAnalysis,
} from '@/api/logistics/manufacturing/bom/modelCostTrend.api'
import type { BomMaterialCostItemModelCostTrend } from '@/types/logistics/manufacturing/bom/modelCostTrend.types'
import {
  ensureTaktPaginationConfigAsync,
  getTaktDefaultPageIndex,
  getTaktDefaultPageSize,
} from '@/utils/taktPaged'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import {
  buildBomExportBaseName,
  buildBomExportFileName,
} from '@/views/logistics/manufacturing/bom/material-cost/utils/bomExportFileName'
import { periodRangeToMovingPricePeriodQuery } from '@/views/logistics/manufacturing/bom/material-cost-trend/composables/useMaterialCostItemAnalysis'
import { useBomMaterialCostAnalysisMasterContext } from '@/views/logistics/manufacturing/bom/material-cost-trend/composables/useMaterialCostAnalysisMasterContext'

const loading = defineModel<boolean>('loading', { default: false })
const hasRows = defineModel<boolean>('hasRows', { default: false })

/** 静态 locales 前缀 */
const localePrefix = 'logistics.manufacturing.bom.model-cost-trend.page'
const { t } = useI18n()
const {
  queryPlantCode,
  queryMaterialType,
  queryModelCode,
  periodRange,
} = useBomMaterialCostAnalysisMasterContext()

/** 成品行 */
const rows = ref<BomMaterialCostItemModelCostTrend[]>([])
/** 期间列 */
const periodOrder = ref<string[]>([])
/** 组件差异月份 */
const comparePeriod = ref('')
/** 成品数 */
const productCount = ref(0)
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
/** 各月合计 */
const periodCostTotals = ref<Record<string, number>>({})
/** 差额合计 */
const varianceAmountTotal = ref<number | null>(null)
/** 共通价格合计 */
const commonPriceTotal = ref<number | null>(null)
/** 差异价格合计 */
const variancePriceTotal = ref<number | null>(null)

/** 摘要 */
const summaryText = computed(() => {
  if (!queryPlantCode.value?.trim()) {
    return t(`${localePrefix}.selectPlantRequired`)
  }
  if (!queryMaterialType.value?.trim()) {
    return t(`${localePrefix}.selectMaterialTypeRequired`)
  }
  if (!periodRange.value?.[0] || !periodRange.value?.[1]) {
    return t(`${localePrefix}.selectPeriodRequired`)
  }
  const model = queryModelCode.value?.trim()
  return t(`${localePrefix}.summary`, {
    plant: queryPlantCode.value,
    materialType: queryMaterialType.value,
    period: `${periodRange.value[0]} ~ ${periodRange.value[1]}`,
    model: model || t(`${localePrefix}.modelAll`),
    productCount: productCount.value,
    compare: comparePeriod.value || periodRange.value[1],
  })
})

/** 列：机种、产品、产品描述、各月成本、差异、共通价格、差异价格、组件差异 */
const columns = computed<TableColumnsType>(() => {
  const cols: TableColumnsType = [
    {
      title: t(`${localePrefix}.columns.model`),
      dataIndex: 'modelCode',
      key: 'modelCode',
      width: 120,
      ellipsis: true,
      fixed: 'left',
    },
    {
      title: t(`${localePrefix}.columns.product`),
      dataIndex: 'productCode',
      key: 'productCode',
      width: 140,
      ellipsis: true,
      fixed: 'left',
    },
    {
      title: t(`${localePrefix}.columns.productDescription`),
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
      width: 130,
      align: 'right',
    })
  }
  cols.push({
    title: t(`${localePrefix}.columns.variance`),
    dataIndex: 'varianceAmount',
    key: 'varianceAmount',
    width: 120,
    align: 'right',
  })
  cols.push({
    title: t(`${localePrefix}.columns.commonPrice`),
    dataIndex: 'commonPrice',
    key: 'commonPrice',
    width: 130,
    align: 'right',
  })
  cols.push({
    title: t(`${localePrefix}.columns.variancePrice`),
    dataIndex: 'variancePrice',
    key: 'variancePrice',
    width: 130,
    align: 'right',
  })
  cols.push({
    title: t(`${localePrefix}.columns.componentVariance`),
    dataIndex: 'componentVarianceText',
    key: 'componentVarianceText',
    width: 280,
    ellipsis: true,
  })
  return cols
})

/** 可见列键 */
const visibleColumnKeys = computed(() => columns.value.map((c) => String(c.key)))

/** 合计首列文案 */
const summaryLabel = computed(() => t('components.business.page.editabletable.summarylabel'))

/**
 * 行主键
 * @param {BomMaterialCostItemModelCostTrend} record 行
 * @returns {string} key
 */
function getRowKey(record: BomMaterialCostItemModelCostTrend): string {
  return [record.plantCode, record.modelCode, record.productCode].join('|')
}

/**
 * 千分位金额（5 位小数）
 * @param {number | null | undefined} value 数值
 * @returns {string} 文本
 */
function formatCost(value?: number | null): string {
  if (value == null || Number.isNaN(value)) return ''
  return value.toLocaleString('en-US', { minimumFractionDigits: 5, maximumFractionDigits: 5 })
}

/**
 * 差额样式
 * @param {number | null | undefined} value 差额
 * @returns {string} class
 */
function varianceClass(value?: number | null): string {
  if (value == null || value === 0) return ''
  if (value > 0) return 'text-red-600'
  return 'text-green-600'
}

/**
 * 期间成本
 * @param {BomMaterialCostItemModelCostTrend} record 行
 * @param {string} columnKey period_yyyy-MM
 * @returns {number | null} 成本
 */
function periodCost(record: BomMaterialCostItemModelCostTrend, columnKey: string): number | null {
  const period = columnKey.replace(/^period_/, '')
  const value = record.periodCosts?.[period]
  return value == null || Number.isNaN(value) ? null : value
}

/**
 * 构建查询
 * @returns 查询 DTO 或 null
 */
function buildQuery() {
  const plantCode = queryPlantCode.value?.trim()
  const materialType = queryMaterialType.value?.trim()
  if (!plantCode || !materialType || !periodRange.value?.[0] || !periodRange.value?.[1]) {
    return null
  }
  const model = queryModelCode.value?.trim()
  return {
    plantCode,
    materialType,
    modelCode: model || undefined,
    pageIndex: pageIndex.value,
    pageSize: pageSize.value,
    ...periodRangeToMovingPricePeriodQuery(periodRange.value),
  }
}

/** 清空 */
function clear() {
  rows.value = []
  periodOrder.value = []
  comparePeriod.value = ''
  productCount.value = 0
  periodCostTotals.value = {}
  varianceAmountTotal.value = null
  commonPriceTotal.value = null
  variancePriceTotal.value = null
  total.value = 0
  pageIndex.value = getTaktDefaultPageIndex()
  hasRows.value = false
}

/** 加载 */
async function loadData() {
  const query = buildQuery()
  if (!query) {
    clear()
    return
  }
  loading.value = true
  try {
    const result = await getBomMaterialCostItemModelCostTrendAnalysis(query)
    rows.value = result.paged?.data ?? []
    total.value = result.paged?.total ?? 0
    periodOrder.value = result.periodOrder ?? []
    comparePeriod.value = result.comparePeriod ?? ''
    productCount.value = result.productCount ?? total.value
    periodCostTotals.value = result.periodCostTotals ?? {}
    varianceAmountTotal.value = result.varianceAmountTotal ?? null
    commonPriceTotal.value = result.commonPriceTotal ?? null
    variancePriceTotal.value = result.variancePriceTotal ?? null
    hasRows.value = rows.value.length > 0
  } catch (error: unknown) {
    clear()
    const err = error as { message?: string }
    message.error(err?.message || t(`${localePrefix}.queryFailed`))
  } finally {
    loading.value = false
    await nextTick()
    recalcTableScrollY()
  }
}

/** 重新加载（重置页码） */
async function reload() {
  pageIndex.value = getTaktDefaultPageIndex()
  await loadData()
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
  try {
    const sheetTitle = t(`${localePrefix}.title`)
    const exportBase = buildBomExportBaseName(sheetTitle, [
      query.plantCode,
      query.modelCode,
      query.materialType])
    const exportMeta = await exportBomMaterialCostItemModelCostTrendAnalysis(
      {
        ...query,
        pageIndex: 1,
        pageSize: 100000,
      },
      sheetTitle,
      buildBomExportFileName(sheetTitle, [
        query.plantCode,
        query.modelCode,
        query.materialType]),
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
  }
}

/** 实测 scroll.y（扣除表头） */
function recalcTableScrollY(): void {
  const wrap = tableWrapRef.value
  if (!wrap || wrap.clientHeight <= 0) return
  const y = Math.floor(wrap.clientHeight - 47)
  tableScrollY.value = Math.max(TAKT_TABLE_SCROLL_Y_MIN, y)
}

/** 监听外壳 */
function startTableScrollObserve(): void {
  stopTableScrollObserve()
  const wrap = tableWrapRef.value
  if (!wrap) return
  recalcTableScrollY()
  tableScrollResizeObserver = new ResizeObserver(() => {
    recalcTableScrollY()
  })
  tableScrollResizeObserver.observe(wrap)
}

/** 停止监听 */
function stopTableScrollObserve(): void {
  tableScrollResizeObserver?.disconnect()
  tableScrollResizeObserver = null
}

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  pageSize.value = getTaktDefaultPageSize()
  await nextTick()
  startTableScrollObserve()
})

onBeforeUnmount(() => {
  stopTableScrollObserve()
})

defineExpose({ reload, loadData, handleExport, clear })
</script>

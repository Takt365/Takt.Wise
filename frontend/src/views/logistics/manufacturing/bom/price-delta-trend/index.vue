<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/price-delta-trend -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：成本差异推移独立页（月成本 + 0价格组 + 价格差异组） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 min-w-0 w-full flex-col overflow-hidden p-4">
    <PriceDeltaTrendQueryForm
      v-model:plant-code="plantCode"
      v-model:material-type="materialType"
      v-model:model-code="modelCode"
      v-model:product-code="productCode"
      v-model:period-range="periodRange"
      v-model:base-period="basePeriod"
      v-model:compare-period="comparePeriod"
      v-model:price-delta-option="priceDeltaOption"
      v-model:impact-line-option="impactLineOption"
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
      export-permission="logistics:manufacturing:bom:pricedelta:trend:export"
      @export="handleExport"
      @refresh="handleRefresh"
    />
    <div
      v-if="hintBasePeriod && hintComparePeriod"
      class="mb-2 text-sm text-text-secondary"
    >
      {{ t(`${localePrefix}.compareHint`, { base: hintBasePeriod, compare: hintComparePeriod, delta: hintPriceDeltaLabel, impact: hintImpactLineLabel }) }}
    </div>
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
        :virtual="true"
        :row-key="getRowKey"
        :pagination="false"
        :show-row-selection="false"
        :scroll="{ x: 'max-content', y: tableScrollY }"
      >
        <template #bodyCell="{ column, record, text }">
          <template v-if="String(column.key).startsWith('period_')">
            {{ formatPeriodCost(record as BomPriceDeltaTrend, String(column.key)) }}
          </template>
          <template v-else-if="column.key === 'priceDelta'">
            {{ formatPriceDelta(record as BomPriceDeltaTrend) }}
          </template>
          <template
            v-else-if="
              column.key === 'zeroPriceGroup'
                || column.key === 'replaceComponentGroup'
                || column.key === 'priceDeltaTrend'
                || column.key === 'componentDeltaGroup'
                || column.key === 'reworkOrderGroup'
                || column.key === 'productDescription'
            "
          >
            <a-tooltip
              v-if="String(text ?? '').trim()"
              placement="topLeft"
              :overlay-style="longTextTooltipStyle"
            >
              <template #title>
                <div class="max-h-80 max-w-md overflow-auto whitespace-pre-wrap break-all text-xs">
                  {{ text }}
                </div>
              </template>
              <span class="block max-w-full truncate">{{ text }}</span>
            </a-tooltip>
            <span v-else>—</span>
          </template>
          <template v-else>
            {{ text }}
          </template>
        </template>
      </TaktSingleTable>
    </div>
    <TaktPagination
      v-if="total > 0"
      class="mt-2 shrink-0"
      :current="pageIndex"
      :page-size="pageSize"
      :total="total"
      :disabled="loading"
      @change="handlePageChange"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 成本差异推移：产品月成本 + 0价格组 + PriceDeltaTrend
 */
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getBomCostOptionPlantOptions } from '@/api/logistics/manufacturing/bom/costOption.api'
import {
  exportBomPriceDeltaTrendData,
  getBomPriceDeltaTrendList,
} from '@/api/logistics/manufacturing/bom/priceDeltaTrend.api'
import type { BomPriceDeltaTrend } from '@/types/logistics/manufacturing/bom/priceDeltaTrend.types'
import {
  ensureTaktPaginationConfigAsync,
  getTaktDefaultPageIndex,
  getTaktDefaultPageSize,
} from '@/utils/taktPaged'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { measureFillHeightScrollYPx, TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/tableScroll'
import { resolveCurrentCompanyRelatedPlantCode } from '@/composables/useCompanyRelatedPlant'
import { useTableRefresh } from '@/composables/useTableRefresh'
import {
  buildDefaultCostingPeriodRange,
  buildDefaultPriceDeltaMonths,
  isCostingMonthInRange,
  periodRangeToCostingDateQuery,
} from '@/views/logistics/manufacturing/bom/material-cost/utils/bomMaterialCostPeriod'
import {
  buildBomExportBaseName,
  buildBomExportFileName,
} from '@/views/logistics/manufacturing/bom/material-cost/utils/bomExportFileName'
import PriceDeltaTrendQueryForm from '@/views/logistics/manufacturing/bom/price-delta-trend/components/PriceDeltaTrendQueryForm.vue'
import { useAnalysisPeriodRangeLimit } from '@/composables/useAnalysisPeriodRangeLimit'

/** 长文本 Tooltip 样式（限制悬停层尺寸，避免整页撑爆） */
const longTextTooltipStyle = {
  maxWidth: '480px',
} as const

const localePrefix = 'logistics.manufacturing.bom.price-delta-trend.page'
const { t } = useI18n()

/** 期间最多 12 个月（查询前校验） */
const { assertPeriodRangeOk } = useAnalysisPeriodRangeLimit()

/** 工厂 */
const plantCode = ref<string | undefined>()
/** 物料类型（默认 FERT） */
const materialType = ref<string | undefined>()
/** 机种（可选） */
const modelCode = ref<string | undefined>()
/** 产品（可选） */
const productCode = ref<string | undefined>()
/** 核算期间 */
const periodRange = ref<[string, string] | null>(buildDefaultCostingPeriodRange(3))
const defaultDeltaMonths = buildDefaultPriceDeltaMonths()
/** 基准月（关注月；默认前月） */
const basePeriod = ref<string | undefined>(defaultDeltaMonths.basePeriod)
/** 比较月（基期；默认基准月减一月） */
const comparePeriod = ref<string | undefined>(defaultDeltaMonths.comparePeriod)
/** 差异选项：all=全部（默认）；gt1/gt5/gt10/gt50/gt100 界面为 >=N，按 |差异| 过滤产品行 */
const priceDeltaOption = ref('all')
/** 价格差异组涨跌：up3down3（默认）/ up5down5 / all */
const impactLineOption = ref('up3down3')
/** 行 */
const rows = ref<BomPriceDeltaTrend[]>([])
/** 期间列 */
const periodOrder = ref<string[]>([])
/** 上次查询生效的基准月 */
const hintBasePeriod = ref('')
/** 上次查询生效的比较月 */
const hintComparePeriod = ref('')
/** 上次查询生效的差异选项码 all / gt1 / gt5 / gt10 / gt50 / gt100 */
const hintPriceDeltaOption = ref('')
/** 上次查询生效的价格差异组涨跌码 up3down3 / up5down5 / all */
const hintImpactLineOption = ref('')
/**
 * 提示用差异选项展示：全部走 i18n；阈值四语同一符号 >=N
 * @returns 展示文案
 */
const hintPriceDeltaLabel = computed(() => formatPriceDeltaOptionLabel(hintPriceDeltaOption.value))
/**
 * 提示用价格差异组涨跌文案
 * @returns 展示文案
 */
const hintImpactLineLabel = computed(() => formatImpactLineOptionLabel(hintImpactLineOption.value))
const loading = ref(false)
const exportLoading = ref(false)
const pageIndex = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const tableWrapRef = ref<HTMLElement | null>(null)
const tableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
let tableScrollResizeObserver: ResizeObserver | null = null

const hasQuery = computed(
  () =>
    !!plantCode.value?.trim()
    && !!materialType.value?.trim()
    && !!periodRange.value?.[0]
    && !!periodRange.value?.[1]
    && !!basePeriod.value?.trim()
    && !!comparePeriod.value?.trim(),
)

/**
 * 可空差异数值比较（null/undefined 靠后）
 * @param a 左值
 * @param b 右值
 * @returns 比较结果
 */
function compareNullablePriceDelta(
  a: number | null | undefined,
  b: number | null | undefined,
): number {
  if (a == null && b == null) {
    return 0
  }
  if (a == null) {
    return 1
  }
  if (b == null) {
    return -1
  }
  return Number(a) - Number(b)
}

const columns = computed<TableColumnsType>(() => {
  const cols: TableColumnsType = [
    {
      title: t('entity.bommaterialcost.modelcode'),
      dataIndex: 'modelCode',
      key: 'modelCode',
      width: 120,
      ellipsis: true,
      fixed: 'left',
      sorter: (a: BomPriceDeltaTrend, b: BomPriceDeltaTrend) =>
        String(a.modelCode ?? '').localeCompare(String(b.modelCode ?? ''), undefined, {
          numeric: true,
          sensitivity: 'base',
        }),
    },
    {
      title: t('entity.bommaterialcost.productcode'),
      dataIndex: 'productCode',
      key: 'productCode',
      width: 140,
      ellipsis: true,
      fixed: 'left',
      sorter: (a: BomPriceDeltaTrend, b: BomPriceDeltaTrend) =>
        String(a.productCode ?? '').localeCompare(String(b.productCode ?? ''), undefined, {
          numeric: true,
          sensitivity: 'base',
        }),
    },
    {
      title: t('entity.bommaterialcost.productdescription'),
      dataIndex: 'productDescription',
      key: 'productDescription',
      width: 200,
      ellipsis: true,
    },
  ]
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
      title: t(`${localePrefix}.priceDelta`),
      dataIndex: 'priceDelta',
      key: 'priceDelta',
      width: 100,
      align: 'right',
      sorter: (a: BomPriceDeltaTrend, b: BomPriceDeltaTrend) =>
        compareNullablePriceDelta(a.priceDelta, b.priceDelta),
    },
    {
      title: t(`${localePrefix}.zeroPriceGroup`),
      dataIndex: 'zeroPriceGroup',
      key: 'zeroPriceGroup',
      width: 200,
      ellipsis: true,
    },
    {
      title: t(`${localePrefix}.replaceComponentGroup`),
      dataIndex: 'replaceComponentGroup',
      key: 'replaceComponentGroup',
      width: 200,
      ellipsis: true,
    },
    {
      title: t(`${localePrefix}.priceDeltaTrend`),
      dataIndex: 'priceDeltaTrend',
      key: 'priceDeltaTrend',
      width: 220,
      ellipsis: true,
    },
    {
      title: t(`${localePrefix}.componentDeltaGroup`),
      dataIndex: 'componentDeltaGroup',
      key: 'componentDeltaGroup',
      width: 220,
      ellipsis: true,
    },
    {
      title: t(`${localePrefix}.reworkOrderGroup`),
      dataIndex: 'reworkOrderGroup',
      key: 'reworkOrderGroup',
      width: 200,
      ellipsis: true,
    },
  )
  return cols
})

const visibleColumnKeys = computed(() => columns.value.map((c) => String(c.key)))

/**
 * 行主键
 * @param record 行
 * @returns key
 */
function getRowKey(record: BomPriceDeltaTrend): string {
  return `${record.plantCode}|${record.modelCode}|${record.productCode}`
}

/**
 * 格式化期间成本
 * @param record 行
 * @param columnKey period_yyyy-MM
 * @returns 文本
 */
function formatPeriodCost(record: BomPriceDeltaTrend, columnKey: string): string {
  const period = columnKey.replace(/^period_/, '')
  const value = record.periodCosts?.[period]
  if (value === null || value === undefined || Number.isNaN(Number(value))) {
    return '—'
  }
  return Number(value).toFixed(5)
}

/**
 * 格式化差异（基准月 − 比较月）
 * @param record 行
 * @returns 文本
 */
function formatPriceDelta(record: BomPriceDeltaTrend): string {
  const value = record.priceDelta
  if (value === null || value === undefined || Number.isNaN(Number(value))) {
    return '—'
  }
  return Number(value).toFixed(5)
}

/**
 * 差异选项展示文案：全部翻译；阈值固定 >=N
 * @param option 选项码
 * @returns 提示用文案
 */
function formatPriceDeltaOptionLabel(option: string): string {
  const key = option.trim()
  if (!key || key === 'all') {
    return t(`${localePrefix}.priceDeltaOptionAll`)
  }
  if (key === 'gt1') {
    return '>=1'
  }
  if (key === 'gt5') {
    return '>=5'
  }
  if (key === 'gt10') {
    return '>=10'
  }
  if (key === 'gt50') {
    return '>=50'
  }
  if (key === 'gt100') {
    return '>=100'
  }
  return key
}

/**
 * 价格差异组涨跌展示文案
 * @param option 选项码
 * @returns 提示用文案
 */
function formatImpactLineOptionLabel(option: string): string {
  const key = option.trim() || 'up3down3'
  if (key === 'up5down5') {
    return t(`${localePrefix}.impactLineUp5Down5`)
  }
  if (key === 'all') {
    return t(`${localePrefix}.impactLineAll`)
  }
  return t(`${localePrefix}.impactLineUp3Down3`)
}

/** 构建查询 */
function buildQuery(overrides?: Record<string, unknown>) {
  const plant = plantCode.value?.trim() ?? ''
  const dates = periodRangeToCostingDateQuery(periodRange.value)
  return {
    pageIndex: pageIndex.value,
    pageSize: pageSize.value,
    plantCode: plant,
    materialType: materialType.value?.trim() || undefined,
    modelCode: modelCode.value?.trim() || undefined,
    productCode: productCode.value?.trim() || undefined,
    costingDateStart: dates.costingDateStart,
    costingDateEnd: dates.costingDateEnd,
    basePeriod: basePeriod.value?.trim() || undefined,
    comparePeriod: comparePeriod.value?.trim() || undefined,
    priceDeltaOption: priceDeltaOption.value?.trim() || 'all',
    impactLineOption: impactLineOption.value?.trim() || 'up3down3',
    ...overrides,
  }
}

/** 加载 */
async function loadData() {
  if (!hasQuery.value) {
    rows.value = []
    total.value = 0
    periodOrder.value = []
    return
  }
  loading.value = true
  try {
    const res = await getBomPriceDeltaTrendList(buildQuery())
    rows.value = res.paged?.data ?? []
    total.value = res.paged?.total ?? 0
    periodOrder.value = res.periodOrder ?? []
    hintBasePeriod.value = res.basePeriod ?? ''
    hintComparePeriod.value = res.comparePeriod ?? ''
    hintPriceDeltaOption.value = priceDeltaOption.value?.trim() || 'all'
    hintImpactLineOption.value = impactLineOption.value?.trim() || 'up3down3'
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t(`${localePrefix}.queryFailed`))
    rows.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

useTableRefresh(loadData)

/** 查询 */
async function handleSearch() {
  if (!plantCode.value?.trim()) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return
  }
  if (!periodRange.value?.[0] || !periodRange.value?.[1]) {
    message.warning(t(`${localePrefix}.selectPeriodRequired`))
    return
  }
  if (!assertPeriodRangeOk(periodRange.value)) {
    return
  }
  if (!materialType.value?.trim()) {
    message.warning(t(`${localePrefix}.selectMaterialTypeRequired`))
    return
  }
  if (!basePeriod.value?.trim()) {
    message.warning(t(`${localePrefix}.selectBasePeriodRequired`))
    return
  }
  if (!comparePeriod.value?.trim()) {
    message.warning(t(`${localePrefix}.selectComparePeriodRequired`))
    return
  }
  if (
    !isCostingMonthInRange(basePeriod.value, periodRange.value)
    || !isCostingMonthInRange(comparePeriod.value, periodRange.value)
  ) {
    message.warning(t(`${localePrefix}.selectDeltaMonthInRange`))
    return
  }
  pageIndex.value = getTaktDefaultPageIndex()
  await loadData()
}

/** 重置 */
async function handleReset() {
  materialType.value = undefined
  modelCode.value = undefined
  productCode.value = undefined
  periodRange.value = buildDefaultCostingPeriodRange(3)
  const deltaMonths = buildDefaultPriceDeltaMonths(periodRange.value)
  basePeriod.value = deltaMonths.basePeriod
  comparePeriod.value = deltaMonths.comparePeriod
  priceDeltaOption.value = 'all'
  impactLineOption.value = 'up3down3'
  pageIndex.value = getTaktDefaultPageIndex()
  rows.value = []
  total.value = 0
  periodOrder.value = []
  hintBasePeriod.value = ''
  hintComparePeriod.value = ''
  hintPriceDeltaOption.value = ''
  hintImpactLineOption.value = ''
  await applyDefaultPlant()
}

/**
 * 默认工厂：当前公司 RelatedPlant 仅当出现在本页 plant-options（RelatedPlant∩本表）时选中；无则清空
 * @returns {Promise<void>}
 */
async function applyDefaultPlant(): Promise<void> {
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
}

/** 刷新 */
async function handleRefresh() {
  await loadData()
}

/**
 * 分页
 * @param page 页码
 * @param size 页大小
 */
async function handlePageChange(page: number, size: number) {
  pageIndex.value = page
  pageSize.value = size
  await loadData()
}

/** 导出 */
async function handleExport() {
  if (!hasQuery.value) {
    return
  }
  exportLoading.value = true
  try {
    const query = buildQuery()
    const plant = plantCode.value?.trim() ?? ''
    const sheetTitle = t(`${localePrefix}.title`)
    const exportBase = buildBomExportBaseName(sheetTitle, [plant])
    const exportMeta = await exportBomPriceDeltaTrendData(
      query,
      sheetTitle,
      buildBomExportFileName(sheetTitle, [plant]),
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

/**
 * 重算表体 scroll.y：容器高度减去表头，避免 y 过大把底部横向滚动条裁掉
 */
function recalcTableScrollY(): void {
  const wrap = tableWrapRef.value
  if (!wrap || wrap.clientHeight <= 0) {
    return
  }
  tableScrollY.value = measureFillHeightScrollYPx(wrap, { subtractTableHeader: true })
}

/**
 * 监听表格容器尺寸，保持纵向/横向滚动条可见
 */
function bindTableScroll(): void {
  tableScrollResizeObserver?.disconnect()
  const el = tableWrapRef.value
  if (!el) {
    return
  }
  recalcTableScrollY()
  tableScrollResizeObserver = new ResizeObserver(() => {
    recalcTableScrollY()
  })
  tableScrollResizeObserver.observe(el)
}

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  pageIndex.value = getTaktDefaultPageIndex()
  pageSize.value = getTaktDefaultPageSize()
  await nextTick()
  bindTableScroll()
  requestAnimationFrame(() => {
    recalcTableScrollY()
  })
  await applyDefaultPlant()
})

onBeforeUnmount(() => {
  tableScrollResizeObserver?.disconnect()
  tableScrollResizeObserver = null
})
</script>

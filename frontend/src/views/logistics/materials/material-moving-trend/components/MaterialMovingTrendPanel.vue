<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/materials/material-moving-trend/components -->
<!-- 文件名称：MaterialMovingTrendPanel.vue -->
<!-- 功能描述：物料月移动价格推移转置涨跌表 -->
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
        :id-column-key="'materialCode'"
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
            <template v-if="isCarriedPeriodPrice(record as MaterialMovingTrend, String(column.key))">
              <a-tooltip
                :title="carriedPeriodTooltip(record as MaterialMovingTrend, String(column.key))"
              >
                <span class="cursor-help border-b border-dotted border-text-secondary text-text-secondary">
                  {{ formatPeriodPrice(record as MaterialMovingTrend, String(column.key)) }}*
                </span>
              </a-tooltip>
            </template>
            <template v-else>
              {{ formatPeriodPrice(record as MaterialMovingTrend, String(column.key)) }}
            </template>
          </template>
          <template v-else-if="column.key === 'currencyCode'">
            <TaktDictTag
              :value="(record as MaterialMovingTrend).currencyCode"
              dict-type="accounting_financial_currency_code"
            />
          </template>
          <template v-else-if="column.key === 'valuation'">
            <TaktDictTag
              :value="(record as MaterialMovingTrend).valuation"
              dict-type="logistics_materials_valuation_class"
            />
          </template>
          <template v-else-if="column.key === 'trend'">
            <span :class="trendClass((record as MaterialMovingTrend).trend || 'none')">
              {{ trendLabel((record as MaterialMovingTrend).trend || 'none') }}
            </span>
          </template>
          <template v-else-if="column.key === 'varianceAmount'">
            <span :class="varianceClass((record as MaterialMovingTrend).varianceAmount)">
              {{ formatCost((record as MaterialMovingTrend).varianceAmount) }}
            </span>
          </template>
          <template v-else-if="column.key === 'variancePercent'">
            <span :class="varianceClass((record as MaterialMovingTrend).varianceAmount)">
              {{ formatPercent((record as MaterialMovingTrend).variancePercent) }}
            </span>
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
              v-if="hasQuery && comparePeriod"
              class="text-text"
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
 * 物料移动价格推移转置涨跌表
 */
import { formatAnalysisVariancePercent } from '@/utils/analysisPercent'
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import {
  exportMaterialMovingTrendAnalysis,
  getMaterialMovingTrendAnalysis,
} from '@/api/logistics/materials/materialMovingTrend.api'
import type { MaterialMovingTrend } from '@/types/logistics/materials/materialMovingTrend.types'
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
  /** 期间年月 */
  periodRange?: [string, string] | null
  /** 评估类别 */
  valuation?: string
  /** 物料编码 */
  materialCode?: string
}>()

const loading = defineModel<boolean>('loading', { default: false })
const hasRows = defineModel<boolean>('hasRows', { default: false })

/** 静态 locales 前缀 */
const localePrefix = 'logistics.materials.material-moving-trend.page'
const { t } = useI18n()

/** 行数据 */
const rows = ref<MaterialMovingTrend[]>([])
/** 期间列 */
const periodOrder = ref<string[]>([])
/** 物料行总数（筛选后） */
const materialCount = ref(0)
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

/** 工厂已选 */
const hasQuery = computed(() => !!props.plantCode?.trim())

/** 摘要 */
const summaryText = computed(() => {
  if (!hasQuery.value) {
    return t(`${localePrefix}.selectPlantRequired`)
  }
  return t(`${localePrefix}.summary`, { count: materialCount.value })
})

/** 动态列 */
const columns = computed<TableColumnsType>(() => {
  const cols: TableColumnsType = [
    {
      title: t('entity.materialmovingprice.materialcode'),
      dataIndex: 'materialCode',
      key: 'materialCode',
      width: 140,
      ellipsis: true,
      fixed: 'left',
    },
    {
      title: t('entity.materialdescription.materialdescription'),
      dataIndex: 'materialDescription',
      key: 'materialDescription',
      width: 160,
      ellipsis: true,
    },
    {
      title: t('entity.materialmovingprice.valuation'),
      dataIndex: 'valuation',
      key: 'valuation',
      width: 100,
    },
    {
      title: t('entity.materialmovingprice.currencycode'),
      dataIndex: 'currencyCode',
      key: 'currencyCode',
      width: 80,
    }]
  for (const period of periodOrder.value) {
    cols.push({
      title: period,
      dataIndex: ['periodUnitPrices', period],
      key: `period_${period}`,
      width: 110,
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
  )
  return cols
})

/** 可见列键 */
const visibleColumnKeys = computed(() => columns.value.map((c) => String(c.key)))

/**
 * 行主键
 * @param {MaterialMovingTrend} record 行
 * @returns {string} key
 */
function getRowKey(record: MaterialMovingTrend): string {
  return `${record.plantCode}|${record.materialCode}`
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
 * 格式化环比变动率（API 为小数比率，如 0.2978 → 29.78%）
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
 * 格式化期间单价
 * @param {MaterialMovingTrend} record 行
 * @param {string} columnKey period_yyyy-MM
 * @returns {string} 文本
 */
function formatPeriodPrice(record: MaterialMovingTrend, columnKey: string): string {
  const period = columnKey.replace(/^period_/, '')
  const value = record.periodUnitPrices?.[period]
  if (value == null || Number.isNaN(value)) return '—'
  return value.toFixed(5)
}

/**
 * 是否回填价
 * @param {MaterialMovingTrend} record 行
 * @param {string} columnKey period_yyyy-MM
 * @returns {boolean} 是否回填
 */
function isCarriedPeriodPrice(record: MaterialMovingTrend, columnKey: string): boolean {
  const period = columnKey.replace(/^period_/, '')
  const price = record.periodUnitPrices?.[period]
  if (price == null || Number.isNaN(price)) return false
  const source = record.periodPriceSourcePeriods?.[period]
  if (!source) return false
  return String(source) !== String(period)
}

/**
 * 回填悬停提示
 * @param {MaterialMovingTrend} record 行
 * @param {string} columnKey period_yyyy-MM
 * @returns {string} 提示
 */
function carriedPeriodTooltip(record: MaterialMovingTrend, columnKey: string): string {
  const period = columnKey.replace(/^period_/, '')
  const source = record.periodPriceSourcePeriods?.[period] || '—'
  return t(`${localePrefix}.carriedFrom`, { period: source })
}

/**
 * 期间区间 → API 参数（与销售价格推移一致：periodDateStart/End 为当月首日，供后端 DateTime? 绑定）
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
    valuation: props.valuation?.trim() || undefined,
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
    const result = await getMaterialMovingTrendAnalysis(query)
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

/** 重新加载（重置页码） */
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
    const exportMeta = await exportMaterialMovingTrendAnalysis(
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
    if (props.plantCode?.trim()) {
      void reload()
    }
  },
)

useTableRefresh(reload)

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  pageSize.value = getTaktDefaultPageSize()
  await nextTick()
  startTableScrollObserve()
})

onBeforeUnmount(() => {
  stopTableScrollObserve()
})

defineExpose({ reload, handleExport, clear })
</script>

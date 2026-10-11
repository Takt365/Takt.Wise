<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/purchase-actual-trend/components -->
<!-- 文件名称：PurchaseActualTrendPanel.vue -->
<!-- 功能描述：采购实绩推移：柱形图（默认）/ 推移表 / 明细；涨跌=(本月单价−上月单价)×本月数量 -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 flex-1 flex-col overflow-hidden">
    <a-tabs
      v-model:active-key="activeTab"
      class="flex min-h-0 flex-1 flex-col overflow-hidden [&_.ant-tabs-content-holder]:min-h-0 [&_.ant-tabs-content-holder]:flex-1 [&_.ant-tabs-content]:h-full [&_.ant-tabs-tabpane]:h-full"
      size="small"
    >
      <a-tab-pane
        key="chart"
        :tab="t(`${localePrefix}.tabs.chart`)"
        :force-render="true"
      >
        <div class="flex h-full min-h-0 flex-col overflow-hidden">
          <div
            ref="chartWrapRef"
            class="min-h-0 flex-1 overflow-hidden"
          >
            <div
              ref="chartElRef"
              class="h-full w-full min-h-[280px]"
            />
          </div>
          <div class="mt-2 shrink-0 truncate text-sm text-text-secondary">
            {{ chartSummaryText }}
          </div>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="trend"
        :tab="t(`${localePrefix}.tabs.trend`)"
        :force-render="true"
      >
        <div class="flex h-full min-h-0 flex-col overflow-hidden">
      <div
        ref="trendTableWrapRef"
        class="min-h-0 flex-1 overflow-hidden"
      >
        <TaktSingleTable
          analysis-table
          class="h-full min-h-0"
          entity-scope="company"
          table-mode="single"
          :columns="trendColumns"
          :visible-column-keys="trendVisibleColumnKeys"
          :id-column-key="'period'"
          :data-source="monthRows"
          :loading="loading"
          :stripe="true"
          :row-key="getTrendRowKey"
          :pagination="false"
          :show-row-selection="false"
          :scroll="{ x: 'max-content', y: trendTableScrollY }"
        >
          <template #bodyCell="{ column, record, text, index }">
            <template v-if="column.key === 'amount'">
              <span class="tabular-nums">{{ formatShareWanYuan((record as HistoryMonthRow).amount) }}</span>
            </template>
            <template v-else-if="column.key === 'impact'">
              <template v-if="Number(index) > 0 && hasPriceImpact(record as HistoryMonthRow)">
                <span class="whitespace-nowrap tabular-nums">
                  <span class="text-red-500">{{ formatShareWanYuan((record as HistoryMonthRow).upAmount) }} {{ t(`${localePrefix}.trend.up`) }}</span>
                  <span class="text-text-secondary">, </span>
                  <span class="text-green-600">{{ formatShareWanYuan((record as HistoryMonthRow).downAmount) }} {{ t(`${localePrefix}.trend.down`) }}</span>
                </span>
              </template>
              <span
                v-else-if="Number(index) > 0"
                class="text-text-secondary"
              >—</span>
            </template>
            <template v-else>
              {{ text }}
            </template>
          </template>
          <template
            v-if="monthRows.length > 0"
            #summary
          >
            <a-table-summary fixed>
              <a-table-summary-row>
                <a-table-summary-cell :index="0">
                  <span class="font-medium text-text">{{ t(`${localePrefix}.total`) }}</span>
                </a-table-summary-cell>
                <a-table-summary-cell
                  :index="1"
                  align="right"
                >
                  <span class="font-medium tabular-nums">{{ formatShareWanYuan(totalAmount) }}</span>
                </a-table-summary-cell>
                <a-table-summary-cell
                  :index="2"
                  align="right"
                >
                  <span class="whitespace-nowrap font-medium tabular-nums">
                    <span class="text-red-500">{{ formatShareWanYuan(totalUpAmount) }} {{ t(`${localePrefix}.trend.up`) }}</span>
                    <span class="text-text-secondary">, </span>
                    <span class="text-green-600">{{ formatShareWanYuan(totalDownAmount) }} {{ t(`${localePrefix}.trend.down`) }}</span>
                  </span>
                </a-table-summary-cell>
              </a-table-summary-row>
            </a-table-summary>
          </template>
          <template #footerRemark>
            <div class="truncate text-sm text-text-secondary">
              {{ trendSummaryText }}
            </div>
          </template>
        </TaktSingleTable>
      </div>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="detail"
        :tab="t(`${localePrefix}.tabs.detail`)"
        :force-render="true"
      >
        <div class="flex h-full min-h-0 flex-col overflow-hidden">
      <div
        ref="detailTableWrapRef"
        class="min-h-0 flex-1 overflow-hidden"
      >
        <TaktSingleTable
          analysis-table
          class="h-full min-h-0"
          entity-scope="company"
          table-mode="single"
          :columns="detailColumns"
          :visible-column-keys="detailVisibleColumnKeys"
          :id-column-key="'rowKey'"
          :data-source="displayDetailRows"
          :loading="loading"
          :stripe="true"
          :row-key="getDetailRowKey"
          :pagination="false"
          :show-row-selection="false"
          :scroll="{ x: 'max-content', y: detailTableScrollY }"
        >
          <template #bodyCell="{ column, record, text }">
            <template v-if="column.key === 'currencyCode'">
              <template v-if="(record as PurchaseActualDetailDisplayRow).isFirst">
                <TaktDictTag
                  :value="(record as PurchaseActualDetailDisplayRow).currencyCode"
                  dict-type="accounting_financial_currency_code"
                />
              </template>
            </template>
            <template v-else-if="column.key === 'amount'">
              <span class="tabular-nums">
                {{ formatAmount((record as PurchaseActualDetailDisplayRow).amount) }}
              </span>
            </template>
            <template v-else-if="column.key === 'unitPrice'">
              <span class="tabular-nums">
                {{ formatUnitPrice((record as PurchaseActualDetailDisplayRow).unitPrice) }}
              </span>
            </template>
            <template v-else-if="column.key === 'quantity'">
              <span class="tabular-nums">
                {{ formatQuantity((record as PurchaseActualDetailDisplayRow).quantity) }}
              </span>
            </template>
            <template v-else-if="column.key === 'month'">
              {{ formatPointDate((record as PurchaseActualDetailDisplayRow).month) }}
            </template>
            <template v-else-if="column.key === 'adjacent'">
              <template v-if="(record as PurchaseActualDetailDisplayRow).adjacentBasePeriod">
                <a-tooltip
                  :title="t(`${localePrefix}.adjacentVs`, {
                    period: formatPointDate((record as PurchaseActualDetailDisplayRow).adjacentBasePeriod || undefined),
                  })"
                >
                  <span :class="trendClass((record as PurchaseActualDetailDisplayRow).adjacentTrend || 'none')">
                    {{ formatDisplayAdjacent(record as PurchaseActualDetailDisplayRow) }}
                  </span>
                </a-tooltip>
              </template>
            </template>
            <template v-else-if="column.key === 'trend'">
              <template v-if="(record as PurchaseActualDetailDisplayRow).isFirst">
                <span :class="trendClass((record as PurchaseActualDetailDisplayRow).trend || 'none')">
                  {{ trendLabel((record as PurchaseActualDetailDisplayRow).trend || 'none') }}
                </span>
              </template>
            </template>
            <template v-else-if="column.key === 'varianceAmount'">
              <template v-if="(record as PurchaseActualDetailDisplayRow).isFirst">
                <span :class="varianceClass((record as PurchaseActualDetailDisplayRow).varianceAmount)">
                  {{ formatAmount((record as PurchaseActualDetailDisplayRow).varianceAmount) }}
                </span>
              </template>
            </template>
            <template v-else>
              {{ text }}
            </template>
          </template>
          <template #footerRemark>
            <div class="truncate text-sm text-text-secondary">
              {{ detailSummaryText }}
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
      </a-tab-pane>
    </a-tabs>
    <TaktColumnDrawer
      entity-scope="company"
      v-model:open="detailColumnSettingVisible"
      :columns="detailColumns"
      :checked-keys="detailVisibleColumnKeys"
      :id-column-key="'rowKey'"
      :action-column-key="'__none__'"
      @update:checked-keys="handleDetailColumnKeysChange"
      @reset="handleDetailColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 采购实绩推移：柱形图 / 推移表 / 明细；涨跌=(本月单价−上月单价)×本月数量
 */
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import * as echarts from 'echarts/core'
import { BarChart, LineChart } from 'echarts/charts'
import {
  GridComponent,
  LegendComponent,
  TitleComponent,
  ToolboxComponent,
  TooltipComponent,
} from 'echarts/components'
import { CanvasRenderer } from 'echarts/renderers'
import type { ECharts } from 'echarts/core'
import {
  exportPurchaseActualTrendAnalysis,
  getPurchaseActualTrendAnalysis,
} from '@/api/logistics/procurement/purchaseActualTrend.api'
import type {
  PurchasePriceTrendMonthSummary,
  PurchasePriceTrend,
  PurchasePriceTrendPoint,
} from '@/types/logistics/procurement/purchasePriceTrend.types'
import { useTableRefresh } from '@/composables/useTableRefresh'
import { useTaktComponentLocale } from '@/composables/useTaktComponentLocale'
import { useThemeStore } from '@/stores/common/useThemeStore'
import { useThemeColorStore } from '@/stores/common/useThemeColorStore'
import {
  ensureTaktPaginationConfigAsync,
  getTaktDefaultPageIndex,
  getTaktDefaultPageSize,
} from '@/utils/taktPaged'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { taktTrendExportFileNames } from '@/utils/naming'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/tableScroll'
import { formatAnalysisVariancePercent, isAnalysisVariancePercentZero } from '@/utils/analysisPercent'

echarts.use([
  BarChart,
  LineChart,
  GridComponent,
  LegendComponent,
  TitleComponent,
  ToolboxComponent,
  TooltipComponent,
  CanvasRenderer,
])

/** 月汇总行（含展示用月份标题） */
type HistoryMonthRow = PurchasePriceTrendMonthSummary & {
  /** 4月 / 2026年4月 */
  monthTitle: string
}

/**
 * 明细展开行（组内首行填物料维；续行留空）
 */
interface PurchaseActualDetailDisplayRow {
  rowKey: string
  isFirst: boolean
  plantCode: string
  materialCode: string
  materialDescription: string
  supplierCode: string
  supplierName: string
  currencyCode: string
  unit: string
  amount?: number | null
  unitPrice?: number | null
  quantity?: number | null
  month?: string
  adjacentTrend?: string
  adjacentBasePeriod?: string | null
  adjacentText?: string
  trend?: string
  varianceAmount?: number | null
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
}>()

const loading = defineModel<boolean>('loading', { default: false })
const hasRows = defineModel<boolean>('hasRows', { default: false })

const localePrefix = 'logistics.procurement.purchase-actual-trend.page'
const { t, locale } = useI18n()
/** ECharts 语言键 */
const { echartsLocale } = useTaktComponentLocale()
/** 明暗主题 */
const themeStore = useThemeStore()
/** 主题色（主色变化时重绘柱色） */
const themeColorStore = useThemeColorStore()

/** 当前 Tab：柱形图 / 推移表 / 明细（默认柱形图） */
const activeTab = ref<'chart' | 'trend' | 'detail'>('chart')
/** 明细行 */
const detailRows = ref<PurchasePriceTrend[]>([])
/** 行总数（筛选后） */
const materialCount = ref(0)
/** 分页 */
const pageIndex = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
/** 各月合计行 */
const monthRows = ref<HistoryMonthRow[]>([])
/** 期间年月（跨年表头用） */
const periodOrder = ref<string[]>([])
/** 期间采购金额 / 涨价合计 / 跌价合计 */
const totalAmount = ref(0)
const totalUpAmount = ref(0)
const totalDownAmount = ref(0)
/** 柱形图外壳 */
const chartWrapRef = ref<HTMLElement | null>(null)
/** 柱形图容器 */
const chartElRef = ref<HTMLDivElement | null>(null)
/** 柱形图实例 */
const netImpactChartRef = { chart: null as ECharts | null }
/** 明细表体外壳 */
const detailTableWrapRef = ref<HTMLElement | null>(null)
/** 推移表体外壳 */
const trendTableWrapRef = ref<HTMLElement | null>(null)
/** 明细表体 scroll.y */
const detailTableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
/** 推移表体 scroll.y */
const trendTableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
/** ResizeObserver */
let tableScrollResizeObserver: ResizeObserver | null = null

/** 柱形图月份（首月为基准月，不展示） */
const chartMonthRows = computed(() =>
  monthRows.value.length > 1 ? monthRows.value.slice(1) : [],
)

/** 柱形图摘要 */
const chartSummaryText = computed(() => {
  if (!props.plantCode?.trim()) {
    return t(`${localePrefix}.selectPlantRequired`)
  }
  return t(`${localePrefix}.chartSummary`, { count: chartMonthRows.value.length })
})

/** 明细摘要 */
const detailSummaryText = computed(() => {
  if (!props.plantCode?.trim()) {
    return t(`${localePrefix}.selectPlantRequired`)
  }
  return t(`${localePrefix}.detailSummary`, { count: materialCount.value })
})

/** 推移表摘要 */
const trendSummaryText = computed(() => {
  if (!props.plantCode?.trim()) {
    return t(`${localePrefix}.selectPlantRequired`)
  }
  return t(`${localePrefix}.summary`, { count: monthRows.value.length })
})

/** 明细列（与采购价格推移一致） */
const detailColumns = computed<TableColumnsType>(() => [
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
    title: t(`${localePrefix}.columns.amount`),
    dataIndex: 'amount',
    key: 'amount',
    width: 120,
    align: 'right',
  },
  {
    title: t(`${localePrefix}.columns.unitPrice`),
    dataIndex: 'unitPrice',
    key: 'unitPrice',
    width: 120,
    align: 'right',
  },
  {
    title: t(`${localePrefix}.columns.quantity`),
    dataIndex: 'quantity',
    key: 'quantity',
    width: 100,
    align: 'right',
  },
  {
    title: t(`${localePrefix}.columns.month`),
    dataIndex: 'month',
    key: 'month',
    width: 100,
    align: 'left',
  },
  {
    title: t(`${localePrefix}.columns.adjacent`),
    dataIndex: 'adjacent',
    key: 'adjacent',
    width: 130,
    align: 'right',
  },
  {
    title: t(`${localePrefix}.columns.trend`),
    dataIndex: 'trend',
    key: 'trend',
    width: 90,
    align: 'left',
    fixed: 'right',
  },
  {
    title: t(`${localePrefix}.columns.varianceAmount`),
    dataIndex: 'varianceAmount',
    key: 'varianceAmount',
    width: 140,
    align: 'right',
    fixed: 'right',
  },
])

/** 明细默认可见列（含期间涨跌 / 期间单价影响金额，可由列设置隐藏） */
const DETAIL_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'materialCode',
  'materialDescription',
  'supplierCode',
  'supplierName',
  'currencyCode',
  'unit',
  'amount',
  'unitPrice',
  'quantity',
  'month',
  'adjacent',
  'trend',
  'varianceAmount',
] as const

/**
 * 将物料行按有业务的月份展开为多行展示
 */
const displayDetailRows = computed<PurchaseActualDetailDisplayRow[]>(() => {
  const result: PurchaseActualDetailDisplayRow[] = []
  for (const row of detailRows.value) {
    const points = (row.periodPoints ?? []).filter((item) => item.hasPurchase !== false)
    if (points.length === 0) {
      result.push(createDetailDisplayRow(row, null, undefined, 0, true))
      continue
    }
    for (let i = 0; i < points.length; i++) {
      result.push(createDetailDisplayRow(row, points[i], i > 0 ? points[i - 1] : undefined, i, i === 0))
    }
  }
  return result
})

/** 明细列设置抽屉 */
const detailColumnSettingVisible = ref(false)
/** 明细当前可见列 */
const detailVisibleColumnKeys = ref<string[]>([...DETAIL_DEFAULT_VISIBLE_COLUMN_KEYS])

/** 月份 / 采购份额（万元） / 涨跌金额（万元） */
const trendColumns = computed<TableColumnsType>(() => [
  {
    title: t(`${localePrefix}.monthColumn`),
    dataIndex: 'monthTitle',
    key: 'monthTitle',
    width: 120,
    align: 'left',
  },
  {
    title: t(`${localePrefix}.amountColumn`),
    dataIndex: 'amount',
    key: 'amount',
    width: 180,
    align: 'right',
  },
  {
    title: t(`${localePrefix}.impactColumn`),
    dataIndex: 'impact',
    key: 'impact',
    width: 280,
    align: 'right',
  },
])

const trendVisibleColumnKeys = computed(() => trendColumns.value.map((c) => String(c.key)))

/**
 * 明细行主键
 * @param {PurchaseActualDetailDisplayRow} record 行
 * @returns {string} key
 */
function getDetailRowKey(record: PurchaseActualDetailDisplayRow): string {
  return record.rowKey
}

/**
 * 构建明细展开展示行
 * @param {PurchasePriceTrend} row 物料行
 * @param {PurchasePriceTrendPoint | null} point 当月
 * @param {PurchasePriceTrendPoint | undefined} previous 上一有业务月
 * @param {number} index 序号
 * @param {boolean} isFirst 是否组内首行
 * @returns {PurchaseActualDetailDisplayRow} 展示行
 */
function createDetailDisplayRow(
  row: PurchasePriceTrend,
  point: PurchasePriceTrendPoint | null,
  previous: PurchasePriceTrendPoint | undefined,
  index: number,
  isFirst: boolean,
): PurchaseActualDetailDisplayRow {
  const partner = row.supplierCode ?? ''
  const partnerName = row.supplierName ?? ''
  const baseKey = `${row.plantCode}|${row.materialCode}|${partner}`
  const adjacentTrend = point && previous
    ? unitPriceAdjacentTrend(point, previous)
    : 'none'
  const adjacentText = point && previous
    ? formatUnitPriceAdjacentPercent(point, previous)
    : ''
  return {
    rowKey: `${baseKey}|${point?.validFrom ?? 'none'}|${index}`,
    isFirst,
    plantCode: isFirst ? row.plantCode : '',
    materialCode: isFirst ? row.materialCode : '',
    materialDescription: isFirst ? (row.materialDescription ?? '') : '',
    supplierCode: isFirst ? partner : '',
    supplierName: isFirst ? partnerName : '',
    currencyCode: isFirst ? (row.currencyCode ?? '') : '',
    unit: isFirst ? (row.unit ?? '') : '',
    amount: point?.amount ?? null,
    unitPrice: point?.unitPrice ?? null,
    quantity: point?.quantity ?? null,
    month: point?.validFrom,
    adjacentTrend,
    adjacentBasePeriod: point && previous
      ? (point.adjacentBasePeriod ?? previous.validFrom ?? null)
      : null,
    adjacentText,
    trend: isFirst ? row.trend : undefined,
    varianceAmount: isFirst ? row.varianceAmount : null,
  }
}

/**
 * 展示行涨跌文案
 * @param {PurchaseActualDetailDisplayRow} record 展示行
 * @returns {string} 文本
 */
function formatDisplayAdjacent(record: PurchaseActualDetailDisplayRow): string {
  return record.adjacentText || ''
}

/**
 * 推移表行主键
 * @param {HistoryMonthRow} record 月汇总
 * @returns {string} key
 */
function getTrendRowKey(record: HistoryMonthRow): string {
  return record.period
}

/**
 * 有效起始日 / 年月展示为 yyyy/M/d 或 yyyy/M
 * @param {string | undefined} value yyyy-MM-dd 或 yyyy-MM
 * @returns {string} 文本
 */
function formatPointDate(value?: string): string {
  if (!value) return '—'
  const parts = value.split('-')
  if (parts.length === 2) {
    return `${parts[0]}/${Number(parts[1])}`
  }
  if (parts.length !== 3) return value
  return `${parts[0]}/${Number(parts[1])}/${Number(parts[2])}`
}

/**
 * 推移表某月是否有因单价造成的涨跌金额
 * @param {HistoryMonthRow} row 月汇总
 * @returns {boolean} 有涨或跌
 */
function hasPriceImpact(row: HistoryMonthRow): boolean {
  return (row.upAmount ?? 0) !== 0 || (row.downAmount ?? 0) !== 0
}

/**
 * 相邻月单价涨跌%：(本月单价−上月单价)/上月单价
 * @param {PurchasePriceTrendPoint} current 本月
 * @param {PurchasePriceTrendPoint | undefined} previous 上一有采购月
 * @returns {string} 文本（0.00% 留空）
 */
function formatUnitPriceAdjacentPercent(current: PurchasePriceTrendPoint, previous?: PurchasePriceTrendPoint): string {
  if (!previous || !current.adjacentBasePeriod) return ''
  const percent = resolveUnitPriceAdjacentPercent(current, previous)
  if (percent == null || isAnalysisVariancePercentZero(percent, 'ratio')) return ''
  const arrow = percent > 0 ? '↑' : percent < 0 ? '↓' : '→'
  const text = formatAnalysisVariancePercent(percent, 'ratio', '')
  return text ? `${arrow} ${text}` : ''
}

/**
 * 相邻月单价涨跌码（后端 adjacentTrend；缺省时按单价变动率推导）
 * @param {PurchasePriceTrendPoint} current 本月
 * @param {PurchasePriceTrendPoint | undefined} previous 上一有采购月
 * @returns {string} up / down / flat / none
 */
function unitPriceAdjacentTrend(current: PurchasePriceTrendPoint, previous?: PurchasePriceTrendPoint): string {
  if (!previous || !current.adjacentBasePeriod) return 'none'
  if (current.adjacentTrend === 'up' || current.adjacentTrend === 'down' || current.adjacentTrend === 'flat') {
    return current.adjacentTrend
  }
  const percent = resolveUnitPriceAdjacentPercent(current, previous)
  if (percent == null || percent === 0) return 'flat'
  return percent > 0 ? 'up' : 'down'
}

/**
 * 相邻月单价变动率：优先后端 adjacentVariancePercent，否则 (本月单价−上月单价)/上月单价
 * @param {PurchasePriceTrendPoint} current 本月
 * @param {PurchasePriceTrendPoint} previous 上一有采购月
 * @returns {number | null} 小数比率
 */
function resolveUnitPriceAdjacentPercent(
  current: PurchasePriceTrendPoint,
  previous: PurchasePriceTrendPoint,
): number | null {
  if (current.adjacentVariancePercent != null && !Number.isNaN(current.adjacentVariancePercent)) {
    return current.adjacentVariancePercent
  }
  const prevPrice = previous.unitPrice ?? 0
  if (prevPrice === 0) return null
  return ((current.unitPrice ?? 0) - prevPrice) / prevPrice
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
 * 期间涨跌文案
 * @param {string} trend 涨跌码
 * @returns {string} 文案
 */
function trendLabel(trend: string): string {
  if (trend === 'up') return t(`${localePrefix}.trend.up`)
  if (trend === 'down') return t(`${localePrefix}.trend.down`)
  if (trend === 'flat') return t(`${localePrefix}.trend.flat`)
  return t(`${localePrefix}.trend.none`)
}

/**
 * 期间单价影响金额样式
 * @param {number | null | undefined} value 影响金额
 * @returns {string} class
 */
function varianceClass(value?: number | null): string {
  if (value == null || Number.isNaN(value) || value === 0) return 'text-text-secondary'
  return value > 0 ? 'text-red-500' : 'text-green-600'
}

/**
 * 单价（5 位小数）
 * @param {number | null | undefined} value 单价
 * @returns {string} 文本
 */
function formatUnitPrice(value?: number | null): string {
  if (value == null || Number.isNaN(value)) return '—'
  return value.toLocaleString('zh-CN', { minimumFractionDigits: 5, maximumFractionDigits: 5 })
}

/**
 * 数量（最多 5 位小数）
 * @param {number | null | undefined} value 数量
 * @returns {string} 文本
 */
function formatQuantity(value?: number | null): string {
  if (value == null || Number.isNaN(value)) return '—'
  return String(Number(value.toFixed(5)))
}

/**
 * 金额（千分位、3 位小数；明细 Tab 用元）
 * @param {number | null | undefined} value 数值
 * @returns {string} 文本
 */
function formatAmount(value?: number | null): string {
  if (value == null || Number.isNaN(value)) return '—'
  return value.toLocaleString('zh-CN', { minimumFractionDigits: 3, maximumFractionDigits: 3 })
}

/** 万元换算（1 万元 = 10,000 元） */
const WAN_YUAN_SCALE = 10000

/**
 * 采购份额 / 涨跌（万元、5 位小数；仅推移表 Tab）
 * @param {number | null | undefined} value 元金额
 * @returns {string} 万元文本
 */
function formatShareWanYuan(value?: number | null): string {
  if (value == null || Number.isNaN(value)) return '—'
  return (value / WAN_YUAN_SCALE).toLocaleString('zh-CN', {
    minimumFractionDigits: 5,
    maximumFractionDigits: 5,
  })
}

/**
 * 年月 yyyy-MM → 4月（跨年则 2026年4月）
 * @param {string | undefined} yearMonth 年月
 * @returns {string} 文本
 */
function formatMonthTitle(yearMonth?: string): string {
  if (!yearMonth) return '—'
  const parts = yearMonth.split('-')
  if (parts.length < 2) return yearMonth
  const month = Number(parts[1])
  const years = new Set(periodOrder.value.map((key) => key.slice(0, 4)))
  if (years.size > 1) {
    return t(`${localePrefix}.monthYear`, { year: parts[0], month })
  }
  return t(`${localePrefix}.month`, { month })
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
  if (!plant || !props.periodRange?.[0]) {
    return null
  }
  return {
    plantCode: plant,
    supplierCode: props.supplierCode?.trim() || undefined,
    materialCode: props.materialCode?.trim() || undefined,
    onlyEnabled: true,
    trendFilter: props.trendFilter || undefined,
    pageIndex: pageIndex.value,
    pageSize: pageSize.value,
    ...periodRangeToQuery(props.periodRange),
  }
}

/** 清空 */
function clear() {
  detailRows.value = []
  materialCount.value = 0
  total.value = 0
  pageIndex.value = getTaktDefaultPageIndex()
  monthRows.value = []
  periodOrder.value = []
  totalAmount.value = 0
  totalUpAmount.value = 0
  totalDownAmount.value = 0
  hasRows.value = false
  void nextTick(() => renderNetImpactChart())
}

/**
 * 元 → 万元（柱形图用，2 位小数）
 * @param {number} yuan 元
 * @returns {number} 万元
 */
function toWanYuan(yuan: number): number {
  return Math.round((yuan / WAN_YUAN_SCALE) * 100) / 100
}

/**
 * 读取 Ant Design CSS 变量（ECharts 需具体色值）
 * @param {string} varName 如 --ant-color-bg-container
 * @param {string} fallback 回退色
 * @returns {string} 计算后色值
 */
function readAntCssColor(varName: string, fallback: string): string {
  if (typeof document === 'undefined') {
    return fallback
  }
  const raw = getComputedStyle(document.documentElement).getPropertyValue(varName).trim()
  return raw || fallback
}

/**
 * 柱形图主题色（随 light/dark 与 Ant token 变化）
 * @returns 画布/网格/轴线/系列色
 */
function resolveChartThemeColors() {
  const isDark = themeStore.resolvedTheme === 'dark'
  return {
    backgroundColor: readAntCssColor('--ant-color-bg-container', isDark ? '#141414' : '#ffffff'),
    gridBackgroundColor: readAntCssColor(
      '--ant-color-fill-quaternary',
      isDark ? 'rgba(255,255,255,0.04)' : '#f1f5f9',
    ),
    borderColor: readAntCssColor('--ant-color-border', isDark ? '#434343' : '#cbd5e1'),
    splitLineColor: readAntCssColor(
      '--ant-color-border-secondary',
      isDark ? '#303030' : '#e2e8f0',
    ),
    axisLineColor: readAntCssColor('--ant-color-border', isDark ? '#434343' : '#94a3b8'),
    textColor: readAntCssColor(
      '--ant-color-text',
      isDark ? 'rgba(255,255,255,0.85)' : 'rgba(0,0,0,0.88)',
    ),
    textSecondary: readAntCssColor(
      '--ant-color-text-secondary',
      isDark ? 'rgba(255,255,255,0.45)' : '#999999',
    ),
    upNetColor: readAntCssColor('--ant-color-error', '#ef4444'),
    downNetColor: readAntCssColor('--ant-color-info', isDark ? '#1668dc' : '#5470c6'),
  }
}

/**
 * 柱形图万元数值（固定 2 位小数）
 * @param {number | string} value 万CNY
 * @returns {string} 展示文案
 */
function formatChartWanYuan(value: number | string): string {
  const num = typeof value === 'number' ? value : Number(value)
  if (Number.isNaN(num)) return '—'
  return num.toLocaleString('zh-CN', {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  })
}

/**
 * 柱标签：涨在正轴、跌在负轴（展示绝对值）
 * @param {number} value 系列原始量（涨为正、跌为正绝对值）
 * @param {'up' | 'down'} forSeries 当前系列
 * @returns {string} 如「涨 1.23」/「跌 0.45」
 */
function formatChartSignedLabel(value: number, forSeries: 'up' | 'down'): string {
  if (value <= 0) return ''
  if (forSeries === 'up') {
    return `${t(`${localePrefix}.chart.seriesUp`)} ${formatChartWanYuan(value)}`
  }
  return `${t(`${localePrefix}.chart.seriesDown`)} ${formatChartWanYuan(value)}`
}

/**
 * 柱形图子标题：期间=选中期间；基准月=期间最小月
 * @returns {string} 子标题文案
 */
function formatChartPeriodSubtext(): string {
  const range = props.periodRange
  if (!range?.[0]) {
    return ''
  }
  const start = range[0]
  const end = range[1] || range[0]
  const startParts = start.split('-')
  const endParts = end.split('-')
  if (startParts.length < 2 || endParts.length < 2) {
    return t(`${localePrefix}.chart.subtext`, {
      period: `${start}~${end}`,
      baseMonth: start,
    })
  }
  const startYear = startParts[0]
  const endYear = endParts[0]
  const startMonth = Number(startParts[1])
  const endMonth = Number(endParts[1])
  const periodText =
    startYear === endYear
      ? `${startMonth}-${endMonth}`
      : `${startYear}/${startMonth}-${endYear}/${endMonth}`
  // 基准月 = 期间最小月（区间起点）
  const baseMonthText =
    startYear === endYear
      ? t(`${localePrefix}.month`, { month: startMonth })
      : t(`${localePrefix}.monthYear`, { year: startYear, month: startMonth })
  return t(`${localePrefix}.chart.subtext`, {
    period: periodText,
    baseMonth: baseMonthText,
  })
}

/**
 * 渲染月度涨跌柱形图（正轴=涨合计、负轴=跌合计；涨跌互不冲抵）
 */
function renderNetImpactChart(): void {
  const el = chartElRef.value
  if (!el) {
    return
  }
  if (!netImpactChartRef.chart) {
    netImpactChartRef.chart = echarts.init(el, undefined, { locale: echartsLocale.value })
  }
  const chartRows = chartMonthRows.value
  const categories = chartRows.map((row) => row.monthTitle || formatMonthTitle(row.period))
  const upValues = chartRows.map((row) => toWanYuan(row.upAmount ?? 0))
  const downValues = chartRows.map((row) => toWanYuan(row.downAmount ?? 0))
  /** 跌在负轴 */
  const downSignedValues = downValues.map((down) => (down > 0 ? -down : 0))
  const upName = t(`${localePrefix}.chart.seriesUp`)
  const downName = t(`${localePrefix}.chart.seriesDown`)
  const unitLabel = t(`${localePrefix}.chart.unitWanYuan`)
  const periodSubtext = formatChartPeriodSubtext()
  const themeColors = resolveChartThemeColors()
  const labelStyle = {
    show: true,
    fontSize: 10,
    fontFamily: '微软雅黑',
    fontWeight: 'bold' as const,
    color: themeColors.textColor,
  }

  netImpactChartRef.chart.setOption(
    {
      backgroundColor: themeColors.backgroundColor,
      title: {
        text: t(`${localePrefix}.chart.title`),
        subtext: periodSubtext,
        left: 'center',
        textStyle: { fontSize: 16, fontWeight: 600, color: themeColors.textColor },
        subtextStyle: { fontSize: 12, color: themeColors.textSecondary },
      },
      tooltip: {
        trigger: 'axis',
        axisPointer: { type: 'shadow' },
        backgroundColor: themeColors.backgroundColor,
        borderColor: themeColors.borderColor,
        textStyle: { color: themeColors.textColor },
        formatter: (items: unknown) => {
          const list = Array.isArray(items) ? items : []
          if (list.length === 0) return ''
          const first = list[0] as { axisValueLabel?: string; axisValue?: string; dataIndex?: number }
          const idx = typeof first.dataIndex === 'number' ? first.dataIndex : 0
          const up = upValues[idx] ?? 0
          const down = downValues[idx] ?? 0
          const net = Math.round((up - down) * 100) / 100
          const netText =
            net > 0
              ? `${upName} ${formatChartWanYuan(net)}`
              : net < 0
                ? `${downName} ${formatChartWanYuan(-net)}`
                : `${t(`${localePrefix}.trend.flat`)} ${formatChartWanYuan(0)}`
          const title = first.axisValueLabel || first.axisValue || ''
          return [
            title,
            `${upName}: ${formatChartWanYuan(up)} ${unitLabel}`,
            `${downName}: ${formatChartWanYuan(down)} ${unitLabel}`,
            `${t(`${localePrefix}.chart.net`)}: ${netText} ${unitLabel}`,
          ].join('<br/>')
        },
      },
      legend: {
        data: [upName, downName],
        left: 'left',
        top: 8,
        textStyle: { color: themeColors.textColor },
      },
      toolbox: {
        show: true,
        iconStyle: { borderColor: themeColors.textSecondary },
        feature: {
          dataView: { show: true, readOnly: false },
          magicType: { show: true, type: ['line', 'bar'] },
          restore: { show: true },
          saveAsImage: { show: true, name: t(`${localePrefix}.chart.title`) },
        },
      },
      calculable: true,
      grid: {
        left: 48,
        right: 36,
        top: 72,
        bottom: 40,
        containLabel: true,
        show: true,
        backgroundColor: themeColors.gridBackgroundColor,
        borderColor: themeColors.borderColor,
        borderWidth: 1,
      },
      xAxis: [
        {
          type: 'category',
          data: categories,
          axisTick: { alignWithLabel: true },
          axisLine: { show: true, onZero: true, lineStyle: { color: themeColors.axisLineColor } },
          axisLabel: { color: themeColors.textSecondary },
          splitLine: {
            show: true,
            lineStyle: { color: themeColors.splitLineColor, type: 'solid' },
          },
        },
      ],
      yAxis: [
        {
          type: 'value',
          name: unitLabel,
          nameTextStyle: { color: themeColors.textSecondary },
          axisLine: { show: true, lineStyle: { color: themeColors.axisLineColor } },
          axisLabel: { color: themeColors.textSecondary },
          splitLine: {
            show: true,
            lineStyle: { color: themeColors.splitLineColor, type: 'solid' },
          },
        },
      ],
      series: [
        {
          name: upName,
          type: 'bar',
          data: upValues,
          barMaxWidth: 28,
          barGap: '20%',
          itemStyle: {
            color: themeColors.upNetColor,
            borderRadius: [8, 8, 0, 0],
          },
          emphasis: { focus: 'series' },
          label: {
            ...labelStyle,
            position: 'top',
            formatter: (params: { dataIndex?: number }) =>
              formatChartSignedLabel(upValues[params.dataIndex ?? 0] ?? 0, 'up'),
          },
        },
        {
          name: downName,
          type: 'bar',
          data: downSignedValues,
          barMaxWidth: 28,
          itemStyle: {
            color: themeColors.downNetColor,
            borderRadius: [0, 0, 8, 8],
          },
          emphasis: { focus: 'series' },
          label: {
            ...labelStyle,
            position: 'bottom',
            formatter: (params: { dataIndex?: number }) =>
              formatChartSignedLabel(downValues[params.dataIndex ?? 0] ?? 0, 'down'),
          },
        },
      ],
    },
    { notMerge: true },
  )
  netImpactChartRef.chart.resize()
}

/** 销毁柱形图 */
function disposeNetImpactChart(): void {
  netImpactChartRef.chart?.dispose()
  netImpactChartRef.chart = null
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
    const result = await getPurchaseActualTrendAnalysis(query)
    detailRows.value = result.paged?.data ?? []
    total.value = result.paged?.total ?? 0
    materialCount.value = result.materialCount ?? 0
    periodOrder.value = result.periodOrder ?? []
    monthRows.value = (result.monthSummaries ?? []).map((item) => ({
      ...item,
      monthTitle: formatMonthTitle(item.period),
    }))
    totalAmount.value = result.totalAmount ?? 0
    totalUpAmount.value = result.totalUpAmount ?? 0
    totalDownAmount.value = result.totalDownAmount ?? 0
    hasRows.value = materialCount.value > 0
  } catch (error: unknown) {
    clear()
    const err = error as { message?: string }
    message.error(err?.message || t(`${localePrefix}.exportFailed`))
  } finally {
    loading.value = false
    await nextTick()
    recalcTableScrollY()
    renderNetImpactChart()
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

/** 清单导出（明细按当前可见列；另含推移表 Sheet） */
async function handleExport() {
  const query = buildQuery()
  if (!query) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return
  }
  if (!hasRows.value) {
    message.warning(t(`${localePrefix}.exportEmpty`))
    return
  }
  try {
    const { sheetName, exportName, fallbackBase } = taktTrendExportFileNames(
      query.plantCode,
      t(`${localePrefix}.title`),
    )
    const exportMeta = await exportPurchaseActualTrendAnalysis(
      {
        ...query,
        pageIndex: 1,
        pageSize: 1,
        detailColumnKeys: resolveExportDetailColumnKeys().join(','),
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

/**
 * 明细导出列：与表格当前可见列一致（去掉占位键）
 * @returns {string[]} 列键
 */
function resolveExportDetailColumnKeys(): string[] {
  const allowed = new Set(DETAIL_DEFAULT_VISIBLE_COLUMN_KEYS.map(String))
  const expanded: string[] = []
  for (const raw of detailVisibleColumnKeys.value) {
    const k = String(raw)
    if (k === 'points') {
      expanded.push('amount', 'unitPrice', 'quantity', 'month', 'adjacent')
      continue
    }
    if (allowed.has(k)) {
      expanded.push(k)
    }
  }
  const keys = [...new Set(expanded)]
  return keys.length > 0 ? keys : [...DETAIL_DEFAULT_VISIBLE_COLUMN_KEYS]
}

/** 打开明细列设置 */
function openDetailColumnSetting() {
  detailColumnSettingVisible.value = true
}

/**
 * 明细列勾选变更
 * @param {Array<string | number>} keys 选中列
 */
function handleDetailColumnKeysChange(keys: Array<string | number>) {
  const allowed = new Set(DETAIL_DEFAULT_VISIBLE_COLUMN_KEYS.map(String))
  detailVisibleColumnKeys.value = keys.map((k) => String(k)).filter((k) => allowed.has(k))
}

/** 明细列设置重置 */
function handleDetailColumnSettingReset() {
  detailVisibleColumnKeys.value = [...DETAIL_DEFAULT_VISIBLE_COLUMN_KEYS]
}

/** 测算表体高度 */
function recalcTableScrollY(): void {
  const headerApprox = 55
  const detailWrap = detailTableWrapRef.value
  if (detailWrap && detailWrap.clientHeight > 0) {
    detailTableScrollY.value = Math.max(TAKT_TABLE_SCROLL_Y_MIN, detailWrap.clientHeight - headerApprox)
  }
  const trendWrap = trendTableWrapRef.value
  if (trendWrap && trendWrap.clientHeight > 0) {
    trendTableScrollY.value = Math.max(TAKT_TABLE_SCROLL_Y_MIN, trendWrap.clientHeight - headerApprox)
  }
  if (activeTab.value === 'chart') {
    netImpactChartRef.chart?.resize()
  }
}

function startTableScrollObserve(): void {
  stopTableScrollObserve()
  tableScrollResizeObserver = new ResizeObserver(() => {
    recalcTableScrollY()
    if (activeTab.value === 'chart') {
      netImpactChartRef.chart?.resize()
    }
  })
  const chartWrap = chartWrapRef.value
  if (chartWrap) {
    tableScrollResizeObserver.observe(chartWrap)
  }
  const detailWrap = detailTableWrapRef.value
  if (detailWrap) {
    tableScrollResizeObserver.observe(detailWrap)
  }
  const trendWrap = trendTableWrapRef.value
  if (trendWrap) {
    tableScrollResizeObserver.observe(trendWrap)
  }
  recalcTableScrollY()
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

watch(activeTab, async (tab) => {
  await nextTick()
  if (tab === 'chart') {
    renderNetImpactChart()
  } else {
    recalcTableScrollY()
  }
})

watch([locale, echartsLocale], async () => {
  disposeNetImpactChart()
  await nextTick()
  renderNetImpactChart()
})

/** 明暗主题 / 主题色变化时重绘柱形图 */
watch(
  [() => themeStore.resolvedTheme, () => themeColorStore.colorPrimary],
  async () => {
    await nextTick()
    renderNetImpactChart()
  },
)

useTableRefresh(reload)

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  pageIndex.value = getTaktDefaultPageIndex()
  pageSize.value = getTaktDefaultPageSize()
  await nextTick()
  startTableScrollObserve()
  renderNetImpactChart()
})

onBeforeUnmount(() => {
  stopTableScrollObserve()
  disposeNetImpactChart()
})

defineExpose({ reload, loadData, handleExport, clear, openDetailColumnSetting })
</script>

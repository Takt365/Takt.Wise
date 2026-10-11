<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/material-cost-trend/components -->
<!-- 文件名称：MaterialCostItemComponentPricePanel.vue -->
<!-- 功能描述：选中产品后的转置月材料成本涨跌明细（期间列） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
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
        scroll-layout="masterDetailLr"
        table-mode="masterDetailDetail"
        :columns="columns"
        :visible-column-keys="visibleColumnKeys"
        :id-column-key="'productCode'"
        :data-source="detailRows"
        :loading="false"
        :stripe="true"
        :row-key="getRowKey"
        :pagination="false"
        :scroll="{ x: 'max-content', y: tableScrollY }"
        :footer-remark="summaryText"
      >
        <template #bodyCell="{ column, record, text }">
          <template v-if="String(column.key).startsWith('period_')">
            {{ formatPeriodCost(record as BomMaterialCostItemComponentMovingPrice, String(column.key)) }}
          </template>
          <template v-else-if="column.key === 'trend'">
            <span :class="trendClass((record as BomMaterialCostItemComponentMovingPrice).trend || 'none')">
              {{ trendLabel((record as BomMaterialCostItemComponentMovingPrice).trend || 'none') }}
            </span>
          </template>
          <template v-else-if="column.key === 'varianceAmount'">
            <span :class="varianceClass((record as BomMaterialCostItemComponentMovingPrice).varianceAmount)">
              {{ formatCost((record as BomMaterialCostItemComponentMovingPrice).varianceAmount) }}
            </span>
          </template>
          <template v-else-if="column.key === 'variancePercent'">
            <span :class="varianceClass((record as BomMaterialCostItemComponentMovingPrice).varianceAmount)">
              {{ formatPercent((record as BomMaterialCostItemComponentMovingPrice).variancePercent) }}
            </span>
          </template>
          <template v-else>
            {{ text }}
          </template>
        </template>
      </TaktSingleTable>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 产品转置月涨跌明细：展示选中产品的各月材料成本与环比
 */
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { measureMasterDetailLrTableScrollY } from '@/composables/useTaktMasterDetailLrScrollY'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/tableScroll'
import type { BomMaterialCostItemComponentMovingPrice } from '@/types/logistics/manufacturing/bom/materialCostTrend.types'
import {
  MATERIAL_COST_ANALYSIS_LOCALE_PREFIX,
  useMaterialCostAnalysis,
} from '../composables/useMaterialCostItemAnalysis'
import { useBomMaterialCostAnalysisMasterContext } from '../composables/useMaterialCostAnalysisMasterContext'

const props = defineProps<{
  /** 期间列顺序（与左表同源） */
  periodOrder?: string[]
}>()

const { t } = useI18n()
const localePrefix = MATERIAL_COST_ANALYSIS_LOCALE_PREFIX
const { formatCost, formatPercent, trendLabel, trendClass, varianceClass } = useMaterialCostAnalysis()
const { selectedMasterRow } = useBomMaterialCostAnalysisMasterContext()

/** 表体外壳 */
const tableWrapRef = ref<HTMLElement | null>(null)
/** 表体 scroll.y */
const tableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
/** ResizeObserver */
let tableScrollResizeObserver: ResizeObserver | null = null

/** 是否已选产品 */
const hasProduct = computed(() => {
  const row = selectedMasterRow.value as Record<string, unknown> | null
  return !!row?.productCode
})

/** 明细行（单产品） */
const detailRows = computed<BomMaterialCostItemComponentMovingPrice[]>(() => {
  if (!hasProduct.value || !selectedMasterRow.value) return []
  return [selectedMasterRow.value as BomMaterialCostItemComponentMovingPrice]
})

/** 期间列 */
const periodOrder = computed(() => {
  if (props.periodOrder?.length) return props.periodOrder
  const row = selectedMasterRow.value as BomMaterialCostItemComponentMovingPrice | null
  const costs = row?.periodMaterialCosts ?? row?.periodUnitPrices ?? {}
  return Object.keys(costs).sort()
})

/** 摘要 */
const summaryText = computed(() => {
  if (!hasProduct.value) {
    return t(`${localePrefix}.selectProductFirst`)
  }
  const row = selectedMasterRow.value as BomMaterialCostItemComponentMovingPrice
  return t(`${localePrefix}.componentPrice.detailSummary`, {
    product: row.productCode,
    description: row.productDescription || '—',
    base: row.basePeriod || '—',
    compare: row.comparePeriod || '—',
  })
})

/** 动态列 */
const columns = computed<TableColumnsType>(() => {
  const cols: TableColumnsType = [
    {
      title: t('entity.bommaterialcostitem.productcode'),
      dataIndex: 'productCode',
      key: 'productCode',
      width: 130,
      ellipsis: true,
      fixed: 'left',
    },
    {
      title: t('entity.bommaterialcost.productdescription'),
      dataIndex: 'productDescription',
      key: 'productDescription',
      width: 160,
      ellipsis: true,
    }]
  for (const period of periodOrder.value) {
    cols.push({
      title: period,
      dataIndex: ['periodMaterialCosts', period],
      key: `period_${period}`,
      width: 120,
      align: 'right',
    })
  }
  cols.push({
    title: t(`${localePrefix}.columns.trend`),
    dataIndex: 'trend',
    key: 'trend',
    width: 80,
    fixed: 'right',
  })
  cols.push({
    title: t(`${localePrefix}.columns.varianceAmount`),
    dataIndex: 'varianceAmount',
    key: 'varianceAmount',
    width: 110,
    align: 'right',
    fixed: 'right',
  })
  cols.push({
    title: t(`${localePrefix}.columns.variancePercent`),
    dataIndex: 'variancePercent',
    key: 'variancePercent',
    width: 90,
    align: 'right',
    fixed: 'right',
  })
  return cols
})

/** 可见列键 */
const visibleColumnKeys = computed(() => columns.value.map((c) => String(c.key)))

/**
 * 行主键
 * @param {BomMaterialCostItemComponentMovingPrice} record 行
 * @returns {string} key
 */
function getRowKey(record: BomMaterialCostItemComponentMovingPrice): string {
  return `${record.plantCode}|${record.modelCode}|${record.productCode}`
}

/**
 * 期间键
 * @param {string} columnKey period_yyyy-MM
 * @returns {string} yyyy-MM
 */
function resolvePeriodKey(columnKey: string): string {
  return columnKey.replace(/^period_/, '')
}

/**
 * 格式化月材料成本
 * @param {BomMaterialCostItemComponentMovingPrice} record 行
 * @param {string} columnKey 列键
 * @returns {string} 文本
 */
function formatPeriodCost(record: BomMaterialCostItemComponentMovingPrice, columnKey: string): string {
  const period = resolvePeriodKey(columnKey)
  const value = record.periodMaterialCosts?.[period] ?? record.periodUnitPrices?.[period]
  if (value == null || Number.isNaN(value)) return '—'
  return value.toFixed(5)
}

/** 实测 scroll.y */
function recalcTableScrollY(): void {
  const wrap = tableWrapRef.value
  if (!wrap || wrap.clientHeight <= 0) return
  tableScrollY.value = measureMasterDetailLrTableScrollY(wrap)
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

/** 选中变更后重算高度 */
async function reload() {
  await nextTick()
  recalcTableScrollY()
}

watch(
  () => selectedMasterRow.value,
  () => {
    void reload()
  },
)

onMounted(async () => {
  await nextTick()
  startTableScrollObserve()
})

onBeforeUnmount(() => {
  stopTableScrollObserve()
})

defineExpose({ reload, recalcTableScrollY })
</script>

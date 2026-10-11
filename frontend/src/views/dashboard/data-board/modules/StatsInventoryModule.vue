<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/dashboard/dataBoard.types/modules -->
<!-- 文件名称：StatsInventoryModule.vue -->
<!-- 功能描述：数据看板在库金额（前三月；卡面总金额，悬停 F/H/R 明细） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="min-h-[80px]">
    <p class="mb-3 text-base font-medium leading-relaxed text-text">
      {{ t('dashboard.data-board.page.inventory.summaryTitle') }}
    </p>
    <StatsMetricGrid
      card-variant="dashboard-compact"
      :loading="loading"
      :items="metricItems"
      :col-xs="12"
      :col-sm="12"
      :col-md="8"
      :col-lg="8"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 在库金额：TaktMaterialMovingPrices/stock-stat
 * 卡面显示总金额；悬停明细 F/H/R（Z792→F、Z790→H、Z300→R）
 */
import { useI18n } from 'vue-i18n'
import StatsMetricGrid from '../components/StatsMetricGrid.vue'
import { usePermissionStore } from '@/stores/identity/usePermissionStore'
import type { MaterialMovingPriceStat } from '@/types/logistics/materials/materialMovingPriceStat.types'
import { enrichDashboardMetricItems } from '../utils/statsMetricDashboard'
import {
  formatDashboardAmountLabel,
  type DashboardAmountUnitKey,
} from '../utils/formatDashboardAmount'
import {
  DASHBOARD_STATS_API,
  DASHBOARD_STATS_PERMISSION,
  DASHBOARD_STATS_ROUTE,
  fetchDashboardGet,
  fetchDashboardMetricIfPermitted,
  scheduleDashboardLoad,
} from '../utils/statsQuery'
import { getLastThreeCalendarMonthRanges } from '../utils/statsDashboardPlant'

/** 评估类别 → 卡面短码（Z792=F 成品，Z790=H 半成品，Z300=R 原材料） */
const VALUATION_DISPLAY_CODES: ReadonlyArray<{ valuation: string; label: string }> = [
  { valuation: 'Z792', label: 'F' },
  { valuation: 'Z790', label: 'H' },
  { valuation: 'Z300', label: 'R' },
]

/** 空在库金额统计 */
const EMPTY_STAT: MaterialMovingPriceStat = {
  statMonth: '',
  monthStockAmount: 0,
  monthRowCount: 0,
  byValuation: [],
}

const { t } = useI18n()
const permissionStore = usePermissionStore()

/** 列表 loading */
const loading = ref(false)
/** 前三月各月统计（key = yyyy-MM） */
const monthStats = ref<Record<string, MaterialMovingPriceStat>>({})

/**
 * 金额单位文案
 * @param unitKey 单位键
 * @returns {string} 单位
 */
function resolveAmountUnit(unitKey: DashboardAmountUnitKey): string {
  return t(`dashboard.data-board.page.amountunit.${unitKey}`)
}

/**
 * 格式化金额标签（1000万）
 * @param amount 元
 * @returns {string} 带单位金额
 */
function formatAmount(amount: number): string {
  return formatDashboardAmountLabel(amount, resolveAmountUnit)
}

/**
 * 按评估类别取金额
 * @param stat 月统计
 * @param valuation 评估类别码
 * @returns {number} 元
 */
function amountByValuation(stat: MaterialMovingPriceStat, valuation: string): number {
  const row = (stat.byValuation ?? []).find((item) => item.valuation === valuation)
  return Number(row?.stockAmount) || 0
}

/**
 * 悬停明细：F=300万，H=100万，R=600万（Z792→F、Z790→H、Z300→R）
 * @param stat 月统计
 * @returns {string} F/H/R 分项
 */
function buildValuationTooltip(stat: MaterialMovingPriceStat): string {
  const parts = VALUATION_DISPLAY_CODES.map(({ valuation, label }) =>
    t('dashboard.data-board.page.inventory.valuationPart', {
      code: label,
      amount: formatAmount(amountByValuation(stat, valuation)),
    }),
  )
  return parts.join(t('dashboard.data-board.page.inventory.breakdownSep'))
}

/** dashboard KPI：卡面总金额；悬停 F/H/R */
const metricItems = computed(() => {
  const ranges = getLastThreeCalendarMonthRanges()
  return enrichDashboardMetricItems(
    ranges.map((range) => {
      const stat = monthStats.value[range.key] ?? EMPTY_STAT
      const total = Number(stat.monthStockAmount) || 0
      return {
        key: range.key,
        title: t('dashboard.data-board.page.inventory.monthamount', { month: range.month }),
        value: total,
        prefix: '¥',
        tooltip: buildValuationTooltip(stat),
        routePath: DASHBOARD_STATS_ROUTE.materialMovingPrice,
      }
    }),
  )
})

/**
 * 加载单月在库金额
 * @param valuationPeriod 评估期间 yyyy-MM
 * @returns {Promise<MaterialMovingPriceStat>} 统计
 */
async function loadMonthStat(valuationPeriod: string): Promise<MaterialMovingPriceStat> {
  const res = await fetchDashboardGet<MaterialMovingPriceStat>(DASHBOARD_STATS_API.materialMovingPriceStockStat, {
    valuationPeriod,
  })
  if (!res) {
    return { ...EMPTY_STAT, statMonth: valuationPeriod, byValuation: [] }
  }
  return {
    ...res,
    byValuation: res.byValuation ?? [],
  }
}

/**
 * 加载前三月在库金额
 * @returns {Promise<void>}
 */
async function loadData(): Promise<void> {
  const canList = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.materialMovingPriceList)
  const ranges = getLastThreeCalendarMonthRanges()
  loading.value = true
  try {
    const stats = await fetchDashboardMetricIfPermitted(
      canList,
      'materialMovingPriceStockStat',
      () =>
        Promise.all(
          ranges.map((range) =>
            loadMonthStat(range.key).then((stat) => ({ key: range.key, stat })),
          ),
        ).then((rows) => {
          const map: Record<string, MaterialMovingPriceStat> = {}
          rows.forEach(({ key, stat }) => {
            map[key] = stat
          })
          return map
        }),
      {},
    )
    monthStats.value = stats
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  scheduleDashboardLoad(loadData)
})

useTableRefresh(() => {
  scheduleDashboardLoad(loadData)
})
</script>

<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/dashboard/dataBoard.types/modules -->
<!-- 文件名称：StatsSalesInvoiceModule.vue -->
<!-- 功能描述：数据看板销售发票金额（近三月） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="min-h-[80px]">
    <p class="mb-3 text-base font-medium leading-relaxed text-text">
      {{ t('dashboard.data-board.page.salesinvoice.summaryTitle') }}
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
 * 销售发票金额：近三个自然月（TaktSalesActualTrends/invoice-stat）
 */
import { useI18n } from 'vue-i18n'
import StatsMetricGrid from '../components/StatsMetricGrid.vue'
import { usePermissionStore } from '@/stores/identity/usePermissionStore'
import type { SalesInvoiceStat } from '@/types/logistics/sales/salesStat.types'
import { enrichDashboardMetricItems } from '../utils/statsMetricDashboard'
import {
  DASHBOARD_STATS_API,
  DASHBOARD_STATS_PERMISSION,
  DASHBOARD_STATS_ROUTE,
  fetchDashboardGet,
  fetchDashboardMetricIfPermitted,
  scheduleDashboardLoad,
} from '../utils/statsQuery'
import { getLastThreeCalendarMonthRanges } from '../utils/statsDashboardPlant'

/** 空销售发票统计 */
const EMPTY_STAT: SalesInvoiceStat = {
  statMonth: '',
  yearMonth: '',
  monthInvoiceCount: 0,
  monthSalesAmount: 0,
  compareInvoiceCount: 0,
  compareSalesAmount: 0,
  invoiceCountYoYPercent: 0,
  salesAmountYoYPercent: 0,
}

const { t } = useI18n()
const permissionStore = usePermissionStore()

/** 列表 loading */
const loading = ref(false)
/** 近三月各月统计（key = yyyy-MM） */
const monthStats = ref<Record<string, SalesInvoiceStat>>({})

/** dashboard KPI 指标 */
const metricItems = computed(() => {
  const ranges = getLastThreeCalendarMonthRanges()
  return enrichDashboardMetricItems(
    ranges.map((range) => {
      const stat = monthStats.value[range.key] ?? EMPTY_STAT
      return {
        key: range.key,
        title: t('dashboard.data-board.page.salesinvoice.monthamount', { month: range.month }),
        value: Number(stat.monthSalesAmount) || 0,
        prefix: '¥',
        precision: 2,
        routePath: DASHBOARD_STATS_ROUTE.salesInvoice,
      }
    }),
  )
})

/**
 * 加载单月销售发票统计
 * @param start 区间开始
 * @param end 区间结束
 * @returns {Promise<SalesInvoiceStat>} 统计
 */
async function loadMonthStat(start: string, end: string): Promise<SalesInvoiceStat> {
  const res = await fetchDashboardGet<SalesInvoiceStat>(DASHBOARD_STATS_API.salesInvoiceStat, {
    postingDateStart: start,
    postingDateEnd: end,
  })
  return res ?? { ...EMPTY_STAT }
}

/**
 * 加载近三个月销售发票金额
 * @returns {Promise<void>}
 */
async function loadData(): Promise<void> {
  const canList = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.salesInvoiceStat)
  const ranges = getLastThreeCalendarMonthRanges()
  loading.value = true
  try {
    const stats = await fetchDashboardMetricIfPermitted(
      canList,
      'salesInvoiceStat',
      () =>
        Promise.all(
          ranges.map((range) =>
            loadMonthStat(range.start, range.end).then((stat) => ({ key: range.key, stat })),
          ),
        ).then((rows) => {
          const map: Record<string, SalesInvoiceStat> = {}
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

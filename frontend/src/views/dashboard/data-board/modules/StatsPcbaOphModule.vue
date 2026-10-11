<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/dashboard/dataBoard.types/modules -->
<!-- 文件名称：StatsPcbaOphModule.vue -->
<!-- 功能描述：数据看板 PCBA OPH（近三月；TaktPcbaOutputDetail 当日完成数合计） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="min-h-[80px]">
    <p class="mb-3 text-base font-medium leading-relaxed text-text">
      {{ t('dashboard.data-board.page.pcbaoph.summaryTitle') }}
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
 * PCBA OPH：近三个自然月当日完成数合计（TaktPcbaOutputDetail.DailyCompletedQty）
 */
import { useI18n } from 'vue-i18n'
import StatsMetricGrid from '../components/StatsMetricGrid.vue'
import { usePermissionStore } from '@/stores/identity/usePermissionStore'
import type { OutputProductionStat } from '@/types/logistics/manufacturing/output/outputStat.types'
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

/** 空生产统计 */
const EMPTY_STAT: OutputProductionStat = {
  statMonth: '',
  monthStdCapacity: 0,
  monthProdActualQty: 0,
  monthAchievementRate: 0,
  monthDowntimeMinutes: 0,
  monthInputMinutes: 0,
  monthProdMinutes: 0,
  monthActualMinutes: 0,
  teams: [],
}

const { t } = useI18n()
const permissionStore = usePermissionStore()

/** 列表 loading */
const loading = ref(false)
/** 近三月各月统计（key = yyyy-MM） */
const monthStats = ref<Record<string, OutputProductionStat>>({})

/** dashboard KPI 指标（当日完成数） */
const metricItems = computed(() => {
  const ranges = getLastThreeCalendarMonthRanges()
  return enrichDashboardMetricItems(
    ranges.map((range) => {
      const stat = monthStats.value[range.key] ?? EMPTY_STAT
      return {
        key: range.key,
        title: t('dashboard.data-board.page.pcbaoph.monthqty', { month: range.month }),
        value: Math.round(Number(stat.monthProdActualQty) || 0),
        precision: 0,
        routePath: DASHBOARD_STATS_ROUTE.pcbaOutput,
      }
    }),
  )
})

/**
 * 加载单月 PCBA 产出统计
 * @param start 区间开始
 * @param end 区间结束
 * @returns {Promise<OutputProductionStat>} 统计
 */
async function loadMonthStat(start: string, end: string): Promise<OutputProductionStat> {
  const res = await fetchDashboardGet<OutputProductionStat>(
    DASHBOARD_STATS_API.pcbaOutputProductionStat,
    { prodDateStart: start, prodDateEnd: end },
  )
  return { ...EMPTY_STAT, ...res, teams: res?.teams ?? [] }
}

/**
 * 加载近三个月 PCBA 当日完成数
 * @returns {Promise<void>}
 */
async function loadData(): Promise<void> {
  const canList = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.pcbaOutputList)
  const ranges = getLastThreeCalendarMonthRanges()
  loading.value = true
  try {
    const stats = await fetchDashboardMetricIfPermitted(
      canList,
      'pcbaProductionStat',
      () =>
        Promise.all(
          ranges.map((range) =>
            loadMonthStat(range.start, range.end).then((stat) => ({ key: range.key, stat })),
          ),
        ).then((rows) => {
          const map: Record<string, OutputProductionStat> = {}
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

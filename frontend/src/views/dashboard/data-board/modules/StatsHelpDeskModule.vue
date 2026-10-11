<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/dashboard/dataBoard.types/modules -->
<!-- 文件名称：StatsHelpDeskModule.vue -->
<!-- 功能描述：数据看板服务台工单（上月：件数/未处理/处理中/已处理） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="min-h-[80px]">
    <p class="mb-3 text-base font-medium leading-relaxed text-text">
      {{ t('dashboard.data-board.page.helpdesk.summaryTitle') }}
    </p>
    <StatsMetricGrid
      card-variant="dashboard-compact"
      :loading="loading"
      :period-label="t('dashboard.data-board.page.periodlastmonth')"
      :items="metricItems"
      :col-xs="12"
      :col-sm="12"
      :col-md="12"
      :col-lg="6"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 服务台工单：上月件数 / 未处理 / 处理中 / 已处理
 */
import { useI18n } from 'vue-i18n'
import StatsMetricGrid from '../components/StatsMetricGrid.vue'
import { enrichDashboardMetricItems } from '../utils/statsMetricDashboard'
import { usePermissionStore } from '@/stores/identity/usePermissionStore'
import type { HelpDeskTicketStat } from '@/types/routine/help-desk/ticketStat.types'
import {
  DASHBOARD_STATS_API,
  DASHBOARD_STATS_PERMISSION,
  DASHBOARD_STATS_ROUTE,
  fetchDashboardGet,
  fetchDashboardMetricIfPermitted,
  scheduleDashboardLoad,
} from '../utils/statsQuery'
import { getDashboardInspectionMonthRange } from '../utils/statsDashboardPlant'

/** 空统计 */
const EMPTY_STAT: HelpDeskTicketStat = {
  statMonth: '',
  monthTicketCount: 0,
  monthPendingCount: 0,
  monthInProgressCount: 0,
  monthProcessedCount: 0,
}

const { t } = useI18n()
const permissionStore = usePermissionStore()

/** 列表 loading */
const loading = ref(false)
/** 工单件数统计 */
const ticketStat = ref<HelpDeskTicketStat>({ ...EMPTY_STAT })

/** dashboard KPI 指标 */
const metricItems = computed(() =>
  enrichDashboardMetricItems([
    {
      key: 'ticketCount',
      title: t('dashboard.data-board.page.helpdesk.ticketcount'),
      value: Number(ticketStat.value.monthTicketCount) || 0,
      routePath: DASHBOARD_STATS_ROUTE.helpDeskTicket,
    },
    {
      key: 'pendingCount',
      title: t('dashboard.data-board.page.helpdesk.pendingcount'),
      value: Number(ticketStat.value.monthPendingCount) || 0,
      routePath: DASHBOARD_STATS_ROUTE.helpDeskTicket,
    },
    {
      key: 'inProgressCount',
      title: t('dashboard.data-board.page.helpdesk.inprogresscount'),
      value: Number(ticketStat.value.monthInProgressCount) || 0,
      routePath: DASHBOARD_STATS_ROUTE.helpDeskTicket,
    },
    {
      key: 'processedCount',
      title: t('dashboard.data-board.page.helpdesk.processedcount'),
      value: Number(ticketStat.value.monthProcessedCount) || 0,
      routePath: DASHBOARD_STATS_ROUTE.helpDeskTicket,
    },
  ]),
)

/**
 * 加载上月服务台工单件数统计
 * @returns {Promise<void>}
 */
async function loadData(): Promise<void> {
  const canList = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.helpDeskTicketList)
  loading.value = true
  try {
    const range = getDashboardInspectionMonthRange(1)
    const stat = await fetchDashboardMetricIfPermitted(
      canList,
      'helpDeskTicketStat',
      async () => {
        const res = await fetchDashboardGet<HelpDeskTicketStat>(DASHBOARD_STATS_API.helpDeskTicketStat, {
          createdAtStart: range.start,
          createdAtEnd: range.end,
        })
        return res ?? { ...EMPTY_STAT }
      },
      { ...EMPTY_STAT },
    )
    ticketStat.value = stat
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

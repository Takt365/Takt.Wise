<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/dashboard/dataBoard.types/modules -->
<!-- 文件名称：StatsQualityComplaintModule.vue -->
<!-- 功能描述：数据看板客诉统计（上月：件数/未处理/处理中/处理完成） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="min-h-[80px]">
    <p class="mb-3 text-base font-medium leading-relaxed text-text">
      {{ t('dashboard.data-board.page.qualitycomplaint.summaryTitle') }}
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
 * 客诉统计：上月客诉件数 / 未处理 / 处理中 / 处理完成
 */
import { useI18n } from 'vue-i18n'
import StatsMetricGrid from '../components/StatsMetricGrid.vue'
import { enrichDashboardMetricItems } from '../utils/statsMetricDashboard'
import { usePermissionStore } from '@/stores/identity/usePermissionStore'
import type { CustomerComplaintStat } from '@/types/logistics/quality/complaint/customerComplaintStat.types'
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
const EMPTY_STAT: CustomerComplaintStat = {
  statMonth: '',
  monthComplaintCount: 0,
  monthPendingCount: 0,
  monthInProgressCount: 0,
  monthCompletedCount: 0,
}

const { t } = useI18n()
const permissionStore = usePermissionStore()

/** 列表 loading */
const loading = ref(false)
/** 客诉件数统计 */
const complaintStat = ref<CustomerComplaintStat>({ ...EMPTY_STAT })

/** dashboard KPI 指标 */
const metricItems = computed(() =>
  enrichDashboardMetricItems([
    {
      key: 'complaintCount',
      title: t('dashboard.data-board.page.qualitycomplaint.complaintcount'),
      value: Number(complaintStat.value.monthComplaintCount) || 0,
      routePath: DASHBOARD_STATS_ROUTE.customerComplaint,
    },
    {
      key: 'pendingCount',
      title: t('dashboard.data-board.page.qualitycomplaint.pendingcount'),
      value: Number(complaintStat.value.monthPendingCount) || 0,
      routePath: DASHBOARD_STATS_ROUTE.customerComplaint,
    },
    {
      key: 'inProgressCount',
      title: t('dashboard.data-board.page.qualitycomplaint.inprogresscount'),
      value: Number(complaintStat.value.monthInProgressCount) || 0,
      routePath: DASHBOARD_STATS_ROUTE.customerComplaint,
    },
    {
      key: 'completedCount',
      title: t('dashboard.data-board.page.qualitycomplaint.completedcount'),
      value: Number(complaintStat.value.monthCompletedCount) || 0,
      routePath: DASHBOARD_STATS_ROUTE.customerComplaint,
    },
  ]),
)

/**
 * 加载上月客诉件数统计
 * @returns {Promise<void>}
 */
async function loadData(): Promise<void> {
  const canList = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.customerComplaintList)
  loading.value = true
  try {
    const range = getDashboardInspectionMonthRange(1)
    const stat = await fetchDashboardMetricIfPermitted(
      canList,
      'customerComplaintStat',
      async () => {
        const res = await fetchDashboardGet<CustomerComplaintStat>(
          DASHBOARD_STATS_API.customerComplaintStat,
          {
            complaintDateStart: range.start,
            complaintDateEnd: range.end,
          },
        )
        return res ?? { ...EMPTY_STAT }
      },
      { ...EMPTY_STAT },
    )
    complaintStat.value = stat
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

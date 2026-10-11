<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/dashboard/dataBoard.types/modules -->
<!-- 文件名称：StatsOnlineModule.vue -->
<!-- 功能描述：数据看板在线统计（在线用户 / 今日访问） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="min-h-[80px]">
    <p class="mb-3 text-base font-medium leading-relaxed text-text">
      {{ t('dashboard.data-board.page.online.summaryTitle') }}
    </p>
    <StatsMetricGrid
    card-variant="dashboard-compact"
    :loading="loading"
    :items="metricItems"
    :col-xs="12"
    :col-sm="12"
    :col-md="12"
    :col-lg="12"
  />
  </div>
</template>

<script setup lang="ts">
/**
 * 在线统计：公司维度看板（在线人数、当日访问）
 */
import { useI18n } from 'vue-i18n'
import StatsMetricGrid from '../components/StatsMetricGrid.vue'
import { usePermissionStore } from '@/stores/identity/usePermissionStore'
import type { OnlineDashboardStatistics } from '@/types/foundation/online.types'
import { enrichDashboardMetricItems } from '../utils/statsMetricDashboard'
import {
  DASHBOARD_STATS_API,
  DASHBOARD_STATS_PERMISSION,
  fetchDashboardGet,
  fetchDashboardMetricIfPermitted,
  scheduleDashboardLoad,
} from '../utils/statsQuery'

const { t } = useI18n()
const permissionStore = usePermissionStore()

/** 列表 loading */
const loading = ref(false)
/** 在线统计 */
const onlineStats = ref({
  users: 0,
  todayVisits: 0,
})

/** dashboard KPI 指标 */
const metricItems = computed(() =>
  enrichDashboardMetricItems([
    { key: 'users', title: t('dashboard.data-board.page.online.users'), value: onlineStats.value.users },
    { key: 'todayvisits', title: t('dashboard.data-board.page.online.todayvisits'), value: onlineStats.value.todayVisits },
  ]),
)

/**
 * 加载在线看板统计
 * @returns {Promise<void>}
 */
async function loadData(): Promise<void> {
  loading.value = true
  try {
    const stats = await fetchDashboardMetricIfPermitted(
      permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.onlineDashboard),
      'onlineDashboard',
      () => fetchDashboardGet<OnlineDashboardStatistics>(DASHBOARD_STATS_API.onlineDashboard),
      null,
    )
    onlineStats.value = {
      users: stats?.onlineUserCount ?? 0,
      todayVisits: stats?.todayVisitCount ?? 0,
    }
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

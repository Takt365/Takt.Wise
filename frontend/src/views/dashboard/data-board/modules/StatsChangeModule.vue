<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/dashboard/dataBoard.types/modules -->
<!-- 文件名称：StatsChangeModule.vue -->
<!-- 功能描述：数据看板设变统计（设变摘要 + 未实施/实施中/已实施） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="min-h-[80px]">
    <p class="mb-3 text-base font-medium leading-relaxed text-text">
      {{ ecSummaryLine }}
    </p>
    <StatsMetricGrid
      card-variant="dashboard-compact"
      :loading="loading"
      :period-label="t('dashboard.data-board.page.periodmonth')"
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
 * 设变统计：TaktEcGijutsus/stat 当月摘要；Kanban 实施中；部门行未实施/已实施
 */
import { useI18n } from 'vue-i18n'
import StatsMetricGrid from '../components/StatsMetricGrid.vue'
import { usePermissionStore } from '@/stores/identity/usePermissionStore'
import { TaktEcImplementationStatus } from '@/constants/logistics/ecImplementationStatus.constants'
import type { EcGijutsuStat } from '@/types/logistics/manufacturing/engineering-change/ecGijutsu.types'
import { enrichDashboardMetricItems } from '../utils/statsMetricDashboard'
import {
  DASHBOARD_STATS_API,
  DASHBOARD_STATS_PERMISSION,
  fetchDashboardGet,
  fetchDashboardDeptExecutionCount,
  fetchDashboardMetricIfPermitted,
  fetchDashboardPagedTotal,
  getCurrentMonthRange,
  scheduleDashboardLoad,
} from '../utils/statsQuery'

/** isImplemented：未实施 */
const EC_DEPT_NOT_IMPLEMENTED = 0
/** isImplemented：已实施 */
const EC_DEPT_IMPLEMENTED = 1

/** 空设变主统计 */
const EMPTY_EC_STAT: EcGijutsuStat = {
  statMonth: '',
  ecCount: 0,
  ecDetailCount: 0,
}

const { t } = useI18n()
const permissionStore = usePermissionStore()

/** 列表 loading */
const loading = ref(false)
/** 设变主表统计 */
const ecStat = ref<EcGijutsuStat>({ ...EMPTY_EC_STAT })
/** 设变实施统计（未实施 / 实施中 / 已实施） */
const changeData = ref({
  notImplemented: 0,
  inProgressEc: 0,
  implemented: 0,
})

/** 当月设变摘要行，如「当月设变1（18）」 */
const ecSummaryLine = computed(() =>
  t('dashboard.data-board.page.change.summaryEcCount', {
    ecCount: ecStat.value.ecCount,
    detailCount: ecStat.value.ecDetailCount,
  }),
)

/** dashboard KPI：未实施、实施中、已实施 */
const metricItems = computed(() =>
  enrichDashboardMetricItems([
    { key: 'notimplemented', title: t('dashboard.data-board.page.change.notimplemented'), value: changeData.value.notImplemented },
    { key: 'inprogress', title: t('dashboard.data-board.page.change.inprogress'), value: changeData.value.inProgressEc },
    { key: 'implemented', title: t('dashboard.data-board.page.change.implemented'), value: changeData.value.implemented },
  ]),
)

/**
 * 加载设变主表统计（当月录入日期范围）
 * @returns {Promise<EcGijutsuStat>} 设变统计
 */
async function loadEcGijutsuStat(): Promise<EcGijutsuStat> {
  const monthRange = getCurrentMonthRange()
  const data = await fetchDashboardGet<EcGijutsuStat>(DASHBOARD_STATS_API.ecStat, {
    ecEntryDateStart: monthRange.start,
    ecEntryDateEnd: monthRange.end,
  })
  return data ?? { ...EMPTY_EC_STAT }
}

/**
 * 加载设变统计（摘要 + 三项 KPI）
 * @returns {Promise<void>}
 */
async function loadData(): Promise<void> {
  const canListEc = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.ecList)
  const canListDept = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.ecDeptExecutionCount)
  const canListKanban = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.ecKanbanList)
  loading.value = true
  try {
    const [stat, notImplemented, inProgressEc, implemented] = await Promise.all([
      fetchDashboardMetricIfPermitted(
        canListEc,
        'ecStat',
        () => loadEcGijutsuStat(),
        { ...EMPTY_EC_STAT },
      ),
      fetchDashboardMetricIfPermitted(
        canListDept,
        'ecDeptNotImplemented',
        () => fetchDashboardDeptExecutionCount(EC_DEPT_NOT_IMPLEMENTED),
        0,
      ),
      fetchDashboardMetricIfPermitted(
        canListKanban,
        'ecKanbanInProgress',
        () => fetchDashboardPagedTotal(DASHBOARD_STATS_API.ecKanbanList, {
          implementationStatus: TaktEcImplementationStatus.InProgress,
        }),
        0,
      ),
      fetchDashboardMetricIfPermitted(
        canListDept,
        'ecDeptImplemented',
        () => fetchDashboardDeptExecutionCount(EC_DEPT_IMPLEMENTED),
        0,
      ),
    ])
    ecStat.value = stat
    changeData.value = { notImplemented, inProgressEc, implemented }
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

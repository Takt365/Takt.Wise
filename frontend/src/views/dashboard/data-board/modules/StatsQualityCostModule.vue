<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/dashboard/dataBoard.types/modules -->
<!-- 文件名称：StatsQualityCostModule.vue -->
<!-- 功能描述：数据看板质量成本（上月：品质业务/事故/应对金额） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="min-h-[80px]">
    <p class="mb-3 text-base font-medium leading-relaxed text-text">
      {{ t('dashboard.data-board.page.qualitycost.summaryTitle') }}
    </p>
    <StatsMetricGrid
      card-variant="dashboard-compact"
      :loading="loading"
      :period-label="t('dashboard.data-board.page.periodlastmonth')"
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
 * 质量成本：品质业务 / 事故 / 应对 三项金额（上月）
 */
import { useI18n } from 'vue-i18n'
import StatsMetricGrid from '../components/StatsMetricGrid.vue'
import { enrichDashboardMetricItems } from '../utils/statsMetricDashboard'
import { usePermissionStore } from '@/stores/identity/usePermissionStore'
import type { QualityCostStat } from '@/types/logistics/quality/cost/qualityCostStat.types'
import {
  DASHBOARD_STATS_API,
  DASHBOARD_STATS_PERMISSION,
  DASHBOARD_STATS_ROUTE,
  fetchDashboardGet,
  fetchDashboardMetricIfPermitted,
  scheduleDashboardLoad,
} from '../utils/statsQuery'
import {
  getDashboardFocusMonth,
  getDashboardInspectionMonthRange,
} from '../utils/statsDashboardPlant'

/** 空金额统计 */
const EMPTY_STAT: QualityCostStat = {
  statMonth: '',
  monthTotalAmount: 0,
  monthRowCount: 0,
}

const { t } = useI18n()
const permissionStore = usePermissionStore()

/** 列表 loading */
const loading = ref(false)
/** 品质业务金额 */
const assuranceStat = ref<QualityCostStat>({ ...EMPTY_STAT })
/** 品质事故金额 */
const incidentStat = ref<QualityCostStat>({ ...EMPTY_STAT })
/** 品质应对金额 */
const issueStat = ref<QualityCostStat>({ ...EMPTY_STAT })

/** dashboard KPI 指标 */
const metricItems = computed(() =>
  enrichDashboardMetricItems([
    {
      key: 'assurance',
      title: t('dashboard.data-board.page.qualitycost.assuranceamount'),
      value: Number(assuranceStat.value.monthTotalAmount) || 0,
      prefix: '¥',
      precision: 2,
      routePath: DASHBOARD_STATS_ROUTE.qualityAssurance,
    },
    {
      key: 'incident',
      title: t('dashboard.data-board.page.qualitycost.incidentamount'),
      value: Number(incidentStat.value.monthTotalAmount) || 0,
      prefix: '¥',
      precision: 2,
      routePath: DASHBOARD_STATS_ROUTE.qualityIncident,
    },
    {
      key: 'issue',
      title: t('dashboard.data-board.page.qualitycost.issueamount'),
      value: Number(issueStat.value.monthTotalAmount) || 0,
      prefix: '¥',
      precision: 2,
      routePath: DASHBOARD_STATS_ROUTE.qualityIssue,
    },
  ]),
)

/**
 * 加载品质业务金额（按 AssuranceMonth）
 * @returns {Promise<QualityCostStat>}
 */
async function loadAssuranceStat(): Promise<QualityCostStat> {
  const focusMonth = getDashboardFocusMonth(1)
  const res = await fetchDashboardGet<QualityCostStat>(DASHBOARD_STATS_API.qualityAssuranceCostStat, {
    statMonth: focusMonth,
  })
  return res ?? { ...EMPTY_STAT, statMonth: focusMonth }
}

/**
 * 加载品质事故/应对金额（按日期区间）
 * @param apiPath cost-stat 路径
 * @returns {Promise<QualityCostStat>}
 */
async function loadDateRangeCostStat(apiPath: string): Promise<QualityCostStat> {
  const range = getDashboardInspectionMonthRange(1)
  const res = await fetchDashboardGet<QualityCostStat>(apiPath, {
    dateStart: range.start,
    dateEnd: range.end,
  })
  return res ?? { ...EMPTY_STAT, statMonth: getDashboardFocusMonth(1) }
}

/**
 * 加载上月质量成本三项金额
 * @returns {Promise<void>}
 */
async function loadData(): Promise<void> {
  const canAssurance = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.qualityAssuranceList)
  const canIncident = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.qualityIncidentList)
  const canIssue = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.qualityIssueList)
  loading.value = true
  try {
    const [assurance, incident, issue] = await Promise.all([
      fetchDashboardMetricIfPermitted(
        canAssurance,
        'qualityAssuranceCostStat',
        loadAssuranceStat,
        { ...EMPTY_STAT },
      ),
      fetchDashboardMetricIfPermitted(
        canIncident,
        'qualityIncidentCostStat',
        () => loadDateRangeCostStat(DASHBOARD_STATS_API.qualityIncidentCostStat),
        { ...EMPTY_STAT },
      ),
      fetchDashboardMetricIfPermitted(
        canIssue,
        'qualityIssueCostStat',
        () => loadDateRangeCostStat(DASHBOARD_STATS_API.qualityIssueCostStat),
        { ...EMPTY_STAT },
      ),
    ])
    assuranceStat.value = assurance
    incidentStat.value = incident
    issueStat.value = issue
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

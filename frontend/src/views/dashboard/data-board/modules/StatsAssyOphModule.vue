<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/dashboard/dataBoard.types/modules -->
<!-- 文件名称：StatsAssyOphModule.vue -->
<!-- 功能描述：数据看板组立 OPH（按生产班组：班别/计划数/生产数/达成率） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="min-h-[80px]">
    <div class="mb-3 flex items-baseline justify-between gap-3">
      <p class="text-base font-medium leading-relaxed text-text">
        {{ t('dashboard.data-board.page.assyoph.summaryTitle') }}
      </p>
      <p class="shrink-0 text-xs leading-relaxed text-text-secondary">
        {{ t('dashboard.data-board.page.periodlastmonth') }}
      </p>
    </div>
    <a-spin :spinning="loading">
      <a-table
        size="small"
        :pagination="false"
        :columns="tableColumns"
        :data-source="tableRows"
        :row-key="(row: OphTeamRow) => row.key"
        :custom-row="createCustomRow"
        :scroll="{ x: true }"
      />
    </a-spin>
  </div>
</template>

<script setup lang="ts">
/**
 * 组立 OPH：上月按生产班组（TeamCode）汇总计划数/生产数/达成率
 */
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { useRouter } from 'vue-router'
import { usePermissionStore } from '@/stores/identity/usePermissionStore'
import type { OutputProductionStat } from '@/types/logistics/manufacturing/output/outputStat.types'
import {
  DASHBOARD_STATS_API,
  DASHBOARD_STATS_PERMISSION,
  DASHBOARD_STATS_ROUTE,
  fetchDashboardGet,
  fetchDashboardMetricIfPermitted,
  getLastMonthRange,
  scheduleDashboardLoad,
} from '../utils/statsQuery'

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

/** 班组行 */
interface OphTeamRow {
  key: string
  teamLabel: string
  planQty: number
  actualQty: number
  achievementRate: number
}

const { t } = useI18n()
const router = useRouter()
const permissionStore = usePermissionStore()

/** 列表 loading */
const loading = ref(false)
/** 组立生产统计 */
const stats = ref<OutputProductionStat>({ ...EMPTY_STAT })

/**
 * 班组编码展示（空则占位）
 * @param teamCode 班组编码
 * @returns {string} 展示文案
 */
function formatTeamLabel(teamCode: string): string {
  const code = teamCode?.trim()
  return code || t('dashboard.data-board.page.assyoph.teamEmpty')
}

/**
 * 是否可跳转产出清单
 * @returns {boolean} 是否可跳转
 */
function canNavigate(): boolean {
  return permissionStore.canAccess(DASHBOARD_STATS_ROUTE.assyOutput)
}

/**
 * 跳转组立产出清单
 * @returns {void}
 */
function navigateToList(): void {
  if (!canNavigate()) {
    return
  }
  void router.push(DASHBOARD_STATS_ROUTE.assyOutput)
}

/**
 * 表格行属性
 * @returns 行 HTML 属性
 */
function createCustomRow() {
  const navigable = canNavigate()
  return {
    style: navigable ? { cursor: 'pointer' } : undefined,
    onClick: () => {
      navigateToList()
    },
  }
}

/** 表格列 */
const tableColumns = computed<TableColumnsType>(() => [
  {
    title: t('dashboard.data-board.page.assyoph.colShift'),
    dataIndex: 'teamLabel',
    key: 'teamLabel',
    width: 100,
  },
  {
    title: t('dashboard.data-board.page.assyoph.colPlan'),
    dataIndex: 'planQty',
    key: 'planQty',
    align: 'right',
  },
  {
    title: t('dashboard.data-board.page.assyoph.colActual'),
    dataIndex: 'actualQty',
    key: 'actualQty',
    align: 'right',
  },
  {
    title: t('dashboard.data-board.page.assyoph.colRate'),
    dataIndex: 'achievementRate',
    key: 'achievementRate',
    align: 'right',
    customRender: ({ text }: { text: number }) => `${Number(text).toFixed(1)}%`,
  },
])

/** 按班组行 */
const tableRows = computed<OphTeamRow[]>(() =>
  (stats.value.teams ?? []).map((item, index) => ({
    key: `${item.teamCode || 'empty'}-${index}`,
    teamLabel: formatTeamLabel(item.teamCode),
    planQty: Math.round(Number(item.stdCapacity) || 0),
    actualQty: Math.round(Number(item.prodActualQty) || 0),
    achievementRate: Number(item.achievementRate) || 0,
  })),
)

/**
 * 加载上月组立 OPH
 * @returns {Promise<void>}
 */
async function loadData(): Promise<void> {
  const canList = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.assyOutputList)
  loading.value = true
  try {
    const month = getLastMonthRange()
    stats.value = await fetchDashboardMetricIfPermitted(
      canList,
      'assyProductionStat',
      () => fetchDashboardGet<OutputProductionStat>(
        DASHBOARD_STATS_API.assyOutputProductionStat,
        { prodDateStart: month.start, prodDateEnd: month.end },
      ).then((res) => ({ ...EMPTY_STAT, ...res, teams: res?.teams ?? [] })),
      { ...EMPTY_STAT },
    )
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

<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/dashboard/dataBoard.types/modules -->
<!-- 文件名称：StatsAssyDefectModule.vue -->
<!-- 功能描述：数据看板组立不良（按生产班组：班别/生产数/无不良/不良数/直行率/不良率） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="min-h-[80px]">
    <div class="mb-3 flex items-baseline justify-between gap-3">
      <p class="text-base font-medium leading-relaxed text-text">
        {{ t('dashboard.data-board.page.assydefect.summaryTitle') }}
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
        :row-key="(row: DefectTeamRow) => row.key"
        :custom-row="createCustomRow"
        :scroll="{ x: true }"
      />
    </a-spin>
  </div>
</template>

<script setup lang="ts">
/**
 * 组立不良：上月按生产班组汇总
 */
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { useRouter } from 'vue-router'
import { usePermissionStore } from '@/stores/identity/usePermissionStore'
import type { DefectStat } from '@/types/logistics/manufacturing/defect/defectStat.types'
import {
  DASHBOARD_STATS_API,
  DASHBOARD_STATS_PERMISSION,
  DASHBOARD_STATS_ROUTE,
  fetchDashboardGet,
  fetchDashboardMetricIfPermitted,
  getLastMonthRange,
  scheduleDashboardLoad,
} from '../utils/statsQuery'

/** 空不良统计 */
const EMPTY_STAT: DefectStat = {
  statMonth: '',
  monthBaseQty: 0,
  monthGoodQty: 0,
  monthDefectQty: 0,
  monthDefectRatePercent: 0,
  monthYieldRatePercent: 0,
  teams: [],
}

/** 班组行 */
interface DefectTeamRow {
  key: string
  teamLabel: string
  baseQty: number
  goodQty: number
  defectQty: number
  yieldRatePercent: number
  defectRatePercent: number
}

const { t } = useI18n()
const router = useRouter()
const permissionStore = usePermissionStore()

/** 列表 loading */
const loading = ref(false)
/** 组立不良统计 */
const stats = ref<DefectStat>({ ...EMPTY_STAT })

/**
 * 班组编码展示
 * @param teamCode 班组编码
 * @returns {string} 展示文案
 */
function formatTeamLabel(teamCode: string): string {
  const code = teamCode?.trim()
  return code || t('dashboard.data-board.page.assydefect.teamEmpty')
}

/**
 * 是否可跳转
 * @returns {boolean} 是否可跳转
 */
function canNavigate(): boolean {
  return permissionStore.canAccess(DASHBOARD_STATS_ROUTE.assyDefect)
}

/**
 * 跳转组立不良清单
 * @returns {void}
 */
function navigateToList(): void {
  if (!canNavigate()) {
    return
  }
  void router.push(DASHBOARD_STATS_ROUTE.assyDefect)
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
    title: t('dashboard.data-board.page.assydefect.colShift'),
    dataIndex: 'teamLabel',
    key: 'teamLabel',
    width: 100,
  },
  {
    title: t('dashboard.data-board.page.assydefect.colBase'),
    dataIndex: 'baseQty',
    key: 'baseQty',
    align: 'right',
  },
  {
    title: t('dashboard.data-board.page.assydefect.colGood'),
    dataIndex: 'goodQty',
    key: 'goodQty',
    align: 'right',
  },
  {
    title: t('dashboard.data-board.page.assydefect.colDefect'),
    dataIndex: 'defectQty',
    key: 'defectQty',
    align: 'right',
  },
  {
    title: t('dashboard.data-board.page.assydefect.colYieldRate'),
    dataIndex: 'yieldRatePercent',
    key: 'yieldRatePercent',
    align: 'right',
    customRender: ({ text }: { text: number }) => `${Number(text).toFixed(1)}%`,
  },
  {
    title: t('dashboard.data-board.page.assydefect.colDefectRate'),
    dataIndex: 'defectRatePercent',
    key: 'defectRatePercent',
    align: 'right',
    customRender: ({ text }: { text: number }) => `${Number(text).toFixed(1)}%`,
  },
])

/** 按班组行 */
const tableRows = computed<DefectTeamRow[]>(() =>
  (stats.value.teams ?? []).map((item, index) => ({
    key: `${item.teamCode || 'empty'}-${index}`,
    teamLabel: formatTeamLabel(item.teamCode),
    baseQty: Math.round(Number(item.baseQty) || 0),
    goodQty: Math.round(Number(item.goodQty) || 0),
    defectQty: Math.round(Number(item.defectQty) || 0),
    yieldRatePercent: Number(item.yieldRatePercent) || 0,
    defectRatePercent: Number(item.defectRatePercent) || 0,
  })),
)

/**
 * 加载上月组立不良
 * @returns {Promise<void>}
 */
async function loadData(): Promise<void> {
  const canList = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.assyDefectList)
  loading.value = true
  try {
    const month = getLastMonthRange()
    stats.value = await fetchDashboardMetricIfPermitted(
      canList,
      'assyDefectStat',
      () => fetchDashboardGet<DefectStat>(DASHBOARD_STATS_API.assyDefectStat, {
        prodDateStart: month.start,
        prodDateEnd: month.end,
      }).then((res) => ({ ...EMPTY_STAT, ...res, teams: res?.teams ?? [] })),
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

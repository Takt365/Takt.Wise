<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/dashboard/dataBoard.types/modules -->
<!-- 文件名称：StatsPcbaModule.vue -->
<!-- 功能描述：数据看板 PCBA 不良（班别 SMT/修理：检查数、修理数） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="min-h-[80px]">
    <div class="mb-3 flex items-baseline justify-between gap-3">
      <p class="text-base font-medium leading-relaxed text-text">
        {{ t('dashboard.data-board.page.pcba.summaryTitle') }}
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
        :row-key="(row: PcbaDefectRow) => row.key"
        :custom-row="createCustomRow"
        :scroll="{ x: true }"
      />
    </a-spin>
  </div>
</template>

<script setup lang="ts">
/**
 * PCBA 不良：上月 SMT 检查数 + 修理数
 */
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { useRouter } from 'vue-router'
import { usePermissionStore } from '@/stores/identity/usePermissionStore'
import type { PcbaDefectBoardStat } from '@/types/logistics/manufacturing/defect/pcbaDefectBoardStat.types'
import {
  DASHBOARD_STATS_API,
  DASHBOARD_STATS_PERMISSION,
  DASHBOARD_STATS_ROUTE,
  fetchDashboardGet,
  fetchDashboardMetricIfPermitted,
  getLastMonthRange,
  scheduleDashboardLoad,
} from '../utils/statsQuery'

/** 空统计 */
const EMPTY_STAT: PcbaDefectBoardStat = {
  statMonth: '',
  inspectionQty: 0,
  repairQty: 0,
}

/** 行类型：smt=检查 / repair=修理 */
type PcbaDefectRowKind = 'smt' | 'repair'

/** 表格行 */
interface PcbaDefectRow {
  key: PcbaDefectRowKind
  kind: PcbaDefectRowKind
  teamLabel: string
  inspectionQty: number | null
  repairQty: number | null
}

const { t } = useI18n()
const router = useRouter()
const permissionStore = usePermissionStore()

/** 列表 loading */
const loading = ref(false)
/** PCBA 不良统计 */
const stats = ref<PcbaDefectBoardStat>({ ...EMPTY_STAT })

/**
 * 是否可跳转检查清单
 * @returns {boolean} 是否可跳转
 */
function canNavigateInspection(): boolean {
  return permissionStore.canAccess(DASHBOARD_STATS_ROUTE.pcbaInspection)
}

/**
 * 是否可跳转修理清单
 * @returns {boolean} 是否可跳转
 */
function canNavigateRepair(): boolean {
  return permissionStore.canAccess(DASHBOARD_STATS_ROUTE.pcbaRepair)
}

/**
 * 按行跳转清单
 * @param kind 行类型
 * @returns {void}
 */
function navigateByKind(kind: PcbaDefectRowKind): void {
  if (kind === 'smt') {
    if (!canNavigateInspection()) {
      return
    }
    void router.push(DASHBOARD_STATS_ROUTE.pcbaInspection)
    return
  }
  if (!canNavigateRepair()) {
    return
  }
  void router.push(DASHBOARD_STATS_ROUTE.pcbaRepair)
}

/**
 * 表格行属性
 * @param record 行数据
 * @returns 行 HTML 属性
 */
function createCustomRow(record: PcbaDefectRow) {
  const navigable =
    record.kind === 'smt' ? canNavigateInspection() : canNavigateRepair()
  return {
    style: navigable ? { cursor: 'pointer' } : undefined,
    onClick: () => {
      navigateByKind(record.kind)
    },
  }
}

/** 表格列 */
const tableColumns = computed<TableColumnsType>(() => [
  {
    title: t('dashboard.data-board.page.pcba.colShift'),
    dataIndex: 'teamLabel',
    key: 'teamLabel',
    width: 100,
  },
  {
    title: t('dashboard.data-board.page.pcba.colInspection'),
    dataIndex: 'inspectionQty',
    key: 'inspectionQty',
    align: 'right',
    customRender: ({ text }: { text: number | null }) =>
      text == null ? '—' : String(text),
  },
  {
    title: t('dashboard.data-board.page.pcba.colRepair'),
    dataIndex: 'repairQty',
    key: 'repairQty',
    align: 'right',
    customRender: ({ text }: { text: number | null }) =>
      text == null ? '—' : String(text),
  },
])

/** 固定两行：SMT / 修理 */
const tableRows = computed<PcbaDefectRow[]>(() => [
  {
    key: 'smt',
    kind: 'smt',
    teamLabel: t('dashboard.data-board.page.pcba.rowSmt'),
    inspectionQty: Math.round(Number(stats.value.inspectionQty) || 0),
    repairQty: null,
  },
  {
    key: 'repair',
    kind: 'repair',
    teamLabel: t('dashboard.data-board.page.pcba.rowRepair'),
    inspectionQty: null,
    repairQty: Math.round(Number(stats.value.repairQty) || 0),
  },
])

/**
 * 加载上月 PCBA 不良统计
 * @returns {Promise<void>}
 */
async function loadData(): Promise<void> {
  const canInspection = permissionStore.hasPermission(
    DASHBOARD_STATS_PERMISSION.pcbaInspectionList,
  )
  const canRepair = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.pcbaRepairList)
  const canList = canInspection || canRepair
  loading.value = true
  try {
    const month = getLastMonthRange()
    stats.value = await fetchDashboardMetricIfPermitted(
      canList,
      'pcbaDefectBoardStat',
      () =>
        fetchDashboardGet<PcbaDefectBoardStat>(DASHBOARD_STATS_API.pcbaDefectBoardStat, {
          prodDateStart: month.start,
          prodDateEnd: month.end,
        }).then((res) => ({ ...EMPTY_STAT, ...res })),
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

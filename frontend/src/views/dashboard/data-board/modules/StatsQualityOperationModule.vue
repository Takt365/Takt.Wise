<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/dashboard/dataBoard.types/modules -->
<!-- 文件名称：StatsQualityOperationModule.vue -->
<!-- 功能描述：数据看板质量检验统计（IQC/IPQC/FQC：批次数/抽样/合格/不合格/合格率/不良率） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="min-h-[80px]">
    <div class="mb-3 flex items-baseline justify-between gap-3">
      <p class="text-base font-medium leading-relaxed text-text">
        {{ t('dashboard.data-board.page.qualityoperation.summaryTitle') }}
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
        :row-key="(row: QualityInspectionTableRow) => row.key"
        :custom-row="createCustomRow"
        :scroll="{ x: true }"
      />
    </a-spin>
  </div>
</template>

<script setup lang="ts">
/**
 * 质量检验统计：IQC / IPQC / FQC 上月检验汇总表（行点击跳转各自明细清单）
 */
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { useRouter } from 'vue-router'
import { usePermissionStore } from '@/stores/identity/usePermissionStore'
import type { QualityInspectionStat } from '@/types/logistics/quality/operation/qualityInspectionStat.types'
import {
  DASHBOARD_STATS_API,
  DASHBOARD_STATS_PERMISSION,
  DASHBOARD_STATS_ROUTE,
  fetchDashboardGet,
  fetchDashboardMetricIfPermitted,
  scheduleDashboardLoad,
} from '../utils/statsQuery'
import { getDashboardInspectionMonthRange } from '../utils/statsDashboardPlant'

/** 空检验统计 */
const EMPTY_INSPECTION_STAT: QualityInspectionStat = {
  statMonth: '',
  monthOrderCount: 0,
  monthSampleQuantity: 0,
  monthQualifiedQuantity: 0,
  monthUnqualifiedQuantity: 0,
  monthPassRatePercent: 0,
}

/** 表格行 */
interface QualityInspectionTableRow {
  key: string
  typeLabel: string
  orderCount: number
  sampleQuantity: number
  qualifiedQuantity: number
  unqualifiedQuantity: number
  passRatePercent: number
  defectRatePercent: number
  routePath: string
}

const { t } = useI18n()
const router = useRouter()
const permissionStore = usePermissionStore()

/** 列表 loading */
const loading = ref(false)
/** IQC 统计 */
const iqcStat = ref<QualityInspectionStat>({ ...EMPTY_INSPECTION_STAT })
/** IPQC 统计 */
const ipqcStat = ref<QualityInspectionStat>({ ...EMPTY_INSPECTION_STAT })
/** FQC 统计 */
const fqcStat = ref<QualityInspectionStat>({ ...EMPTY_INSPECTION_STAT })

/**
 * 计算不良率（不合格 / 抽样 × 100）
 * @param unqualified 不合格数量
 * @param sample 抽样数量
 * @returns {number} 不良率 %
 */
function calcDefectRatePercent(unqualified: number, sample: number): number {
  if (sample <= 0) {
    return 0
  }
  return Math.round((unqualified / sample) * 10000) / 100
}

/**
 * 行是否可导航到明细清单
 * @param row 表格行
 * @returns {boolean} 是否可跳转
 */
function isRowNavigable(row: QualityInspectionTableRow): boolean {
  const path = row.routePath?.trim()
  if (!path) {
    return false
  }
  return permissionStore.canAccess(path)
}

/**
 * 跳转对应检验单明细清单
 * @param row 表格行
 * @returns {void}
 */
function navigateToOrderList(row: QualityInspectionTableRow): void {
  if (!isRowNavigable(row)) {
    return
  }
  void router.push(row.routePath)
}

/**
 * 表格行属性（可导航行显示手型并绑定点击）
 * @param record 行数据
 * @returns 行 HTML 属性
 */
function createCustomRow(record: QualityInspectionTableRow) {
  const navigable = isRowNavigable(record)
  return {
    style: navigable ? { cursor: 'pointer' } : undefined,
    onClick: () => {
      navigateToOrderList(record)
    },
  }
}

/**
 * 将检验统计映射为表格行
 * @param key 行键
 * @param typeLabel 类型文案
 * @param stat 统计
 * @param routePath 明细清单路由
 * @returns {QualityInspectionTableRow} 表格行
 */
function mapStatToRow(
  key: string,
  typeLabel: string,
  stat: QualityInspectionStat,
  routePath: string,
): QualityInspectionTableRow {
  return {
    key,
    typeLabel,
    orderCount: stat.monthOrderCount,
    sampleQuantity: stat.monthSampleQuantity,
    qualifiedQuantity: stat.monthQualifiedQuantity,
    unqualifiedQuantity: stat.monthUnqualifiedQuantity,
    passRatePercent: Number(stat.monthPassRatePercent) || 0,
    defectRatePercent: calcDefectRatePercent(stat.monthUnqualifiedQuantity, stat.monthSampleQuantity),
    routePath,
  }
}

/** 表格列 */
const tableColumns = computed<TableColumnsType>(() => [
  {
    title: t('dashboard.data-board.page.qualityoperation.colType'),
    dataIndex: 'typeLabel',
    key: 'typeLabel',
    width: 72,
  },
  {
    title: t('dashboard.data-board.page.qualityoperation.colOrders'),
    dataIndex: 'orderCount',
    key: 'orderCount',
    align: 'right',
  },
  {
    title: t('dashboard.data-board.page.qualityoperation.colSample'),
    dataIndex: 'sampleQuantity',
    key: 'sampleQuantity',
    align: 'right',
  },
  {
    title: t('dashboard.data-board.page.qualityoperation.colQualified'),
    dataIndex: 'qualifiedQuantity',
    key: 'qualifiedQuantity',
    align: 'right',
  },
  {
    title: t('dashboard.data-board.page.qualityoperation.colUnqualified'),
    dataIndex: 'unqualifiedQuantity',
    key: 'unqualifiedQuantity',
    align: 'right',
  },
  {
    title: t('dashboard.data-board.page.qualityoperation.colPassRate'),
    dataIndex: 'passRatePercent',
    key: 'passRatePercent',
    align: 'right',
    customRender: ({ text }: { text: number }) => `${Number(text).toFixed(1)}%`,
  },
  {
    title: t('dashboard.data-board.page.qualityoperation.colDefectRate'),
    dataIndex: 'defectRatePercent',
    key: 'defectRatePercent',
    align: 'right',
    customRender: ({ text }: { text: number }) => `${Number(text).toFixed(1)}%`,
  },
])

/** 表格数据：IQC / IPQC / FQC */
const tableRows = computed(() => [
  mapStatToRow(
    'iqc',
    t('dashboard.data-board.page.qualityoperation.typeIqc'),
    iqcStat.value,
    DASHBOARD_STATS_ROUTE.iqcOrder,
  ),
  mapStatToRow(
    'ipqc',
    t('dashboard.data-board.page.qualityoperation.typeIpqc'),
    ipqcStat.value,
    DASHBOARD_STATS_ROUTE.ipqcOrder,
  ),
  mapStatToRow(
    'fqc',
    t('dashboard.data-board.page.qualityoperation.typeFqc'),
    fqcStat.value,
    DASHBOARD_STATS_ROUTE.fqcOrder,
  ),
])

/**
 * 加载检验统计
 * @param path API 路径
 * @returns {Promise<QualityInspectionStat>} 统计
 */
async function loadInspectionStat(path: string): Promise<QualityInspectionStat> {
  const range = getDashboardInspectionMonthRange(1)
  const data = await fetchDashboardGet<QualityInspectionStat>(path, {
    inspectionDateStart: range.start,
    inspectionDateEnd: range.end,
  })
  return data ?? { ...EMPTY_INSPECTION_STAT }
}

/**
 * 加载 IQC / IPQC / FQC 上月统计
 * @returns {Promise<void>}
 */
async function loadData(): Promise<void> {
  const canIqc = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.iqcOrderList)
  const canIpqc = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.ipqcOrderList)
  const canFqc = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.fqcOrderList)
  loading.value = true
  try {
    const [iqc, ipqc, fqc] = await Promise.all([
      fetchDashboardMetricIfPermitted(
        canIqc,
        'iqcOrderStat',
        () => loadInspectionStat(DASHBOARD_STATS_API.iqcOrderStat),
        { ...EMPTY_INSPECTION_STAT },
      ),
      fetchDashboardMetricIfPermitted(
        canIpqc,
        'ipqcOrderStat',
        () => loadInspectionStat(DASHBOARD_STATS_API.ipqcOrderStat),
        { ...EMPTY_INSPECTION_STAT },
      ),
      fetchDashboardMetricIfPermitted(
        canFqc,
        'fqcOrderStat',
        () => loadInspectionStat(DASHBOARD_STATS_API.fqcOrderStat),
        { ...EMPTY_INSPECTION_STAT },
      ),
    ])
    iqcStat.value = iqc
    ipqcStat.value = ipqc
    fqcStat.value = fqc
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

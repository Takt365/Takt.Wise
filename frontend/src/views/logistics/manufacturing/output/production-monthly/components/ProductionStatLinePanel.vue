<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/production-monthly/components -->
<!-- 文件名称：ProductionStatLinePanel.vue -->
<!-- 功能描述：生产统计明细表（组立日报行、PCBA 明细行） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 min-w-0 flex-1 flex-col overflow-hidden">
    <div
      ref="tableWrapRef"
      class="min-h-0 min-w-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        analysis-table
        class="h-full min-h-0"
        entity-scope="company"
        table-mode="single"
        scroll-layout="pageWithInnerTabs"
        :columns="columns"
        :visible-column-keys="visibleColumnKeys"
        :data-source="rows"
        :loading="loading"
        :stripe="true"
        :row-key="getRowKey"
        :pagination="false"
        :show-row-selection="false"
        :scroll="{ y: tableScrollY }"
      >
        <template #bodyCell="{ column, record, text }">
          <template v-if="column.key === 'prodDate'">
            {{ formatDate((record as ProductionStatLine).prodDate) }}
          </template>
          <template v-else-if="column.key === 'achievementRate'">
            {{ formatRate((record as ProductionStatLine).achievementRate) }}
          </template>
          <template v-else-if="column.key === 'completedStatus'">
            <TaktDictTag
              :value="(record as ProductionStatLine).completedStatus"
              dict-type="logistics_manufacturing_pcba_completed_status"
            />
          </template>
          <template v-else>
            {{ text }}
          </template>
        </template>
      </TaktSingleTable>
    </div>
    <TaktPagination
      v-if="total > 0"
      class="mt-2 shrink-0"
      :current="pageIndex"
      :page-size="pageSize"
      :total="total"
      @change="handlePageChange"
    />
    <TaktColumnDrawer
      v-model:open="columnSettingOpen"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 生产统计明细：四个页签各自的列，数据为日报行而不是月份汇总
 */
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import {
  exportProductionStatLine,
  getProductionStatLineList,
} from '@/api/logistics/manufacturing/output/productionMonthly.api'
import type { ProductionStatLine } from '@/types/logistics/manufacturing/output/productionStatLine.types'
import { useTaktTableViewportScrollY } from '@/composables/useTaktTableViewportScrollY'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { taktTrendExportFileNames } from '@/utils/naming'
import {
  ensureTaktPaginationConfigAsync,
  getTaktDefaultPageIndex,
  getTaktDefaultPageSize,
} from '@/utils/taktPaged'
import { measureConstrainedTableScrollY, TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/tableScroll'

const props = defineProps<{
  /** 页签 */
  statTab?: string
  /** 工厂 */
  plantCode?: string
  /** 期间 */
  periodRange?: [string, string] | null
  /** 班组 */
  teamCode?: string
  /** 机种 */
  modelCode?: string
  /** 物料 */
  materialCode?: string
  /** 生产类别 */
  prodCategory?: string
  /** 当前页签已保存的可见列。空表示全部列 */
  savedVisibleKeys?: string[]
}>()

const emit = defineEmits<{
  'update:savedVisibleKeys': [keys: string[]]
}>()

const loading = defineModel<boolean>('loading', { default: false })
const hasRows = defineModel<boolean>('hasRows', { default: false })

/** 静态文案前缀 */
const localePrefix = 'logistics.manufacturing.output.production-monthly.page'
const { t } = useI18n()
/** 行 */
const rows = ref<ProductionStatLine[]>([])
/** 总数 */
const total = ref(0)
/** 页码 */
const pageIndex = ref(1)
/** 页大小 */
const pageSize = ref(20)
/** 表格容器 */
const tableWrapRef = ref<HTMLElement | null>(null)
/** 页签页标准表体高度（视口减去内嵌页签） */
const viewportScrollYPx = useTaktTableViewportScrollY('pageWithInnerTabs')
/** 外壳内实测表体高度（已扣表头与横向滚动条） */
const fittedScrollY = ref(0)
/**
 * 表体高度：不超过页签标准高度，并且落在表格外壳内，横向滚动条才不会被裁掉
 */
const tableScrollY = computed(() => {
  const cap = viewportScrollYPx.value
  if (fittedScrollY.value > 0) {
    return Math.max(TAKT_TABLE_SCROLL_Y_MIN, Math.min(cap, fittedScrollY.value))
  }
  return cap
})
/** 高度观察 */
let tableResizeObserver: ResizeObserver | null = null

/** 组立页签 */
const isAssy = computed(() => props.statTab === 'assy-team' || props.statTab === 'assy-model')
/** 按班组 */
const groupByTeam = computed(() => props.statTab === 'assy-team' || props.statTab === 'pcba-team')

/** 列 */
const columns = computed<TableColumnsType>(() => {
  const col = (key: string, titleKey: string, width: number) => ({
    title: t(`${localePrefix}.columns.${titleKey}`),
    dataIndex: key,
    key,
    width,
    ellipsis: true,
  })
  if (!isAssy.value) {
    const list = []
    if (groupByTeam.value) {
      list.push(col('teamCode', 'pcbaTeamCode', 90))
    }
    list.push(
      col('prodDate', 'prodDate', 110),
      col('batchCode', 'batchCode', 140),
      col('lotQty', 'lotQty', 100),
      col('modelCode', 'modelCode', 140),
      col('pcbBoardType', 'pcbBoardType', 110),
      col('panelSide', 'panelSide', 80),
      col('actualQty', 'pcbaActualQty', 100),
      col('cumulativeQty', 'cumulativeQty', 100),
      col('completedStatus', 'completedStatus', 110),
      col('defectQty', groupByTeam.value ? 'defectUnits' : 'defectQty', 100),
    )
    return list
  }
  if (groupByTeam.value) {
    return [
      col('teamCode', 'teamCode', 100),
      col('prodDate', 'prodDate', 110),
      col('batchCode', 'batchCode', 140),
      col('materialCode', 'materialCode', 130),
      col('modelCode', 'modelCode', 140),
      col('stdMinutes', 'stdMinutes', 80),
      col('planQty', 'planQty', 100),
      col('directLabor', 'directLabor', 70),
      col('indirectLabor', 'indirectLabor', 70),
      col('laborMinutes', 'laborMinutes', 90),
      col('actualQty', 'actualQty', 100),
      col('actualStdMinutes', 'actualStdMinutes', 90),
      col('stdDiff', 'stdDiff', 90),
      col('qtyDiff', 'qtyDiff', 100),
      col('achievementRate', 'achievementRate', 90),
    ]
  }
  return [
    col('prodDate', 'prodDate', 110),
    col('modelCode', 'modelCodeOnly', 140),
    col('stdMinutes', 'stdMinutes', 80),
    col('planQty', 'planQty', 100),
    col('laborMinutes', 'laborMinutesFull', 100),
    col('actualQty', 'actualQty', 100),
    col('directLabor', 'directLaborCount', 90),
    col('actualStdMinutes', 'actualStdMinutes', 90),
    col('stdDiff', 'stdDiff', 90),
    col('qtyDiff', 'qtyDiff', 100),
    col('achievementRate', 'achievementRate', 90),
  ]
})

/** 列设置抽屉 */
const columnSettingOpen = defineModel<boolean>('columnSettingOpen', { default: false })
/** 当前页签可见列。有保存结果时原样使用，避免勾选被滤掉后又弹回 */
const visibleColumnKeys = computed(() => {
  if (props.savedVisibleKeys?.length) {
    return props.savedVisibleKeys
  }
  return columns.value.map((col) => String(col.key ?? col.dataIndex))
})

/**
 * 列设置保存
 * @param {string[]} keys 可见列
 */
function handleColumnKeysChange(keys: string[]) {
  emit('update:savedVisibleKeys', keys.map((key) => String(key)))
}

/**
 * 行键
 * @param {ProductionStatLine} record 行
 * @returns {string} 键
 */
function getRowKey(record: ProductionStatLine): string {
  return record.rowKey || `${record.prodDate}|${record.teamCode}|${record.modelCode}|${record.batchCode}`
}

/**
 * 日期 yyyy-MM-dd
 * @param {string} value 日期
 * @returns {string} 文本
 */
function formatDate(value: string): string {
  if (!value) {
    return ''
  }
  return String(value).slice(0, 10)
}

/**
 * 达成率
 * @param {number} value 百分比
 * @returns {string} 文本
 */
function formatRate(value: number): string {
  const rate = Number(value)
  if (!Number.isFinite(rate)) {
    return ''
  }
  return `${rate}%`
}

/**
 * 查询参数
 * @returns 参数或 null
 */
function buildQuery() {
  const plant = props.plantCode?.trim()
  if (!plant || !props.periodRange?.[0] || !props.periodRange?.[1]) {
    return null
  }
  return {
    statTab: props.statTab,
    plantCode: plant,
    periodDateStart: `${props.periodRange[0]}-01`,
    periodDateEnd: `${props.periodRange[1]}-01`,
    teamCode: props.teamCode?.trim() || undefined,
    modelCode: props.modelCode?.trim() || undefined,
    materialCode: props.materialCode?.trim() || undefined,
    prodCategory: props.prodCategory?.trim() || undefined,
    pageIndex: pageIndex.value,
    pageSize: pageSize.value,
  }
}

/** 清空 */
function clear() {
  rows.value = []
  total.value = 0
  hasRows.value = false
}

/** 重新查询，回到第一页 */
async function reload() {
  pageIndex.value = getTaktDefaultPageIndex()
  await load()
}

/** 按当前页查询 */
async function load() {
  const query = buildQuery()
  if (!query) {
    return
  }
  loading.value = true
  try {
    const result = await getProductionStatLineList(query)
    rows.value = result?.paged?.data ?? []
    total.value = result?.paged?.total ?? result?.rowCount ?? 0
    hasRows.value = rows.value.length > 0
  } catch (err: unknown) {
    rows.value = []
    total.value = 0
    hasRows.value = false
    const messageText = err instanceof Error ? err.message : ''
    message.error(messageText || t(`${localePrefix}.exportFailed`))
  } finally {
    loading.value = false
  }
}

/**
 * 翻页
 * @param {number} page 页码
 * @param {number} size 页大小
 */
function handlePageChange(page: number, size: number) {
  pageIndex.value = page
  pageSize.value = size
  void load()
}

/** 导出 */
async function handleExport() {
  const query = buildQuery()
  if (!query || rows.value.length === 0) {
    message.warning(t(`${localePrefix}.exportEmpty`))
    return
  }
  const { sheetName, exportName, fallbackBase } = taktTrendExportFileNames(
    query.plantCode,
    t(`${localePrefix}.title`),
  )
  const exportMeta = await exportProductionStatLine(
    { ...query, pageIndex: 1, pageSize: 1 },
    sheetName,
    exportName,
  )
  const blob = (exportMeta as { blob?: Blob }).blob ?? (exportMeta as unknown as Blob)
  const fileName = resolveExportDownloadFileName({
    contentDisposition: (exportMeta as { contentDisposition?: string | null }).contentDisposition ?? null,
    contentType: (exportMeta as { contentType?: string | null }).contentType ?? null,
    fallbackBase,
  })
  const url = window.URL.createObjectURL(blob)
  const link = document.createElement('a')
  link.href = url
  link.download = fileName
  link.click()
  window.URL.revokeObjectURL(url)
  message.success(t(`${localePrefix}.exportSuccess`))
}

defineExpose({ reload, handleExport, clear })

watch(rows, () => {
  void nextTick(() => measureTableScrollY())
})

/** 按外壳重算表体高度 */
function measureTableScrollY() {
  fittedScrollY.value = measureConstrainedTableScrollY(tableWrapRef.value)
}

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  pageIndex.value = getTaktDefaultPageIndex()
  pageSize.value = getTaktDefaultPageSize()
  await nextTick()
  measureTableScrollY()
  const wrap = tableWrapRef.value
  if (!wrap) {
    return
  }
  tableResizeObserver = new ResizeObserver(() => measureTableScrollY())
  tableResizeObserver.observe(wrap)
})

onBeforeUnmount(() => {
  tableResizeObserver?.disconnect()
  tableResizeObserver = null
})
</script>

<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/quality-stat -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：质量统计（进货按供应商、过程按工序、完成品按客户） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 min-w-0 flex-col p-4">
    <a-tabs
      v-model:activeKey="statTab"
      class="quality-stat-tabs flex min-h-0 min-w-0 flex-1 flex-col overflow-hidden [&_.ant-tabs-content]:h-full [&_.ant-tabs-content]:w-full [&_.ant-tabs-content]:min-w-0 [&_.ant-tabs-content-holder]:flex [&_.ant-tabs-content-holder]:w-full [&_.ant-tabs-content-holder]:min-h-0 [&_.ant-tabs-content-holder]:min-w-0 [&_.ant-tabs-content-holder]:flex-1 [&_.ant-tabs-content-holder]:flex-col [&_.ant-tabs-tabpane]:flex [&_.ant-tabs-tabpane]:h-full [&_.ant-tabs-tabpane]:w-full [&_.ant-tabs-tabpane]:min-h-0 [&_.ant-tabs-tabpane]:min-w-0 [&_.ant-tabs-tabpane]:flex-col"
      @change="handleStatTabChange"
    >
      <a-tab-pane
        v-for="tab in statTabs"
        :key="tab.key"
        :tab="t(`${localePrefix}.tabs.${tab.label}`)"
        class="h-full min-h-0 min-w-0"
      >
        <div
          v-if="statTab === tab.key"
          class="flex h-full min-h-0 min-w-0 flex-col"
        >
    <div class="takt-query-bar quality-stat-query-bar shrink-0">
      <div class="flex min-w-0 flex-1 flex-nowrap items-center gap-x-2">
        <div class="flex min-w-0 flex-1 items-center gap-1">
          <span class="shrink-0 whitespace-nowrap text-sm text-text-secondary">{{ t(`${localePrefix}.periodRange`) }}</span>
          <a-range-picker
            v-model:value="periodRange"
            picker="month"
            format="YYYY-MM"
            value-format="YYYY-MM"
            class="min-w-0 w-full"
            :placeholder="[t(`${localePrefix}.periodRange`), t(`${localePrefix}.periodRange`)]"
            :disabled-date="disabledDate"
            @calendar-change="onCalendarChange"
            @open-change="onOpenChange"
          />
        </div>
        <div class="flex min-w-0 flex-1 items-center gap-1">
          <span class="shrink-0 whitespace-nowrap text-sm text-text-secondary">{{ dimensionLabel }}</span>
          <TaktSelect
            :key="`${statTab}-dimension`"
            v-model:value="dimensionCode"
            :api-url="dimensionOptionsUrl"
            :api-params="scopedApiParams"
            :disabled="!plantCode?.trim()"
            class="min-w-0 w-full"
            allow-clear
            show-search
            :placeholder="t('common.page.form.placeholder.selectonly')"
          />
        </div>
      </div>
      <a-space class="query-actions shrink-0">
        <a-button
          class="takt-button-query"
          :loading="loading"
          @click="handleSearch"
        >
          <template #icon>
            <RiSearchLine class="takt-remix-icon" />
          </template>
          {{ t('common.page.button.query') }}
        </a-button>
        <a-button
          class="takt-button-reset"
          :disabled="loading"
          @click="handleReset"
        >
          <template #icon>
            <RiRefreshLine class="takt-remix-icon" />
          </template>
          {{ t('common.page.button.reset') }}
        </a-button>
      </a-space>
    </div>
    <TaktToolsBar
      :show-create="false"
      :show-update="false"
      :show-delete="false"
      :show-import="false"
      :show-export="true"
      :show-expand="false"
      :show-advanced-query="false"
      :show-column-setting="false"
      :show-fullscreen="false"
      :show-refresh="true"
      :export-disabled="!plantCode?.trim() || rows.length === 0"
      :export-loading="exportLoading"
      :refresh-loading="loading"
      export-permission="logistics:quality:operation:stat:export"
      @export="handleExport"
      @refresh="loadData"
    />
    <div
      ref="tableWrapRef"
      class="min-h-0 min-w-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        analysis-table
        class="h-full min-h-0 min-w-0"
        entity-scope="company"
        table-mode="single"
        :columns="columns"
        :data-source="rows"
        :loading="loading"
        :stripe="true"
        :row-key="getRowKey"
        :pagination="false"
        :show-row-selection="false"
        :scroll="{ x: 'max-content', y: tableScrollY }"
      />
    </div>
    <TaktPagination
      v-if="total > 0"
      class="mt-2 shrink-0"
      :current="pageIndex"
      :page-size="pageSize"
      :total="total"
      @change="handlePageChange"
    />
        </div>
      </a-tab-pane>
    </a-tabs>
  </div>
</template>

<script setup lang="ts">
/**
 * 质量统计：进货检验按供应商、制程检验按工序、出货检验按客户汇总
 */
import { message } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { RiRefreshLine, RiSearchLine } from '@remixicon/vue'
import {
  exportQualityStat,
  getQualityStatDimensionOptionsUrl,
  getQualityStatList,
  getQualityStatPlantOptionsUrl,
} from '@/api/logistics/quality/operation/qualityStat.api'
import { useAnalysisPeriodRangeLimit } from '@/composables/useAnalysisPeriodRangeLimit'
import { useAnalysisQueryDefaults } from '@/composables/useAnalysisQueryDefaults'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import type { QualityStatRow } from '@/types/logistics/quality/operation/qualityStat.types'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { taktTrendExportFileNames } from '@/utils/naming'
import {
  ensureTaktPaginationConfigAsync,
  getTaktDefaultPageIndex,
  getTaktDefaultPageSize,
} from '@/utils/taktPaged'
import { measureConstrainedTableScrollY, TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/tableScroll'

const { t } = useI18n()
/** 期间最多 12 个月 */
const { disabledDate, onCalendarChange, onOpenChange, assertPeriodRangeOk } = useAnalysisPeriodRangeLimit()
/** 静态文案前缀 */
const localePrefix = 'logistics.quality.operation.quality-stat.page'
const tenantStore = useTenantStore()

/** 工厂 */
const plantCode = ref<string | undefined>()
/** 期间 */
const periodRange = ref<[string, string] | null>(null)
const { applyDefaultPlantFromCompany, applyAnalysisQueryDefaults } = useAnalysisQueryDefaults(plantCode, periodRange)
/** 页签（每个页签自带查询、工具栏和表格） */
const statTabs = [
  { key: 'iqc', label: 'iqc' },
  { key: 'ipqc', label: 'ipqc' },
  { key: 'fqc', label: 'fqc' },
] as const
/** 当前页签 */
const statTab = ref<string>(statTabs[0].key)
/** 供应商 / 工序 / 客户 */
const dimensionCode = ref<string | undefined>()
/** 列表 loading */
const loading = ref(false)
/** 导出 loading */
const exportLoading = ref(false)
/** 当前页行 */
const rows = ref<QualityStatRow[]>([])
/** 总行数 */
const total = ref(0)
/** 页码 */
const pageIndex = ref(1)
/** 页大小 */
const pageSize = ref(20)
/** 表格容器。写在 v-for 里时 Vue 会收成数组 */
const tableWrapRef = ref<HTMLElement | HTMLElement[] | null>(null)
/** 表体高度观察 */
let tableResizeObserver: ResizeObserver | null = null
/** 表体高度 */
const tableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
/** 工厂选项 */
const plantOptionsUrl = getQualityStatPlantOptionsUrl()
/** 维度选项 */
const dimensionOptionsUrl = getQualityStatDimensionOptionsUrl()
/** 工厂选项参数 */
const plantApiParams = computed(() => ({ statTab: statTab.value }))
/** 维度选项参数 */
const scopedApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  return plant ? { statTab: statTab.value, plantCode: plant } : { statTab: statTab.value }
})
/** 维度标题 */
const dimensionLabel = computed(() => t(`${localePrefix}.dimension.${statTab.value}`))
/** 列 */
const columns = computed(() => [
  { title: t(`${localePrefix}.columns.plantCode`), dataIndex: 'plantCode', key: 'plantCode', width: 90 },
  { title: dimensionLabel.value, dataIndex: 'dimensionCode', key: 'dimensionCode', width: 120 },
  { title: t(`${localePrefix}.dimensionName.${statTab.value}`), dataIndex: 'dimensionName', key: 'dimensionName', width: 180 },
  { title: t(`${localePrefix}.columns.orderCount`), dataIndex: 'orderCount', key: 'orderCount', width: 100 },
  { title: t(`${localePrefix}.columns.sampleQty`), dataIndex: 'sampleQty', key: 'sampleQty', width: 110 },
  { title: t(`${localePrefix}.columns.qualifiedQty`), dataIndex: 'qualifiedQty', key: 'qualifiedQty', width: 110 },
  { title: t(`${localePrefix}.columns.unqualifiedQty`), dataIndex: 'unqualifiedQty', key: 'unqualifiedQty', width: 120 },
  { title: t(`${localePrefix}.columns.returnQty`), dataIndex: 'returnQty', key: 'returnQty', width: 110 },
  { title: t(`${localePrefix}.columns.defectRatePercent`), dataIndex: 'defectRatePercent', key: 'defectRatePercent', width: 100 },
  { title: t(`${localePrefix}.columns.qualifiedRatePercent`), dataIndex: 'qualifiedRatePercent', key: 'qualifiedRatePercent', width: 100 },
])

/**
 * 行键
 * @param {QualityStatRow} record 行
 * @returns {string} 键
 */
function getRowKey(record: QualityStatRow): string {
  return `${statTab.value}|${record.plantCode}|${record.dimensionCode}`
}

/** 清空表格 */
function clearTable() {
  rows.value = []
  total.value = 0
}

/**
 * 组装查询
 * @returns 查询参数或 null
 */
function buildQuery() {
  const plant = plantCode.value?.trim()
  if (!plant || !periodRange.value?.[0] || !periodRange.value?.[1]) {
    return null
  }
  return {
    statTab: statTab.value,
    plantCode: plant,
    periodDateStart: `${periodRange.value[0]}-01`,
    periodDateEnd: `${periodRange.value[1]}-01`,
    dimensionCode: dimensionCode.value?.trim() || undefined,
    pageIndex: pageIndex.value,
    pageSize: pageSize.value,
  }
}

/** 查询 */
async function loadData() {
  const query = buildQuery()
  if (!query) {
    return
  }
  loading.value = true
  try {
    const result = await getQualityStatList(query)
    rows.value = result?.paged?.data ?? []
    total.value = result?.paged?.total ?? result?.rowCount ?? 0
  } catch (err: unknown) {
    rows.value = []
    total.value = 0
    const messageText = err instanceof Error ? err.message : ''
    message.error(messageText || t(`${localePrefix}.exportFailed`))
  } finally {
    loading.value = false
  }
}

/** 点击查询 */
function handleSearch() {
  if (!plantCode.value?.trim()) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return
  }
  if (!periodRange.value?.[0] || !periodRange.value?.[1]) {
    message.warning(t(`${localePrefix}.selectPeriodRequired`))
    return
  }
  if (!assertPeriodRangeOk(periodRange.value)) {
    return
  }
  pageIndex.value = getTaktDefaultPageIndex()
  void loadData()
}

/** 切换页签 */
function handleStatTabChange() {
  dimensionCode.value = undefined
  clearTable()
}

/** 重置 */
async function handleReset() {
  dimensionCode.value = undefined
  await applyAnalysisQueryDefaults()
  clearTable()
}

/**
 * 翻页
 * @param {number} page 页码
 * @param {number} size 页大小
 */
function handlePageChange(page: number, size: number) {
  pageIndex.value = page
  pageSize.value = size
  void loadData()
}

/** 导出 */
async function handleExport() {
  const query = buildQuery()
  if (!query) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return
  }
  if (rows.value.length === 0) {
    message.warning(t(`${localePrefix}.exportEmpty`))
    return
  }
  exportLoading.value = true
  try {
    const { sheetName, exportName, fallbackBase } = taktTrendExportFileNames(
      query.plantCode,
      t(`${localePrefix}.title`),
    )
    const exportMeta = await exportQualityStat(
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
  } catch (err: unknown) {
    const messageText = err instanceof Error ? err.message : ''
    message.error(messageText || t(`${localePrefix}.exportFailed`))
  } finally {
    exportLoading.value = false
  }
}

watch(plantCode, (value, previous) => {
  if (value === previous) {
    return
  }
  dimensionCode.value = undefined
  clearTable()
})

watch(
  () => tenantStore.companyCode,
  () => {
    dimensionCode.value = undefined
    clearTable()
    void applyDefaultPlantFromCompany()
  },
)

/**
 * 当前页签里的表格外壳
 * @returns 容器
 */
function activeTableWrap(): HTMLElement | null {
  const value = tableWrapRef.value
  if (Array.isArray(value)) {
    return value.find((item) => item != null) ?? null
  }
  return value
}

/** 按外壳重算表体高度，并留出横向滚动条 */
function measureTableScrollY() {
  const measured = measureConstrainedTableScrollY(activeTableWrap())
  tableScrollY.value = measured > 0 ? Math.max(TAKT_TABLE_SCROLL_Y_MIN, measured) : TAKT_TABLE_SCROLL_Y_MIN
}

/** 重新观察当前页签的表格外壳 */
function bindTableResize() {
  tableResizeObserver?.disconnect()
  tableResizeObserver = null
  const wrap = activeTableWrap()
  if (!wrap) {
    return
  }
  measureTableScrollY()
  tableResizeObserver = new ResizeObserver(() => measureTableScrollY())
  tableResizeObserver.observe(wrap)
}

watch(statTab, () => {
  void nextTick(() => bindTableResize())
})

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  pageIndex.value = getTaktDefaultPageIndex()
  pageSize.value = getTaktDefaultPageSize()
  await applyAnalysisQueryDefaults()
  await nextTick()
  bindTableResize()
})

onBeforeUnmount(() => {
  tableResizeObserver?.disconnect()
  tableResizeObserver = null
})
</script>

<style scoped>
.quality-stat-query-bar {
  display: flex;
  flex-wrap: nowrap;
  align-items: center;
  gap: 8px;
  width: 100%;
  margin: 4px 0;
  box-sizing: border-box;
}

.query-actions :deep(.ant-btn) {
  display: inline-flex;
  align-items: center;
  gap: 4px;
}

.query-actions :deep(.ant-btn .anticon) {
  margin-inline-end: 0 !important;
}
</style>

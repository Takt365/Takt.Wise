<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/changeover-stat -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：切换统计（按期间列出生产切换记录，可导出） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 flex-col p-4">
    <div class="takt-query-bar">
      <div class="flex min-w-0 flex-1 flex-wrap items-center gap-x-3 gap-y-2">
        <div class="flex items-center gap-1">
          <span class="shrink-0 whitespace-nowrap text-sm text-text-secondary">{{ t(`${localePrefix}.periodRange`) }}</span>
          <a-range-picker
            v-model:value="periodRange"
            picker="month"
            format="YYYY-MM"
            value-format="YYYY-MM"
            class="w-56"
            :placeholder="[t(`${localePrefix}.periodRange`), t(`${localePrefix}.periodRange`)]"
            :disabled-date="disabledDate"
            @calendar-change="onCalendarChange"
            @open-change="onOpenChange"
          />
        </div>
        <div class="flex items-center gap-1">
          <span class="shrink-0 whitespace-nowrap text-sm text-text-secondary">{{ t('entity.productionchangeover.prodcategory') }}</span>
          <TaktSelect
            v-model:value="prodCategory"
            dict-type="logistics_manufacturing_prod_category"
            class="w-36"
            allow-clear
            show-search
            :placeholder="t('common.page.form.placeholder.selectonly')"
          />
        </div>
        <div class="flex items-center gap-1">
          <span class="shrink-0 whitespace-nowrap text-sm text-text-secondary">{{ t('entity.productionchangeover.changeovercategory') }}</span>
          <TaktSelect
            v-model:value="changeoverCategory"
            dict-type="logistics_manufacturing_changeover_category"
            class="w-36"
            allow-clear
            show-search
            :placeholder="t('common.page.form.placeholder.selectonly')"
          />
        </div>
        <div class="flex items-center gap-1">
          <span class="shrink-0 whitespace-nowrap text-sm text-text-secondary">{{ t('entity.productionchangeover.teamcode') }}</span>
          <TaktSelect
            :key="plantCode || 'team'"
            v-model:value="teamCode"
            :api-url="teamOptionsUrl"
            :api-params="teamApiParams"
            :disabled="!plantCode?.trim()"
            class="w-36"
            allow-clear
            show-search
            :placeholder="t('common.page.form.placeholder.selectonly')"
          />
        </div>
      </div>
      <a-space class="query-actions">
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
      class="mt-2"
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
      :export-disabled="rows.length === 0"
      :export-loading="exportLoading"
      :refresh-loading="loading"
      export-permission="logistics:manufacturing:output:changeover:stat:export"
      @export="handleExport"
      @refresh="loadData"
    />
    <div
      ref="tableWrapRef"
      class="min-h-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        analysis-table
        class="h-full min-h-0"
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
      >
        <template #bodyCell="{ column, record, text }">
          <template v-if="column.key === 'prodDate'">
            {{ formatDate((record as ChangeoverStatRow).prodDate) }}
          </template>
          <template v-else-if="column.key === 'prodCategory'">
            <TaktDictTag
              :value="(record as ChangeoverStatRow).prodCategory"
              dict-type="logistics_manufacturing_prod_category"
            />
          </template>
          <template v-else-if="column.key === 'changeoverCategory'">
            <TaktDictTag
              :value="(record as ChangeoverStatRow).changeoverCategory"
              dict-type="logistics_manufacturing_changeover_category"
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
  </div>
</template>

<script setup lang="ts">
/**
 * 切换统计：按工厂与期间列出生产切换记录
 */
import { message } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { RiRefreshLine, RiSearchLine } from '@remixicon/vue'
import {
  exportChangeoverStat,
  getChangeoverStatList,
  getChangeoverStatPlantOptionsUrl,
  getChangeoverStatTeamOptionsUrl,
} from '@/api/logistics/manufacturing/output/changeoverStat.api'
import { useAnalysisPeriodRangeLimit } from '@/composables/useAnalysisPeriodRangeLimit'
import { useAnalysisQueryDefaults } from '@/composables/useAnalysisQueryDefaults'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import type { ChangeoverStatRow } from '@/types/logistics/manufacturing/output/changeoverStat.types'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { taktTrendExportFileNames } from '@/utils/naming'
import {
  ensureTaktPaginationConfigAsync,
  getTaktDefaultPageIndex,
  getTaktDefaultPageSize,
} from '@/utils/taktPaged'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/tableScroll'

const { t } = useI18n()
/** 期间最多 12 个月 */
const { disabledDate, onCalendarChange, onOpenChange, assertPeriodRangeOk } = useAnalysisPeriodRangeLimit()
/** 静态文案前缀 */
const localePrefix = 'logistics.manufacturing.output.changeover-stat.page'
const tenantStore = useTenantStore()
/** 工厂 */
const plantCode = ref<string | undefined>()
/** 期间 */
const periodRange = ref<[string, string] | null>(null)
const { applyDefaultPlantFromCompany, applyAnalysisQueryDefaults } = useAnalysisQueryDefaults(plantCode, periodRange)
/** 生产类别 */
const prodCategory = ref<string | undefined>()
/** 切换类别 */
const changeoverCategory = ref<string | undefined>()
/** 班组 */
const teamCode = ref<string | undefined>()
/** 列表 loading */
const loading = ref(false)
/** 导出 loading */
const exportLoading = ref(false)
/** 当前页行 */
const rows = ref<ChangeoverStatRow[]>([])
/** 总行数 */
const total = ref(0)
/** 页码 */
const pageIndex = ref(1)
/** 页大小 */
const pageSize = ref(20)
/** 表格容器 */
const tableWrapRef = ref<HTMLElement | null>(null)
/** 表体高度 */
const tableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
/** 高度观察 */
let tableResizeObserver: ResizeObserver | null = null
/** 工厂选项 */
const plantOptionsUrl = getChangeoverStatPlantOptionsUrl()
/** 班组选项 */
const teamOptionsUrl = getChangeoverStatTeamOptionsUrl()
/** 班组选项参数 */
const teamApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  return plant ? { plantCode: plant } : {}
})
/** 列 */
const columns = computed(() => {
  const field = (key: string, entityKey: string, width: number) => ({
    title: t(`entity.productionchangeover.${entityKey}`),
    dataIndex: key,
    key,
    width,
    ellipsis: true,
  })
  return [
    { title: t(`${localePrefix}.columns.plantCode`), dataIndex: 'plantCode', key: 'plantCode', width: 90, ellipsis: true },
    field('prodCategory', 'prodcategory', 100),
    field('changeoverCategory', 'changeovercategory', 100),
    field('prodDate', 'proddate', 110),
    field('teamCode', 'teamcode', 100),
    field('currentProdOrderCode', 'currentprodordercode', 140),
    field('currentModelCode', 'currentmodelcode', 140),
    field('changeoverProdOrderCode', 'changeoverprodordercode', 140),
    field('changeoverModelCode', 'changeovermodelcode', 140),
    field('changeoverCount', 'changeovercount', 90),
    field('changeoverTime', 'changeovertime', 90),
    field('instrumentSetupTime', 'instrumentsetuptime', 100),
    field('totalChangeoverTime', 'totalchangeovertime', 110),
    field('readSopTime', 'readsoptime', 120),
    field('learningTime', 'learningtime', 90),
    field('personCount', 'personcount', 80),
    field('totalLearningTime', 'totallearningtime', 110),
    field('totalSopTime', 'totalsoptime', 110),
  ]
})

/**
 * 行键
 * @param {ChangeoverStatRow} record 行
 * @returns {string} 键
 */
function getRowKey(record: ChangeoverStatRow): string {
  return record.rowKey
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

/** 清空表格 */
function clearTable() {
  rows.value = []
  total.value = 0
}

/** 清空可选条件 */
function clearFilters() {
  prodCategory.value = undefined
  changeoverCategory.value = undefined
  teamCode.value = undefined
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
    plantCode: plant,
    periodDateStart: `${periodRange.value[0]}-01`,
    periodDateEnd: `${periodRange.value[1]}-01`,
    prodCategory: prodCategory.value?.trim() || undefined,
    changeoverCategory: changeoverCategory.value?.trim() || undefined,
    teamCode: teamCode.value?.trim() || undefined,
    pageIndex: pageIndex.value,
    pageSize: pageSize.value,
  }
}

/** 查询当前页 */
async function loadData() {
  const query = buildQuery()
  if (!query) {
    message.warning(t(`${localePrefix}.selectRequired`))
    return
  }
  if (!assertPeriodRangeOk(periodRange.value)) {
    return
  }
  loading.value = true
  try {
    const result = await getChangeoverStatList(query)
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

/** 从第一页查询 */
function handleSearch() {
  pageIndex.value = getTaktDefaultPageIndex()
  void loadData()
}

/** 重置条件并清空表格 */
async function handleReset() {
  clearFilters()
  clearTable()
  await applyAnalysisQueryDefaults()
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

/** 导出当前筛选的全部清单 */
async function handleExport() {
  const query = buildQuery()
  if (!query || rows.value.length === 0) {
    message.warning(t(`${localePrefix}.exportEmpty`))
    return
  }
  exportLoading.value = true
  try {
    const { sheetName, exportName, fallbackBase } = taktTrendExportFileNames(
      query.plantCode,
      t(`${localePrefix}.title`),
    )
    const exportMeta = await exportChangeoverStat(
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
  teamCode.value = undefined
  clearTable()
})

watch(
  () => tenantStore.companyCode,
  () => {
    clearFilters()
    clearTable()
    void applyDefaultPlantFromCompany()
  },
)

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  pageIndex.value = getTaktDefaultPageIndex()
  pageSize.value = getTaktDefaultPageSize()
  await applyAnalysisQueryDefaults()
  const wrap = tableWrapRef.value
  if (!wrap) {
    return
  }
  const measure = () => {
    tableScrollY.value = Math.max(TAKT_TABLE_SCROLL_Y_MIN, wrap.clientHeight - 40)
  }
  measure()
  tableResizeObserver = new ResizeObserver(measure)
  tableResizeObserver.observe(wrap)
})

onBeforeUnmount(() => {
  tableResizeObserver?.disconnect()
  tableResizeObserver = null
})
</script>

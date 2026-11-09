<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-stat -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变统计（各执行部门页签下列出该实施表的字段） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="ec-list-layout flex h-full min-h-0 min-w-0 flex-col p-4">
    <TaktPaneTabs
      v-model:active-key="statTab"
      class="ec-stat-dept-tabs shrink-0"
      hide-content
      :items="statTabItems"
      @change="handleDeptChange"
    />
    <div class="flex min-h-0 min-w-0 flex-1 flex-col overflow-hidden">
      <TaktQueryBar
        v-model="queryKeyword"
        class="shrink-0"
        :loading="loading"
        :placeholder="t('common.page.form.placeholder.search')"
        @search="handleSearch"
        @reset="handleReset"
      >
        <template #leading>
          <a-range-picker
            v-model:value="issuePeriodRange"
            picker="month"
            format="YYYY-MM"
            value-format="YYYY-MM"
            :disabled="periodHasValue(entryPeriodRange)"
            :placeholder="[t(`${localePrefix}.issuePeriodRange`), t(`${localePrefix}.issuePeriodRange`)]"
            :disabled-date="issuePeriodLimit.disabledDate"
            @calendar-change="issuePeriodLimit.onCalendarChange"
            @open-change="issuePeriodLimit.onOpenChange"
          />
          <a-range-picker
            v-model:value="entryPeriodRange"
            picker="month"
            format="YYYY-MM"
            value-format="YYYY-MM"
            :disabled="periodHasValue(issuePeriodRange)"
            :placeholder="[t(`${localePrefix}.entryPeriodRange`), t(`${localePrefix}.entryPeriodRange`)]"
            :disabled-date="entryPeriodLimit.disabledDate"
            @calendar-change="entryPeriodLimit.onCalendarChange"
            @open-change="entryPeriodLimit.onOpenChange"
          />
          <span class="whitespace-nowrap text-sm text-text-secondary">{{ t(`${localePrefix}.ecCode`) }}</span>
          <TaktSelect
            :key="`${statTab}-ec`"
            v-model:value="ecCode"
            :api-url="ecOptionsUrl"
            :api-params="scopedApiParams"
            :disabled="!plantCode?.trim()"
            class="w-48"
            allow-clear
            show-search
            :placeholder="t('common.page.form.placeholder.selectonly')"
          />
        </template>
      </TaktQueryBar>
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
        export-permission="logistics:manufacturing:engineering:change:stat:export"
        @export="handleExport"
        @refresh="loadData"
      />
      <TaktSingleTable
        analysis-table
        class="min-h-0 min-w-0 flex-1"
        entity-scope="company"
        table-mode="single"
        scroll-layout="pageWithInnerTabs"
        :columns="columns"
        :data-source="rows"
        :loading="loading"
        :stripe="true"
        :row-key="getRowKey"
        :pagination="false"
        :show-row-selection="false"
        :scroll="{ x: 'max-content' }"
      />
      <TaktPagination
        v-if="total > 0"
        class="shrink-0"
        :current="pageIndex"
        :page-size="pageSize"
        :total="total"
        @change="handlePageChange"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 设变统计：各执行部门实施表经技术主表关联。表格列取自该部门实施表
 */
import '../ec-list-layout.css'
import { message } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import TaktDictTag from '@/components/common/TaktDictTag/index.vue'
import type { TaktPaneTabItem } from '@/components/business/TaktPaneTabs/index.vue'
import {
  exportEcStat,
  getEcStatEcOptionsUrl,
  getEcStatList,
  getEcStatPlantOptionsUrl,
} from '@/api/logistics/manufacturing/engineering-change/ecStat.api'
import { useAnalysisPeriodRangeLimit } from '@/composables/useAnalysisPeriodRangeLimit'
import { useAnalysisQueryDefaults } from '@/composables/useAnalysisQueryDefaults'
import {
  TAKT_ANALYSIS_PERIOD_MAX_MONTHS,
  buildDefaultThroughCurrentPeriodRange,
} from '@/views/logistics/manufacturing/bom/material-cost/utils/bomMaterialCostPeriod'
import { useEntityFieldI18n } from '@/composables/useEntityFieldI18n'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import type { EcStatRow } from '@/types/logistics/manufacturing/engineering-change/ecStat.types'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { taktTrendExportFileNames } from '@/utils/naming'
import {
  ensureTaktPaginationConfigAsync,
  getTaktDefaultPageIndex,
  getTaktDefaultPageSize,
} from '@/utils/taktPaged'
const { t } = useI18n()
/** 发行期间最多 12 个月，可选到本月 */
const issuePeriodLimit = useAnalysisPeriodRangeLimit(TAKT_ANALYSIS_PERIOD_MAX_MONTHS, { allowCurrentMonth: true })
/** 录入期间最多 12 个月，可选到本月 */
const entryPeriodLimit = useAnalysisPeriodRangeLimit(TAKT_ANALYSIS_PERIOD_MAX_MONTHS, { allowCurrentMonth: true })
/** 静态文案前缀 */
const localePrefix = 'logistics.manufacturing.engineering-change.ec-stat.page'
/** 执行部门页签顺序 */
const deptTabs = ['pur', 'pc', 'iqc', 'mc', 'mtl', 'mfg2', 'mfg1', 'qc', 'me'] as const
/** 部门页签定义（可按 show/permission 删减） */
const statTabItems = computed<TaktPaneTabItem[]>(() =>
  deptTabs.map((dept) => ({
    key: dept,
    label: t(`${localePrefix}.tabs.${dept}`),
  })),
)
const tenantStore = useTenantStore()

/** 工厂 */
const plantCode = ref<string | undefined>()
/** 发行期间 */
const issuePeriodRange = ref<[string, string] | null>(null)
/** 录入期间 */
const entryPeriodRange = ref<[string, string] | null>(null)
const { applyDefaultPlantFromCompany } = useAnalysisQueryDefaults(plantCode, issuePeriodRange)

/**
 * 发行期间默认截止本月，共 3 个月
 */
function applyIssuePeriodThroughCurrent(): void {
  issuePeriodRange.value = buildDefaultThroughCurrentPeriodRange(3)
}

/**
 * 写入默认工厂，并把发行期间收到本月
 * @returns {Promise<void>}
 */
async function applyEcStatQueryDefaults(): Promise<void> {
  applyIssuePeriodThroughCurrent()
  await applyDefaultPlantFromCompany()
}
/** 部门页签 */
const statTab = ref<(typeof deptTabs)[number]>('pur')
/** 设变单号 */
const ecCode = ref<string | undefined>()
/** 关键词 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 导出 loading */
const exportLoading = ref(false)
/** 当前页行 */
const rows = ref<EcStatRow[]>([])
/** 总行数 */
const total = ref(0)
/** 页码 */
const pageIndex = ref(1)
/** 页大小 */
const pageSize = ref(20)
/** 工厂选项 */
const plantOptionsUrl = getEcStatPlantOptionsUrl()
/** 设变选项 */
const ecOptionsUrl = getEcStatEcOptionsUrl()
/** 工厂选项参数 */
const plantApiParams = computed(() => ({ statTab: statTab.value }))
/** 设变选项参数 */
const scopedApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  return plant ? { statTab: statTab.value, plantCode: plant } : { statTab: statTab.value }
})
/** 是否类字段，用 sys_yes_no 显示 */
const yesNoFields = new Set(['isImplemented', 'isSopUpdated', 'ecNewRequiresInspection'])
/** 各部门实施表要显示的列。顺序与该表字段一致，并带上设变单号 */
const deptColumnFields: Record<(typeof deptTabs)[number], readonly string[]> = {
  pur: ['ecCode', 'ecModelCodeGroup', 'ecNewMaterialCodeGroup', 'isImplemented', 'inboundDate'],
  pc: ['ecCode', 'ecModelCodeGroup', 'ecRootMaterialCodeGroup', 'isImplemented', 'scheduledDate', 'scheduledBatch'],
  iqc: ['ecCode', 'ecModelCodeGroup', 'ecRootMaterialCodeGroup', 'ecNewMaterialCodeGroup', 'ecNewRequiresInspection', 'isImplemented', 'inspectionDate'],
  mc: ['ecCode', 'ecModelCodeGroup', 'ecRootMaterialCodeGroup', 'ecNewMaterialCodeGroup', 'ecNewWarehouse', 'isImplemented', 'scheduledDate', 'scheduledBatch'],
  mtl: ['ecCode', 'ecModelCodeGroup', 'ecRootMaterialCodeGroup', 'ecNewMaterialCodeGroup', 'ecNewWarehouse', 'isImplemented', 'scheduledDate', 'scheduledBatch'],
  mfg2: ['ecCode', 'ecModelCodeGroup', 'ecRootMaterialCodeGroup', 'isImplemented', 'productionDate', 'productionBatch'],
  mfg1: ['ecCode', 'ecModelCodeGroup', 'ecRootMaterialCodeGroup', 'isImplemented', 'productionDate', 'productionBatch'],
  qc: ['ecCode', 'ecModelCodeGroup', 'ecRootMaterialCodeGroup', 'isImplemented', 'productionDate', 'productionBatch', 'inspectionDate', 'inspectionBatch'],
  me: ['ecCode', 'ecModelCodeGroup', 'ecRootMaterialCodeGroup', 'isSopUpdated', 'sopDate'],
}
/** 各部门实体字段文案 */
const deptFieldLabel = {
  pur: useEntityFieldI18n('ecpur'),
  pc: useEntityFieldI18n('ecpc'),
  iqc: useEntityFieldI18n('eciqc'),
  mc: useEntityFieldI18n('ecmc'),
  mtl: useEntityFieldI18n('ecmtl'),
  mfg2: useEntityFieldI18n('ecmfg2'),
  mfg1: useEntityFieldI18n('ecmfg1'),
  qc: useEntityFieldI18n('ecqc'),
  me: useEntityFieldI18n('ecme'),
}
/** 列 */
const columns = computed(() => {
  const tab = statTab.value
  const label = deptFieldLabel[tab].label
  return deptColumnFields[tab].map((field) => {
    const column = {
      title: label(field),
      dataIndex: field,
      key: field,
      width: columnWidth(field),
      ellipsis: true,
    }
    if (!yesNoFields.has(field)) {
      return column
    }
    return {
      ...column,
      customRender: ({ record }: { record: EcStatRow }) => {
        const raw = record[field as keyof EcStatRow]
        const value = typeof raw === 'number' || typeof raw === 'string' ? raw : undefined
        return h(TaktDictTag, { dictType: 'sys_yes_no', value })
      },
    }
  })
})

/**
 * 列宽
 * @param {string} field 字段
 * @returns {number} 宽度
 */
function columnWidth(field: string): number {
  if (field.endsWith('Group')) {
    return 220
  }
  if (field.endsWith('Date')) {
    return 150
  }
  if (field === 'isImplemented' || field === 'isSopUpdated' || field === 'ecNewRequiresInspection') {
    return 100
  }
  return 140
}

/**
 * 行键
 * @param {EcStatRow} record 行
 * @returns {string} 键
 */
function getRowKey(record: EcStatRow): string {
  return record.id
}

/** 清空表格 */
function clearTable() {
  rows.value = []
  total.value = 0
}

/**
 * 年月区间是否已开始选择
 * @param range 年月区间
 * @returns {boolean} 任一端有值
 */
function periodHasValue(range: [string, string] | null): boolean {
  return !!(range?.[0] || range?.[1])
}

/**
 * 年月区间是否两端都已选
 * @param range 年月区间
 * @returns {boolean} 已选完整区间
 */
function hasFullPeriod(range: [string, string] | null): boolean {
  return !!(range?.[0] && range?.[1])
}

/**
 * 组装查询。发行期间与录入期间互斥，只提交有值的那一个
 * @returns 查询参数或 null
 */
function buildQuery() {
  const plant = plantCode.value?.trim()
  const issueReady = hasFullPeriod(issuePeriodRange.value)
  const entryReady = hasFullPeriod(entryPeriodRange.value)
  if (!plant || (!issueReady && !entryReady)) {
    return null
  }
  return {
    statTab: statTab.value,
    implStatus: 'all',
    plantCode: plant,
    periodDateStart: issueReady ? `${issuePeriodRange.value![0]}-01` : undefined,
    periodDateEnd: issueReady ? `${issuePeriodRange.value![1]}-01` : undefined,
    entryPeriodDateStart: entryReady ? `${entryPeriodRange.value![0]}-01` : undefined,
    entryPeriodDateEnd: entryReady ? `${entryPeriodRange.value![1]}-01` : undefined,
    ecCode: ecCode.value?.trim() || undefined,
    keyWords: queryKeyword.value.trim() || undefined,
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
    const result = await getEcStatList(query)
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
  const issueReady = hasFullPeriod(issuePeriodRange.value)
  const entryReady = hasFullPeriod(entryPeriodRange.value)
  if (!issueReady && !entryReady) {
    message.warning(t(`${localePrefix}.selectPeriodRequired`))
    return
  }
  if (issueReady && !issuePeriodLimit.assertPeriodRangeOk(issuePeriodRange.value)) {
    return
  }
  if (entryReady && !entryPeriodLimit.assertPeriodRangeOk(entryPeriodRange.value)) {
    return
  }
  pageIndex.value = getTaktDefaultPageIndex()
  void loadData()
}

watch(issuePeriodRange, (value) => {
  if (value?.[0] || value?.[1]) {
    entryPeriodRange.value = null
  }
})

watch(entryPeriodRange, (value) => {
  if (value?.[0] || value?.[1]) {
    issuePeriodRange.value = null
  }
})

/** 切换部门 */
function handleDeptChange() {
  ecCode.value = undefined
  queryKeyword.value = ''
  clearTable()
}

/** 重置 */
async function handleReset() {
  ecCode.value = undefined
  queryKeyword.value = ''
  entryPeriodRange.value = null
  statTab.value = 'pur'
  await applyEcStatQueryDefaults()
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
    const exportMeta = await exportEcStat(
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
  ecCode.value = undefined
  queryKeyword.value = ''
  clearTable()
})

watch(
  () => tenantStore.companyCode,
  () => {
    ecCode.value = undefined
    queryKeyword.value = ''
    clearTable()
    void applyDefaultPlantFromCompany()
  },
)

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  pageIndex.value = getTaktDefaultPageIndex()
  pageSize.value = getTaktDefaultPageSize()
  await applyEcStatQueryDefaults()
})
</script>

<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/defect/defect-stat -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：不良统计（组立按工单、组立按批次、SMT检查、PCBA修理） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 min-w-0 flex-col p-4">
    <a-tabs
      v-model:activeKey="statTab"
      class="defect-stat-tabs flex min-h-0 min-w-0 flex-1 flex-col overflow-hidden [&_.ant-tabs-content]:h-full [&_.ant-tabs-content]:w-full [&_.ant-tabs-content]:min-w-0 [&_.ant-tabs-content-holder]:flex [&_.ant-tabs-content-holder]:w-full [&_.ant-tabs-content-holder]:min-h-0 [&_.ant-tabs-content-holder]:min-w-0 [&_.ant-tabs-content-holder]:flex-1 [&_.ant-tabs-content-holder]:flex-col [&_.ant-tabs-tabpane]:flex [&_.ant-tabs-tabpane]:h-full [&_.ant-tabs-tabpane]:w-full [&_.ant-tabs-tabpane]:min-h-0 [&_.ant-tabs-tabpane]:min-w-0 [&_.ant-tabs-tabpane]:flex-col"
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
    <div class="takt-query-bar defect-stat-query-bar shrink-0">
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
          <span class="shrink-0 whitespace-nowrap text-sm text-text-secondary">{{ t(`${localePrefix}.prodCategory`) }}</span>
          <TaktSelect
            :key="`${statTab}-category`"
            v-model:value="prodCategory"
            dict-type="logistics_manufacturing_prod_category"
            class="min-w-0 w-full"
            allow-clear
            show-search
            :placeholder="t('common.page.form.placeholder.selectonly')"
          />
        </div>
        <div
          v-if="!isBatchTab"
          class="flex min-w-0 flex-1 items-center gap-1"
        >
          <span class="shrink-0 whitespace-nowrap text-sm text-text-secondary">{{ t(`${localePrefix}.prodOrderCode`) }}</span>
          <TaktSelect
            :key="`${statTab}-order`"
            v-model:value="prodOrderCode"
            :api-url="orderOptionsUrl"
            :api-params="scopedApiParams"
            :disabled="!plantCode?.trim()"
            class="min-w-0 w-full"
            allow-clear
            show-search
            :placeholder="t('common.page.form.placeholder.selectonly')"
          />
        </div>
        <div class="flex min-w-0 flex-1 items-center gap-1">
          <span class="shrink-0 whitespace-nowrap text-sm text-text-secondary">{{ t(`${localePrefix}.batchCode`) }}</span>
          <TaktSelect
            :key="`${statTab}-batch`"
            v-model:value="batchCode"
            :api-url="batchOptionsUrl"
            :api-params="scopedApiParams"
            :disabled="!plantCode?.trim()"
            class="min-w-0 w-full"
            allow-clear
            show-search
            :placeholder="t('common.page.form.placeholder.selectonly')"
          />
        </div>
        <div class="flex min-w-0 flex-1 items-center gap-1">
          <span class="shrink-0 whitespace-nowrap text-sm text-text-secondary">{{ t(`${localePrefix}.modelCode`) }}</span>
          <TaktSelect
            :key="`${statTab}-model`"
            v-model:value="modelCode"
            :api-url="modelOptionsUrl"
            :api-params="scopedApiParams"
            :disabled="!plantCode?.trim()"
            class="min-w-0 w-full"
            allow-clear
            show-search
            :placeholder="t('common.page.form.placeholder.selectonly')"
          />
        </div>
        <div class="flex min-w-0 flex-1 items-center gap-1">
          <span class="shrink-0 whitespace-nowrap text-sm text-text-secondary">{{ t(`${localePrefix}.materialCode`) }}</span>
          <TaktSelect
            :key="`${statTab}-material`"
            v-model:value="materialCode"
            :api-url="materialOptionsUrl"
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
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="false"
      :show-refresh="true"
      :export-disabled="!plantCode?.trim() || rows.length === 0"
      :export-loading="exportLoading"
      :refresh-loading="loading"
      export-permission="logistics:manufacturing:defect:stat:export"
      @export="handleExport"
      @advanced-query="advancedQueryVisible = true"
      @column-setting="columnSettingVisible = true"
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
        :visible-column-keys="visibleColumnKeys"
        :data-source="rows"
        :loading="loading"
        :stripe="true"
        :row-key="getRowKey"
        :pagination="false"
        :show-row-selection="false"
        scroll-layout="pageWithInnerTabs"
        :scroll="{ y: tableScrollY }"
      >
        <template #bodyCell="{ column, record, text }">
          <template v-if="dictTypeOf(column.key)">
            <TaktDictTag
              :value="(record as DefectOrderStatRow)[column.key as keyof DefectOrderStatRow] as string | number"
              :dict-type="dictTypeOf(column.key)"
            />
          </template>
          <template v-else-if="dateKeys.has(String(column.key))">
            {{ formatDate(text) }}
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
      </a-tab-pane>
    </a-tabs>
    <TaktQueryDrawer
      :key="statTab"
      v-model:open="advancedQueryVisible"
      :fields="queryFieldsMeta"
      :default-visible-count="7"
      :storage-key="`takt-query-fields-defect-stat-${statTab}`"
      :submit-loading="loading"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
        <a-form-item
          v-show="isFieldVisible('periodRange')"
          :label="t(`${localePrefix}.periodRange`)"
        >
          <a-range-picker
            v-model:value="periodRange"
            picker="month"
            format="YYYY-MM"
            value-format="YYYY-MM"
            class="w-full"
            :disabled-date="disabledDate"
            @calendar-change="onCalendarChange"
            @open-change="onOpenChange"
          />
        </a-form-item>
        <a-form-item
          v-show="isFieldVisible('prodCategory')"
          :label="t(`${localePrefix}.prodCategory`)"
        >
          <TaktSelect
            v-model:value="prodCategory"
            dict-type="logistics_manufacturing_prod_category"
            allow-clear
            show-search
            :placeholder="t('common.page.form.placeholder.selectonly')"
          />
        </a-form-item>
        <a-form-item
          v-if="!isBatchTab"
          v-show="isFieldVisible('prodOrderCode')"
          :label="t(`${localePrefix}.prodOrderCode`)"
        >
          <TaktSelect
            v-model:value="prodOrderCode"
            :api-url="orderOptionsUrl"
            :api-params="scopedApiParams"
            :disabled="!plantCode?.trim()"
            allow-clear
            show-search
            :placeholder="t('common.page.form.placeholder.selectonly')"
          />
        </a-form-item>
        <a-form-item
          v-show="isFieldVisible('batchCode')"
          :label="t(`${localePrefix}.batchCode`)"
        >
          <TaktSelect
            v-model:value="batchCode"
            :api-url="batchOptionsUrl"
            :api-params="scopedApiParams"
            :disabled="!plantCode?.trim()"
            allow-clear
            show-search
            :placeholder="t('common.page.form.placeholder.selectonly')"
          />
        </a-form-item>
        <a-form-item
          v-show="isFieldVisible('modelCode')"
          :label="t(`${localePrefix}.modelCode`)"
        >
          <TaktSelect
            v-model:value="modelCode"
            :api-url="modelOptionsUrl"
            :api-params="scopedApiParams"
            :disabled="!plantCode?.trim()"
            allow-clear
            show-search
            :placeholder="t('common.page.form.placeholder.selectonly')"
          />
        </a-form-item>
        <a-form-item
          v-show="isFieldVisible('materialCode')"
          :label="t(`${localePrefix}.materialCode`)"
        >
          <TaktSelect
            v-model:value="materialCode"
            :api-url="materialOptionsUrl"
            :api-params="scopedApiParams"
            :disabled="!plantCode?.trim()"
            allow-clear
            show-search
            :placeholder="t('common.page.form.placeholder.selectonly')"
          />
        </a-form-item>
      </template>
    </TaktQueryDrawer>
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
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
 * 不良统计：组立按工单/批次实体列，SMT 检查与 PCBA 修理按明细列
 */
import { message } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { RiRefreshLine, RiSearchLine } from '@remixicon/vue'
import {
  exportDefectStat,
  getDefectStatBatchOptionsUrl,
  getDefectStatList,
  getDefectStatMaterialOptionsUrl,
  getDefectStatModelOptionsUrl,
  getDefectStatOrderOptionsUrl,
  getDefectStatPlantOptionsUrl,
} from '@/api/logistics/manufacturing/defect/defectOrderStat.api'
import { useAnalysisPeriodRangeLimit } from '@/composables/useAnalysisPeriodRangeLimit'
import { useAnalysisQueryDefaults } from '@/composables/useAnalysisQueryDefaults'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import type { DefectOrderStatRow } from '@/types/logistics/manufacturing/defect/defectOrderStat.types'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { taktTrendExportFileNames } from '@/utils/naming'
import {
  ensureTaktPaginationConfigAsync,
  getTaktDefaultPageIndex,
  getTaktDefaultPageSize,
} from '@/utils/taktPaged'
import { measureConstrainedTableScrollY, TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/tableScroll'
import { useEntityFieldI18n } from '@/composables/useEntityFieldI18n'
import { useTaktTableViewportScrollY } from '@/composables/useTaktTableViewportScrollY'
import { ASSYBATCHDEFECT_LIST_FIELDS } from '@/views/logistics/manufacturing/defect/assy-batch-defect/composables/useAssyBatchDefectI18n'
import { ASSYORDERDEFECT_LIST_FIELDS } from '@/views/logistics/manufacturing/defect/assy-order-defect/composables/useAssyOrderDefectI18n'
import { PCBAINSPECTION_LIST_FIELDS } from '@/views/logistics/manufacturing/defect/pcba-inspection/composables/usePcbaInspectionI18n'
import { PCBAINSPECTIONDETAIL_LIST_FIELDS } from '@/views/logistics/manufacturing/defect/pcba-inspection/composables/usePcbaInspectionDetailI18n'
import { PCBAREPAIR_LIST_FIELDS } from '@/views/logistics/manufacturing/defect/pcba-repair/composables/usePcbaRepairI18n'
import { PCBAREPAIRDETAIL_LIST_FIELDS } from '@/views/logistics/manufacturing/defect/pcba-repair/composables/usePcbaRepairDetailI18n'

const { t } = useI18n()
/** 期间最多 12 个月 */
const { disabledDate, onCalendarChange, onOpenChange, assertPeriodRangeOk } = useAnalysisPeriodRangeLimit()
/** 静态文案前缀 */
const localePrefix = 'logistics.manufacturing.defect.defect-stat.page'
const tenantStore = useTenantStore()

/** 工厂 */
const plantCode = ref<string | undefined>()
/** 期间 */
const periodRange = ref<[string, string] | null>(null)
const { applyDefaultPlantFromCompany, applyAnalysisQueryDefaults } = useAnalysisQueryDefaults(plantCode, periodRange)
/** 页签（每个页签自带查询、工具栏和表格） */
const statTabs = [
  { key: 'assy-order', label: 'assyOrder' },
  { key: 'assy-batch', label: 'assyBatch' },
  { key: 'smt-inspection', label: 'smtInspection' },
  { key: 'pcba-repair', label: 'pcbaRepair' },
] as const
/** 当前页签 */
const statTab = ref<string>(statTabs[0].key)
/** 生产类别 */
const prodCategory = ref<string | undefined>()
/** 工单 */
const prodOrderCode = ref<string | undefined>()
/** 批次 */
const batchCode = ref<string | undefined>()
/** 机种 */
const modelCode = ref<string | undefined>()
/** 物料 */
const materialCode = ref<string | undefined>()
/** 列表 loading */
const loading = ref(false)
/** 导出 loading */
const exportLoading = ref(false)
/** 当前页行 */
const rows = ref<DefectOrderStatRow[]>([])
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
/** 页签页标准表体高度 */
const viewportScrollYPx = useTaktTableViewportScrollY('pageWithInnerTabs')
/** 外壳内实测表体高度 */
const fittedScrollY = ref(0)
/** 表体高度：不超过页签标准，并留出横向滚动条 */
const tableScrollY = computed(() => {
  const cap = viewportScrollYPx.value
  if (fittedScrollY.value > 0) {
    return Math.max(TAKT_TABLE_SCROLL_Y_MIN, Math.min(cap, fittedScrollY.value))
  }
  return cap
})
/** 是否批次页签 */
const isBatchTab = computed(() => statTab.value.endsWith('-batch'))
/** 工厂选项 */
const plantOptionsUrl = getDefectStatPlantOptionsUrl()
/** 工单选项 */
const orderOptionsUrl = getDefectStatOrderOptionsUrl()
/** 批次选项 */
const batchOptionsUrl = getDefectStatBatchOptionsUrl()
/** 机种选项 */
const modelOptionsUrl = getDefectStatModelOptionsUrl()
/** 物料选项 */
const materialOptionsUrl = getDefectStatMaterialOptionsUrl()
/** 工厂选项参数 */
const plantApiParams = computed(() => ({ statTab: statTab.value }))
/** 依赖工厂的选项参数 */
const scopedApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  return plant ? { statTab: statTab.value, plantCode: plant } : { statTab: statTab.value }
})
/** 组立工单字段文案 */
const orderField = useEntityFieldI18n('assyorderdefect')
/** 组立批次字段文案 */
const batchField = useEntityFieldI18n('assybatchdefect')
/** SMT 检查主表字段文案 */
const inspectionField = useEntityFieldI18n('pcbainspection')
/** SMT 检查明细字段文案 */
const inspectionDetailField = useEntityFieldI18n('pcbainspectiondetail')
/** PCBA 修理主表字段文案 */
const repairField = useEntityFieldI18n('pcbarepair')
/** PCBA 修理明细字段文案 */
const repairDetailField = useEntityFieldI18n('pcbarepairdetail')
/** 日期列 */
const dateKeys = new Set(['lastProdDate', 'inspectionDate', 'bSideAssemblyDate', 'tSideAssemblyDate', 'prodDate'])
/** 字典列 */
const dictTypes: Record<string, string> = {
  prodCategory: 'logistics_manufacturing_prod_category',
  prodOrderType: 'logistics_manufacturing_prod_order_type',
  orderStatus: 'logistics_manufacturing_prod_status',
  batchStatus: 'logistics_manufacturing_prod_status',
  pcbaBoardType: 'logistics_manufacturing_pcba_function',
  visualInspectionLine: 'logistics_manufacturing_visual_inspection_line_category',
  aoiLine: 'logistics_manufacturing_aoi_inspection_line_category',
  shiftNo: 'logistics_manufacturing_shift_category',
  inspectionStatus: 'logistics_manufacturing_pcba_inspection_status',
  defectLocation: 'logistics_manufacturing_pcb_location_category',
  defectEngineering: 'logistics_manufacturing_defect_category',
  defectResponsibility: 'logistics_manufacturing_defect_responsibility_category',
  defectNature: 'logistics_manufacturing_defect_nature',
}
/** 主键和外键不进统计表 */
const hiddenStatFields = new Set(['pcbaInspectionId', 'pcbaRepairId', 'isObsolete'])

/**
 * 字典类型
 * @param {unknown} key 列键
 * @returns {string} 字典类型，没有时为空串
 */
function dictTypeOf(key: unknown): string {
  return dictTypes[String(key)] ?? ''
}

/**
 * 日期显示为 yyyy-MM-dd
 * @param {unknown} value 日期
 * @returns {string} 文本
 */
function formatDate(value: unknown): string {
  if (value == null || value === '') {
    return ''
  }
  return String(value).slice(0, 10)
}

/**
 * 一列
 * @param {string} title 标题
 * @param {string} dataIndex 字段
 * @param {number} width 宽度
 * @returns 列
 */
function columnOf(title: string, dataIndex: string, width = 120) {
  const wide = dataIndex.endsWith('Group') || dataIndex === 'content' || dataIndex === 'defectSymptom' || dataIndex === 'defectReason' || dataIndex === 'remark'
  return { title, dataIndex, key: dataIndex, width: wide ? 220 : width, ellipsis: true }
}

/** 列 */
const columns = computed(() => {
  if (statTab.value === 'assy-order') {
    return ASSYORDERDEFECT_LIST_FIELDS.map((field) => columnOf(orderField.label(field), field, field === 'plantCode' ? 90 : 120))
  }
  if (statTab.value === 'assy-batch') {
    return ASSYBATCHDEFECT_LIST_FIELDS.map((field) => columnOf(batchField.label(field), field, field === 'plantCode' ? 90 : 120))
  }
  if (statTab.value === 'smt-inspection') {
    const header = PCBAINSPECTION_LIST_FIELDS.map((field) => columnOf(inspectionField.label(field), field))
    const detail = PCBAINSPECTIONDETAIL_LIST_FIELDS
      .filter((field) => !hiddenStatFields.has(field) && field !== 'prodOrderCode')
      .map((field) => columnOf(inspectionDetailField.label(field), field === 'TeamCode' ? 'teamCode' : field))
    return [columnOf(t('common.page.entity.plantcode'), 'plantCode', 90), ...header, ...detail]
  }
  const header = PCBAREPAIR_LIST_FIELDS.map((field) => columnOf(repairField.label(field), field === 'TeamCode' ? 'teamCode' : field))
  const detail = PCBAREPAIRDETAIL_LIST_FIELDS
    .filter((field) => !hiddenStatFields.has(field) && field !== 'prodOrderCode')
    .map((field) => columnOf(
      repairDetailField.label(field === 'repairOperator' ? 'repairOperatorName' : field),
      field === 'TeamCode' ? 'detailTeamCode' : field === 'repairOperator' ? 'repairOperatorName' : field,
    ))
  return [columnOf(t('common.page.entity.plantcode'), 'plantCode', 90), ...header, ...detail]
})

/** 各页签用户选择的可见列。未设置时用全部业务列 */
const visibleColumnKeysByTab = reactive<Record<string, string[]>>({})
/** 当前页签列键 */
function currentColumnKeys(): string[] {
  return columns.value.map((col) => String(col.key ?? col.dataIndex))
}
/** 当前页签可见列。原样使用抽屉回传的键，避免勾选被滤掉后又弹回 */
const visibleColumnKeys = computed(() => visibleColumnKeysByTab[statTab.value] ?? currentColumnKeys())

watch(statTab, (tab) => {
  if (!visibleColumnKeysByTab[tab]?.length) {
    visibleColumnKeysByTab[tab] = currentColumnKeys()
  }
}, { immediate: true })
/** 高级查询 */
const advancedQueryVisible = ref(false)
/** 列设置 */
const columnSettingVisible = ref(false)
/** 高级查询字段 */
const queryFieldsMeta = computed(() => {
  const fields = [
    { key: 'periodRange', label: t(`${localePrefix}.periodRange`) },
    { key: 'prodCategory', label: t(`${localePrefix}.prodCategory`) },
  ]
  if (!isBatchTab.value) {
    fields.push({ key: 'prodOrderCode', label: t(`${localePrefix}.prodOrderCode`) })
  }
  fields.push(
    { key: 'batchCode', label: t(`${localePrefix}.batchCode`) },
    { key: 'modelCode', label: t(`${localePrefix}.modelCode`) },
    { key: 'materialCode', label: t(`${localePrefix}.materialCode`) },
  )
  return fields
})

/**
 * 列设置保存
 * @param {string[]} keys 可见列
 */
function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeysByTab[statTab.value] = keys.map((key) => String(key))
}

/** 高级查询提交 */
function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  handleSearch()
}

/** 高级查询清空可选条件 */
function handleAdvancedQueryReset() {
  clearFilters()
}

/**
 * 行键
 * @param {DefectOrderStatRow} record 行
 * @returns {string} 键
 */
function getRowKey(record: DefectOrderStatRow): string {
  return record.rowKey || `${statTab.value}|${record.prodCategory}|${record.prodOrderCode}|${record.batchCode}`
}

/** 清空表格 */
function clearTable() {
  rows.value = []
  total.value = 0
}

/** 清空可选条件 */
function clearFilters() {
  prodCategory.value = undefined
  prodOrderCode.value = undefined
  batchCode.value = undefined
  modelCode.value = undefined
  materialCode.value = undefined
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
    prodOrderCode: isBatchTab.value ? undefined : prodOrderCode.value?.trim() || undefined,
    batchCode: batchCode.value?.trim() || undefined,
    modelCode: modelCode.value?.trim() || undefined,
    materialCode: materialCode.value?.trim() || undefined,
    prodCategory: prodCategory.value?.trim() || undefined,
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
    const result = await getDefectStatList(query)
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
  clearFilters()
  clearTable()
}

/** 重置 */
async function handleReset() {
  clearFilters()
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
    const exportMeta = await exportDefectStat(
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
  clearFilters()
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
  await nextTick()
  bindTableResize()
})

watch(statTab, () => {
  void nextTick(() => bindTableResize())
})

/**
 * 取出当前页签的表格外壳
 * @returns 外壳元素
 */
function activeTableWrap(): HTMLElement | null {
  const value = tableWrapRef.value
  if (Array.isArray(value)) {
    return value.find((item) => item != null) ?? null
  }
  return value
}

/** 按外壳重算表体高度，给横向滚动条留位置 */
function measureTableScrollY() {
  fittedScrollY.value = measureConstrainedTableScrollY(activeTableWrap())
}

/** 页签切换后重新观察当前表格外壳 */
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

onBeforeUnmount(() => {
  tableResizeObserver?.disconnect()
  tableResizeObserver = null
})
</script>

<style scoped>
.defect-stat-query-bar {
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

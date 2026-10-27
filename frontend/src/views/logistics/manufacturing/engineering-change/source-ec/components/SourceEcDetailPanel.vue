<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/source-ec/components -->
<!-- 文件名称：SourceEcDetailPanel.vue -->
<!-- 功能描述：设变来源明细只读列表（按主表选中 sourceEcId 分页查询/导出） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="source-ec-detail-panel flex h-full min-h-0 flex-col overflow-hidden">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      export-permission="logistics:manufacturing:engineering:change:source:ec:export"
      :show-create="false"
      :show-update="false"
      :show-delete="false"
      :show-expand="false"
      :show-refresh="true"
      :show-import="false"
      :show-export="true"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :export-disabled="!hasMasterSelection"
      :export-loading="loading"
      :refresh-loading="loading"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
    />
    <div
      ref="detailTableWrapRef"
      class="source-ec-detail-panel__table-wrap min-h-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :virtual="true"
        :row-key="getSourceEcDetailId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="sourceEcDetailId"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        scroll-layout="masterDetailLr"
        table-mode="masterDetailDetail"
        :scroll="{ y: detailTableScrollY }"
        :show-row-selection="true"
        @change="handleTableChange"
        @pagination-change="handleMasterDetailPaginationChange"
        @resize-column="handleResizeColumn"
      >
        <template #summary>
          <a-table-summary fixed>
            <a-table-summary-row>
              <a-table-summary-cell :index="0" />
              <a-table-summary-cell
                v-for="cell in summaryCells"
                :key="cell.key"
                :index="cell.index"
              >
                <span class="text-sm font-medium">{{ cell.text }}</span>
              </a-table-summary-cell>
            </a-table-summary-row>
          </a-table-summary>
        </template>
      </TaktSingleTable>
    </div>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-engineering-change-source-ec-source-ec-detail"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('cultureCode')">
      <a-form-item :label="pi.queryLabel('cultureCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.cultureCode"
          dict-type="sys_culture_code"
          :placeholder="pi.queryPh('cultureCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceEcCode')">
      <a-form-item :label="pi.queryLabel('sourceEcCode')">
        <a-input
          v-model:value="advancedQueryForm.sourceEcCode"
          :placeholder="pi.queryPh('sourceEcCode', 'required')"
          show-count
          :maxlength="6"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="pi.queryLabel('lineNumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="pi.queryPh('lineNumber', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceRootMaterialCode')">
      <a-form-item :label="pi.queryLabel('sourceRootMaterialCode')">
        <a-input
          v-model:value="advancedQueryForm.sourceRootMaterialCode"
          :placeholder="pi.queryPh('sourceRootMaterialCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceParentMaterialCode')">
      <a-form-item :label="pi.queryLabel('sourceParentMaterialCode')">
        <a-input
          v-model:value="advancedQueryForm.sourceParentMaterialCode"
          :placeholder="pi.queryPh('sourceParentMaterialCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceOldMaterialCode')">
      <a-form-item :label="pi.queryLabel('sourceOldMaterialCode')">
        <a-input
          v-model:value="advancedQueryForm.sourceOldMaterialCode"
          :placeholder="pi.queryPh('sourceOldMaterialCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceOldMaterialDescription')">
      <a-form-item :label="pi.queryLabel('sourceOldMaterialDescription')">
        <a-input
          v-model:value="advancedQueryForm.sourceOldMaterialDescription"
          :placeholder="pi.queryPh('sourceOldMaterialDescription', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceOldUsageQuantity')">
      <a-form-item :label="pi.queryLabel('sourceOldUsageQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.sourceOldUsageQuantity"
          :placeholder="pi.queryPh('sourceOldUsageQuantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceOldItemPosition')">
      <a-form-item :label="pi.queryLabel('sourceOldItemPosition')">
        <a-input
          v-model:value="advancedQueryForm.sourceOldItemPosition"
          :placeholder="pi.queryPh('sourceOldItemPosition', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceNewMaterialCode')">
      <a-form-item :label="pi.queryLabel('sourceNewMaterialCode')">
        <a-input
          v-model:value="advancedQueryForm.sourceNewMaterialCode"
          :placeholder="pi.queryPh('sourceNewMaterialCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceNewMaterialDescription')">
      <a-form-item :label="pi.queryLabel('sourceNewMaterialDescription')">
        <a-input
          v-model:value="advancedQueryForm.sourceNewMaterialDescription"
          :placeholder="pi.queryPh('sourceNewMaterialDescription', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceNewUsageQuantity')">
      <a-form-item :label="pi.queryLabel('sourceNewUsageQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.sourceNewUsageQuantity"
          :placeholder="pi.queryPh('sourceNewUsageQuantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceNewItemPosition')">
      <a-form-item :label="pi.queryLabel('sourceNewItemPosition')">
        <a-input
          v-model:value="advancedQueryForm.sourceNewItemPosition"
          :placeholder="pi.queryPh('sourceNewItemPosition', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceBomCode')">
      <a-form-item :label="pi.queryLabel('sourceBomCode')">
        <a-input
          v-model:value="advancedQueryForm.sourceBomCode"
          :placeholder="pi.queryPh('sourceBomCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceCompatibility')">
      <a-form-item :label="pi.queryLabel('sourceCompatibility')">
        <a-input
          v-model:value="advancedQueryForm.sourceCompatibility"
          :placeholder="pi.queryPh('sourceCompatibility', 'optional')"
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('source2ndVendor')">
      <a-form-item :label="pi.queryLabel('source2ndVendor')">
        <TaktSelect
          v-model:value="advancedQueryForm.source2ndVendor"
          dict-type="logistics_manufacturing_ec_2nd_vendor"
          :placeholder="pi.queryPh('source2ndVendor', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceInstruction')">
      <a-form-item :label="pi.queryLabel('sourceInstruction')">
        <TaktSelect
          v-model:value="advancedQueryForm.sourceInstruction"
          dict-type="logistics_manufacturing_ec_source_instruction"
          :placeholder="pi.queryPh('sourceInstruction', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceOldPartDisposition')">
      <a-form-item :label="pi.queryLabel('sourceOldPartDisposition')">
        <TaktSelect
          v-model:value="advancedQueryForm.sourceOldPartDisposition"
          dict-type="logistics_manufacturing_ec_old_part_disposition"
          :placeholder="pi.queryPh('sourceOldPartDisposition', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceBomEffectiveDateStart')">
      <a-form-item :label="pi.queryLabel('sourceBomEffectiveDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.sourceBomEffectiveDateStart"
          :placeholder="pi.queryPh('sourceBomEffectiveDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceBomEffectiveDateEnd')">
      <a-form-item :label="pi.queryLabel('sourceBomEffectiveDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.sourceBomEffectiveDateEnd"
          :placeholder="pi.queryPh('sourceBomEffectiveDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isObsolete')">
      <a-form-item :label="pi.queryLabel('isObsolete')">
        <TaktSelect
          v-model:value="advancedQueryForm.isObsolete"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isObsolete', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="pi.queryLabel('createdAtStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="pi.queryPh('createdAtStart', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="pi.queryLabel('createdAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="pi.queryPh('createdAtEnd', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('extField')">
      <a-form-item
        name="extField"
        class="takt-form-item-ext-field"
        :label-col="{ style: { width: 'auto', maxWidth: 'none', flex: '0 0 auto' } }"
        :wrapper-col="{ style: { flex: '1 1 0', minWidth: 0 } }"
      >
        <template #label>
          <span class="takt-form-ext-field-label">
            <a-tooltip
              :title="t('common.page.entity.extfieldhint')"
              placement="top"
            >
              <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
            </a-tooltip>
            <span>{{ pi.queryLabel('extField') }}</span>
          </span>
        </template>
        <a-textarea
          v-model:value="advancedQueryForm.extField"
          :placeholder="t('common.page.form.placeholder.extfield')"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('remark')">
      <a-form-item :label="pi.queryLabel('remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="pi.queryPh('remark', 'optional')"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      </template>
    </TaktQueryDrawer>
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      id-column-key="sourceEcDetailId"
      entity-scope="company"
      table-mode="masterDetailDetail"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 设变来源明细只读右栏（查询/导出）
 * @module views/logistics/manufacturing/engineering-change/source-ec/components
 */
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { measureMasterDetailLrTableScrollY } from '@/composables/useTaktMasterDetailLrScrollY'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/tableScroll'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import {
  filterMergedColumnsByDefaultVisible,
  filterTableColumnsByVisibleKeys,
  mergeDefaultColumns,
  normalizeUserTableColumns,
} from '@/utils/tableColumns'
import { formatSummaryValue } from '@/components/business/TaktEditableTable/editableTableUtils'
import { RiQuestionLine } from '@remixicon/vue'
import { useSourceEcMasterContext } from '../composables/useSourceEcMasterContext'
import {
  getSourceEcDetailList,
  exportSourceEcDetail,
} from '@/api/logistics/manufacturing/engineering-change/sourceEcDetail.api'
import type { SourceEcDetail, SourceEcDetailQuery } from '@/types/logistics/manufacturing/engineering-change/sourceEcDetail.types'

import {
  useSourceEcDetailI18n,
  SOURCEECDETAIL_DEFAULT_VISIBLE_COLUMN_KEYS,
  SOURCEECDETAIL_SUMMARY_SUM_FIELDS,
  SOURCEECDETAIL_QUERY_STRING_FIELDS,
  SOURCEECDETAIL_QUERY_FIELDS,
} from '../composables/useSourceEcDetailI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useSourceEcDetailI18n()

const { t } = useI18n()
const { selecmedMasterRow } = useSourceEcMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSourceEcDetail')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() }),
)

const loading = ref(false)

/** 子表滚动区容器（扣除查询/工具栏后剩余高度） */
const detailTableWrapRef = ref<HTMLElement | null>(null)
/** 子表 scroll.y（按 __table-wrap 实测，避免沿用主表共享高度导致双滚动条） */
const detailTableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
let detailTableScrollResizeObserver: ResizeObserver | null = null

/** 按子表容器重算 scroll.y（扣除表头 + 汇总行，避免合计被裁切或双滚动条） */
function recalcDetailTableScrollY(): void {
  const wrap = detailTableWrapRef.value
  if (!wrap) {
    return
  }
  detailTableScrollY.value = measureMasterDetailLrTableScrollY(wrap, { reserveSummaryRow: true })
}

/** 监听子表容器尺寸变化 */
function startDetailTableScrollObserve(): void {
  stopDetailTableScrollObserve()
  recalcDetailTableScrollY()
  const wrap = detailTableWrapRef.value
  if (!wrap) {
    return
  }
  detailTableScrollResizeObserver = new ResizeObserver(() => {
    recalcDetailTableScrollY()
  })
  detailTableScrollResizeObserver.observe(wrap)
}

/** 停止监听子表容器尺寸 */
function stopDetailTableScrollObserve(): void {
  detailTableScrollResizeObserver?.disconnect()
  detailTableScrollResizeObserver = null
}
const dataSource = ref<SourceEcDetail[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selecmedRow = ref<SourceEcDetail | null>(null)
const selecmedRows = ref<SourceEcDetail[]>([])
const selecmedRowKeys = ref<(string | number)[]>([])

const advancedQueryVisible = ref(false)
/**
 * 是否存在任一业务查询条件（分页除外）；无参时不请求列表/导出
 * @returns {boolean}
 */
function hasAnyListQueryFilter(): boolean {
  const kw = (queryKeyword.value ?? '').trim()
  if (kw.length > 0) {
    return true
  }
  const form = advancedQueryForm.value
  for (const key of SOURCEECDETAIL_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    return true
  }
  if (form.sourceOldUsageQuantity !== undefined && form.sourceOldUsageQuantity !== null) {
    return true
  }
  if (form.sourceNewUsageQuantity !== undefined && form.sourceNewUsageQuantity !== null) {
    return true
  }
  if (form.isObsolete !== undefined && form.isObsolete !== null) {
    return true
  }
  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(SOURCEECDETAIL_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof SOURCEECDETAIL_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    lineNumber: undefined as number | undefined,
    sourceOldUsageQuantity: undefined as number | undefined,
    sourceNewUsageQuantity: undefined as number | undefined,
    isObsolete: undefined as number | undefined,  }
}
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() =>
  SOURCEECDETAIL_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)

function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
}
const columnSettingVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([...SOURCEECDETAIL_DEFAULT_VISIBLE_COLUMN_KEYS])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = [...SOURCEECDETAIL_DEFAULT_VISIBLE_COLUMN_KEYS]
}

const entityIdName = 'sourceEcDetailId'
const masterSourceEcId = computed((): string => {
  const id = (selecmedMasterRow.value as Record<string, unknown> | null)?.['sourceEcId']
  return id != null ? String(id) : ''
})
const hasMasterSelection = computed(() => masterSourceEcId.value !== '')

function getSourceEcDetailId(record: SourceEcDetail | Record<string, unknown>): string {
  return String((record as SourceEcDetail)?.[entityIdName] ?? '')
}

function getSourceEcDetailField(record: SourceEcDetail | Record<string, unknown>, field: string): unknown {
  return (record as SourceEcDetail)?.[field as keyof SourceEcDetail]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'sourceEcDetailId',
    key: 'sourceEcDetailId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: SourceEcDetail }) =>
      String(getSourceEcDetailField(record, 'sourceEcDetailId') ?? ''),
  },
  {
    title: pi.label('sourceEcId'),
    dataIndex: 'sourceEcId',
    key: 'sourceEcId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SourceEcDetail }) =>
      String(getSourceEcDetailField(record, 'sourceEcId') ?? ''),
  },
  {
    title: pi.label('sourceEcCode'),
    dataIndex: 'sourceEcCode',
    key: 'sourceEcCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SourceEcDetail }) =>
      String(getSourceEcDetailField(record, 'sourceEcCode') ?? ''),
  },
  {
    title: pi.label('lineNumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SourceEcDetail }) =>
      String(getSourceEcDetailField(record, 'lineNumber') ?? ''),
  },
  {
    title: pi.label('sourceRootMaterialCode'),
    dataIndex: 'sourceRootMaterialCode',
    key: 'sourceRootMaterialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SourceEcDetail }) =>
      String(getSourceEcDetailField(record, 'sourceRootMaterialCode') ?? ''),
  },
  {
    title: pi.label('sourceParentMaterialCode'),
    dataIndex: 'sourceParentMaterialCode',
    key: 'sourceParentMaterialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SourceEcDetail }) =>
      String(getSourceEcDetailField(record, 'sourceParentMaterialCode') ?? ''),
  },
  {
    title: pi.label('sourceOldMaterialCode'),
    dataIndex: 'sourceOldMaterialCode',
    key: 'sourceOldMaterialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SourceEcDetail }) =>
      String(getSourceEcDetailField(record, 'sourceOldMaterialCode') ?? ''),
  },
  {
    title: pi.label('sourceOldMaterialDescription'),
    dataIndex: 'sourceOldMaterialDescription',
    key: 'sourceOldMaterialDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SourceEcDetail }) =>
      String(getSourceEcDetailField(record, 'sourceOldMaterialDescription') ?? ''),
  },
  {
    title: pi.label('sourceOldUsageQuantity'),
    dataIndex: 'sourceOldUsageQuantity',
    key: 'sourceOldUsageQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SourceEcDetail }) =>
      String(getSourceEcDetailField(record, 'sourceOldUsageQuantity') ?? ''),
  },
  {
    title: pi.label('sourceOldItemPosition'),
    dataIndex: 'sourceOldItemPosition',
    key: 'sourceOldItemPosition',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SourceEcDetail }) =>
      String(getSourceEcDetailField(record, 'sourceOldItemPosition') ?? ''),
  },
  {
    title: pi.label('sourceNewMaterialCode'),
    dataIndex: 'sourceNewMaterialCode',
    key: 'sourceNewMaterialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SourceEcDetail }) =>
      String(getSourceEcDetailField(record, 'sourceNewMaterialCode') ?? ''),
  },
  {
    title: pi.label('sourceNewMaterialDescription'),
    dataIndex: 'sourceNewMaterialDescription',
    key: 'sourceNewMaterialDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SourceEcDetail }) =>
      String(getSourceEcDetailField(record, 'sourceNewMaterialDescription') ?? ''),
  },
  {
    title: pi.label('sourceNewUsageQuantity'),
    dataIndex: 'sourceNewUsageQuantity',
    key: 'sourceNewUsageQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SourceEcDetail }) =>
      String(getSourceEcDetailField(record, 'sourceNewUsageQuantity') ?? ''),
  },
  {
    title: pi.label('sourceNewItemPosition'),
    dataIndex: 'sourceNewItemPosition',
    key: 'sourceNewItemPosition',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SourceEcDetail }) =>
      String(getSourceEcDetailField(record, 'sourceNewItemPosition') ?? ''),
  },
  {
    title: pi.label('sourceBomCode'),
    dataIndex: 'sourceBomCode',
    key: 'sourceBomCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SourceEcDetail }) =>
      String(getSourceEcDetailField(record, 'sourceBomCode') ?? ''),
  },
  {
    title: pi.label('sourceCompatibility'),
    dataIndex: 'sourceCompatibility',
    key: 'sourceCompatibility',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SourceEcDetail }) =>
      String(getSourceEcDetailField(record, 'sourceCompatibility') ?? ''),
  },
  {
    title: pi.label('source2ndVendor'),
    dataIndex: 'source2ndVendor',
    key: 'source2ndVendor',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SourceEcDetail }) =>
      String(getSourceEcDetailField(record, 'source2ndVendor') ?? ''),
  },
  {
    title: pi.label('sourceInstruction'),
    dataIndex: 'sourceInstruction',
    key: 'sourceInstruction',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SourceEcDetail }) =>
      String(getSourceEcDetailField(record, 'sourceInstruction') ?? ''),
  },
  {
    title: pi.label('sourceOldPartDisposition'),
    dataIndex: 'sourceOldPartDisposition',
    key: 'sourceOldPartDisposition',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SourceEcDetail }) =>
      String(getSourceEcDetailField(record, 'sourceOldPartDisposition') ?? ''),
  },
  {
    title: pi.label('sourceBomEffectiveDate'),
    dataIndex: 'sourceBomEffectiveDate',
    key: 'sourceBomEffectiveDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SourceEcDetail }) =>
      String(getSourceEcDetailField(record, 'sourceBomEffectiveDate') ?? ''),
  },
  {
    title: pi.label('isObsolete'),
    dataIndex: 'isObsolete',
    key: 'isObsolete',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SourceEcDetail }) =>
      String(getSourceEcDetailField(record, 'isObsolete') ?? ''),
  },
])

/** 与 TaktSingleTable 展示列对齐（用于汇总行单元格） */
const resolvedSummaryColumns = computed(() => {
  const userCols = normalizeUserTableColumns(columns.value)
  const merged = mergeDefaultColumns(userCols, t, true, 'company')
  const keys = visibleColumnKeys.value
  if (keys.length > 0) {
    return filterTableColumnsByVisibleKeys(merged, keys, merged)
  }
  return filterMergedColumnsByDefaultVisible(merged, userCols, {
    idColumnKey: 'sourceEcDetailId',
    actionColumnKey: 'action',
    tableMode: 'masterDetailDetail',
    entityScope: 'company',
  })
})

const summarySumFieldSet = new Set<string>(SOURCEECDETAIL_SUMMARY_SUM_FIELDS)

/** 汇总行首列文案 */
const summaryLabel = computed(() => t('components.business.page.editabletable.summarylabel'))

/** 汇总行单元格（index 与 a-table 列序一致：0=行选择，1..n=展示列） */
const summaryCells = computed(() => {
  const cells: Array<{ key: string; text: string; index: number }> = []
  resolvedSummaryColumns.value.forEach((col, columnIndex) => {
    const key = String(col.key ?? columnIndex)
    let text = ''
    if (columnIndex === 0) {
      text = summaryLabel.value
    } else if (isSummarySumField(key)) {
      text = formatSummaryFieldTotal(key)
    }
    cells.push({
      key,
      text,
      index: columnIndex + 1,
    })
  })
  return cells
})

/** 是否参与当前页合计 */
function isSummarySumField(field: string): boolean {
  return summarySumFieldSet.has(field)
}

/** 当前页 dataSource 各合计列求和 */
const summaryFieldTotals = computed(() => {
  const totals = Object.fromEntries(
    SOURCEECDETAIL_SUMMARY_SUM_FIELDS.map((field) => [field, 0]),
  ) as Record<(typeof SOURCEECDETAIL_SUMMARY_SUM_FIELDS)[number], number>
  for (const row of dataSource.value) {
    for (const field of SOURCEECDETAIL_SUMMARY_SUM_FIELDS) {
      const num = Number(getSourceEcDetailField(row, field))
      if (Number.isFinite(num)) {
        totals[field] += num
      }
    }
  }
  return totals
})

/** 格式化合计单元格展示值 */
function formatSummaryFieldTotal(field: string): string {
  if (!isSummarySumField(field)) {
    return ''
  }
  return formatSummaryValue(summaryFieldTotals.value[field as keyof typeof summaryFieldTotals.value])
}
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: SourceEcDetail[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: SourceEcDetail, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (selecmedRow.value && getSourceEcDetailId(selecmedRow.value) === getSourceEcDetailId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: SourceEcDetail[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: SourceEcDetail) {
  const key = getSourceEcDetailId(record)
  return {
    onClick: () => {
      selecmedRowKeys.value = [key]
      selecmedRows.value = [record]
      selecmedRow.value = record
    },
    class: selecmedRowKeys.value.includes(key)
      ? 'takt-master-detail-table-row-selecmed cursor-pointer'
      : 'cursor-pointer',
  }
}

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {SourceEcDetailQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<SourceEcDetailQuery>): SourceEcDetailQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: SourceEcDetailQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    sourceEcId: masterSourceEcId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof SourceEcDetailQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of SOURCEECDETAIL_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  if (form.sourceOldUsageQuantity !== undefined && form.sourceOldUsageQuantity !== null) {
    query.sourceOldUsageQuantity = form.sourceOldUsageQuantity
  }
  if (form.sourceNewUsageQuantity !== undefined && form.sourceNewUsageQuantity !== null) {
    query.sourceNewUsageQuantity = form.sourceNewUsageQuantity
  }
  if (form.isObsolete !== undefined && form.isObsolete !== null) {
    query.isObsolete = form.isObsolete
  }
  return query
}

async function loadData() {
  if (!hasMasterSelection.value) {
    dataSource.value = []
    total.value = 0
    selecmedRowKeys.value = []
    selecmedRows.value = []
    selecmedRow.value = null
    return
  }
  loading.value = true
  try {
    const res = await getSourceEcDetailList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

function reload() {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

/** 主表选中变更时自动加载子表 */
watch(masterSourceEcId, () => {
  reload()
})

/** 租户/公司切换时刷新子表 */
useTableRefresh(loadData)

onMounted(() => {
  startDetailTableScrollObserve()
})

onBeforeUnmount(() => {
  stopDetailTableScrollObserve()
})

watch(
  () => loading.value,
  (isLoading) => {
    if (!isLoading) {
      void nextTick(() => recalcDetailTableScrollY())
    }
  },
)

watch(
  () => [dataSource.value.length, visibleColumnKeys.value.join(',')],
  () => {
    void nextTick(() => recalcDetailTableScrollY())
  },
)

watch(hasMasterSelection, (selecmed) => {
  if (selecmed) {
    void nextTick(() => startDetailTableScrollObserve())
  }
})

function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleQueryReset() {
  queryKeyword.value = ''
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleRefresh() {
  void loadData()
}

async function handleExport() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  try {
    loading.value = true
    if (!hasAnyListQueryFilter()) {
      return
    }
    const exportMeta = await exportSourceEcDetail(
      buildListQuery({ pageIndex: 1, pageSize: 100000 }),
      excelNames.sheet,
      excelNames.fileBase
    )
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = `${excelNames.fileBase}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as { contentDisposition?: string | null }).contentDisposition ?? null,
      contentType: (exportMeta as { contentType?: string | null }).contentType ?? null,
      fallbackBase,
    })
    const blob = (exportMeta as { blob?: Blob }).blob ?? (exportMeta as Blob)
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t('common.feedback.export.success', { target: pi.self() }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
function handleTableChange() {}

function handleResizeColumn() {}

/**
 * 主子表内嵌分页变更
 * @param page 页码
 * @param size 每页条数
 */
function handleMasterDetailPaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  void loadData()
}

defineExpose({ reload, loadData })
</script>

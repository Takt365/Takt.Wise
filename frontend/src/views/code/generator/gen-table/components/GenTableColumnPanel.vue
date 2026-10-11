<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/code/generator/gen-table/components -->
<!-- 文件名称：GenTableColumnPanel.vue -->
<!-- 功能描述：Takt代码生成表配置实体 特例：继承组合 4：无关联工厂、无语言主表实体右侧明细 genTableColumn 独立 CRUD（按主表选中 genTableId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="gen-table-column-panel flex h-full min-h-0 flex-col overflow-hidden">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="code:generator:gen:table:create"
      update-permission="code:generator:gen:table:update"
      delete-permission="code:generator:gen:table:delete"
      import-permission="code:generator:gen:table:import"
      export-permission="code:generator:gen:table:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-expand="false"
      :show-refresh="true"

      :show-import="true"
      :show-export="true"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :import-disabled="!hasMasterSelection"
      :export-disabled="!hasMasterSelection"
      :import-loading="loading"
      :export-loading="loading"
      @import="handleImport"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      :create-disabled="!hasMasterSelection"
      :update-disabled="updateDisabled"
      :delete-disabled="deleteDisabled"
      :create-loading="loading"
      :update-loading="loading"
      :delete-loading="loading"
      :refresh-loading="loading"
      @create="handleCreate"
      @update="handleUpdate"
      @delete="handleDelete"
      @refresh="handleRefresh"
    />
    <div
      ref="detailTableWrapRef"
      class="gen-table-column-panel__table-wrap min-h-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="tenant"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :virtual="true"
        :row-key="getGenTableColumnId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="genTableColumnId"
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
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      :width="formModalWidthPx"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <GenTableColumnForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterGenTableId"
        :master-row="selectedMasterRow"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-code-generator-gen-table-gen-table-column"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="pi.queryLabel('lineNumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="pi.queryPh('lineNumber', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('databaseColumnName')">
      <a-form-item :label="pi.queryLabel('databaseColumnName')">
        <a-input
          v-model:value="advancedQueryForm.databaseColumnName"
          :placeholder="pi.queryPh('databaseColumnName', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('columnComment')">
      <a-form-item :label="pi.queryLabel('columnComment')">
        <a-input
          v-model:value="advancedQueryForm.columnComment"
          :placeholder="pi.queryPh('columnComment', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('databaseDataType')">
      <a-form-item :label="pi.queryLabel('databaseDataType')">
        <TaktSelect
          v-model:value="advancedQueryForm.databaseDataType"
          dict-type="sys_db_data_type"
          :placeholder="pi.queryPh('databaseDataType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('csharpDataType')">
      <a-form-item :label="pi.queryLabel('csharpDataType')">
        <TaktSelect
          v-model:value="advancedQueryForm.csharpDataType"
          dict-type="code_generator_csharp_data_type"
          :placeholder="pi.queryPh('csharpDataType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('csharpColumnName')">
      <a-form-item :label="pi.queryLabel('csharpColumnName')">
        <a-input
          v-model:value="advancedQueryForm.csharpColumnName"
          :placeholder="pi.queryPh('csharpColumnName', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('length')">
      <a-form-item :label="pi.queryLabel('length')">
        <a-input-number
          v-model:value="advancedQueryForm.length"
          :placeholder="pi.queryPh('length', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('decimalDigits')">
      <a-form-item :label="pi.queryLabel('decimalDigits')">
        <a-input-number
          v-model:value="advancedQueryForm.decimalDigits"
          :placeholder="pi.queryPh('decimalDigits', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isPk')">
      <a-form-item :label="pi.queryLabel('isPk')">
        <TaktSelect
          v-model:value="advancedQueryForm.isPk"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isPk', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isIncrement')">
      <a-form-item :label="pi.queryLabel('isIncrement')">
        <TaktSelect
          v-model:value="advancedQueryForm.isIncrement"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isIncrement', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isRequired')">
      <a-form-item :label="pi.queryLabel('isRequired')">
        <TaktSelect
          v-model:value="advancedQueryForm.isRequired"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isRequired', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isCreate')">
      <a-form-item :label="pi.queryLabel('isCreate')">
        <TaktSelect
          v-model:value="advancedQueryForm.isCreate"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isCreate', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isUpdate')">
      <a-form-item :label="pi.queryLabel('isUpdate')">
        <TaktSelect
          v-model:value="advancedQueryForm.isUpdate"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isUpdate', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isUnique')">
      <a-form-item :label="pi.queryLabel('isUnique')">
        <TaktSelect
          v-model:value="advancedQueryForm.isUnique"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isUnique', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isList')">
      <a-form-item :label="pi.queryLabel('isList')">
        <TaktSelect
          v-model:value="advancedQueryForm.isList"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isList', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isExport')">
      <a-form-item :label="pi.queryLabel('isExport')">
        <TaktSelect
          v-model:value="advancedQueryForm.isExport"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isExport', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isSort')">
      <a-form-item :label="pi.queryLabel('isSort')">
        <TaktSelect
          v-model:value="advancedQueryForm.isSort"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isSort', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isQuery')">
      <a-form-item :label="pi.queryLabel('isQuery')">
        <TaktSelect
          v-model:value="advancedQueryForm.isQuery"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isQuery', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('queryType')">
      <a-form-item :label="pi.queryLabel('queryType')">
        <TaktSelect
          v-model:value="advancedQueryForm.queryType"
          dict-type="code_generator_query_type"
          :placeholder="pi.queryPh('queryType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('htmlType')">
      <a-form-item :label="pi.queryLabel('htmlType')">
        <TaktSelect
          v-model:value="advancedQueryForm.htmlType"
          dict-type="code_generator_display_type"
          :placeholder="pi.queryPh('htmlType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('dictType')">
      <a-form-item :label="pi.queryLabel('dictType')">
        <TaktSelect
          v-model:value="advancedQueryForm.dictType"
          api-url="TaktDictTypes/options"
          :placeholder="pi.queryPh('dictType', 'select')"
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
    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: pi.self() })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        :entity-i18n-key="GENTABLECOLUMN_SELF_I18N_KEY"
        file-type="xlsx"
        :sheet-name="excelNames.sheet"
        :template-file-name="excelNames.fileBase"
        :download-template="handleDownloadTemplate"
        :import-file="handleImportFile"
        :max-size="10"
        :max-rows="1000"
        @success="handleImportSuccess"
      />
    </TaktModal>
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      id-column-key="genTableColumnId"
      action-column-key="action"
      entity-scope="tenant"
      table-mode="masterDetailDetail"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * Takt代码生成表配置实体 特例：继承组合 4：无关联工厂、无语言子表 genTableColumn 右栏面板
 * @module views/code/generator/gen-table/components
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { measureMasterDetailLrTableScrollY } from '@/composables/useTaktMasterDetailLrScrollY'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/tableScroll'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import {
  filterMergedColumnsByDefaultVisible,
  filterTableColumnsByVisibleKeys,
  mergeDefaultColumns,
  normalizeUserTableColumns,
} from '@/utils/tableColumns'
import { formatSummaryValue } from '@/components/business/TaktEditableTable/editableTableUtils'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'
import GenTableColumnForm from './GenTableColumnForm.vue'
import { useGenTableMasterContext } from '../composables/useGenTableMasterContext'
import {
  getGenTableColumnList,
  getGenTableColumnById,
  createGenTableColumn,
  updateGenTableColumn,
  deleteGenTableColumnById,
  deleteGenTableColumnBatch,
  getGenTableColumnTemplate,
  importGenTableColumn,
  exportGenTableColumn,
} from '@/api/code/generator/genTableColumn.api'
import type { GenTableColumn, GenTableColumnQuery } from '@/types/code/generator/genTableColumn.types'

import {
  useGenTableColumnI18n,
  GENTABLECOLUMN_DEFAULT_VISIBLE_COLUMN_KEYS,
  GENTABLECOLUMN_SUMMARY_SUM_FIELDS,
  GENTABLECOLUMN_QUERY_STRING_FIELDS,
  GENTABLECOLUMN_QUERY_FIELDS,
  GENTABLECOLUMN_SELF_I18N_KEY,
} from '../composables/useGenTableColumnI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useGenTableColumnI18n()

const { t } = useI18n()
const { selectedMasterRow } = useGenTableMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktGenTableColumn')
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
const dataSource = ref<GenTableColumn[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<GenTableColumn | null>(null)
const selectedRows = ref<GenTableColumn[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<GenTableColumn>>({})
const formLoading = ref(false)
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

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
  for (const key of GENTABLECOLUMN_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    return true
  }
  if (form.length !== undefined && form.length !== null) {
    return true
  }
  if (form.decimalDigits !== undefined && form.decimalDigits !== null) {
    return true
  }
  if (form.isPk !== undefined && form.isPk !== null) {
    return true
  }
  if (form.isIncrement !== undefined && form.isIncrement !== null) {
    return true
  }
  if (form.isRequired !== undefined && form.isRequired !== null) {
    return true
  }
  if (form.isCreate !== undefined && form.isCreate !== null) {
    return true
  }
  if (form.isUpdate !== undefined && form.isUpdate !== null) {
    return true
  }
  if (form.isUnique !== undefined && form.isUnique !== null) {
    return true
  }
  if (form.isList !== undefined && form.isList !== null) {
    return true
  }
  if (form.isExport !== undefined && form.isExport !== null) {
    return true
  }
  if (form.isSort !== undefined && form.isSort !== null) {
    return true
  }
  if (form.isQuery !== undefined && form.isQuery !== null) {
    return true
  }
  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(GENTABLECOLUMN_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof GENTABLECOLUMN_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    lineNumber: undefined as number | undefined,
    length: undefined as number | undefined,
    decimalDigits: undefined as number | undefined,
    isPk: undefined as number | undefined,
    isIncrement: undefined as number | undefined,
    isRequired: undefined as number | undefined,
    isCreate: undefined as number | undefined,
    isUpdate: undefined as number | undefined,
    isUnique: undefined as number | undefined,
    isList: undefined as number | undefined,
    isExport: undefined as number | undefined,
    isSort: undefined as number | undefined,
    isQuery: undefined as number | undefined,  }
}
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() =>
  GENTABLECOLUMN_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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
const visibleColumnKeys = ref<string[]>([...GENTABLECOLUMN_DEFAULT_VISIBLE_COLUMN_KEYS])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = [...GENTABLECOLUMN_DEFAULT_VISIBLE_COLUMN_KEYS]
}
const importVisible = ref(false)

const entityIdName = 'genTableColumnId'
const masterGenTableId = computed((): string => {
  const id = (selectedMasterRow.value as Record<string, unknown> | null)?.['genTableId']
  return id != null ? String(id) : ''
})
const hasMasterSelection = computed(() => masterGenTableId.value !== '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getGenTableColumnId(record: GenTableColumn | Record<string, unknown>): string {
  return String((record as GenTableColumn)?.[entityIdName] ?? '')
}

function getGenTableColumnField(record: GenTableColumn | Record<string, unknown>, field: string): unknown {
  return (record as GenTableColumn)?.[field as keyof GenTableColumn]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'genTableColumnId',
    key: 'genTableColumnId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: GenTableColumn }) =>
      String(getGenTableColumnField(record, 'genTableColumnId') ?? ''),
  },
  {
    title: pi.label('genTableId'),
    dataIndex: 'genTableId',
    key: 'genTableId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: GenTableColumn }) =>
      String(getGenTableColumnField(record, 'genTableId') ?? ''),
  },
  {
    title: pi.label('lineNumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: GenTableColumn }) =>
      String(getGenTableColumnField(record, 'lineNumber') ?? ''),
  },
  {
    title: pi.label('databaseColumnName'),
    dataIndex: 'databaseColumnName',
    key: 'databaseColumnName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: GenTableColumn }) =>
      String(getGenTableColumnField(record, 'databaseColumnName') ?? ''),
  },
  {
    title: pi.label('columnComment'),
    dataIndex: 'columnComment',
    key: 'columnComment',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: GenTableColumn }) =>
      String(getGenTableColumnField(record, 'columnComment') ?? ''),
  },
  {
    title: pi.label('databaseDataType'),
    dataIndex: 'databaseDataType',
    key: 'databaseDataType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: GenTableColumn }) =>
      String(getGenTableColumnField(record, 'databaseDataType') ?? ''),
  },
  {
    title: pi.label('csharpDataType'),
    dataIndex: 'csharpDataType',
    key: 'csharpDataType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: GenTableColumn }) =>
      String(getGenTableColumnField(record, 'csharpDataType') ?? ''),
  },
  {
    title: pi.label('csharpColumnName'),
    dataIndex: 'csharpColumnName',
    key: 'csharpColumnName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: GenTableColumn }) =>
      String(getGenTableColumnField(record, 'csharpColumnName') ?? ''),
  },
  {
    title: pi.label('length'),
    dataIndex: 'length',
    key: 'length',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: GenTableColumn }) =>
      String(getGenTableColumnField(record, 'length') ?? ''),
  },
  {
    title: pi.label('decimalDigits'),
    dataIndex: 'decimalDigits',
    key: 'decimalDigits',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: GenTableColumn }) =>
      String(getGenTableColumnField(record, 'decimalDigits') ?? ''),
  },
  {
    title: pi.label('isPk'),
    dataIndex: 'isPk',
    key: 'isPk',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: GenTableColumn }) =>
      String(getGenTableColumnField(record, 'isPk') ?? ''),
  },
  {
    title: pi.label('isIncrement'),
    dataIndex: 'isIncrement',
    key: 'isIncrement',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: GenTableColumn }) =>
      String(getGenTableColumnField(record, 'isIncrement') ?? ''),
  },
  {
    title: pi.label('isRequired'),
    dataIndex: 'isRequired',
    key: 'isRequired',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: GenTableColumn }) =>
      String(getGenTableColumnField(record, 'isRequired') ?? ''),
  },
  {
    title: pi.label('isCreate'),
    dataIndex: 'isCreate',
    key: 'isCreate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: GenTableColumn }) =>
      String(getGenTableColumnField(record, 'isCreate') ?? ''),
  },
  {
    title: pi.label('isUpdate'),
    dataIndex: 'isUpdate',
    key: 'isUpdate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: GenTableColumn }) =>
      String(getGenTableColumnField(record, 'isUpdate') ?? ''),
  },
  {
    title: pi.label('isUnique'),
    dataIndex: 'isUnique',
    key: 'isUnique',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: GenTableColumn }) =>
      String(getGenTableColumnField(record, 'isUnique') ?? ''),
  },
  {
    title: pi.label('isList'),
    dataIndex: 'isList',
    key: 'isList',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: GenTableColumn }) =>
      String(getGenTableColumnField(record, 'isList') ?? ''),
  },
  {
    title: pi.label('isExport'),
    dataIndex: 'isExport',
    key: 'isExport',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: GenTableColumn }) =>
      String(getGenTableColumnField(record, 'isExport') ?? ''),
  },
  {
    title: pi.label('isSort'),
    dataIndex: 'isSort',
    key: 'isSort',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: GenTableColumn }) =>
      String(getGenTableColumnField(record, 'isSort') ?? ''),
  },
  {
    title: pi.label('isQuery'),
    dataIndex: 'isQuery',
    key: 'isQuery',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: GenTableColumn }) =>
      String(getGenTableColumnField(record, 'isQuery') ?? ''),
  },
  {
    title: pi.label('queryType'),
    dataIndex: 'queryType',
    key: 'queryType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: GenTableColumn }) =>
      String(getGenTableColumnField(record, 'queryType') ?? ''),
  },
  {
    title: pi.label('htmlType'),
    dataIndex: 'htmlType',
    key: 'htmlType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: GenTableColumn }) =>
      String(getGenTableColumnField(record, 'htmlType') ?? ''),
  },
  {
    title: pi.label('dictType'),
    dataIndex: 'dictType',
    key: 'dictType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: GenTableColumn }) =>
      String(getGenTableColumnField(record, 'dictType') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'code:generator:gen:table:update',
        onClick: (record: GenTableColumn) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'code:generator:gen:table:delete',
        onClick: (record: GenTableColumn) => void handleDeleteOne(record),
      },
    ],
  }),
])

/** 与 TaktSingleTable 展示列对齐（用于汇总行单元格） */
const resolvedSummaryColumns = computed(() => {
  const userCols = normalizeUserTableColumns(columns.value)
  const merged = mergeDefaultColumns(userCols, t, true, 'tenant')
  const keys = visibleColumnKeys.value
  if (keys.length > 0) {
    return filterTableColumnsByVisibleKeys(merged, keys, merged)
  }
  return filterMergedColumnsByDefaultVisible(merged, userCols, {
    idColumnKey: 'genTableColumnId',
    actionColumnKey: 'action',
    tableMode: 'masterDetailDetail',
    entityScope: 'tenant',
  })
})

const summarySumFieldSet = new Set<string>(GENTABLECOLUMN_SUMMARY_SUM_FIELDS)

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
    GENTABLECOLUMN_SUMMARY_SUM_FIELDS.map((field) => [field, 0]),
  ) as Record<(typeof GENTABLECOLUMN_SUMMARY_SUM_FIELDS)[number], number>
  for (const row of dataSource.value) {
    for (const field of GENTABLECOLUMN_SUMMARY_SUM_FIELDS) {
      const num = Number(getGenTableColumnField(row, field))
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
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: GenTableColumn[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: GenTableColumn, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getGenTableColumnId(selectedRow.value) === getGenTableColumnId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: GenTableColumn[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: GenTableColumn) {
  const key = getGenTableColumnId(record)
  return {
    onClick: () => {
      selectedRowKeys.value = [key]
      selectedRows.value = [record]
      selectedRow.value = record
    },
    class: selectedRowKeys.value.includes(key)
      ? 'takt-master-detail-table-row-selected cursor-pointer'
      : 'cursor-pointer',
  }
}

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {GenTableColumnQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<GenTableColumnQuery>): GenTableColumnQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: GenTableColumnQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    genTableId: masterGenTableId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof GenTableColumnQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of GENTABLECOLUMN_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  if (form.length !== undefined && form.length !== null) {
    query.length = form.length
  }
  if (form.decimalDigits !== undefined && form.decimalDigits !== null) {
    query.decimalDigits = form.decimalDigits
  }
  if (form.isPk !== undefined && form.isPk !== null) {
    query.isPk = form.isPk
  }
  if (form.isIncrement !== undefined && form.isIncrement !== null) {
    query.isIncrement = form.isIncrement
  }
  if (form.isRequired !== undefined && form.isRequired !== null) {
    query.isRequired = form.isRequired
  }
  if (form.isCreate !== undefined && form.isCreate !== null) {
    query.isCreate = form.isCreate
  }
  if (form.isUpdate !== undefined && form.isUpdate !== null) {
    query.isUpdate = form.isUpdate
  }
  if (form.isUnique !== undefined && form.isUnique !== null) {
    query.isUnique = form.isUnique
  }
  if (form.isList !== undefined && form.isList !== null) {
    query.isList = form.isList
  }
  if (form.isExport !== undefined && form.isExport !== null) {
    query.isExport = form.isExport
  }
  if (form.isSort !== undefined && form.isSort !== null) {
    query.isSort = form.isSort
  }
  if (form.isQuery !== undefined && form.isQuery !== null) {
    query.isQuery = form.isQuery
  }
  return query
}

async function loadData() {
  if (!hasMasterSelection.value) {
    dataSource.value = []
    total.value = 0
    selectedRowKeys.value = []
    selectedRows.value = []
    selectedRow.value = null
    return
  }
  loading.value = true
  try {
    const res = await getGenTableColumnList(buildListQuery())
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
watch(masterGenTableId, () => {
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

watch(hasMasterSelection, (selected) => {
  if (selected) {
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

function handleCreate() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: GenTableColumn) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getGenTableColumnById(getGenTableColumnId(record))
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

function handleUpdate() {
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.edit'),
      entity: pi.self(),
    }))
  }
}

async function handleFormSubmit() {
  const refInst = formRef.value
  if (!refInst?.validate) return
  try {
    await refInst.validate()
  } catch {
    return
  }
  formLoading.value = true
  try {
    const payload = refInst.getValues?.()
    const id = formData.value?.genTableColumnId
    if (id) {
      await updateGenTableColumn(id, payload)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createGenTableColumn(payload)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    await loadData()
  } finally {
    formLoading.value = false
  }
}

function handleFormCancel() {
  formVisible.value = false
}

async function handleDeleteOne(record: GenTableColumn) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: pi.self(),
      name: t('common.tip.this.target', { target: pi.self() }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteGenTableColumnById(getGenTableColumnId(record))
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: pi.self(),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: pi.self(),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getGenTableColumnId(r)).filter(Boolean)
      await deleteGenTableColumnBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      await loadData()
    },
  })
}

function handleRefresh() {
  void loadData()
}

/** 打开导入对话框 */
function handleImport() {
  if (!hasMasterSelection.value) {
      message.warning(t('common.status.empty'))
      return
    }
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getGenTableColumnTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importGenTableColumn(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  void loadData()
  if (result.fail === 0 && result.success > 0) {
    setTimeout(() => { importVisible.value = false }, 2000)
  }
}

/** 关闭导入对话框 */
function handleImportCancel() {
  importVisible.value = false
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
    const exportMeta = await exportGenTableColumn(
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

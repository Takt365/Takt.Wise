<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/assurance-incoming/components -->
<!-- 文件名称：AssuranceIncomingPanel.vue -->
<!-- 功能描述：品质业务主表主表实体右侧明细 qualityAssuranceIncoming 独立 CRUD（按主表选中 qualityAssuranceId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="assurance-incoming-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.qualityassuranceincoming._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:quality:cost:assurance:create"
      update-permission="logistics:quality:cost:assurance:update"
      delete-permission="logistics:quality:cost:assurance:delete"
      import-permission="logistics:quality:cost:assurance:import"
      export-permission="logistics:quality:cost:assurance:export"
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
    <div class="assurance-incoming-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getQualityAssuranceIncomingId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="qualityAssuranceIncomingId"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        scroll-layout="masterDetailLr"
        table-mode="single"
        :show-row-selection="true"
        @change="handleTableChange"
        @pagination-change="handleMasterDetailPaginationChange"
        @resize-column="handleResizeColumn"
      />
    </div>
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      :width="formModalWidthPx"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <QualityAssuranceIncomingForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterQualityAssuranceId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-quality-cost-assurance-incoming-assurance-incoming"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('qualityAssuranceCode')">
      <a-form-item :label="t('entity.qualityassuranceincoming.qualityassurancecode')">
        <a-input
          v-model:value="advancedQueryForm.qualityAssuranceCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityassuranceincoming.qualityassurancecode') })"
          show-count
          :maxlength="30"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.qualityassuranceincoming.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityassuranceincoming.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('directManpowerCostPerMinute')">
      <a-form-item :label="t('entity.qualityassuranceincoming.directmanpowercostperminute')">
        <a-input-number
          v-model:value="advancedQueryForm.directManpowerCostPerMinute"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityassuranceincoming.directmanpowercostperminute') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('incomingInspectionCost')">
      <a-form-item :label="t('entity.qualityassuranceincoming.incominginspectioncost')">
        <a-input-number
          v-model:value="advancedQueryForm.incomingInspectionCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityassuranceincoming.incominginspectioncost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionTimeMinutes')">
      <a-form-item :label="t('entity.qualityassuranceincoming.inspectiontimeminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.inspectionTimeMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityassuranceincoming.inspectiontimeminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('travelCost')">
      <a-form-item :label="t('entity.qualityassuranceincoming.travelcost')">
        <a-input-number
          v-model:value="advancedQueryForm.travelCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityassuranceincoming.travelcost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('otherExpenses')">
      <a-form-item :label="t('entity.qualityassuranceincoming.otherexpenses')">
        <a-input-number
          v-model:value="advancedQueryForm.otherExpenses"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityassuranceincoming.otherexpenses') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('incomingNote')">
      <a-form-item :label="t('entity.qualityassuranceincoming.incomingnote')">
        <a-textarea
          v-model:value="advancedQueryForm.incomingNote"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityassuranceincoming.incomingnote') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="t('common.page.entity.createdatstart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('common.page.entity.createdatstart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="t('common.page.entity.createdatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('common.page.entity.createdatend') })"
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
            <span>{{ t('common.page.entity.extfield') }}</span>
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
      <a-form-item :label="t('common.page.entity.remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      </template>
    </TaktQueryDrawer>
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.qualityassuranceincoming._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.qualityassuranceincoming._self"
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
      id-column-key="qualityAssuranceIncomingId"
      action-column-key="action"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 品质业务主表子表 qualityAssuranceIncoming 右栏面板
 * @module views/logistics/quality/cost/assurance-incoming/components
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'
import QualityAssuranceIncomingForm from './AssuranceIncomingForm.vue'
import { useQualityAssuranceMasterContext } from '../composables/useAssuranceMasterContext'
import {
  getQualityAssuranceIncomingList,
  getQualityAssuranceIncomingById,
  createQualityAssuranceIncoming,
  updateQualityAssuranceIncoming,
  deleteQualityAssuranceIncomingById,
  deleteQualityAssuranceIncomingBatch,
  getQualityAssuranceIncomingTemplate,
  importQualityAssuranceIncoming,
  exportQualityAssuranceIncoming,
} from '@/api/logistics/quality/cost/assuranceIncoming.api'
import type { QualityAssuranceIncoming, QualityAssuranceIncomingQuery } from '@/types/logistics/quality/cost/assuranceIncoming.types'

const { t } = useI18n()
const { selectedMasterRow } = useQualityAssuranceMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktQualityAssuranceIncoming')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.qualityassuranceincoming._self') }),
)

const loading = ref(false)
const dataSource = ref<QualityAssuranceIncoming[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<QualityAssuranceIncoming | null>(null)
const selectedRows = ref<QualityAssuranceIncoming[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<QualityAssuranceIncoming>>({})
const formLoading = ref(false)
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  qualityAssuranceCode: '',
  lineNumber: undefined as number | undefined,
  directManpowerCostPerMinute: undefined as number | undefined,
  incomingInspectionCost: undefined as number | undefined,
  inspectionTimeMinutes: undefined as number | undefined,
  travelCost: undefined as number | undefined,
  otherExpenses: undefined as number | undefined,
  incomingNote: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'qualityAssuranceCode', label: t('entity.qualityassuranceincoming.qualityassurancecode') },
  { key: 'lineNumber', label: t('entity.qualityassuranceincoming.linenumber') },
  { key: 'directManpowerCostPerMinute', label: t('entity.qualityassuranceincoming.directmanpowercostperminute') },
  { key: 'incomingInspectionCost', label: t('entity.qualityassuranceincoming.incominginspectioncost') },
  { key: 'inspectionTimeMinutes', label: t('entity.qualityassuranceincoming.inspectiontimeminutes') },
  { key: 'travelCost', label: t('entity.qualityassuranceincoming.travelcost') },
  { key: 'otherExpenses', label: t('entity.qualityassuranceincoming.otherexpenses') },
  { key: 'incomingNote', label: t('entity.qualityassuranceincoming.incomingnote') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extField', label: t('common.page.entity.extfield') },
  { key: 'remark', label: t('common.page.entity.remark') }])

/**
 * 高级查询字段标签
 * @param key 字段 key
 */
function fieldLabel(key: string): string {
  return queryFieldsMeta.value.find((f) => f.key === key)?.label ?? key
}

function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  qualityAssuranceCode: '',
  lineNumber: undefined as number | undefined,
  directManpowerCostPerMinute: undefined as number | undefined,
  incomingInspectionCost: undefined as number | undefined,
  inspectionTimeMinutes: undefined as number | undefined,
  travelCost: undefined as number | undefined,
  otherExpenses: undefined as number | undefined,
  incomingNote: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
}
const columnSettingVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}
const importVisible = ref(false)

const entityIdName = 'qualityAssuranceIncomingId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.qualityAssuranceId)
const masterQualityAssuranceId = computed(() => selectedMasterRow.value?.qualityAssuranceId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getQualityAssuranceIncomingId(record: QualityAssuranceIncoming | Record<string, unknown>): string {
  return String((record as QualityAssuranceIncoming)?.[entityIdName] ?? '')
}

function getQualityAssuranceIncomingField(record: QualityAssuranceIncoming | Record<string, unknown>, field: string): unknown {
  return (record as QualityAssuranceIncoming)?.[field as keyof QualityAssuranceIncoming]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'qualityAssuranceIncomingId',
    key: 'qualityAssuranceIncomingId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: QualityAssuranceIncoming }) =>
      String(getQualityAssuranceIncomingField(record, 'qualityAssuranceIncomingId') ?? ''),
  },
  {
    title: t('entity.qualityassuranceincoming.qualityassurancecode'),
    dataIndex: 'qualityAssuranceCode',
    key: 'qualityAssuranceCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityAssuranceIncoming }) =>
      String(getQualityAssuranceIncomingField(record, 'qualityAssuranceCode') ?? ''),
  },
  {
    title: t('entity.qualityassuranceincoming.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityAssuranceIncoming }) =>
      String(getQualityAssuranceIncomingField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.qualityassuranceincoming.directmanpowercostperminute'),
    dataIndex: 'directManpowerCostPerMinute',
    key: 'directManpowerCostPerMinute',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityAssuranceIncoming }) =>
      String(getQualityAssuranceIncomingField(record, 'directManpowerCostPerMinute') ?? ''),
  },
  {
    title: t('entity.qualityassuranceincoming.incominginspectioncost'),
    dataIndex: 'incomingInspectionCost',
    key: 'incomingInspectionCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityAssuranceIncoming }) =>
      String(getQualityAssuranceIncomingField(record, 'incomingInspectionCost') ?? ''),
  },
  {
    title: t('entity.qualityassuranceincoming.inspectiontimeminutes'),
    dataIndex: 'inspectionTimeMinutes',
    key: 'inspectionTimeMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityAssuranceIncoming }) =>
      String(getQualityAssuranceIncomingField(record, 'inspectionTimeMinutes') ?? ''),
  },
  {
    title: t('entity.qualityassuranceincoming.travelcost'),
    dataIndex: 'travelCost',
    key: 'travelCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityAssuranceIncoming }) =>
      String(getQualityAssuranceIncomingField(record, 'travelCost') ?? ''),
  },
  {
    title: t('entity.qualityassuranceincoming.otherexpenses'),
    dataIndex: 'otherExpenses',
    key: 'otherExpenses',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityAssuranceIncoming }) =>
      String(getQualityAssuranceIncomingField(record, 'otherExpenses') ?? ''),
  },
  {
    title: t('entity.qualityassuranceincoming.incomingnote'),
    dataIndex: 'incomingNote',
    key: 'incomingNote',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityAssuranceIncoming }) =>
      String(getQualityAssuranceIncomingField(record, 'incomingNote') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:cost:assurance:update',
        onClick: (record: QualityAssuranceIncoming) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:cost:assurance:delete',
        onClick: (record: QualityAssuranceIncoming) => void handleDeleteOne(record),
      }],
  })])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: QualityAssuranceIncoming[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: QualityAssuranceIncoming, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getQualityAssuranceIncomingId(selectedRow.value) === getQualityAssuranceIncomingId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: QualityAssuranceIncoming[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: QualityAssuranceIncoming) {
  const key = getQualityAssuranceIncomingId(record)
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
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {QualityAssuranceIncomingQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<QualityAssuranceIncomingQuery>): QualityAssuranceIncomingQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: QualityAssuranceIncomingQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    qualityAssuranceId: masterQualityAssuranceId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof QualityAssuranceIncomingQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('qualityAssuranceCode', form.qualityAssuranceCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  if (form.directManpowerCostPerMinute !== undefined && form.directManpowerCostPerMinute !== null) {
    query.directManpowerCostPerMinute = form.directManpowerCostPerMinute
  }
  if (form.incomingInspectionCost !== undefined && form.incomingInspectionCost !== null) {
    query.incomingInspectionCost = form.incomingInspectionCost
  }
  if (form.inspectionTimeMinutes !== undefined && form.inspectionTimeMinutes !== null) {
    query.inspectionTimeMinutes = form.inspectionTimeMinutes
  }
  if (form.travelCost !== undefined && form.travelCost !== null) {
    query.travelCost = form.travelCost
  }
  if (form.otherExpenses !== undefined && form.otherExpenses !== null) {
    query.otherExpenses = form.otherExpenses
  }
  assignTrimmed('incomingNote', form.incomingNote)
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('extField', form.extField)
  assignTrimmed('remark', form.remark)
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
    const res = await getQualityAssuranceIncomingList(buildListQuery())
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
watch(masterQualityAssuranceId, () => {
  reload()
})

/** 租户/公司切换时刷新子表 */
useTableRefresh(loadData)

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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.qualityassuranceincoming._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: QualityAssuranceIncoming) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.qualityassuranceincoming._self') })
  formLoading.value = true
  try {
    const detail = await getQualityAssuranceIncomingById(getQualityAssuranceIncomingId(record))
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
      entity: t('entity.qualityassuranceincoming._self'),
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
    const id = formData.value?.qualityAssuranceIncomingId
    if (id) {
      await updateQualityAssuranceIncoming(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.qualityassuranceincoming._self') }))
    } else {
      await createQualityAssuranceIncoming(payload)
      message.success(t('common.feedback.created', { target: t('entity.qualityassuranceincoming._self') }))
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

async function handleDeleteOne(record: QualityAssuranceIncoming) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.qualityassuranceincoming._self'),
      name: t('common.tip.this.target', { target: t('entity.qualityassuranceincoming._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteQualityAssuranceIncomingById(getQualityAssuranceIncomingId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.qualityassuranceincoming._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.qualityassuranceincoming._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.qualityassuranceincoming._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getQualityAssuranceIncomingId(r)).filter(Boolean)
      await deleteQualityAssuranceIncomingBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.qualityassuranceincoming._self') }))
      await loadData()
    },
  })
}

function handleRefresh() {
  void loadData()
}

function handleImport() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  importVisible.value = true
}

async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getQualityAssuranceIncomingTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importQualityAssuranceIncoming(file, sheetName)
}

function handleImportSuccess(result: { success: number; fail: number; errors: string[] }) {
  void loadData()
  if (result.fail === 0) {
    setTimeout(() => {
      importVisible.value = false
    }, 2000)
  }
}

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
    const exportMeta = await exportQualityAssuranceIncoming(
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
    message.success(t('common.feedback.export.success', { target: t('entity.qualityassuranceincoming._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.qualityassuranceincoming._self') }))
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

<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/assurance-customer-response/components -->
<!-- 文件名称：AssuranceCustomerResponsePanel.vue -->
<!-- 功能描述：品质业务主表主表实体右侧明细 qualityAssuranceCustomerResponse 独立 CRUD（按主表选中 qualityAssuranceId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="assurance-customer-response-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.qualityassurancecustomerresponse._self') }}
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
    <div class="assurance-customer-response-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getQualityAssuranceCustomerResponseId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="qualityAssuranceCustomerResponseId"
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
      <QualityAssuranceCustomerResponseForm
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
      storage-key="takt-query-fields-logistics-quality-cost-assurance-customer-response-assurance-customer-response"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('qualityAssuranceCode')">
      <a-form-item :label="t('entity.qualityassurancecustomerresponse.qualityassurancecode')">
        <a-input
          v-model:value="advancedQueryForm.qualityAssuranceCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityassurancecustomerresponse.qualityassurancecode') })"
          show-count
          :maxlength="30"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.qualityassurancecustomerresponse.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityassurancecustomerresponse.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('responseCost')">
      <a-form-item :label="t('entity.qualityassurancecustomerresponse.responsecost')">
        <a-input-number
          v-model:value="advancedQueryForm.responseCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityassurancecustomerresponse.responsecost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workTimeMinutes')">
      <a-form-item :label="t('entity.qualityassurancecustomerresponse.worktimeminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.workTimeMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityassurancecustomerresponse.worktimeminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('otherExpenses')">
      <a-form-item :label="t('entity.qualityassurancecustomerresponse.otherexpenses')">
        <a-input-number
          v-model:value="advancedQueryForm.otherExpenses"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityassurancecustomerresponse.otherexpenses') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerResponseNote')">
      <a-form-item :label="t('entity.qualityassurancecustomerresponse.customerresponsenote')">
        <a-textarea
          v-model:value="advancedQueryForm.customerResponseNote"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityassurancecustomerresponse.customerresponsenote') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.qualityassurancecustomerresponse._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.qualityassurancecustomerresponse._self"
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
      id-column-key="qualityAssuranceCustomerResponseId"
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
 * 品质业务主表子表 qualityAssuranceCustomerResponse 右栏面板
 * @module views/logistics/quality/cost/assurance-customer-response/components
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
import QualityAssuranceCustomerResponseForm from './AssuranceCustomerResponseForm.vue'
import { useQualityAssuranceMasterContext } from '../composables/useAssuranceMasterContext'
import {
  getQualityAssuranceCustomerResponseList,
  getQualityAssuranceCustomerResponseById,
  createQualityAssuranceCustomerResponse,
  updateQualityAssuranceCustomerResponse,
  deleteQualityAssuranceCustomerResponseById,
  deleteQualityAssuranceCustomerResponseBatch,
  getQualityAssuranceCustomerResponseTemplate,
  importQualityAssuranceCustomerResponse,
  exportQualityAssuranceCustomerResponse,
} from '@/api/logistics/quality/cost/assuranceCustomerResponse.api'
import type { QualityAssuranceCustomerResponse, QualityAssuranceCustomerResponseQuery } from '@/types/logistics/quality/cost/assuranceCustomerResponse.types'

const { t } = useI18n()
const { selecmedMasterRow } = useQualityAssuranceMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktQualityAssuranceCustomerResponse')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.qualityassurancecustomerresponse._self') }),
)

const loading = ref(false)
const dataSource = ref<QualityAssuranceCustomerResponse[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selecmedRow = ref<QualityAssuranceCustomerResponse | null>(null)
const selecmedRows = ref<QualityAssuranceCustomerResponse[]>([])
const selecmedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<QualityAssuranceCustomerResponse>>({})
const formLoading = ref(false)
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  qualityAssuranceCode: '',
  lineNumber: undefined as number | undefined,
  responseCost: undefined as number | undefined,
  workTimeMinutes: undefined as number | undefined,
  otherExpenses: undefined as number | undefined,
  customerResponseNote: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'qualityAssuranceCode', label: t('entity.qualityassurancecustomerresponse.qualityassurancecode') },
  { key: 'lineNumber', label: t('entity.qualityassurancecustomerresponse.linenumber') },
  { key: 'responseCost', label: t('entity.qualityassurancecustomerresponse.responsecost') },
  { key: 'workTimeMinutes', label: t('entity.qualityassurancecustomerresponse.worktimeminutes') },
  { key: 'otherExpenses', label: t('entity.qualityassurancecustomerresponse.otherexpenses') },
  { key: 'customerResponseNote', label: t('entity.qualityassurancecustomerresponse.customerresponsenote') },
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
  responseCost: undefined as number | undefined,
  workTimeMinutes: undefined as number | undefined,
  otherExpenses: undefined as number | undefined,
  customerResponseNote: '',
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

const entityIdName = 'qualityAssuranceCustomerResponseId'
const hasMasterSelection = computed(() => !!selecmedMasterRow.value?.qualityAssuranceId)
const masterQualityAssuranceId = computed(() => selecmedMasterRow.value?.qualityAssuranceId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selecmedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selecmedRows.value.length === 0)

function getQualityAssuranceCustomerResponseId(record: QualityAssuranceCustomerResponse | Record<string, unknown>): string {
  return String((record as QualityAssuranceCustomerResponse)?.[entityIdName] ?? '')
}

function getQualityAssuranceCustomerResponseField(record: QualityAssuranceCustomerResponse | Record<string, unknown>, field: string): unknown {
  return (record as QualityAssuranceCustomerResponse)?.[field as keyof QualityAssuranceCustomerResponse]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'qualityAssuranceCustomerResponseId',
    key: 'qualityAssuranceCustomerResponseId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: QualityAssuranceCustomerResponse }) =>
      String(getQualityAssuranceCustomerResponseField(record, 'qualityAssuranceCustomerResponseId') ?? ''),
  },
  {
    title: t('entity.qualityassurancecustomerresponse.qualityassurancecode'),
    dataIndex: 'qualityAssuranceCode',
    key: 'qualityAssuranceCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityAssuranceCustomerResponse }) =>
      String(getQualityAssuranceCustomerResponseField(record, 'qualityAssuranceCode') ?? ''),
  },
  {
    title: t('entity.qualityassurancecustomerresponse.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityAssuranceCustomerResponse }) =>
      String(getQualityAssuranceCustomerResponseField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.qualityassurancecustomerresponse.responsecost'),
    dataIndex: 'responseCost',
    key: 'responseCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityAssuranceCustomerResponse }) =>
      String(getQualityAssuranceCustomerResponseField(record, 'responseCost') ?? ''),
  },
  {
    title: t('entity.qualityassurancecustomerresponse.worktimeminutes'),
    dataIndex: 'workTimeMinutes',
    key: 'workTimeMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityAssuranceCustomerResponse }) =>
      String(getQualityAssuranceCustomerResponseField(record, 'workTimeMinutes') ?? ''),
  },
  {
    title: t('entity.qualityassurancecustomerresponse.otherexpenses'),
    dataIndex: 'otherExpenses',
    key: 'otherExpenses',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityAssuranceCustomerResponse }) =>
      String(getQualityAssuranceCustomerResponseField(record, 'otherExpenses') ?? ''),
  },
  {
    title: t('entity.qualityassurancecustomerresponse.customerresponsenote'),
    dataIndex: 'customerResponseNote',
    key: 'customerResponseNote',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityAssuranceCustomerResponse }) =>
      String(getQualityAssuranceCustomerResponseField(record, 'customerResponseNote') ?? ''),
  },
  {
    title: t('entity.qualityassurancecustomerresponse.operation'),
    dataIndex: 'operation',
    key: 'operation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityAssuranceCustomerResponse }) =>
      String(getQualityAssuranceCustomerResponseField(record, 'operation') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:cost:assurance:update',
        onClick: (record: QualityAssuranceCustomerResponse) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:cost:assurance:delete',
        onClick: (record: QualityAssuranceCustomerResponse) => void handleDeleteOne(record),
      }],
  })])

const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: QualityAssuranceCustomerResponse[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: QualityAssuranceCustomerResponse, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (getQualityAssuranceCustomerResponseId(selecmedRow.value) === getQualityAssuranceCustomerResponseId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: QualityAssuranceCustomerResponse[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: QualityAssuranceCustomerResponse) {
  const key = getQualityAssuranceCustomerResponseId(record)
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
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {QualityAssuranceCustomerResponseQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<QualityAssuranceCustomerResponseQuery>): QualityAssuranceCustomerResponseQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: QualityAssuranceCustomerResponseQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    qualityAssuranceId: masterQualityAssuranceId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof QualityAssuranceCustomerResponseQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('qualityAssuranceCode', form.qualityAssuranceCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  if (form.responseCost !== undefined && form.responseCost !== null) {
    query.responseCost = form.responseCost
  }
  if (form.workTimeMinutes !== undefined && form.workTimeMinutes !== null) {
    query.workTimeMinutes = form.workTimeMinutes
  }
  if (form.otherExpenses !== undefined && form.otherExpenses !== null) {
    query.otherExpenses = form.otherExpenses
  }
  assignTrimmed('customerResponseNote', form.customerResponseNote)
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
    selecmedRowKeys.value = []
    selecmedRows.value = []
    selecmedRow.value = null
    return
  }
  loading.value = true
  try {
    const res = await getQualityAssuranceCustomerResponseList(buildListQuery())
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.qualityassurancecustomerresponse._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: QualityAssuranceCustomerResponse) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.qualityassurancecustomerresponse._self') })
  formLoading.value = true
  try {
    const detail = await getQualityAssuranceCustomerResponseById(getQualityAssuranceCustomerResponseId(record))
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

function handleUpdate() {
  if (selecmedRow.value) {
    void handleEdit(selecmedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.edit'),
      entity: t('entity.qualityassurancecustomerresponse._self'),
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
    const id = formData.value?.qualityAssuranceCustomerResponseId
    if (id) {
      await updateQualityAssuranceCustomerResponse(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.qualityassurancecustomerresponse._self') }))
    } else {
      await createQualityAssuranceCustomerResponse(payload)
      message.success(t('common.feedback.created', { target: t('entity.qualityassurancecustomerresponse._self') }))
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

async function handleDeleteOne(record: QualityAssuranceCustomerResponse) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.qualityassurancecustomerresponse._self'),
      name: t('common.tip.this.target', { target: t('entity.qualityassurancecustomerresponse._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteQualityAssuranceCustomerResponseById(getQualityAssuranceCustomerResponseId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.qualityassurancecustomerresponse._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.qualityassurancecustomerresponse._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.qualityassurancecustomerresponse._self'),
      count: selecmedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r) => getQualityAssuranceCustomerResponseId(r)).filter(Boolean)
      await deleteQualityAssuranceCustomerResponseBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.qualityassurancecustomerresponse._self') }))
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
  const res = await getQualityAssuranceCustomerResponseTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importQualityAssuranceCustomerResponse(file, sheetName)
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
    const exportMeta = await exportQualityAssuranceCustomerResponse(
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
    message.success(t('common.feedback.export.success', { target: t('entity.qualityassurancecustomerresponse._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.qualityassurancecustomerresponse._self') }))
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

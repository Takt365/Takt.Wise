<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/sop/sop-argument/components -->
<!-- 文件名称：SopArgumentPanel.vue -->
<!-- 功能描述：SOP 工位执行追溯实体主表实体右侧明细 sopArgument 独立 CRUD（按主表选中 sopExecId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="sop-argument-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.sopargument._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:manufacturing:sop:exec:create"
      update-permission="logistics:manufacturing:sop:exec:update"
      delete-permission="logistics:manufacturing:sop:exec:delete"
      import-permission="logistics:manufacturing:sop:exec:import"
      export-permission="logistics:manufacturing:sop:exec:export"
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
    <div class="sop-argument-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getSopArgumentId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="sopArgumentId"
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
      <SopArgumentForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterSopExecId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-sop-sop-argument-sop-argument"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('execId')">
      <a-form-item :label="t('entity.sopargument.execid')">
        <a-input
          v-model:value="advancedQueryForm.execId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopargument.execid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('execStepId')">
      <a-form-item :label="t('entity.sopargument.execstepid')">
        <a-input
          v-model:value="advancedQueryForm.execStepId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopargument.execstepid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('routingItemParameterId')">
      <a-form-item :label="t('entity.sopargument.routingitemparameterid')">
        <a-input
          v-model:value="advancedQueryForm.routingItemParameterId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopargument.routingitemparameterid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('paramCode')">
      <a-form-item :label="t('entity.sopargument.paramcode')">
        <a-input
          v-model:value="advancedQueryForm.paramCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopargument.paramcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualValue')">
      <a-form-item :label="t('entity.sopargument.actualvalue')">
        <a-input-number
          v-model:value="advancedQueryForm.actualValue"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopargument.actualvalue') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isOutOfRange')">
      <a-form-item :label="t('entity.sopargument.isoutofrange')">
        <TaktSelect
          v-model:value="advancedQueryForm.isOutOfRange"
          dict-type="sys_yes_no"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sopargument.isoutofrange') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('recordedAtStart')">
      <a-form-item :label="t('entity.sopargument.recordedatstart')">
        <a-input
          v-model:value="advancedQueryForm.recordedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopargument.recordedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('recordedAtEnd')">
      <a-form-item :label="t('entity.sopargument.recordedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.recordedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sopargument.recordedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
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
      :title="t('common.dialog.title.import', { entity: t('entity.sopargument._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.sopargument._self"
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
      id-column-key="sopArgumentId"
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
 * SOP 工位执行追溯实体子表 sopArgument 右栏面板
 * @module views/logistics/manufacturing/sop/sop-argument/components
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
import SopArgumentForm from './SopArgumentForm.vue'
import { useSopExecMasterContext } from '../composables/useExecMasterContext'
import {
  getSopArgumentList,
  getSopArgumentById,
  createSopArgument,
  updateSopArgument,
  deleteSopArgumentById,
  deleteSopArgumentBatch,
  getSopArgumentTemplate,
  importSopArgument,
  exportSopArgument,
} from '@/api/logistics/manufacturing/sop/sopArgument.api'
import type { SopArgument, SopArgumentQuery } from '@/types/logistics/manufacturing/sop/sopArgument.types'

const { t } = useI18n()
const { selectedMasterRow } = useSopExecMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSopArgument')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.sopargument._self') }),
)

const loading = ref(false)
const dataSource = ref<SopArgument[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<SopArgument | null>(null)
const selectedRows = ref<SopArgument[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<SopArgument>>({})
const formLoading = ref(false)
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  execId: '',
  execStepId: '',
  routingItemParameterId: '',
  paramCode: '',
  actualValue: undefined as number | undefined,
  isOutOfRange: undefined as number | undefined,
  recordedAtStart: '',
  recordedAtEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'execId', label: t('entity.sopargument.execid') },
  { key: 'execStepId', label: t('entity.sopargument.execstepid') },
  { key: 'routingItemParameterId', label: t('entity.sopargument.routingitemparameterid') },
  { key: 'paramCode', label: t('entity.sopargument.paramcode') },
  { key: 'actualValue', label: t('entity.sopargument.actualvalue') },
  { key: 'isOutOfRange', label: t('entity.sopargument.isoutofrange') },
  { key: 'recordedAtStart', label: t('entity.sopargument.recordedatstart') },
  { key: 'recordedAtEnd', label: t('entity.sopargument.recordedatend') },
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
  execId: '',
  execStepId: '',
  routingItemParameterId: '',
  paramCode: '',
  actualValue: undefined as number | undefined,
  isOutOfRange: undefined as number | undefined,
  recordedAtStart: '',
  recordedAtEnd: '',
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

const entityIdName = 'sopArgumentId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.sopExecId)
const masterSopExecId = computed(() => selectedMasterRow.value?.sopExecId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getSopArgumentId(record: SopArgument | Record<string, unknown>): string {
  return String((record as SopArgument)?.[entityIdName] ?? '')
}

function getSopArgumentField(record: SopArgument | Record<string, unknown>, field: string): unknown {
  return (record as SopArgument)?.[field as keyof SopArgument]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'sopArgumentId',
    key: 'sopArgumentId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: SopArgument }) =>
      String(getSopArgumentField(record, 'sopArgumentId') ?? ''),
  },
  {
    title: t('entity.sopargument.execid'),
    dataIndex: 'execId',
    key: 'execId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SopArgument }) =>
      String(getSopArgumentField(record, 'execId') ?? ''),
  },
  {
    title: t('entity.sopargument.execstepid'),
    dataIndex: 'execStepId',
    key: 'execStepId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SopArgument }) =>
      String(getSopArgumentField(record, 'execStepId') ?? ''),
  },
  {
    title: t('entity.sopargument.routingitemparameterid'),
    dataIndex: 'routingItemParameterId',
    key: 'routingItemParameterId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SopArgument }) =>
      String(getSopArgumentField(record, 'routingItemParameterId') ?? ''),
  },
  {
    title: t('entity.sopargument.paramcode'),
    dataIndex: 'paramCode',
    key: 'paramCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SopArgument }) =>
      String(getSopArgumentField(record, 'paramCode') ?? ''),
  },
  {
    title: t('entity.sopargument.actualvalue'),
    dataIndex: 'actualValue',
    key: 'actualValue',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SopArgument }) =>
      String(getSopArgumentField(record, 'actualValue') ?? ''),
  },
  {
    title: t('entity.sopargument.isoutofrange'),
    dataIndex: 'isOutOfRange',
    key: 'isOutOfRange',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SopArgument }) =>
      String(getSopArgumentField(record, 'isOutOfRange') ?? ''),
  },
  {
    title: t('entity.sopargument.recordedat'),
    dataIndex: 'recordedAt',
    key: 'recordedAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SopArgument }) =>
      String(getSopArgumentField(record, 'recordedAt') ?? ''),
  },
  {
    title: t('entity.sopargument.exec'),
    dataIndex: 'exec',
    key: 'exec',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SopArgument }) =>
      String(getSopArgumentField(record, 'exec') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:sop:exec:update',
        onClick: (record: SopArgument) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:sop:exec:delete',
        onClick: (record: SopArgument) => void handleDeleteOne(record),
      }],
  })])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: SopArgument[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: SopArgument, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getSopArgumentId(selectedRow.value) === getSopArgumentId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: SopArgument[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: SopArgument) {
  const key = getSopArgumentId(record)
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
 * @returns {SopArgumentQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<SopArgumentQuery>): SopArgumentQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: SopArgumentQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    sopExecId: masterSopExecId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof SopArgumentQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('execId', form.execId)
  assignTrimmed('execStepId', form.execStepId)
  assignTrimmed('routingItemParameterId', form.routingItemParameterId)
  assignTrimmed('paramCode', form.paramCode)
  if (form.actualValue !== undefined && form.actualValue !== null) {
    query.actualValue = form.actualValue
  }
  if (form.isOutOfRange !== undefined && form.isOutOfRange !== null) {
    query.isOutOfRange = form.isOutOfRange
  }
  assignTrimmed('recordedAtStart', form.recordedAtStart)
  assignTrimmed('recordedAtEnd', form.recordedAtEnd)
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
    const res = await getSopArgumentList(buildListQuery())
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
watch(masterSopExecId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.sopargument._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: SopArgument) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.sopargument._self') })
  formLoading.value = true
  try {
    const detail = await getSopArgumentById(getSopArgumentId(record))
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
      entity: t('entity.sopargument._self'),
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
    const id = formData.value?.sopArgumentId
    if (id) {
      await updateSopArgument(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.sopargument._self') }))
    } else {
      await createSopArgument(payload)
      message.success(t('common.feedback.created', { target: t('entity.sopargument._self') }))
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

async function handleDeleteOne(record: SopArgument) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.sopargument._self'),
      name: t('common.tip.this.target', { target: t('entity.sopargument._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteSopArgumentById(getSopArgumentId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.sopargument._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.sopargument._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.sopargument._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getSopArgumentId(r)).filter(Boolean)
      await deleteSopArgumentBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.sopargument._self') }))
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
  const res = await getSopArgumentTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importSopArgument(file, sheetName)
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
    const exportMeta = await exportSopArgument(
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
    message.success(t('common.feedback.export.success', { target: t('entity.sopargument._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.sopargument._self') }))
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

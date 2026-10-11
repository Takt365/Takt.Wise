<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/sop/sop-step-check-item/components -->
<!-- 文件名称：SopStepCheckItemPanel.vue -->
<!-- 功能描述：SOP 工步实体主表实体右侧明细 sopStepCheckItem 独立 CRUD（按主表选中 sopStepId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="sop-step-check-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.sopstepcheckitem._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:manufacturing:sop:doc:create"
      update-permission="logistics:manufacturing:sop:doc:update"
      delete-permission="logistics:manufacturing:sop:doc:delete"
      import-permission="logistics:manufacturing:sop:doc:import"
      export-permission="logistics:manufacturing:sop:doc:export"
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
    <div class="sop-step-check-item-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getSopStepCheckItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="sopStepCheckItemId"
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
      <SopStepCheckItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterSopStepId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-sop-sop-step-check-item-sop-step-check-item"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('stepId')">
      <a-form-item :label="t('entity.sopstepcheckitem.stepid')">
        <a-input
          v-model:value="advancedQueryForm.stepId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopstepcheckitem.stepid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('checkItemName')">
      <a-form-item :label="t('entity.sopstepcheckitem.checkitemname')">
        <a-input
          v-model:value="advancedQueryForm.checkItemName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopstepcheckitem.checkitemname') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('checkMethod')">
      <a-form-item :label="t('entity.sopstepcheckitem.checkmethod')">
        <a-input
          v-model:value="advancedQueryForm.checkMethod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopstepcheckitem.checkmethod') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('checkStandard')">
      <a-form-item :label="t('entity.sopstepcheckitem.checkstandard')">
        <a-input
          v-model:value="advancedQueryForm.checkStandard"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopstepcheckitem.checkstandard') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isRequired')">
      <a-form-item :label="t('entity.sopstepcheckitem.isrequired')">
        <TaktSelect
          v-model:value="advancedQueryForm.isRequired"
          dict-type="sys_yes_no"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sopstepcheckitem.isrequired') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.sopstepcheckitem._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.sopstepcheckitem._self"
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
      id-column-key="sopStepCheckItemId"
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
 * SOP 工步实体子表 sopStepCheckItem 右栏面板
 * @module views/logistics/manufacturing/sop/sop-step-check-item/components
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
import SopStepCheckItemForm from './SopStepCheckItemForm.vue'
import { useSopStepMasterContext } from '../composables/useStepMasterContext'
import {
  getSopStepCheckItemList,
  getSopStepCheckItemById,
  createSopStepCheckItem,
  updateSopStepCheckItem,
  deleteSopStepCheckItemById,
  deleteSopStepCheckItemBatch,
  getSopStepCheckItemTemplate,
  importSopStepCheckItem,
  exportSopStepCheckItem,
} from '@/api/logistics/manufacturing/sop/sopStepCheckItem.api'
import type { SopStepCheckItem, SopStepCheckItemQuery } from '@/types/logistics/manufacturing/sop/sopStepCheckItem.types'

const { t } = useI18n()
const { selectedMasterRow } = useSopStepMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSopStepCheckItem')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.sopstepcheckitem._self') }),
)

const loading = ref(false)
const dataSource = ref<SopStepCheckItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<SopStepCheckItem | null>(null)
const selectedRows = ref<SopStepCheckItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<SopStepCheckItem>>({})
const formLoading = ref(false)
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  stepId: '',
  checkItemName: '',
  checkMethod: '',
  checkStandard: '',
  isRequired: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'stepId', label: t('entity.sopstepcheckitem.stepid') },
  { key: 'checkItemName', label: t('entity.sopstepcheckitem.checkitemname') },
  { key: 'checkMethod', label: t('entity.sopstepcheckitem.checkmethod') },
  { key: 'checkStandard', label: t('entity.sopstepcheckitem.checkstandard') },
  { key: 'isRequired', label: t('entity.sopstepcheckitem.isrequired') },
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
  stepId: '',
  checkItemName: '',
  checkMethod: '',
  checkStandard: '',
  isRequired: undefined as number | undefined,
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

const entityIdName = 'sopStepCheckItemId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.sopStepId)
const masterSopStepId = computed(() => selectedMasterRow.value?.sopStepId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getSopStepCheckItemId(record: SopStepCheckItem | Record<string, unknown>): string {
  return String((record as SopStepCheckItem)?.[entityIdName] ?? '')
}

function getSopStepCheckItemField(record: SopStepCheckItem | Record<string, unknown>, field: string): unknown {
  return (record as SopStepCheckItem)?.[field as keyof SopStepCheckItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'sopStepCheckItemId',
    key: 'sopStepCheckItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: SopStepCheckItem }) =>
      String(getSopStepCheckItemField(record, 'sopStepCheckItemId') ?? ''),
  },
  {
    title: t('entity.sopstepcheckitem.stepid'),
    dataIndex: 'stepId',
    key: 'stepId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SopStepCheckItem }) =>
      String(getSopStepCheckItemField(record, 'stepId') ?? ''),
  },
  {
    title: t('entity.sopstepcheckitem.checkitemname'),
    dataIndex: 'checkItemName',
    key: 'checkItemName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SopStepCheckItem }) =>
      String(getSopStepCheckItemField(record, 'checkItemName') ?? ''),
  },
  {
    title: t('entity.sopstepcheckitem.checkmethod'),
    dataIndex: 'checkMethod',
    key: 'checkMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SopStepCheckItem }) =>
      String(getSopStepCheckItemField(record, 'checkMethod') ?? ''),
  },
  {
    title: t('entity.sopstepcheckitem.checkstandard'),
    dataIndex: 'checkStandard',
    key: 'checkStandard',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SopStepCheckItem }) =>
      String(getSopStepCheckItemField(record, 'checkStandard') ?? ''),
  },
  {
    title: t('entity.sopstepcheckitem.isrequired'),
    dataIndex: 'isRequired',
    key: 'isRequired',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SopStepCheckItem }) =>
      String(getSopStepCheckItemField(record, 'isRequired') ?? ''),
  },
  {
    title: t('entity.sopstepcheckitem.step'),
    dataIndex: 'step',
    key: 'step',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SopStepCheckItem }) =>
      String(getSopStepCheckItemField(record, 'step') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:sop:doc:update',
        onClick: (record: SopStepCheckItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:sop:doc:delete',
        onClick: (record: SopStepCheckItem) => void handleDeleteOne(record),
      }],
  })])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: SopStepCheckItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: SopStepCheckItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getSopStepCheckItemId(selectedRow.value) === getSopStepCheckItemId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: SopStepCheckItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: SopStepCheckItem) {
  const key = getSopStepCheckItemId(record)
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
 * @returns {SopStepCheckItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<SopStepCheckItemQuery>): SopStepCheckItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: SopStepCheckItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    sopStepId: masterSopStepId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof SopStepCheckItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('stepId', form.stepId)
  assignTrimmed('checkItemName', form.checkItemName)
  assignTrimmed('checkMethod', form.checkMethod)
  assignTrimmed('checkStandard', form.checkStandard)
  if (form.isRequired !== undefined && form.isRequired !== null) {
    query.isRequired = form.isRequired
  }
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
    const res = await getSopStepCheckItemList(buildListQuery())
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
watch(masterSopStepId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.sopstepcheckitem._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: SopStepCheckItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.sopstepcheckitem._self') })
  formLoading.value = true
  try {
    const detail = await getSopStepCheckItemById(getSopStepCheckItemId(record))
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
      entity: t('entity.sopstepcheckitem._self'),
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
    const id = formData.value?.sopStepCheckItemId
    if (id) {
      await updateSopStepCheckItem(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.sopstepcheckitem._self') }))
    } else {
      await createSopStepCheckItem(payload)
      message.success(t('common.feedback.created', { target: t('entity.sopstepcheckitem._self') }))
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

async function handleDeleteOne(record: SopStepCheckItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.sopstepcheckitem._self'),
      name: t('common.tip.this.target', { target: t('entity.sopstepcheckitem._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteSopStepCheckItemById(getSopStepCheckItemId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.sopstepcheckitem._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.sopstepcheckitem._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.sopstepcheckitem._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getSopStepCheckItemId(r)).filter(Boolean)
      await deleteSopStepCheckItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.sopstepcheckitem._self') }))
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
  const res = await getSopStepCheckItemTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importSopStepCheckItem(file, sheetName)
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
    const exportMeta = await exportSopStepCheckItem(
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
    message.success(t('common.feedback.export.success', { target: t('entity.sopstepcheckitem._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.sopstepcheckitem._self') }))
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

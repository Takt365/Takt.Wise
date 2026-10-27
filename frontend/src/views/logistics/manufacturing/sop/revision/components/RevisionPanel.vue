<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/sop/revision/components -->
<!-- 文件名称：RevisionPanel.vue -->
<!-- 功能描述：SOP 文档头实体主表实体右侧明细 sopRevision 独立 CRUD（按主表选中 sopDocId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="revision-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.soprevision._self') }}
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
    <div class="revision-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="approval"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getSopRevisionId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="sopRevisionId"
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
      <SopRevisionForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterSopDocId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-sop-revision-revision"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('sopId')">
      <a-form-item :label="t('entity.soprevision.sopid')">
        <a-input
          v-model:value="advancedQueryForm.sopId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.soprevision.sopid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('revision')">
      <a-form-item :label="t('entity.soprevision.revision')">
        <a-input
          v-model:value="advancedQueryForm.revision"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.soprevision.revision') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fileUrl')">
      <a-form-item :label="t('entity.soprevision.fileurl')">
        <a-input
          v-model:value="advancedQueryForm.fileUrl"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.soprevision.fileurl') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('changeDesc')">
      <a-form-item :label="t('entity.soprevision.changedesc')">
        <a-input
          v-model:value="advancedQueryForm.changeDesc"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.soprevision.changedesc') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecnId')">
      <a-form-item :label="t('entity.soprevision.ecnid')">
        <a-input
          v-model:value="advancedQueryForm.ecnId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.soprevision.ecnid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isLocked')">
      <a-form-item :label="t('entity.soprevision.islocked')">
        <TaktSelect
          v-model:value="advancedQueryForm.isLocked"
          dict-type="sys_yes_no"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.soprevision.islocked') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('forceLeaderAck')">
      <a-form-item :label="t('entity.soprevision.forceleaderack')">
        <TaktSelect
          v-model:value="advancedQueryForm.forceLeaderAck"
          dict-type="sys_yes_no"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.soprevision.forceleaderack') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('revisionStatus')">
      <a-form-item :label="t('entity.soprevision.revisionstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.revisionStatus"
          dict-type="sys_lifecycle_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.soprevision.revisionstatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveRule')">
      <a-form-item :label="t('entity.soprevision.effectiverule')">
        <TaktSelect
          v-model:value="advancedQueryForm.effectiveRule"
          dict-type="logistics_sop_effective_rule"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.soprevision.effectiverule') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.soprevision._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.soprevision._self"
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
      id-column-key="sopRevisionId"
      action-column-key="action"
      entity-scope="approval"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * SOP 文档头实体子表 sopRevision 右栏面板
 * @module views/logistics/manufacturing/sop/revision/components
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
import SopRevisionForm from './RevisionForm.vue'
import { useSopDocMasterContext } from '../composables/useDocMasterContext'
import {
  getSopRevisionList,
  getSopRevisionById,
  createSopRevision,
  updateSopRevision,
  deleteSopRevisionById,
  deleteSopRevisionBatch,
  getSopRevisionTemplate,
  importSopRevision,
  exportSopRevision,
} from '@/api/logistics/manufacturing/sop/revision.api'
import type { SopRevision, SopRevisionQuery } from '@/types/logistics/manufacturing/sop/revision.types'

const { t } = useI18n()
const { selecmedMasterRow } = useSopDocMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSopRevision')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.soprevision._self') }),
)

const loading = ref(false)
const dataSource = ref<SopRevision[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selecmedRow = ref<SopRevision | null>(null)
const selecmedRows = ref<SopRevision[]>([])
const selecmedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<SopRevision>>({})
const formLoading = ref(false)
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  sopId: '',
  revision: '',
  fileUrl: '',
  changeDesc: '',
  ecnId: '',
  isLocked: undefined as number | undefined,
  forceLeaderAck: undefined as number | undefined,
  revisionStatus: undefined as number | undefined,
  effectiveRule: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'sopId', label: t('entity.soprevision.sopid') },
  { key: 'revision', label: t('entity.soprevision.revision') },
  { key: 'fileUrl', label: t('entity.soprevision.fileurl') },
  { key: 'changeDesc', label: t('entity.soprevision.changedesc') },
  { key: 'ecnId', label: t('entity.soprevision.ecnid') },
  { key: 'isLocked', label: t('entity.soprevision.islocked') },
  { key: 'forceLeaderAck', label: t('entity.soprevision.forceleaderack') },
  { key: 'revisionStatus', label: t('entity.soprevision.revisionstatus') },
  { key: 'effectiveRule', label: t('entity.soprevision.effectiverule') },
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
  sopId: '',
  revision: '',
  fileUrl: '',
  changeDesc: '',
  ecnId: '',
  isLocked: undefined as number | undefined,
  forceLeaderAck: undefined as number | undefined,
  revisionStatus: undefined as number | undefined,
  effectiveRule: undefined as number | undefined,
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

const entityIdName = 'sopRevisionId'
const hasMasterSelection = computed(() => !!selecmedMasterRow.value?.sopDocId)
const masterSopDocId = computed(() => selecmedMasterRow.value?.sopDocId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selecmedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selecmedRows.value.length === 0)

function getSopRevisionId(record: SopRevision | Record<string, unknown>): string {
  return String((record as SopRevision)?.[entityIdName] ?? '')
}

function getSopRevisionField(record: SopRevision | Record<string, unknown>, field: string): unknown {
  return (record as SopRevision)?.[field as keyof SopRevision]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'sopRevisionId',
    key: 'sopRevisionId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: SopRevision }) =>
      String(getSopRevisionField(record, 'sopRevisionId') ?? ''),
  },
  {
    title: t('entity.soprevision.sopid'),
    dataIndex: 'sopId',
    key: 'sopId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SopRevision }) =>
      String(getSopRevisionField(record, 'sopId') ?? ''),
  },
  {
    title: t('entity.soprevision.revision'),
    dataIndex: 'revision',
    key: 'revision',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SopRevision }) =>
      String(getSopRevisionField(record, 'revision') ?? ''),
  },
  {
    title: t('entity.soprevision.fileurl'),
    dataIndex: 'fileUrl',
    key: 'fileUrl',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SopRevision }) =>
      String(getSopRevisionField(record, 'fileUrl') ?? ''),
  },
  {
    title: t('entity.soprevision.changedesc'),
    dataIndex: 'changeDesc',
    key: 'changeDesc',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SopRevision }) =>
      String(getSopRevisionField(record, 'changeDesc') ?? ''),
  },
  {
    title: t('entity.soprevision.ecnid'),
    dataIndex: 'ecnId',
    key: 'ecnId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SopRevision }) =>
      String(getSopRevisionField(record, 'ecnId') ?? ''),
  },
  {
    title: t('entity.soprevision.islocked'),
    dataIndex: 'isLocked',
    key: 'isLocked',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SopRevision }) =>
      String(getSopRevisionField(record, 'isLocked') ?? ''),
  },
  {
    title: t('entity.soprevision.forceleaderack'),
    dataIndex: 'forceLeaderAck',
    key: 'forceLeaderAck',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SopRevision }) =>
      String(getSopRevisionField(record, 'forceLeaderAck') ?? ''),
  },
  {
    title: t('entity.soprevision.revisionstatus'),
    dataIndex: 'revisionStatus',
    key: 'revisionStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SopRevision }) =>
      String(getSopRevisionField(record, 'revisionStatus') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:sop:doc:update',
        onClick: (record: SopRevision) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:sop:doc:delete',
        onClick: (record: SopRevision) => void handleDeleteOne(record),
      }],
  })])

const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: SopRevision[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: SopRevision, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (getSopRevisionId(selecmedRow.value) === getSopRevisionId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: SopRevision[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: SopRevision) {
  const key = getSopRevisionId(record)
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
 * @returns {SopRevisionQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<SopRevisionQuery>): SopRevisionQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: SopRevisionQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    sopDocId: masterSopDocId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof SopRevisionQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('sopId', form.sopId)
  assignTrimmed('revision', form.revision)
  assignTrimmed('fileUrl', form.fileUrl)
  assignTrimmed('changeDesc', form.changeDesc)
  assignTrimmed('ecnId', form.ecnId)
  if (form.isLocked !== undefined && form.isLocked !== null) {
    query.isLocked = form.isLocked
  }
  if (form.forceLeaderAck !== undefined && form.forceLeaderAck !== null) {
    query.forceLeaderAck = form.forceLeaderAck
  }
  if (form.revisionStatus !== undefined && form.revisionStatus !== null) {
    query.revisionStatus = form.revisionStatus
  }
  if (form.effectiveRule !== undefined && form.effectiveRule !== null) {
    query.effectiveRule = form.effectiveRule
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
    selecmedRowKeys.value = []
    selecmedRows.value = []
    selecmedRow.value = null
    return
  }
  loading.value = true
  try {
    const res = await getSopRevisionList(buildListQuery())
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
watch(masterSopDocId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.soprevision._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: SopRevision) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.soprevision._self') })
  formLoading.value = true
  try {
    const detail = await getSopRevisionById(getSopRevisionId(record))
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
      entity: t('entity.soprevision._self'),
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
    const id = formData.value?.sopRevisionId
    if (id) {
      await updateSopRevision(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.soprevision._self') }))
    } else {
      await createSopRevision(payload)
      message.success(t('common.feedback.created', { target: t('entity.soprevision._self') }))
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

async function handleDeleteOne(record: SopRevision) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.soprevision._self'),
      name: t('common.tip.this.target', { target: t('entity.soprevision._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteSopRevisionById(getSopRevisionId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.soprevision._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.soprevision._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.soprevision._self'),
      count: selecmedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r) => getSopRevisionId(r)).filter(Boolean)
      await deleteSopRevisionBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.soprevision._self') }))
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
  const res = await getSopRevisionTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importSopRevision(file, sheetName)
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
    const exportMeta = await exportSopRevision(
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
    message.success(t('common.feedback.export.success', { target: t('entity.soprevision._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.soprevision._self') }))
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

<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/aps/aps-order/components -->
<!-- 文件名称：ApsOperationPanel.vue -->
<!-- 功能描述：APS 排程订单主表实体右侧明细 apsOperation 独立 CRUD（按主表选中 apsOrderId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="aps-operation-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.apsoperation._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:manufacturing:aps:schedule:create"
      update-permission="logistics:manufacturing:aps:schedule:update"
      delete-permission="logistics:manufacturing:aps:schedule:delete"
      import-permission="logistics:manufacturing:aps:schedule:import"
      export-permission="logistics:manufacturing:aps:schedule:export"
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
    <div class="aps-operation-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getApsOperationId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="apsOperationId"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        scroll-layout="masterDetailLr"
        table-mode="masterDetailDetail"
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
      <ApsOperationForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterApsOrderId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-scheduling-aps-order-aps-operation"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('apsOrderCode')">
      <a-form-item :label="t('entity.apsoperation.apsordercode')">
        <a-input
          v-model:value="advancedQueryForm.apsOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsoperation.apsordercode') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.apsoperation.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsoperation.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('routingItemId')">
      <a-form-item :label="t('entity.apsoperation.routingitemid')">
        <a-input
          v-model:value="advancedQueryForm.routingItemId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsoperation.routingitemid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('processCode')">
      <a-form-item :label="t('entity.apsoperation.processcode')">
        <a-input
          v-model:value="advancedQueryForm.processCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsoperation.processcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('processName')">
      <a-form-item :label="t('entity.apsoperation.processname')">
        <a-input
          v-model:value="advancedQueryForm.processName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsoperation.processname') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workCenterCode')">
      <a-form-item :label="t('entity.apsoperation.workcentercode')">
        <a-input
          v-model:value="advancedQueryForm.workCenterCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsoperation.workcentercode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workCenterResourceId')">
      <a-form-item :label="t('entity.apsoperation.workcenterresourceid')">
        <a-input
          v-model:value="advancedQueryForm.workCenterResourceId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsoperation.workcenterresourceid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedStartTimeStart')">
      <a-form-item :label="t('entity.apsoperation.plannedstarttimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedStartTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsoperation.plannedstarttimestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedStartTimeEnd')">
      <a-form-item :label="t('entity.apsoperation.plannedstarttimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedStartTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsoperation.plannedstarttimeend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedEndTimeStart')">
      <a-form-item :label="t('entity.apsoperation.plannedendtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedEndTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsoperation.plannedendtimestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedEndTimeEnd')">
      <a-form-item :label="t('entity.apsoperation.plannedendtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedEndTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsoperation.plannedendtimeend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedDurationMinutes')">
      <a-form-item :label="t('entity.apsoperation.planneddurationminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.plannedDurationMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsoperation.planneddurationminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('changeoverMinutes')">
      <a-form-item :label="t('entity.apsoperation.changeoverminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.changeoverMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsoperation.changeoverminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('operationStatus')">
      <a-form-item :label="t('entity.apsoperation.operationstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.operationStatus"
          dict-type="aps_operation_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsoperation.operationstatus') })"
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
    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.apsoperation._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        entity-i18n-key="entity.apsoperation._self"
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
      id-column-key="apsOperationId"
      action-column-key="action"
      entity-scope="company"
      table-mode="masterDetailDetail"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * APS 排程订单子表 apsOperation 右栏面板
 * @module views/logistics/manufacturing/aps/aps-order/components
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'
import ApsOperationForm from './ApsOperationForm.vue'
import { useApsOrderMasterContext } from '../composables/useApsOrderMasterContext'
import {
  getApsOperationList,
  getApsOperationById,
  createApsOperation,
  updateApsOperation,
  deleteApsOperationById,
  deleteApsOperationBatch,
  getApsOperationTemplate,
  importApsOperation,
  exportApsOperation,
} from '@/api/logistics/manufacturing/aps/apsOperation.api'
import type { ApsOperation, ApsOperationQuery } from '@/types/logistics/manufacturing/aps/apsOperation.types'

const { t } = useI18n()
const { selectedMasterRow } = useApsOrderMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktApsOperation')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.apsoperation._self') }),
)

const loading = ref(false)
const dataSource = ref<ApsOperation[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<ApsOperation | null>(null)
const selectedRows = ref<ApsOperation[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<ApsOperation>>({})
const formLoading = ref(false)
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  apsOrderCode: '',
  lineNumber: undefined as number | undefined,
  routingItemId: '',
  processCode: '',
  processName: '',
  workCenterCode: '',
  workCenterResourceId: '',
  plannedStartTimeStart: '',
  plannedStartTimeEnd: '',
  plannedEndTimeStart: '',
  plannedEndTimeEnd: '',
  plannedDurationMinutes: undefined as number | undefined,
  changeoverMinutes: undefined as number | undefined,
  operationStatus: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'apsOrderCode', label: t('entity.apsoperation.apsordercode') },
  { key: 'lineNumber', label: t('entity.apsoperation.linenumber') },
  { key: 'routingItemId', label: t('entity.apsoperation.routingitemid') },
  { key: 'processCode', label: t('entity.apsoperation.processcode') },
  { key: 'processName', label: t('entity.apsoperation.processname') },
  { key: 'workCenterCode', label: t('entity.apsoperation.workcentercode') },
  { key: 'workCenterResourceId', label: t('entity.apsoperation.workcenterresourceid') },
  { key: 'plannedStartTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.apsoperation.plannedstarttime')) },
  { key: 'plannedStartTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.apsoperation.plannedstarttime')) },
  { key: 'plannedEndTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.apsoperation.plannedendtime')) },
  { key: 'plannedEndTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.apsoperation.plannedendtime')) },
  { key: 'plannedDurationMinutes', label: t('entity.apsoperation.planneddurationminutes') },
  { key: 'changeoverMinutes', label: t('entity.apsoperation.changeoverminutes') },
  { key: 'operationStatus', label: t('entity.apsoperation.operationstatus') },
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
  apsOrderCode: '',
  lineNumber: undefined as number | undefined,
  routingItemId: '',
  processCode: '',
  processName: '',
  workCenterCode: '',
  workCenterResourceId: '',
  plannedStartTimeStart: '',
  plannedStartTimeEnd: '',
  plannedEndTimeStart: '',
  plannedEndTimeEnd: '',
  plannedDurationMinutes: undefined as number | undefined,
  changeoverMinutes: undefined as number | undefined,
  operationStatus: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
}
const columnSettingVisible = ref(false)
/** 表格当前可见列 key（空数组时按 tableMode=masterDetailDetail 默认 id+4 业务列） */
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

const entityIdName = 'apsOperationId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.apsOrderId)
const masterApsOrderId = computed(() => selectedMasterRow.value?.apsOrderId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getApsOperationId(record: ApsOperation | Record<string, unknown>): string {
  return String((record as ApsOperation)?.[entityIdName] ?? '')
}

function getApsOperationField(record: ApsOperation | Record<string, unknown>, field: string): unknown {
  return (record as ApsOperation)?.[field as keyof ApsOperation]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'apsOperationId',
    key: 'apsOperationId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: ApsOperation }) =>
      String(getApsOperationField(record, 'apsOperationId') ?? ''),
  },
  {
    title: t('entity.apsoperation.apsordercode'),
    dataIndex: 'apsOrderCode',
    key: 'apsOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsOperation }) =>
      String(getApsOperationField(record, 'apsOrderCode') ?? ''),
  },
  {
    title: t('entity.apsoperation.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsOperation }) =>
      String(getApsOperationField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.apsoperation.routingitemid'),
    dataIndex: 'routingItemId',
    key: 'routingItemId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsOperation }) =>
      String(getApsOperationField(record, 'routingItemId') ?? ''),
  },
  {
    title: t('entity.apsoperation.processcode'),
    dataIndex: 'processCode',
    key: 'processCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsOperation }) =>
      String(getApsOperationField(record, 'processCode') ?? ''),
  },
  {
    title: t('entity.apsoperation.processname'),
    dataIndex: 'processName',
    key: 'processName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsOperation }) =>
      String(getApsOperationField(record, 'processName') ?? ''),
  },
  {
    title: t('entity.apsoperation.workcentercode'),
    dataIndex: 'workCenterCode',
    key: 'workCenterCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsOperation }) =>
      String(getApsOperationField(record, 'workCenterCode') ?? ''),
  },
  {
    title: t('entity.apsoperation.workcenterresourceid'),
    dataIndex: 'workCenterResourceId',
    key: 'workCenterResourceId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsOperation }) =>
      String(getApsOperationField(record, 'workCenterResourceId') ?? ''),
  },
  {
    title: t('entity.apsoperation.plannedstarttime'),
    dataIndex: 'plannedStartTime',
    key: 'plannedStartTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsOperation }) =>
      String(getApsOperationField(record, 'plannedStartTime') ?? ''),
  },
  {
    title: t('entity.apsoperation.plannedendtime'),
    dataIndex: 'plannedEndTime',
    key: 'plannedEndTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsOperation }) =>
      String(getApsOperationField(record, 'plannedEndTime') ?? ''),
  },
  {
    title: t('entity.apsoperation.planneddurationminutes'),
    dataIndex: 'plannedDurationMinutes',
    key: 'plannedDurationMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsOperation }) =>
      String(getApsOperationField(record, 'plannedDurationMinutes') ?? ''),
  },
  {
    title: t('entity.apsoperation.changeoverminutes'),
    dataIndex: 'changeoverMinutes',
    key: 'changeoverMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsOperation }) =>
      String(getApsOperationField(record, 'changeoverMinutes') ?? ''),
  },
  {
    title: t('entity.apsoperation.operationstatus'),
    dataIndex: 'operationStatus',
    key: 'operationStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsOperation }) =>
      String(getApsOperationField(record, 'operationStatus') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:aps:schedule:update',
        onClick: (record: ApsOperation) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:aps:schedule:delete',
        onClick: (record: ApsOperation) => void handleDeleteOne(record),
      }],
  })])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: ApsOperation[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: ApsOperation, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getApsOperationId(selectedRow.value) === getApsOperationId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: ApsOperation[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: ApsOperation) {
  const key = getApsOperationId(record)
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
 * @returns {ApsOperationQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<ApsOperationQuery>): ApsOperationQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: ApsOperationQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    apsOrderId: masterApsOrderId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof ApsOperationQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('apsOrderCode', form.apsOrderCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('routingItemId', form.routingItemId)
  assignTrimmed('processCode', form.processCode)
  assignTrimmed('processName', form.processName)
  assignTrimmed('workCenterCode', form.workCenterCode)
  assignTrimmed('workCenterResourceId', form.workCenterResourceId)
  assignTrimmed('plannedStartTimeStart', form.plannedStartTimeStart)
  assignTrimmed('plannedStartTimeEnd', form.plannedStartTimeEnd)
  assignTrimmed('plannedEndTimeStart', form.plannedEndTimeStart)
  assignTrimmed('plannedEndTimeEnd', form.plannedEndTimeEnd)
  if (form.plannedDurationMinutes !== undefined && form.plannedDurationMinutes !== null) {
    query.plannedDurationMinutes = form.plannedDurationMinutes
  }
  if (form.changeoverMinutes !== undefined && form.changeoverMinutes !== null) {
    query.changeoverMinutes = form.changeoverMinutes
  }
  if (form.operationStatus !== undefined && form.operationStatus !== null) {
    query.operationStatus = form.operationStatus
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
    const res = await getApsOperationList(buildListQuery())
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
watch(masterApsOrderId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.apsoperation._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: ApsOperation) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.apsoperation._self') })
  formLoading.value = true
  try {
    const detail = await getApsOperationById(getApsOperationId(record))
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
      entity: t('entity.apsoperation._self'),
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
    const id = formData.value?.apsOperationId
    if (id) {
      await updateApsOperation(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.apsoperation._self') }))
    } else {
      await createApsOperation(payload)
      message.success(t('common.feedback.created', { target: t('entity.apsoperation._self') }))
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

async function handleDeleteOne(record: ApsOperation) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.apsoperation._self'),
      name: t('common.tip.this.target', { target: t('entity.apsoperation._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteApsOperationById(getApsOperationId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.apsoperation._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.apsoperation._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.apsoperation._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getApsOperationId(r)).filter(Boolean)
      await deleteApsOperationBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.apsoperation._self') }))
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
  const res = await getApsOperationTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importApsOperation(file, sheetName)
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
    const exportMeta = await exportApsOperation(
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
    message.success(t('common.feedback.export.success', { target: t('entity.apsoperation._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.apsoperation._self') }))
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

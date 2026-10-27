<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/incident-item/components -->
<!-- 文件名称：IncidentItemPanel.vue -->
<!-- 功能描述：品质事故主表主表实体右侧明细 qualityIncidentItem 独立 CRUD（按主表选中 qualityIncidentId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="incident-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.qualityincidentitem._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:quality:cost:incident:create"
      update-permission="logistics:quality:cost:incident:update"
      delete-permission="logistics:quality:cost:incident:delete"
      import-permission="logistics:quality:cost:incident:import"
      export-permission="logistics:quality:cost:incident:export"
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
    <div class="incident-item-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getQualityIncidentItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="qualityIncidentItemId"
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
      <QualityIncidentItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterQualityIncidentId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-quality-cost-incident-item-incident-item"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('qualityIncidentCode')">
      <a-form-item :label="t('entity.qualityincidentitem.qualityincidentcode')">
        <a-input
          v-model:value="advancedQueryForm.qualityIncidentCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityincidentitem.qualityincidentcode') })"
          show-count
          :maxlength="30"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.qualityincidentitem.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityincidentitem.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="t('entity.qualityincidentitem.materialcode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityincidentitem.materialcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialDescription')">
      <a-form-item :label="t('entity.qualityincidentitem.materialdescription')">
        <a-input
          v-model:value="advancedQueryForm.materialDescription"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityincidentitem.materialdescription') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scrapCost')">
      <a-form-item :label="t('entity.qualityincidentitem.scrapcost')">
        <a-input-number
          v-model:value="advancedQueryForm.scrapCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityincidentitem.scrapcost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scrapSize')">
      <a-form-item :label="t('entity.qualityincidentitem.scrapsize')">
        <a-input-number
          v-model:value="advancedQueryForm.scrapSize"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityincidentitem.scrapsize') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('partPrice')">
      <a-form-item :label="t('entity.qualityincidentitem.partprice')">
        <a-input-number
          v-model:value="advancedQueryForm.partPrice"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityincidentitem.partprice') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scrapReasonCost')">
      <a-form-item :label="t('entity.qualityincidentitem.scrapreasoncost')">
        <a-input-number
          v-model:value="advancedQueryForm.scrapReasonCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityincidentitem.scrapreasoncost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('freightCharges')">
      <a-form-item :label="t('entity.qualityincidentitem.freightcharges')">
        <a-input-number
          v-model:value="advancedQueryForm.freightCharges"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityincidentitem.freightcharges') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('otherExpenses')">
      <a-form-item :label="t('entity.qualityincidentitem.otherexpenses')">
        <a-input-number
          v-model:value="advancedQueryForm.otherExpenses"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityincidentitem.otherexpenses') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reasonWorkTimeMinutes')">
      <a-form-item :label="t('entity.qualityincidentitem.reasonworktimeminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.reasonWorkTimeMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityincidentitem.reasonworktimeminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('tax')">
      <a-form-item :label="t('entity.qualityincidentitem.tax')">
        <a-input-number
          v-model:value="advancedQueryForm.tax"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityincidentitem.tax') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reasonOtherExpenses')">
      <a-form-item :label="t('entity.qualityincidentitem.reasonotherexpenses')">
        <a-input-number
          v-model:value="advancedQueryForm.reasonOtherExpenses"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityincidentitem.reasonotherexpenses') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scrapNote')">
      <a-form-item :label="t('entity.qualityincidentitem.scrapnote')">
        <a-textarea
          v-model:value="advancedQueryForm.scrapNote"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityincidentitem.scrapnote') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.qualityincidentitem._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.qualityincidentitem._self"
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
      id-column-key="qualityIncidentItemId"
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
 * 品质事故主表子表 qualityIncidentItem 右栏面板
 * @module views/logistics/quality/cost/incident-item/components
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
import QualityIncidentItemForm from './IncidentItemForm.vue'
import { useQualityIncidentMasterContext } from '../composables/useIncidentMasterContext'
import {
  getQualityIncidentItemList,
  getQualityIncidentItemById,
  createQualityIncidentItem,
  updateQualityIncidentItem,
  deleteQualityIncidentItemById,
  deleteQualityIncidentItemBatch,
  getQualityIncidentItemTemplate,
  importQualityIncidentItem,
  exportQualityIncidentItem,
} from '@/api/logistics/quality/cost/incidentItem.api'
import type { QualityIncidentItem, QualityIncidentItemQuery } from '@/types/logistics/quality/cost/incidentItem.types'

const { t } = useI18n()
const { selecmedMasterRow } = useQualityIncidentMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktQualityIncidentItem')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.qualityincidentitem._self') }),
)

const loading = ref(false)
const dataSource = ref<QualityIncidentItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selecmedRow = ref<QualityIncidentItem | null>(null)
const selecmedRows = ref<QualityIncidentItem[]>([])
const selecmedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<QualityIncidentItem>>({})
const formLoading = ref(false)
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  qualityIncidentCode: '',
  lineNumber: undefined as number | undefined,
  materialCode: '',
  materialDescription: '',
  scrapCost: undefined as number | undefined,
  scrapSize: undefined as number | undefined,
  partPrice: undefined as number | undefined,
  scrapReasonCost: undefined as number | undefined,
  freightCharges: undefined as number | undefined,
  otherExpenses: undefined as number | undefined,
  reasonWorkTimeMinutes: undefined as number | undefined,
  tax: undefined as number | undefined,
  reasonOtherExpenses: undefined as number | undefined,
  scrapNote: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'qualityIncidentCode', label: t('entity.qualityincidentitem.qualityincidentcode') },
  { key: 'lineNumber', label: t('entity.qualityincidentitem.linenumber') },
  { key: 'materialCode', label: t('entity.qualityincidentitem.materialcode') },
  { key: 'materialDescription', label: t('entity.qualityincidentitem.materialdescription') },
  { key: 'scrapCost', label: t('entity.qualityincidentitem.scrapcost') },
  { key: 'scrapSize', label: t('entity.qualityincidentitem.scrapsize') },
  { key: 'partPrice', label: t('entity.qualityincidentitem.partprice') },
  { key: 'scrapReasonCost', label: t('entity.qualityincidentitem.scrapreasoncost') },
  { key: 'freightCharges', label: t('entity.qualityincidentitem.freightcharges') },
  { key: 'otherExpenses', label: t('entity.qualityincidentitem.otherexpenses') },
  { key: 'reasonWorkTimeMinutes', label: t('entity.qualityincidentitem.reasonworktimeminutes') },
  { key: 'tax', label: t('entity.qualityincidentitem.tax') },
  { key: 'reasonOtherExpenses', label: t('entity.qualityincidentitem.reasonotherexpenses') },
  { key: 'scrapNote', label: t('entity.qualityincidentitem.scrapnote') },
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
  qualityIncidentCode: '',
  lineNumber: undefined as number | undefined,
  materialCode: '',
  materialDescription: '',
  scrapCost: undefined as number | undefined,
  scrapSize: undefined as number | undefined,
  partPrice: undefined as number | undefined,
  scrapReasonCost: undefined as number | undefined,
  freightCharges: undefined as number | undefined,
  otherExpenses: undefined as number | undefined,
  reasonWorkTimeMinutes: undefined as number | undefined,
  tax: undefined as number | undefined,
  reasonOtherExpenses: undefined as number | undefined,
  scrapNote: '',
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

const entityIdName = 'qualityIncidentItemId'
const hasMasterSelection = computed(() => !!selecmedMasterRow.value?.qualityIncidentId)
const masterQualityIncidentId = computed(() => selecmedMasterRow.value?.qualityIncidentId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selecmedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selecmedRows.value.length === 0)

function getQualityIncidentItemId(record: QualityIncidentItem | Record<string, unknown>): string {
  return String((record as QualityIncidentItem)?.[entityIdName] ?? '')
}

function getQualityIncidentItemField(record: QualityIncidentItem | Record<string, unknown>, field: string): unknown {
  return (record as QualityIncidentItem)?.[field as keyof QualityIncidentItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'qualityIncidentItemId',
    key: 'qualityIncidentItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: QualityIncidentItem }) =>
      String(getQualityIncidentItemField(record, 'qualityIncidentItemId') ?? ''),
  },
  {
    title: t('entity.qualityincidentitem.qualityincidentcode'),
    dataIndex: 'qualityIncidentCode',
    key: 'qualityIncidentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityIncidentItem }) =>
      String(getQualityIncidentItemField(record, 'qualityIncidentCode') ?? ''),
  },
  {
    title: t('entity.qualityincidentitem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityIncidentItem }) =>
      String(getQualityIncidentItemField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.qualityincidentitem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityIncidentItem }) =>
      String(getQualityIncidentItemField(record, 'materialCode') ?? ''),
  },
  {
    title: t('entity.qualityincidentitem.materialdescription'),
    dataIndex: 'materialDescription',
    key: 'materialDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityIncidentItem }) =>
      String(getQualityIncidentItemField(record, 'materialDescription') ?? ''),
  },
  {
    title: t('entity.qualityincidentitem.scrapcost'),
    dataIndex: 'scrapCost',
    key: 'scrapCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityIncidentItem }) =>
      String(getQualityIncidentItemField(record, 'scrapCost') ?? ''),
  },
  {
    title: t('entity.qualityincidentitem.scrapsize'),
    dataIndex: 'scrapSize',
    key: 'scrapSize',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityIncidentItem }) =>
      String(getQualityIncidentItemField(record, 'scrapSize') ?? ''),
  },
  {
    title: t('entity.qualityincidentitem.partprice'),
    dataIndex: 'partPrice',
    key: 'partPrice',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityIncidentItem }) =>
      String(getQualityIncidentItemField(record, 'partPrice') ?? ''),
  },
  {
    title: t('entity.qualityincidentitem.scrapreasoncost'),
    dataIndex: 'scrapReasonCost',
    key: 'scrapReasonCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityIncidentItem }) =>
      String(getQualityIncidentItemField(record, 'scrapReasonCost') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:cost:incident:update',
        onClick: (record: QualityIncidentItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:cost:incident:delete',
        onClick: (record: QualityIncidentItem) => void handleDeleteOne(record),
      }],
  })])

const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: QualityIncidentItem[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: QualityIncidentItem, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (getQualityIncidentItemId(selecmedRow.value) === getQualityIncidentItemId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: QualityIncidentItem[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: QualityIncidentItem) {
  const key = getQualityIncidentItemId(record)
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
 * @returns {QualityIncidentItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<QualityIncidentItemQuery>): QualityIncidentItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: QualityIncidentItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    qualityIncidentId: masterQualityIncidentId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof QualityIncidentItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('qualityIncidentCode', form.qualityIncidentCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('materialCode', form.materialCode)
  assignTrimmed('materialDescription', form.materialDescription)
  if (form.scrapCost !== undefined && form.scrapCost !== null) {
    query.scrapCost = form.scrapCost
  }
  if (form.scrapSize !== undefined && form.scrapSize !== null) {
    query.scrapSize = form.scrapSize
  }
  if (form.partPrice !== undefined && form.partPrice !== null) {
    query.partPrice = form.partPrice
  }
  if (form.scrapReasonCost !== undefined && form.scrapReasonCost !== null) {
    query.scrapReasonCost = form.scrapReasonCost
  }
  if (form.freightCharges !== undefined && form.freightCharges !== null) {
    query.freightCharges = form.freightCharges
  }
  if (form.otherExpenses !== undefined && form.otherExpenses !== null) {
    query.otherExpenses = form.otherExpenses
  }
  if (form.reasonWorkTimeMinutes !== undefined && form.reasonWorkTimeMinutes !== null) {
    query.reasonWorkTimeMinutes = form.reasonWorkTimeMinutes
  }
  if (form.tax !== undefined && form.tax !== null) {
    query.tax = form.tax
  }
  if (form.reasonOtherExpenses !== undefined && form.reasonOtherExpenses !== null) {
    query.reasonOtherExpenses = form.reasonOtherExpenses
  }
  assignTrimmed('scrapNote', form.scrapNote)
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
    const res = await getQualityIncidentItemList(buildListQuery())
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
watch(masterQualityIncidentId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.qualityincidentitem._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: QualityIncidentItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.qualityincidentitem._self') })
  formLoading.value = true
  try {
    const detail = await getQualityIncidentItemById(getQualityIncidentItemId(record))
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
      entity: t('entity.qualityincidentitem._self'),
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
    const id = formData.value?.qualityIncidentItemId
    if (id) {
      await updateQualityIncidentItem(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.qualityincidentitem._self') }))
    } else {
      await createQualityIncidentItem(payload)
      message.success(t('common.feedback.created', { target: t('entity.qualityincidentitem._self') }))
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

async function handleDeleteOne(record: QualityIncidentItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.qualityincidentitem._self'),
      name: t('common.tip.this.target', { target: t('entity.qualityincidentitem._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteQualityIncidentItemById(getQualityIncidentItemId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.qualityincidentitem._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.qualityincidentitem._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.qualityincidentitem._self'),
      count: selecmedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r) => getQualityIncidentItemId(r)).filter(Boolean)
      await deleteQualityIncidentItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.qualityincidentitem._self') }))
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
  const res = await getQualityIncidentItemTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importQualityIncidentItem(file, sheetName)
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
    const exportMeta = await exportQualityIncidentItem(
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
    message.success(t('common.feedback.export.success', { target: t('entity.qualityincidentitem._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.qualityincidentitem._self') }))
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

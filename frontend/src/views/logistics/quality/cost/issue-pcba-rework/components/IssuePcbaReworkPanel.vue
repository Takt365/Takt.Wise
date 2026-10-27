<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/issue-pcba-rework/components -->
<!-- 文件名称：IssuePcbaReworkPanel.vue -->
<!-- 功能描述：品质问题应对主表主表实体右侧明细 qualityIssuePcbaRework 独立 CRUD（按主表选中 qualityIssueId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="issue-pcba-rework-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.qualityissuepcbarework._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:quality:cost:issue:create"
      update-permission="logistics:quality:cost:issue:update"
      delete-permission="logistics:quality:cost:issue:delete"
      import-permission="logistics:quality:cost:issue:import"
      export-permission="logistics:quality:cost:issue:export"
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
    <div class="issue-pcba-rework-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getQualityIssuePcbaReworkId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="qualityIssuePcbaReworkId"
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
      <QualityIssuePcbaReworkForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterQualityIssueId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-quality-cost-issue-pcba-rework-issue-pcba-rework"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('qualityIssueCode')">
      <a-form-item :label="t('entity.qualityissuepcbarework.qualityissuecode')">
        <a-input
          v-model:value="advancedQueryForm.qualityIssueCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuepcbarework.qualityissuecode') })"
          show-count
          :maxlength="30"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.qualityissuepcbarework.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuepcbarework.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pcbaDefectParts')">
      <a-form-item :label="t('entity.qualityissuepcbarework.pcbadefectparts')">
        <a-input
          v-model:value="advancedQueryForm.pcbaDefectParts"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuepcbarework.pcbadefectparts') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pcbaReworkCost')">
      <a-form-item :label="t('entity.qualityissuepcbarework.pcbareworkcost')">
        <a-input-number
          v-model:value="advancedQueryForm.pcbaReworkCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuepcbarework.pcbareworkcost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pcbaReworkTimeMinutes')">
      <a-form-item :label="t('entity.qualityissuepcbarework.pcbareworktimeminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.pcbaReworkTimeMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuepcbarework.pcbareworktimeminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pcbaReinspectionTimeMinutes')">
      <a-form-item :label="t('entity.qualityissuepcbarework.pcbareinspectiontimeminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.pcbaReinspectionTimeMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuepcbarework.pcbareinspectiontimeminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pcbaTravelCost')">
      <a-form-item :label="t('entity.qualityissuepcbarework.pcbatravelcost')">
        <a-input-number
          v-model:value="advancedQueryForm.pcbaTravelCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuepcbarework.pcbatravelcost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pcbaWarehouseCost')">
      <a-form-item :label="t('entity.qualityissuepcbarework.pcbawarehousecost')">
        <a-input-number
          v-model:value="advancedQueryForm.pcbaWarehouseCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuepcbarework.pcbawarehousecost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pcbaOtherExpenses')">
      <a-form-item :label="t('entity.qualityissuepcbarework.pcbaotherexpenses')">
        <a-input-number
          v-model:value="advancedQueryForm.pcbaOtherExpenses"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuepcbarework.pcbaotherexpenses') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pcbaReworkNote')">
      <a-form-item :label="t('entity.qualityissuepcbarework.pcbareworknote')">
        <a-textarea
          v-model:value="advancedQueryForm.pcbaReworkNote"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityissuepcbarework.pcbareworknote') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pcbaScrapCost')">
      <a-form-item :label="t('entity.qualityissuepcbarework.pcbascrapcost')">
        <a-input-number
          v-model:value="advancedQueryForm.pcbaScrapCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuepcbarework.pcbascrapcost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pcbaCustomerName')">
      <a-form-item :label="t('entity.qualityissuepcbarework.pcbacustomername')">
        <a-input
          v-model:value="advancedQueryForm.pcbaCustomerName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuepcbarework.pcbacustomername') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pcbaDebitNoteCode')">
      <a-form-item :label="t('entity.qualityissuepcbarework.pcbadebitnoteCode')">
        <a-textarea
          v-model:value="advancedQueryForm.pcbaDebitNoteCode"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityissuepcbarework.pcbadebitnoteCode') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pcbaOtherExpenses2')">
      <a-form-item :label="t('entity.qualityissuepcbarework.pcbaotherexpenses2')">
        <a-input-number
          v-model:value="advancedQueryForm.pcbaOtherExpenses2"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuepcbarework.pcbaotherexpenses2') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pcbaNote')">
      <a-form-item :label="t('entity.qualityissuepcbarework.pcbanote')">
        <a-textarea
          v-model:value="advancedQueryForm.pcbaNote"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityissuepcbarework.pcbanote') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('responseGroupName')">
      <a-form-item :label="t('entity.qualityissuepcbarework.responsegroupname')">
        <a-input
          v-model:value="advancedQueryForm.responseGroupName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuepcbarework.responsegroupname') })"
          show-count
          :maxlength="20"
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
      :title="t('common.dialog.title.import', { entity: t('entity.qualityissuepcbarework._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.qualityissuepcbarework._self"
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
      id-column-key="qualityIssuePcbaReworkId"
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
 * 品质问题应对主表子表 qualityIssuePcbaRework 右栏面板
 * @module views/logistics/quality/cost/issue-pcba-rework/components
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
import QualityIssuePcbaReworkForm from './IssuePcbaReworkForm.vue'
import { useQualityIssueMasterContext } from '../composables/useIssueMasterContext'
import {
  getQualityIssuePcbaReworkList,
  getQualityIssuePcbaReworkById,
  createQualityIssuePcbaRework,
  updateQualityIssuePcbaRework,
  deleteQualityIssuePcbaReworkById,
  deleteQualityIssuePcbaReworkBatch,
  getQualityIssuePcbaReworkTemplate,
  importQualityIssuePcbaRework,
  exportQualityIssuePcbaRework,
} from '@/api/logistics/quality/cost/issuePcbaRework.api'
import type { QualityIssuePcbaRework, QualityIssuePcbaReworkQuery } from '@/types/logistics/quality/cost/issuePcbaRework.types'

const { t } = useI18n()
const { selecmedMasterRow } = useQualityIssueMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktQualityIssuePcbaRework')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.qualityissuepcbarework._self') }),
)

const loading = ref(false)
const dataSource = ref<QualityIssuePcbaRework[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selecmedRow = ref<QualityIssuePcbaRework | null>(null)
const selecmedRows = ref<QualityIssuePcbaRework[]>([])
const selecmedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<QualityIssuePcbaRework>>({})
const formLoading = ref(false)
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  qualityIssueCode: '',
  lineNumber: undefined as number | undefined,
  pcbaDefectParts: '',
  pcbaReworkCost: undefined as number | undefined,
  pcbaReworkTimeMinutes: undefined as number | undefined,
  pcbaReinspectionTimeMinutes: undefined as number | undefined,
  pcbaTravelCost: undefined as number | undefined,
  pcbaWarehouseCost: undefined as number | undefined,
  pcbaOtherExpenses: undefined as number | undefined,
  pcbaReworkNote: '',
  pcbaScrapCost: undefined as number | undefined,
  pcbaCustomerName: '',
  pcbaDebitNoteCode: '',
  pcbaOtherExpenses2: undefined as number | undefined,
  pcbaNote: '',
  responseGroupName: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'qualityIssueCode', label: t('entity.qualityissuepcbarework.qualityissuecode') },
  { key: 'lineNumber', label: t('entity.qualityissuepcbarework.linenumber') },
  { key: 'pcbaDefectParts', label: t('entity.qualityissuepcbarework.pcbadefectparts') },
  { key: 'pcbaReworkCost', label: t('entity.qualityissuepcbarework.pcbareworkcost') },
  { key: 'pcbaReworkTimeMinutes', label: t('entity.qualityissuepcbarework.pcbareworktimeminutes') },
  { key: 'pcbaReinspectionTimeMinutes', label: t('entity.qualityissuepcbarework.pcbareinspectiontimeminutes') },
  { key: 'pcbaTravelCost', label: t('entity.qualityissuepcbarework.pcbatravelcost') },
  { key: 'pcbaWarehouseCost', label: t('entity.qualityissuepcbarework.pcbawarehousecost') },
  { key: 'pcbaOtherExpenses', label: t('entity.qualityissuepcbarework.pcbaotherexpenses') },
  { key: 'pcbaReworkNote', label: t('entity.qualityissuepcbarework.pcbareworknote') },
  { key: 'pcbaScrapCost', label: t('entity.qualityissuepcbarework.pcbascrapcost') },
  { key: 'pcbaCustomerName', label: t('entity.qualityissuepcbarework.pcbacustomername') },
  { key: 'pcbaDebitNoteCode', label: t('entity.qualityissuepcbarework.pcbadebitnoteCode') },
  { key: 'pcbaOtherExpenses2', label: t('entity.qualityissuepcbarework.pcbaotherexpenses2') },
  { key: 'pcbaNote', label: t('entity.qualityissuepcbarework.pcbanote') },
  { key: 'responseGroupName', label: t('entity.qualityissuepcbarework.responsegroupname') },
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
  qualityIssueCode: '',
  lineNumber: undefined as number | undefined,
  pcbaDefectParts: '',
  pcbaReworkCost: undefined as number | undefined,
  pcbaReworkTimeMinutes: undefined as number | undefined,
  pcbaReinspectionTimeMinutes: undefined as number | undefined,
  pcbaTravelCost: undefined as number | undefined,
  pcbaWarehouseCost: undefined as number | undefined,
  pcbaOtherExpenses: undefined as number | undefined,
  pcbaReworkNote: '',
  pcbaScrapCost: undefined as number | undefined,
  pcbaCustomerName: '',
  pcbaDebitNoteCode: '',
  pcbaOtherExpenses2: undefined as number | undefined,
  pcbaNote: '',
  responseGroupName: '',
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

const entityIdName = 'qualityIssuePcbaReworkId'
const hasMasterSelection = computed(() => !!selecmedMasterRow.value?.qualityIssueId)
const masterQualityIssueId = computed(() => selecmedMasterRow.value?.qualityIssueId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selecmedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selecmedRows.value.length === 0)

function getQualityIssuePcbaReworkId(record: QualityIssuePcbaRework | Record<string, unknown>): string {
  return String((record as QualityIssuePcbaRework)?.[entityIdName] ?? '')
}

function getQualityIssuePcbaReworkField(record: QualityIssuePcbaRework | Record<string, unknown>, field: string): unknown {
  return (record as QualityIssuePcbaRework)?.[field as keyof QualityIssuePcbaRework]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'qualityIssuePcbaReworkId',
    key: 'qualityIssuePcbaReworkId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: QualityIssuePcbaRework }) =>
      String(getQualityIssuePcbaReworkField(record, 'qualityIssuePcbaReworkId') ?? ''),
  },
  {
    title: t('entity.qualityissuepcbarework.qualityissuecode'),
    dataIndex: 'qualityIssueCode',
    key: 'qualityIssueCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityIssuePcbaRework }) =>
      String(getQualityIssuePcbaReworkField(record, 'qualityIssueCode') ?? ''),
  },
  {
    title: t('entity.qualityissuepcbarework.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityIssuePcbaRework }) =>
      String(getQualityIssuePcbaReworkField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.qualityissuepcbarework.pcbadefectparts'),
    dataIndex: 'pcbaDefectParts',
    key: 'pcbaDefectParts',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityIssuePcbaRework }) =>
      String(getQualityIssuePcbaReworkField(record, 'pcbaDefectParts') ?? ''),
  },
  {
    title: t('entity.qualityissuepcbarework.pcbareworkcost'),
    dataIndex: 'pcbaReworkCost',
    key: 'pcbaReworkCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityIssuePcbaRework }) =>
      String(getQualityIssuePcbaReworkField(record, 'pcbaReworkCost') ?? ''),
  },
  {
    title: t('entity.qualityissuepcbarework.pcbareworktimeminutes'),
    dataIndex: 'pcbaReworkTimeMinutes',
    key: 'pcbaReworkTimeMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityIssuePcbaRework }) =>
      String(getQualityIssuePcbaReworkField(record, 'pcbaReworkTimeMinutes') ?? ''),
  },
  {
    title: t('entity.qualityissuepcbarework.pcbareinspectiontimeminutes'),
    dataIndex: 'pcbaReinspectionTimeMinutes',
    key: 'pcbaReinspectionTimeMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityIssuePcbaRework }) =>
      String(getQualityIssuePcbaReworkField(record, 'pcbaReinspectionTimeMinutes') ?? ''),
  },
  {
    title: t('entity.qualityissuepcbarework.pcbatravelcost'),
    dataIndex: 'pcbaTravelCost',
    key: 'pcbaTravelCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityIssuePcbaRework }) =>
      String(getQualityIssuePcbaReworkField(record, 'pcbaTravelCost') ?? ''),
  },
  {
    title: t('entity.qualityissuepcbarework.pcbawarehousecost'),
    dataIndex: 'pcbaWarehouseCost',
    key: 'pcbaWarehouseCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityIssuePcbaRework }) =>
      String(getQualityIssuePcbaReworkField(record, 'pcbaWarehouseCost') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:cost:issue:update',
        onClick: (record: QualityIssuePcbaRework) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:cost:issue:delete',
        onClick: (record: QualityIssuePcbaRework) => void handleDeleteOne(record),
      }],
  })])

const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: QualityIssuePcbaRework[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: QualityIssuePcbaRework, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (getQualityIssuePcbaReworkId(selecmedRow.value) === getQualityIssuePcbaReworkId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: QualityIssuePcbaRework[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: QualityIssuePcbaRework) {
  const key = getQualityIssuePcbaReworkId(record)
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
 * @returns {QualityIssuePcbaReworkQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<QualityIssuePcbaReworkQuery>): QualityIssuePcbaReworkQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: QualityIssuePcbaReworkQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    qualityIssueId: masterQualityIssueId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof QualityIssuePcbaReworkQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('qualityIssueCode', form.qualityIssueCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('pcbaDefectParts', form.pcbaDefectParts)
  if (form.pcbaReworkCost !== undefined && form.pcbaReworkCost !== null) {
    query.pcbaReworkCost = form.pcbaReworkCost
  }
  if (form.pcbaReworkTimeMinutes !== undefined && form.pcbaReworkTimeMinutes !== null) {
    query.pcbaReworkTimeMinutes = form.pcbaReworkTimeMinutes
  }
  if (form.pcbaReinspectionTimeMinutes !== undefined && form.pcbaReinspectionTimeMinutes !== null) {
    query.pcbaReinspectionTimeMinutes = form.pcbaReinspectionTimeMinutes
  }
  if (form.pcbaTravelCost !== undefined && form.pcbaTravelCost !== null) {
    query.pcbaTravelCost = form.pcbaTravelCost
  }
  if (form.pcbaWarehouseCost !== undefined && form.pcbaWarehouseCost !== null) {
    query.pcbaWarehouseCost = form.pcbaWarehouseCost
  }
  if (form.pcbaOtherExpenses !== undefined && form.pcbaOtherExpenses !== null) {
    query.pcbaOtherExpenses = form.pcbaOtherExpenses
  }
  assignTrimmed('pcbaReworkNote', form.pcbaReworkNote)
  if (form.pcbaScrapCost !== undefined && form.pcbaScrapCost !== null) {
    query.pcbaScrapCost = form.pcbaScrapCost
  }
  assignTrimmed('pcbaCustomerName', form.pcbaCustomerName)
  assignTrimmed('pcbaDebitNoteCode', form.pcbaDebitNoteCode)
  if (form.pcbaOtherExpenses2 !== undefined && form.pcbaOtherExpenses2 !== null) {
    query.pcbaOtherExpenses2 = form.pcbaOtherExpenses2
  }
  assignTrimmed('pcbaNote', form.pcbaNote)
  assignTrimmed('responseGroupName', form.responseGroupName)
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
    const res = await getQualityIssuePcbaReworkList(buildListQuery())
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
watch(masterQualityIssueId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.qualityissuepcbarework._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: QualityIssuePcbaRework) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.qualityissuepcbarework._self') })
  formLoading.value = true
  try {
    const detail = await getQualityIssuePcbaReworkById(getQualityIssuePcbaReworkId(record))
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
      entity: t('entity.qualityissuepcbarework._self'),
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
    const id = formData.value?.qualityIssuePcbaReworkId
    if (id) {
      await updateQualityIssuePcbaRework(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.qualityissuepcbarework._self') }))
    } else {
      await createQualityIssuePcbaRework(payload)
      message.success(t('common.feedback.created', { target: t('entity.qualityissuepcbarework._self') }))
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

async function handleDeleteOne(record: QualityIssuePcbaRework) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.qualityissuepcbarework._self'),
      name: t('common.tip.this.target', { target: t('entity.qualityissuepcbarework._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteQualityIssuePcbaReworkById(getQualityIssuePcbaReworkId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.qualityissuepcbarework._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.qualityissuepcbarework._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.qualityissuepcbarework._self'),
      count: selecmedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r) => getQualityIssuePcbaReworkId(r)).filter(Boolean)
      await deleteQualityIssuePcbaReworkBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.qualityissuepcbarework._self') }))
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
  const res = await getQualityIssuePcbaReworkTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importQualityIssuePcbaRework(file, sheetName)
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
    const exportMeta = await exportQualityIssuePcbaRework(
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
    message.success(t('common.feedback.export.success', { target: t('entity.qualityissuepcbarework._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.qualityissuepcbarework._self') }))
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

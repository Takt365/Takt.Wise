<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/issue-meeting/components -->
<!-- 文件名称：IssueMeetingPanel.vue -->
<!-- 功能描述：品质问题应对主表主表实体右侧明细 qualityIssueMeeting 独立 CRUD（按主表选中 qualityIssueId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="issue-meeting-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.qualityissuemeeting._self') }}
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
    <div class="issue-meeting-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getQualityIssueMeetingId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="qualityIssueMeetingId"
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
      <QualityIssueMeetingForm
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
      storage-key="takt-query-fields-logistics-quality-cost-issue-meeting-issue-meeting"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('qualityIssueCode')">
      <a-form-item :label="t('entity.qualityissuemeeting.qualityissuecode')">
        <a-input
          v-model:value="advancedQueryForm.qualityIssueCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuemeeting.qualityissuecode') })"
          show-count
          :maxlength="30"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.qualityissuemeeting.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuemeeting.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('directManpowerCostPerMinute')">
      <a-form-item :label="t('entity.qualityissuemeeting.directmanpowercostperminute')">
        <a-input-number
          v-model:value="advancedQueryForm.directManpowerCostPerMinute"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuemeeting.directmanpowercostperminute') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('indirectManpowerCostPerMinute')">
      <a-form-item :label="t('entity.qualityissuemeeting.indirectmanpowercostperminute')">
        <a-input-number
          v-model:value="advancedQueryForm.indirectManpowerCostPerMinute"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuemeeting.indirectmanpowercostperminute') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('meetingInvestigationContent')">
      <a-form-item :label="t('entity.qualityissuemeeting.meetinginvestigationcontent')">
        <a-textarea
          v-model:value="advancedQueryForm.meetingInvestigationContent"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityissuemeeting.meetinginvestigationcontent') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('meetingInvestigationCost')">
      <a-form-item :label="t('entity.qualityissuemeeting.meetinginvestigationcost')">
        <a-input-number
          v-model:value="advancedQueryForm.meetingInvestigationCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuemeeting.meetinginvestigationcost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('meetingTimeMinutes')">
      <a-form-item :label="t('entity.qualityissuemeeting.meetingtimeminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.meetingTimeMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuemeeting.meetingtimeminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('directParticipantCount')">
      <a-form-item :label="t('entity.qualityissuemeeting.directparticipantcount')">
        <a-input-number
          v-model:value="advancedQueryForm.directParticipantCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuemeeting.directparticipantcount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('indirectParticipantCount')">
      <a-form-item :label="t('entity.qualityissuemeeting.indirectparticipantcount')">
        <a-input-number
          v-model:value="advancedQueryForm.indirectParticipantCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuemeeting.indirectparticipantcount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('investigationWorkTimeMinutes')">
      <a-form-item :label="t('entity.qualityissuemeeting.investigationworktimeminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.investigationWorkTimeMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuemeeting.investigationworktimeminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('travelCost')">
      <a-form-item :label="t('entity.qualityissuemeeting.travelcost')">
        <a-input-number
          v-model:value="advancedQueryForm.travelCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuemeeting.travelcost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('otherExpenses')">
      <a-form-item :label="t('entity.qualityissuemeeting.otherexpenses')">
        <a-input-number
          v-model:value="advancedQueryForm.otherExpenses"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuemeeting.otherexpenses') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('otherWorkTimeMinutes')">
      <a-form-item :label="t('entity.qualityissuemeeting.otherworktimeminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.otherWorkTimeMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuemeeting.otherworktimeminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('otherApparatusCost')">
      <a-form-item :label="t('entity.qualityissuemeeting.otherapparatuscost')">
        <a-input-number
          v-model:value="advancedQueryForm.otherApparatusCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuemeeting.otherapparatuscost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('responseGroupName')">
      <a-form-item :label="t('entity.qualityissuemeeting.responsegroupname')">
        <a-input
          v-model:value="advancedQueryForm.responseGroupName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuemeeting.responsegroupname') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.qualityissuemeeting._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.qualityissuemeeting._self"
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
      id-column-key="qualityIssueMeetingId"
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
 * 品质问题应对主表子表 qualityIssueMeeting 右栏面板
 * @module views/logistics/quality/cost/issue-meeting/components
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
import QualityIssueMeetingForm from './IssueMeetingForm.vue'
import { useQualityIssueMasterContext } from '../composables/useIssueMasterContext'
import {
  getQualityIssueMeetingList,
  getQualityIssueMeetingById,
  createQualityIssueMeeting,
  updateQualityIssueMeeting,
  deleteQualityIssueMeetingById,
  deleteQualityIssueMeetingBatch,
  getQualityIssueMeetingTemplate,
  importQualityIssueMeeting,
  exportQualityIssueMeeting,
} from '@/api/logistics/quality/cost/issueMeeting.api'
import type { QualityIssueMeeting, QualityIssueMeetingQuery } from '@/types/logistics/quality/cost/issueMeeting.types'

const { t } = useI18n()
const { selectedMasterRow } = useQualityIssueMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktQualityIssueMeeting')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.qualityissuemeeting._self') }),
)

const loading = ref(false)
const dataSource = ref<QualityIssueMeeting[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<QualityIssueMeeting | null>(null)
const selectedRows = ref<QualityIssueMeeting[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<QualityIssueMeeting>>({})
const formLoading = ref(false)
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  qualityIssueCode: '',
  lineNumber: undefined as number | undefined,
  directManpowerCostPerMinute: undefined as number | undefined,
  indirectManpowerCostPerMinute: undefined as number | undefined,
  meetingInvestigationContent: '',
  meetingInvestigationCost: undefined as number | undefined,
  meetingTimeMinutes: undefined as number | undefined,
  directParticipantCount: undefined as number | undefined,
  indirectParticipantCount: undefined as number | undefined,
  investigationWorkTimeMinutes: undefined as number | undefined,
  travelCost: undefined as number | undefined,
  otherExpenses: undefined as number | undefined,
  otherWorkTimeMinutes: undefined as number | undefined,
  otherApparatusCost: undefined as number | undefined,
  responseGroupName: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'qualityIssueCode', label: t('entity.qualityissuemeeting.qualityissuecode') },
  { key: 'lineNumber', label: t('entity.qualityissuemeeting.linenumber') },
  { key: 'directManpowerCostPerMinute', label: t('entity.qualityissuemeeting.directmanpowercostperminute') },
  { key: 'indirectManpowerCostPerMinute', label: t('entity.qualityissuemeeting.indirectmanpowercostperminute') },
  { key: 'meetingInvestigationContent', label: t('entity.qualityissuemeeting.meetinginvestigationcontent') },
  { key: 'meetingInvestigationCost', label: t('entity.qualityissuemeeting.meetinginvestigationcost') },
  { key: 'meetingTimeMinutes', label: t('entity.qualityissuemeeting.meetingtimeminutes') },
  { key: 'directParticipantCount', label: t('entity.qualityissuemeeting.directparticipantcount') },
  { key: 'indirectParticipantCount', label: t('entity.qualityissuemeeting.indirectparticipantcount') },
  { key: 'investigationWorkTimeMinutes', label: t('entity.qualityissuemeeting.investigationworktimeminutes') },
  { key: 'travelCost', label: t('entity.qualityissuemeeting.travelcost') },
  { key: 'otherExpenses', label: t('entity.qualityissuemeeting.otherexpenses') },
  { key: 'otherWorkTimeMinutes', label: t('entity.qualityissuemeeting.otherworktimeminutes') },
  { key: 'otherApparatusCost', label: t('entity.qualityissuemeeting.otherapparatuscost') },
  { key: 'responseGroupName', label: t('entity.qualityissuemeeting.responsegroupname') },
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
  directManpowerCostPerMinute: undefined as number | undefined,
  indirectManpowerCostPerMinute: undefined as number | undefined,
  meetingInvestigationContent: '',
  meetingInvestigationCost: undefined as number | undefined,
  meetingTimeMinutes: undefined as number | undefined,
  directParticipantCount: undefined as number | undefined,
  indirectParticipantCount: undefined as number | undefined,
  investigationWorkTimeMinutes: undefined as number | undefined,
  travelCost: undefined as number | undefined,
  otherExpenses: undefined as number | undefined,
  otherWorkTimeMinutes: undefined as number | undefined,
  otherApparatusCost: undefined as number | undefined,
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

const entityIdName = 'qualityIssueMeetingId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.qualityIssueId)
const masterQualityIssueId = computed(() => selectedMasterRow.value?.qualityIssueId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getQualityIssueMeetingId(record: QualityIssueMeeting | Record<string, unknown>): string {
  return String((record as QualityIssueMeeting)?.[entityIdName] ?? '')
}

function getQualityIssueMeetingField(record: QualityIssueMeeting | Record<string, unknown>, field: string): unknown {
  return (record as QualityIssueMeeting)?.[field as keyof QualityIssueMeeting]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'qualityIssueMeetingId',
    key: 'qualityIssueMeetingId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: QualityIssueMeeting }) =>
      String(getQualityIssueMeetingField(record, 'qualityIssueMeetingId') ?? ''),
  },
  {
    title: t('entity.qualityissuemeeting.qualityissuecode'),
    dataIndex: 'qualityIssueCode',
    key: 'qualityIssueCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityIssueMeeting }) =>
      String(getQualityIssueMeetingField(record, 'qualityIssueCode') ?? ''),
  },
  {
    title: t('entity.qualityissuemeeting.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityIssueMeeting }) =>
      String(getQualityIssueMeetingField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.qualityissuemeeting.directmanpowercostperminute'),
    dataIndex: 'directManpowerCostPerMinute',
    key: 'directManpowerCostPerMinute',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityIssueMeeting }) =>
      String(getQualityIssueMeetingField(record, 'directManpowerCostPerMinute') ?? ''),
  },
  {
    title: t('entity.qualityissuemeeting.indirectmanpowercostperminute'),
    dataIndex: 'indirectManpowerCostPerMinute',
    key: 'indirectManpowerCostPerMinute',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityIssueMeeting }) =>
      String(getQualityIssueMeetingField(record, 'indirectManpowerCostPerMinute') ?? ''),
  },
  {
    title: t('entity.qualityissuemeeting.meetinginvestigationcontent'),
    dataIndex: 'meetingInvestigationContent',
    key: 'meetingInvestigationContent',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityIssueMeeting }) =>
      String(getQualityIssueMeetingField(record, 'meetingInvestigationContent') ?? ''),
  },
  {
    title: t('entity.qualityissuemeeting.meetinginvestigationcost'),
    dataIndex: 'meetingInvestigationCost',
    key: 'meetingInvestigationCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityIssueMeeting }) =>
      String(getQualityIssueMeetingField(record, 'meetingInvestigationCost') ?? ''),
  },
  {
    title: t('entity.qualityissuemeeting.meetingtimeminutes'),
    dataIndex: 'meetingTimeMinutes',
    key: 'meetingTimeMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityIssueMeeting }) =>
      String(getQualityIssueMeetingField(record, 'meetingTimeMinutes') ?? ''),
  },
  {
    title: t('entity.qualityissuemeeting.directparticipantcount'),
    dataIndex: 'directParticipantCount',
    key: 'directParticipantCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: QualityIssueMeeting }) =>
      String(getQualityIssueMeetingField(record, 'directParticipantCount') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:cost:issue:update',
        onClick: (record: QualityIssueMeeting) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:cost:issue:delete',
        onClick: (record: QualityIssueMeeting) => void handleDeleteOne(record),
      }],
  })])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: QualityIssueMeeting[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: QualityIssueMeeting, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getQualityIssueMeetingId(selectedRow.value) === getQualityIssueMeetingId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: QualityIssueMeeting[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: QualityIssueMeeting) {
  const key = getQualityIssueMeetingId(record)
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
 * @returns {QualityIssueMeetingQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<QualityIssueMeetingQuery>): QualityIssueMeetingQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: QualityIssueMeetingQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    qualityIssueId: masterQualityIssueId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof QualityIssueMeetingQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('qualityIssueCode', form.qualityIssueCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  if (form.directManpowerCostPerMinute !== undefined && form.directManpowerCostPerMinute !== null) {
    query.directManpowerCostPerMinute = form.directManpowerCostPerMinute
  }
  if (form.indirectManpowerCostPerMinute !== undefined && form.indirectManpowerCostPerMinute !== null) {
    query.indirectManpowerCostPerMinute = form.indirectManpowerCostPerMinute
  }
  assignTrimmed('meetingInvestigationContent', form.meetingInvestigationContent)
  if (form.meetingInvestigationCost !== undefined && form.meetingInvestigationCost !== null) {
    query.meetingInvestigationCost = form.meetingInvestigationCost
  }
  if (form.meetingTimeMinutes !== undefined && form.meetingTimeMinutes !== null) {
    query.meetingTimeMinutes = form.meetingTimeMinutes
  }
  if (form.directParticipantCount !== undefined && form.directParticipantCount !== null) {
    query.directParticipantCount = form.directParticipantCount
  }
  if (form.indirectParticipantCount !== undefined && form.indirectParticipantCount !== null) {
    query.indirectParticipantCount = form.indirectParticipantCount
  }
  if (form.investigationWorkTimeMinutes !== undefined && form.investigationWorkTimeMinutes !== null) {
    query.investigationWorkTimeMinutes = form.investigationWorkTimeMinutes
  }
  if (form.travelCost !== undefined && form.travelCost !== null) {
    query.travelCost = form.travelCost
  }
  if (form.otherExpenses !== undefined && form.otherExpenses !== null) {
    query.otherExpenses = form.otherExpenses
  }
  if (form.otherWorkTimeMinutes !== undefined && form.otherWorkTimeMinutes !== null) {
    query.otherWorkTimeMinutes = form.otherWorkTimeMinutes
  }
  if (form.otherApparatusCost !== undefined && form.otherApparatusCost !== null) {
    query.otherApparatusCost = form.otherApparatusCost
  }
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
    selectedRowKeys.value = []
    selectedRows.value = []
    selectedRow.value = null
    return
  }
  loading.value = true
  try {
    const res = await getQualityIssueMeetingList(buildListQuery())
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.qualityissuemeeting._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: QualityIssueMeeting) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.qualityissuemeeting._self') })
  formLoading.value = true
  try {
    const detail = await getQualityIssueMeetingById(getQualityIssueMeetingId(record))
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
      entity: t('entity.qualityissuemeeting._self'),
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
    const id = formData.value?.qualityIssueMeetingId
    if (id) {
      await updateQualityIssueMeeting(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.qualityissuemeeting._self') }))
    } else {
      await createQualityIssueMeeting(payload)
      message.success(t('common.feedback.created', { target: t('entity.qualityissuemeeting._self') }))
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

async function handleDeleteOne(record: QualityIssueMeeting) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.qualityissuemeeting._self'),
      name: t('common.tip.this.target', { target: t('entity.qualityissuemeeting._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteQualityIssueMeetingById(getQualityIssueMeetingId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.qualityissuemeeting._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.qualityissuemeeting._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.qualityissuemeeting._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getQualityIssueMeetingId(r)).filter(Boolean)
      await deleteQualityIssueMeetingBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.qualityissuemeeting._self') }))
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
  const res = await getQualityIssueMeetingTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importQualityIssueMeeting(file, sheetName)
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
    const exportMeta = await exportQualityIssueMeeting(
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
    message.success(t('common.feedback.export.success', { target: t('entity.qualityissuemeeting._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.qualityissuemeeting._self') }))
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

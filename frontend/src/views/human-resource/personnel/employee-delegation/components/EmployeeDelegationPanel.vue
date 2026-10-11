<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/human-resource/personnel/employee-delegation/components -->
<!-- 文件名称：EmployeeDelegationPanel.vue -->
<!-- 功能描述：员工实体主表实体右侧明细 employeeDelegation 独立 CRUD（按主表选中 originalEmployeeId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="employee-delegation-panel flex h-full min-h-0 flex-col overflow-hidden">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="human:resource:personnel:employee:delegation:create"
      update-permission="human:resource:personnel:employee:delegation:update"
      delete-permission="human:resource:personnel:employee:delegation:delete"
      import-permission="human:resource:personnel:employee:delegation:import"
      export-permission="human:resource:personnel:employee:delegation:export"
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
      class="employee-delegation-panel__table-wrap min-h-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :virtual="true"
        :row-key="getEmployeeDelegationId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="employeeDelegationId"
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
      <EmployeeDelegationForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterEmployeeId"
        :master-row="selectedMasterRow"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-human-resource-personnel-employee-delegation-employee-delegation"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('cultureCode')">
      <a-form-item :label="pi.queryLabel('cultureCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.cultureCode"
          dict-type="sys_culture_code"
          :placeholder="pi.queryPh('cultureCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="pi.queryLabel('plantCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.plantCode"
          api-url="TaktPlants/options"
          :placeholder="pi.queryPh('plantCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('proxyEmployeeId')">
      <a-form-item :label="pi.queryLabel('proxyEmployeeId')">
        <TaktSelect
          v-model:value="advancedQueryForm.proxyEmployeeId"
          api-url="TaktEmployees/options"
          :placeholder="pi.queryPh('proxyEmployeeId', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('proxyEmployeeCode')">
      <a-form-item :label="pi.queryLabel('proxyEmployeeCode')">
        <a-input
          v-model:value="advancedQueryForm.proxyEmployeeCode"
          :placeholder="pi.queryPh('proxyEmployeeCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('proxyEmployeeName')">
      <a-form-item :label="pi.queryLabel('proxyEmployeeName')">
        <a-input
          v-model:value="advancedQueryForm.proxyEmployeeName"
          :placeholder="pi.queryPh('proxyEmployeeName', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('originalEmployeeCode')">
      <a-form-item :label="pi.queryLabel('originalEmployeeCode')">
        <a-input
          v-model:value="advancedQueryForm.originalEmployeeCode"
          :placeholder="pi.queryPh('originalEmployeeCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('originalEmployeeName')">
      <a-form-item :label="pi.queryLabel('originalEmployeeName')">
        <a-input
          v-model:value="advancedQueryForm.originalEmployeeName"
          :placeholder="pi.queryPh('originalEmployeeName', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('delegationType')">
      <a-form-item :label="pi.queryLabel('delegationType')">
        <TaktSelect
          v-model:value="advancedQueryForm.delegationType"
          dict-type="humanresource_personnel_employee_delegation_type"
          :placeholder="pi.queryPh('delegationType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scopeType')">
      <a-form-item :label="pi.queryLabel('scopeType')">
        <TaktSelect
          v-model:value="advancedQueryForm.scopeType"
          dict-type="humanresource_personnel_employee_delegation_scope"
          :placeholder="pi.queryPh('scopeType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scopeId')">
      <a-form-item :label="pi.queryLabel('scopeId')">
        <a-textarea
          v-model:value="advancedQueryForm.scopeId"
          :placeholder="pi.queryPh('scopeId', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reason')">
      <a-form-item :label="pi.queryLabel('reason')">
        <a-input
          v-model:value="advancedQueryForm.reason"
          :placeholder="pi.queryPh('reason', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('startDateStart')">
      <a-form-item :label="pi.queryLabel('startDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.startDateStart"
          :placeholder="pi.queryPh('startDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('startDateEnd')">
      <a-form-item :label="pi.queryLabel('startDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.startDateEnd"
          :placeholder="pi.queryPh('startDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('endDateStart')">
      <a-form-item :label="pi.queryLabel('endDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.endDateStart"
          :placeholder="pi.queryPh('endDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('endDateEnd')">
      <a-form-item :label="pi.queryLabel('endDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.endDateEnd"
          :placeholder="pi.queryPh('endDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
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
        :entity-i18n-key="EMPLOYEEDELEGATION_SELF_I18N_KEY"
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
      id-column-key="employeeDelegationId"
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
 * 员工实体子表 employeeDelegation 右栏面板
 * @module views/human-resource/personnel/employee-delegation/components
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
import EmployeeDelegationForm from './EmployeeDelegationForm.vue'
import { useEmployeeMasterContext } from '../composables/useEmployeeMasterContext'
import {
  getEmployeeDelegationList,
  getEmployeeDelegationById,
  createEmployeeDelegation,
  updateEmployeeDelegation,
  deleteEmployeeDelegationById,
  deleteEmployeeDelegationBatch,
  getEmployeeDelegationTemplate,
  importEmployeeDelegation,
  exportEmployeeDelegation,
} from '@/api/human-resource/personnel/employeeDelegation.api'
import type { EmployeeDelegation, EmployeeDelegationQuery } from '@/types/human-resource/personnel/employeeDelegation.types'

import {
  useEmployeeDelegationI18n,
  EMPLOYEEDELEGATION_DEFAULT_VISIBLE_COLUMN_KEYS,
  EMPLOYEEDELEGATION_SUMMARY_SUM_FIELDS,
  EMPLOYEEDELEGATION_QUERY_STRING_FIELDS,
  EMPLOYEEDELEGATION_QUERY_FIELDS,
  EMPLOYEEDELEGATION_SELF_I18N_KEY,
} from '../composables/useEmployeeDelegationI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useEmployeeDelegationI18n()

const { t } = useI18n()
const { selectedMasterRow } = useEmployeeMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktEmployeeDelegation')
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
const dataSource = ref<EmployeeDelegation[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<EmployeeDelegation | null>(null)
const selectedRows = ref<EmployeeDelegation[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<EmployeeDelegation>>({})
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
  for (const key of EMPLOYEEDELEGATION_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.delegationType !== undefined && form.delegationType !== null) {
    return true
  }
  if (form.scopeType !== undefined && form.scopeType !== null) {
    return true
  }
  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(EMPLOYEEDELEGATION_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof EMPLOYEEDELEGATION_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    delegationType: undefined as number | undefined,
    scopeType: undefined as number | undefined,  }
}
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() =>
  EMPLOYEEDELEGATION_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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
const visibleColumnKeys = ref<string[]>([...EMPLOYEEDELEGATION_DEFAULT_VISIBLE_COLUMN_KEYS])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = [...EMPLOYEEDELEGATION_DEFAULT_VISIBLE_COLUMN_KEYS]
}
const importVisible = ref(false)

const entityIdName = 'employeeDelegationId'
const masterEmployeeId = computed((): string => {
  const id = (selectedMasterRow.value as Record<string, unknown> | null)?.['employeeId']
  return id != null ? String(id) : ''
})
const hasMasterSelection = computed(() => masterEmployeeId.value !== '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getEmployeeDelegationId(record: EmployeeDelegation | Record<string, unknown>): string {
  return String((record as EmployeeDelegation)?.[entityIdName] ?? '')
}

function getEmployeeDelegationField(record: EmployeeDelegation | Record<string, unknown>, field: string): unknown {
  return (record as EmployeeDelegation)?.[field as keyof EmployeeDelegation]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'employeeDelegationId',
    key: 'employeeDelegationId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: EmployeeDelegation }) =>
      String(getEmployeeDelegationField(record, 'employeeDelegationId') ?? ''),
  },
  {
    title: pi.label('proxyEmployeeId'),
    dataIndex: 'proxyEmployeeId',
    key: 'proxyEmployeeId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EmployeeDelegation }) =>
      String(getEmployeeDelegationField(record, 'proxyEmployeeId') ?? ''),
  },
  {
    title: pi.label('proxyEmployeeCode'),
    dataIndex: 'proxyEmployeeCode',
    key: 'proxyEmployeeCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EmployeeDelegation }) =>
      String(getEmployeeDelegationField(record, 'proxyEmployeeCode') ?? ''),
  },
  {
    title: pi.label('proxyEmployeeName'),
    dataIndex: 'proxyEmployeeName',
    key: 'proxyEmployeeName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EmployeeDelegation }) =>
      String(getEmployeeDelegationField(record, 'proxyEmployeeName') ?? ''),
  },
  {
    title: pi.label('originalEmployeeId'),
    dataIndex: 'originalEmployeeId',
    key: 'originalEmployeeId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EmployeeDelegation }) =>
      String(getEmployeeDelegationField(record, 'originalEmployeeId') ?? ''),
  },
  {
    title: pi.label('originalEmployeeCode'),
    dataIndex: 'originalEmployeeCode',
    key: 'originalEmployeeCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EmployeeDelegation }) =>
      String(getEmployeeDelegationField(record, 'originalEmployeeCode') ?? ''),
  },
  {
    title: pi.label('originalEmployeeName'),
    dataIndex: 'originalEmployeeName',
    key: 'originalEmployeeName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EmployeeDelegation }) =>
      String(getEmployeeDelegationField(record, 'originalEmployeeName') ?? ''),
  },
  {
    title: pi.label('delegationType'),
    dataIndex: 'delegationType',
    key: 'delegationType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EmployeeDelegation }) =>
      String(getEmployeeDelegationField(record, 'delegationType') ?? ''),
  },
  {
    title: pi.label('scopeType'),
    dataIndex: 'scopeType',
    key: 'scopeType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EmployeeDelegation }) =>
      String(getEmployeeDelegationField(record, 'scopeType') ?? ''),
  },
  {
    title: pi.label('scopeId'),
    dataIndex: 'scopeId',
    key: 'scopeId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EmployeeDelegation }) =>
      String(getEmployeeDelegationField(record, 'scopeId') ?? ''),
  },
  {
    title: pi.label('reason'),
    dataIndex: 'reason',
    key: 'reason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EmployeeDelegation }) =>
      String(getEmployeeDelegationField(record, 'reason') ?? ''),
  },
  {
    title: pi.label('startDate'),
    dataIndex: 'startDate',
    key: 'startDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EmployeeDelegation }) =>
      String(getEmployeeDelegationField(record, 'startDate') ?? ''),
  },
  {
    title: pi.label('endDate'),
    dataIndex: 'endDate',
    key: 'endDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EmployeeDelegation }) =>
      String(getEmployeeDelegationField(record, 'endDate') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'human:resource:personnel:employee:delegation:update',
        onClick: (record: EmployeeDelegation) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'human:resource:personnel:employee:delegation:delete',
        onClick: (record: EmployeeDelegation) => void handleDeleteOne(record),
      },
    ],
  }),
])

/** 与 TaktSingleTable 展示列对齐（用于汇总行单元格） */
const resolvedSummaryColumns = computed(() => {
  const userCols = normalizeUserTableColumns(columns.value)
  const merged = mergeDefaultColumns(userCols, t, true, 'company')
  const keys = visibleColumnKeys.value
  if (keys.length > 0) {
    return filterTableColumnsByVisibleKeys(merged, keys, merged)
  }
  return filterMergedColumnsByDefaultVisible(merged, userCols, {
    idColumnKey: 'employeeDelegationId',
    actionColumnKey: 'action',
    tableMode: 'masterDetailDetail',
    entityScope: 'company',
  })
})

const summarySumFieldSet = new Set<string>(EMPLOYEEDELEGATION_SUMMARY_SUM_FIELDS)

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
    EMPLOYEEDELEGATION_SUMMARY_SUM_FIELDS.map((field) => [field, 0]),
  ) as Record<(typeof EMPLOYEEDELEGATION_SUMMARY_SUM_FIELDS)[number], number>
  for (const row of dataSource.value) {
    for (const field of EMPLOYEEDELEGATION_SUMMARY_SUM_FIELDS) {
      const num = Number(getEmployeeDelegationField(row, field))
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
  onChange: (keys: (string | number)[], rows: EmployeeDelegation[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: EmployeeDelegation, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getEmployeeDelegationId(selectedRow.value) === getEmployeeDelegationId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: EmployeeDelegation[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: EmployeeDelegation) {
  const key = getEmployeeDelegationId(record)
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
 * @returns {EmployeeDelegationQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<EmployeeDelegationQuery>): EmployeeDelegationQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: EmployeeDelegationQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    originalEmployeeId: masterEmployeeId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof EmployeeDelegationQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of EMPLOYEEDELEGATION_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.delegationType !== undefined && form.delegationType !== null) {
    query.delegationType = form.delegationType
  }
  if (form.scopeType !== undefined && form.scopeType !== null) {
    query.scopeType = form.scopeType
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
    const res = await getEmployeeDelegationList(buildListQuery())
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
watch(masterEmployeeId, () => {
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

async function handleEdit(record: EmployeeDelegation) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getEmployeeDelegationById(getEmployeeDelegationId(record))
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
    const id = formData.value?.employeeDelegationId
    if (id) {
      await updateEmployeeDelegation(id, payload)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createEmployeeDelegation(payload)
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

async function handleDeleteOne(record: EmployeeDelegation) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: pi.self(),
      name: t('common.tip.this.target', { target: pi.self() }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteEmployeeDelegationById(getEmployeeDelegationId(record))
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
      const ids = selectedRows.value.map((r) => getEmployeeDelegationId(r)).filter(Boolean)
      await deleteEmployeeDelegationBatch(ids)
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
  const res = await getEmployeeDelegationTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importEmployeeDelegation(file, sheetName)
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
    const exportMeta = await exportEmployeeDelegation(
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

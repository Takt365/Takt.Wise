<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/code/database/table-archive -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：数据表归档标准 CRUD；工具栏/行内按年归档（预览/立即/后台）与预建年表 -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4">
    <a-typography-title :level="4" class="!mb-1">
      {{ t('code.database.table-archive.page.title') }}
    </a-typography-title>
    <a-typography-text type="secondary" class="mb-4 block">
      {{ t('code.database.table-archive.page.subtitle') }}
    </a-typography-text>

    <!-- 查询栏 -->
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    />

    <!-- 工具栏 -->
    <TaktToolsBar
      create-permission="code:database:table:archive:create"
      update-permission="code:database:table:archive:update"
      delete-permission="code:database:table:archive:delete"
      import-permission="code:database:table:archive:import"
      export-permission="code:database:table:archive:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="true"
      :show-export="true"
      :show-expand="false"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      :create-loading="loading"
      :update-disabled="updateDisabled"
      :update-loading="loading"
      :delete-disabled="deleteDisabled"
      :delete-loading="loading"
      :refresh-loading="loading"
      @create="handleCreate"
      @update="handleUpdate"
      @delete="handleDelete"
      @import="handleImport"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
    >
      <template #left>
        <a-space>
          <a-button
            v-permission="'code:database:table:archive:create'"
            class="takt-button-create"
            :loading="loading"
            @click="handleCreate"
          >
            <template #icon>
              <RiAddLine class="takt-remix-icon" />
            </template>
            {{ t('common.page.button.create') }}
          </a-button>
          <a-button
            v-permission="'code:database:table:archive:update'"
            class="takt-button-update"
            :disabled="updateDisabled"
            :loading="loading"
            @click="handleUpdate"
          >
            <template #icon>
              <RiEditLine class="takt-remix-icon" />
            </template>
            {{ t('common.page.button.update') }}
          </a-button>
          <a-button
            v-permission="'code:database:table:archive:delete'"
            class="takt-button-delete"
            :disabled="deleteDisabled"
            :loading="loading"
            @click="handleDelete"
          >
            <template #icon>
              <RiDeleteBinLine class="takt-remix-icon" />
            </template>
            {{ t('common.page.button.delete') }}
          </a-button>
          <a-button
            v-permission="'code:database:table:archive:import'"
            class="takt-button-import"
            :loading="loading"
            @click="handleImport"
          >
            <template #icon>
              <RiImportLine class="takt-remix-icon" />
            </template>
            {{ t('common.page.button.import') }}
          </a-button>
          <a-button
            v-permission="'code:database:table:archive:export'"
            class="takt-button-export"
            :loading="loading"
            @click="handleExport"
          >
            <template #icon>
              <RiExportLine class="takt-remix-icon" />
            </template>
            {{ t('common.page.button.export') }}
          </a-button>
          <a-button
            v-permission="'code:database:table:archive:archive'"
            class="takt-button-query"
            :disabled="archiveToolbarDisabled"
            @click="openArchiveModal()"
          >
            <template #icon>
              <RiInboxArchiveLine class="takt-remix-icon" />
            </template>
            {{ t('code.database.table-archive.page.archive.title') }}
          </a-button>
          <a-button
            v-permission="'code:database:table:archive:create'"
            class="takt-button-query"
            :disabled="ensureYearsToolbarDisabled"
            :loading="ensureYearsLoading"
            @click="openEnsureYearsModal()"
          >
            <template #icon>
              <RiTableLine class="takt-remix-icon" />
            </template>
            {{ t('code.database.table-archive.page.ensureyears.title') }}
          </a-button>
        </a-space>
      </template>
    </TaktToolsBar>

    <!-- 表格 -->
    <TaktSingleTable
      entity-scope="company"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="entityIdName"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getTableArchiveId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"
      :pagination="false"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'archiveKeyKind'">
          <TaktDictTag
            :value="record.archiveKeyKind"
            dict-type="sys_archive_key_kind"
          />
        </template>
        <template v-else-if="column.key === 'archiveStatus'">
          <a-switch
            :checked="Number(record.archiveStatus) === 1"
            :checked-children="t('common.page.button.enable')"
            :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleArchiveStatusChange(record, Boolean(checked))"
          />
        </template>
      </template>
    </TaktSingleTable>

    <!-- 分页 -->
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />

    <!-- 新增/编辑 -->
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      :width="formModalWidthPx"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <TableArchiveForm
        v-if="formVisible"
        :key="formData?.tableArchiveId ?? 'create'"
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>

    <!-- 高级查询 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-code-database-table-archive'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
        <div v-show="isFieldVisible('targetTenantCode')">
          <a-form-item :label="pi.queryLabel('targetTenantCode')">
            <a-input
              v-model:value="advancedQueryForm.targetTenantCode"
              :placeholder="pi.queryPh('targetTenantCode', 'required')"
              show-count
              :maxlength="3"
              allow-clear
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('targetDatabaseName')">
          <a-form-item :label="pi.queryLabel('targetDatabaseName')">
            <a-input
              v-model:value="advancedQueryForm.targetDatabaseName"
              :placeholder="pi.queryPh('targetDatabaseName', 'required')"
              show-count
              :maxlength="40"
              allow-clear
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('tableName')">
          <a-form-item :label="pi.queryLabel('tableName')">
            <a-input
              v-model:value="advancedQueryForm.tableName"
              :placeholder="pi.queryPh('tableName', 'required')"
              allow-clear
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('archiveKeyColumn')">
          <a-form-item :label="pi.queryLabel('archiveKeyColumn')">
            <a-input
              v-model:value="advancedQueryForm.archiveKeyColumn"
              :placeholder="pi.queryPh('archiveKeyColumn', 'required')"
              allow-clear
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('archiveKeyKind')">
          <a-form-item :label="pi.queryLabel('archiveKeyKind')">
            <TaktSelect
              v-model:value="advancedQueryForm.archiveKeyKind"
              dict-type="sys_archive_key_kind"
              :placeholder="pi.queryPh('archiveKeyKind', 'select')"
              allow-clear
              class="w-full"
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('retainHotYears')">
          <a-form-item :label="pi.queryLabel('retainHotYears')">
            <a-input-number
              v-model:value="advancedQueryForm.retainHotYears"
              :min="0"
              class="w-full"
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('archiveName')">
          <a-form-item :label="pi.queryLabel('archiveName')">
            <a-input
              v-model:value="advancedQueryForm.archiveName"
              :placeholder="pi.queryPh('archiveName', 'optional')"
              allow-clear
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('archiveStatus')">
          <a-form-item :label="pi.queryLabel('archiveStatus')">
            <TaktSelect
              v-model:value="advancedQueryForm.archiveStatus"
              dict-type="sys_normal_disable"
              :placeholder="pi.queryPh('archiveStatus', 'select')"
              class="w-full"
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('remark')">
          <a-form-item :label="pi.queryLabel('remark')">
            <a-textarea
              v-model:value="advancedQueryForm.remark"
              :placeholder="pi.queryPh('remark', 'optional')"
              :rows="3"
              show-count
              :maxlength="400"
              allow-clear
            />
          </a-form-item>
        </div>
      </template>
    </TaktQueryDrawer>

    <!-- 列设置 -->
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="entityIdName"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />

    <!-- 导入 -->
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
        :entity-i18n-key="TABLE_ARCHIVE_SELF_I18N_KEY"
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

    <!-- 按年归档 -->
    <TaktModal
      v-model:open="archiveModalVisible"
      :title="t('code.database.table-archive.page.archive.title')"
      width="60%"
      :confirm-loading="false"
      :footer="null"
      @cancel="handleArchiveModalCancel"
    >
      <a-form layout="vertical">
        <a-form-item :label="t('code.database.table-archive.page.archive.year')" required>
          <a-input-number
            v-model:value="archiveYear"
            :min="1970"
            :max="9999"
            :precision="0"
            class="w-full"
          />
        </a-form-item>
        <a-form-item :label="t('code.database.table-archive.page.archive.selectpolicies')">
          <a-typography-text type="secondary">
            {{ selectedEnabledPolicyIds.length }} / {{ selectedRows.length }}
          </a-typography-text>
        </a-form-item>
        <div class="mb-4 flex flex-wrap gap-2">
          <a-button
            v-permission="'code:database:table:archive:archive'"
            :loading="previewLoading"
            :disabled="archiveActionDisabled"
            @click="handleArchivePreview"
          >
            {{ t('code.database.table-archive.page.archive.preview') }}
          </a-button>
          <a-button
            v-permission="'code:database:table:archive:archive'"
            type="primary"
            :loading="runNowLoading"
            :disabled="archiveActionDisabled"
            @click="handleArchiveRunNow"
          >
            {{ t('code.database.table-archive.page.archive.runnow') }}
          </a-button>
          <a-button
            v-permission="'code:database:table:archive:schedule'"
            :disabled="archiveActionDisabled"
            @click="openScheduleModalFromArchive"
          >
            {{ t('code.database.table-archive.page.archive.schedule') }}
          </a-button>
        </div>
        <a-table
          v-if="previewItems.length > 0"
          :columns="previewColumns"
          :data-source="previewItems"
          :pagination="false"
          row-key="policyId"
          size="small"
          bordered
        />
        <a-typography-text v-if="previewTotal > 0" type="secondary" class="mt-2 block">
          {{ t('code.database.table-archive.page.archive.previewtotal', { count: previewTotal }) }}
        </a-typography-text>
      </a-form>
    </TaktModal>

    <!-- 后台归档 -->
    <TaktModal
      v-model:open="scheduleModalVisible"
      :title="t('code.database.table-archive.page.archive.schedule')"
      :confirm-loading="scheduleSubmitLoading"
      @ok="handleScheduleSubmit"
      @cancel="handleScheduleCancel"
    >
      <a-form layout="vertical">
        <a-form-item :label="t('code.database.table-archive.page.archive.scheduledat')" required>
          <a-date-picker
            v-model:value="scheduleAtValue"
            show-time
            class="w-full"
            format="YYYY-MM-DD HH:mm:ss"
            value-format="YYYY-MM-DD HH:mm:ss"
          />
        </a-form-item>
      </a-form>
    </TaktModal>

    <!-- 预建年表 -->
    <TaktModal
      v-model:open="ensureYearsModalVisible"
      :title="t('code.database.table-archive.page.ensureyears.title')"
      :confirm-loading="ensureYearsLoading"
      @ok="handleEnsureYearsSubmit"
      @cancel="handleEnsureYearsCancel"
    >
      <a-form layout="vertical">
        <a-form-item :label="t('code.database.table-archive.page.ensureyears.yearstart')" required>
          <a-input-number
            v-model:value="ensureYearStart"
            :min="1970"
            :max="9999"
            :precision="0"
            class="w-full"
          />
        </a-form-item>
        <a-form-item :label="t('code.database.table-archive.page.ensureyears.yearend')" required>
          <a-input-number
            v-model:value="ensureYearEnd"
            :min="1970"
            :max="9999"
            :precision="0"
            class="w-full"
          />
        </a-form-item>
        <a-typography-text type="secondary">
          {{ ensureYearsHint }}
        </a-typography-text>
        <a-typography-text v-if="ensureYearsResult" type="success" class="mt-2 block">
          {{ t('code.database.table-archive.page.ensureyears.result', { tables: ensureYearsResult }) }}
        </a-typography-text>
      </a-form>
    </TaktModal>
  </div>
</template>

<script setup lang="ts">
/**
 * 数据表归档标准 CRUD 列表页
 * @module views/code/database/table-archive
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import {
  RiAddLine,
  RiInboxArchiveLine,
  RiDeleteBinLine,
  RiEditLine,
  RiExportLine,
  RiImportLine,
  RiPlayLine,
  RiTableLine,
  RiTimerLine,
} from '@remixicon/vue'
import {
  createTableArchive,
  deleteTableArchiveById,
  deleteTableArchiveBatch,
  ensureYearTables,
  exportTableArchive,
  getTableArchiveById,
  getTableArchiveList,
  getTableArchiveTemplate,
  importTableArchive,
  previewTableArchive,
  runTableArchiveNow,
  scheduleTableArchive,
  updateTableArchive,
  updateTableArchiveStatus,
} from '@/api/code/database/tableArchive.api'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import type {
  TableArchive,
  TableArchivePreviewItem,
  TableArchiveQuery,
} from '@/types/code/database/tableArchive.types'
import TableArchiveForm from './components/TableArchiveForm.vue'
import {
  useTableArchiveI18n,
  TABLE_ARCHIVE_LIST_FIELDS,
  TABLE_ARCHIVE_QUERY_FIELDS,
  TABLE_ARCHIVE_QUERY_STRING_FIELDS,
  TABLE_ARCHIVE_SELF_I18N_KEY,
} from './composables/useTableArchiveI18n'

const pi = useTableArchiveI18n()
type TableArchiveRowRecord = TableArchive | Record<string, unknown>
const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktTableArchive')
const entityIdName = 'tableArchiveId'
const MAX_ENSURE_YEAR_SPAN = 30

const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() }),
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<TableArchive[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const selectedRow = ref<TableArchiveRowRecord | null>(null)
const selectedRows = ref<TableArchiveRowRecord[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<TableArchive> | null>(null)
const formLoading = ref(false)
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()
const advancedQueryVisible = ref(false)
const columnSettingVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const visibleQueryFieldKeys = ref<string[]>([])
const importVisible = ref(false)
const archiveModalVisible = ref(false)
const archiveYear = ref(new Date().getFullYear() - 1)
const previewLoading = ref(false)
const previewItems = ref<TableArchivePreviewItem[]>([])
const previewTotal = ref(0)
const runNowLoading = ref(false)
const scheduleModalVisible = ref(false)
const scheduleAtValue = ref<string | null>(null)
const scheduleSubmitLoading = ref(false)
const schedulePolicyIds = ref<string[]>([])
const ensureYearsModalVisible = ref(false)
const ensureYearStart = ref(new Date().getFullYear())
const ensureYearEnd = ref(new Date().getFullYear() + 1)
const ensureYearsLoading = ref(false)
const ensureYearsResult = ref('')
const runLoadingId = ref<string | null>(null)

function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(
    TABLE_ARCHIVE_QUERY_STRING_FIELDS.map((key) => [key, '']),
  ) as Record<(typeof TABLE_ARCHIVE_QUERY_STRING_FIELDS)[number], string>
  return {
    ...form,
    archiveKeyKind: undefined as number | undefined,
    retainHotYears: undefined as number | undefined,
    sortOrder: undefined as number | undefined,
    archiveStatus: undefined as number | undefined,
  }
}

const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
const queryFieldsMeta = computed(() =>
  TABLE_ARCHIVE_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

const selectedEnabledPolicyIds = computed(() =>
  selectedRows.value
    .filter((row) => Number((row as TableArchive).archiveStatus) === 1)
    .map((row) => getTableArchiveId(row))
    .filter(Boolean),
)

const archiveToolbarDisabled = computed(() => selectedEnabledPolicyIds.value.length === 0)
const ensureYearsToolbarDisabled = computed(() => selectedRows.value.length !== 1)
const archiveActionDisabled = computed(() =>
  selectedEnabledPolicyIds.value.length === 0 || !archiveYear.value,
)

const ensureYearsHint = computed(() => {
  const tableName = selectedRows.value.length === 1
    ? String((selectedRows.value[0] as TableArchive).tableName || '')
    : '{table}'
  return t('code.database.table-archive.page.ensureyears.yearshint').replace('{表名}', tableName)
})

const previewColumns = computed<TableColumnsType>(() => [
  { title: pi.label('archiveName'), dataIndex: 'archiveName', key: 'archiveName', ellipsis: true },
  { title: pi.label('tableName'), dataIndex: 'tableName', key: 'tableName', ellipsis: true },
  { title: pi.label('tableName'), dataIndex: 'archiveTableName', key: 'archiveTableName', ellipsis: true },
  { title: t('code.database.table-archive.page.archive.year'), dataIndex: 'archiveYear', key: 'archiveYear', width: 90 },
  { title: t('code.database.table-archive.page.archive.preview'), dataIndex: 'sourceRowCount', key: 'sourceRowCount', width: 120 }])

function buildListQuery(overrides?: Partial<TableArchiveQuery>): TableArchiveQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: TableArchiveQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  for (const key of TABLE_ARCHIVE_QUERY_STRING_FIELDS) {
    const v = (form[key] ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  if (form.archiveKeyKind !== undefined && form.archiveKeyKind !== null) {
    query.archiveKeyKind = form.archiveKeyKind
  }
  if (form.retainHotYears !== undefined && form.retainHotYears !== null) {
    query.retainHotYears = form.retainHotYears
  }
  if (form.sortOrder !== undefined && form.sortOrder !== null) {
    query.sortOrder = form.sortOrder
  }
  if (form.archiveStatus !== undefined && form.archiveStatus !== null) {
    query.archiveStatus = form.archiveStatus
  }
  return query
}

function buildListColumn(
  key: string,
  title: string,
  options?: { width?: number; fixed?: 'left' },
) {
  return {
    title,
    dataIndex: key,
    key,
    width: options?.width ?? 120,
    resizable: true,
    ellipsis: true,
    ...(options?.fixed ? { fixed: options.fixed } : {}),
  }
}

const columns = computed<TableColumnsType>(() => [
  buildListColumn(entityIdName, t('common.page.entity.id'), { width: 80, fixed: 'left' }),
  ...TABLE_ARCHIVE_LIST_FIELDS.map((key) => buildListColumn(key, pi.label(key))),
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'code:database:table:archive:update',
        onClick: (record: TableArchiveRowRecord) => handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'code:database:table:archive:delete',
        onClick: (record: TableArchiveRowRecord) => handleDeleteOne(record),
      },
      {
        key: 'archive-run',
        label: t('code.database.table-archive.page.archive.runnow'),
        shape: 'plain',
        icon: RiPlayLine,
        permission: 'code:database:table:archive:archive',
        loadingFn: (record: TableArchiveRowRecord) =>
          runLoadingId.value === getTableArchiveId(record),
        onClick: (record: TableArchiveRowRecord) => handleRowRunNow(record),
      },
      {
        key: 'archive-schedule',
        label: t('code.database.table-archive.page.archive.schedule'),
        shape: 'plain',
        icon: RiTimerLine,
        permission: 'code:database:table:archive:schedule',
        onClick: (record: TableArchiveRowRecord) => openArchiveModal(record),
      },
      {
        key: 'ensure-years',
        label: t('code.database.table-archive.page.ensureyears.title'),
        shape: 'plain',
        icon: RiTableLine,
        permission: 'code:database:table:archive:create',
        onClick: (record: TableArchiveRowRecord) => openEnsureYearsModal(record),
      }],
  })])

function getTableArchiveId(record: TableArchiveRowRecord): string {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}

function buildYearRange(start: number, end: number): number[] {
  const from = Math.min(start, end)
  const to = Math.max(start, end)
  const years: number[] = []
  for (let year = from; year <= to; year += 1) {
    years.push(year)
  }
  return years
}

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: TableArchiveRowRecord[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: TableArchiveRowRecord, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (
      selectedRow.value
      && getTableArchiveId(selectedRow.value) === getTableArchiveId(record)
    ) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: TableArchiveRowRecord[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

const onClickRow = (record: TableArchiveRowRecord) => ({
  onClick: () => {
    const key = getTableArchiveId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) =>
      selectedRowKeys.value.includes(getTableArchiveId(item)),
    )
    selectedRow.value = selectedRowKeys.value.length === 1 ? (selectedRows.value[0] ?? null) : null
    if (rowSelection.value.onChange) {
      rowSelection.value.onChange(selectedRowKeys.value, selectedRows.value)
    }
  },
})

async function loadData() {
  loading.value = true
  try {
    const res = await getTableArchiveList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: unknown) {
    logger.error('[TableArchive] load failed', { error })
    message.error(t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

useTableRefresh(loadData)

function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}

async function handleEdit(record: TableArchiveRowRecord) {
  const id = getTableArchiveId(record)
  if (!id) {
    return
  }
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getTableArchiveById(id)
    formData.value = detail ?? ({ ...record } as Partial<TableArchive>)
    formVisible.value = true
  } catch {
    message.error(t('common.feedback.load.data.failed'))
  } finally {
    formLoading.value = false
  }
}

function handleUpdate() {
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
  } else {
    message.warning(
      t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: pi.self() }),
    )
  }
}

async function handleFormSubmit() {
  const refInst = formRef.value
  if (!refInst?.validate) {
    return
  }
  try {
    await refInst.validate()
  } catch {
    return
  }
  formLoading.value = true
  try {
    const payload = refInst.getValues?.()
    const id = formData.value?.tableArchiveId
    if (id) {
      await updateTableArchive(id, payload)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createTableArchive(payload)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    formData.value = null
    nextTick(() => formRef.value?.resetFields())
    loadData()
  } finally {
    formLoading.value = false
  }
}

function handleFormCancel() {
  formVisible.value = false
  formData.value = null
  nextTick(() => formRef.value?.resetFields())
}

async function handleExport() {
  try {
    loading.value = true
    const exportMeta = await exportTableArchive(
      buildListQuery({ pageIndex: 1, pageSize: 100000 }),
      excelNames.sheet,
      excelNames.fileBase,
    )
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = `${excelNames.fileBase}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as { contentDisposition?: string | null }).contentDisposition ?? null,
      contentType: (exportMeta as { contentType?: string | null }).contentType ?? null,
      fallbackBase,
    })
    const blob = (exportMeta as { blob?: Blob }).blob ?? exportMeta
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
    logger.error('[TableArchive] export failed', { error })
    message.error(t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}

function handleImport() {
  importVisible.value = true
}

function handleImportCancel() {
  importVisible.value = false
}

async function handleDownloadTemplate() {
  return getTableArchiveTemplate(excelNames.sheet, excelNames.fileBase)
}

async function handleImportFile(file: globalThis.File) {
  return importTableArchive(file, excelNames.sheet)
}

function handleImportSuccess() {
  importVisible.value = false
  loadData()
}

async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(
      t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: pi.self() }),
    )
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
      const ids = selectedRows.value.map((row) => getTableArchiveId(row)).filter(Boolean)
      await deleteTableArchiveBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      loadData()
    },
  })
}

function handleDeleteOne(record: TableArchiveRowRecord) {
  const id = getTableArchiveId(record)
  if (!id) {
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: pi.self(),
      name: t('common.tip.this.target', { target: pi.self() }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteTableArchiveById(id)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      loadData()
    },
  })
}

async function handleArchiveStatusChange(record: TableArchiveRowRecord, checked: boolean) {
  const id = getTableArchiveId(record)
  if (!id) {
    return
  }
  const archiveStatus = checked ? 1 : 0
  try {
    await updateTableArchiveStatus({ tableArchiveId: id, archiveStatus })
    ;(record as TableArchive).archiveStatus = archiveStatus
    message.success(t('common.feedback.updated', { target: pi.self() }))
  } catch (error: unknown) {
    logger.error('[TableArchive] status update failed', { error, id })
  }
}

function resolveArchivePolicyIds(record?: TableArchiveRowRecord): string[] {
  if (record) {
    const id = getTableArchiveId(record)
    return id && Number((record as TableArchive).archiveStatus) === 1 ? [id] : []
  }
  return [...selectedEnabledPolicyIds.value]
}

function openArchiveModal(record?: TableArchiveRowRecord) {
  if (record) {
    const id = getTableArchiveId(record)
    if (!id) {
      return
    }
    selectedRowKeys.value = [id]
    selectedRows.value = [record]
    selectedRow.value = record
  }
  const policyIds = resolveArchivePolicyIds(record)
  if (policyIds.length === 0) {
    message.warning(t('code.database.table-archive.page.archive.emptyselection'))
    return
  }
  previewItems.value = []
  previewTotal.value = 0
  archiveModalVisible.value = true
}

function handleArchiveModalCancel() {
  archiveModalVisible.value = false
  previewItems.value = []
  previewTotal.value = 0
}

async function handleArchivePreview() {
  const policyIds = selectedEnabledPolicyIds.value
  if (policyIds.length === 0 || !archiveYear.value) {
    message.warning(t('code.database.table-archive.page.archive.emptyselection'))
    return
  }
  previewLoading.value = true
  try {
    const result = await previewTableArchive({
      policyIds,
      archiveYear: archiveYear.value,
    })
    previewItems.value = result.items ?? []
    previewTotal.value = result.totalRowCount ?? 0
  } catch (error: unknown) {
    logger.error('[TableArchive] preview failed', { error })
    message.error(t('code.database.table-archive.page.archive.failed'))
  } finally {
    previewLoading.value = false
  }
}

async function handleArchiveRunNow() {
  const policyIds = selectedEnabledPolicyIds.value
  if (policyIds.length === 0 || !archiveYear.value) {
    message.warning(t('code.database.table-archive.page.archive.emptyselection'))
    return
  }
  runNowLoading.value = true
  try {
    await runTableArchiveNow({ policyIds, archiveYear: archiveYear.value })
    message.success(t('code.database.table-archive.page.archive.runsuccess'))
    archiveModalVisible.value = false
  } catch (error: unknown) {
    logger.error('[TableArchive] run-now failed', { error })
  } finally {
    runNowLoading.value = false
  }
}

async function handleRowRunNow(record: TableArchiveRowRecord) {
  const policyIds = resolveArchivePolicyIds(record)
  if (policyIds.length === 0) {
    message.warning(t('code.database.table-archive.page.archive.emptyselection'))
    return
  }
  const id = policyIds[0]
  runLoadingId.value = id
  try {
    await runTableArchiveNow({
      policyIds,
      archiveYear: archiveYear.value || new Date().getFullYear() - 1,
    })
    message.success(t('code.database.table-archive.page.archive.runsuccess'))
  } catch (error: unknown) {
    logger.error('[TableArchive] row run-now failed', { error, id })
  } finally {
    runLoadingId.value = null
  }
}

function openScheduleModalFromArchive() {
  const policyIds = selectedEnabledPolicyIds.value
  if (policyIds.length === 0) {
    message.warning(t('code.database.table-archive.page.archive.emptyselection'))
    return
  }
  schedulePolicyIds.value = policyIds
  scheduleAtValue.value = null
  scheduleModalVisible.value = true
}

function handleScheduleCancel() {
  scheduleModalVisible.value = false
  schedulePolicyIds.value = []
  scheduleAtValue.value = null
}

function validateScheduleAt(): boolean {
  if (!scheduleAtValue.value) {
    message.warning(t('code.database.table-archive.page.archive.schedulerequired'))
    return false
  }
  const when = new Date(scheduleAtValue.value.replace(/-/g, '/'))
  if (Number.isNaN(when.getTime()) || when.getTime() <= Date.now()) {
    message.warning(t('code.database.table-archive.page.archive.schedulefuture'))
    return false
  }
  return true
}

async function handleScheduleSubmit() {
  if (!validateScheduleAt() || schedulePolicyIds.value.length === 0 || !archiveYear.value) {
    return
  }
  scheduleSubmitLoading.value = true
  try {
    await scheduleTableArchive({
      policyIds: schedulePolicyIds.value,
      archiveYear: archiveYear.value,
      scheduledAt: scheduleAtValue.value,
    })
    message.success(t('code.database.table-archive.page.archive.schedulesuccess'))
    scheduleModalVisible.value = false
    archiveModalVisible.value = false
    schedulePolicyIds.value = []
    scheduleAtValue.value = null
  } catch (error: unknown) {
    logger.error('[TableArchive] schedule failed', { error })
  } finally {
    scheduleSubmitLoading.value = false
  }
}

function openEnsureYearsModal(record?: TableArchiveRowRecord) {
  if (record) {
    const id = getTableArchiveId(record)
    if (!id) {
      return
    }
    selectedRowKeys.value = [id]
    selectedRows.value = [record]
    selectedRow.value = record
  }
  if (selectedRows.value.length !== 1) {
    message.warning(
      t('common.tip.select.to.action', {
        action: t('code.database.table-archive.page.ensureyears.title'),
        entity: pi.self(),
      }),
    )
    return
  }
  ensureYearsResult.value = ''
  const currentYear = new Date().getFullYear()
  ensureYearStart.value = currentYear
  ensureYearEnd.value = currentYear + 1
  ensureYearsModalVisible.value = true
}

function handleEnsureYearsCancel() {
  ensureYearsModalVisible.value = false
  ensureYearsResult.value = ''
}

async function handleEnsureYearsSubmit() {
  if (selectedRows.value.length !== 1) {
    return
  }
  const policyId = getTableArchiveId(selectedRows.value[0])
  const start = Number(ensureYearStart.value)
  const end = Number(ensureYearEnd.value)
  if (!policyId || Number.isNaN(start) || Number.isNaN(end)) {
    message.warning(t('code.database.table-archive.page.ensureyears.emptyyears'))
    return
  }
  const years = buildYearRange(start, end)
  if (years.length === 0) {
    message.warning(t('code.database.table-archive.page.ensureyears.emptyyears'))
    return
  }
  if (years.length > MAX_ENSURE_YEAR_SPAN) {
    message.warning(t('code.database.table-archive.page.ensureyears.spantoolarge'))
    return
  }
  ensureYearsLoading.value = true
  try {
    const result = await ensureYearTables({ policyId, years })
    ensureYearsResult.value = (result.yearTableNames ?? []).join(', ')
    message.success(t('code.database.table-archive.page.ensureyears.success'))
  } catch (error: unknown) {
    logger.error('[TableArchive] ensure year tables failed', { error, policyId })
    message.error(t('code.database.table-archive.page.ensureyears.failed'))
  } finally {
    ensureYearsLoading.value = false
  }
}

function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
}

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}

function handleRefresh() {
  loadData()
}

function handleTableChange() {}

function handleResizeColumn() {}

function handlePaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  loadData()
}

function handlePaginationSizeChange(_current: number, size: number) {
  currentPage.value = getTaktDefaultPageIndex()
  pageSize.value = size
  loadData()
}

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  loadData()
})
</script>
